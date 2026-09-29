import win32com.client
import os

def build_cun04():
    ea = win32com.client.Dispatch('EA.Repository')
    try:
        ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
        diag = ea.GetDiagramByID(57)
        diag.Name = "CUN04:Diagrama Actividad"
        diag.Update()
        
        pkg = ea.GetPackageByID(4)

        # Clear existing connectors for Diagram 57
        ea.Execute("DELETE FROM t_connector WHERE DiagramID = 57")

        # 684: Nutricionista (Actor)
        # 685: FormTurnero_DNI101 -> rename to frmTurnero_DNI101
        el_ui = ea.GetElementByID(685)
        el_ui.Name = "frmTurnero_DNI101"
        el_ui.Update()

        # 686: frmModificarTurno_DNI101
        el_mod = ea.GetElementByID(686)
        el_mod.Name = "frmModificarTurno_DNI101"
        el_mod.Update()

        # 687: TurnoBLL_DNI101
        el_bll = ea.GetElementByID(687)
        el_bll.Name = "TurnoBLL_DNI101"
        el_bll.Update()

        # 692: DAL -> TurnoDAL_DNI101
        el_tdal = ea.GetElementByID(692)
        el_tdal.Name = "TurnoDAL_DNI101"
        el_tdal.Update()

        # 688: TurnoBE_DNI101
        el_be = ea.GetElementByID(688)
        el_be.Name = "TurnoBE_DNI101"
        el_be.Update()

        # 689: IEstadoTurno_DNI101
        el_state = ea.GetElementByID(689)
        el_state.Name = "IEstadoTurno_DNI101"
        el_state.Update()

        # 690: DigitoVerificadorBLL
        el_dv = ea.GetElementByID(690)
        el_dv.Name = "DigitoVerificadorBLL"
        el_dv.Update()

        # 691: EventoBLL -> BitacoraBLL
        el_bit = ea.GetElementByID(691)
        el_bit.Name = "BitacoraBLL"
        el_bit.Update()

        # Check or create AgendaMedicaDAL_DNI101 for CUN04
        amdal_id = None
        for el in pkg.Elements:
            if el.Name == "AgendaMedicaDAL_DNI101" and el.Type == "Object":
                amdal_id = el.ElementID
                break
        
        if not amdal_id:
            el_amdal = pkg.Elements.AddNew("AgendaMedicaDAL_DNI101", "Object")
            el_amdal.Update()
            pkg.Elements.Refresh()
            amdal_id = el_amdal.ElementID

        # Ensure AgendaMedicaDAL_DNI101 is in Diagram 57
        found_amdal = False
        for dobj in diag.DiagramObjects:
            if dobj.ElementID == amdal_id:
                found_amdal = True
                break
        if not found_amdal:
            dobj = diag.DiagramObjects.AddNew("l=0;r=0;t=0;b=0;", "")
            dobj.ElementID = amdal_id
            dobj.Update()
            diag.DiagramObjects.Refresh()

        # Check or create ServicesSessionManager for CUN04
        sess_id = None
        for el in pkg.Elements:
            if el.Name == "ServicesSessionManager" and el.Type == "Object":
                sess_id = el.ElementID
                break
        
        if not sess_id:
            el_sess = pkg.Elements.AddNew("ServicesSessionManager", "Object")
            el_sess.Update()
            pkg.Elements.Refresh()
            sess_id = el_sess.ElementID

        # Ensure ServicesSessionManager is in Diagram 57
        found_sess = False
        for dobj in diag.DiagramObjects:
            if dobj.ElementID == sess_id:
                found_sess = True
                break
        if not found_sess:
            dobj = diag.DiagramObjects.AddNew("l=0;r=0;t=0;b=0;", "")
            dobj.ElementID = sess_id
            dobj.Update()
            diag.DiagramObjects.Refresh()

        # Coordinates (11 lifelines)
        lifeline_bottom = -1350
        coords = {
            684:      (15,   105,  -50, lifeline_bottom, 1, 60),
            685:      (130,  250,  -50, lifeline_bottom, 2, 190),
            686:      (265,  415,  -50, lifeline_bottom, 3, 340),
            687:      (430,  550,  -50, lifeline_bottom, 4, 490),
            692:      (570,  690,  -50, lifeline_bottom, 5, 630),
            688:      (710,  830,  -50, lifeline_bottom, 6, 770),
            689:      (845,  975,  -50, lifeline_bottom, 7, 910),
            amdal_id: (980,  1140, -50, lifeline_bottom, 8, 1060),
            690:      (1145, 1275, -50, lifeline_bottom, 9, 1210),
            sess_id:  (1285, 1445, -50, lifeline_bottom, 10, 1365),
            691:      (1455, 1575, -50, lifeline_bottom, 11, 1515),
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

        c_nutri = 60
        c_ui = 190
        c_mod = 340
        c_bll = 490
        c_tdal = 630
        c_be = 770
        c_state = 910
        c_amdal = 1060
        c_dv = 1210
        c_sess = 1365
        c_bit = 1515

        id_nutri = 684
        id_ui = 685
        id_mod = 686
        id_bll = 687
        id_tdal = 692
        id_be = 688
        id_state = 689
        id_amdal = amdal_id
        id_dv = 690
        id_sess = sess_id
        id_bit = 691

        messages = [
            # 1. Nutricionista hace clic en Modificar Turno
            (1,  id_nutri, id_ui,   "btn_Modificar_Turno_Click()", "SynchCall", c_nutri, -90, c_ui, -90),
            # 2. UI obtiene turno de la fila seleccionada
            (2,  id_ui,    id_ui,   "ObtenerTurnoSeleccionado()", "SynchCall", c_ui, -125, c_ui + 30, -145),
            # 3. UI abre ventana modal pasando el turno seleccionado (Línea 183-185)
            (3,  id_ui,    id_mod,  "ShowDialog(turnoSeleccionado)", "SynchCall", c_ui, -180, c_mod, -180),
            # 4. Nutricionista modifica campos y hace clic en Guardar (Línea 144)
            (4,  id_nutri, id_mod,  "btnGuardar_Click(nuevoMotivo, nuevoEstado)", "SynchCall", c_nutri, -215, c_mod, -215),
            # 5. frmModificarTurno toma código y datos de turno (Líneas 152-154)
            (5,  id_mod,   id_mod,  "ObtenerDatosTurno()", "SynchCall", c_mod, -250, c_mod + 30, -270),
            # 6. frmModificarTurno solicita confirmación (Línea 171)
            (6,  id_mod,   id_nutri,"SolicitarConfirmacion(codigoTurno)", "SynchCall", c_mod, -305, c_nutri, -305),
            # 7. Nutricionista confirma (Línea 179)
            (7,  id_nutri, id_mod,  "Confirmar(DialogResult.Yes)", "SynchCall", c_nutri, -340, c_mod, -340),
            # 8. frmModificarTurno llama a TurnoBLL (Línea 186)
            (8,  id_mod,   id_bll,  "ModificarTurno(codigoTurno, nuevoMotivo, nuevoEstado)", "SynchCall", c_mod, -380, c_bll, -380),
            # 9. TurnoBLL recupera turno de TurnoDAL (Línea 243)
            (9,  id_bll,   id_tdal, "ObtenerPorCodigo(codigoTurno)", "SynchCall", c_bll, -420, c_tdal, -420),
            # 10. TurnoDAL retorna turnoBE
            (10, id_tdal,  id_bll,  "", "Return", c_tdal, -455, c_bll, -455),
            # 11. TurnoBLL delega cambio de estado a TurnoBE (Líneas 274-286)
            (11, id_bll,   id_be,   "CambiarEstado(nuevoEstado, nuevoMotivo)", "SynchCall", c_bll, -495, c_be, -495),
            # 12. TurnoBE delega en patrón State IEstadoTurno
            (12, id_be,    id_state,"CambiarEstado(this)", "SynchCall", c_be, -530, c_state, -530),
            # 13. Retorno de IEstadoTurno
            (13, id_state, id_be,   "", "Return", c_state, -565, c_be, -565),
            # 14. Retorno de TurnoBE a TurnoBLL
            (14, id_be,    id_bll,  "", "Return", c_be, -600, c_bll, -600),
            # 15. TurnoBLL calcula DV individual (Línea 292)
            (15, id_bll,   id_bll,  "CalcularDV(cadenaDV)", "SynchCall", c_bll, -635, c_bll + 30, -655),
            # 16. TurnoBLL persiste cambios en TurnoDAL (Línea 295)
            (16, id_bll,   id_tdal, "ModificarTurnoEstadoYMotivo(idTurno, motivo, estado, dv)", "SynchCall", c_bll, -690, c_tdal, -690),
            # 17. TurnoDAL confirma persistencia
            (17, id_tdal,  id_bll,  "", "Return", c_tdal, -725, c_bll, -725),
            # 18. [opt: nuevoEstado == 'Cancelado'] TurnoBLL libera bloque horario en AgendaMedicaDAL (Línea 306)
            (18, id_bll,   id_amdal,"[opt: Cancelado] ActualizarEstadoBloque(idBloque, 'Disponible')", "SynchCall", c_bll, -765, c_amdal, -765),
            # 19. AgendaMedicaDAL confirma liberación de bloque (ida y vuelta)
            (19, id_amdal, id_bll,  "", "Return", c_amdal, -800, c_bll, -800),
            # 20. TurnoBLL recalcula Dígitos Verificadores globales (Línea 314) - NO RETURN
            (20, id_bll,   id_dv,   "RecalcularYPersistir()", "SynchCall", c_bll, -840, c_dv, -840),
            # 21. TurnoBLL obtiene DNI del usuario de sesión (Línea 321)
            (21, id_bll,   id_sess, "ObtenerDniUsuarioActual()", "SynchCall", c_bll, -880, c_sess, -880),
            # 22. ServicesSessionManager retorna dniActual
            (22, id_sess,  id_bll,  "", "Return", c_sess, -915, c_bll, -915),
            # 23. TurnoBLL registra evento en BitacoraBLL (Línea 322) - NO RETURN
            (23, id_bll,   id_bit,  "RegistrarEvento(2, 'Turno modificado con éxito', dniActual, 'TurneroNutricional')", "SynchCall", c_bll, -955, c_bit, -955),
            # 24. TurnoBLL retorna true a frmModificarTurno (Línea 326)
            (24, id_bll,   id_mod,  "", "Return", c_bll, -995, c_mod, -995),
            # 25. frmModificarTurno muestra mensaje de éxito al Nutricionista (Línea 191)
            (25, id_mod,   id_nutri,"MostrarMensajeExito('Turno modificado exitosamente')", "SynchCall", c_mod, -1045, c_nutri, -1045),
            # 26. frmModificarTurno cierra y retorna DialogResult.OK a frmTurnero (Líneas 199-200)
            (26, id_mod,   id_ui,   "", "Return", c_mod, -1095, c_ui, -1095),
            # 27. frmTurnero refresca la grilla de turnos (Línea 189)
            (27, id_ui,    id_ui,   "CargarTurnos()", "SynchCall", c_ui, -1145, c_ui + 30, -1165)
        ]

        for seq, s_id, r_id, m_name, stype, sx, sy, ex, ey in messages:
            sender_el = ea.GetElementByID(s_id)
            conn = sender_el.Connectors.AddNew(m_name, "Sequence")
            conn.SupplierID = r_id
            conn.SubType = stype
            conn.Update()

            escaped_name = m_name.replace("'", "''") if m_name else ""
            sql = f"UPDATE t_connector SET DiagramID = 57, SeqNo = {seq}, Name = '{escaped_name}', PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey} WHERE Connector_ID = {conn.ConnectorID}"
            ea.Execute(sql)

        diag.Update()

        # Export image
        out_img = r"c:\Users\Danie\Desktop\GIT\TD\scratch\cun04_actividad.png"
        proj = ea.GetProjectInterface()
        proj.PutDiagramImageToFile(diag.DiagramGUID, out_img, 1)
        print("Successfully updated CUN04 and exported image to", out_img)

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    build_cun04()
