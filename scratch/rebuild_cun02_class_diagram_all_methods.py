import win32com.client
import os
import shutil

def rebuild_cun02_class_diagram():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    try:
        print("=== REBUILDING CUN02 CLASS DIAGRAM WITH COMPLETE METHODS & CLEAN NAMES ===")
        pkg = ea.GetPackageByID(7) # Package 7: Class Model

        # Helper to get or create element in Package 7
        def get_or_create_element(name, el_type="Class"):
            for el in pkg.Elements:
                if el.Name == name and el.Type == el_type:
                    return el
            new_el = pkg.Elements.AddNew(name, el_type)
            new_el.Update()
            pkg.Elements.Refresh()
            return new_el

        # Helper for BE classes: populate ONLY attributes, delete all methods
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

        # Helper for BLL and DAL classes: populate ONLY methods, delete all attributes
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

        # Helper to add «use» dependency
        def add_use_dependency(source_el, target_el, role=""):
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

        # ==============================================================
        # 1. POPULATE BE CLASSES (ATTRIBUTES ONLY)
        # ==============================================================
        el_tutor_be = get_or_create_element("TutorBE_DNI101", "Class")
        tutor_attrs = [
            ("Apellido_DNI101", "string"),
            ("DniTutor_DNI101", "string"),
            ("DV", "string"),
            ("Email_DNI101", "string"),
            ("FechaRegistro_DNI101", "DateTime"),
            ("IdTutor_DNI101", "int"),
            ("Nombre_DNI101", "string"),
            ("NombreCompleto", "string"),
            ("Parentesco_DNI101", "string"),
            ("Telefono_DNI101", "string")
        ]
        populate_attributes_only(el_tutor_be, tutor_attrs)

        el_paciente_be = get_or_create_element("PacienteBE_DNI101", "Class")
        paciente_attrs = [
            ("Apellido_DNI101", "string"),
            ("DNINiño_DNI101", "string"),
            ("DV", "string"),
            ("Email_DNI101", "string"),
            ("FechaNacimiento_DNI101", "DateTime"),
            ("IdPaciente_DNI101", "int"),
            ("Nombre_DNI101", "string"),
            ("NombreCompleto", "string"),
            ("ObraSocial_DNI101", "string"),
            ("Sexo_DNI101", "string"),
            ("Telefono_DNI101", "string")
        ]
        populate_attributes_only(el_paciente_be, paciente_attrs)

        # ==============================================================
        # 2. POPULATE BLL CLASSES (COMPLETE METHODS FROM CODE)
        # ==============================================================
        # PacienteBLL_DNI101
        el_paciente_bll = get_or_create_element("PacienteBLL_DNI101", "Class")
        paciente_bll_methods = [
            ("ListarPacientes", "List<PacienteBE_DNI101>", []),
            ("ModificarPaciente", "bool", [
                ("paciente", "PacienteBE_DNI101")
            ]),
            ("ObtenerPacientePorDNI", "PacienteBE_DNI101", [
                ("dniNiño", "string")
            ]),
            ("ObtenerPorId", "PacienteBE_DNI101", [
                ("idPaciente", "int")
            ]),
            ("RegistrarPaciente", "int", [
                ("nombre", "string"),
                ("apellido", "string"),
                ("dniNiño", "string"),
                ("telefono", "string"),
                ("email", "string"),
                ("obraSocial", "string")
            ])
        ]
        populate_methods_only(el_paciente_bll, paciente_bll_methods)

        # EventoBLL (ALL 3 methods from EventoBLL.cs)
        el_evento_bll = get_or_create_element("EventoBLL", "Class")
        evento_bll_methods = [
            ("BuscarEventos", "List<EventoBE>", [
                ("desde", "DateTime"),
                ("hasta", "DateTime")
            ]),
            ("RegistrarEvento", "bool", [
                ("criticidad", "int"),
                ("descripcion", "string"),
                ("dni", "int"),
                ("modulo", "string")
            ]),
            ("VerEventos", "List<EventoBE>", [])
        ]
        populate_methods_only(el_evento_bll, evento_bll_methods)

        # DigitoVerificadorBLL (ALL 4 methods from DigitoVerificadorBLL.cs)
        el_dv_bll = get_or_create_element("DigitoVerificadorBLL", "Class")
        dv_bll_methods = [
            ("ActualizarDVIndividualesUsuarios", "void", []),
            ("ObtenerUsuariosCorruptos", "List<string>", []),
            ("RecalcularYPersistir", "void", []),
            ("VerificarBaseDatos", "bool", [])
        ]
        populate_methods_only(el_dv_bll, dv_bll_methods)

        # ==============================================================
        # 3. POPULATE DAL CLASSES (COMPLETE METHODS FROM CODE)
        # ==============================================================
        # PacienteDAL_DNI101
        el_paciente_dal = get_or_create_element("PacienteDAL_DNI101", "Class")
        paciente_dal_methods = [
            ("Guardar", "int", [
                ("paciente", "PacienteBE_DNI101")
            ]),
            ("ListarTodos", "List<PacienteBE_DNI101>", []),
            ("Modificar", "bool", [
                ("paciente", "PacienteBE_DNI101")
            ]),
            ("ObtenerPacientePorDNI", "PacienteBE_DNI101", [
                ("dniNiño", "string")
            ]),
            ("ObtenerPorId", "PacienteBE_DNI101", [
                ("idPaciente", "int")
            ])
        ]
        populate_methods_only(el_paciente_dal, paciente_dal_methods)

        # EventoDAL (ALL 3 methods from EventoDAL.cs)
        el_evento_dal = get_or_create_element("EventoDAL", "Class")
        evento_dal_methods = [
            ("FiltrarBitacora", "List<EventoBE>", [
                ("desde", "DateTime"),
                ("hasta", "DateTime")
            ]),
            ("GuardarBitacora", "void", [
                ("newBitacora", "EventoBE")
            ]),
            ("ObtenerBitacora", "List<EventoBE>", [])
        ]
        populate_methods_only(el_evento_dal, evento_dal_methods)

        # DigitoVerificadorDAL (ALL methods from DigitoVerificadorDAL.cs)
        el_dv_dal = get_or_create_element("DigitoVerificadorDAL", "Class")
        dv_dal_methods = [
            ("ActualizarDVRegistro", "void", [
                ("schema", "string"),
                ("tabla", "string"),
                ("nombreColumnaId", "string"),
                ("valorId", "object"),
                ("dvCalculado", "string")
            ]),
            ("ExisteTablaDV", "bool", []),
            ("ObtenerDatosTabla", "DataTable", [
                ("schema", "string"),
                ("table", "string")
            ]),
            ("ObtenerResumenPersistido", "List<ResumenDigitoVerificador>", []),
            ("ObtenerTablasPersistentes", "List<(string, string)>", []),
            ("ReemplazarResumen", "void", [
                ("resumen", "IEnumerable<ResumenDigitoVerificador>")
            ])
        ]
        populate_methods_only(el_dv_dal, dv_dal_methods)

        pkg.Elements.Refresh()

        # ==============================================================
        # 4. CONNECTORS
        # ==============================================================
        all_cun02_elements = [
            el_tutor_be, el_paciente_be,
            el_paciente_bll, el_evento_bll, el_dv_bll,
            el_paciente_dal, el_evento_dal, el_dv_dal
        ]
        all_ids = [e.ElementID for e in all_cun02_elements]
        id_str = ",".join(str(i) for i in all_ids)

        # Remove existing connectors between these 8 elements
        ea.Execute(f"DELETE FROM t_connector WHERE Start_Object_ID IN ({id_str}) AND End_Object_ID IN ({id_str})")
        for el in all_cun02_elements:
            el.Connectors.Refresh()

        # A. Weak Aggregation: TutorBE ◇---> PacienteBE (1 to 1..*)
        c_agg = el_tutor_be.Connectors.AddNew("", "Aggregation")
        c_agg.SupplierID = el_paciente_be.ElementID
        c_agg.Direction = "Source -> Destination"
        c_agg.ClientEnd.Cardinality = "1"
        c_agg.SupplierEnd.Cardinality = "1..*"
        c_agg.SupplierEnd.Role = "Pacientes_DNI101"
        c_agg.Update()
        el_tutor_be.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 1, DestIsAggregate = 0, SubType = 'Weak', Stereotype = null WHERE Connector_ID = {c_agg.ConnectorID}")

        # B. ONLY BLL HAS RELATION OF «use» WITH BE AND DAL!
        # PacienteBLL -> PacienteBE & TutorBE
        add_use_dependency(el_paciente_bll, el_paciente_be)
        add_use_dependency(el_paciente_bll, el_tutor_be)

        # PacienteBLL -> PacienteDAL
        add_use_dependency(el_paciente_bll, el_paciente_dal)

        # PacienteBLL -> EventoBLL & DigitoVerificadorBLL
        add_use_dependency(el_paciente_bll, el_evento_bll)
        add_use_dependency(el_paciente_bll, el_dv_bll)

        # EventoBLL -> EventoDAL
        add_use_dependency(el_evento_bll, el_evento_dal)

        # DigitoVerificadorBLL -> DigitoVerificadorDAL
        add_use_dependency(el_dv_bll, el_dv_dal)

        # ==============================================================
        # 5. DIAGRAM 30 CONFIGURATION & LAYOUT
        # ==============================================================
        diag30 = ea.GetDiagramByID(30)
        diag30.Name = "CUN02: Diagrama de Clases (BE, BLL y DAL)"
        diag30.Notes = "Diagrama de Clases de CUN02 (Registrar Paciente Pediátrico) con arquitectura multicapa: BE (Entidades), BLL (Lógica de Negocio y Seguridad) y DAL (Acceso a Datos). Únicamente BLL mantiene relaciones de uso («use»). Todos los métodos y atributos están sincronizados con el código fuente C#."
        diag30.parentID = 38 # Move to UseCase 38 (CUN02)
        diag30.Update()
        ea.Execute("UPDATE t_diagram SET ParentID = 38, ShowForeign = 0, ShowPackageContents = 0 WHERE Diagram_ID = 30")

        # Clear existing DiagramObjects in Diagram 30
        for i in range(diag30.DiagramObjects.Count - 1, -1, -1):
            diag30.DiagramObjects.Delete(i)
        diag30.DiagramObjects.Refresh()

        # Layout for Diagram 30 (Generous width to show all method signatures without truncation):
        # Column 1: Paciente / Tutor (Left = 50 .. Right = 590, width: 540)
        #   TutorBE:      Left = 50,  Right = 310, Top = 50,  Bottom = 330
        #   PacienteBE:   Left = 330, Right = 590, Top = 50,  Bottom = 330
        #   PacienteBLL:  Left = 50,  Right = 590, Top = 410, Bottom = 630
        #   PacienteDAL:  Left = 50,  Right = 590, Top = 710, Bottom = 930
        #
        # Column 2: Evento / Bitacora (Left = 670 .. Right = 1110, width: 440)
        #   EventoBLL:    Left = 670, Right = 1110, Top = 410, Bottom = 630
        #   EventoDAL:    Left = 670, Right = 1110, Top = 710, Bottom = 930
        #
        # Column 3: Digito Verificador (Left = 1180 .. Right = 1720, width: 540)
        #   DV BLL:       Left = 1180, Right = 1720, Top = 410, Bottom = 630
        #   DV DAL:       Left = 1180, Right = 1720, Top = 710, Bottom = 930

        coords30 = [
            # Tier 1: BE
            (el_tutor_be, 50, 50, 310, 330),
            (el_paciente_be, 330, 50, 590, 330),
            # Tier 2: BLL
            (el_paciente_bll, 50, 410, 590, 630),
            (el_evento_bll, 670, 410, 1110, 630),
            (el_dv_bll, 1180, 410, 1720, 630),
            # Tier 3: DAL
            (el_paciente_dal, 50, 710, 590, 930),
            (el_evento_dal, 670, 710, 1110, 930),
            (el_dv_dal, 1180, 710, 1720, 930)
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
        ea.SaveDiagram(diag30.DiagramID)

        # StyleEx and PDATA identical to Diagram 25 (HideScopeNames, ShowOpRetType, OpParams, etc.)
        ea.Execute("""
UPDATE t_diagram
SET ShowForeign = 0,
    ShowPackageContents = 0,
    StyleEx = 'ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TConnectorNotation=UML 2.1;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;',
    PDATA = 'HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=0;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;'
WHERE Diagram_ID = 30
        """)

        # Clean orthogonal links
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 30")

        # Export Diagram 30 image
        scratch_dir = r"c:\Users\Danie\Desktop\GIT\TD\scratch"
        out_png30 = os.path.join(scratch_dir, "cun02_clases_bll_dal_be.png")
        if os.path.exists(out_png30):
            os.remove(out_png30)
        proj.PutDiagramImageToFile(diag30.DiagramGUID, out_png30, 1)
        print("Diagram 30 exported to:", out_png30)

        artifact_dir = r"C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e"
        dest_png30 = os.path.join(artifact_dir, "cun02_clases_bll_dal_be.png")
        shutil.copy2(out_png30, dest_png30)
        print("Copied to brain artifact:", dest_png30)

        print("=== REBUILD COMPLETED SUCCESSFULLY ===")

    finally:
        ea.CloseFile()
        ea.Exit()
        print("EA closed.")

if __name__ == "__main__":
    rebuild_cun02_class_diagram()
