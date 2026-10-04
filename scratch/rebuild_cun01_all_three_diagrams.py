import win32com.client
import os
import shutil

def run_rebuild():
    eap_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP'
    scratch_dir = r'c:\Users\Danie\Desktop\GIT\TD\scratch'
    artifact_dir = r'C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e'

    print("Connecting to Enterprise Architect...")
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    try:
        # =====================================================================
        # 1. DIAGRAM 12: CUN01 DIAGRAMA DE SECUENCIA
        # =====================================================================
        print("\n=== 1. UPDATING DIAGRAM 12 (CUN01: Diagrama de Secuencia) ===")
        diag12 = ea.GetDiagramByID(12)
        diag12.Name = "CUN01: Diagrama de Secuencia"
        diag12.Notes = "Diagrama de Secuencia de CUN01 (Registrar Turno) con arquitectura estricta: DAL retorna DataTable crudo a BLL, BLL instancia BEs mediante Dynamic Lifeline Creation, Is Return verificado (PDATA4='1') y DAL desacoplada de BE."
        diag12.Update()

        # Clean existing connectors for Diagram 12
        ea.Execute("DELETE FROM t_diagramlinks WHERE DiagramID = 12")
        ea.Execute("DELETE FROM t_connector WHERE DiagramID = 12")

        # Lifelines layout based on the approved professor reference (media_1791049745237.png)
        # Centers: Nutri=68, GUI=192, AgendaMedicaBLL=389, PacienteBLL=537, TurnoBLL=671, CUN02=710, EventoBLL=820, DV_BLL=969, TurnoBE=1097, DAL=1128, State=1233
        # To avoid horizontal overlaps between TurnoBE and DAL, we adjust X coordinates slightly so lines are crisp:
        lifelines_layout = [
            (219, "Nutricionista",              18,   118),   # Center: 68
            (754, "GUI",                        167,  217),   # Center: 192
            (221, "AgendaMedicaBLL_DNI101",     319,  459),   # Center: 389
            (222, "PacienteBLL_DNI101",         477,  597),   # Center: 537
            (38,  "CUN02: Registrar Paciente",  640,  780),   # Center: 710 (Dynamic Oval)
            (225, "TurnoBLL_DNI101",            611,  732),   # Center: 671
            (230, "BitacoraBLL",                  760,  880),   # Center: 820
            (229, "DigitoVerificadorBLL",       909,  1029),  # Center: 969
            (226, "TurnoBE_DNI101",             1037, 1157),  # Center: 1097 (Dynamic Box)
            (228, "DAL",                        1180, 1320),  # Center: 1250 (Clear of TurnoBE)
            (227, "TurnoSolicitadoState_DNI101",1350, 1490)   # Center: 1420 (Dynamic Box)
        ]

        el_map = {}
        coords_x = {}
        for el_id, el_name, left, right in lifelines_layout:
            el = ea.GetElementByID(el_id)
            if el.Name != el_name:
                el.Name = el_name
                el.Update()
            el_map[el_name] = el
            center_x = (left + right) // 2
            coords_x[el_id] = center_x
            ea.Execute(f"UPDATE t_diagramobjects SET RectLeft = {left}, RectRight = {right}, RectTop = -50, RectBottom = -2176 WHERE Diagram_ID = 12 AND Object_ID = {el_id}")
            el.Connectors.Refresh()

        nutri = el_map["Nutricionista"]
        gui = el_map["GUI"]
        agenda_bll = el_map["AgendaMedicaBLL_DNI101"]
        pac_bll = el_map["PacienteBLL_DNI101"]
        cun02 = el_map["CUN02: Registrar Paciente"]
        turno_bll = el_map["TurnoBLL_DNI101"]
        turno_be = el_map["TurnoBE_DNI101"]
        turno_state = el_map["TurnoSolicitadoState_DNI101"]
        bitacora_bll = el_map["BitacoraBLL"]
        dv_bll = el_map["DigitoVerificadorBLL"]
        dal = el_map["DAL"]

        # Fragments
        # 732: Identificacion de Paciente (alt)
        ea.Execute("UPDATE t_diagramobjects SET RectTop = -595, RectBottom = -910, RectLeft = 6, RectRight = 1500 WHERE Diagram_ID = 12 AND Object_ID = 732")
        part_desc_732 = "@PAR;Name=Paciente No Registrado [Punto de Extension CUN-02];Size=195;GUID={B584A820-20FD-4ba8-B15D-F637FC121C09};@ENDPAR;@PAR;Name=Paciente Registrado;Size=105;GUID={CFA9E348-3644-4f01-9E75-159A17873AB2};@ENDPAR;"
        el_732 = ea.GetElementByID(732)
        ea.Execute(f"UPDATE t_xref SET Description = '{part_desc_732}' WHERE Client = '{el_732.ElementGUID}' AND Name = 'Partitions'")

        # 728: Superposicion de Turno (alt)
        ea.Execute("UPDATE t_diagramobjects SET RectTop = -1085, RectBottom = -1145, RectLeft = 65, RectRight = 1500 WHERE Diagram_ID = 12 AND Object_ID = 728")

        # 734: Campos obligatorios (alt)
        ea.Execute("UPDATE t_diagramobjects SET RectTop = -1160, RectBottom = -1880, RectLeft = 47, RectRight = 1500 WHERE Diagram_ID = 12 AND Object_ID = 734")
        part_desc_734 = "@PAR;Name=Campos Obligatorios Cargados;Size=660;GUID={40503C30-96B6-4297-B4C1-D247B7205CB8};@ENDPAR;@PAR;Name=Campos Obligatorios No Cargados;Size=50;GUID={E8E05D84-17A7-4189-BA08-F5E1B4E22AB9};@ENDPAR;"
        el_734 = ea.GetElementByID(734)
        ea.Execute(f"UPDATE t_xref SET Description = '{part_desc_734}' WHERE Client = '{el_734.ElementGUID}' AND Name = 'Partitions'")

        # Messages definition
        # Fields: (seq, src, dst, msg_name, subtype, y, pdata1, pdata2, pdata3, pdata4)
        seq_messages = [
            # --- FASE 1: Consulta de Bloques y Disponibilidad ---
            (1, nutri, gui, "btn_Registrar_Turno()", "SynchCall", -151, "Synchronous", "", "Call", ""),
            (2, gui, gui, "ConsultarDisponibilidad()", "SynchCall", -175, "Synchronous", "", "Call", ""),
            (3, gui, agenda_bll, "ListarBloquesDisponibles(fecha, dniNutri)", "SynchCall", -205, "Synchronous", "", "Call", ""),
            (4, agenda_bll, dal, "ListarBloquesDisponibles(fecha, dniNutri)", "SynchCall", -245, "Synchronous", "", "Call", ""),
            (5, dal, agenda_bll, "", "Return", -280, "Synchronous", "", "Call", "1"),
            (6, agenda_bll, dal, "ObtenerHorariosOcupadosPorProfesional(dniNutri, fecha)", "SynchCall", -315, "Synchronous", "", "Call", ""),
            (7, dal, agenda_bll, "", "Return", -350, "Synchronous", "", "Call", "1"),
            (8, agenda_bll, gui, "", "Return", -385, "Synchronous", "", "Call", "1"),
            (9, gui, nutri, "", "Return", -420, "Synchronous", "", "Call", "1"),

            # --- FASE 2: Detección en txtDniNiño_Leave y Punto de Extensión CUN-02 ---
            (10, nutri, gui, "txtDniNiño_Leave(dniNiño)", "SynchCall", -460, "Synchronous", "", "Call", ""),
            (11, gui, pac_bll, "ObtenerOAsegurarPaciente(dniNiño, callbackUI)", "SynchCall", -495, "Synchronous", "", "Call", ""),
            (12, pac_bll, dal, "ObtenerPacientePorDNI(dniNiño)", "SynchCall", -535, "Synchronous", "", "Call", ""),
            (13, dal, pac_bll, "", "Return", -565, "Synchronous", "", "Call", "1"),

            # [alt Paciente No Registrado - CUN02 Dynamic Creation]
            (14, pac_bll, cun02, "RegistrarPaciente(dniNiño)", "New", -625, "Synchronous", "", "Call", ""),
            (15, nutri, cun02, "IngresarDatosPaciente(apellido, nombre, dni, tel, obraSocial, email)", "SynchCall", -665, "Synchronous", "", "Call", ""),
            (16, cun02, dal, "Guardar(nombre, apellido, dni, tel, obraSocial, email)", "SynchCall", -705, "Synchronous", "", "Call", ""),
            (17, dal, cun02, "", "Return", -740, "Synchronous", "", "Call", "1"),
            (18, cun02, pac_bll, "", "Return", -775, "Synchronous", "", "Call", "1"),

            # Paciente Registrado returns
            (19, pac_bll, gui, "", "Return", -845, "Synchronous", "", "Call", "1"),
            (20, gui, nutri, "", "Return", -885, "Synchronous", "", "Call", "1"),

            # --- FASE 3: Confirmación y Registro de Turno ---
            (21, nutri, gui, "btnRegistrarTurno_Click()", "SynchCall", -930, "Synchronous", "", "Call", ""),
            (22, gui, gui, "ValidarCamposObligatorios()", "SynchCall", -960, "Synchronous", "", "Call", ""),
            (23, gui, turno_bll, "RegistrarTurno(dniNiño, fecha, horario, motivo, idBloque, dniNutri)", "SynchCall", -995, "Synchronous", "", "Call", ""),
            (24, turno_bll, pac_bll, "ObtenerPacientePorDNI(dniNiño)", "SynchCall", -1030, "Synchronous", "", "Call", ""),
            (25, pac_bll, turno_bll, "", "Return", -1060, "Synchronous", "", "Call", "1"),
            (26, turno_bll, dal, "ExisteTurnoParaProfesional(dniNutri, fecha, hora)", "SynchCall", -1095, "Synchronous", "", "Call", ""),
            (27, dal, turno_bll, "", "Return", -1125, "Synchronous", "", "Call", "1"),

            # [alt Campos obligatorios cargados]
            # Dynamic creation of TurnoBE_DNI101 (arrow lands at -1175, creating the lifeline)
            (28, turno_bll, turno_be, "new TurnoBE_DNI101(dniNiño, fecha, horario, motivo)", "New", -1175, "Synchronous", "", "Call", ""),
            (29, turno_bll, turno_be, "GenerarCodigoUnico()", "SynchCall", -1210, "Synchronous", "", "Call", ""),
            # Dynamic creation of TurnoSolicitadoState_DNI101 (arrow lands at -1245, creating the lifeline)
            (30, turno_bll, turno_state, "new TurnoSolicitadoState_DNI101()", "New", -1245, "Synchronous", "", "Call", ""),
            (31, turno_bll, turno_be, "ConfigurarEstado(state)", "SynchCall", -1280, "Synchronous", "", "Call", ""),
            (32, turno_bll, turno_bll, "CalcularDV(cadenaDV)", "SynchCall", -1315, "Synchronous", "", "Call", ""),
            (33, turno_bll, dal, "RegistrarTurnoConBloque(codigoTurno, fecha, hora, motivo, estado, idPaciente, dniNutri, idBloque, dv)", "SynchCall", -1355, "Synchronous", "", "Call", ""),
            (34, dal, turno_bll, "", "Return", -1390, "Synchronous", "", "Call", "1"),
            (35, turno_bll, bitacora_bll, "RegistrarBitacora(1, Turno registrado..., dniNutri, TurneroNutricional)", "SynchCall", -1430, "Synchronous", "", "Call", ""),
            (36, bitacora_bll, dal, "GuardarBitacora(bitacora)", "SynchCall", -1465, "Synchronous", "", "Call", ""),
            (37, dal, bitacora_bll, "", "Return", -1500, "Synchronous", "", "Call", "1"),
            (38, bitacora_bll, turno_bll, "", "Return", -1530, "Synchronous", "", "Call", "1"),
            (39, turno_bll, dv_bll, "RecalcularYPersistir()", "SynchCall", -1565, "Synchronous", "", "Call", ""),
            (40, dv_bll, dal, "PersistirIntegridadCompleta(resumen, filasDV)", "SynchCall", -1605, "Synchronous", "", "Call", ""),
            (41, dal, dv_bll, "", "Return", -1640, "Synchronous", "", "Call", "1"),
            (42, dv_bll, turno_bll, "", "Return", -1670, "Synchronous", "", "Call", "1"),
            (43, turno_bll, gui, "", "Return", -1710, "Synchronous", "", "Call", "1"),
            (44, gui, nutri, "", "Return", -1750, "Synchronous", "", "Call", "1")
        ]

        print(f"Inserting {len(seq_messages)} connectors into Diagram 12...")
        for item in seq_messages:
            seq, src, dst, msg_name, subtype, y, p1, p2, p3, p4 = item
            clean_name = msg_name.replace("'", "''")
            conn = src.Connectors.AddNew(clean_name, "Sequence")
            conn.SupplierID = dst.ElementID
            conn.SequenceNo = seq
            conn.SubType = subtype
            conn.DiagramID = 12
            conn.Update()
            src.Connectors.Refresh()

            cid = conn.ConnectorID
            sx = coords_x.get(src.ElementID, 100)
            ex = coords_x.get(dst.ElementID, 200)

            if src.ElementID == dst.ElementID:
                ex = sx + 25
                ey = y - 15
            else:
                ey = y

            p1_sql = p1.replace("'", "''")
            p2_sql = p2.replace("'", "''")
            p3_sql = p3.replace("'", "''")
            p4_sql = p4.replace("'", "''")

            sql_update = f"""
            UPDATE t_connector 
            SET PtStartX = {sx}, PtEndX = {ex}, PtStartY = {y}, PtEndY = {ey},
                PDATA1 = '{p1_sql}', PDATA2 = '{p2_sql}', PDATA3 = '{p3_sql}', PDATA4 = '{p4_sql}',
                RouteStyle = 1, LineColor = -1
            WHERE Connector_ID = {cid}
            """
            ea.Execute(sql_update)

        diag12.Update()
        diag12.DiagramObjects.Refresh()
        ea.SaveDiagram(12)

        out_dss1 = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\DSS\CUN01 Registrar Turno.png'
        out_dss2 = os.path.join(scratch_dir, "cun01_sequence.png")
        out_dss3 = os.path.join(artifact_dir, "cun01_sequence.png")
        proj.PutDiagramImageToFile(diag12.DiagramGUID, out_dss1, 1)
        shutil.copy2(out_dss1, out_dss2)
        shutil.copy2(out_dss1, out_dss3)
        print("Diagram 12 exported successfully to:", out_dss1)

        # =====================================================================
        # 2. DIAGRAM 25: CUN01 DIAGRAMA DE CLASES (BLL, DAL, BE)
        # =====================================================================
        print("\n=== 2. UPDATING DIAGRAM 25 (CUN01: Diagrama de Clases) ===")
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

        # BE elements
        paciente_attrs = [
            ("IdPaciente_DNI101", "int"),
            ("DNINiño_DNI101", "string"),
            ("Nombre_DNI101", "string"),
            ("Apellido_DNI101", "string"),
            ("Telefono_DNI101", "string?"),
            ("Email_DNI101", "string?"),
            ("FechaNacimiento_DNI101", "DateTime"),
            ("Sexo_DNI101", "string?"),
            ("ObraSocial_DNI101", "string?"),
            ("NombreCompleto", "string"),
            ("DV", "string?")
        ]
        el_paciente_be = get_or_create_element("PacienteBE_DNI101", "Class")
        populate_attributes_only(el_paciente_be, paciente_attrs)

        turno_attrs = [
            ("IdTurno_DNI101", "int"),
            ("CodigoTurno_DNI101", "string"),
            ("FechaTurno_DNI101", "DateTime"),
            ("HoraTurno_DNI101", "TimeSpan"),
            ("MotivoConsulta_DNI101", "string"),
            ("EstadoTurno_DNI101", "string"),
            ("IdPaciente_DNI101", "int"),
            ("DniNutricionista_DNI101", "int"),
            ("IdBloque_DNI101", "int?"),
            ("Paciente_DNI101", "PacienteBE_DNI101"),
            ("DV", "string?")
        ]
        el_turno_be = get_or_create_element("TurnoBE_DNI101", "Class")
        populate_attributes_only(el_turno_be, turno_attrs)

        agenda_attrs = [
            ("IdAgenda_DNI101", "int"),
            ("Fecha_DNI101", "DateTime"),
            ("EstadoAgenda_DNI101", "string"),
            ("DniNutricionista_DNI101", "int"),
            ("BloquesHorarios_DNI101", "List<BloqueHorarioBE_DNI101>"),
            ("DV", "string?")
        ]
        el_agenda_be = get_or_create_element("AgendaMedicaBE_DNI101", "Class")
        populate_attributes_only(el_agenda_be, agenda_attrs)

        bloque_attrs = [
            ("IdBloque_DNI101", "int"),
            ("IdAgenda_DNI101", "int"),
            ("HoraInicio_DNI101", "TimeSpan"),
            ("HoraFin_DNI101", "TimeSpan"),
            ("EstadoBloque_DNI101", "string"),
            ("DV", "string?")
        ]
        el_bloque_be = get_or_create_element("BloqueHorarioBE_DNI101", "Class")
        populate_attributes_only(el_bloque_be, bloque_attrs)

        # BLL elements (instantiate BEs, return typed objects, orchestrate DAL)
        paciente_bll_methods = [
            ("ListarPacientes", "List<PacienteBE_DNI101>", []),
            ("ModificarPaciente", "bool", [("paciente", "PacienteBE_DNI101")]),
            ("ObtenerOAsegurarPaciente", "PacienteBE_DNI101", [("dniNiño", "string"), ("solicitarRegistroUI", "Func<string, bool>")]),
            ("ObtenerPacientePorDNI", "PacienteBE_DNI101", [("dniNiño", "string")]),
            ("ObtenerPorId", "PacienteBE_DNI101", [("idPaciente", "int")]),
            ("RegistrarPaciente", "int", [("nombre", "string"), ("apellido", "string"), ("dniNiño", "string"), ("telefono", "string?"), ("email", "string?"), ("obraSocial", "string?")])
        ]
        el_paciente_bll = get_or_create_element("PacienteBLL_DNI101", "Class")
        populate_methods_only(el_paciente_bll, paciente_bll_methods)

        agenda_bll_methods = [
            ("ActualizarEstadoBloque", "bool", [("idBloque", "int"), ("nuevoEstado", "string")]),
            ("ListarBloquesDisponibles", "List<BloqueHorarioBE_DNI101>", [("fecha", "DateTime"), ("dniNutricionista", "int")]),
            ("ObtenerAgendaMedica", "AgendaMedicaBE_DNI101", [("fecha", "DateTime"), ("dniNutricionista", "int")])
        ]
        el_agenda_bll = get_or_create_element("AgendaMedicaBLL_DNI101", "Class")
        populate_methods_only(el_agenda_bll, agenda_bll_methods)

        turno_bll_methods = [
            ("AgendarTurno", "bool", [("turno", "TurnoBE_DNI101")]),
            ("CancelarTurno", "bool", [("codigoTurno", "string"), ("motivo", "string")]),
            ("ListarTurnos", "List<TurnoBE_DNI101>", [("fecha", "DateTime?"), ("estado", "string?")]),
            ("ListarTurnosPorPaciente", "List<TurnoBE_DNI101>", [("idPaciente", "int")]),
            ("MarcarAsistencia", "bool", [("idTurno", "int")]),
            ("ModificarTurno", "bool", [("codigoTurno", "string"), ("motivo", "string"), ("estado", "string?"), ("fecha", "DateTime?"), ("hora", "TimeSpan?")]),
            ("ObtenerPorCodigo", "TurnoBE_DNI101", [("codigoTurno", "string")]),
            ("ObtenerPorId", "TurnoBE_DNI101", [("idTurno", "int")]),
            ("RegistrarTurno", "TurnoBE_DNI101", [("dniNiño", "string"), ("fecha", "DateTime"), ("horario", "string"), ("motivo", "string"), ("idBloque", "int?"), ("dniNutricionistaParam", "int?")]),
            ("ReprogramarTurno", "bool", [("codigoTurno", "string"), ("nuevaFecha", "DateTime"), ("nuevaHora", "TimeSpan"), ("nuevoIdBloque", "int?"), ("nuevoDniNutricionista", "int?")])
        ]
        el_turno_bll = get_or_create_element("TurnoBLL_DNI101", "Class")
        populate_methods_only(el_turno_bll, turno_bll_methods)

        # DAL elements: Pure Data Access / Table Data Gateway (NO BE parameters, NO BE returns)
        paciente_dal_methods = [
            ("Guardar", "int", [("dniNiño", "string"), ("nombre", "string"), ("apellido", "string"), ("tel", "string?"), ("email", "string?"), ("fechaNac", "DateTime"), ("sexo", "string?"), ("os", "string?"), ("dv", "string?")]),
            ("ListarTodos", "DataTable", []),
            ("Modificar", "bool", [("idPaciente", "int"), ("dniNiño", "string"), ("nombre", "string"), ("apellido", "string"), ("tel", "string?"), ("email", "string?"), ("fechaNac", "DateTime"), ("sexo", "string?"), ("os", "string?"), ("dv", "string?")]),
            ("ObtenerPacientePorDNI", "DataTable", [("dniNiño", "string")]),
            ("ObtenerPorId", "DataTable", [("idPaciente", "int")])
        ]
        el_paciente_dal = get_or_create_element("PacienteDAL_DNI101", "Class")
        populate_methods_only(el_paciente_dal, paciente_dal_methods)

        agenda_dal_methods = [
            ("ActualizarEstadoBloque", "bool", [("idBloque", "int"), ("nuevoEstado", "string")]),
            ("ListarBloquesDisponibles", "DataTable", [("fecha", "DateTime"), ("dniNutricionista", "int")])
        ]
        el_agenda_dal = get_or_create_element("AgendaMedicaDAL_DNI101", "Class")
        populate_methods_only(el_agenda_dal, agenda_dal_methods)

        turno_dal_methods = [
            ("ActualizarEstado", "bool", [("idTurno", "int"), ("nuevoEstado", "string")]),
            ("ActualizarEstadoBloque", "bool", [("idBloque", "int"), ("nuevoEstado", "string")]),
            ("CancelarTurno", "bool", [("idTurno", "int"), ("motivo", "string"), ("dv", "string?")]),
            ("CancelarTurnoConBloque", "bool", [("idTurno", "int"), ("motivo", "string"), ("idBloque", "int?"), ("dv", "string?")]),
            ("ExisteTurnoParaProfesional", "bool", [("dniNutricionista", "int"), ("fecha", "DateTime"), ("hora", "TimeSpan"), ("idTurnoExcluir", "int?")]),
            ("Guardar", "int", [("codigo", "string"), ("fecha", "DateTime"), ("hora", "TimeSpan"), ("motivo", "string"), ("estado", "string"), ("idPaciente", "int"), ("dniNutri", "int"), ("idBloque", "int?"), ("dv", "string?")]),
            ("ListarTodos", "DataTable", []),
            ("ListarTurnosPorFecha", "DataTable", [("fecha", "DateTime")]),
            ("ListarTurnosPorPaciente", "DataTable", [("idPaciente", "int")]),
            ("ModificarTurno", "bool", [("idTurno", "int"), ("fecha", "DateTime"), ("hora", "TimeSpan"), ("motivo", "string"), ("estado", "string"), ("dv", "string?")]),
            ("ObtenerHorariosOcupadosPorProfesional", "List<TimeSpan>", [("dniNutricionista", "int"), ("fecha", "DateTime")]),
            ("ObtenerPorCodigo", "DataTable", [("codigoTurno", "string")]),
            ("ObtenerPorId", "DataTable", [("idTurno", "int")]),
            ("RegistrarTurnoConBloque", "int", [("codigo", "string"), ("fecha", "DateTime"), ("hora", "TimeSpan"), ("motivo", "string"), ("estado", "string"), ("idPaciente", "int"), ("dniNutri", "int"), ("idBloque", "int?"), ("dv", "string?")]),
            ("ReprogramarTurno", "bool", [("idTurno", "int"), ("nuevaFecha", "DateTime"), ("nuevaHora", "TimeSpan"), ("nuevoIdBloque", "int?"), ("dniNutricionista", "int?"), ("dv", "string?")]),
            ("ReprogramarTurnoConBloques", "bool", [("idTurno", "int"), ("nuevaFecha", "DateTime"), ("nuevaHora", "TimeSpan"), ("nuevoIdBloque", "int?"), ("idBloqueAnterior", "int?"), ("dniNutricionista", "int?"), ("dv", "string?")])
        ]
        el_turno_dal = get_or_create_element("TurnoDAL_DNI101", "Class")
        populate_methods_only(el_turno_dal, turno_dal_methods)

        # Clear connectors between these elements
        all_c01_elements = [
            el_paciente_be, el_turno_be, el_bloque_be, el_agenda_be,
            el_paciente_bll, el_turno_bll, el_agenda_bll,
            el_paciente_dal, el_turno_dal, el_agenda_dal
        ]
        all_c01_ids = [e.ElementID for e in all_c01_elements]
        id_str = ",".join(str(i) for i in all_c01_ids)
        ea.Execute(f"DELETE FROM t_connector WHERE Start_Object_ID IN ({id_str}) AND End_Object_ID IN ({id_str})")
        for el in all_c01_elements:
            el.Connectors.Refresh()

        # Connectors for Diagram 25:
        # 1. BE Relationships:
        # PacienteBE (1) ◇---> (0..*) TurnoBE (Aggregation)
        c_agg = el_paciente_be.Connectors.AddNew("", "Aggregation")
        c_agg.SupplierID = el_turno_be.ElementID
        c_agg.Direction = "Source -> Destination"
        c_agg.ClientEnd.Cardinality = "1"
        c_agg.SupplierEnd.Cardinality = "0..*"
        c_agg.SupplierEnd.Role = "Turnos"
        c_agg.Update()
        el_paciente_be.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 1, DestIsAggregate = 0, SubType = 'Weak', Stereotype = null WHERE Connector_ID = {c_agg.ConnectorID}")

        # AgendaMedicaBE (1) ◆---> (1..*) BloqueHorarioBE (Composition)
        c_comp = el_agenda_be.Connectors.AddNew("", "Aggregation")
        c_comp.SupplierID = el_bloque_be.ElementID
        c_comp.Direction = "Source -> Destination"
        c_comp.ClientEnd.Cardinality = "1"
        c_comp.SupplierEnd.Cardinality = "1..*"
        c_comp.SupplierEnd.Role = "BloquesHorarios_DNI101"
        c_comp.Update()
        el_agenda_be.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 2, DestIsAggregate = 0, SubType = 'Strong', Stereotype = null WHERE Connector_ID = {c_comp.ConnectorID}")

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

        # TurnoBE uses BloqueHorarioBE
        add_use(el_turno_be, el_bloque_be, "+IdBloque_DNI101")

        # BLL to BE dependencies (BLL uses and instantiates BE)
        add_use(el_paciente_bll, el_paciente_be)
        add_use(el_turno_bll, el_turno_be)
        add_use(el_agenda_bll, el_bloque_be)
        add_use(el_agenda_bll, el_agenda_be)

        # BLL to BLL dependencies
        add_use(el_turno_bll, el_paciente_bll)
        add_use(el_turno_bll, el_agenda_bll)

        # BLL to DAL dependencies (BLL uses DAL)
        add_use(el_paciente_bll, el_paciente_dal)
        add_use(el_turno_bll, el_turno_dal)
        add_use(el_agenda_bll, el_agenda_dal)

        # CRITICAL ARCHITECTURE RULE:
        # NO ARROWS FROM DAL TO BE! (DAL does NOT use BE)

        # Layout Diagram 25:
        # 3-Tier Layered Architecture with ample width to prevent any clipping:
        diag25 = ea.GetDiagramByID(25)
        diag25.Name = "CUN01: Diagrama de Clases (BE, BLL y DAL)"
        diag25.Notes = "Diagrama de Clases CUN01 (Registrar Turno): BLL usa DAL y BE; DAL no tiene acoplamiento ni dependencias hacia BE."
        diag25.Update()

        for i in range(diag25.DiagramObjects.Count - 1, -1, -1):
            diag25.DiagramObjects.Delete(i)
        diag25.DiagramObjects.Refresh()

        coords25 = [
            # Tier 1: BE (Top: 40, Bottom: 320)
            (el_paciente_be,  50,   40,  460,  320),
            (el_turno_be,     520,  40,  1080, 320),
            (el_bloque_be,    1140, 40,  1440, 320),
            (el_agenda_be,    1500, 40,  1940, 320),

            # Tier 2: BLL (Top: 400, Bottom: 660)
            (el_paciente_bll, 50,   400, 460,  660),
            (el_turno_bll,    520,  400, 1440, 660),
            (el_agenda_bll,   1500, 400, 1940, 660),

            # Tier 3: DAL (Top: 740, Bottom: 1100)
            (el_paciente_dal, 50,   740, 460,  1100),
            (el_turno_dal,    520,  740, 1440, 1100),
            (el_agenda_dal,   1500, 740, 1940, 1100)
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
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 25")

        out_dc1 = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Clases\CU01 Registrar Turno.png'
        out_dc2 = os.path.join(scratch_dir, "cun01_clases_bll_dal_be.png")
        out_dc3 = os.path.join(artifact_dir, "cun01_clases_bll_dal_be.png")
        proj.PutDiagramImageToFile(diag25.DiagramGUID, out_dc1, 1)
        shutil.copy2(out_dc1, out_dc2)
        shutil.copy2(out_dc1, out_dc3)
        print("Diagram 25 exported successfully to:", out_dc1)

        # =====================================================================
        # 3. DIAGRAM 26: CUN01 DER (MODELO RELACIONAL)
        # =====================================================================
        print("\n=== 3. UPDATING DIAGRAM 26 (CUN01: DER Modelo Relacional) ===")
        diag26 = ea.GetDiagramByID(26)
        diag26.Name = "CUN01: DER (Modelo Relacional)"
        diag26.Notes = "Diagrama Entidad-Relación (DER) CUN01: Turnos_DNI101, Pacientes_DNI101, AgendasMedicas_DNI101, BloquesHorarios_DNI101, Usuarios, Bitacora y DV con notación Information Engineering (patas de gallo)."
        diag26.Update()

        for i in range(diag26.DiagramObjects.Count - 1, -1, -1):
            diag26.DiagramObjects.Delete(i)
        diag26.DiagramObjects.Refresh()

        el_paciente_tab = ea.GetElementByID(430)
        el_usuario_tab = ea.GetElementByID(431)
        el_agenda_tab = ea.GetElementByID(432)
        el_bloque_tab = ea.GetElementByID(433)
        el_turno_tab = ea.GetElementByID(434)
        el_dv_tab = ea.GetElementByID(435)
        el_bitacora_tab = ea.GetElementByID(436)

        # Remove ID_Perfil from Usuarios if present, matching the approved DER reference exactly
        for i in range(el_usuario_tab.Attributes.Count - 1, -1, -1):
            if el_usuario_tab.Attributes[i].Name == "ID_Perfil":
                el_usuario_tab.Attributes.Delete(i)
        el_usuario_tab.Attributes.Refresh()
        el_usuario_tab.Update()

        coords26 = [
            (el_paciente_tab, 30,  40,  350, 350),
            (el_dv_tab,       30,  420, 350, 540),
            (el_bitacora_tab, 440, 40,  760, 240),
            (el_usuario_tab,  440, 320, 760, 620),
            (el_turno_tab,    440, 700, 760, 1010),
            (el_agenda_tab,   850, 40,  1170, 240),
            (el_bloque_tab,   850, 330, 1170, 550)
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

        out_der1 = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Entidad Relacion\CU01 DER (Modelo Relacional).png'
        out_der2 = os.path.join(scratch_dir, "cun01_der.png")
        out_der3 = os.path.join(artifact_dir, "cun01_der.png")
        proj.PutDiagramImageToFile(diag26.DiagramGUID, out_der1, 1)
        shutil.copy2(out_der1, out_der2)
        shutil.copy2(out_der1, out_der3)
        print("Diagram 26 exported successfully to:", out_der1)

        print("\nAll 3 CUN01 diagrams (Sequence, Class Diagram, DER) rebuilt and exported successfully!")

    finally:
        ea.CloseFile()
        ea.Exit()
        print("EA session closed cleanly.")

if __name__ == '__main__':
    run_rebuild()
