import win32com.client
import os
import shutil
import xml.etree.ElementTree as ET

def sync_all():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()
    scratch_dir = r"c:\Users\Danie\Desktop\GIT\TD\scratch"
    artifact_dir = r"C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e"

    try:
        print("=== 1. CLEANING TABLE 430 (Pacientes_DNI101) & REMOVING TUTORES ===")
        el_paciente_tab = ea.GetElementByID(430)

        # 1.1 Remove IdTutor_DNI101 from Pacientes_DNI101 table
        for i in range(el_paciente_tab.Attributes.Count - 1, -1, -1):
            attr = el_paciente_tab.Attributes.GetAt(i)
            if "tutor" in attr.Name.lower():
                print(f"Deleting attribute {attr.Name} from Pacientes_DNI101")
                el_paciente_tab.Attributes.Delete(i)
        el_paciente_tab.Attributes.Refresh()

        # Ensure Telefono and Email exist in Pacientes_DNI101
        attr_names = [a.Name for a in el_paciente_tab.Attributes]
        if "Telefono_DNI101" not in attr_names:
            a = el_paciente_tab.Attributes.AddNew("Telefono_DNI101", "VARCHAR(50)")
            a.Stereotype = "NULL"
            a.Update()
        if "Email_DNI101" not in attr_names:
            a = el_paciente_tab.Attributes.AddNew("Email_DNI101", "VARCHAR(100)")
            a.Stereotype = "NULL"
            a.Update()
        el_paciente_tab.Attributes.Refresh()
        el_paciente_tab.Update()

        # 1.2 Remove connector 889 (between Tutores_DNI101 and Pacientes_DNI101)
        ea.Execute("DELETE FROM t_connector WHERE Start_Object_ID = 429 OR End_Object_ID = 429")
        el_paciente_tab.Connectors.Refresh()

        # ==============================================================
        # 2. REBUILD CUN02 DER (DIAGRAM 31)
        # ==============================================================
        print("=== 2. REBUILDING CUN02 DER (DIAGRAM 31) ===")
        diag31 = ea.GetDiagramByID(31)
        diag31.Name = "CUN02: DER (Modelo Relacional)"
        diag31.Notes = "Diagrama Entidad-Relación (DER) del CUN02 (Registrar Paciente Pediátrico). Tablas afectadas: Pacientes_DNI101, Bitacora, Usuarios y DV. Tutores_DNI101 ha sido removida por no utilizarse."
        diag31.parentID = 38
        diag31.Update()

        # Clear existing DiagramObjects
        for i in range(diag31.DiagramObjects.Count - 1, -1, -1):
            diag31.DiagramObjects.Delete(i)
        diag31.DiagramObjects.Refresh()

        el_usuario_tab = ea.GetElementByID(431)
        el_dv_tab = ea.GetElementByID(435)
        el_bitacora_tab = ea.GetElementByID(436)

        # 2-column balanced layout for CUN02 DER:
        # Left column: Pacientes_DNI101 (Top = 50, Bottom = 340) & DV (Top = 410, Bottom = 530)
        # Right column: Bitacora (Top = 50, Bottom = 250) & Usuarios (Top = 330, Bottom = 640)
        coords31 = [
            (el_paciente_tab, 50, 50, 380, 350),
            (el_dv_tab, 50, 420, 380, 540),
            (el_bitacora_tab, 470, 50, 810, 250),
            (el_usuario_tab, 470, 330, 810, 640)
        ]

        for el, left, top, right, bottom in coords31:
            do = diag31.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag31.DiagramObjects.Refresh()
        diag31.Update()
        ea.SaveDiagram(31)

        # Style with Information Engineering (crows foot) and orthogonal links
        ea.Execute("""
UPDATE t_diagram
SET StyleEx = 'TConnectorNotation=Information Engineering;ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;',
    PDATA = 'HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=0;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;',
    ShowForeign = 0,
    ShowPackageContents = 0
WHERE Diagram_ID = 31
        """)
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 31")

        out_png31 = os.path.join(scratch_dir, "cun02_der.png")
        if os.path.exists(out_png31):
            os.remove(out_png31)
        proj.PutDiagramImageToFile(diag31.DiagramGUID, out_png31, 1)
        shutil.copy2(out_png31, os.path.join(artifact_dir, "cun02_der.png"))
        print("Diagram 31 exported to:", out_png31)

        # ==============================================================
        # 3. REBUILD CUN01 DER (DIAGRAM 26) - ALL 7 AFFECTED TABLES, NO TUTOR
        # ==============================================================
        print("=== 3. REBUILDING CUN01 DER (DIAGRAM 26) ===")
        diag26 = ea.GetDiagramByID(26)
        diag26.Name = "CUN01: DER (Modelo Relacional)"
        diag26.Notes = "Diagrama Entidad-Relación (DER) del CUN01 (Registrar Turno) con todas las 7 tablas afectadas: Turnos_DNI101, BloquesHorarios_DNI101, AgendasMedicas_DNI101, Pacientes_DNI101, Usuarios, Bitacora y DV. Tutores_DNI101 ha sido excluida por no pertenecer al modelo."
        diag26.parentID = 12
        diag26.Update()

        # Clear existing DiagramObjects
        for i in range(diag26.DiagramObjects.Count - 1, -1, -1):
            diag26.DiagramObjects.Delete(i)
        diag26.DiagramObjects.Refresh()

        el_agenda_tab = ea.GetElementByID(432)
        el_bloque_tab = ea.GetElementByID(433)
        el_turno_tab = ea.GetElementByID(434)

        # 3-column layout:
        # Left column (Patient & Integrity):
        #   Pacientes_DNI101: Left = 50,  Right = 360, Top = 50,  Bottom = 350
        #   DV:               Left = 50,  Right = 360, Top = 430, Bottom = 550
        # Center column (Turn & Audit):
        #   Bitacora:         Left = 440, Right = 770, Top = 50,  Bottom = 250
        #   Usuarios:         Left = 440, Right = 770, Top = 330, Bottom = 640
        #   Turnos_DNI101:    Left = 440, Right = 770, Top = 720, Bottom = 1030
        # Right column (Agenda & Blocks):
        #   AgendasMedicas_DNI101:  Left = 850, Right = 1170, Top = 50,  Bottom = 240
        #   BloquesHorarios_DNI101: Left = 850, Right = 1170, Top = 340, Bottom = 560

        coords26 = [
            (el_paciente_tab, 50, 50, 360, 350),
            (el_dv_tab, 50, 430, 360, 550),
            (el_bitacora_tab, 440, 50, 770, 250),
            (el_usuario_tab, 440, 330, 770, 640),
            (el_turno_tab, 440, 720, 770, 1030),
            (el_agenda_tab, 850, 50, 1170, 240),
            (el_bloque_tab, 850, 340, 1170, 560)
        ]

        for el, left, top, right, bottom in coords26:
            do = diag26.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag26.DiagramObjects.Refresh()
        diag26.Update()
        ea.SaveDiagram(26)

        ea.Execute("""
UPDATE t_diagram
SET StyleEx = 'TConnectorNotation=Information Engineering;ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;',
    PDATA = 'HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=0;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;',
    ShowForeign = 0,
    ShowPackageContents = 0
WHERE Diagram_ID = 26
        """)
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 26")

        out_png26 = os.path.join(scratch_dir, "cun01_der.png")
        if os.path.exists(out_png26):
            os.remove(out_png26)
        proj.PutDiagramImageToFile(diag26.DiagramGUID, out_png26, 1)
        shutil.copy2(out_png26, os.path.join(artifact_dir, "cun01_der.png"))
        print("Diagram 26 exported to:", out_png26)

        # ==============================================================
        # 4. REBUILD CUN01 CLASS DIAGRAM (DIAGRAM 25) - COMPLETE METHODS
        # ==============================================================
        print("=== 4. REBUILDING CUN01 CLASS DIAGRAM (DIAGRAM 25) ===")
        pkg = ea.GetPackageByID(7) # Package 7: Class Model

        def get_or_create_element(name, el_type="Class"):
            for el in pkg.Elements:
                if el.Name == name and el.Type == el_type:
                    return el
            new_el = pkg.Elements.AddNew(name, el_type)
            new_el.Update()
            pkg.Elements.Refresh()
            return new_el

        def populate_attributes_only(el, attrs):
            for i in range(el.Attributes.Count - 1, -1, -1):
                el.Attributes.Delete(i)
            el.Attributes.Refresh()

            for a_name, a_type in attrs:
                attr = el.Attributes.AddNew(a_name, a_type)
                attr.Visibility = "Private"
                attr.Update()
            el.Attributes.Refresh()

            for i in range(el.Methods.Count - 1, -1, -1):
                el.Methods.Delete(i)
            el.Methods.Refresh()
            el.Update()

        def populate_methods_only(el, methods):
            for i in range(el.Attributes.Count - 1, -1, -1):
                el.Attributes.Delete(i)
            el.Attributes.Refresh()

            for i in range(el.Methods.Count - 1, -1, -1):
                el.Methods.Delete(i)
            el.Methods.Refresh()

            for m_name, m_ret, m_params in methods:
                meth = el.Methods.AddNew(m_name, m_ret)
                meth.Visibility = "Public"
                meth.Update()
                pos = 0
                for p_name, p_type in m_params:
                    param = meth.Parameters.AddNew(p_name, p_type)
                    param.Position = pos
                    param.Update()
                    pos += 1
                meth.Parameters.Refresh()
                meth.Update()
            el.Methods.Refresh()
            el.Update()

        # BE elements
        el_paciente_be = get_or_create_element("PacienteBE_DNI101", "Class")
        el_turno_be = get_or_create_element("TurnoBE_DNI101", "Class")
        el_bloque_be = get_or_create_element("BloqueHorarioBE_DNI101", "Class")
        el_agenda_be = get_or_create_element("AgendaMedicaBE_DNI101", "Class")

        # BLL elements
        el_paciente_bll = get_or_create_element("PacienteBLL_DNI101", "Class")
        el_turno_bll = get_or_create_element("TurnoBLL_DNI101", "Class")
        el_agenda_bll = get_or_create_element("AgendaMedicaBLL_DNI101", "Class")

        # DAL elements
        el_paciente_dal = get_or_create_element("PacienteDAL_DNI101", "Class")
        el_turno_dal = get_or_create_element("TurnoDAL_DNI101", "Class")
        el_agenda_dal = get_or_create_element("AgendaMedicaDAL_DNI101", "Class")

        # Populate ALL methods for TurnoBLL_DNI101
        turno_bll_methods = [
            ("AgendarTurno", "bool", [("turno", "TurnoBE_DNI101")]),
            ("CancelarTurno", "bool", [("codigoTurno", "string"), ("motivo", "string")]),
            ("ListarTurnos", "List<TurnoBE_DNI101>", [("fecha", "DateTime?"), ("estado", "string?")]),
            ("MarcarAsistencia", "bool", [("idTurno", "int")]),
            ("ModificarTurno", "bool", [("codigoTurno", "string"), ("motivo", "string"), ("estado", "string?"), ("fecha", "DateTime?"), ("hora", "TimeSpan?")]),
            ("ObtenerPorCodigo", "TurnoBE_DNI101", [("codigoTurno", "string")]),
            ("ObtenerPorId", "TurnoBE_DNI101", [("idTurno", "int")]),
            ("RegistrarTurno", "TurnoBE_DNI101", [("dniNiño", "string"), ("fecha", "DateTime"), ("horario", "string"), ("motivo", "string"), ("idBloque", "int?"), ("dniNutricionistaParam", "int?")]),
            ("ReprogramarTurno", "bool", [("codigoTurno", "string"), ("nuevaFecha", "DateTime"), ("nuevaHora", "TimeSpan"), ("nuevoIdBloque", "int?"), ("nuevoDniNutricionista", "int?")])
        ]
        populate_methods_only(el_turno_bll, turno_bll_methods)

        # Populate ALL methods for TurnoDAL_DNI101
        turno_dal_methods = [
            ("ActualizarEstado", "bool", [("idTurno", "int"), ("nuevoEstado", "string")]),
            ("ActualizarEstadoBloque", "bool", [("idBloque", "int"), ("nuevoEstado", "string")]),
            ("CancelarTurno", "bool", [("idTurno", "int"), ("motivo", "string"), ("dv", "string?")]),
            ("ExisteTurnoParaProfesional", "bool", [("dniNutricionista", "int"), ("fecha", "DateTime"), ("hora", "TimeSpan"), ("idTurnoExcluir", "int?")]),
            ("Guardar", "int", [("turno", "TurnoBE_DNI101")]),
            ("ListarTodos", "List<TurnoBE_DNI101>", []),
            ("ListarTurnosPorFecha", "List<TurnoBE_DNI101>", [("fecha", "DateTime")]),
            ("ListarTurnosPorPaciente", "List<TurnoBE_DNI101>", [("idPaciente", "int")]),
            ("ModificarTurno", "bool", [("idTurno", "int"), ("fecha", "DateTime"), ("hora", "TimeSpan"), ("motivo", "string"), ("estado", "string"), ("dv", "string?")]),
            ("ObtenerHorariosOcupadosPorProfesional", "List<TimeSpan>", [("dniNutricionista", "int"), ("fecha", "DateTime")]),
            ("ObtenerPorCodigo", "TurnoBE_DNI101", [("codigoTurno", "string")]),
            ("ObtenerPorId", "TurnoBE_DNI101", [("idTurno", "int")]),
            ("ReprogramarTurno", "bool", [("idTurno", "int"), ("nuevaFecha", "DateTime"), ("nuevaHora", "TimeSpan"), ("nuevoIdBloque", "int?"), ("dniNutricionista", "int?"), ("dv", "string?")])
        ]
        populate_methods_only(el_turno_dal, turno_dal_methods)

        # Populate ALL methods for AgendaMedicaBLL_DNI101
        agenda_bll_methods = [
            ("ActualizarEstadoBloque", "bool", [("idBloque", "int"), ("nuevoEstado", "string")]),
            ("ListarBloquesDisponibles", "List<BloqueHorarioBE_DNI101>", [("fecha", "DateTime"), ("dniNutricionista", "int")])
        ]
        populate_methods_only(el_agenda_bll, agenda_bll_methods)

        # Populate ALL methods for AgendaMedicaDAL_DNI101
        agenda_dal_methods = [
            ("ActualizarEstadoBloque", "bool", [("idBloque", "int"), ("nuevoEstado", "string")]),
            ("ListarBloquesDisponibles", "List<BloqueHorarioBE_DNI101>", [("fecha", "DateTime"), ("dniNutricionista", "int")])
        ]
        populate_methods_only(el_agenda_dal, agenda_dal_methods)

        # Populate ALL methods for PacienteBLL_DNI101 (NO TUTOR)
        paciente_bll_methods = [
            ("ListarPacientes", "List<PacienteBE_DNI101>", []),
            ("ModificarPaciente", "bool", [("paciente", "PacienteBE_DNI101")]),
            ("ObtenerPacientePorDNI", "PacienteBE_DNI101", [("dniNiño", "string")]),
            ("ObtenerPorId", "PacienteBE_DNI101", [("idPaciente", "int")]),
            ("RegistrarPaciente", "int", [("nombre", "string"), ("apellido", "string"), ("dniNiño", "string"), ("telefono", "string"), ("email", "string"), ("obraSocial", "string")])
        ]
        populate_methods_only(el_paciente_bll, paciente_bll_methods)

        # Populate ALL methods for PacienteDAL_DNI101
        paciente_dal_methods = [
            ("Guardar", "int", [("paciente", "PacienteBE_DNI101")]),
            ("ListarTodos", "List<PacienteBE_DNI101>", []),
            ("Modificar", "bool", [("paciente", "PacienteBE_DNI101")]),
            ("ObtenerPacientePorDNI", "PacienteBE_DNI101", [("dniNiño", "string")]),
            ("ObtenerPorId", "PacienteBE_DNI101", [("idPaciente", "int")])
        ]
        populate_methods_only(el_paciente_dal, paciente_dal_methods)

        # BE Connectors & Relationships
        cun01_elements = [
            el_paciente_be, el_turno_be, el_bloque_be, el_agenda_be,
            el_paciente_bll, el_turno_bll, el_agenda_bll,
            el_paciente_dal, el_turno_dal, el_agenda_dal
        ]
        c01_ids = [e.ElementID for e in cun01_elements]
        id_str = ",".join(str(i) for i in c01_ids)

        # Clear existing connectors between CUN01 elements
        ea.Execute(f"DELETE FROM t_connector WHERE Start_Object_ID IN ({id_str}) AND End_Object_ID IN ({id_str})")
        for el in cun01_elements:
            el.Connectors.Refresh()

        # BE Relations:
        # 1. PacienteBE (1) ◇---> (0..*) TurnoBE (Aggregation)
        c_agg = el_paciente_be.Connectors.AddNew("", "Aggregation")
        c_agg.SupplierID = el_turno_be.ElementID
        c_agg.Direction = "Source -> Destination"
        c_agg.ClientEnd.Cardinality = "1"
        c_agg.SupplierEnd.Cardinality = "0..*"
        c_agg.SupplierEnd.Role = "Turnos"
        c_agg.Update()
        el_paciente_be.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 1, DestIsAggregate = 0, SubType = 'Weak', Stereotype = null WHERE Connector_ID = {c_agg.ConnectorID}")

        # 2. AgendaMedicaBE (1) ◆---> (1..*) BloqueHorarioBE (Composition)
        c_comp = el_agenda_be.Connectors.AddNew("", "Aggregation")
        c_comp.SupplierID = el_bloque_be.ElementID
        c_comp.Direction = "Source -> Destination"
        c_comp.ClientEnd.Cardinality = "1"
        c_comp.SupplierEnd.Cardinality = "1..*"
        c_comp.SupplierEnd.Role = "BloquesHorarios_DNI101"
        c_comp.Update()
        el_agenda_be.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 2, DestIsAggregate = 0, SubType = 'Strong', Stereotype = null WHERE Connector_ID = {c_comp.ConnectorID}")

        # 3. TurnoBE - - - > BloqueHorarioBE («use»)
        def add_use(source_el, target_el, role=""):
            conn = source_el.Connectors.AddNew("", "Dependency")
            conn.SupplierID = target_el.ElementID
            conn.Stereotype = "use"
            conn.Direction = "Source -> Destination"
            if role:
                conn.SupplierEnd.Role = role
            conn.Update()
            source_el.Connectors.Refresh()
            ea.Execute(f"UPDATE t_connector SET Stereotype = 'use' WHERE Connector_ID = {conn.ConnectorID}")
            return conn

        add_use(el_turno_be, el_bloque_be, "IdBloque_DNI101")

        # 4. ONLY BLL HAS RELATION OF «use» WITH BE AND DAL!
        # BLL -> BE
        add_use(el_turno_bll, el_turno_be)
        add_use(el_paciente_bll, el_paciente_be)
        add_use(el_agenda_bll, el_bloque_be)
        add_use(el_agenda_bll, el_agenda_be)

        # BLL -> BLL
        add_use(el_turno_bll, el_paciente_bll)
        add_use(el_turno_bll, el_agenda_bll)

        # BLL -> DAL
        add_use(el_turno_bll, el_turno_dal)
        add_use(el_paciente_bll, el_paciente_dal)
        add_use(el_agenda_bll, el_agenda_dal)

        # DAL HAS ZERO OUTGOING RELATIONS TO BE!

        # 5. DIAGRAM 25 LAYOUT (WITHOUT TUTOR, GENEROUS WIDTHS)
        diag25 = ea.GetDiagramByID(25)
        diag25.Name = "CUN01: Diagrama de Clases (BE, BLL y DAL)"
        diag25.Notes = "Diagrama de Clases de CUN01 (Registrar Turno) con arquitectura de 3 capas: BE (Entidades), BLL (Lógica de Negocio) y DAL (Acceso a Datos). Todos los métodos de las clases están sincronizados con el código fuente C#."
        diag25.parentID = 12
        diag25.Update()

        # Clear existing DiagramObjects in Diagram 25
        for i in range(diag25.DiagramObjects.Count - 1, -1, -1):
            diag25.DiagramObjects.Delete(i)
        diag25.DiagramObjects.Refresh()

        # 3 Columns Layout:
        # Col 1 (Paciente): Left = 50, Right = 460 (width: 410)
        #   PacienteBE:     Top = 50,  Bottom = 330
        #   PacienteBLL:    Top = 410, Bottom = 630
        #   PacienteDAL:    Top = 710, Bottom = 930
        #
        # Col 2 (Turno):    Left = 530, Right = 1170 (width: 640)
        #   TurnoBE:        Top = 50,  Bottom = 330
        #   TurnoBLL:       Top = 410, Bottom = 650
        #   TurnoDAL:       Top = 710, Bottom = 1010
        #
        # Col 3 (Agenda / Bloque): Left = 1240, Right = 1860 (width: 620)
        #   BloqueHorarioBE:  Left = 1240, Right = 1530, Top = 50, Bottom = 330
        #   AgendaMedicaBE:   Left = 1570, Right = 1860, Top = 50, Bottom = 330
        #   AgendaMedicaBLL:  Left = 1240, Right = 1860, Top = 410, Bottom = 580
        #   AgendaMedicaDAL:  Left = 1240, Right = 1860, Top = 710, Bottom = 880

        coords25 = [
            # Col 1: Paciente
            (el_paciente_be, 50, 50, 460, 330),
            (el_paciente_bll, 50, 410, 460, 630),
            (el_paciente_dal, 50, 710, 460, 930),
            # Col 2: Turno
            (el_turno_be, 530, 50, 1170, 330),
            (el_turno_bll, 530, 410, 1170, 650),
            (el_turno_dal, 530, 710, 1170, 1010),
            # Col 3: Agenda / Bloque
            (el_bloque_be, 1240, 50, 1530, 330),
            (el_agenda_be, 1570, 50, 1860, 330),
            (el_agenda_bll, 1240, 410, 1860, 580),
            (el_agenda_dal, 1240, 710, 1860, 880)
        ]

        for el, left, top, right, bottom in coords25:
            do = diag25.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag25.DiagramObjects.Refresh()
        diag25.Update()
        ea.SaveDiagram(25)

        ea.Execute("""
UPDATE t_diagram
SET StyleEx = 'ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TConnectorNotation=UML 2.1;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;',
    PDATA = 'HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=0;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;',
    ShowForeign = 0,
    ShowPackageContents = 0
WHERE Diagram_ID = 25
        """)
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 25")

        out_png25 = os.path.join(scratch_dir, "cun01_clases_bll_dal_be_exact.png")
        if os.path.exists(out_png25):
            os.remove(out_png25)
        proj.PutDiagramImageToFile(diag25.DiagramGUID, out_png25, 1)
        shutil.copy2(out_png25, os.path.join(artifact_dir, "cun01_clases_bll_dal_be_exact.png"))
        print("Diagram 25 exported to:", out_png25)

        # ==============================================================
        # 5. REBUILD CUN02 CLASS DIAGRAM (DIAGRAM 30) - REMOVE TUTOR
        # ==============================================================
        print("=== 5. REBUILDING CUN02 CLASS DIAGRAM (DIAGRAM 30) - REMOVING TUTOR ===")
        diag30 = ea.GetDiagramByID(30)
        diag30.Name = "CUN02: Diagrama de Clases (BE, BLL y DAL)"
        diag30.Notes = "Diagrama de Clases de CUN02 (Registrar Paciente Pediátrico) con separación de capas BE (Entidades), BLL (Lógica y Seguridad) y DAL (Acceso a Datos). Únicamente BLL mantiene relaciones de uso («use»). TutorBE ha sido removido por no pertenecer al modelo."
        diag30.parentID = 38
        diag30.Update()

        for i in range(diag30.DiagramObjects.Count - 1, -1, -1):
            diag30.DiagramObjects.Delete(i)
        diag30.DiagramObjects.Refresh()

        el_evento_bll = get_or_create_element("EventoBLL", "Class")
        el_dv_bll = get_or_create_element("DigitoVerificadorBLL", "Class")
        el_evento_dal = get_or_create_element("EventoDAL", "Class")
        el_dv_dal = get_or_create_element("DigitoVerificadorDAL", "Class")

        # Clear connectors of TutorBE
        el_tutor_be = get_or_create_element("TutorBE_DNI101", "Class")
        ea.Execute(f"DELETE FROM t_connector WHERE Start_Object_ID = {el_tutor_be.ElementID} OR End_Object_ID = {el_tutor_be.ElementID}")
        el_tutor_be.Connectors.Refresh()

        # Connectors for CUN02 without Tutor:
        # PacienteBLL -> PacienteBE («use»)
        add_use(el_paciente_bll, el_paciente_be)
        # PacienteBLL -> PacienteDAL («use»)
        add_use(el_paciente_bll, el_paciente_dal)
        # PacienteBLL -> EventoBLL & DV BLL («use»)
        add_use(el_paciente_bll, el_evento_bll)
        add_use(el_paciente_bll, el_dv_bll)
        # EventoBLL -> EventoDAL («use»)
        add_use(el_evento_bll, el_evento_dal)
        # DV BLL -> DV DAL («use»)
        add_use(el_dv_bll, el_dv_dal)

        # 3 Column Layout for CUN02:
        # Col 1: Paciente (Left = 50, Right = 520, width 470)
        #   PacienteBE:  Top = 50,  Bottom = 330
        #   PacienteBLL: Top = 410, Bottom = 630
        #   PacienteDAL: Top = 710, Bottom = 930
        #
        # Col 2: Evento (Left = 600, Right = 1040, width 440)
        #   EventoBLL:   Top = 410, Bottom = 630
        #   EventoDAL:   Top = 710, Bottom = 930
        #
        # Col 3: DV (Left = 1110, Right = 1650, width 540)
        #   DV BLL:      Top = 410, Bottom = 630
        #   DV DAL:      Top = 710, Bottom = 930

        coords30 = [
            # Col 1: Paciente
            (el_paciente_be, 50, 50, 520, 330),
            (el_paciente_bll, 50, 410, 520, 630),
            (el_paciente_dal, 50, 710, 520, 930),
            # Col 2: Evento
            (el_evento_bll, 600, 410, 1040, 630),
            (el_evento_dal, 600, 710, 1040, 930),
            # Col 3: DV
            (el_dv_bll, 1110, 410, 1650, 630),
            (el_dv_dal, 1110, 710, 1650, 930)
        ]

        for el, left, top, right, bottom in coords30:
            do = diag30.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag30.DiagramObjects.Refresh()
        diag30.Update()
        ea.SaveDiagram(30)

        ea.Execute("""
UPDATE t_diagram
SET StyleEx = 'ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TConnectorNotation=UML 2.1;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;',
    PDATA = 'HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=0;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;',
    ShowForeign = 0,
    ShowPackageContents = 0
WHERE Diagram_ID = 30
        """)
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 30")

        out_png30 = os.path.join(scratch_dir, "cun02_clases_bll_dal_be.png")
        if os.path.exists(out_png30):
            os.remove(out_png30)
        proj.PutDiagramImageToFile(diag30.DiagramGUID, out_png30, 1)
        shutil.copy2(out_png30, os.path.join(artifact_dir, "cun02_clases_bll_dal_be.png"))
        print("Diagram 30 exported to:", out_png30)

        print("=== ALL 4 DIAGRAMS (CUN01 CLASES, CUN01 DER, CUN02 CLASES, CUN02 DER) UPDATED SUCCESSFULLY ===")

    finally:
        ea.CloseFile()
        ea.Exit()
        print("EA closed.")

if __name__ == "__main__":
    sync_all()
