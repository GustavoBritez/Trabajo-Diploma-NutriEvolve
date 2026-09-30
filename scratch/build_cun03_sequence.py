import win32com.client
import os

def build_cun03():
    ea = win32com.client.Dispatch('EA.Repository')
    try:
        ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
        diag = ea.GetDiagramByID(13)
        diag.Name = "CUN03: Diagrama de Secuencia"
        diag.Update()

        pkg = ea.GetPackageByID(4)
        uc = ea.GetElementByID(13)

        # Clear existing connectors for Diagram 13
        ea.Execute("DELETE FROM t_connector WHERE DiagramID = 13")

        # Check / Get / Create all required elements:
        # 1. Nutricionista (Actor) - ID 266
        el_nutri = ea.GetElementByID(266)

        # 2. frmTurnero_DNI101 (Object) - ID 267
        el_turnero = ea.GetElementByID(267)
        el_turnero.Name = "frmTurnero_DNI101"
        el_turnero.Update()

        # 3. FrmReprogramarTurno_DNI101 (Object) under UC 13
        el_reprog = None
        for el in uc.Elements:
            if el.Name == "FrmReprogramarTurno_DNI101":
                el_reprog = el
                break
        if not el_reprog:
            el_reprog = uc.Elements.AddNew("FrmReprogramarTurno_DNI101", "Object")
            el_reprog.Update()

        # 4. AgendaMedicaBLL_DNI101 (Object) - ID 269
        el_agenda_bll = ea.GetElementByID(269)

        # 5. TurnoBLL_DNI101 (Object) - ID 268
        el_turno_bll = ea.GetElementByID(268)

        # 6. TurnoBE_DNI101 (Object) - ID 270
        el_be = ea.GetElementByID(270)

        # 7. TurnoConfirmadoState_DNI101 (Object) - ID 271
        el_state = ea.GetElementByID(271)

        # 8. DigitoVerificadorBLL (Object) - ID 273
        el_dv = ea.GetElementByID(273)

        # 9. ServicesSessionManager (Object) - ID 701
        el_session = ea.GetElementByID(701)

        # 10. EventoBLL (Object) - ID 274
        el_bitacora = ea.GetElementByID(274)

        # 11. DAL ÚNICA (Object) - ID 272
        el_dal = ea.GetElementByID(272)
        el_dal.Name = "DAL"
        el_dal.Update()

        # Lifelines configuration: (ElementID, Left, Right, CenterX)
        # Exactly 11 lifelines with a single generic DAL at the right
        lifeline_bottom = -1620
        lifelines_info = [
            (el_nutri.ElementID,       20,   120,  70),
            (el_turnero.ElementID,     160,  280,  220),
            (el_reprog.ElementID,      320,  480,  400),
            (el_agenda_bll.ElementID,  520,  660,  590),
            (el_turno_bll.ElementID,   700,  840,  770),
            (el_be.ElementID,          880,  1000, 940),
            (el_state.ElementID,       1040, 1220, 1130),
            (el_dv.ElementID,          1260, 1400, 1330),
            (el_session.ElementID,     1440, 1580, 1510),
            (el_bitacora.ElementID,    1620, 1740, 1680),
            (el_dal.ElementID,         1780, 1880, 1830)
        ]

        # Clear old diagram objects and re-add our 11 lifelines
        ea.Execute("DELETE FROM t_diagramobjects WHERE Diagram_ID = 13")
        diag.DiagramObjects.Refresh()

        for idx, (eid, l, r, cx) in enumerate(lifelines_info):
            d_obj = diag.DiagramObjects.AddNew(f"l={l};r={r};t=-50;b={lifeline_bottom}", "")
            d_obj.ElementID = eid
            d_obj.Sequence = idx + 1
            d_obj.Update()

        diag.DiagramObjects.Refresh()
        diag.Update()

        # Map ID to CenterX
        centers = {eid: cx for eid, l, r, cx in lifelines_info}

        # Sequence of messages:
        # (seq, start_id, end_id, name, subtype, is_self)
        msg_defs = [
            # Phase 1: Clic en Reprogramar en frmTurnero_DNI101
            (1, el_nutri.ElementID, el_turnero.ElementID, "btn_Reprogramar_Turno_Click(sender, e)", "SynchCall", False),
            (2, el_turnero.ElementID, el_turnero.ElementID, "ObtenerTurnoSeleccionado()", "SynchCall", True),
            (3, el_turnero.ElementID, el_turno_bll.ElementID, "ObtenerPorId(idTurno)", "SynchCall", False),
            (4, el_turno_bll.ElementID, el_dal.ElementID, "ObtenerPorId(idTurno)", "SynchCall", False),
            (5, el_dal.ElementID, el_turno_bll.ElementID, "", "Return", False),
            (6, el_turno_bll.ElementID, el_turnero.ElementID, "", "Return", False),
            (7, el_turnero.ElementID, el_reprog.ElementID, "new FrmReprogramarTurno_DNI101(turnoCompleto)", "SynchCall", False),
            (8, el_turnero.ElementID, el_reprog.ElementID, "ShowDialog(this)", "SynchCall", False),

            # Phase 2: Carga y consulta de bloques horarios disponibles
            (9, el_reprog.ElementID, el_agenda_bll.ElementID, "ListarBloquesDisponibles(fecha, dniNutricionista)", "SynchCall", False),
            (10, el_agenda_bll.ElementID, el_dal.ElementID, "ListarBloquesDisponibles(fecha, dniNutricionista)", "SynchCall", False),
            (11, el_dal.ElementID, el_agenda_bll.ElementID, "", "Return", False),
            (12, el_agenda_bll.ElementID, el_reprog.ElementID, "", "Return", False),
            (13, el_reprog.ElementID, el_reprog.ElementID, "CargarBloquesDisponibles()", "SynchCall", True),

            # Phase 3: Selección de nuevo horario y Confirmación
            (14, el_nutri.ElementID, el_reprog.ElementID, "SeleccionarNuevoHorario(nuevaFecha, nuevoIdBloque)", "SynchCall", False),
            (15, el_nutri.ElementID, el_reprog.ElementID, "btnConfirmar_Click(sender, e)", "SynchCall", False),
            (16, el_reprog.ElementID, el_turno_bll.ElementID, "ReprogramarTurno(idTurno, nuevaFecha, nuevaHora, nuevoIdBloque, dniNutri)", "SynchCall", False),

            # Phase 4: State Pattern y DV
            (17, el_turno_bll.ElementID, el_be.ElementID, "Reprogramar(nuevaFecha, nuevaHora, nuevoIdBloque)", "SynchCall", False),
            (18, el_be.ElementID, el_state.ElementID, "Reprogramar(this, nuevaFecha, nuevaHora, nuevoIdBloque)", "SynchCall", False),
            (19, el_state.ElementID, el_be.ElementID, "", "Return", False),
            (20, el_be.ElementID, el_turno_bll.ElementID, "", "Return", False),
            (21, el_turno_bll.ElementID, el_turno_bll.ElementID, "CalcularDV(cadenaDV)", "SynchCall", True),

            # Phase 5: Persistencia en DAL Única y Actualización de Bloques
            (22, el_turno_bll.ElementID, el_dal.ElementID, "ReprogramarTurno(idTurno, nuevaFecha, nuevaHora, nuevoIdBloque, dniNutri, dv)", "SynchCall", False),
            (23, el_dal.ElementID, el_turno_bll.ElementID, "", "Return", False),
            (24, el_turno_bll.ElementID, el_dal.ElementID, "ActualizarEstadoBloque(idBloqueAnterior, 'Disponible')", "SynchCall", False),
            (25, el_dal.ElementID, el_turno_bll.ElementID, "", "Return", False),
            (26, el_turno_bll.ElementID, el_dal.ElementID, "ActualizarEstadoBloque(nuevoIdBloque, 'Ocupado')", "SynchCall", False),
            (27, el_dal.ElementID, el_turno_bll.ElementID, "", "Return", False),

            # Phase 6: Dígito Verificador, Sesión y Bitácora
            (28, el_turno_bll.ElementID, el_dv.ElementID, "RecalcularYPersistir()", "SynchCall", False),
            (29, el_dv.ElementID, el_dal.ElementID, "ActualizarDV(firmas)", "SynchCall", False),
            (30, el_turno_bll.ElementID, el_session.ElementID, "ObtenerDniUsuarioActual()", "SynchCall", False),
            (31, el_session.ElementID, el_turno_bll.ElementID, "", "Return", False),
            (32, el_turno_bll.ElementID, el_bitacora.ElementID, "RegistrarEvento(2, 'Turno reprogramado...', dniActual, 'TurneroNutricional')", "SynchCall", False),
            (33, el_bitacora.ElementID, el_dal.ElementID, "GuardarBitacora(evento)", "SynchCall", False),

            # Phase 7: Retorno a UI, MessageBox, Close y CargarTurnos
            (34, el_turno_bll.ElementID, el_reprog.ElementID, "", "Return", False),
            (35, el_reprog.ElementID, el_nutri.ElementID, "MessageBox.Show('¡El turno ha sido reprogramado exitosamente!')", "SynchCall", False),
            (36, el_reprog.ElementID, el_turnero.ElementID, "", "Return", False),
            (37, el_turnero.ElementID, el_turnero.ElementID, "CargarTurnos()", "SynchCall", True),
            (38, el_turnero.ElementID, el_nutri.ElementID, "", "Return", False)
        ]

        start_y = -95
        step_y = 38

        for idx, (seq, s_id, r_id, m_name, stype, is_self) in enumerate(msg_defs):
            sy = start_y - (idx * step_y)
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
            sql = f"UPDATE t_connector SET DiagramID = 13, SeqNo = {seq}, Name = '{escaped_name}', PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey} WHERE Connector_ID = {conn.ConnectorID}"
            ea.Execute(sql)

        diag.Update()
        print("Diagram 13 updated successfully with single DAL.")

        # Export image
        out_img = r"C:\Users\Danie\Desktop\GIT\TD\scratch\cun03_secuencia.png"
        proj = ea.GetProjectInterface()
        proj.PutDiagramImageToFile(diag.DiagramGUID, out_img, 1)
        print("Exported image to:", out_img)

    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    build_cun03()
