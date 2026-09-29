import win32com.client

def test_clean_cun02():
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

    diag = ea.GetDiagramByID(13)
    diag.Name = 'CUN02: Reprogramar Turno'
    diag.Update()

    # Clear old connectors
    ea.Execute('DELETE FROM t_connector WHERE DiagramID = 13')

    lifeline_bottom = -1620

    # Layout coordinates:
    # 266: Nutricionista       X: 30..130    (center 80)
    # 267: FormTurnero_DNI101  X: 170..290   (center 230)
    # 269: AgendaMedicaBLL     X: 330..470   (center 400)
    # 268: TurnoBLL_DNI101     X: 510..630   (center 570)
    # 270: TurnoBE_DNI101      X: 670..790   (center 730)
    # 271: TurnoConfirmadoSt   X: 830..1010  (center 920)
    # 273: DigitoVerificador   X: 1050..1170 (center 1110)
    # 274: EventoBLL           X: 1210..1310 (center 1260)
    # 272: DAL                 X: 1350..1450 (center 1400)

    # Let's see: remove element 276 from DiagramObjects of Diagram 13 if not used, or hide it
    # We can remove 276 from diagram objects:
    to_delete = []
    for i, do in enumerate(diag.DiagramObjects):
        if do.ElementID == 276:
            to_delete.append(i)
    for idx in reversed(to_delete):
        diag.DiagramObjects.Delete(idx)
    diag.DiagramObjects.Refresh()

    coords = {
        266: (30, 130, -50, lifeline_bottom, 2),
        267: (170, 290, -50, lifeline_bottom, 3),
        269: (330, 470, -50, lifeline_bottom, 4),
        268: (510, 630, -50, lifeline_bottom, 5),
        270: (670, 790, -50, lifeline_bottom, 6),
        271: (830, 1010, -50, lifeline_bottom, 7),
        273: (1050, 1170, -50, lifeline_bottom, 8),
        274: (1210, 1310, -50, lifeline_bottom, 9),
        272: (1350, 1450, -50, lifeline_bottom, 10)
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

    # Messages definition:
    # (seq, s_id, r_id, name, type, sx, sy, ex, ey)
    messages = [
        # --- Fase 1: Selección y Recuperación del Turno (Paso 2 y 3) ---
        (1,  266, 267, 'SeleccionarTurno(idTurno)', 'SynchCall', 80, -95, 230, -95),
        (2,  267, 268, 'ObtenerTurnoPorId(idTurno)', 'SynchCall', 230, -135, 570, -135),
        (3,  268, 272, 'ObtenerPorId(idTurno)', 'SynchCall', 570, -175, 1400, -175),
        (4,  272, 268, '', 'Return', 1400, -210, 570, -210),
        (5,  268, 270, 'CambiarEstado(TurnoConfirmadoState)', 'SynchCall', 570, -245, 730, -245),
        (6,  270, 268, '', 'Return', 730, -280, 570, -280),
        (7,  268, 267, '', 'Return', 570, -315, 230, -315),
        (8,  267, 267, 'MostrarDetallesTurno()', 'SynchCall', 230, -350, 260, -370),

        # --- Fase 2: Consulta de Disponibilidad / CU07 (Paso 4 y 5) ---
        (9,  266, 267, 'SeleccionarNuevaFecha(nuevaFecha, dniNutricionista)', 'SynchCall', 80, -420, 230, -420),
        (10, 267, 269, 'ListarBloquesDisponibles(nuevaFecha, dniNutricionista)', 'SynchCall', 230, -460, 400, -460),
        (11, 269, 272, 'ListarBloquesDisponibles(nuevaFecha, dniNutricionista)', 'SynchCall', 400, -500, 1400, -500),
        (12, 272, 269, '', 'Return', 1400, -535, 400, -535),
        (13, 269, 267, '', 'Return', 400, -570, 230, -570),
        (14, 267, 267, 'CargarSelectorHorariosDisponibles()', 'SynchCall', 230, -605, 260, -625),

        # --- Fase 3: Petición de Reprogramación y Patrón State (Paso 6 y 7) ---
        (15, 266, 267, 'ReprogramarTurno(idTurno, nuevaFecha, nuevaHora, nuevoIdBloque)', 'SynchCall', 80, -675, 230, -675),
        (16, 267, 268, 'ReprogramarTurno(idTurno, nuevaFecha, nuevaHora, nuevoIdBloque)', 'SynchCall', 230, -715, 570, -715),
        (17, 268, 270, 'Reprogramar(nuevaFecha, nuevaHora, nuevoIdBloque)', 'SynchCall', 570, -755, 730, -755),
        (18, 270, 271, 'Reprogramar(turno, nuevaFecha, nuevaHora, nuevoIdBloque)', 'SynchCall', 730, -795, 920, -795),
        (19, 271, 270, '', 'Return', 920, -835, 730, -835),
        (20, 270, 268, '', 'Return', 730, -870, 570, -870),

        # --- Fase 4: Persistencia y Actualización de Bloques en DAL (Paso 8) ---
        (21, 268, 272, 'ReprogramarTurno(idTurno, nuevaFecha, nuevaHora, nuevoIdBloque)', 'SynchCall', 570, -920, 1400, -920),
        (22, 272, 268, '', 'Return', 1400, -955, 570, -955),
        (23, 268, 272, "ActualizarEstadoBloque(idBloqueAnterior, 'Disponible')", 'SynchCall', 570, -995, 1400, -995),
        (24, 272, 268, '', 'Return', 1400, -1030, 570, -1030),
        (25, 268, 272, "ActualizarEstadoBloque(nuevoIdBloque, 'Ocupado')", 'SynchCall', 570, -1070, 1400, -1070),
        (26, 272, 268, '', 'Return', 1400, -1105, 570, -1105),

        # --- Fase 5: Dígitos Verificadores (Paso 9) - SIN RETORNO ---
        (27, 268, 268, 'CalcularDVHorizontal(turnoBE)', 'SynchCall', 570, -1145, 600, -1165),
        (28, 268, 273, 'RecalcularYPersistir()', 'SynchCall', 570, -1210, 1110, -1210),
        (29, 273, 272, 'ActualizarDV(firmas)', 'SynchCall', 1110, -1250, 1400, -1250),

        # --- Fase 6: Bitácora de Auditoría (Paso 9) - SIN RETORNO ---
        (30, 268, 274, "RegistrarEvento('Turnos', 'Reprogramacion', dniNutricionista, 'Medio')", 'SynchCall', 570, -1295, 1260, -1295),
        (31, 274, 272, 'GuardarBitacora(evento)', 'SynchCall', 1260, -1335, 1400, -1335),

        # --- Fase 7: Confirmación y Actualización UI (Paso 10) ---
        (32, 268, 267, '', 'Return', 570, -1375, 230, -1375),
        (33, 267, 267, 'ActualizarGrillaTurnos()', 'SynchCall', 230, -1415, 260, -1435),
        (34, 267, 266, "MostrarMensajeConfirmacion('Turno reprogramado exitosamente')", 'SynchCall', 230, -1480, 80, -1480)
    ]

    for seq, s_id, r_id, m_name, stype, sx, sy, ex, ey in messages:
        sender_el = ea.GetElementByID(s_id)
        conn = sender_el.Connectors.AddNew(m_name, 'Sequence')
        conn.SupplierID = r_id
        conn.SubType = stype
        conn.Update()

        sql = f"UPDATE t_connector SET DiagramID = 13, SeqNo = {seq}, PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey} WHERE Connector_ID = {conn.ConnectorID}"
        ea.Execute(sql)

    diag.Update()
    out_img = r'C:\Users\Danie\Desktop\GIT\TD\scratch\cun02_sequence_clean.png'
    ea.GetProjectInterface().PutDiagramImageToFile(diag.DiagramGUID, out_img, 1)

    ea.CloseFile()
    ea.Exit()
    print('Executed test_clean_cun02 successfully')

if __name__ == '__main__':
    test_clean_cun02()
