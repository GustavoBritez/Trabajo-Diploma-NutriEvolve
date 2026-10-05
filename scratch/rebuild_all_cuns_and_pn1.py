import win32com.client
import os
import shutil
import uuid

def run():
    eap_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP'
    scratch_dir = r'c:\Users\Danie\Desktop\GIT\TD\scratch'
    artifact_dir = r'C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e'

    dss_dir = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\DSS'
    dc_dir = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Clases'
    der_dir = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Entidad Relacion'

    print("Connecting to Enterprise Architect...")
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    def export_and_copy(diag, base_name, dest_folder):
        out_path = os.path.join(dest_folder, base_name + ".png")
        if os.path.exists(out_path):
            try:
                os.remove(out_path)
            except:
                pass
        proj.PutDiagramImageToFile(diag.DiagramGUID, out_path, 1)
        shutil.copy2(out_path, os.path.join(scratch_dir, base_name + ".png"))
        shutil.copy2(out_path, os.path.join(artifact_dir, base_name + ".png"))
        print(f"Exported: {out_path}")

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
        el.Update()

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

    def add_use(src, dst, role=""):
        conn = src.Connectors.AddNew("", "Dependency")
        conn.SupplierID = dst.ElementID
        conn.Stereotype = "use"
        conn.Direction = "Source -> Destination"
        if role:
            conn.SupplierEnd.Role = role
        conn.Update()
        src.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET Stereotype = 'use' WHERE Connector_ID = {conn.ConnectorID}")
        return conn

    try:
        # =====================================================================
        # 1. CUN03: REPROGRAMAR TURNO
        # =====================================================================
        print("\n================== CUN03: REPROGRAMAR TURNO ==================")
        # --- 1.1 Sequence Diagram (Diagram ID 13) ---
        print("Rebuilding Diagram 13 (CUN03 Sequence)...")
        diag13 = ea.GetDiagramByID(13)
        diag13.Name = "CUN03: Diagrama de Secuencia"
        diag13.Notes = "Diagrama de Secuencia CUN03 (Reprogramar Turno): Consulta de disponibilidad a AgendaMedicaBLL, validación en UI, invocación a TurnoBLL_DNI101, validación de estado y superposición en DAL, delegación a TurnoBE_DNI101 (State Pattern), persistencia atómica en DAL, auditoría en BitacoraBLL y recálculo en DigitoVerificadorBLL. Retornos sin texto (PDATA4='1')."
        diag13.Update()

        ea.Execute("DELETE FROM t_diagramlinks WHERE DiagramID = 13")
        ea.Execute("DELETE FROM t_connector WHERE DiagramID = 13")

        el_nutri13 = ea.GetElementByID(266)
        el_gui13 = ea.GetElementByID(767)
        el_agenda_bll13 = ea.GetElementByID(269)
        el_turno_bll13 = ea.GetElementByID(268)
        el_turno_be13 = ea.GetElementByID(270)
        el_bit_bll13 = ea.GetElementByID(274)
        if el_bit_bll13.Name != "BitacoraBLL":
            el_bit_bll13.Name = "BitacoraBLL"
            el_bit_bll13.Update()
        el_dv_bll13 = ea.GetElementByID(273)
        el_dal13 = ea.GetElementByID(272)
        if el_dal13.Name != "DAL":
            el_dal13.Name = "DAL"
            el_dal13.Update()

        el_frag13 = ea.GetElementByID(741)

        layout13 = [
            (el_nutri13,      20,   120,  -50,  -1120), # 70
            (el_gui13,        150,  250,  -50,  -1120), # 200
            (el_agenda_bll13, 290,  430,  -50,  -1120), # 360
            (el_turno_bll13,  490,  630,  -50,  -1120), # 560
            (el_turno_be13,   690,  830,  -50,  -1120), # 760
            (el_bit_bll13,    890,  1030, -50,  -1120), # 960
            (el_dv_bll13,     1090, 1230, -50,  -1120), # 1160
            (el_dal13,        1290, 1430, -50,  -1120), # 1360
        ]

        for i in range(diag13.DiagramObjects.Count - 1, -1, -1):
            diag13.DiagramObjects.Delete(i)
        diag13.DiagramObjects.Refresh()

        coords13_x = {}
        for el, left, right, top, bottom in layout13:
            do = diag13.DiagramObjects.AddNew(f"l={left};r={right};t={abs(top)};b={abs(bottom)};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = top
            do.bottom = bottom
            do.Update()
            coords13_x[el.ElementID] = (left + right) // 2

        # Configure Fragment 741 (alt)
        do_frag13 = diag13.DiagramObjects.AddNew("l=10;r=1450;t=520;b=960;", "")
        do_frag13.ElementID = el_frag13.ElementID
        do_frag13.left = 10
        do_frag13.right = 1450
        do_frag13.top = -520
        do_frag13.bottom = -960
        do_frag13.Sequence = 1
        do_frag13.Update()

        guid1 = str(uuid.uuid4()).upper()
        guid2 = str(uuid.uuid4()).upper()
        part_desc13 = f"@PAR;Name=[Datos Validos y Disponibles];Size=310;GUID={{{guid2}}};@ENDPAR;@PAR;Name=[Estado Invalido o Superposicion: Asistio / Cancelado / Ocupado];Size=130;GUID={{{guid1}}};@ENDPAR;"
        ea.Execute(f"DELETE FROM t_xref WHERE Client = '{el_frag13.ElementGUID}' AND Name = 'Partitions'")
        ea.Execute(f"INSERT INTO t_xref (XrefID, Name, Type, Visibility, Partition, Description, Client) VALUES ('{{{str(uuid.uuid4()).upper()}}}', 'Partitions', 'element property', 'Public', '0', '{part_desc13}', '{el_frag13.ElementGUID}')")

        diag13.DiagramObjects.Refresh()

        messages13 = [
            # Fase 1: Consulta de disponibilidad
            (1, el_nutri13, el_gui13, "SeleccionarNuevaFecha(fecha, dniNutri)", "SynchCall", -110, "Synchronous", "", "Call", ""),
            (2, el_gui13, el_agenda_bll13, "ListarBloquesDisponibles(fecha, dniNutri)", "SynchCall", -140, "Synchronous", "", "Call", ""),
            (3, el_agenda_bll13, el_dal13, "ListarBloquesDisponibles(fecha, dniNutri)", "SynchCall", -170, "Synchronous", "", "Call", ""),
            (4, el_dal13, el_agenda_bll13, "", "Return", -200, "Synchronous", "", "Call", "1"),
            (5, el_agenda_bll13, el_gui13, "", "Return", -230, "Synchronous", "", "Call", "1"),
            (6, el_gui13, el_nutri13, "", "Return", -260, "Synchronous", "", "Call", "1"),

            # Fase 2: Confirmación y validación
            (7, el_nutri13, el_gui13, "btnConfirmar_Click()", "SynchCall", -295, "Synchronous", "", "Call", ""),
            (8, el_gui13, el_gui13, "ValidarCamposObligatorios()", "SynchCall", -325, "Synchronous", "", "Call", ""),
            (9, el_gui13, el_turno_bll13, "ReprogramarTurno(codigoTurno, nuevaFecha, nuevaHora, nuevoIdBloque, dniNutri)", "SynchCall", -360, "Synchronous", "", "Call", ""),
            (10, el_turno_bll13, el_dal13, "ObtenerPorCodigo(codigoTurno)", "SynchCall", -395, "Synchronous", "", "Call", ""),
            (11, el_dal13, el_turno_bll13, "", "Return", -425, "Synchronous", "", "Call", "1"),
            (12, el_turno_bll13, el_dal13, "ExisteTurnoParaProfesional(dniNutri, nuevaFecha, nuevaHora)", "SynchCall", -460, "Synchronous", "", "Call", ""),
            (13, el_dal13, el_turno_bll13, "", "Return", -490, "Synchronous", "", "Call", "1"),

            # ALT PARTITION 1: [Estado Invalido o Superposicion] (-520 a -650)
            (14, el_turno_bll13, el_gui13, "", "Return", -555, "Synchronous", "", "Call", "1"),
            (15, el_gui13, el_nutri13, "MostrarMensaje(msg_error_reprogramar)", "SynchCall", -590, "Synchronous", "", "Call", ""),
            (16, el_nutri13, el_gui13, "", "Return", -620, "Synchronous", "", "Call", "1"),

            # ALT PARTITION 2: [Datos Validos y Disponibles] (-650 a -960)
            (17, el_turno_bll13, el_turno_be13, "Reprogramar(nuevaFecha, nuevaHora, nuevoIdBloque)", "SynchCall", -675, "Synchronous", "", "Call", ""),
            (18, el_turno_be13, el_turno_bll13, "", "Return", -705, "Synchronous", "", "Call", "1"),
            (19, el_turno_bll13, el_dal13, "ReprogramarTurnoConBloques(idTurno, nuevaFecha, nuevaHora, nuevoIdBloque, idBloqueAnt, dniNutri, dv)", "SynchCall", -740, "Synchronous", "", "Call", ""),
            (20, el_dal13, el_turno_bll13, "", "Return", -770, "Synchronous", "", "Call", "1"),
            (21, el_turno_bll13, el_bit_bll13, "RegistrarBitacora(2, Turno reprogramado..., dniActual, TurneroNutricional)", "SynchCall", -800, "Synchronous", "", "Call", ""),
            (22, el_bit_bll13, el_dal13, "GuardarBitacora(bitacora)", "SynchCall", -830, "Synchronous", "", "Call", ""),
            (23, el_dal13, el_bit_bll13, "", "Return", -855, "Synchronous", "", "Call", "1"),
            (24, el_bit_bll13, el_turno_bll13, "", "Return", -875, "Synchronous", "", "Call", "1"),
            (25, el_turno_bll13, el_dv_bll13, "RecalcularYPersistir()", "SynchCall", -900, "Synchronous", "", "Call", ""),
            (26, el_dv_bll13, el_dal13, "PersistirIntegridadCompleta(resumen, filasDV)", "SynchCall", -925, "Synchronous", "", "Call", ""),
            (27, el_dal13, el_dv_bll13, "", "Return", -945, "Synchronous", "", "Call", "1"),
            (28, el_dv_bll13, el_turno_bll13, "", "Return", -965, "Synchronous", "", "Call", "1"),

            # Post-fragment
            (29, el_turno_bll13, el_gui13, "", "Return", -995, "Synchronous", "", "Call", "1"),
            (30, el_gui13, el_nutri13, "MostrarMensaje(msg_turno_reprogramado_ok_det)", "SynchCall", -1025, "Synchronous", "", "Call", ""),
            (31, el_nutri13, el_gui13, "", "Return", -1055, "Synchronous", "", "Call", "1"),
            (32, el_gui13, el_gui13, "Close()", "SynchCall", -1080, "Synchronous", "", "Call", "")
        ]

        for item in messages13:
            seq, src, dst, msg_name, subtype, y, p1, p2, p3, p4 = item
            clean_name = msg_name.replace("'", "''")
            conn = src.Connectors.AddNew(clean_name, "Sequence")
            conn.SupplierID = dst.ElementID
            conn.SequenceNo = seq
            conn.SubType = subtype
            conn.DiagramID = 13
            conn.Update()
            src.Connectors.Refresh()

            sx = coords13_x.get(src.ElementID, 100)
            ex = coords13_x.get(dst.ElementID, 200)
            ey = y - 15 if src.ElementID == dst.ElementID else y
            if src.ElementID == dst.ElementID:
                ex = sx + 25

            p1_sql = p1.replace("'", "''")
            p2_sql = p2.replace("'", "''")
            p3_sql = p3.replace("'", "''")
            p4_sql = p4.replace("'", "''")

            ea.Execute(f"""
            UPDATE t_connector 
            SET PtStartX = {sx}, PtEndX = {ex}, PtStartY = {y}, PtEndY = {ey},
                PDATA1 = '{p1_sql}', PDATA2 = '{p2_sql}', PDATA3 = '{p3_sql}', PDATA4 = '{p4_sql}',
                RouteStyle = 1, LineColor = -1
            WHERE Connector_ID = {conn.ConnectorID}
            """)

        diag13.Update()
        diag13.DiagramObjects.Refresh()
        ea.SaveDiagram(13)
        export_and_copy(diag13, "CUN03 Reprogramar Turno", dss_dir)

        # --- 1.2 Class Diagram (Diagram ID 60) ---
        print("Rebuilding Diagram 60 (CUN03 Classes)...")
        diag60 = ea.GetDiagramByID(60)
        diag60.Name = "CUN03: Diagrama de Clases (BE, BLL y DAL)"
        diag60.Notes = "Diagrama de Clases CUN03 (Reprogramar Turno): Arquitectura de 3 capas. BLL orquesta el negocio y usa BE y DAL. DAL desacoplada de BE."
        diag60.parentID = 13
        diag60.Update()

        el_turno_be = ea.GetElementByID(663)
        el_bloque_be = ea.GetElementByID(665)
        el_agenda_be = ea.GetElementByID(664)

        el_turno_bll = ea.GetElementByID(756)
        el_agenda_bll = ea.GetElementByID(758)
        el_bit_bll = ea.GetElementByID(762)
        el_dv_bll = ea.GetElementByID(763)

        el_turno_dal = ea.GetElementByID(759)
        el_agenda_dal = ea.GetElementByID(761)
        el_bit_dal = ea.GetElementByID(764)
        el_dv_dal = ea.GetElementByID(765)

        c03_all = [el_turno_be, el_bloque_be, el_agenda_be, el_turno_bll, el_agenda_bll, el_bit_bll, el_dv_bll, el_turno_dal, el_agenda_dal, el_bit_dal, el_dv_dal]
        c03_ids = ",".join(str(e.ElementID) for e in c03_all)
        ea.Execute(f"DELETE FROM t_connector WHERE Start_Object_ID IN ({c03_ids}) AND End_Object_ID IN ({c03_ids})")
        for e in c03_all:
            e.Connectors.Refresh()

        # BE Connectors
        c_comp = el_agenda_be.Connectors.AddNew("", "Aggregation")
        c_comp.SupplierID = el_bloque_be.ElementID
        c_comp.Direction = "Source -> Destination"
        c_comp.ClientEnd.Cardinality = "1"
        c_comp.SupplierEnd.Cardinality = "1..*"
        c_comp.Update()
        el_agenda_be.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 2, DestIsAggregate = 0, SubType = 'Strong', Stereotype = null WHERE Connector_ID = {c_comp.ConnectorID}")

        add_use(el_turno_be, el_bloque_be, "+IdBloque_DNI101")

        # BLL to BE
        add_use(el_turno_bll, el_turno_be)
        add_use(el_agenda_bll, el_bloque_be)
        add_use(el_agenda_bll, el_agenda_be)

        # BLL to BLL
        add_use(el_turno_bll, el_agenda_bll)
        add_use(el_turno_bll, el_bit_bll)
        add_use(el_turno_bll, el_dv_bll)

        # BLL to DAL
        add_use(el_turno_bll, el_turno_dal)
        add_use(el_agenda_bll, el_agenda_dal)
        add_use(el_bit_bll, el_bit_dal)
        add_use(el_dv_bll, el_dv_dal)

        for i in range(diag60.DiagramObjects.Count - 1, -1, -1):
            diag60.DiagramObjects.Delete(i)
        diag60.DiagramObjects.Refresh()

        coords60 = [
            # Tier 1: BE
            (el_turno_be,   50,   40,  550,  320),
            (el_bloque_be,  620,  40,  920,  320),
            (el_agenda_be,  980,  40,  1380, 320),

            # Tier 2: BLL
            (el_turno_bll,  50,   400, 550,  660),
            (el_agenda_bll, 620,  400, 980,  660),
            (el_bit_bll,    1040, 400, 1440, 660),
            (el_dv_bll,     1500, 400, 2040, 660),

            # Tier 3: DAL
            (el_turno_dal,  50,   740, 550,  1100),
            (el_agenda_dal, 620,  740, 980,  1100),
            (el_bit_dal,    1040, 740, 1440, 1100),
            (el_dv_dal,     1500, 740, 2040, 1100),
        ]

        for el, left, top, right, bottom in coords60:
            do = diag60.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag60.DiagramObjects.Refresh()
        diag60.Update()
        ea.SaveDiagram(60)
        ea.Execute("""
UPDATE t_diagram
SET ShowForeign = 0, ShowPackageContents = 0,
    StyleEx = 'ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TConnectorNotation=UML 2.1;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;',
    PDATA = 'HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=0;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;'
WHERE Diagram_ID = 60
        """)
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 60")
        export_and_copy(diag60, "CUN03 Reprogramar Turno", dc_dir)

        # --- 1.3 DER (Diagram ID 61) ---
        print("Rebuilding Diagram 61 (CUN03 DER)...")
        diag61 = ea.GetDiagramByID(61)
        diag61.Name = "CUN03: DER (Modelo Relacional)"
        diag61.Notes = "Diagrama Entidad-Relación CUN03 (Reprogramar Turno): Turnos_DNI101, BloquesHorarios_DNI101, AgendasMedicas_DNI101, Usuarios, Bitacora y DV con notación Information Engineering."
        diag61.parentID = 13
        diag61.Update()

        for i in range(diag61.DiagramObjects.Count - 1, -1, -1):
            diag61.DiagramObjects.Delete(i)
        diag61.DiagramObjects.Refresh()

        el_tab_turno = ea.GetElementByID(434)
        el_tab_bloque = ea.GetElementByID(433)
        el_tab_agenda = ea.GetElementByID(432)
        el_tab_usr = ea.GetElementByID(431)
        el_tab_bit = ea.GetElementByID(436)
        el_tab_dv = ea.GetElementByID(435)

        coords61 = [
            (el_tab_usr,    50,  40,  410, 350),
            (el_tab_agenda, 490, 40,  830, 240),
            (el_tab_bloque, 490, 320, 830, 540),
            (el_tab_turno,  490, 620, 830, 930),
            (el_tab_bit,    910, 40,  1250, 250),
            (el_tab_dv,     910, 330, 1250, 460),
        ]

        for el, left, top, right, bottom in coords61:
            do = diag61.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag61.DiagramObjects.Refresh()
        diag61.Update()
        ea.SaveDiagram(61)
        ea.Execute("""
UPDATE t_diagram
SET StyleEx = 'TConnectorNotation=Information Engineering;ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;',
    PDATA = 'HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=0;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;',
    ShowForeign = 0, ShowPackageContents = 0
WHERE Diagram_ID = 61
        """)
        ea.Execute("DELETE FROM t_diagramlinks WHERE DiagramID = 61")
        ea.Execute("INSERT INTO t_diagramlinks (DiagramID, ConnectorID, Style, Hidden) VALUES (61, 891, 'Mode=3;', 0)") # Usuarios -> Agendas
        ea.Execute("INSERT INTO t_diagramlinks (DiagramID, ConnectorID, Style, Hidden) VALUES (61, 894, 'Mode=3;', 0)") # Usuarios -> Turnos
        ea.Execute("INSERT INTO t_diagramlinks (DiagramID, ConnectorID, Style, Hidden) VALUES (61, 895, 'Mode=3;', 0)") # Usuarios -> Bitacora
        ea.Execute("INSERT INTO t_diagramlinks (DiagramID, ConnectorID, Style, Hidden) VALUES (61, 880, 'Mode=3;', 0)") # Agendas -> Bloques
        export_and_copy(diag61, "CUN03 DER (Modelo Relacional)", der_dir)

        # =====================================================================
        # 2. CUN04: MODIFICAR TURNO
        # =====================================================================
        print("\n================== CUN04: MODIFICAR TURNO ==================")
        # --- 2.1 Sequence Diagram (Diagram ID 57) ---
        print("Rebuilding Diagram 57 (CUN04 Sequence)...")
        diag57 = ea.GetDiagramByID(57)
        diag57.Name = "CUN04: Diagrama de Secuencia"
        diag57.Notes = "Diagrama de Secuencia CUN04 (Modificar Turno): UI valida campos, TurnoBLL_DNI101 orquesta la lógica, valida estado previo en DAL, delega transición a TurnoBE_DNI101 (State Pattern), persiste en TurnoDAL_DNI101, audita en BitacoraBLL y recalcula en DigitoVerificadorBLL. Retornos sin texto (PDATA4='1')."
        diag57.Update()

        ea.Execute("DELETE FROM t_diagramlinks WHERE DiagramID = 57")
        ea.Execute("DELETE FROM t_connector WHERE DiagramID = 57")

        el_nutri57 = ea.GetElementByID(684)
        el_gui57 = ea.GetElementByID(755)
        el_turno_bll57 = ea.GetElementByID(687)
        el_turno_be57 = ea.GetElementByID(688)
        el_bit_bll57 = ea.GetElementByID(691)
        if el_bit_bll57.Name != "BitacoraBLL":
            el_bit_bll57.Name = "BitacoraBLL"
            el_bit_bll57.Update()
        el_dv_bll57 = ea.GetElementByID(690)
        el_dal57 = ea.GetElementByID(692)
        if el_dal57.Name != "DAL":
            el_dal57.Name = "DAL"
            el_dal57.Update()

        el_frag57 = ea.GetElementByID(743)

        layout57 = [
            (el_nutri57,     20,   120,  -50,  -980), # 70
            (el_gui57,       150,  250,  -50,  -980), # 200
            (el_turno_bll57, 290,  430,  -50,  -980), # 360
            (el_turno_be57,  490,  630,  -50,  -980), # 560
            (el_bit_bll57,   690,  830,  -50,  -980), # 760
            (el_dv_bll57,    890,  1030, -50,  -980), # 960
            (el_dal57,       1090, 1230, -50,  -980), # 1160
        ]

        for i in range(diag57.DiagramObjects.Count - 1, -1, -1):
            diag57.DiagramObjects.Delete(i)
        diag57.DiagramObjects.Refresh()

        coords57_x = {}
        for el, left, right, top, bottom in layout57:
            do = diag57.DiagramObjects.AddNew(f"l={left};r={right};t={abs(top)};b={abs(bottom)};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = top
            do.bottom = bottom
            do.Update()
            coords57_x[el.ElementID] = (left + right) // 2

        # Configure Fragment 743 (alt)
        do_frag57 = diag57.DiagramObjects.AddNew("l=10;r=1250;t=270;b=805;", "")
        do_frag57.ElementID = el_frag57.ElementID
        do_frag57.left = 10
        do_frag57.right = 1250
        do_frag57.top = -270
        do_frag57.bottom = -805
        do_frag57.Sequence = 1
        do_frag57.Update()

        guid1 = str(uuid.uuid4()).upper()
        guid2 = str(uuid.uuid4()).upper()
        part_desc57 = f"@PAR;Name=[Datos Validos y Transicion Permitida];Size=400;GUID={{{guid2}}};@ENDPAR;@PAR;Name=[Estado Invalido: Ya finalizado o cancelado];Size=135;GUID={{{guid1}}};@ENDPAR;"
        ea.Execute(f"DELETE FROM t_xref WHERE Client = '{el_frag57.ElementGUID}' AND Name = 'Partitions'")
        ea.Execute(f"INSERT INTO t_xref (XrefID, Name, Type, Visibility, Partition, Description, Client) VALUES ('{{{str(uuid.uuid4()).upper()}}}', 'Partitions', 'element property', 'Public', '0', '{part_desc57}', '{el_frag57.ElementGUID}')")

        diag57.DiagramObjects.Refresh()

        messages57 = [
            (1, el_nutri57, el_gui57, "btnGuardar_Click()", "SynchCall", -110, "Synchronous", "", "Call", ""),
            (2, el_gui57, el_gui57, "ValidarCamposObligatorios()", "SynchCall", -145, "Synchronous", "", "Call", ""),
            (3, el_gui57, el_turno_bll57, "ModificarTurno(codigoTurno, motivo, estado, fecha, hora)", "SynchCall", -180, "Synchronous", "", "Call", ""),
            (4, el_turno_bll57, el_dal57, "ObtenerPorCodigo(codigoTurno)", "SynchCall", -215, "Synchronous", "", "Call", ""),
            (5, el_dal57, el_turno_bll57, "", "Return", -245, "Synchronous", "", "Call", "1"),

            # ALT PARTITION 1: [Estado Invalido] (-270 a -405)
            (6, el_turno_bll57, el_gui57, "", "Return", -295, "Synchronous", "", "Call", "1"),
            (7, el_gui57, el_nutri57, "MostrarMensaje(msg_estado_no_permite_modificar)", "SynchCall", -330, "Synchronous", "", "Call", ""),
            (8, el_nutri57, el_gui57, "", "Return", -365, "Synchronous", "", "Call", "1"),

            # ALT PARTITION 2: [Datos Validos y Transicion Permitida] (-405 a -805)
            (9, el_turno_bll57, el_turno_be57, "ConfigurarEstadoPorNombre(nuevoEstado)", "SynchCall", -435, "Synchronous", "", "Call", ""),
            (10, el_turno_be57, el_turno_bll57, "", "Return", -465, "Synchronous", "", "Call", "1"),
            (11, el_turno_bll57, el_dal57, "ModificarTurno(idTurno, fecha, hora, motivo, estado, dv)", "SynchCall", -500, "Synchronous", "", "Call", ""),
            (12, el_dal57, el_turno_bll57, "", "Return", -530, "Synchronous", "", "Call", "1"),
            (13, el_turno_bll57, el_bit_bll57, "RegistrarBitacora(2, Turno modificado..., dniActual, TurneroNutricional)", "SynchCall", -565, "Synchronous", "", "Call", ""),
            (14, el_bit_bll57, el_dal57, "GuardarBitacora(bitacora)", "SynchCall", -600, "Synchronous", "", "Call", ""),
            (15, el_dal57, el_bit_bll57, "", "Return", -630, "Synchronous", "", "Call", "1"),
            (16, el_bit_bll57, el_turno_bll57, "", "Return", -655, "Synchronous", "", "Call", "1"),
            (17, el_turno_bll57, el_dv_bll57, "RecalcularYPersistir()", "SynchCall", -685, "Synchronous", "", "Call", ""),
            (18, el_dv_bll57, el_dal57, "PersistirIntegridadCompleta(resumen, filasDV)", "SynchCall", -715, "Synchronous", "", "Call", ""),
            (19, el_dal57, el_dv_bll57, "", "Return", -745, "Synchronous", "", "Call", "1"),
            (20, el_dv_bll57, el_turno_bll57, "", "Return", -770, "Synchronous", "", "Call", "1"),

            # Post-fragment
            (21, el_turno_bll57, el_gui57, "", "Return", -830, "Synchronous", "", "Call", "1"),
            (22, el_gui57, el_nutri57, "MostrarMensaje(msg_turno_modificado_ok)", "SynchCall", -865, "Synchronous", "", "Call", ""),
            (23, el_nutri57, el_gui57, "", "Return", -895, "Synchronous", "", "Call", "1"),
            (24, el_gui57, el_gui57, "Close()", "SynchCall", -925, "Synchronous", "", "Call", "")
        ]

        for item in messages57:
            seq, src, dst, msg_name, subtype, y, p1, p2, p3, p4 = item
            clean_name = msg_name.replace("'", "''")
            conn = src.Connectors.AddNew(clean_name, "Sequence")
            conn.SupplierID = dst.ElementID
            conn.SequenceNo = seq
            conn.SubType = subtype
            conn.DiagramID = 57
            conn.Update()
            src.Connectors.Refresh()

            sx = coords57_x.get(src.ElementID, 100)
            ex = coords57_x.get(dst.ElementID, 200)
            ey = y - 15 if src.ElementID == dst.ElementID else y
            if src.ElementID == dst.ElementID:
                ex = sx + 25

            p1_sql = p1.replace("'", "''")
            p2_sql = p2.replace("'", "''")
            p3_sql = p3.replace("'", "''")
            p4_sql = p4.replace("'", "''")

            ea.Execute(f"""
            UPDATE t_connector 
            SET PtStartX = {sx}, PtEndX = {ex}, PtStartY = {y}, PtEndY = {ey},
                PDATA1 = '{p1_sql}', PDATA2 = '{p2_sql}', PDATA3 = '{p3_sql}', PDATA4 = '{p4_sql}',
                RouteStyle = 1, LineColor = -1
            WHERE Connector_ID = {conn.ConnectorID}
            """)

        diag57.Update()
        diag57.DiagramObjects.Refresh()
        ea.SaveDiagram(57)
        export_and_copy(diag57, "CUN04 Modificar Turno", dss_dir)

        # --- 2.2 Class Diagram (Diagram ID 62) ---
        print("Rebuilding Diagram 62 (CUN04 Classes)...")
        diag62 = ea.GetDiagramByID(62)
        diag62.Name = "CUN04: Diagrama de Clases (BE, BLL y DAL)"
        diag62.Notes = "Diagrama de Clases CUN04 (Modificar Turno): Arquitectura de 3 capas. BLL usa BE y DAL. DAL desacoplada de BE."
        diag62.parentID = 39
        diag62.Update()

        c04_all = [el_turno_be, el_bloque_be, el_turno_bll, el_bit_bll, el_dv_bll, el_turno_dal, el_bit_dal, el_dv_dal]
        c04_ids = ",".join(str(e.ElementID) for e in c04_all)
        ea.Execute(f"DELETE FROM t_connector WHERE Start_Object_ID IN ({c04_ids}) AND End_Object_ID IN ({c04_ids})")
        for e in c04_all:
            e.Connectors.Refresh()

        add_use(el_turno_be, el_bloque_be, "+IdBloque_DNI101")
        add_use(el_turno_bll, el_turno_be)
        add_use(el_turno_bll, el_turno_dal)
        add_use(el_turno_bll, el_bit_bll)
        add_use(el_turno_bll, el_dv_bll)
        add_use(el_bit_bll, el_bit_dal)
        add_use(el_dv_bll, el_dv_dal)

        for i in range(diag62.DiagramObjects.Count - 1, -1, -1):
            diag62.DiagramObjects.Delete(i)
        diag62.DiagramObjects.Refresh()

        coords62 = [
            # Tier 1: BE
            (el_turno_be,   50,   40,  550,  320),
            (el_bloque_be,  620,  40,  920,  320),

            # Tier 2: BLL
            (el_turno_bll,  50,   400, 550,  660),
            (el_bit_bll,    620,  400, 1020, 660),
            (el_dv_bll,     1080, 400, 1620, 660),

            # Tier 3: DAL
            (el_turno_dal,  50,   740, 550,  1100),
            (el_bit_dal,    620,  740, 1020, 1100),
            (el_dv_dal,     1080, 740, 1620, 1100),
        ]

        for el, left, top, right, bottom in coords62:
            do = diag62.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag62.DiagramObjects.Refresh()
        diag62.Update()
        ea.SaveDiagram(62)
        ea.Execute("""
UPDATE t_diagram
SET ShowForeign = 0, ShowPackageContents = 0,
    StyleEx = 'ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TConnectorNotation=UML 2.1;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;',
    PDATA = 'HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=0;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;'
WHERE Diagram_ID = 62
        """)
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 62")
        export_and_copy(diag62, "CUN04 Modificar Turno", dc_dir)

        # --- 2.3 DER (Diagram ID 63) ---
        print("Rebuilding Diagram 63 (CUN04 DER)...")
        diag63 = ea.GetDiagramByID(63)
        diag63.Name = "CUN04: DER (Modelo Relacional)"
        diag63.Notes = "Diagrama Entidad-Relación CUN04 (Modificar Turno): Turnos_DNI101, BloquesHorarios_DNI101, Usuarios, Bitacora y DV con notación Information Engineering."
        diag63.parentID = 39
        diag63.Update()

        for i in range(diag63.DiagramObjects.Count - 1, -1, -1):
            diag63.DiagramObjects.Delete(i)
        diag63.DiagramObjects.Refresh()

        coords63 = [
            (el_tab_usr,    50,  40,  410, 350),
            (el_tab_turno,  490, 40,  830, 350),
            (el_tab_bloque, 490, 420, 830, 640),
            (el_tab_bit,    910, 40,  1250, 250),
            (el_tab_dv,     910, 330, 1250, 460),
        ]

        for el, left, top, right, bottom in coords63:
            do = diag63.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag63.DiagramObjects.Refresh()
        diag63.Update()
        ea.SaveDiagram(63)
        ea.Execute("""
UPDATE t_diagram
SET StyleEx = 'TConnectorNotation=Information Engineering;ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;',
    PDATA = 'HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=0;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;',
    ShowForeign = 0, ShowPackageContents = 0
WHERE Diagram_ID = 63
        """)
        ea.Execute("DELETE FROM t_diagramlinks WHERE DiagramID = 63")
        ea.Execute("INSERT INTO t_diagramlinks (DiagramID, ConnectorID, Style, Hidden) VALUES (63, 894, 'Mode=3;', 0)") # Usuarios -> Turnos
        ea.Execute("INSERT INTO t_diagramlinks (DiagramID, ConnectorID, Style, Hidden) VALUES (63, 895, 'Mode=3;', 0)") # Usuarios -> Bitacora
        export_and_copy(diag63, "CUN04 DER (Modelo Relacional)", der_dir)

        # =====================================================================
        # 3. CUN05: CANCELAR TURNO
        # =====================================================================
        print("\n================== CUN05: CANCELAR TURNO ==================")
        # --- 3.1 Sequence Diagram (Diagram ID 58) ---
        print("Rebuilding Diagram 58 (CUN05 Sequence)...")
        diag58 = ea.GetDiagramByID(58)
        diag58.Name = "CUN05: Diagrama de Secuencia"
        diag58.Notes = "Diagrama de Secuencia CUN05 (Cancelar Turno): UI solicita confirmación, TurnoBLL_DNI101 orquesta la cancelación, valida que no esté cancelado ni atendido, delega transición 'Cancelado' a TurnoBE_DNI101 (State Pattern), persiste y libera bloque en TurnoDAL_DNI101, audita en BitacoraBLL y recalcula en DigitoVerificadorBLL. Retornos sin texto (PDATA4='1')."
        diag58.Update()

        ea.Execute("DELETE FROM t_diagramlinks WHERE DiagramID = 58")
        ea.Execute("DELETE FROM t_connector WHERE DiagramID = 58")

        el_nutri58 = ea.GetElementByID(693)
        el_gui58 = ea.GetElementByID(768)
        el_turno_bll58 = ea.GetElementByID(695)
        el_turno_be58 = ea.GetElementByID(696)
        el_bit_bll58 = ea.GetElementByID(699)
        if el_bit_bll58.Name != "BitacoraBLL":
            el_bit_bll58.Name = "BitacoraBLL"
            el_bit_bll58.Update()
        el_dv_bll58 = ea.GetElementByID(698)
        el_dal58 = ea.GetElementByID(708)
        if el_dal58.Name != "DAL":
            el_dal58.Name = "DAL"
            el_dal58.Update()

        el_frag58 = ea.GetElementByID(747)

        layout58 = [
            (el_nutri58,     20,   120,  -50,  -980), # 70
            (el_gui58,       150,  250,  -50,  -980), # 200
            (el_turno_bll58, 290,  430,  -50,  -980), # 360
            (el_turno_be58,  490,  630,  -50,  -980), # 560
            (el_bit_bll58,   690,  830,  -50,  -980), # 760
            (el_dv_bll58,    890,  1030, -50,  -980), # 960
            (el_dal58,       1090, 1230, -50,  -980), # 1160
        ]

        for i in range(diag58.DiagramObjects.Count - 1, -1, -1):
            diag58.DiagramObjects.Delete(i)
        diag58.DiagramObjects.Refresh()

        coords58_x = {}
        for el, left, right, top, bottom in layout58:
            do = diag58.DiagramObjects.AddNew(f"l={left};r={right};t={abs(top)};b={abs(bottom)};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = top
            do.bottom = bottom
            do.Update()
            coords58_x[el.ElementID] = (left + right) // 2

        # Configure Fragment 747 (alt)
        do_frag58 = diag58.DiagramObjects.AddNew("l=10;r=1250;t=270;b=805;", "")
        do_frag58.ElementID = el_frag58.ElementID
        do_frag58.left = 10
        do_frag58.right = 1250
        do_frag58.top = -270
        do_frag58.bottom = -805
        do_frag58.Sequence = 1
        do_frag58.Update()

        guid1 = str(uuid.uuid4()).upper()
        guid2 = str(uuid.uuid4()).upper()
        part_desc58 = f"@PAR;Name=[Cancelacion Permitida];Size=400;GUID={{{guid2}}};@ENDPAR;@PAR;Name=[Estado Invalido: Ya cancelado o atendido];Size=135;GUID={{{guid1}}};@ENDPAR;"
        ea.Execute(f"DELETE FROM t_xref WHERE Client = '{el_frag58.ElementGUID}' AND Name = 'Partitions'")
        ea.Execute(f"INSERT INTO t_xref (XrefID, Name, Type, Visibility, Partition, Description, Client) VALUES ('{{{str(uuid.uuid4()).upper()}}}', 'Partitions', 'element property', 'Public', '0', '{part_desc58}', '{el_frag58.ElementGUID}')")

        diag58.DiagramObjects.Refresh()

        messages58 = [
            (1, el_nutri58, el_gui58, "btnCancelarTurno_Click()", "SynchCall", -110, "Synchronous", "", "Call", ""),
            (2, el_gui57, el_gui57, "ConfirmarCancelacion()", "SynchCall", -145, "Synchronous", "", "Call", ""),
            (3, el_gui58, el_turno_bll58, "CancelarTurno(codigoTurno, motivo)", "SynchCall", -180, "Synchronous", "", "Call", ""),
            (4, el_turno_bll58, el_dal58, "ObtenerPorCodigo(codigoTurno)", "SynchCall", -215, "Synchronous", "", "Call", ""),
            (5, el_dal58, el_turno_bll58, "", "Return", -245, "Synchronous", "", "Call", "1"),

            # ALT PARTITION 1: [Estado Invalido] (-270 a -405)
            (6, el_turno_bll58, el_gui58, "", "Return", -295, "Synchronous", "", "Call", "1"),
            (7, el_gui58, el_nutri58, "MostrarMensaje(msg_no_permite_cancelacion)", "SynchCall", -330, "Synchronous", "", "Call", ""),
            (8, el_nutri58, el_gui58, "", "Return", -365, "Synchronous", "", "Call", "1"),

            # ALT PARTITION 2: [Cancelacion Permitida] (-405 a -805)
            (9, el_turno_bll58, el_turno_be58, "Cancelar(motivo)", "SynchCall", -435, "Synchronous", "", "Call", ""),
            (10, el_turno_be58, el_turno_bll58, "", "Return", -465, "Synchronous", "", "Call", "1"),
            (11, el_turno_bll58, el_dal58, "CancelarTurnoConBloque(idTurno, motivo, idBloque, dv)", "SynchCall", -500, "Synchronous", "", "Call", ""),
            (12, el_dal58, el_turno_bll58, "", "Return", -530, "Synchronous", "", "Call", "1"),
            (13, el_turno_bll58, el_bit_bll58, "RegistrarBitacora(2, Turno cancelado..., dniActual, TurneroNutricional)", "SynchCall", -565, "Synchronous", "", "Call", ""),
            (14, el_bit_bll58, el_dal58, "GuardarBitacora(bitacora)", "SynchCall", -600, "Synchronous", "", "Call", ""),
            (15, el_dal58, el_bit_bll58, "", "Return", -630, "Synchronous", "", "Call", "1"),
            (16, el_bit_bll58, el_turno_bll58, "", "Return", -655, "Synchronous", "", "Call", "1"),
            (17, el_turno_bll58, el_dv_bll58, "RecalcularYPersistir()", "SynchCall", -685, "Synchronous", "", "Call", ""),
            (18, el_dv_bll58, el_dal58, "PersistirIntegridadCompleta(resumen, filasDV)", "SynchCall", -715, "Synchronous", "", "Call", ""),
            (19, el_dal58, el_dv_bll58, "", "Return", -745, "Synchronous", "", "Call", "1"),
            (20, el_dv_bll58, el_turno_bll58, "", "Return", -770, "Synchronous", "", "Call", "1"),

            # Post-fragment
            (21, el_turno_bll58, el_gui58, "", "Return", -830, "Synchronous", "", "Call", "1"),
            (22, el_gui58, el_nutri58, "MostrarMensaje(msg_turno_cancelado_ok)", "SynchCall", -865, "Synchronous", "", "Call", ""),
            (23, el_nutri58, el_gui58, "", "Return", -895, "Synchronous", "", "Call", "1"),
            (24, el_gui58, el_gui58, "ActualizarGrillaTurnos()", "SynchCall", -925, "Synchronous", "", "Call", "")
        ]

        for item in messages58:
            seq, src, dst, msg_name, subtype, y, p1, p2, p3, p4 = item
            clean_name = msg_name.replace("'", "''")
            conn = src.Connectors.AddNew(clean_name, "Sequence")
            conn.SupplierID = dst.ElementID
            conn.SequenceNo = seq
            conn.SubType = subtype
            conn.DiagramID = 58
            conn.Update()
            src.Connectors.Refresh()

            sx = coords58_x.get(src.ElementID, 100)
            ex = coords58_x.get(dst.ElementID, 200)
            ey = y - 15 if src.ElementID == dst.ElementID else y
            if src.ElementID == dst.ElementID:
                ex = sx + 25

            p1_sql = p1.replace("'", "''")
            p2_sql = p2.replace("'", "''")
            p3_sql = p3.replace("'", "''")
            p4_sql = p4.replace("'", "''")

            ea.Execute(f"""
            UPDATE t_connector 
            SET PtStartX = {sx}, PtEndX = {ex}, PtStartY = {y}, PtEndY = {ey},
                PDATA1 = '{p1_sql}', PDATA2 = '{p2_sql}', PDATA3 = '{p3_sql}', PDATA4 = '{p4_sql}',
                RouteStyle = 1, LineColor = -1
            WHERE Connector_ID = {conn.ConnectorID}
            """)

        diag58.Update()
        diag58.DiagramObjects.Refresh()
        ea.SaveDiagram(58)
        export_and_copy(diag58, "CUN05 Cancelar Turno", dss_dir)
        # Also copy to legacy filename CUN05Cancelar.png
        shutil.copy2(os.path.join(dss_dir, "CUN05 Cancelar Turno.png"), os.path.join(dss_dir, "CUN05Cancelar.png"))

        # --- 3.2 Class Diagram (Diagram ID 64) ---
        print("Rebuilding Diagram 64 (CUN05 Classes)...")
        diag64 = ea.GetDiagramByID(64)
        diag64.Name = "CUN05: Diagrama de Clases (BE, BLL y DAL)"
        diag64.Notes = "Diagrama de Clases CUN05 (Cancelar Turno): Arquitectura de 3 capas. BLL usa BE y DAL. DAL desacoplada de BE."
        diag64.parentID = 37
        diag64.Update()

        c05_all = [el_turno_be, el_bloque_be, el_turno_bll, el_bit_bll, el_dv_bll, el_turno_dal, el_bit_dal, el_dv_dal]
        c05_ids = ",".join(str(e.ElementID) for e in c05_all)
        ea.Execute(f"DELETE FROM t_connector WHERE Start_Object_ID IN ({c05_ids}) AND End_Object_ID IN ({c05_ids})")
        for e in c05_all:
            e.Connectors.Refresh()

        add_use(el_turno_be, el_bloque_be, "+IdBloque_DNI101")
        add_use(el_turno_bll, el_turno_be)
        add_use(el_turno_bll, el_turno_dal)
        add_use(el_turno_bll, el_bit_bll)
        add_use(el_turno_bll, el_dv_bll)
        add_use(el_bit_bll, el_bit_dal)
        add_use(el_dv_bll, el_dv_dal)

        for i in range(diag64.DiagramObjects.Count - 1, -1, -1):
            diag64.DiagramObjects.Delete(i)
        diag64.DiagramObjects.Refresh()

        coords64 = [
            # Tier 1: BE
            (el_turno_be,   50,   40,  550,  320),
            (el_bloque_be,  620,  40,  920,  320),

            # Tier 2: BLL
            (el_turno_bll,  50,   400, 550,  660),
            (el_bit_bll,    620,  400, 1020, 660),
            (el_dv_bll,     1080, 400, 1620, 660),

            # Tier 3: DAL
            (el_turno_dal,  50,   740, 550,  1100),
            (el_bit_dal,    620,  740, 1020, 1100),
            (el_dv_dal,     1080, 740, 1620, 1100),
        ]

        for el, left, top, right, bottom in coords64:
            do = diag64.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag64.DiagramObjects.Refresh()
        diag64.Update()
        ea.SaveDiagram(64)
        ea.Execute("""
UPDATE t_diagram
SET ShowForeign = 0, ShowPackageContents = 0,
    StyleEx = 'ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TConnectorNotation=UML 2.1;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;',
    PDATA = 'HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=0;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;'
WHERE Diagram_ID = 64
        """)
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 64")
        export_and_copy(diag64, "CUN05 Cancelar Turno", dc_dir)

        # --- 3.3 DER (Diagram ID 65) ---
        print("Rebuilding Diagram 65 (CUN05 DER)...")
        diag65 = ea.GetDiagramByID(65)
        diag65.Name = "CUN05: DER (Modelo Relacional)"
        diag65.Notes = "Diagrama Entidad-Relación CUN05 (Cancelar Turno): Turnos_DNI101, BloquesHorarios_DNI101, Usuarios, Bitacora y DV con notación Information Engineering."
        diag65.parentID = 37
        diag65.Update()

        for i in range(diag65.DiagramObjects.Count - 1, -1, -1):
            diag65.DiagramObjects.Delete(i)
        diag65.DiagramObjects.Refresh()

        coords65 = [
            (el_tab_usr,    50,  40,  410, 350),
            (el_tab_turno,  490, 40,  830, 350),
            (el_tab_bloque, 490, 420, 830, 640),
            (el_tab_bit,    910, 40,  1250, 250),
            (el_tab_dv,     910, 330, 1250, 460),
        ]

        for el, left, top, right, bottom in coords65:
            do = diag65.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag65.DiagramObjects.Refresh()
        diag65.Update()
        ea.SaveDiagram(65)
        ea.Execute("""
UPDATE t_diagram
SET StyleEx = 'TConnectorNotation=Information Engineering;ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;',
    PDATA = 'HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=0;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;',
    ShowForeign = 0, ShowPackageContents = 0
WHERE Diagram_ID = 65
        """)
        ea.Execute("DELETE FROM t_diagramlinks WHERE DiagramID = 65")
        ea.Execute("INSERT INTO t_diagramlinks (DiagramID, ConnectorID, Style, Hidden) VALUES (65, 894, 'Mode=3;', 0)") # Usuarios -> Turnos
        ea.Execute("INSERT INTO t_diagramlinks (DiagramID, ConnectorID, Style, Hidden) VALUES (65, 895, 'Mode=3;', 0)") # Usuarios -> Bitacora
        export_and_copy(diag65, "CUN05 DER (Modelo Relacional)", der_dir)

        # =====================================================================
        # 4. DIAGRAMA DE CLASES GENERAL DEL PN1 (Diagram ID 68)
        # =====================================================================
        print("\n================== DIAGRAMA DE CLASES GENERAL PN1 ==================")
        print("Rebuilding Diagram 68 (Proceso de Negocio 1 General)...")
        diag68 = ea.GetDiagramByID(68)
        diag68.Name = "Proceso de Negocio 1 General"
        diag68.Notes = "Diagrama de Clases General de Dominio del Proceso de Negocio 1 (PN1 - Turnero Pediátrico): Modelado de entidades del negocio (Paciente, Turno, Agenda, Bloque, Usuario) junto a la jerarquía del Patrón State (IEstadoTurno y estados concretos)."
        diag68.Update()

        el_pac_dom = ea.GetElementByID(417)
        el_tur_dom = ea.GetElementByID(418)
        el_blo_dom = ea.GetElementByID(425)
        el_age_dom = ea.GetElementByID(424)
        el_usr_dom = ea.GetElementByID(426)

        el_state_iface = ea.GetElementByID(419)
        el_state_sol = ea.GetElementByID(420)
        el_state_conf = ea.GetElementByID(421)
        el_state_asis = ea.GetElementByID(422)
        el_state_canc = ea.GetElementByID(423)

        for i in range(diag68.DiagramObjects.Count - 1, -1, -1):
            diag68.DiagramObjects.Delete(i)
        diag68.DiagramObjects.Refresh()

        # Layout for Diagram 68:
        # Row 1: Core Domain Entities
        # Row 2: State Pattern Interface
        # Row 3: Concrete State Classes
        coords68 = [
            # Row 1
            (el_pac_dom,     50,   40,  460,  360),
            (el_tur_dom,     530,  40,  1050, 420),
            (el_blo_dom,     1130, 40,  1470, 270),
            (el_age_dom,     1550, 40,  1950, 300),
            (el_usr_dom,     2030, 40,  2390, 340),

            # Row 2: State Interface
            (el_state_iface, 660,  500, 940,  620),

            # Row 3: 4 States
            (el_state_sol,   80,   700, 480,  820),
            (el_state_conf,  530,  700, 930,  820),
            (el_state_asis,  980,  700, 1380, 820),
            (el_state_canc,  1430, 700, 1830, 820),
        ]

        for el, left, top, right, bottom in coords68:
            do = diag68.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag68.DiagramObjects.Refresh()
        diag68.Update()
        ea.SaveDiagram(68)
        ea.Execute("""
UPDATE t_diagram
SET ShowForeign = 0, ShowPackageContents = 0,
    StyleEx = 'ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TConnectorNotation=UML 2.1;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;',
    PDATA = 'HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=0;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;'
WHERE Diagram_ID = 68
        """)

        # Add diagramlinks for Diagram 68
        ea.Execute("DELETE FROM t_diagramlinks WHERE DiagramID = 68")
        links68 = [877, 878, 879, 880, 881, 882, 883, 884, 885, 886]
        for cid in links68:
            ea.Execute(f"INSERT INTO t_diagramlinks (DiagramID, ConnectorID, Style, Hidden) VALUES (68, {cid}, 'Mode=3;', 0)")

        export_and_copy(diag68, "Proceso de Negocio 1 General", dc_dir)

        print("\n================== ALL 10 DIAGRAMS REBUILT AND EXPORTED SUCCESSFULLY ==================")

    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass
        print("EA session closed cleanly.")

if __name__ == '__main__':
    run()
