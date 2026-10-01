import win32com.client
import os

def create_or_get_element(parent, name, el_type, subtype=0, notes=""):
    for el in parent.Elements:
        if el.Name == name and el.Type == el_type:
            if subtype != 0:
                el.Subtype = subtype
            if notes:
                el.Notes = notes
            el.Update()
            return el
    el = parent.Elements.AddNew(name, el_type)
    if subtype != 0:
        el.Subtype = subtype
    if notes:
        el.Notes = notes
    el.Update()
    return el

def build_both():
    ea = win32com.client.Dispatch('EA.Repository')
    try:
        ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
        proj = ea.GetProjectInterface()

        # =====================================================================
        # 1. BUILD CUN01: REGISTRAR TURNO (Diagram ID 12)
        # =====================================================================
        print("--- Building CUN01: Diagrama de Secuencia ---")
        diag1 = ea.GetDiagramByID(12)
        diag1.Name = "CUN01: Diagrama de Secuencia"
        diag1.Update()
        diag1_id = 12

        uc12 = ea.GetElementByID(12)

        # Clear old connectors
        ea.Execute(f"DELETE FROM t_connector WHERE DiagramID = {diag1_id}")

        # Lifelines in UC 12:
        el_nutri1 = ea.GetElementByID(219)
        el_turnero1 = ea.GetElementByID(220)
        el_turnero1.Name = "frmTurnero_DNI101"
        el_turnero1.Update()

        el_agenda_bll1 = ea.GetElementByID(221)
        el_pac_bll1 = ea.GetElementByID(222)
        el_cun02_oval = ea.GetElementByID(38)
        el_turno_bll1 = ea.GetElementByID(225)
        el_turno_be1 = ea.GetElementByID(226)
        el_state1 = ea.GetElementByID(227)
        el_state1.Name = "TurnoSolicitadoState_DNI101"
        el_state1.Update()
        el_bit1 = ea.GetElementByID(230)
        el_dv1 = ea.GetElementByID(229)
        el_dal1 = ea.GetElementByID(228)
        el_dal1.Name = "DAL"
        el_dal1.Update()

        # Fragments for CUN01:
        frag1_pac = create_or_get_element(uc12, "[Flujo 6.1: Paciente No Registrado - Inclusión CUN02]", "InteractionFragment", subtype=1)
        frag1_sup = create_or_get_element(uc12, "[Superposición de Turno: ExisteTurnoParaProfesional]", "InteractionFragment", subtype=1)

        lifeline_bottom1 = -1740
        # (ElementID, Left, Right, CenterX)
        lifelines1_info = [
            (el_nutri1.ElementID,       20,   120,  70),
            (el_turnero1.ElementID,     160,  280,  220),
            (el_agenda_bll1.ElementID,  320,  460,  390),
            (el_pac_bll1.ElementID,     490,  610,  550),
            (el_cun02_oval.ElementID,   640,  780,  710),
            (el_turno_bll1.ElementID,   820,  940,  880),
            (el_turno_be1.ElementID,    970,  1090, 1030),
            (el_state1.ElementID,       1120, 1240, 1180),
            (el_bit1.ElementID,         1270, 1390, 1330),
            (el_dv1.ElementID,          1420, 1540, 1480),
            (el_dal1.ElementID,         1580, 1720, 1650),
            # Fragment boxes:
            (frag1_pac.ElementID,       50,   800,  0),
            (frag1_sup.ElementID,       50,   1740, 0)
        ]

        ea.Execute(f"DELETE FROM t_diagramobjects WHERE Diagram_ID = {diag1_id}")
        diag1.DiagramObjects.Refresh()

        for idx, item in enumerate(lifelines1_info):
            eid = item[0]
            l = item[1]
            r = item[2]
            if eid == frag1_pac.ElementID:
                d_obj = diag1.DiagramObjects.AddNew(f"l={l};r={r};t=-470;b=-660", "")
                d_obj.ElementID = eid
                d_obj.Sequence = 1
                d_obj.Update()
            elif eid == frag1_sup.ElementID:
                d_obj = diag1.DiagramObjects.AddNew(f"l={l};r={r};t=-820;b=-960", "")
                d_obj.ElementID = eid
                d_obj.Sequence = 1
                d_obj.Update()
            elif eid == el_cun02_oval.ElementID:
                d_obj = diag1.DiagramObjects.AddNew(f"l={l};r={r};t=-565;b=-645", "")
                d_obj.ElementID = eid
                d_obj.Sequence = idx + 2
                d_obj.Update()
            else:
                d_obj = diag1.DiagramObjects.AddNew(f"l={l};r={r};t=-50;b={lifeline_bottom1}", "")
                d_obj.ElementID = eid
                d_obj.Sequence = idx + 2
                d_obj.Update()

        diag1.DiagramObjects.Refresh()
        diag1.Update()

        centers1 = {eid: cx for eid, l, r, cx in lifelines1_info if cx > 0}

        msg1_defs = [
            (1,  el_nutri1.ElementID, el_turnero1.ElementID, "btn_Registrar_Turno_Click(sender, e)", "SynchCall", False),
            (2,  el_turnero1.ElementID, el_turnero1.ElementID, "ConsultarDisponibilidad()", "SynchCall", True),
            (3,  el_turnero1.ElementID, el_agenda_bll1.ElementID, "ListarBloquesDisponibles(fecha, dniNutri)", "SynchCall", False),
            (4,  el_agenda_bll1.ElementID, el_dal1.ElementID, "ListarBloquesDisponibles(fecha, dniNutri)", "SynchCall", False),
            (5,  el_dal1.ElementID, el_agenda_bll1.ElementID, "", "Return", False),
            (6,  el_agenda_bll1.ElementID, el_dal1.ElementID, "ObtenerHorariosOcupadosPorProfesional(dniNutri, fecha)", "SynchCall", False),
            (7,  el_dal1.ElementID, el_agenda_bll1.ElementID, "", "Return", False),
            (8,  el_agenda_bll1.ElementID, el_turnero1.ElementID, "", "Return", False),
            (9,  el_turnero1.ElementID, el_pac_bll1.ElementID, "ObtenerPacientePorDNI(dniNiño)", "SynchCall", False),
            (10, el_pac_bll1.ElementID, el_dal1.ElementID, "ObtenerPacientePorDNI(dniNiño)", "SynchCall", False),
            (11, el_dal1.ElementID, el_pac_bll1.ElementID, "", "Return", False),
            (12, el_pac_bll1.ElementID, el_turnero1.ElementID, "", "Return", False),

            # Frag 1 (Paciente No Registrado - Extensión CUN02)
            (13, el_turnero1.ElementID, el_nutri1.ElementID, "InformarPacienteNoRegistrado()", "SynchCall", False),
            (14, el_nutri1.ElementID, el_turnero1.ElementID, "IngresarDatosPaciente(nombre, apellido, dniNiño, ...)", "SynchCall", False),
            (15, el_turnero1.ElementID, el_cun02_oval.ElementID, "RegistrarPaciente(nombre, apellido, dniNiño, ...)", "New", False),
            (16, el_cun02_oval.ElementID, el_turnero1.ElementID, "PacienteRegistradoOK()", "Return", False),

            # Agendamiento (Nombre exacto del botón y evento en código C#)
            (17, el_nutri1.ElementID, el_turnero1.ElementID, "btnRegistrarTurno_Click(sender, e)", "SynchCall", False),
            (18, el_turnero1.ElementID, el_turno_bll1.ElementID, "RegistrarTurno(dniNiño, fecha, horario, motivo, dniNutri, idBloque)", "SynchCall", False),

            # Verificación Anti-Superposición
            (19, el_turno_bll1.ElementID, el_dal1.ElementID, "ExisteTurnoParaProfesional(dniNutri, fecha, hora)", "SynchCall", False),
            (20, el_dal1.ElementID, el_turno_bll1.ElementID, "", "Return", False),
            # Frag 2 (Superposición de Turno)
            (21, el_turno_bll1.ElementID, el_turnero1.ElementID, "throw InvalidOperationException('El profesional seleccionado ya cuenta con un turno asignado')", "Return", False),
            (22, el_turnero1.ElementID, el_nutri1.ElementID, "MostrarError('El profesional seleccionado ya cuenta con un turno asignado')", "SynchCall", False),
            (23, el_turnero1.ElementID, el_turnero1.ElementID, "ConsultarDisponibilidad()", "SynchCall", True),

            # Flujo Normal
            (24, el_turno_bll1.ElementID, el_turno_be1.ElementID, "new TurnoBE_DNI101(dniNiño, fecha, horario, motivo)", "SynchCall", False),
            (25, el_turno_bll1.ElementID, el_turno_be1.ElementID, "GenerarCodigoUnico()", "SynchCall", False),
            (26, el_turno_bll1.ElementID, el_state1.ElementID, "new TurnoSolicitadoState_DNI101()", "SynchCall", False),
            (27, el_turno_bll1.ElementID, el_turno_be1.ElementID, "CambiarEstado(TurnoSolicitadoState)", "SynchCall", False),
            (28, el_turno_be1.ElementID, el_turno_bll1.ElementID, "", "Return", False),
            (29, el_turno_bll1.ElementID, el_turno_bll1.ElementID, "CalcularDV(cadenaDV)", "SynchCall", True),
            (30, el_turno_bll1.ElementID, el_dal1.ElementID, "Guardar(turnoBE)", "SynchCall", False),
            (31, el_dal1.ElementID, el_turno_bll1.ElementID, "", "Return", False),
            (32, el_turno_bll1.ElementID, el_dal1.ElementID, "ActualizarEstadoBloque(idBloque, 'Ocupado')", "SynchCall", False),
            (33, el_dal1.ElementID, el_turno_bll1.ElementID, "", "Return", False),

            # Auditoría y DV Global (Nuevo orden)
            (34, el_turno_bll1.ElementID, el_bit1.ElementID, "RegistrarEvento(1, 'Turno registrado...', dniNutri, 'TurneroNutricional')", "SynchCall", False),
            (35, el_bit1.ElementID, el_dal1.ElementID, "GuardarBitacora(evento)", "SynchCall", False),
            (36, el_turno_bll1.ElementID, el_dv1.ElementID, "RecalcularYPersistir()", "SynchCall", False),
            (37, el_dv1.ElementID, el_dal1.ElementID, "ActualizarDV(firmas)", "SynchCall", False),

            # Finalización
            (38, el_turno_bll1.ElementID, el_turnero1.ElementID, "", "Return", False),
            (39, el_turnero1.ElementID, el_nutri1.ElementID, "MessageBox.Show('¡Turno registrado exitosamente!')", "SynchCall", False),
            (40, el_turnero1.ElementID, el_turnero1.ElementID, "CargarTurnos()", "SynchCall", True),
            (41, el_turnero1.ElementID, el_nutri1.ElementID, "", "Return", False)
        ]

        y_coords1 = [
            -95,   # 1
            -135,  # 2 ConsultarDisponibilidad (self)
            -175,  # 3 ListarBloquesDisponibles
            -210,  # 4 ListarBloquesDisponibles (to DAL)
            -240,  # 5 return
            -275,  # 6 ObtenerHorariosOcupadosPorProfesional
            -305,  # 7 return
            -340,  # 8 return to UI
            -385,  # 9 ObtenerPacientePorDNI
            -420,  # 10 ObtenerPacientePorDNI (to DAL)
            -450,  # 11 return
            -480,  # 12 return to UI
            # Frag 1 (-465 to -660)
            -510, -545, -585, -625,
            # Outside
            -700, -740,
            # Query and Frag 2 (-820 to -960)
            -775, -810,
            -845, -885, -925,
            # Normal Flow below Frag 2 (clean gap!)
            -1040, -1075, -1110, -1145, -1180, -1215, -1250, -1285, -1320, -1355,
            -1390, -1425, -1460, -1495, -1535, -1570, -1605, -1645
        ]

        for idx, (seq, s_id, r_id, m_name, stype, is_self) in enumerate(msg1_defs):
            sy = y_coords1[idx]
            sx = centers1[s_id]
            if is_self:
                ex = sx + 30
                ey = sy - 20
            else:
                ex = centers1[r_id]
                ey = sy

            sender_el = ea.GetElementByID(s_id)
            conn = sender_el.Connectors.AddNew(m_name, "Sequence")
            conn.SupplierID = r_id
            conn.SubType = stype
            conn.Update()

            escaped_name = m_name.replace("'", "''") if m_name else ""
            sql = f"UPDATE t_connector SET DiagramID = {diag1_id}, SeqNo = {seq}, Name = '{escaped_name}', PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey} WHERE Connector_ID = {conn.ConnectorID}"
            ea.Execute(sql)

        diag1.Update()
        print("CUN01 Diagram updated successfully.")
        out_img1 = r"C:\Users\Danie\Desktop\GIT\TD\scratch\cun01_secuencia.png"
        proj.PutDiagramImageToFile(diag1.DiagramGUID, out_img1, 1)
        print("Exported CUN01 image to:", out_img1)


        # =====================================================================
        # 2. BUILD CUN02: REGISTRAR PACIENTE (Diagram ID 56)
        # =====================================================================
        print("--- Building CUN02: Diagrama de Secuencia ---")
        diag2 = ea.GetDiagramByID(56)
        diag2.Name = "CUN02: Diagrama de Secuencia"
        diag2.Update()
        diag2_id = 56

        uc38 = ea.GetElementByID(38)

        # Clear old connectors
        ea.Execute(f"DELETE FROM t_connector WHERE DiagramID = {diag2_id}")

        # Lifelines in UC 38:
        el_nutri2 = ea.GetElementByID(677)
        el_ui2 = ea.GetElementByID(678)
        el_bll2 = ea.GetElementByID(679)
        el_be2 = ea.GetElementByID(680)
        el_bit2 = ea.GetElementByID(682)
        el_dv2 = ea.GetElementByID(681)
        el_dal2 = ea.GetElementByID(683)
        el_dal2.Name = "DAL"
        el_dal2.Update()

        # Fragments for CUN02:
        frag2_campos = create_or_get_element(uc38, "[Flujo 7.1: Campos Obligatorios Incompletos]", "InteractionFragment", subtype=1)
        frag2_dni = create_or_get_element(uc38, "[Flujo 8.1: DNI Ya Registrado en BD]", "InteractionFragment", subtype=1)

        lifeline_bottom2 = -900
        # (ElementID, Left, Right, CenterX)
        lifelines2_info = [
            (el_nutri2.ElementID,  20,   120,  70),
            (el_ui2.ElementID,     180,  320,  250),
            (el_bll2.ElementID,    380,  520,  450),
            (el_be2.ElementID,     580,  720,  650),
            (el_bit2.ElementID,    780,  920,  850),
            (el_dv2.ElementID,     980,  1120, 1050),
            (el_dal2.ElementID,    1180, 1300, 1240),
            # Fragment boxes:
            (frag2_campos.ElementID, 15, 360, 0),
            (frag2_dni.ElementID,    15, 1320, 0)
        ]

        ea.Execute(f"DELETE FROM t_diagramobjects WHERE Diagram_ID = {diag2_id}")
        diag2.DiagramObjects.Refresh()

        for idx, item in enumerate(lifelines2_info):
            eid = item[0]
            l = item[1]
            r = item[2]
            if eid == frag2_campos.ElementID:
                d_obj = diag2.DiagramObjects.AddNew(f"l={l};r={r};t=-115;b=-245", "")
                d_obj.ElementID = eid
                d_obj.Sequence = 1
                d_obj.Update()
            elif eid == frag2_dni.ElementID:
                d_obj = diag2.DiagramObjects.AddNew(f"l={l};r={r};t=-365;b=-415", "")
                d_obj.ElementID = eid
                d_obj.Sequence = 1
                d_obj.Update()
            else:
                d_obj = diag2.DiagramObjects.AddNew(f"l={l};r={r};t=-50;b={lifeline_bottom2}", "")
                d_obj.ElementID = eid
                d_obj.Sequence = idx + 2
                d_obj.Update()

        diag2.DiagramObjects.Refresh()
        diag2.Update()

        centers2 = {eid: cx for eid, l, r, cx in lifelines2_info if cx > 0}

        msg2_defs = [
            # 1. Nutricionista presiona Registrar Paciente
            (1,  el_nutri2.ElementID, el_ui2.ElementID, "btnRegistrarPaciente_Click(nombre, apellido, dniNiño, telefono, email, obraSocial)", "SynchCall", False),

            # 2. Alternancia: [7.1: Campos Incompletos] (Inside Frag 1)
            (2,  el_ui2.ElementID, el_ui2.ElementID, "ValidarCamposObligatorios()", "SynchCall", True),
            (3,  el_ui2.ElementID, el_nutri2.ElementID, "MostrarAdvertencia('Por favor complete los campos obligatorios')", "SynchCall", False),
            (4,  el_ui2.ElementID, el_ui2.ElementID, "txtControl.Focus()", "SynchCall", True),

            # 3. Invocación BLL unificada (Refactorización!)
            (5,  el_ui2.ElementID, el_bll2.ElementID, "RegistrarPaciente(nombre, apellido, dniNiño, telefono, email, obraSocial)", "SynchCall", False),

            # 4. Verificación de DNI en DAL
            (6,  el_bll2.ElementID, el_dal2.ElementID, "ObtenerPacientePorDNI(dniNiño)", "SynchCall", False),
            (7,  el_dal2.ElementID, el_bll2.ElementID, "", "Return", False),

            # 5. Alternancia: [8.1: DNI Duplicado] (Inside Frag 2)
            (8,  el_bll2.ElementID, el_ui2.ElementID, "throw InvalidOperationException('El paciente con DNI ya se encuentra registrado')", "Return", False),
            (9,  el_ui2.ElementID, el_nutri2.ElementID, "MostrarError('El paciente con DNI ya se encuentra registrado')", "SynchCall", False),

            # 6. Flujo Normal de Creación y Persistencia (Below Frag 2)
            (10, el_bll2.ElementID, el_be2.ElementID, "new PacienteBE_DNI101(nombre, apellido, dniNiño, telefono, email, obraSocial)", "SynchCall", False),
            (11, el_bll2.ElementID, el_dal2.ElementID, "Guardar(nuevoPaciente)", "SynchCall", False),
            (12, el_dal2.ElementID, el_bll2.ElementID, "", "Return", False),

            # 7. Auditoría y Dígitos Verificadores (Nuevo Orden!)
            (13, el_bll2.ElementID, el_bit2.ElementID, "RegistrarEvento(1, 'Registro de Paciente Pediátrico...', dniActual, 'TurneroNutricional')", "SynchCall", False),
            (14, el_bit2.ElementID, el_dal2.ElementID, "GuardarBitacora(evento)", "SynchCall", False),
            (15, el_bll2.ElementID, el_dv2.ElementID, "RecalcularYPersistir()", "SynchCall", False),
            (16, el_dv2.ElementID, el_dal2.ElementID, "ActualizarDV(firmas)", "SynchCall", False),

            # 8. Retorno a UI y Confirmación
            (17, el_bll2.ElementID, el_ui2.ElementID, "", "Return", False),
            (18, el_ui2.ElementID, el_nutri2.ElementID, "MessageBox.Show('El paciente fue registrado exitosamente')", "SynchCall", False),
            (19, el_ui2.ElementID, el_ui2.ElementID, "Close()", "SynchCall", True),
            (20, el_ui2.ElementID, el_nutri2.ElementID, "", "Return", False)
        ]

        y_coords2 = [
            -75,  # 1: btnRegistrarPaciente_Click
            # Inside Frag 1 (-115 to -245):
            -140, # 2: ValidarCamposObligatorios (self)
            -175, # 3: MostrarAdvertencia
            -210, # 4: txtControl.Focus (self)
            # Below Frag 1:
            -275, # 5: RegistrarPaciente (BLL)
            -315, # 6: ObtenerPacientePorDNI
            -345, # 7: Return
            # Inside Frag 2 (-365 to -445):
            -390, # 8: throw InvalidOperationException
            -425, # 9: MostrarError
            # Below Frag 2 (clean gap!):
            -480, # 10: new PacienteBE_DNI101
            -520, # 11: Guardar
            -555, # 12: Return
            -595, # 13: RegistrarEvento
            -630, # 14: GuardarBitacora
            -670, # 15: RecalcularYPersistir
            -705, # 16: ActualizarDV
            -745, # 17: Return to UI
            -780, # 18: MessageBox.Show
            -815, # 19: Close (self)
            -850  # 20: Return to Nutri
        ]

        for idx, (seq, s_id, r_id, m_name, stype, is_self) in enumerate(msg2_defs):
            sy = y_coords2[idx]
            sx = centers2[s_id]
            if is_self:
                ex = sx + 30
                ey = sy - 20
            else:
                ex = centers2[r_id]
                ey = sy

            sender_el = ea.GetElementByID(s_id)
            conn = sender_el.Connectors.AddNew(m_name, "Sequence")
            conn.SupplierID = r_id
            conn.SubType = stype
            conn.Update()

            escaped_name = m_name.replace("'", "''") if m_name else ""
            sql = f"UPDATE t_connector SET DiagramID = {diag2_id}, SeqNo = {seq}, Name = '{escaped_name}', PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey} WHERE Connector_ID = {conn.ConnectorID}"
            ea.Execute(sql)

        diag2.Update()
        print("CUN02 Diagram updated successfully.")
        out_img2 = r"C:\Users\Danie\Desktop\GIT\TD\scratch\cun02_secuencia.png"
        proj.PutDiagramImageToFile(diag2.DiagramGUID, out_img2, 1)
        print("Exported CUN02 image to:", out_img2)

    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    build_both()
