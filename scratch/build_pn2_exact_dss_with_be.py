import win32com.client
import os
import shutil

def build_pn2_exact_dss():
    eap_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP'
    dss_dir = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\DSS'
    scratch_dir = r'C:\Users\Danie\Desktop\GIT\TD\scratch'
    artifact_dir = r'C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e'

    print("Connecting to Enterprise Architect COM API...")
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    try:
        uc06 = ea.GetElementByID(42)  # CUN06: Seleccionar Paciente
        uc07 = ea.GetElementByID(46)  # CUN07: Graficar Diagnostico
        uc08 = ea.GetElementByID(674) # CUN08: Registrar Consulta
        uc09 = ea.GetElementByID(675) # CUN09: Prescribir Plan

        el_nutri_actor = ea.GetElementByID(266) # Nutricionista (Actor)

        def get_or_create_boundary(parent_el, name="GUI"):
            for el in parent_el.Elements:
                if el.Name == name and (el.Type == "Sequence" or el.Type == "Boundary"):
                    if el.Stereotype != "boundary":
                        el.Stereotype = "boundary"
                        el.Update()
                        ea.Execute(f"UPDATE t_object SET Stereotype = 'boundary' WHERE Object_ID = {el.ElementID}")
                    return el
            el = parent_el.Elements.AddNew(name, "Sequence")
            el.Stereotype = "boundary"
            el.Update()
            parent_el.Elements.Refresh()
            ea.Execute(f"UPDATE t_object SET Stereotype = 'boundary' WHERE Object_ID = {el.ElementID}")
            return el

        def get_or_create_child_object(parent_el, name, el_type="Object", stereo=""):
            for el in parent_el.Elements:
                if el.Name == name and el.Type == el_type:
                    return el
            el = parent_el.Elements.AddNew(name, el_type)
            if stereo:
                el.Stereotype = stereo
            el.Update()
            parent_el.Elements.Refresh()
            return el

        def clear_diagram_objects(diag):
            for i in range(diag.DiagramObjects.Count - 1, -1, -1):
                diag.DiagramObjects.Delete(i)
            diag.DiagramObjects.Refresh()
            diag.Update()

        def export_diagram(diag, out_files):
            ea.ReloadDiagram(diag.DiagramID)
            primary = out_files[0]
            proj.PutDiagramImageToFile(diag.DiagramGUID, primary, 1)
            for copy_dst in out_files[1:]:
                shutil.copy2(primary, copy_dst)
            print(f"Exported diagram '{diag.Name}' (ID {diag.DiagramID}) -> {primary}")

        def build_sequence_diagram(diag_id, diag_name, notes, lifelines_data, messages_data, out_pngs, lifeline_bottom=-1000):
            diag = ea.GetDiagramByID(diag_id)
            diag.Name = diag_name
            diag.Notes = notes
            diag.Update()

            ea.Execute(f"DELETE FROM t_connector WHERE DiagramID = {diag_id}")
            clear_diagram_objects(diag)

            centers = {}
            for idx, (el, left, right, cx) in enumerate(lifelines_data):
                centers[el.ElementID] = cx
                d_obj = diag.DiagramObjects.AddNew(f"l={left};r={right};t=-50;b={lifeline_bottom}", "")
                d_obj.ElementID = el.ElementID
                d_obj.Sequence = idx + 1
                d_obj.Update()

            diag.DiagramObjects.Refresh()
            diag.Update()

            for seq, s_el, r_el, m_name, stype, y, p1, p2, p3, p4 in messages_data:
                # Regla estricta: los mensajes de retorno deben estar completamente vacios
                if stype == "Return":
                    m_name = ""

                s_id = s_el.ElementID
                r_id = r_el.ElementID
                sx = centers[s_id]
                is_self = (s_id == r_id)
                if is_self:
                    ex = sx + 25
                    ey = y - 15
                else:
                    ex = centers[r_id]
                    ey = y

                conn = s_el.Connectors.AddNew(m_name, "Sequence")
                conn.SupplierID = r_id
                conn.SubType = stype
                conn.SequenceNo = seq
                conn.DiagramID = diag_id
                conn.Update()
                s_el.Connectors.Refresh()

                clean_name = m_name.replace("'", "''")
                ea.Execute(f"""
                UPDATE t_connector 
                SET DiagramID = {diag_id}, SeqNo = {seq}, Name = '{clean_name}',
                    PtStartX = {sx}, PtStartY = {y}, PtEndX = {ex}, PtEndY = {ey},
                    PDATA1 = '{p1}', PDATA2 = '{p2}', PDATA3 = '{p3}', PDATA4 = '{p4}',
                    RouteStyle = 1, LineColor = -1
                WHERE Connector_ID = {conn.ConnectorID}
                """)

            export_diagram(diag, out_pngs)

        # =====================================================================
        # 1. CUN06: SELECCIONAR PACIENTE (Diag ID 111) - SIN CUSTOM MESSAGEBOX
        # =====================================================================
        print("\n--- Updating DSS CUN06 (ID 111) with BE, no CustomMessageBox, and blank returns ---")
        el_gui_c06 = get_or_create_boundary(uc06, "GUI")
        el_obj_pac_bll6 = get_or_create_child_object(uc06, "PacienteBLL_DNI101", "Object")
        el_obj_dal6 = get_or_create_child_object(uc06, "DAL", "Object")
        el_obj_pac_be6 = get_or_create_child_object(uc06, "PacienteBE_DNI101", "Object")

        lifelines_c06 = [
            (el_nutri_actor,      20,   120,  70),
            (el_gui_c06,          160,  240,  200),
            (el_obj_pac_bll6,     300,  480,  390),
            (el_obj_dal6,         540,  680,  610),
            (el_obj_pac_be6,      740,  920,  830)
        ]

        messages_c06 = [
            # Nutricionista abre Seguimiento Nutricional
            (1, el_nutri_actor, el_gui_c06, "btnSeguimientoNutricional_Click()", "SynchCall", -90, "Synchronous", "", "Call", ""),
            (2, el_gui_c06, el_gui_c06, "CargarPacientes()", "SynchCall", -125, "Synchronous", "", "Call", ""),
            (3, el_gui_c06, el_obj_pac_bll6, "ListarPacientes()", "SynchCall", -160, "Synchronous", "", "Call", ""),
            (4, el_obj_pac_bll6, el_obj_dal6, "ListarTodos()", "SynchCall", -195, "Synchronous", "", "Call", ""),
            (5, el_obj_dal6, el_obj_pac_bll6, "", "Return", -230, "Synchronous", "", "Call", "1"),
            # Mapeo a entidad BE
            (6, el_obj_pac_bll6, el_obj_pac_be6, "new PacienteBE_DNI101(row)", "SynchCall", -265, "Synchronous", "", "Call", ""),
            (7, el_obj_pac_be6, el_obj_pac_bll6, "", "Return", -295, "Synchronous", "", "Call", "1"),
            (8, el_obj_pac_bll6, el_gui_c06, "", "Return", -330, "Synchronous", "", "Call", "1"),
            # Población de Grilla y cálculo en BE
            (9, el_gui_c06, el_gui_c06, "PoblarGrilla(_listaPacientes)", "SynchCall", -365, "Synchronous", "", "Call", ""),
            (10, el_gui_c06, el_obj_pac_be6, "p.ObtenerEdadMeses()", "SynchCall", -400, "Synchronous", "", "Call", ""),
            (11, el_obj_pac_be6, el_gui_c06, "", "Return", -430, "Synchronous", "", "Call", "1"),
            (12, el_gui_c06, el_gui_c06, "Rows.Add(...) & Rows[i].Tag = p", "SynchCall", -465, "Synchronous", "", "Call", ""),
            (13, el_gui_c06, el_nutri_actor, "", "Return", -500, "Synchronous", "", "Call", "1"),
            # Selección de paciente y despliegue de opciones clínicas
            (14, el_nutri_actor, el_gui_c06, "dgvPacientes_CellClick(e)", "SynchCall", -540, "Synchronous", "", "Call", ""),
            (15, el_gui_c06, el_gui_c06, "fila.Tag as PacienteBE_DNI101", "SynchCall", -575, "Synchronous", "", "Call", ""),
            (16, el_gui_c06, el_obj_pac_be6, "paciente.NombreCompleto, paciente.ObtenerEdadMeses()", "SynchCall", -610, "Synchronous", "", "Call", ""),
            (17, el_obj_pac_be6, el_gui_c06, "", "Return", -640, "Synchronous", "", "Call", "1"),
            (18, el_gui_c06, el_gui_c06, "MostrarOpciones('Graficar Diagnostico', 'Registrar Consulta', 'Prescribir Plan')", "SynchCall", -675, "Synchronous", "", "Call", ""),
            (19, el_gui_c06, el_nutri_actor, "", "Return", -710, "Synchronous", "", "Call", "1")
        ]

        build_sequence_diagram(
            111, "CUN06: Diagrama de Secuencia",
            "Diagrama de Secuencia CUN06 (Seleccionar Paciente): UI modelada como Boundary (GUI). Nutricionista accede a Seguimiento Nutricional, GUI dispara ListarPacientes hacia BLL -> DAL con 1 solo hit. BLL mapea DataRow a instancias PacienteBE_DNI101. GUI puebla grilla invocando ObtenerEdadMeses() en la BE y guardando la referencia en fila.Tag. Al hacer clic, GUI recupera la BE y despliega las opciones clinicas interactivas.",
            lifelines_c06, messages_c06,
            [
                os.path.join(dss_dir, "CUN06 Seleccionar Paciente.png"),
                os.path.join(scratch_dir, "cun06_secuencia.png"),
                os.path.join(artifact_dir, "cun06_secuencia.png")
            ],
            lifeline_bottom=-770
        )

        # =====================================================================
        # 2. CUN07: GRAFICAR DIAGNÓSTICO (Diag ID 112)
        # =====================================================================
        print("\n--- Updating DSS CUN07 (ID 112) with BE & blank return messages ---")
        el_gui_c07 = get_or_create_boundary(uc07, "GUI")
        el_obj_pac_be7 = get_or_create_child_object(uc07, "PacienteBE_DNI101", "Object")
        el_obj_cons_bll7 = get_or_create_child_object(uc07, "ConsultaNutricionalBLL_DNI101", "Object")
        el_obj_dal7 = get_or_create_child_object(uc07, "DAL", "Object")
        el_obj_cons_be7 = get_or_create_child_object(uc07, "ConsultaNutricionalBE_DNI101", "Object")
        el_obj_oms_data = get_or_create_child_object(uc07, "CurvasOMSData_DNI101", "Object")

        lifelines_c07 = [
            (el_nutri_actor,    20,   120,  70),
            (el_gui_c07,        160,  240,  200),
            (el_obj_pac_be7,    280,  440,  360),
            (el_obj_cons_bll7,  480,  680,  580),
            (el_obj_dal7,       720,  840,  780),
            (el_obj_cons_be7,   880, 1080,  980),
            (el_obj_oms_data,  1120, 1280, 1200)
        ]

        messages_c07 = [
            # Nutricionista abre graficador con paciente seleccionado
            (1, el_nutri_actor, el_gui_c07, "frmGraficarDiagnostico(paciente)", "SynchCall", -90, "Synchronous", "", "Call", ""),
            (2, el_gui_c07, el_gui_c07, "ConfigurarTextosPaciente()", "SynchCall", -125, "Synchronous", "", "Call", ""),
            (3, el_gui_c07, el_obj_pac_be7, "paciente.NombreCompleto, paciente.ObtenerEdadMeses()", "SynchCall", -160, "Synchronous", "", "Call", ""),
            (4, el_obj_pac_be7, el_gui_c07, "", "Return", -190, "Synchronous", "", "Call", "1"),
            # Carga de historial clínico
            (5, el_gui_c07, el_gui_c07, "CargarDatos()", "SynchCall", -225, "Synchronous", "", "Call", ""),
            (6, el_gui_c07, el_obj_cons_bll7, "ObtenerHistorialPorPaciente(paciente.IdPaciente_DNI101)", "SynchCall", -260, "Synchronous", "", "Call", ""),
            (7, el_obj_cons_bll7, el_obj_dal7, "ObtenerHistorialPorPaciente(idPaciente)", "SynchCall", -295, "Synchronous", "", "Call", ""),
            (8, el_obj_dal7, el_obj_cons_bll7, "", "Return", -330, "Synchronous", "", "Call", "1"),
            (9, el_obj_cons_bll7, el_obj_cons_be7, "new ConsultaNutricionalBE_DNI101(row, medicion, diagnostico, alertas)", "SynchCall", -365, "Synchronous", "", "Call", ""),
            (10, el_obj_cons_be7, el_obj_cons_bll7, "", "Return", -395, "Synchronous", "", "Call", "1"),
            (11, el_obj_cons_bll7, el_gui_c07, "", "Return", -430, "Synchronous", "", "Call", "1"),
            # Referencias percentilares OMS y mediciones reales
            (12, el_gui_c07, el_obj_oms_data, "ObtenerCurva(tipoIndicador, sexo)", "SynchCall", -465, "Synchronous", "", "Call", ""),
            (13, el_obj_oms_data, el_gui_c07, "", "Return", -495, "Synchronous", "", "Call", "1"),
            (14, el_gui_c07, el_obj_cons_be7, "c.Medicion (Peso, Talla, IMC), c.Diagnostico.ClasificacionOMS", "SynchCall", -530, "Synchronous", "", "Call", ""),
            (15, el_obj_cons_be7, el_gui_c07, "", "Return", -560, "Synchronous", "", "Call", "1"),
            (16, el_gui_c07, el_gui_c07, "ActualizarGrafico() & Plot.Add.ScatterLine(...)", "SynchCall", -595, "Synchronous", "", "Call", ""),
            (17, el_gui_c07, el_nutri_actor, "", "Return", -630, "Synchronous", "", "Call", "1")
        ]

        build_sequence_diagram(
            112, "CUN07: Diagrama de Secuencia",
            "Diagrama de Secuencia CUN07 (Graficar Diagnóstico): UI modelada como Boundary (GUI). Nutricionista selecciona graficar, GUI lee atributos de PacienteBE_DNI101, solicita historial a ConsultaNutricionalBLL -> DAL con 1 solo hit. BLL mapea DataTable a ConsultaNutricionalBE_DNI101 con mediciones y diagnóstico. GUI obtiene patrones de referencia OMS desde CurvasOMSData_DNI101 y grafica curvas percentilares y puntos de control en ScottPlot.",
            lifelines_c07, messages_c07,
            [
                os.path.join(dss_dir, "CUN07 Graficar Diagnostico.png"),
                os.path.join(scratch_dir, "cun07_secuencia.png"),
                os.path.join(artifact_dir, "cun07_secuencia.png")
            ],
            lifeline_bottom=-690
        )

        # =====================================================================
        # 3. CUN08: REGISTRAR CONSULTA (Diag ID 113)
        # =====================================================================
        print("\n--- Updating DSS CUN08 (ID 113) with BE & blank return messages ---")
        el_gui_c08 = get_or_create_boundary(uc08, "GUI")
        el_obj_pac_be8 = get_or_create_child_object(uc08, "PacienteBE_DNI101", "Object")
        el_obj_cons_bll8 = get_or_create_child_object(uc08, "ConsultaNutricionalBLL_DNI101", "Object")
        el_obj_strat_ctx = get_or_create_child_object(uc08, "EvaluadorNutricionalContext_DNI101", "Object")
        el_obj_strat = get_or_create_child_object(uc08, "IEvaluadorNutricionalStrategy_DNI101", "Object")
        el_obj_cons_be8 = get_or_create_child_object(uc08, "ConsultaNutricionalBE_DNI101", "Object")
        el_obj_bit8 = get_or_create_child_object(uc08, "BitacoraBLL", "Object")
        el_obj_dv8 = get_or_create_child_object(uc08, "DigitoVerificadorBLL", "Object")
        el_obj_dal8 = get_or_create_child_object(uc08, "DAL", "Object")

        lifelines_c08 = [
            (el_nutri_actor,    20,   120,  70),
            (el_gui_c08,        160,  240,  200),
            (el_obj_pac_be8,    280,  440,  360),
            (el_obj_cons_bll8,  480,  680,  580),
            (el_obj_strat_ctx,  720,  910,  815),
            (el_obj_strat,      950, 1160, 1055),
            (el_obj_cons_be8,  1200, 1400, 1300),
            (el_obj_bit8,      1440, 1560, 1500),
            (el_obj_dv8,       1600, 1740, 1670),
            (el_obj_dal8,      1780, 1900, 1840)
        ]

        messages_c08 = [
            # Inicialización y evaluación interactiva en vivo
            (1, el_nutri_actor, el_gui_c08, "frmRegistrarConsulta(paciente)", "SynchCall", -90, "Synchronous", "", "Call", ""),
            (2, el_gui_c08, el_obj_pac_be8, "paciente.NombreCompleto, paciente.ObtenerEdadMeses()", "SynchCall", -125, "Synchronous", "", "Call", ""),
            (3, el_obj_pac_be8, el_gui_c08, "", "Return", -155, "Synchronous", "", "Call", "1"),
            (4, el_nutri_actor, el_gui_c08, "txtMediciones_TextChanged(peso, talla)", "SynchCall", -190, "Synchronous", "", "Call", ""),
            (5, el_gui_c08, el_obj_cons_bll8, "CalcularIMC(peso, talla)", "SynchCall", -225, "Synchronous", "", "Call", ""),
            (6, el_obj_cons_bll8, el_gui_c08, "", "Return", -255, "Synchronous", "", "Call", "1"),
            (7, el_gui_c08, el_obj_cons_bll8, "EvaluarEstadoNutricional(imc, edadMeses, sexo)", "SynchCall", -290, "Synchronous", "", "Call", ""),
            (8, el_obj_cons_bll8, el_obj_strat_ctx, "EjecutarEvaluacion(imc, edadMeses, sexo)", "SynchCall", -325, "Synchronous", "", "Call", ""),
            (9, el_obj_strat_ctx, el_obj_strat, "Evaluar(imc, edadMeses, sexo)", "SynchCall", -360, "Synchronous", "", "Call", ""),
            (10, el_obj_strat, el_obj_strat_ctx, "", "Return", -390, "Synchronous", "", "Call", "1"),
            (11, el_obj_strat_ctx, el_obj_cons_bll8, "", "Return", -420, "Synchronous", "", "Call", "1"),
            (12, el_obj_cons_bll8, el_gui_c08, "", "Return", -450, "Synchronous", "", "Call", "1"),
            (13, el_gui_c08, el_nutri_actor, "", "Return", -480, "Synchronous", "", "Call", "1"),
            # Guardado y persistencia
            (14, el_nutri_actor, el_gui_c08, "btnGuardar_Click()", "SynchCall", -520, "Synchronous", "", "Call", ""),
            (15, el_gui_c08, el_gui_c08, "ValidarCamposObligatorios()", "SynchCall", -555, "Synchronous", "", "Call", ""),
            (16, el_gui_c08, el_obj_pac_be8, "ObtenerEdadMeses()", "SynchCall", -590, "Synchronous", "", "Call", ""),
            (17, el_obj_pac_be8, el_gui_c08, "", "Return", -620, "Synchronous", "", "Call", "1"),
            (18, el_gui_c08, el_obj_cons_bll8, "RegistrarConsulta(idPaciente, edad, sexo, peso, talla, pc, fecha, ...)", "SynchCall", -655, "Synchronous", "", "Call", ""),
            (19, el_obj_cons_bll8, el_obj_cons_bll8, "ValidarRangosBiologicos(peso, talla)", "SynchCall", -690, "Synchronous", "", "Call", ""),
            (20, el_obj_cons_bll8, el_obj_cons_be8, "new ConsultaNutricionalBE_DNI101(datos, medicion, diagnostico)", "SynchCall", -725, "Synchronous", "", "Call", ""),
            (21, el_obj_cons_be8, el_obj_cons_bll8, "", "Return", -755, "Synchronous", "", "Call", "1"),
            (22, el_obj_cons_bll8, el_obj_dal8, "GuardarConsultaCompleta(...) [1 DB Hit: Transaccion Atomica]", "SynchCall", -790, "Synchronous", "", "Call", ""),
            (23, el_obj_dal8, el_obj_cons_bll8, "", "Return", -820, "Synchronous", "", "Call", "1"),
            # Transversales: Bitácora y Dígito Verificador
            (24, el_obj_cons_bll8, el_obj_bit8, "RegistrarBitacora(2, 'CUN08: Registro de control...', dniNutri, 'SeguimientoNutricional')", "SynchCall", -855, "Synchronous", "", "Call", ""),
            (25, el_obj_bit8, el_obj_dal8, "GuardarBitacora(evento)", "SynchCall", -890, "Synchronous", "", "Call", ""),
            (26, el_obj_dal8, el_obj_bit8, "", "Return", -920, "Synchronous", "", "Call", "1"),
            (27, el_obj_bit8, el_obj_cons_bll8, "", "Return", -950, "Synchronous", "", "Call", "1"),
            (28, el_obj_cons_bll8, el_obj_dv8, "RecalcularYPersistir()", "SynchCall", -985, "Synchronous", "", "Call", ""),
            (29, el_obj_dv8, el_obj_dal8, "PersistirIntegridadCompleta(resumen, filasDV)", "SynchCall", -1020, "Synchronous", "", "Call", ""),
            (30, el_obj_dal8, el_obj_dv8, "", "Return", -1050, "Synchronous", "", "Call", "1"),
            (31, el_obj_dv8, el_obj_cons_bll8, "", "Return", -1080, "Synchronous", "", "Call", "1"),
            (32, el_obj_cons_bll8, el_gui_c08, "", "Return", -1110, "Synchronous", "", "Call", "1"),
            (33, el_gui_c08, el_gui_c08, "MessageBox.Show('Consulta registrada con exito')", "SynchCall", -1145, "Synchronous", "", "Call", ""),
            (34, el_gui_c08, el_gui_c08, "Close()", "SynchCall", -1180, "Synchronous", "", "Call", ""),
            (35, el_gui_c08, el_nutri_actor, "", "Return", -1215, "Synchronous", "", "Call", "1")
        ]

        build_sequence_diagram(
            113, "CUN08: Diagrama de Secuencia",
            "Diagrama de Secuencia CUN08 (Registrar Consulta): UI modelada como Boundary (GUI). Nutricionista ingresa mediciones con cálculo en vivo vía BLL y Strategy Pattern (OMS). Al presionar Guardar, GUI obtiene datos de PacienteBE_DNI101 y dispara hacia BLL. BLL valida rangos, instancia ConsultaNutricionalBE_DNI101 y ejecuta persistencia atómica en DAL con 1 único hit transaccional. Luego audita en BitacoraBLL hacia DAL y recalcula integridad en DigitoVerificadorBLL hacia DAL.",
            lifelines_c08, messages_c08,
            [
                os.path.join(dss_dir, "CUN08 Registrar Consulta.png"),
                os.path.join(scratch_dir, "cun08_secuencia.png"),
                os.path.join(artifact_dir, "cun08_secuencia.png")
            ],
            lifeline_bottom=-1280
        )

        # =====================================================================
        # 4. CUN09: PRESCRIBIR PLAN (Diag ID 114)
        # =====================================================================
        print("\n--- Updating DSS CUN09 (ID 114) with BE & blank return messages ---")
        el_gui_c09 = get_or_create_boundary(uc09, "GUI")
        el_obj_pac_be9 = get_or_create_child_object(uc09, "PacienteBE_DNI101", "Object")
        el_obj_cons_bll9 = get_or_create_child_object(uc09, "ConsultaNutricionalBLL_DNI101", "Object")
        el_obj_plan_bll9 = get_or_create_child_object(uc09, "PlanAlimentarioBLL_DNI101", "Object")
        el_obj_plan_be9 = get_or_create_child_object(uc09, "PlanAlimentarioBE_DNI101", "Object")
        el_obj_bit9 = get_or_create_child_object(uc09, "BitacoraBLL", "Object")
        el_obj_dv9 = get_or_create_child_object(uc09, "DigitoVerificadorBLL", "Object")
        el_obj_dal9 = get_or_create_child_object(uc09, "DAL", "Object")

        lifelines_c09 = [
            (el_nutri_actor,    20,   120,  70),
            (el_gui_c09,        160,  240,  200),
            (el_obj_pac_be9,    280,  440,  360),
            (el_obj_cons_bll9,  480,  680,  580),
            (el_obj_plan_bll9,  720,  920,  820),
            (el_obj_plan_be9,   960, 1150, 1055),
            (el_obj_bit9,      1190, 1310, 1250),
            (el_obj_dv9,       1350, 1490, 1420),
            (el_obj_dal9,      1530, 1650, 1590)
        ]

        messages_c09 = [
            # Inicialización y carga de contexto clínico
            (1, el_nutri_actor, el_gui_c09, "frmPrescribirPlan(paciente)", "SynchCall", -90, "Synchronous", "", "Call", ""),
            (2, el_gui_c09, el_obj_pac_be9, "paciente.NombreCompleto, paciente.ObtenerEdadMeses()", "SynchCall", -125, "Synchronous", "", "Call", ""),
            (3, el_obj_pac_be9, el_gui_c09, "", "Return", -155, "Synchronous", "", "Call", "1"),
            (4, el_gui_c09, el_obj_cons_bll9, "ObtenerUltimaConsultaPorPaciente(paciente.IdPaciente_DNI101)", "SynchCall", -190, "Synchronous", "", "Call", ""),
            (5, el_obj_cons_bll9, el_obj_dal9, "ObtenerUltimaConsultaPorPaciente(idPaciente)", "SynchCall", -225, "Synchronous", "", "Call", ""),
            (6, el_obj_dal9, el_obj_cons_bll9, "", "Return", -255, "Synchronous", "", "Call", "1"),
            (7, el_obj_cons_bll9, el_gui_c09, "", "Return", -285, "Synchronous", "", "Call", "1"),
            (8, el_gui_c09, el_obj_plan_bll9, "ObtenerUltimoPlanPorPaciente(paciente.IdPaciente_DNI101)", "SynchCall", -320, "Synchronous", "", "Call", ""),
            (9, el_obj_plan_bll9, el_obj_dal9, "ObtenerUltimoPlanPorPaciente(idPaciente)", "SynchCall", -355, "Synchronous", "", "Call", ""),
            (10, el_obj_dal9, el_obj_plan_bll9, "", "Return", -385, "Synchronous", "", "Call", "1"),
            (11, el_obj_plan_bll9, el_gui_c09, "", "Return", -415, "Synchronous", "", "Call", "1"),
            # Cálculo de macronutrientes interactivo
            (12, el_nutri_actor, el_gui_c09, "txtNutri_TextChanged(kcal, carbos, prote, grasas)", "SynchCall", -455, "Synchronous", "", "Call", ""),
            (13, el_gui_c09, el_gui_c09, "RecalcularGramos() & ValidarSuma100()", "SynchCall", -490, "Synchronous", "", "Call", ""),
            (14, el_gui_c09, el_nutri_actor, "", "Return", -520, "Synchronous", "", "Call", "1"),
            # Prescripción y persistencia
            (15, el_nutri_actor, el_gui_c09, "btnGuardar_Click()", "SynchCall", -560, "Synchronous", "", "Call", ""),
            (16, el_gui_c09, el_gui_c09, "ValidarDistribucion100(carbos, prote, grasas)", "SynchCall", -595, "Synchronous", "", "Call", ""),
            (17, el_gui_c09, el_obj_plan_bll9, "PrescribirPlan(idConsulta, kcal, carbos, prote, grasas, pautas, metas)", "SynchCall", -630, "Synchronous", "", "Call", ""),
            (18, el_obj_plan_bll9, el_obj_plan_bll9, "ValidarDistribucionCalorica()", "SynchCall", -665, "Synchronous", "", "Call", ""),
            (19, el_obj_plan_bll9, el_obj_plan_be9, "new PlanAlimentarioBE_DNI101(datos)", "SynchCall", -700, "Synchronous", "", "Call", ""),
            (20, el_obj_plan_be9, el_obj_plan_bll9, "", "Return", -730, "Synchronous", "", "Call", "1"),
            (21, el_obj_plan_bll9, el_obj_dal9, "GuardarPlanAlimentario(...) [1 DB Hit]", "SynchCall", -765, "Synchronous", "", "Call", ""),
            (22, el_obj_dal9, el_obj_plan_bll9, "", "Return", -795, "Synchronous", "", "Call", "1"),
            # Transversales: Bitácora y Dígito Verificador
            (23, el_obj_plan_bll9, el_obj_bit9, "RegistrarBitacora(2, 'CUN09: Plan Alimentario...', dniNutri, 'SeguimientoNutricional')", "SynchCall", -830, "Synchronous", "", "Call", ""),
            (24, el_obj_bit9, el_obj_dal9, "GuardarBitacora(evento)", "SynchCall", -865, "Synchronous", "", "Call", ""),
            (25, el_obj_dal9, el_obj_bit9, "", "Return", -895, "Synchronous", "", "Call", "1"),
            (26, el_obj_bit9, el_obj_plan_bll9, "", "Return", -925, "Synchronous", "", "Call", "1"),
            (27, el_obj_plan_bll9, el_obj_dv9, "RecalcularYPersistir()", "SynchCall", -960, "Synchronous", "", "Call", ""),
            (28, el_obj_dv9, el_obj_dal9, "PersistirIntegridadCompleta(resumen, filasDV)", "SynchCall", -995, "Synchronous", "", "Call", ""),
            (29, el_obj_dal9, el_obj_dv9, "", "Return", -1025, "Synchronous", "", "Call", "1"),
            (30, el_obj_dv9, el_obj_plan_bll9, "", "Return", -1055, "Synchronous", "", "Call", "1"),
            (31, el_obj_plan_bll9, el_gui_c09, "", "Return", -1085, "Synchronous", "", "Call", "1"),
            (32, el_gui_c09, el_gui_c09, "MessageBox.Show('Plan prescrito con exito')", "SynchCall", -1120, "Synchronous", "", "Call", ""),
            (33, el_gui_c09, el_gui_c09, "Close()", "SynchCall", -1155, "Synchronous", "", "Call", ""),
            (34, el_gui_c09, el_nutri_actor, "", "Return", -1190, "Synchronous", "", "Call", "1")
        ]

        build_sequence_diagram(
            114, "CUN09: Diagrama de Secuencia",
            "Diagrama de Secuencia CUN09 (Prescribir Plan): UI modelada como Boundary (GUI). Nutricionista abre prescripción, GUI lee atributos de PacienteBE_DNI101, consulta última atención en ConsultaNutricionalBLL y plan previo en PlanAlimentarioBLL. Validación de macronutrientes interactiva (100%), GUI dispara hacia PlanAlimentarioBLL, la cual valida, instancia PlanAlimentarioBE_DNI101 y persiste en DAL con 1 solo hit. Luego audita en BitacoraBLL hacia DAL y recalcula integridad en DigitoVerificadorBLL hacia DAL.",
            lifelines_c09, messages_c09,
            [
                os.path.join(dss_dir, "CUN09 Prescribir Plan.png"),
                os.path.join(scratch_dir, "cun09_secuencia.png"),
                os.path.join(artifact_dir, "cun09_secuencia.png")
            ],
            lifeline_bottom=-1250
        )

        print("\nAll 4 Sequence Diagrams (CUN06 - CUN09) rebuilt successfully without CustomMessageBox!")

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    build_pn2_exact_dss()
