import win32com.client
import os

def build_cun05():
    ea = win32com.client.Dispatch('EA.Repository')
    try:
        ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
        diag = ea.GetDiagramByID(58)
        diag.Name = "CUN05:Diagrama Actividad"
        diag.Update()
        
        pkg = ea.GetPackageByID(4)

        # Clear existing connectors for Diagram 58
        ea.Execute("DELETE FROM t_connector WHERE DiagramID = 58")

        # Let's inspect objects 693 to 700:
        # 693: Nutricionista (Actor)
        # 694: FormTurnero_DNI101 -> rename to frmTurnero_DNI101
        el_ui = ea.GetElementByID(694)
        el_ui.Name = "frmTurnero_DNI101"
        el_ui.Update()

        # 695: TurnoBLL_DNI101
        el_bll = ea.GetElementByID(695)
        el_bll.Name = "TurnoBLL_DNI101"
        el_bll.Update()

        # 696: TurnoBE_DNI101
        el_be = ea.GetElementByID(696)
        el_be.Name = "TurnoBE_DNI101"
        el_be.Update()

        # 700: DAL -> TurnoDAL_DNI101
        el_tdal = ea.GetElementByID(700)
        el_tdal.Name = "TurnoDAL_DNI101"
        el_tdal.Update()

        # 697: TurnoCanceladoState_DNI101 -> AgendaMedicaDAL_DNI101
        el_amdal = ea.GetElementByID(697)
        el_amdal.Name = "AgendaMedicaDAL_DNI101"
        el_amdal.Update()

        # 698: DigitoVerificadorBLL
        el_dv = ea.GetElementByID(698)
        el_dv.Name = "DigitoVerificadorBLL"
        el_dv.Update()

        # 699: EventoBLL -> BitacoraBLL
        el_bit = ea.GetElementByID(699)
        el_bit.Name = "BitacoraBLL"
        el_bit.Update()

        # Check or create ServicesSessionManager
        session_id = None
        for el in pkg.Elements:
            if el.Name == "ServicesSessionManager":
                session_id = el.ElementID
                break
        
        if not session_id:
            el_session = pkg.Elements.AddNew("ServicesSessionManager", "Object")
            el_session.Update()
            pkg.Elements.Refresh()
            session_id = el_session.ElementID

        # Ensure ServicesSessionManager is in Diagram 58
        found_in_diag = False
        for dobj in diag.DiagramObjects:
            if dobj.ElementID == session_id:
                found_in_diag = True
                break
        
        if not found_in_diag:
            dobj = diag.DiagramObjects.AddNew("l=0;r=0;t=0;b=0;", "")
            dobj.ElementID = session_id
            dobj.Update()
            diag.DiagramObjects.Refresh()

        # Coordinate layout
        # Lifelines:
        # 1. 693: Nutricionista (Actor)
        # 2. 694: frmTurnero_DNI101
        # 3. 695: TurnoBLL_DNI101
        # 4. 700: TurnoDAL_DNI101
        # 5. 696: TurnoBE_DNI101
        # 6. 697: AgendaMedicaDAL_DNI101
        # 7. 698: DigitoVerificadorBLL
        # 8. session_id: ServicesSessionManager
        # 9. 699: BitacoraBLL

        lifeline_bottom = -1150
        coords = {
            693:        (20,   120,  -50, lifeline_bottom, 1, 70),
            694:        (155,  285,  -50, lifeline_bottom, 2, 220),
            695:        (325,  455,  -50, lifeline_bottom, 3, 390),
            700:        (485,  615,  -50, lifeline_bottom, 4, 550),
            696:        (645,  765,  -50, lifeline_bottom, 5, 705),
            697:        (795,  955,  -50, lifeline_bottom, 6, 875),
            698:        (985,  1125, -50, lifeline_bottom, 7, 1055),
            session_id: (1155, 1315, -50, lifeline_bottom, 8, 1235),
            699:        (1345, 1465, -50, lifeline_bottom, 9, 1405),
        }

        for d_obj in diag.DiagramObjects:
            if d_obj.ElementID in coords:
                l, r, t, b, seq, cx = coords[d_obj.ElementID]
                d_obj.left = l
                d_obj.right = r
                d_obj.top = t
                d_obj.bottom = b
                d_obj.Sequence = seq
                d_obj.Update()

        diag.DiagramObjects.Refresh()
        diag.Update()

        # Centers:
        c_nutri = 70
        c_ui = 220
        c_bll = 390
        c_tdal = 550
        c_be = 705
        c_amdal = 875
        c_dv = 1055
        c_sess = 1235
        c_bit = 1405

        id_nutri = 693
        id_ui = 694
        id_bll = 695
        id_tdal = 700
        id_be = 696
        id_amdal = 697
        id_dv = 698
        id_sess = session_id
        id_bit = 699

        messages = [
            # 1. Clic en Cancelar Turno
            (1,  id_nutri, id_ui,   "btn_Cancelar_Turno_Click()", "SynchCall", c_nutri, -90, c_ui, -90),
            # 2. UI obtiene turno de la fila seleccionada
            (2,  id_ui,    id_ui,   "ObtenerTurnoSeleccionado()", "SynchCall", c_ui, -125, c_ui + 30, -145),
            # 3. UI solicita confirmación
            (3,  id_ui,    id_nutri,"SolicitarConfirmacion(codigoTurno)", "SynchCall", c_ui, -180, c_nutri, -180),
            # 4. Confirmación del usuario
            (4,  id_nutri, id_ui,   "Confirmar(DialogResult.Yes)", "SynchCall", c_nutri, -215, c_ui, -215),
            # 5. UI llama a TurnoBLL (Línea 241)
            (5,  id_ui,    id_bll,  "CancelarTurno(codigoTurno)", "SynchCall", c_ui, -255, c_bll, -255),
            # 6. TurnoBLL llama a TurnoDAL (Línea 338)
            (6,  id_bll,   id_tdal, "ObtenerPorCodigo(codigoTurno)", "SynchCall", c_bll, -295, c_tdal, -295),
            # 7. TurnoDAL devuelve TurnoBE
            (7,  id_tdal,  id_bll,  "", "Return", c_tdal, -330, c_bll, -330),
            # 8. TurnoBLL delega cancelación a TurnoBE (Línea 357)
            (8,  id_bll,   id_be,   "Cancelar(motivo)", "SynchCall", c_bll, -370, c_be, -370),
            # 9. Retorno de TurnoBE
            (9,  id_be,    id_bll,  "", "Return", c_be, -405, c_bll, -405),
            # 10. TurnoBLL calcula DV individual (Línea 361)
            (10, id_bll,   id_bll,  "CalcularDV(cadenaDV)", "SynchCall", c_bll, -440, c_bll + 30, -460),
            # 11. TurnoBLL persiste cancelación en TurnoDAL (Línea 364)
            (11, id_bll,   id_tdal, "CancelarTurno(idTurno, motivo, dv)", "SynchCall", c_bll, -495, c_tdal, -495),
            # 12. TurnoDAL confirma persistencia
            (12, id_tdal,  id_bll,  "", "Return", c_tdal, -530, c_bll, -530),
            # 13. TurnoBLL libera bloque horario en AgendaMedicaDAL (Línea 375)
            (13, id_bll,   id_amdal,"ActualizarEstadoBloque(idBloque, 'Disponible')", "SynchCall", c_bll, -570, c_amdal, -570),
            # 14. AgendaMedicaDAL confirma liberación de bloque
            (14, id_amdal, id_bll,  "", "Return", c_amdal, -605, c_bll, -605),
            # 15. TurnoBLL recalcula Dígitos Verificadores globales (Línea 383) - NO RETURN
            (15, id_bll,   id_dv,   "RecalcularYPersistir()", "SynchCall", c_bll, -645, c_dv, -645),
            # 16. TurnoBLL obtiene DNI del usuario de sesión (Línea 390)
            (16, id_bll,   id_sess, "ObtenerDniUsuarioActual()", "SynchCall", c_bll, -685, c_sess, -685),
            # 17. ServicesSessionManager devuelve dniActual
            (17, id_sess,  id_bll,  "", "Return", c_sess, -720, c_bll, -720),
            # 18. TurnoBLL registra evento en BitacoraBLL (Línea 391) - NO RETURN
            (18, id_bll,   id_bit,  "RegistrarEvento(2, 'Turno cancelado con éxito', dniActual, 'TurneroNutricional')", "SynchCall", c_bll, -760, c_bit, -760),
            # 19. TurnoBLL retorna true a la UI (Línea 395)
            (19, id_bll,   id_ui,   "", "Return", c_bll, -800, c_ui, -800),
            # 20. UI muestra mensaje de éxito al Nutricionista (Línea 244)
            (20, id_ui,    id_nutri,"MostrarMensajeExito('Turno cancelado con éxito')", "SynchCall", c_ui, -840, c_nutri, -840)
        ]

        for seq, s_id, r_id, m_name, stype, sx, sy, ex, ey in messages:
            sender_el = ea.GetElementByID(s_id)
            conn = sender_el.Connectors.AddNew(m_name, "Sequence")
            conn.SupplierID = r_id
            conn.SubType = stype
            conn.Update()

            escaped_name = m_name.replace("'", "''") if m_name else ""
            sql = f"UPDATE t_connector SET DiagramID = 58, SeqNo = {seq}, Name = '{escaped_name}', PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey} WHERE Connector_ID = {conn.ConnectorID}"
            ea.Execute(sql)

        diag.Update()

        # Export image
        out_img = r"c:\Users\Danie\Desktop\GIT\TD\scratch\cun05_actividad.png"
        proj = ea.GetProjectInterface()
        proj.PutDiagramImageToFile(diag.DiagramGUID, out_img, 1)
        print("Successfully updated CUN05 and exported image to", out_img)

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    build_cun05()
