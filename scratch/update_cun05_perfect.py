import win32com.client

def update_cun05():
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

    diag = ea.GetDiagramByID(58)
    diag.Name = "CUN05:Diagrama Actividad"
    diag.Update()

    # Clear connectors for Diagram 58
    ea.Execute("DELETE FROM t_connector WHERE DiagramID = 58")

    # Objects in Diagram 58:
    # 693: Nutricionista (Actor)
    # 694: FormTurnero_DNI101
    # 695: TurnoBLL_DNI101
    # 696: TurnoBE_DNI101
    # 697: TurnoCanceladoState_DNI101
    # 698: DigitoVerificadorBLL
    # 699: EventoBLL
    # 700: DAL

    lifeline_bottom = -1450
    coords = {
        693: (30, 130, -50, lifeline_bottom, 1),      # center: 80
        694: (170, 290, -50, lifeline_bottom, 2),     # center: 230
        695: (350, 470, -50, lifeline_bottom, 3),     # center: 410
        696: (520, 640, -50, lifeline_bottom, 4),     # center: 580
        697: (690, 870, -50, lifeline_bottom, 5),     # center: 780
        698: (920, 1040, -50, lifeline_bottom, 6),    # center: 980
        699: (1080, 1180, -50, lifeline_bottom, 7),   # center: 1130
        700: (1220, 1320, -50, lifeline_bottom, 8)    # center: 1270
    }

    for d_obj in diag.DiagramObjects:
        if d_obj.ElementID in coords:
            l, r, t, b, seq = coords[d_obj.ElementID]
            d_obj.left = l
            d_obj.right = r
            d_obj.top = t
            d_obj.bottom = b
            d_obj.Sequence = seq
            d_obj.Update()

    diag.DiagramObjects.Refresh()
    diag.Update()

    messages = [
        # 1. Nutricionista hace clic en Cancelar Turno
        (1,  693, 694, "btn_Cancelar_Turno_Click()", "SynchCall", 80, -95, 230, -95),
        # 2. UI obtiene turno de la fila seleccionada en dgvTurnos
        (2,  694, 694, "ObtenerTurnoSeleccionado()", "SynchCall", 230, -130, 260, -150),
        # 3. UI solicita confirmación al usuario
        (3,  694, 693, "SolicitarConfirmacion(codigoTurno)", "SynchCall", 230, -185, 80, -185),
        # 4. Nutricionista confirma
        (4,  693, 694, "Confirmar(DialogResult.Yes)", "SynchCall", 80, -220, 230, -220),
        # 5. UI llama a CancelarTurno en BLL
        (5,  694, 695, "CancelarTurno(codigoTurno)", "SynchCall", 230, -260, 410, -260),
        # 6. BLL recupera el turno por código en DAL (Paso 6)
        (6,  695, 700, "ObtenerPorCodigo(codigoTurno)", "SynchCall", 410, -300, 1270, -300),
        (7,  700, 695, "", "Return", 1270, -335, 410, -335),
        # 7. Delegación al patrón State (Paso 6 y 7)
        (8,  695, 696, "Cancelar(motivo)", "SynchCall", 410, -375, 580, -375),
        (9,  696, 697, "Cancelar(this, motivo)", "SynchCall", 580, -410, 780, -410),
        (10, 697, 696, "", "Return", 780, -445, 580, -445),
        (11, 696, 695, "", "Return", 580, -480, 410, -480),
        # 8. BLL calcula DV horizontal individual
        (12, 695, 695, "CalcularDV(cadenaDV)", "SynchCall", 410, -515, 440, -535),
        # 9. BLL persiste la cancelación en DAL con el nuevo DV
        (13, 695, 700, "CancelarTurno(idTurno, motivo, dv)", "SynchCall", 410, -575, 1270, -575),
        (14, 700, 695, "", "Return", 1270, -610, 410, -610),
        # 10. BLL libera el bloque horario asignado en agenda (Paso 8)
        (15, 695, 700, "ActualizarEstadoBloque(idBloque, 'Disponible')", "SynchCall", 410, -650, 1270, -650),
        (16, 700, 695, "", "Return", 1270, -685, 410, -685),
        # 11. BLL recalcula Dígitos Verificadores globales (Paso 10) - NO RETURN
        (17, 695, 698, "RecalcularYPersistir()", "SynchCall", 410, -725, 980, -725),
        (18, 698, 700, "ActualizarDV(firmas)", "SynchCall", 980, -760, 1270, -760),
        # 12. BLL registra evento en Bitácora (Paso 11) - NO RETURN
        (19, 695, 699, "RegistrarEvento(2, 'Turno Cancelado', dniActual, 'TurneroNutricional')", "SynchCall", 410, -800, 1130, -800),
        (20, 699, 700, "GuardarBitacora(evento)", "SynchCall", 1130, -835, 1270, -835),
        # 13. Retorno a la UI
        (21, 695, 694, "", "Return", 410, -875, 230, -875),
        # 14. UI refresca grilla y muestra confirmación (Paso 12)
        (22, 694, 694, "CargarTurnos()", "SynchCall", 230, -915, 260, -935),
        (23, 694, 693, "MostrarMensajeExito('Turno cancelado exitosamente')", "SynchCall", 230, -975, 80, -975)
    ]

    for seq, s_id, r_id, m_name, stype, sx, sy, ex, ey in messages:
        sender_el = ea.GetElementByID(s_id)
        conn = sender_el.Connectors.AddNew(m_name, "Sequence")
        conn.SupplierID = r_id
        conn.SubType = stype
        conn.Update()
        
        # Escape single quotes in m_name for SQL update
        escaped_name = m_name.replace("'", "''") if m_name else ""
        sql = f"UPDATE t_connector SET DiagramID = 58, SeqNo = {seq}, Name = '{escaped_name}', PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey} WHERE Connector_ID = {conn.ConnectorID}"
        ea.Execute(sql)

    diag.Update()
    
    # Export image snapshot
    img_path = r"C:\Users\Danie\Desktop\GIT\TD\scratch\cun05_actividad.png"
    ea.GetProjectInterface().PutDiagramImageToFile(diag.DiagramGUID, img_path, 1)
    print(f"Exported updated image to {img_path}")

    ea.CloseFile()
    ea.Exit()
    print("CUN05 diagram successfully updated and synchronized!")

if __name__ == '__main__':
    update_cun05()
