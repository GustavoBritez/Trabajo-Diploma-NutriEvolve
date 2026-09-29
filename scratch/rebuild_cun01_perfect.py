import win32com.client

def rebuild_cun01_perfect():
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    
    diag = ea.GetDiagramByID(12)
    diag.Name = "CUN01: Registrar Turno"
    diag.Update()
    print("Ensured diagram name: CUN01: Registrar Turno")
    
    # Clean all existing connectors for DiagramID 12
    ea.Execute("DELETE FROM t_connector WHERE DiagramID = 12")
    
    # Remove unwanted lifelines if still present
    to_remove = [223, 224, 647]
    for i in range(diag.DiagramObjects.Count - 1, -1, -1):
        d_obj = diag.DiagramObjects.GetAt(i)
        if d_obj.ElementID in to_remove:
            diag.DiagramObjects.Delete(i)
    diag.DiagramObjects.Refresh()
    
    # Participant Centers:
    # 219 (Nutricionista): 80
    # 220 (FormTurnero_DNI101): 230
    # 221 (AgendaMedicaBLL_DNI101): 395
    # 222 (PacienteBLL_DNI101): 550
    # 38  (CUN02: Registrar Paciente): 710
    # 225 (TurnoBLL_DNI101): 880
    # 226 (TurnoBE_DNI101): 1025
    # 227 (TurnoSolicitadoState): 1175
    # 229 (DigitoVerificadorBLL): 1335
    # 230 (EventoBLL): 1485
    # 228 (DAL): 1620
    
    lifeline_bottom = -1580
    
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
        # InteractionFragment 232:
        232: (20, 790, -520, -710, 1),
        # CUN02 oval: Sequence = 6
        38:  (640, 780, -590, -700, 6)
    }
    
    existing_ids = set()
    for d_obj in diag.DiagramObjects:
        existing_ids.add(d_obj.ElementID)
        if d_obj.ElementID in coords:
            l, r, t, b, seq = coords[d_obj.ElementID]
            d_obj.left = l
            d_obj.right = r
            d_obj.top = t
            d_obj.bottom = b
            d_obj.Sequence = seq
            d_obj.Update()
            
    if 38 not in existing_ids:
        l, r, t, b, seq = coords[38]
        new_obj = diag.DiagramObjects.AddNew(f"l={l};r={r};t={t};b={b};", '')
        new_obj.ElementID = 38
        new_obj.left = l
        new_obj.right = r
        new_obj.top = t
        new_obj.bottom = b
        new_obj.Sequence = seq
        new_obj.Update()
        
    diag.DiagramObjects.Refresh()
    diag.Update()
    print("Lifelines and DiagramObjects updated")
    
    # Message definitions:
    # seq, sender_id, receiver_id, name, subtype, start_x, start_y, end_x, end_y
    messages = [
        # 1. Nutricionista triggers RegistrarTurno
        (1,  219, 220, "RegistrarTurno(DNINiño, Fecha, Horario, Motivo)", "SynchCall", 80, -110, 230, -110),
        
        # 2. Consultar disponibilidad
        (2,  220, 221, "ListarBloquesDisponibles(Fecha, dniNutricionista)", "SynchCall", 230, -150, 395, -150),
        (3,  221, 228, "ListarBloquesDisponibles(Fecha, dniNutricionista)", "SynchCall", 395, -190, 1620, -190),
        (4,  228, 221, "", "Return", 1620, -230, 395, -230),
        (5,  221, 220, "", "Return", 395, -270, 230, -270),
        (6,  220, 220, "CargarSelectorHorariosDisponibles()", "SynchCall", 230, -310, 260, -330),
        
        # 3. Buscar Paciente
        (7,  220, 222, "ObtenerPacientePorDNI(DNINiño)", "SynchCall", 230, -370, 550, -370),
        (8,  222, 228, "ObtenerPacientePorDNI(DNINiño)", "SynchCall", 550, -410, 1620, -410),
        (9,  228, 222, "", "Return", 1620, -450, 550, -450),
        (10, 222, 220, "", "Return", 550, -490, 230, -490),
        
        # 4. Flujo Alternativo: CUN-02 Paciente No Registrado (Punto de Extensión)
        (11, 220, 219, "InformarPacienteNoRegistrado()", "SynchCall", 230, -540, 80, -540),
        (12, 219, 220, "IngresarDatosPaciente(Nombre, Apellido, DNINiño, Telefono, Email, ObraSocial)", "SynchCall", 80, -580, 230, -580),
        (13, 220, 38,  "RegistrarPaciente(Nombre, Apellido, DNINiño, Telefono, Email, ObraSocial)", "New", 230, -620, 710, -620),
        (14, 38,  220, "PacienteRegistradoOK()", "Return", 710, -660, 230, -660),
        (15, 220, 38,  "", "Delete", 230, -690, 710, -690),
        
        # 5. Validación en UI
        (16, 220, 220, "ValidarCamposObligatorios()", "SynchCall", 230, -740, 260, -760),
        
        # 6. Agendamiento en TurnoBLL
        (17, 220, 225, "RegistrarTurno(DNINiño, Fecha, Horario, Motivo)", "SynchCall", 230, -800, 880, -800),
        
        # 7. Orquestación BLL y Patrón State
        (18, 225, 226, "new TurnoBE_DNI101(DNINiño, Fecha, Horario, Motivo)", "New", 880, -840, 1025, -840),
        (19, 225, 226, "GenerarCodigoUnico()", "SynchCall", 880, -880, 1025, -880),
        (20, 225, 227, "new TurnoSolicitadoState()", "New", 880, -920, 1175, -920),
        (21, 225, 226, "CambiarEstado(TurnoSolicitadoState)", "SynchCall", 880, -960, 1025, -960),
        (22, 226, 225, "", "Return", 1025, -1000, 880, -1000),
        
        # 8. Persistencia y actualización de bloque en DAL
        (23, 225, 228, "Guardar(turnoBE)", "SynchCall", 880, -1040, 1620, -1040),
        (24, 228, 225, "", "Return", 1620, -1080, 880, -1080),
        (25, 225, 228, "ActualizarEstadoBloque(idBloque, 'Ocupado')", "SynchCall", 880, -1120, 1620, -1120),
        (26, 228, 225, "", "Return", 1620, -1160, 880, -1160),
        
        # 9. Dígitos Verificadores (sin retornos)
        (27, 225, 225, "CalcularDVHorizontal(turnoBE)", "SynchCall", 880, -1200, 910, -1220),
        (28, 225, 229, "RecalcularYPersistir()", "SynchCall", 880, -1250, 1335, -1250),
        (29, 229, 228, "ActualizarDV(firmas)", "SynchCall", 1335, -1290, 1620, -1290),
        
        # 10. Bitácora de Auditoría (sin retornos)
        (30, 225, 230, "RegistrarEvento('Turnos', 'Agendamiento', dniNutricionista, 'Medio')", "SynchCall", 880, -1330, 1485, -1330),
        (31, 230, 228, "GuardarBitacora(evento)", "SynchCall", 1485, -1370, 1620, -1370),
        
        # 11. Retorno y Confirmación al Nutricionista
        (32, 225, 220, "", "Return", 880, -1410, 230, -1410),
        (33, 220, 220, "ActualizarGrillaTurnos()", "SynchCall", 230, -1450, 260, -1470),
        (34, 220, 219, "MostrarMensajeExito(codigoTurno)", "SynchCall", 230, -1510, 80, -1510)
    ]
    
    for seq, s_id, r_id, m_name, stype, sx, sy, ex, ey in messages:
        sender_el = ea.GetElementByID(s_id)
        conn = sender_el.Connectors.AddNew(m_name, "Sequence")
        conn.SupplierID = r_id
        conn.SubType = stype
        conn.Update()
        
        sql = f"UPDATE t_connector SET DiagramID = 12, SeqNo = {seq}, PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey}, StyleEx = NULL WHERE Connector_ID = {conn.ConnectorID}"
        ea.Execute(sql)
        
    print(f"Created all {len(messages)} messages cleanly without numbering")
    
    # Save diagram and export
    diag.Update()
    out_img = r'C:\Users\Danie\Desktop\GIT\TD\scratch\cun01_sequence_perfect.png'
    ea.GetProjectInterface().PutDiagramImageToFile(diag.DiagramGUID, out_img, 1)
    print(f"Diagram image exported to {out_img}")
    
    ea.CloseFile()
    ea.Exit()
    print("Finished perfectly")

if __name__ == '__main__':
    rebuild_cun01_perfect()
