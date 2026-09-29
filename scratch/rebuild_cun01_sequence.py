import win32com.client
import sys

def rebuild_cun01():
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    
    diag = ea.GetDiagramByID(12)
    diag.Name = "CUN01: Registrar Turno"
    diag.Update()
    print("Renamed diagram to CUN01: Registrar Turno")
    
    # Clean old connectors of diagram 12
    ea.Execute("DELETE FROM t_connector WHERE DiagramID = 12")
    print("Cleaned old connectors for DiagramID 12")
    
    # Manage diagram objects
    # Remove TutorBE (223), PacienteBE (224), BASE ING (647)
    to_remove = [223, 224, 647]
    for i in range(diag.DiagramObjects.Count - 1, -1, -1):
        d_obj = diag.DiagramObjects.GetAt(i)
        if d_obj.ElementID in to_remove:
            diag.DiagramObjects.Delete(i)
    diag.DiagramObjects.Refresh()
    
    # Lifeline Layout Configuration
    # Centers:
    # Nutricionista (219): 80
    # FormTurnero_DNI101 (220): 230
    # AgendaMedicaBLL_DNI101 (221): 395
    # PacienteBLL_DNI101 (222): 550
    # CUN02: Registrar Paciente (38): 710
    # TurnoBLL_DNI101 (225): 880
    # TurnoBE_DNI101 (226): 1025
    # TurnoSolicitadoState (227): 1175
    # DigitoVerificadorBLL (229): 1335
    # EventoBLL (230): 1485
    # DAL (228): 1620
    
    coords = {
        219: ("l=30;r=130;t=-50;b=-1850;", 80),
        220: ("l=170;r=290;t=-50;b=-1850;", 230),
        221: ("l=330;r=460;t=-50;b=-1850;", 395),
        222: ("l=490;r=610;t=-50;b=-1850;", 550),
        225: ("l=820;r=940;t=-50;b=-1850;", 880),
        226: ("l=970;r=1080;t=-50;b=-1850;", 1025),
        227: ("l=1110;r=1240;t=-50;b=-1850;", 1175),
        229: ("l=1270;r=1400;t=-50;b=-1850;", 1335),
        230: ("l=1430;r=1540;t=-50;b=-1850;", 1485),
        228: ("l=1570;r=1670;t=-50;b=-1850;", 1620),
        # InteractionFragment
        232: ("l=20;r=790;t=-560;b=-800;", 0),
        # UseCase oval lifeline
        38:  ("l=640;r=780;t=-650;b=-800;", 710)
    }
    
    existing_ids = set()
    for d_obj in diag.DiagramObjects:
        existing_ids.add(d_obj.ElementID)
        if d_obj.ElementID in coords:
            pos_str = coords[d_obj.ElementID][0]
            # parse l, r, t, b
            parts = dict(p.split('=') for p in pos_str.strip(';').split(';') if p)
            d_obj.left = int(parts['l'])
            d_obj.right = int(parts['r'])
            d_obj.top = int(parts['t'])
            d_obj.bottom = int(parts['b'])
            d_obj.Update()
            
    # Add CUN02 (38) if not already on diagram
    if 38 not in existing_ids:
        pos_str = coords[38][0]
        new_obj = diag.DiagramObjects.AddNew(pos_str, '')
        new_obj.ElementID = 38
        new_obj.Update()
        
    diag.DiagramObjects.Refresh()
    diag.Update()
    print("Lifelines and DiagramObjects positioned properly")
    
    # Message definitions:
    # seq, sender_id, receiver_id, name, subtype, start_x, start_y, end_x, end_y, extra_style
    messages = [
        # Phase 1: Disponibilidad
        (1,  219, 220, "1: SeleccionarFechaYProfesional(fecha, dniNutricionista)", "SynchCall", 80, -110, 230, -110, ""),
        (2,  220, 221, "2: ListarBloquesDisponibles(fecha, dniNutricionista)", "SynchCall", 230, -150, 395, -150, ""),
        (3,  221, 228, "3: ListarBloquesDisponibles(fecha, dniNutricionista)", "SynchCall", 395, -190, 1620, -190, ""),
        (4,  228, 221, "4: List<BloqueHorarioBE_DNI101>", "Return", 1620, -230, 395, -230, ""),
        (5,  221, 220, "5: List<BloqueHorarioBE_DNI101>", "Return", 395, -270, 230, -270, ""),
        (6,  220, 220, "6: CargarSelectorHorariosDisponibles()", "SynchCall", 230, -310, 260, -330, ""),
        
        # Phase 2: Búsqueda Paciente
        (7,  219, 220, "7: IngresarDNI(dniNiño)", "SynchCall", 80, -370, 230, -370, ""),
        (8,  220, 222, "8: ObtenerPacientePorDNI(dniNiño)", "SynchCall", 230, -410, 550, -410, ""),
        (9,  222, 228, "9: ObtenerPacientePorDNI(dniNiño)", "SynchCall", 550, -450, 1620, -450, ""),
        (10, 228, 222, "10: null (No Registrado)", "Return", 1620, -490, 550, -490, ""),
        (11, 222, 220, "11: null", "Return", 550, -530, 230, -530, ""),
        
        # Flujo alternativo 6.1 (Punto de Extensión CUN-02)
        (12, 220, 219, "12: InformarPacienteNoRegistrado()", "SynchCall", 230, -590, 80, -590, ""),
        (13, 219, 220, "13: IngresarDatosPaciente(Nombre, Apellido, DNINiño, Telefono, Email, ObraSocial)", "SynchCall", 80, -630, 230, -630, ""),
        (14, 220, 38,  "14: RegistrarPaciente(Nombre, Apellido, DNINiño, Telefono, Email, ObraSocial)", "SynchCall", 230, -670, 710, -670, ""),
        (15, 38,  220, "15: PacienteRegistradoOK()", "Return", 710, -730, 230, -730, ""),
        (16, 220, 38,  "", "Delete", 710, -760, 710, -760, "hidden=1;"), # Destruction marker X
        
        # Phase 3: Registro de Turno
        (17, 219, 220, "17: SeleccionarHorarioEIngresarMotivo(horario, motivo)", "SynchCall", 80, -840, 230, -840, ""),
        (18, 219, 220, "18: Click btnRegistrarTurno", "SynchCall", 80, -880, 230, -880, ""),
        (19, 220, 220, "19: ValidarCamposObligatorios()", "SynchCall", 230, -920, 260, -940, ""),
        
        # Phase 4: Orquestación en BLL
        (20, 220, 225, "20: RegistrarTurno(DNINiño, Fecha, Horario, Motivo)", "SynchCall", 230, -970, 880, -970, ""),
        (21, 225, 226, "21: new TurnoBE_DNI101(DNINiño, Fecha, Horario, Motivo)", "SynchCall", 880, -1010, 1025, -1010, ""),
        (22, 225, 226, "22: GenerarCodigoUnico()", "SynchCall", 880, -1050, 1025, -1050, ""),
        (23, 225, 227, "23: new TurnoSolicitadoState()", "SynchCall", 880, -1090, 1175, -1090, ""),
        (24, 225, 226, "24: CambiarEstado(TurnoSolicitadoState)", "SynchCall", 880, -1130, 1025, -1130, ""),
        
        # Persistencia en DAL
        (25, 225, 228, "25: Guardar(turnoBE)", "SynchCall", 880, -1170, 1620, -1170, ""),
        (26, 228, 225, "26: idTurnoGenerado", "Return", 1620, -1210, 880, -1210, ""),
        (27, 225, 228, "27: ActualizarEstadoBloque(idBloque, 'Ocupado')", "SynchCall", 880, -1250, 1620, -1250, ""),
        (28, 228, 225, "28: true", "Return", 1620, -1290, 880, -1290, ""),
        
        # Dígitos Verificadores
        (29, 225, 225, "29: CalcularDVHorizontal(turnoBE)", "SynchCall", 880, -1330, 910, -1350, ""),
        (30, 225, 229, "30: RecalcularYPersistir()", "SynchCall", 880, -1380, 1335, -1380, ""),
        (31, 229, 228, "31: ActualizarDV(firmas)", "SynchCall", 1335, -1420, 1620, -1420, ""),
        (32, 228, 229, "32: true", "Return", 1620, -1460, 1335, -1460, ""),
        (33, 229, 225, "33: true", "Return", 1335, -1500, 880, -1500, ""),
        
        # Bitácora
        (34, 225, 230, "34: RegistrarEvento('Turnos', 'Agendamiento', dniNutricionista, 'Medio')", "SynchCall", 880, -1540, 1485, -1540, ""),
        (35, 230, 228, "35: GuardarBitacora(evento)", "SynchCall", 1485, -1580, 1620, -1580, ""),
        (36, 228, 230, "36: true", "Return", 1620, -1620, 1485, -1620, ""),
        (37, 230, 225, "37: true", "Return", 1485, -1660, 880, -1660, ""),
        
        # Feedback y UI
        (38, 225, 220, "38: true (codigoTurno)", "Return", 880, -1700, 230, -1700, ""),
        (39, 220, 220, "39: ActualizarGrillaTurnos()", "SynchCall", 230, -1740, 260, -1760, ""),
        (40, 220, 219, "40: MostrarMensajeExito(codigoTurno)", "SynchCall", 230, -1790, 80, -1790, "")
    ]
    
    for seq, s_id, r_id, m_name, stype, sx, sy, ex, ey, style in messages:
        sender_el = ea.GetElementByID(s_id)
        conn = sender_el.Connectors.AddNew(m_name, "Sequence")
        conn.SupplierID = r_id
        conn.SubType = stype
        conn.Update()
        
        sql = f"UPDATE t_connector SET DiagramID = 12, SeqNo = {seq}, PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey}"
        if style:
            sql += f", StyleEx = '{style}'"
        sql += f" WHERE Connector_ID = {conn.ConnectorID}"
        ea.Execute(sql)
        
    print(f"Created and positioned all {len(messages)} messages successfully")
    
    diag.Update()
    out_img = r'C:\Users\Danie\Desktop\GIT\TD\scratch\cun01_sequence.png'
    ea.GetProjectInterface().PutDiagramImageToFile(diag.DiagramGUID, out_img, 1)
    print(f"Diagram image exported to {out_img}")
    
    ea.CloseFile()
    ea.Exit()
    print("Done rebuild_cun01")

if __name__ == '__main__':
    rebuild_cun01()
