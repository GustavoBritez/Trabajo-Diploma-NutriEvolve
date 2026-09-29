import win32com.client

def update_cun01_masterpiece():
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

    diag = ea.GetDiagramByID(12)
    diag.Name = 'CUN01: Registrar Turno'
    diag.Update()

    # Clean all connectors for DiagramID 12
    ea.Execute('DELETE FROM t_connector WHERE DiagramID = 12')

    # Lifelines
    lifeline_bottom = -1600
    coords = {
        219: (30, 130, -50, lifeline_bottom, 2),
        220: (170, 290, -50, lifeline_bottom, 3),
        221: (330, 460, -50, lifeline_bottom, 4),
        222: (490, 610, -50, lifeline_bottom, 5),
        225: (820, 940, -50, lifeline_bottom, 8),
        226: (970, 1080, -50, lifeline_bottom, 9),
        227: (1110, 1240, -50, lifeline_bottom, 10),
        229: (1270, 1400, -50, lifeline_bottom, 12),
        230: (1430, 1540, -50, lifeline_bottom, 13),
        228: (1570, 1670, -50, lifeline_bottom, 11),
        # Fragment 232: Left=140, Right=790, Top=-530, Bottom=-660
        232: (140, 790, -530, -660, 1),
        # CUN02 oval: Top = -575, Bottom = -635
        38:  (640, 780, -575, -635, 6)
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

    # Messages:
    messages = [
        # 1. Nutricionista triggers RegistrarTurno
        (1,  219, 220, 'RegistrarTurno(DNINiño, Fecha, Horario, Motivo)', 'SynchCall', 80, -95, 230, -95, None),
        
        # 2. Consultar disponibilidad (CUN-07)
        (2,  220, 221, 'ListarBloquesDisponibles(Fecha, dniNutricionista)', 'SynchCall', 230, -135, 395, -135, None),
        (3,  221, 228, 'ListarBloquesDisponibles(Fecha, dniNutricionista)', 'SynchCall', 395, -175, 1620, -175, None),
        (4,  228, 221, '', 'Return', 1620, -210, 395, -210, None),
        (5,  221, 220, '', 'Return', 395, -245, 230, -245, None),
        (6,  220, 220, 'CargarSelectorHorariosDisponibles()', 'SynchCall', 230, -280, 260, -300, None),
        
        # 3. Buscar Paciente
        (7,  220, 222, 'ObtenerPacientePorDNI(DNINiño)', 'SynchCall', 230, -340, 550, -340, None),
        (8,  222, 228, 'ObtenerPacientePorDNI(DNINiño)', 'SynchCall', 550, -380, 1620, -380, None),
        (9,  228, 222, '', 'Return', 1620, -415, 550, -415, None),
        (10, 222, 220, '', 'Return', 550, -450, 230, -450, None),
        
        # 4. Flujo Alternativo: CUN-02 Paciente No Registrado (Punto de Extensión)
        # Step 6.1.1: InformarPacienteNoRegistrado (above box)
        (11, 220, 219, 'InformarPacienteNoRegistrado()', 'SynchCall', 230, -495, 80, -495, None),
        # Step 6.1.2: IngresarDatosPaciente (enters the box from Nutricionista to FormTurnero)
        (12, 219, 220, 'IngresarDatosPaciente(Nombre, Apellido, DNINiño, Telefono, Email, ObraSocial)', 'SynchCall', 80, -565, 230, -565, None),
        # Step 6.1.3: RegistrarPaciente (invokes CUN02 oval inside box)
        (13, 220, 38,  'RegistrarPaciente(Nombre, Apellido, DNINiño, Telefono, Email, ObraSocial)', 'New', 230, -595, 710, -595, None),
        (14, 38,  220, 'PacienteRegistradoOK()', 'Return', 710, -630, 230, -630, None),
        # Lifeline terminates automatically with 'X' right under message 14!
        
        # 5. Validación en UI (Step 9: Outside and below the box)
        (15, 220, 220, 'ValidarCamposObligatorios()', 'SynchCall', 230, -700, 260, -720, None),
        
        # 6. Agendamiento en TurnoBLL
        (16, 220, 225, 'RegistrarTurno(DNINiño, Fecha, Horario, Motivo)', 'SynchCall', 230, -760, 880, -760, None),
        
        # 7. Orquestación BLL y Patrón State (Step 10)
        (17, 225, 226, 'new TurnoBE_DNI101(DNINiño, Fecha, Horario, Motivo)', 'SynchCall', 880, -800, 1025, -800, None),
        (18, 225, 226, 'GenerarCodigoUnico()', 'SynchCall', 880, -840, 1025, -840, None),
        (19, 225, 227, 'new TurnoSolicitadoState()', 'SynchCall', 880, -880, 1175, -880, None),
        (20, 225, 226, 'CambiarEstado(TurnoSolicitadoState)', 'SynchCall', 880, -920, 1025, -920, None),
        (21, 226, 225, '', 'Return', 1025, -960, 880, -960, None),
        
        # 8. Persistencia y actualización de bloque en DAL (Step 11 & 12)
        (22, 225, 228, 'Guardar(turnoBE)', 'SynchCall', 880, -1000, 1620, -1000, None),
        (23, 228, 225, '', 'Return', 1620, -1040, 880, -1040, None),
        (24, 225, 228, "ActualizarEstadoBloque(idBloque, 'Ocupado')", 'SynchCall', 880, -1080, 1620, -1080, None),
        (25, 228, 225, '', 'Return', 1620, -1120, 880, -1120, None),
        
        # 9. Dígitos Verificadores (Step 13) - NO RETURNS
        (26, 225, 225, 'CalcularDVHorizontal(turnoBE)', 'SynchCall', 880, -1160, 910, -1180, None),
        (27, 225, 229, 'RecalcularYPersistir()', 'SynchCall', 880, -1210, 1335, -1210, None),
        (28, 229, 228, 'ActualizarDV(firmas)', 'SynchCall', 1335, -1250, 1620, -1250, None),
        
        # 10. Bitácora de Auditoría (Step 14) - NO RETURNS
        (29, 225, 230, "RegistrarEvento('Turnos', 'Agendamiento', dniNutricionista, 'Medio')", 'SynchCall', 880, -1290, 1485, -1290, None),
        (30, 230, 228, 'GuardarBitacora(evento)', 'SynchCall', 1485, -1330, 1620, -1330, None),
        
        # 11. Retorno y Confirmación al Nutricionista (Step 15)
        (31, 225, 220, '', 'Return', 880, -1370, 230, -1370, None),
        (32, 220, 220, 'ActualizarGrillaTurnos()', 'SynchCall', 230, -1410, 260, -1430, None),
        (33, 220, 219, 'MostrarMensajeExito(codigoTurno)', 'SynchCall', 230, -1470, 80, -1470, None)
    ]

    for seq, s_id, r_id, m_name, stype, sx, sy, ex, ey, style in messages:
        sender_el = ea.GetElementByID(s_id)
        conn = sender_el.Connectors.AddNew(m_name, 'Sequence')
        conn.SupplierID = r_id
        conn.SubType = stype
        conn.Update()
        
        style_val = f"'{style}'" if style else 'NULL'
        sql = f"UPDATE t_connector SET DiagramID = 12, SeqNo = {seq}, PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey}, StyleEx = {style_val} WHERE Connector_ID = {conn.ConnectorID}"
        ea.Execute(sql)

    diag.Update()
    out_img = r'C:\Users\Danie\Desktop\GIT\TD\scratch\cun01_sequence_gold.png'
    ea.GetProjectInterface().PutDiagramImageToFile(diag.DiagramGUID, out_img, 1)

    ea.CloseFile()
    ea.Exit()
    print('Executed gold successfully')

if __name__ == '__main__':
    update_cun01_masterpiece()
