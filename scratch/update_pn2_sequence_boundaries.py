import win32com.client
import os
import shutil

def polish_and_clean_pn2():
    eap_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP'
    dss_dir = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\DSS'
    scratch_dir = r'C:\Users\Danie\Desktop\GIT\TD\scratch'
    artifact_dir = r'C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e'

    print("Connecting to Enterprise Architect...")
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    try:
        pkg4 = ea.GetPackageByID(4)

        uc06 = ea.GetElementByID(42)  # CUN06
        uc07 = ea.GetElementByID(46)  # CUN07
        uc08 = ea.GetElementByID(674) # CUN08
        uc09 = ea.GetElementByID(675) # CUN09

        el_nutri_actor = ea.GetElementByID(266)

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
        # 1. CUN06: SELECCIONAR PACIENTE (Diag ID 111)
        # =====================================================================
        print("\n--- Updating DSS CUN06 (ID 111) with generic DAL ---")
        el_gui_c06 = get_or_create_boundary(uc06, "GUI")
        el_obj_pac_bll = get_or_create_child_object(uc06, "PacienteBLL_DNI101", "Object")
        el_obj_dal6 = get_or_create_child_object(uc06, "DAL", "Object")

        lifelines_c06 = [
            (el_nutri_actor,      20,   120,  70),
            (el_gui_c06,          160,  240,  200),
            (el_obj_pac_bll,      290,  450,  370),
            (el_obj_dal6,         500,  640,  570)
        ]

        messages_c06 = [
            (1, el_nutri_actor, el_gui_c06, "btnSeguimientoNutricional_Click()", "SynchCall", -90, "Synchronous", "", "Call", ""),
            (2, el_gui_c06, el_gui_c06, "CargarPacientes()", "SynchCall", -130, "Synchronous", "", "Call", ""),
            (3, el_gui_c06, el_obj_pac_bll, "ListarPacientes()", "SynchCall", -170, "Synchronous", "", "Call", ""),
            (4, el_obj_pac_bll, el_obj_dal6, "ListarTodos()", "SynchCall", -210, "Synchronous", "", "Call", ""),
            (5, el_obj_dal6, el_obj_pac_bll, "", "Return", -250, "Synchronous", "", "Call", "1"),
            (6, el_obj_pac_bll, el_gui_c06, "", "Return", -290, "Synchronous", "", "Call", "1"),
            (7, el_gui_c06, el_nutri_actor, "", "Return", -330, "Synchronous", "", "Call", "1"),
            (8, el_nutri_actor, el_gui_c06, "dgvPacientes_CellClick(paciente)", "SynchCall", -370, "Synchronous", "", "Call", ""),
            (9, el_gui_c06, el_gui_c06, "MostrarOpciones('Graficar Diagnostico', 'Registrar Consulta', 'Prescribir Plan')", "SynchCall", -410, "Synchronous", "", "Call", ""),
            (10, el_gui_c06, el_nutri_actor, "", "Return", -450, "Synchronous", "", "Call", "1")
        ]

        build_sequence_diagram(
            111, "CUN06: Diagrama de Secuencia",
            "Diagrama de Secuencia CUN06 (Seleccionar Paciente): UI modelada como Boundary (GUI). Nutricionista accede a Seguimiento Nutricional, GUI dispara ListarPacientes hacia BLL -> DAL genérica (1 solo DB hit). Al seleccionar paciente, GUI despliega opciones interactivas.",
            lifelines_c06, messages_c06,
            [
                os.path.join(dss_dir, "CUN06 Seleccionar Paciente.png"),
                os.path.join(scratch_dir, "cun06_secuencia.png"),
                os.path.join(artifact_dir, "cun06_secuencia.png")
            ],
            lifeline_bottom=-510
        )

        # =====================================================================
        # 2. CUN07: GRAFICAR DIAGNÓSTICO (Diag ID 112)
        # =====================================================================
        print("\n--- Updating DSS CUN07 (ID 112) with generic DAL ---")
        el_gui_c07 = get_or_create_boundary(uc07, "GUI")
        el_obj_cons_bll7 = get_or_create_child_object(uc07, "ConsultaNutricionalBLL_DNI101", "Object")
        el_obj_oms_data = get_or_create_child_object(uc07, "CurvasOMSData_DNI101", "Object")
        el_obj_dal7 = get_or_create_child_object(uc07, "DAL", "Object")

        lifelines_c07 = [
            (el_nutri_actor,    20,   120,  70),
            (el_gui_c07,        160,  240,  200),
            (el_obj_cons_bll7,  290,  490,  390),
            (el_obj_oms_data,   540,  710,  625),
            (el_obj_dal7,       760,  900,  830)
        ]

        messages_c07 = [
            (1, el_nutri_actor, el_gui_c07, "btnGraficarDiagnostico_Click(paciente)", "SynchCall", -90, "Synchronous", "", "Call", ""),
            (2, el_gui_c07, el_gui_c07, "InicializarGraficoScottPlot()", "SynchCall", -130, "Synchronous", "", "Call", ""),
            (3, el_gui_c07, el_obj_cons_bll7, "ObtenerHistorialCompleto(idPaciente)", "SynchCall", -170, "Synchronous", "", "Call", ""),
            (4, el_obj_cons_bll7, el_obj_dal7, "ObtenerHistorialConsultasPorPaciente(idPaciente)", "SynchCall", -210, "Synchronous", "", "Call", ""),
            (5, el_obj_dal7, el_obj_cons_bll7, "", "Return", -250, "Synchronous", "", "Call", "1"),
            (6, el_obj_cons_bll7, el_gui_c07, "", "Return", -290, "Synchronous", "", "Call", "1"),
            (7, el_gui_c07, el_obj_oms_data, "ObtenerCurva(tipoCurva, sexo)", "SynchCall", -330, "Synchronous", "", "Call", ""),
            (8, el_obj_oms_data, el_gui_c07, "", "Return", -370, "Synchronous", "", "Call", "1"),
            (9, el_gui_c07, el_gui_c07, "GraficarCurvasYConsultas(curvasOMS, historialPaciente)", "SynchCall", -410, "Synchronous", "", "Call", ""),
            (10, el_gui_c07, el_nutri_actor, "", "Return", -450, "Synchronous", "", "Call", "1")
        ]

        build_sequence_diagram(
            112, "CUN07: Diagrama de Secuencia",
            "Diagrama de Secuencia CUN07 (Graficar Diagnóstico): UI modelada como Boundary (GUI). Nutricionista selecciona graficar, GUI solicita historial a BLL -> DAL genérica (1 único hit), obtiene patrones de referencia OMS y grafica curvas ScottPlot de alta fidelidad.",
            lifelines_c07, messages_c07,
            [
                os.path.join(dss_dir, "CUN07 Graficar Diagnostico.png"),
                os.path.join(scratch_dir, "cun07_secuencia.png"),
                os.path.join(artifact_dir, "cun07_secuencia.png")
            ],
            lifeline_bottom=-510
        )

        # =====================================================================
        # 3. CUN08: REGISTRAR CONSULTA (Diag ID 113)
        # =====================================================================
        print("\n--- Updating DSS CUN08 (ID 113) with generic DAL ---")
        el_gui_c08 = get_or_create_boundary(uc08, "GUI")
        el_obj_cons_bll8 = get_or_create_child_object(uc08, "ConsultaNutricionalBLL_DNI101", "Object")
        el_obj_strat_ctx = get_or_create_child_object(uc08, "EvaluadorNutricionalContext_DNI101", "Object")
        el_obj_strat = get_or_create_child_object(uc08, "IEvaluadorNutricionalStrategy_DNI101", "Object")
        el_obj_bit8 = get_or_create_child_object(uc08, "BitacoraBLL", "Object")
        el_obj_dv8 = get_or_create_child_object(uc08, "DigitoVerificadorBLL", "Object")
        el_obj_dal8 = get_or_create_child_object(uc08, "DAL", "Object")

        lifelines_c08 = [
            (el_nutri_actor,    20,   120,  70),
            (el_gui_c08,        160,  240,  200),
            (el_obj_cons_bll8,  290,  490,  390),
            (el_obj_strat_ctx,  530,  730,  630),
            (el_obj_strat,      770,  990,  880),
            (el_obj_bit8,       1030, 1160, 1095),
            (el_obj_dv8,        1200, 1350, 1275),
            (el_obj_dal8,       1390, 1510, 1450)
        ]

        messages_c08 = [
            # Evaluación en vivo durante carga
            (1, el_nutri_actor, el_gui_c08, "txtMediciones_TextChanged(peso, talla, pc)", "SynchCall", -90, "Synchronous", "", "Call", ""),
            (2, el_gui_c08, el_obj_cons_bll8, "CalcularIMC(peso, talla)", "SynchCall", -125, "Synchronous", "", "Call", ""),
            (3, el_obj_cons_bll8, el_gui_c08, "", "Return", -155, "Synchronous", "", "Call", "1"),
            (4, el_gui_c08, el_obj_strat_ctx, "EjecutarEvaluacion(edadMeses, sexo, peso, talla, imc)", "SynchCall", -190, "Synchronous", "", "Call", ""),
            (5, el_obj_strat_ctx, el_obj_strat, "Evaluar(edadMeses, sexo, peso, talla, imc)", "SynchCall", -225, "Synchronous", "", "Call", ""),
            (6, el_obj_strat, el_obj_strat_ctx, "", "Return", -255, "Synchronous", "", "Call", "1"),
            (7, el_obj_strat_ctx, el_gui_c08, "", "Return", -285, "Synchronous", "", "Call", "1"),
            (8, el_gui_c08, el_nutri_actor, "", "Return", -315, "Synchronous", "", "Call", "1"),

            # Guardado y persistencia atómica en DAL genérica
            (9, el_nutri_actor, el_gui_c08, "btnGuardarConsulta_Click()", "SynchCall", -355, "Synchronous", "", "Call", ""),
            (10, el_gui_c08, el_gui_c08, "ValidarCamposObligatorios()", "SynchCall", -390, "Synchronous", "", "Call", ""),
            (11, el_gui_c08, el_obj_cons_bll8, "RegistrarConsultaCompleta(idPaciente, dniNutri, edad, peso, talla, pc, ...)", "SynchCall", -425, "Synchronous", "", "Call", ""),
            (12, el_obj_cons_bll8, el_obj_cons_bll8, "ValidarRangosAntropometricos()", "SynchCall", -460, "Synchronous", "", "Call", ""),
            (13, el_obj_cons_bll8, el_obj_cons_bll8, "CalcularDVConsulta()", "SynchCall", -495, "Synchronous", "", "Call", ""),
            (14, el_obj_cons_bll8, el_obj_dal8, "GuardarConsultaCompleta(...) [1 DB Hit: Transacción Atómica]", "SynchCall", -530, "Synchronous", "", "Call", ""),
            (15, el_obj_dal8, el_obj_cons_bll8, "", "Return", -565, "Synchronous", "", "Call", "1"),
            (16, el_obj_cons_bll8, el_obj_bit8, "RegistrarEvento(1, 'Registro Consulta Nutricional...', dniNutri, 'SeguimientoNutricional')", "SynchCall", -600, "Synchronous", "", "Call", ""),
            (17, el_obj_bit8, el_obj_dal8, "GuardarBitacora(bitacora)", "SynchCall", -635, "Synchronous", "", "Call", ""),
            (18, el_obj_dal8, el_obj_bit8, "", "Return", -665, "Synchronous", "", "Call", "1"),
            (19, el_obj_bit8, el_obj_cons_bll8, "", "Return", -695, "Synchronous", "", "Call", "1"),
            (20, el_obj_cons_bll8, el_obj_dv8, "RecalcularYPersistir()", "SynchCall", -730, "Synchronous", "", "Call", ""),
            (21, el_obj_dv8, el_obj_dal8, "PersistirIntegridadCompleta(resumen, filasDV)", "SynchCall", -765, "Synchronous", "", "Call", ""),
            (22, el_obj_dal8, el_obj_dv8, "", "Return", -795, "Synchronous", "", "Call", "1"),
            (23, el_obj_dv8, el_obj_cons_bll8, "", "Return", -825, "Synchronous", "", "Call", "1"),
            (24, el_obj_cons_bll8, el_gui_c08, "", "Return", -855, "Synchronous", "", "Call", "1"),
            (25, el_gui_c08, el_gui_c08, "Close()", "SynchCall", -885, "Synchronous", "", "Call", ""),
            (26, el_gui_c08, el_nutri_actor, "", "Return", -915, "Synchronous", "", "Call", "1")
        ]

        build_sequence_diagram(
            113, "CUN08: Diagrama de Secuencia",
            "Diagrama de Secuencia CUN08 (Registrar Consulta): UI modelada como Boundary (GUI). Nutricionista ingresa mediciones con cálculo en vivo vía BLL y Strategy Pattern (OMS). Al presionar Guardar, GUI dispara hacia BLL -> persistencia atómica en DAL con 1 único hit transaccional, auditoría en BitacoraBLL hacia DAL y recálculo de integridad en DigitoVerificadorBLL hacia DAL.",
            lifelines_c08, messages_c08,
            [
                os.path.join(dss_dir, "CUN08 Registrar Consulta.png"),
                os.path.join(scratch_dir, "cun08_secuencia.png"),
                os.path.join(artifact_dir, "cun08_secuencia.png")
            ],
            lifeline_bottom=-980
        )

        # =====================================================================
        # 4. CUN09: PRESCRIBIR PLAN (Diag ID 114)
        # =====================================================================
        print("\n--- Updating DSS CUN09 (ID 114) with generic DAL ---")
        el_gui_c09 = get_or_create_boundary(uc09, "GUI")
        el_obj_plan_bll9 = get_or_create_child_object(uc09, "PlanAlimentarioBLL_DNI101", "Object")
        el_obj_bit9 = get_or_create_child_object(uc09, "BitacoraBLL", "Object")
        el_obj_dv9 = get_or_create_child_object(uc09, "DigitoVerificadorBLL", "Object")
        el_obj_dal9 = get_or_create_child_object(uc09, "DAL", "Object")

        lifelines_c09 = [
            (el_nutri_actor,    20,   120,  70),
            (el_gui_c09,        160,  240,  200),
            (el_obj_plan_bll9,  290,  490,  390),
            (el_obj_bit9,       530,  660,  595),
            (el_obj_dv9,        700,  850,  775),
            (el_obj_dal9,       890,  1010, 950)
        ]

        messages_c09 = [
            # Cálculo de macronutrientes interactivo
            (1, el_nutri_actor, el_gui_c09, "numMacronutrientes_ValueChanged(pctCarb, pctProt, pctGrasas)", "SynchCall", -90, "Synchronous", "", "Call", ""),
            (2, el_gui_c09, el_gui_c09, "CalcularGramosYValidarSuma100()", "SynchCall", -125, "Synchronous", "", "Call", ""),
            (3, el_gui_c09, el_nutri_actor, "", "Return", -155, "Synchronous", "", "Call", "1"),

            # Guardado de plan alimentario en DAL genérica
            (4, el_nutri_actor, el_gui_c09, "btnGuardarPlan_Click()", "SynchCall", -195, "Synchronous", "", "Call", ""),
            (5, el_gui_c09, el_gui_c09, "ValidarCamposObligatorios()", "SynchCall", -230, "Synchronous", "", "Call", ""),
            (6, el_gui_c09, el_obj_plan_bll9, "PrescribirPlan(idConsulta, calorias, pctCarb, pctProt, pctGrasas, pautas, metas)", "SynchCall", -265, "Synchronous", "", "Call", ""),
            (7, el_obj_plan_bll9, el_obj_plan_bll9, "ValidarDistribucionCalorica()", "SynchCall", -300, "Synchronous", "", "Call", ""),
            (8, el_obj_plan_bll9, el_obj_plan_bll9, "CalcularDVPlan()", "SynchCall", -335, "Synchronous", "", "Call", ""),
            (9, el_obj_plan_bll9, el_obj_dal9, "GuardarPlanAlimentario(...) [1 DB Hit]", "SynchCall", -370, "Synchronous", "", "Call", ""),
            (10, el_obj_dal9, el_obj_plan_bll9, "", "Return", -405, "Synchronous", "", "Call", "1"),
            (11, el_obj_plan_bll9, el_obj_bit9, "RegistrarEvento(1, 'Prescripción de Plan Alimentario...', dniNutri, 'SeguimientoNutricional')", "SynchCall", -440, "Synchronous", "", "Call", ""),
            (12, el_obj_bit9, el_obj_dal9, "GuardarBitacora(bitacora)", "SynchCall", -475, "Synchronous", "", "Call", ""),
            (13, el_obj_dal9, el_obj_bit9, "", "Return", -505, "Synchronous", "", "Call", "1"),
            (14, el_obj_bit9, el_obj_plan_bll9, "", "Return", -535, "Synchronous", "", "Call", "1"),
            (15, el_obj_plan_bll9, el_obj_dv9, "RecalcularYPersistir()", "SynchCall", -570, "Synchronous", "", "Call", ""),
            (16, el_obj_dv9, el_obj_dal9, "PersistirIntegridadCompleta(resumen, filasDV)", "SynchCall", -605, "Synchronous", "", "Call", ""),
            (17, el_obj_dal9, el_obj_dv9, "", "Return", -635, "Synchronous", "", "Call", "1"),
            (18, el_obj_dv9, el_obj_plan_bll9, "", "Return", -665, "Synchronous", "", "Call", "1"),
            (19, el_obj_plan_bll9, el_gui_c09, "", "Return", -695, "Synchronous", "", "Call", "1"),
            (20, el_gui_c09, el_gui_c09, "Close()", "SynchCall", -725, "Synchronous", "", "Call", ""),
            (21, el_gui_c09, el_nutri_actor, "", "Return", -755, "Synchronous", "", "Call", "1")
        ]

        build_sequence_diagram(
            114, "CUN09: Diagrama de Secuencia",
            "Diagrama de Secuencia CUN09 (Prescribir Plan): UI modelada como Boundary (GUI). Validación de macronutrientes interactiva (100%), GUI dispara hacia PlanAlimentarioBLL -> persistencia en DAL genérica con 1 solo hit, registro en BitacoraBLL hacia DAL y recálculo en DigitoVerificadorBLL hacia DAL.",
            lifelines_c09, messages_c09,
            [
                os.path.join(dss_dir, "CUN09 Prescribir Plan.png"),
                os.path.join(scratch_dir, "cun09_secuencia.png"),
                os.path.join(artifact_dir, "cun09_secuencia.png")
            ],
            lifeline_bottom=-820
        )

        print("\nAll 4 Sequence Diagrams updated with Boundary UI and generic DAL successfully!")

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == '__main__':
    polish_and_clean_pn2()
