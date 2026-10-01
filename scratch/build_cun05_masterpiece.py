import win32com.client

def build_cun05():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    try:
        print("--- Building CUN05: Diagrama de Secuencia ---")
        diag = ea.GetDiagramByID(58)
        diag.Name = "CUN05: Diagrama de Secuencia"
        diag.Update()
        diag_id = 58

        uc = ea.GetElementByID(37)

        # Clear existing connectors for Diagram 58
        ea.Execute("DELETE FROM t_connector WHERE DiagramID = 58")

        # 1. Nutricionista (Actor) - ID 693
        el_nutri = ea.GetElementByID(693)

        # 2. frmTurnero_DNI101 (Object) - ID 694
        el_turnero = ea.GetElementByID(694)
        el_turnero.Name = "frmTurnero_DNI101"
        el_turnero.Update()

        # 3. TurnoBLL_DNI101 (Object) - ID 695
        el_bll = ea.GetElementByID(695)
        el_bll.Name = "TurnoBLL_DNI101"
        el_bll.Update()

        # 4. TurnoBE_DNI101 (Object) - ID 696
        el_be = ea.GetElementByID(696)
        el_be.Name = "TurnoBE_DNI101"
        el_be.Update()

        # 5. IEstadoTurno_DNI101 (Object)
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

        el_state = create_or_get_element(uc, "IEstadoTurno_DNI101", "Object")

        # 6. ServicesSessionManager (Object) - ID 701
        el_session = ea.GetElementByID(701)
        el_session.Name = "ServicesSessionManager"
        el_session.Update()

        # 7. EventoBLL (Object) - ID 699
        el_bit = ea.GetElementByID(699)
        el_bit.Name = "EventoBLL"
        el_bit.Update()

        # 8. DigitoVerificadorBLL (Object) - ID 698
        el_dv = ea.GetElementByID(698)
        el_dv.Name = "DigitoVerificadorBLL"
        el_dv.Update()

        # 9. DAL (Object) - ID 708
        el_dal = ea.GetElementByID(708)
        el_dal.Name = "DAL"
        el_dal.Update()

        # Fragment: [Flujo 4.1.1: Estado no permite cancelación]
        frag_estado = create_or_get_element(uc, "[Flujo 4.1.1: Estado no permite cancelación]", "InteractionFragment", subtype=1)
        lifeline_bottom = -1140

        # (ElementID, Left, Right, CenterX)
        lifelines_info = [
            (el_nutri.ElementID,    20,   120,  70),
            (el_turnero.ElementID,  160,  290,  225),
            (el_bll.ElementID,      330,  470,  400),
            (el_be.ElementID,       510,  630,  570),
            (el_state.ElementID,    670,  810,  740),
            (el_session.ElementID,  850, 1010,  930),
            (el_bit.ElementID,     1050, 1160, 1105),
            (el_dv.ElementID,      1200, 1340, 1270),
            (el_dal.ElementID,     1380, 1500, 1440),
            # Fragment
            (frag_estado.ElementID,  15, 1480, 0)
        ]

        ea.Execute("DELETE FROM t_diagramobjects WHERE Diagram_ID = 58")
        diag.DiagramObjects.Refresh()

        for idx, item in enumerate(lifelines_info):
            eid = item[0]
            l = item[1]
            r = item[2]
            if eid == frag_estado.ElementID:
                d_obj = diag.DiagramObjects.AddNew(f"l={l};r={r};t=-325;b=-420", "")
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
            # Fase 1: Interacción de UI y Confirmación
            (1,  el_nutri.ElementID,   el_turnero.ElementID, "btn_Cancelar_Turno_Click(sender, e)", "SynchCall", False),
            (2,  el_turnero.ElementID, el_turnero.ElementID, "ObtenerTurnoSeleccionado()", "SynchCall", True),
            (3,  el_turnero.ElementID, el_nutri.ElementID,   "MessageBox.Show('¿Está seguro de que desea cancelar el turno?...', MessageBoxButtons.YesNo)", "SynchCall", False),
            (4,  el_nutri.ElementID,   el_turnero.ElementID, "Confirmar(DialogResult.Yes)", "SynchCall", False),

            # Fase 2: Invocación a BLL y Recuperación del Turno
            (5,  el_turnero.ElementID, el_bll.ElementID,     "CancelarTurno(codigoTurno)", "SynchCall", False),
            (6,  el_bll.ElementID,     el_dal.ElementID,     "ObtenerPorCodigo(codigoTurno)", "SynchCall", False),
            (7,  el_dal.ElementID,     el_bll.ElementID,     "", "Return", False),

            # Alternancia: [Flujo 4.1.1: Estado no permite cancelación]
            (8,  el_bll.ElementID,     el_turnero.ElementID, "throw InvalidOperationException('El turno ya se encuentra cancelado o fue atendido')", "Return", False),
            (9,  el_turnero.ElementID, el_nutri.ElementID,   "MessageBox.Show('El turno ya se encuentra cancelado o fue atendido')", "SynchCall", False),

            # Fase 3: Patrón State (Transición a Cancelado - Flujo Normal)
            (10, el_bll.ElementID,     el_be.ElementID,      "Cancelar(motivo)", "SynchCall", False),
            (11, el_be.ElementID,      el_state.ElementID,   "Cancelar(this, motivo)", "SynchCall", False),
            (12, el_state.ElementID,   el_be.ElementID,      "", "Return", False),
            (13, el_be.ElementID,      el_bll.ElementID,     "", "Return", False),

            # Fase 4: Cálculo de DV Individual y Persistencia del Turno
            (14, el_bll.ElementID,     el_bll.ElementID,     "CalcularDV(cadenaDV)", "SynchCall", True),
            (15, el_bll.ElementID,     el_dal.ElementID,     "CancelarTurno(idTurno, motivo, dv)", "SynchCall", False),
            (16, el_dal.ElementID,     el_bll.ElementID,     "", "Return", False),

            # Fase 5: Liberación de Bloque Horario en Agenda
            (17, el_bll.ElementID,     el_dal.ElementID,     "ActualizarEstadoBloque(idBloque, 'Disponible')", "SynchCall", False),
            (18, el_dal.ElementID,     el_bll.ElementID,     "", "Return", False),

            # Fase 6: Auditoría y Dígito Verificador Global
            (19, el_bll.ElementID,     el_session.ElementID, "ObtenerDniUsuarioActual()", "SynchCall", False),
            (20, el_session.ElementID, el_bll.ElementID,     "", "Return", False),
            (21, el_bll.ElementID,     el_bit.ElementID,     "RegistrarEvento(2, 'Turno cancelado (CUN05)...', dniActual, 'TurneroNutricional')", "SynchCall", False),
            (22, el_bit.ElementID,     el_dal.ElementID,     "GuardarBitacora(evento)", "SynchCall", False),
            (23, el_bll.ElementID,     el_dv.ElementID,      "RecalcularYPersistir()", "SynchCall", False),
            (24, el_dv.ElementID,      el_dal.ElementID,     "ActualizarDV(firmas)", "SynchCall", False),

            # Fase 7: Retorno a la UI y Refresco de Grilla
            (25, el_bll.ElementID,     el_turnero.ElementID, "", "Return", False),
            (26, el_turnero.ElementID, el_turnero.ElementID, "CargarTurnos()", "SynchCall", True),
            (27, el_turnero.ElementID, el_nutri.ElementID,   "MessageBox.Show('El turno ha sido cancelado con éxito...')", "SynchCall", False),
            (28, el_turnero.ElementID, el_nutri.ElementID,   "", "Return", False)
        ]

        y_coords = [
            -80,   # 1 btn_Cancelar_Turno_Click
            -115,  # 2 ObtenerTurnoSeleccionado (self)
            -155,  # 3 MessageBox.Show('¿Está seguro...?')
            -190,  # 4 Confirmar(DialogResult.Yes)
            -230,  # 5 CancelarTurno(codigoTurno)
            -265,  # 6 ObtenerPorCodigo
            -295,  # 7 return DAL
            # Frag 1 (-325 to -420)
            -355,  # 8 throw InvalidOperationException
            -390,  # 9 MessageBox.Show('El turno ya se encuentra cancelado o fue atendido')
            # Below Frag 1 (Flujo Normal)
            -455,  # 10 Cancelar(motivo)
            -490,  # 11 Cancelar(this, motivo)
            -520,  # 12 return IEstadoTurno
            -550,  # 13 return TurnoBE
            -585,  # 14 CalcularDV (self)
            -620,  # 15 CancelarTurno DAL
            -650,  # 16 return DAL
            -685,  # 17 ActualizarEstadoBloque DAL
            -715,  # 18 return DAL
            -750,  # 19 ObtenerDniUsuarioActual
            -780,  # 20 return Session
            -815,  # 21 RegistrarEvento
            -850,  # 22 GuardarBitacora DAL
            -885,  # 23 RecalcularYPersistir
            -920,  # 24 ActualizarDV DAL
            -960,  # 25 return to frmTurnero
            -995,  # 26 CargarTurnos (self)
            -1035, # 27 MessageBox.Show éxito
            -1070  # 28 return to Nutri
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
        print("Diagram 58 (CUN05) updated successfully.")

        # Export image
        out_img = r"C:\Users\Danie\Desktop\GIT\TD\scratch\cun05_secuencia.png"
        proj = ea.GetProjectInterface()
        proj.PutDiagramImageToFile(diag.DiagramGUID, out_img, 1)
        print("Exported CUN05 image to:", out_img)

    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    build_cun05()
