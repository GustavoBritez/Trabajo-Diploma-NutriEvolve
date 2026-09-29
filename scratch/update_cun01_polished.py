import win32com.client

def update_cun01_polished():
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

    diag = ea.GetDiagramByID(12)
    diag.Name = 'CUN01: Registrar Turno'
    diag.Update()

    # Clean all connectors for DiagramID 12
    ea.Execute('DELETE FROM t_connector WHERE DiagramID = 12')

    # Lifelines
    lifeline_bottom = -1660
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
        # Fragment 232: Left=140, Right=790, Top=-525, Bottom=-695
        232: (140, 790, -525, -695, 1),
        # CUN02 oval: Top = -560, Bottom = -615
        38:  (640, 780, -560, -615, 6)
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
        
        # 2. Consultar disponibilidad
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
        (12, 219, 220, 'IngresarDatosPaciente(Nombre, Apellido, DNINiño, Telefono, Email, ObraSocial)', 'SynchCall', 80, -560, 230, -560, None),
        # Step 6.1.3: RegistrarPaciente (invokes CUN02 oval inside box)
        (13, 220, 38,  'RegistrarPaciente(Nombre, Apellido, DNINiño, Telefono, Email, ObraSocial)', 'New', 230, -585, 710, -585, None),
        (14, 38,  220, 'PacienteRegistradoOK()', 'Return', 710, -615, 230, -615, None),
        (15, 220, 38,  '', 'Delete', 230, -630, 710, -630, None),
        
        # 5. Validación en UI (Outside Fragment, clearly below box bottom)
        (16, 220, 220, 'ValidarCamposObligatorios()', 'SynchCall', 230, -760, 260, -780, None),
        
        # 6. Agendamiento en TurnoBLL
        (17, 220, 225, 'RegistrarTurno(DNINiño, Fecha, Horario, Motivo)', 'SynchCall', 230, -820, 880, -820, None),
        
        # 7. Orquestación BLL y Patrón State
        (18, 225, 226, 'new TurnoBE_DNI101(DNINiño, Fecha, Horario, Motivo)', 'SynchCall', 880, -860, 1025, -860, None),
        (19, 225, 226, 'GenerarCodigoUnico()', 'SynchCall', 880, -900, 1025, -900, None),
        (20, 225, 227, 'new TurnoSolicitadoState()', 'SynchCall', 880, -940, 1175, -940, None),
        (21, 225, 226, 'CambiarEstado(TurnoSolicitadoState)', 'SynchCall', 880, -980, 1025, -980, None),
        (22, 226, 225, '', 'Return', 1025, -1020, 880, -1020, None),
        
        # 8. Persistencia y actualización de bloque en DAL
        (23, 225, 228, 'Guardar(turnoBE)', 'SynchCall', 880, -1060, 1620, -1060, None),
        (24, 228, 225, '', 'Return', 1620, -1100, 880, -1100, None),
        (25, 225, 228, "ActualizarEstadoBloque(idBloque, 'Ocupado')", 'SynchCall', 880, -1140, 1620, -1140, None),
        (26, 228, 225, '', 'Return', 1620, -1180, 880, -1180, None),
        
        # 9. Dígitos Verificadores (NO RETURNS)
        (27, 225, 225, 'CalcularDVHorizontal(turnoBE)', 'SynchCall', 880, -1220, 910, -1240, None),
        (28, 225, 229, 'RecalcularYPersistir()', 'SynchCall', 880, -1280, 1335, -1280, None),
        (29, 229, 228, 'ActualizarDV(firmas)', 'SynchCall', 1335, -1320, 1620, -1320, None),
        
        # 10. Bitácora de Auditoría (NO RETURNS)
        (30, 225, 230, "RegistrarEvento('Turnos', 'Agendamiento', dniNutricionista, 'Medio')", 'SynchCall', 880, -1360, 1485, -1360, None),
        (31, 230, 228, 'GuardarBitacora(evento)', 'SynchCall', 1485, -1400, 1620, -1400, None),
        
        # 11. Retorno y Confirmación al Nutricionista
        (32, 225, 220, '', 'Return', 880, -1440, 230, -1440, None),
        (33, 220, 220, 'ActualizarGrillaTurnos()', 'SynchCall', 230, -1480, 260, -1500, None),
        (34, 220, 219, 'MostrarMensajeExito(codigoTurno)', 'SynchCall', 230, -1540, 80, -1540, None)
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
    out_img = r'C:\Users\Danie\Desktop\GIT\TD\scratch\cun01_sequence_perfect6.png'
    ea.GetProjectInterface().PutDiagramImageToFile(diag.DiagramGUID, out_img, 1)

    ea.CloseFile()
    ea.Exit()
    print('Executed perfect6 successfully')

if __name__ == '__main__':
    update_cun01_polished()
