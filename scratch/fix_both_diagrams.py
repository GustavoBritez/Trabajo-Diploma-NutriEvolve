import win32com.client
import os

def fix_both_diagrams():
    ea = win32com.client.Dispatch('EA.Repository')
    try:
        ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
        proj = ea.GetProjectInterface()
        pkg = ea.GetPackageByID(4)

        # =========================================================================
        # 1. FIX DIAGRAM 58 - CUN05: Cancelar Turno
        # =========================================================================
        diag58 = ea.GetDiagramByID(58)
        diag58.Name = "CUN05: Diagrama de Secuencia"
        diag58.Update()

        # Delete connectors for Diagram 58
        ea.Execute("DELETE FROM t_connector WHERE DiagramID = 58")

        # Elements for CUN05:
        # 693: Nutricionista (Actor)
        # 694: frmTurnero_DNI101
        # 695: TurnoBLL_DNI101
        # 700: TurnoDAL_DNI101
        # 696: TurnoBE_DNI101
        # 697: AgendaMedicaDAL_DNI101
        # 698: DigitoVerificadorBLL
        # 701: ServicesSessionManager
        # 699: BitacoraBLL

        el_ui58 = ea.GetElementByID(694)
        el_ui58.Name = "frmTurnero_DNI101"
        el_ui58.Update()

        el_bll58 = ea.GetElementByID(695)
        el_bll58.Name = "TurnoBLL_DNI101"
        el_bll58.Update()

        el_tdal58 = ea.GetElementByID(700)
        el_tdal58.Name = "TurnoDAL_DNI101"
        el_tdal58.Update()

        el_amdal58 = ea.GetElementByID(697)
        el_amdal58.Name = "AgendaMedicaDAL_DNI101"
        el_amdal58.Update()

        el_bit58 = ea.GetElementByID(699)
        el_bit58.Name = "BitacoraBLL"
        el_bit58.Update()

        # Ensure correct DiagramObjects in Diagram 58
        cun05_element_ids = [693, 694, 695, 700, 696, 697, 698, 701, 699]
        
        # Remove any unwanted objects (like 704, 705, 706)
        for i in range(diag58.DiagramObjects.Count - 1, -1, -1):
            dobj = diag58.DiagramObjects.GetAt(i)
            if dobj.ElementID not in cun05_element_ids:
                diag58.DiagramObjects.Delete(i)
        diag58.DiagramObjects.Refresh()

        # Ensure all required elements are present in Diagram 58
        existing_in_58 = [diag58.DiagramObjects.GetAt(i).ElementID for i in range(diag58.DiagramObjects.Count)]
        for eid in cun05_element_ids:
            if eid not in existing_in_58:
                dobj = diag58.DiagramObjects.AddNew("l=0;r=0;t=0;b=0;", "")
                dobj.ElementID = eid
                dobj.Update()
        diag58.DiagramObjects.Refresh()

        # Layout coordinates for CUN05
        # Centers:
        # 1. 693: Nutricionista (70)
        # 2. 694: frmTurnero_DNI101 (220)
        # 3. 695: TurnoBLL_DNI101 (390)
        # 4. 700: TurnoDAL_DNI101 (550)
        # 5. 696: TurnoBE_DNI101 (705)
        # 6. 697: AgendaMedicaDAL_DNI101 (875)
        # 7. 698: DigitoVerificadorBLL (1055)
        # 8. 701: ServicesSessionManager (1235)
        # 9. 699: BitacoraBLL (1405)

        lifeline_bottom58 = -1150
        coords58 = {
            693: (20,   120,  -50, lifeline_bottom58, 1),
            694: (155,  285,  -50, lifeline_bottom58, 2),
            695: (325,  455,  -50, lifeline_bottom58, 3),
            700: (485,  615,  -50, lifeline_bottom58, 4),
            696: (645,  765,  -50, lifeline_bottom58, 5),
            697: (795,  955,  -50, lifeline_bottom58, 6),
            698: (985,  1125, -50, lifeline_bottom58, 7),
            701: (1155, 1315, -50, lifeline_bottom58, 8),
            699: (1345, 1465, -50, lifeline_bottom58, 9),
        }

        for dobj in diag58.DiagramObjects:
            if dobj.ElementID in coords58:
                l, r, t, b, seq = coords58[dobj.ElementID]
                dobj.left = l
                dobj.right = r
                dobj.top = t
                dobj.bottom = b
                dobj.Sequence = seq
                dobj.Update()

        diag58.DiagramObjects.Refresh()
        diag58.Update()

        c58_nutri = 70
        c58_ui = 220
        c58_bll = 390
        c58_tdal = 550
        c58_be = 705
        c58_amdal = 875
        c58_dv = 1055
        c58_sess = 1235
        c58_bit = 1405

        messages58 = [
            # 1. Clic en Cancelar Turno
            (1,  693, 694, "btn_Cancelar_Turno_Click()", "SynchCall", c58_nutri, -90, c58_ui, -90),
            # 2. UI obtiene turno de la fila seleccionada
            (2,  694, 694, "ObtenerTurnoSeleccionado()", "SynchCall", c58_ui, -125, c58_ui + 30, -145),
            # 3. UI solicita confirmación
            (3,  694, 693, "SolicitarConfirmacion(codigoTurno)", "SynchCall", c58_ui, -180, c58_nutri, -180),
            # 4. Confirmación del usuario
            (4,  693, 694, "Confirmar(DialogResult.Yes)", "SynchCall", c58_nutri, -215, c58_ui, -215),
            # 5. UI llama a TurnoBLL (Línea 241)
            (5,  694, 695, "CancelarTurno(codigoTurno)", "SynchCall", c58_ui, -255, c58_bll, -255),
            # 6. TurnoBLL llama a TurnoDAL (Línea 338)
            (6,  695, 700, "ObtenerPorCodigo(codigoTurno)", "SynchCall", c58_bll, -295, c58_tdal, -295),
            # 7. TurnoDAL devuelve TurnoBE
            (7,  700, 695, "", "Return", c58_tdal, -330, c58_bll, -330),
            # 8. TurnoBLL delega cancelación a TurnoBE (Línea 357)
            (8,  695, 696, "Cancelar(motivo)", "SynchCall", c58_bll, -370, c58_be, -370),
            # 9. Retorno de TurnoBE
            (9,  696, 695, "", "Return", c58_be, -405, c58_bll, -405),
            # 10. TurnoBLL calcula DV individual (Línea 361)
            (10, 695, 695, "CalcularDV(cadenaDV)", "SynchCall", c58_bll, -440, c58_bll + 30, -460),
            # 11. TurnoBLL persiste cancelación en TurnoDAL (Línea 364)
            (11, 695, 700, "CancelarTurno(idTurno, motivo, dv)", "SynchCall", c58_bll, -495, c58_tdal, -495),
            # 12. TurnoDAL confirma persistencia
            (12, 700, 695, "", "Return", c58_tdal, -530, c58_bll, -530),
            # 13. TurnoBLL libera bloque horario en AgendaMedicaDAL (Línea 375)
            (13, 695, 697, "ActualizarEstadoBloque(idBloque, 'Disponible')", "SynchCall", c58_bll, -570, c58_amdal, -570),
            # 14. AgendaMedicaDAL confirma liberación de bloque
            (14, 697, 695, "", "Return", c58_amdal, -605, c58_bll, -605),
            # 15. TurnoBLL recalcula Dígitos Verificadores globales (Línea 383) - NO RETURN
            (15, 695, 698, "RecalcularYPersistir()", "SynchCall", c58_bll, -645, c58_dv, -645),
            # 16. TurnoBLL obtiene DNI del usuario de sesión (Línea 390)
            (16, 695, 701, "ObtenerDniUsuarioActual()", "SynchCall", c58_bll, -685, c58_sess, -685),
            # 17. ServicesSessionManager devuelve dniActual
            (17, 701, 695, "", "Return", c58_sess, -720, c58_bll, -720),
            # 18. TurnoBLL registra evento en BitacoraBLL (Línea 391) - NO RETURN
            (18, 695, 699, "RegistrarEvento(2, 'Turno cancelado con éxito', dniActual, 'TurneroNutricional')", "SynchCall", c58_bll, -760, c58_bit, -760),
            # 19. TurnoBLL retorna true a la UI (Línea 395)
            (19, 695, 694, "", "Return", c58_bll, -800, c58_ui, -800),
            # 20. UI muestra mensaje de éxito al Nutricionista (Línea 244)
            (20, 694, 693, "MostrarMensajeExito('Turno cancelado con éxito')", "SynchCall", c58_ui, -840, c58_nutri, -840)
        ]

        for seq, s_id, r_id, m_name, stype, sx, sy, ex, ey in messages58:
            sender_el = ea.GetElementByID(s_id)
            conn = sender_el.Connectors.AddNew(m_name, "Sequence")
            conn.SupplierID = r_id
            conn.SubType = stype
            conn.Update()

            escaped_name = m_name.replace("'", "''") if m_name else ""
            sql = f"UPDATE t_connector SET DiagramID = 58, SeqNo = {seq}, Name = '{escaped_name}', PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey} WHERE Connector_ID = {conn.ConnectorID}"
            ea.Execute(sql)

        diag58.Update()
        out58 = r"c:\Users\Danie\Desktop\GIT\TD\scratch\cun05_secuencia.png"
        proj.PutDiagramImageToFile(diag58.DiagramGUID, out58, 1)
        print("Diagram 58 (CUN05) cleanly generated and exported to", out58)

        # =========================================================================
        # 2. VERIFY / FIX DIAGRAM 57 - CUN04: Modificar Turno
        # =========================================================================
        diag57 = ea.GetDiagramByID(57)
        diag57.Name = "CUN04: Diagrama de Secuencia"
        diag57.Update()

        ea.Execute("DELETE FROM t_connector WHERE DiagramID = 57")

        el_ui57 = ea.GetElementByID(685)
        el_ui57.Name = "frmTurnero_DNI101"
        el_ui57.Update()

        el_mod57 = ea.GetElementByID(686)
        el_mod57.Name = "frmModificarTurno_DNI101"
        el_mod57.Update()

        el_bll57 = ea.GetElementByID(687)
        el_bll57.Name = "TurnoBLL_DNI101"
        el_bll57.Update()

        el_tdal57 = ea.GetElementByID(692)
        el_tdal57.Name = "TurnoDAL_DNI101"
        el_tdal57.Update()

        el_be57 = ea.GetElementByID(688)
        el_be57.Name = "TurnoBE_DNI101"
        el_be57.Update()

        el_state57 = ea.GetElementByID(689)
        el_state57.Name = "IEstadoTurno_DNI101"
        el_state57.Update()

        el_dv57 = ea.GetElementByID(690)
        el_dv57.Name = "DigitoVerificadorBLL"
        el_dv57.Update()

        el_bit57 = ea.GetElementByID(691)
        el_bit57.Name = "BitacoraBLL"
        el_bit57.Update()

        # AgendaMedicaDAL_DNI101
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

        # Ensure AgendaMedicaDAL is in Diagram 57
        found_amdal = False
        for dobj in diag57.DiagramObjects:
            if dobj.ElementID == amdal_id:
                found_amdal = True
                break
        if not found_amdal:
            dobj = diag57.DiagramObjects.AddNew("l=0;r=0;t=0;b=0;", "")
            dobj.ElementID = amdal_id
            dobj.Update()
            diag57.DiagramObjects.Refresh()

        # Ensure ServicesSessionManager (701) is in Diagram 57
        found_sess = False
        for dobj in diag57.DiagramObjects:
            if dobj.ElementID == 701:
                found_sess = True
                break
        if not found_sess:
            dobj = diag57.DiagramObjects.AddNew("l=0;r=0;t=0;b=0;", "")
            dobj.ElementID = 701
            dobj.Update()
            diag57.DiagramObjects.Refresh()

        lifeline_bottom57 = -1350
        coords57 = {
            684:      (15,   105,  -50, lifeline_bottom57, 1),
            685:      (130,  250,  -50, lifeline_bottom57, 2),
            686:      (265,  415,  -50, lifeline_bottom57, 3),
            687:      (430,  550,  -50, lifeline_bottom57, 4),
            692:      (570,  690,  -50, lifeline_bottom57, 5),
            688:      (710,  830,  -50, lifeline_bottom57, 6),
            689:      (845,  975,  -50, lifeline_bottom57, 7),
            amdal_id: (980,  1140, -50, lifeline_bottom57, 8),
            690:      (1145, 1275, -50, lifeline_bottom57, 9),
            701:      (1285, 1445, -50, lifeline_bottom57, 10),
            691:      (1455, 1575, -50, lifeline_bottom57, 11),
        }

        for dobj in diag57.DiagramObjects:
            if dobj.ElementID in coords57:
                l, r, t, b, seq = coords57[dobj.ElementID]
                dobj.left = l
                dobj.right = r
                dobj.top = t
                dobj.bottom = b
                dobj.Sequence = seq
                dobj.Update()

        diag57.DiagramObjects.Refresh()
        diag57.Update()

        c57_nutri = 60
        c57_ui = 190
        c57_mod = 340
        c57_bll = 490
        c57_tdal = 630
        c57_be = 770
        c57_state = 910
        c57_amdal = 1060
        c57_dv = 1210
        c57_sess = 1365
        c57_bit = 1515

        messages57 = [
            (1,  684, 685, "btn_Modificar_Turno_Click()", "SynchCall", c57_nutri, -90, c57_ui, -90),
            (2,  685, 685, "ObtenerTurnoSeleccionado()", "SynchCall", c57_ui, -125, c57_ui + 30, -145),
            (3,  685, 686, "ShowDialog(turnoSeleccionado)", "SynchCall", c57_ui, -180, c57_mod, -180),
            (4,  684, 686, "btnGuardar_Click(nuevoMotivo, nuevoEstado)", "SynchCall", c57_nutri, -215, c57_mod, -215),
            (5,  686, 686, "ObtenerDatosTurno()", "SynchCall", c57_mod, -250, c57_mod + 30, -270),
            (6,  686, 684, "SolicitarConfirmacion(codigoTurno)", "SynchCall", c57_mod, -305, c57_nutri, -305),
            (7,  684, 686, "Confirmar(DialogResult.Yes)", "SynchCall", c57_nutri, -340, c57_mod, -340),
            (8,  686, 687, "ModificarTurno(codigoTurno, nuevoMotivo, nuevoEstado)", "SynchCall", c57_mod, -380, c57_bll, -380),
            (9,  687, 692, "ObtenerPorCodigo(codigoTurno)", "SynchCall", c57_bll, -420, c57_tdal, -420),
            (10, 692, 687, "", "Return", c57_tdal, -455, c57_bll, -455),
            (11, 687, 688, "CambiarEstado(nuevoEstado, nuevoMotivo)", "SynchCall", c57_bll, -495, c57_be, -495),
            (12, 688, 689, "CambiarEstado(this)", "SynchCall", c57_be, -530, c57_state, -530),
            (13, 689, 688, "", "Return", c57_state, -565, c57_be, -565),
            (14, 688, 687, "", "Return", c57_be, -600, c57_bll, -600),
            (15, 687, 687, "CalcularDV(cadenaDV)", "SynchCall", c57_bll, -635, c57_bll + 30, -655),
            (16, 687, 692, "ModificarTurnoEstadoYMotivo(idTurno, motivo, estado, dv)", "SynchCall", c57_bll, -690, c57_tdal, -690),
            (17, 692, 687, "", "Return", c57_tdal, -725, c57_bll, -725),
            (18, 687, amdal_id, "[opt: Cancelado] ActualizarEstadoBloque(idBloque, 'Disponible')", "SynchCall", c57_bll, -765, c57_amdal, -765),
            (19, amdal_id, 687, "", "Return", c57_amdal, -800, c57_bll, -800),
            (20, 687, 690, "RecalcularYPersistir()", "SynchCall", c57_bll, -840, c57_dv, -840),
            (21, 687, 701, "ObtenerDniUsuarioActual()", "SynchCall", c57_bll, -880, c57_sess, -880),
            (22, 701, 687, "", "Return", c57_sess, -915, c57_bll, -915),
            (23, 687, 691, "RegistrarEvento(2, 'Turno modificado con éxito', dniActual, 'TurneroNutricional')", "SynchCall", c57_bll, -955, c57_bit, -955),
            (24, 687, 686, "", "Return", c57_bll, -995, c57_mod, -995),
            (25, 686, 684, "MostrarMensajeExito('Turno modificado exitosamente')", "SynchCall", c57_mod, -1045, c57_nutri, -1045),
            (26, 686, 685, "", "Return", c57_mod, -1095, c57_ui, -1095),
            (27, 685, 685, "CargarTurnos()", "SynchCall", c57_ui, -1145, c57_ui + 30, -1165)
        ]

        for seq, s_id, r_id, m_name, stype, sx, sy, ex, ey in messages57:
            sender_el = ea.GetElementByID(s_id)
            conn = sender_el.Connectors.AddNew(m_name, "Sequence")
            conn.SupplierID = r_id
            conn.SubType = stype
            conn.Update()

            escaped_name = m_name.replace("'", "''") if m_name else ""
            sql = f"UPDATE t_connector SET DiagramID = 57, SeqNo = {seq}, Name = '{escaped_name}', PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey} WHERE Connector_ID = {conn.ConnectorID}"
            ea.Execute(sql)

        diag57.Update()
        out57 = r"c:\Users\Danie\Desktop\GIT\TD\scratch\cun04_secuencia.png"
        proj.PutDiagramImageToFile(diag57.DiagramGUID, out57, 1)
        print("Diagram 57 (CUN04) cleanly generated and exported to", out57)

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    fix_both_diagrams()
