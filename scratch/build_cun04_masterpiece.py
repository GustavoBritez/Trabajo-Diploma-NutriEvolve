import win32com.client

def build_cun04():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    try:
        print("--- Building CUN04: Diagrama de Secuencia ---")
        diag = ea.GetDiagramByID(57)
        diag.Name = "CUN04: Diagrama de Secuencia"
        diag.Update()
        diag_id = 57

        uc = ea.GetElementByID(39)

        # Clear existing connectors for Diagram 57
        ea.Execute("DELETE FROM t_connector WHERE DiagramID = 57")

        # 1. Nutricionista (Actor) - ID 684
        el_nutri = ea.GetElementByID(684)

        # 2. frmTurnero_DNI101 (Object) - ID 685
        el_turnero = ea.GetElementByID(685)
        el_turnero.Name = "frmTurnero_DNI101"
        el_turnero.Update()

        # 3. frmModificarTurno_DNI101 (Object) - ID 686
        el_mod = ea.GetElementByID(686)
        el_mod.Name = "frmModificarTurno_DNI101"
        el_mod.Update()

        # 4. TurnoBLL_DNI101 (Object) - ID 687
        el_bll = ea.GetElementByID(687)
        el_bll.Name = "TurnoBLL_DNI101"
        el_bll.Update()

        # 5. TurnoBE_DNI101 (Object) - ID 688
        el_be = ea.GetElementByID(688)
        el_be.Name = "TurnoBE_DNI101"
        el_be.Update()

        # 6. IEstadoTurno_DNI101 (Object) - ID 689
        el_state = ea.GetElementByID(689)
        el_state.Name = "IEstadoTurno_DNI101"
        el_state.Update()

        # 7. EventoBLL (Object) - ID 691
        el_bit = ea.GetElementByID(691)
        el_bit.Name = "EventoBLL"
        el_bit.Update()

        # 8. DigitoVerificadorBLL (Object) - ID 690
        el_dv = ea.GetElementByID(690)
        el_dv.Name = "DigitoVerificadorBLL"
        el_dv.Update()

        # 9. DAL (Object) - ID 692
        el_dal = ea.GetElementByID(692)
        el_dal.Name = "DAL"
        el_dal.Update()

        # Fragments helper
        def create_or_get_element(parent, name, el_type, subtype=0):
            for el in parent.Elements:
                if el.Name == name and el.Type == el_type:
                    el.Subtype = subtype
                    el.Update()
                    return el
            new_el = parent.Elements.AddNew(name, el_type)
            new_el.Subtype = subtype
            new_el.Update()
            parent.Elements.Refresh()
            return new_el

        frag1_vacio = create_or_get_element(uc, "[Flujo 5.1: Motivo de consulta vacío]", "InteractionFragment", subtype=1)
        frag2_estado = create_or_get_element(uc, "[Flujo 7.1: Estado anterior no permite modificación (Asistió/Cancelado)]", "InteractionFragment", subtype=1)
        frag3_liberar = create_or_get_element(uc, "[liberarBloque: nuevoEstado == 'Cancelado']", "InteractionFragment", subtype=1)

        lifeline_bottom = -1400
        # (ElementID, Left, Right, CenterX)
        lifelines_info = [
            (el_nutri.ElementID,   20,   120,  70),
            (el_turnero.ElementID, 160,  280,  220),
            (el_mod.ElementID,     320,  480,  400),
            (el_bll.ElementID,     520,  660,  590),
            (el_be.ElementID,      700,  820,  760),
            (el_state.ElementID,   860,  1000, 930),
            (el_bit.ElementID,     1040, 1160, 1100),
            (el_dv.ElementID,      1200, 1340, 1270),
            (el_dal.ElementID,     1380, 1500, 1440),
            # Fragments
            (frag1_vacio.ElementID,   15, 1480, 0),
            (frag2_estado.ElementID,  15, 1480, 0),
            (frag3_liberar.ElementID, 15, 1480, 0)
        ]

        ea.Execute("DELETE FROM t_diagramobjects WHERE Diagram_ID = 57")
        diag.DiagramObjects.Refresh()

        for idx, item in enumerate(lifelines_info):
            eid = item[0]
            l = item[1]
            r = item[2]
            if eid == frag1_vacio.ElementID:
                d_obj = diag.DiagramObjects.AddNew(f"l={l};r={r};t=-310;b=-400", "")
                d_obj.ElementID = eid
                d_obj.Sequence = 1
                d_obj.Update()
            elif eid == frag2_estado.ElementID:
                d_obj = diag.DiagramObjects.AddNew(f"l={l};r={r};t=-530;b=-620", "")
                d_obj.ElementID = eid
                d_obj.Sequence = 1
                d_obj.Update()
            elif eid == frag3_liberar.ElementID:
                d_obj = diag.DiagramObjects.AddNew(f"l={l};r={r};t=-880;b=-970", "")
                d_obj.ElementID = eid
                d_obj.Sequence = 1
                d_obj.Update()
            else:
                d_obj = diag.DiagramObjects.AddNew(f"l={l};r={r};t=-50;b={lifeline_bottom}", "")
                d_obj.ElementID = eid
                d_obj.Sequence = idx + 2
                d_obj.Update()

        diag.DiagramObjects.Refresh()
        diag.Update()

        centers = {eid: cx for eid, l, r, cx in lifelines_info if cx > 0}

        msg_defs = [
            # Fase 1: Apertura y Carga del Modal
            (1,  el_nutri.ElementID, el_turnero.ElementID, "btn_Modificar_Turno_Click(sender, e)", "SynchCall", False),
            (2,  el_turnero.ElementID, el_turnero.ElementID, "ObtenerTurnoSeleccionado()", "SynchCall", True),
            (3,  el_turnero.ElementID, el_mod.ElementID, "new frmModificarTurno_DNI101(turnoSeleccionado)", "SynchCall", False),
            (4,  el_turnero.ElementID, el_mod.ElementID, "ShowDialog(this)", "SynchCall", False),
            (5,  el_mod.ElementID, el_mod.ElementID, "MostrarDatosTurno(_turnoActual)", "SynchCall", True),

            # Fase 2: Edición y Validación en Formulario
            (6,  el_nutri.ElementID, el_mod.ElementID, "btnGuardar_Click(sender, e)", "SynchCall", False),

            # Alternancia 1: [Flujo 5.1: Motivo de consulta vacío]
            (7,  el_mod.ElementID, el_nutri.ElementID, "MostrarAdvertencia('El motivo de consulta no puede estar vacío')", "SynchCall", False),
            (8,  el_mod.ElementID, el_mod.ElementID, "txtMotivo.Focus()", "SynchCall", True),

            # Fase 3: Invocación a Negocio y Recuperación
            (9,  el_mod.ElementID, el_bll.ElementID, "ModificarTurno(codigoTurno, nuevoMotivo, nuevoEstado)", "SynchCall", False),
            (10, el_bll.ElementID, el_dal.ElementID, "ObtenerPorCodigo(codigoTurno)", "SynchCall", False),
            (11, el_dal.ElementID, el_bll.ElementID, "", "Return", False),

            # Alternancia 2: [Flujo 7.1: Estado anterior no permite modificación (Asistió/Cancelado)]
            (12, el_bll.ElementID, el_mod.ElementID, "throw InvalidOperationException('El turno no permite cambiar a otro estado')", "Return", False),
            (13, el_mod.ElementID, el_nutri.ElementID, "MostrarError('El turno no permite cambiar a otro estado')", "SynchCall", False),

            # Fase 4: Patrón State, DV Individual y Persistencia (Flujo Normal)
            (14, el_bll.ElementID, el_be.ElementID, "ConfigurarEstadoPorNombre(nuevoEstado)", "SynchCall", False),
            (15, el_be.ElementID, el_state.ElementID, "CambiarEstado(nuevoEstado)", "SynchCall", False),
            (16, el_state.ElementID, el_be.ElementID, "", "Return", False),
            (17, el_be.ElementID, el_bll.ElementID, "", "Return", False),
            (18, el_bll.ElementID, el_bll.ElementID, "CalcularDV(cadenaDV)", "SynchCall", True),
            (19, el_bll.ElementID, el_dal.ElementID, "ModificarTurno(idTurno, fecha, hora, motivo, estado, dv)", "SynchCall", False),
            (20, el_dal.ElementID, el_bll.ElementID, "", "Return", False),

            # Alternancia 3: [liberarBloque: nuevoEstado == 'Cancelado']
            (21, el_bll.ElementID, el_dal.ElementID, "ActualizarEstadoBloque(idBloque, 'Disponible')", "SynchCall", False),
            (22, el_dal.ElementID, el_bll.ElementID, "", "Return", False),

            # Fase 5: Auditoría y Dígito Verificador Global
            (23, el_bll.ElementID, el_bit.ElementID, "RegistrarEvento(2, 'Turno modificado...', dniActual, 'TurneroNutricional')", "SynchCall", False),
            (24, el_bit.ElementID, el_dal.ElementID, "GuardarBitacora(evento)", "SynchCall", False),
            (25, el_bll.ElementID, el_dv.ElementID, "RecalcularYPersistir()", "SynchCall", False),
            (26, el_dv.ElementID, el_dal.ElementID, "ActualizarDV(firmas)", "SynchCall", False),

            # Fase 6: Retorno a UI y Cierre
            (27, el_bll.ElementID, el_mod.ElementID, "", "Return", False),
            (28, el_mod.ElementID, el_nutri.ElementID, "MessageBox.Show('El turno fue modificado exitosamente')", "SynchCall", False),
            (29, el_mod.ElementID, el_mod.ElementID, "Close()", "SynchCall", True),
            (30, el_mod.ElementID, el_turnero.ElementID, "", "Return", False),
            (31, el_turnero.ElementID, el_turnero.ElementID, "CargarTurnos()", "SynchCall", True),
            (32, el_turnero.ElementID, el_nutri.ElementID, "", "Return", False)
        ]

        y_coords = [
            -80,   # 1 btn_Modificar_Turno_Click
            -115,  # 2 ObtenerTurnoSeleccionado (self)
            -150,  # 3 new frmModificarTurno_DNI101
            -185,  # 4 ShowDialog
            -220,  # 5 MostrarDatosTurno (self)
            -255,  # 6 btnGuardar_Click
            # Frag 1 (-310 to -400)
            -340,  # 7 MostrarAdvertencia
            -375,  # 8 txtMotivo.Focus (self)
            # Below Frag 1
            -440,  # 9 ModificarTurno
            -475,  # 10 ObtenerPorCodigo
            -505,  # 11 return
            # Frag 2 (-530 to -620)
            -560,  # 12 throw InvalidOperationException
            -595,  # 13 MostrarError
            # Below Frag 2 (Normal flow)
            -660,  # 14 ConfigurarEstadoPorNombre
            -695,  # 15 CambiarEstado
            -725,  # 16 return
            -755,  # 17 return
            -790,  # 18 CalcularDV (self)
            -825,  # 19 ModificarTurno (DAL)
            -855,  # 20 return
            # Frag 3 (-880 to -970)
            -910,  # 21 ActualizarEstadoBloque
            -940,  # 22 return
            # Below Frag 3
            -1010, # 23 RegistrarEvento
            -1045, # 24 GuardarBitacora
            -1080, # 25 RecalcularYPersistir
            -1115, # 26 ActualizarDV
            -1150, # 27 return to frmModificarTurno_DNI101
            -1185, # 28 MessageBox.Show
            -1220, # 29 Close (self)
            -1255, # 30 return to frmTurnero_DNI101
            -1290, # 31 CargarTurnos (self)
            -1325  # 32 return to Nutri
        ]

        for idx, (seq, s_id, r_id, m_name, stype, is_self) in enumerate(msg_defs):
            sy = y_coords[idx]
            sx = centers[s_id]
            if is_self:
                ex = sx + 30
                ey = sy - 20
            else:
                ex = centers[r_id]
                ey = sy

            sender_el = ea.GetElementByID(s_id)
            conn = sender_el.Connectors.AddNew(m_name, "Sequence")
            conn.SupplierID = r_id
            conn.SubType = stype
            conn.Update()

            escaped_name = m_name.replace("'", "''") if m_name else ""
            sql = f"UPDATE t_connector SET DiagramID = {diag_id}, SeqNo = {seq}, Name = '{escaped_name}', PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey} WHERE Connector_ID = {conn.ConnectorID}"
            ea.Execute(sql)

        diag.Update()
        print("Diagram 57 (CUN04) updated successfully.")

        # Export image
        out_img = r"C:\Users\Danie\Desktop\GIT\TD\scratch\cun04_secuencia.png"
        proj = ea.GetProjectInterface()
        proj.PutDiagramImageToFile(diag.DiagramGUID, out_img, 1)
        print("Exported CUN04 image to:", out_img)

    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    build_cun04()
