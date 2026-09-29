import win32com.client

def sync_cu01_diagram_eap(eap_path):
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    
    diagram = ea.GetDiagramByID(12)
    pkg = ea.GetPackageByID(diagram.PackageID)
    
    # Get elements
    elements_map = {}
    for do in diagram.DiagramObjects:
        el = ea.GetElementByID(do.ElementID)
        elements_map[el.Name] = el
        
    print("Found elements on diagram 12:", list(elements_map.keys()))
    
    # Ensure SQL / Database object exists on diagram 12
    sql_obj = None
    for obj in pkg.Elements:
        if obj.Name == "BASE ING" or obj.Name == "SQL" or obj.Name == "Base de Datos":
            sql_obj = obj
            break
    if not sql_obj:
        sql_obj = pkg.Elements.AddNew("BASE ING", "Object")
        sql_obj.Update()
        pkg.Elements.Refresh()
        
    has_sql_do = False
    for do in diagram.DiagramObjects:
        if do.ElementID == sql_obj.ElementID:
            has_sql_do = True
            break
    if not has_sql_do:
        ndo = diagram.DiagramObjects.AddNew("l=1480;r=1580;t=-50;b=-1850;", "")
        ndo.ElementID = sql_obj.ElementID
        ndo.Update()
        diagram.DiagramObjects.Refresh()
    elements_map["BASE ING"] = sql_obj

    # Clean existing connectors for Diagram 12
    ea.Execute("DELETE FROM t_diagramlinks WHERE DiagramID = 12")
    ea.Execute("DELETE FROM t_connector WHERE DiagramID = 12")
    
    # Refresh all element connectors
    for el in elements_map.values():
        el.Connectors.Refresh()
        
    # Full list of messages from DSS CU01 Registro
    op = elements_map.get("Nutricionista")
    ui = elements_map.get("FormTurnero_DNI101")
    ag_bll = elements_map.get("AgendaMedicaBLL_DNI101")
    pac_bll = elements_map.get("PacienteBLL_DNI101")
    tutor_be = elements_map.get("TutorBE_DNI101")
    pac_be = elements_map.get("PacienteBE_DNI101")
    turno_bll = elements_map.get("TurnoBLL_DNI101")
    turno_be = elements_map.get("TurnoBE_DNI101")
    state = elements_map.get("TurnoSolicitadoState")
    dv_bll = elements_map.get("DigitoVerificadorBLL")
    bit_bll = elements_map.get("EventoBLL")
    dal = elements_map.get("DAL")
    sql = elements_map.get("BASE ING")
    
    msg_list = [
        # Fase 1: Consulta de Bloques
        (1, op, ui, "1: SeleccionarFechaYProfesional()", "SynchCall"),
        (2, ui, ag_bll, "2: ObtenerBloquesDisponibles(fecha, dni)", "SynchCall"),
        (3, ag_bll, dal, "3: ListarBloquesDisponibles(fecha, dni)", "SynchCall"),
        (4, dal, sql, "4: SELECT BloquesHorarios", "SynchCall"),
        (5, sql, dal, "5: DataTable (Bloques)", "Return"),
        (6, dal, ag_bll, "6: List(BloqueHorarioBE)", "Return"),
        (7, ag_bll, ui, "7: List(BloqueHorarioBE)", "Return"),
        (8, ui, ui, "8: CargarComboBloques()", "SynchCall"),
        
        # Fase 2: Búsqueda o Alta
        (9, op, ui, "9: BuscarPaciente(dniNiño)", "SynchCall"),
        (10, ui, pac_bll, "10: ObtenerPacientePorDNI(dniNiño)", "SynchCall"),
        (11, pac_bll, dal, "11: ObtenerPorDNI(dniNiño)", "SynchCall"),
        (12, dal, sql, "12: SELECT Pacientes_DNI101", "SynchCall"),
        (13, sql, dal, "13: DataTable (Paciente)", "Return"),
        (14, dal, pac_bll, "14: PacienteBE (o null)", "Return"),
        (15, pac_bll, ui, "15: PacienteBE (o null)", "Return"),
        
        # Punto de extensión: Paciente No Registrado
        (16, op, ui, "16: [alt: Paciente No Registrado] IngresarDatosTutorYNiño()", "SynchCall"),
        (17, ui, tutor_be, "17: new TutorBE_DNI101()", "SynchCall"),
        (18, ui, pac_bll, "18: RegistrarTutor(tutorBE)", "SynchCall"),
        (19, pac_bll, dal, "19: GuardarTutor(tutorBE)", "SynchCall"),
        (20, dal, sql, "20: INSERT INTO Tutores_DNI101", "SynchCall"),
        (21, sql, dal, "21: idTutorGenerado", "Return"),
        (22, dal, pac_bll, "22: idTutorGenerado", "Return"),
        (23, pac_bll, ui, "23: idTutor", "Return"),
        
        (24, ui, pac_be, "24: new PacienteBE_DNI101()", "SynchCall"),
        (25, ui, pac_bll, "25: RegistrarPaciente(pacienteBE)", "SynchCall"),
        (26, pac_bll, dal, "26: GuardarPaciente(pacienteBE)", "SynchCall"),
        (27, dal, sql, "27: INSERT INTO Pacientes_DNI101", "SynchCall"),
        (28, sql, dal, "28: idPacienteGenerado", "Return"),
        (29, dal, pac_bll, "29: idPacienteGenerado", "Return"),
        (30, pac_bll, ui, "30: idPaciente", "Return"),
        
        # Fase 3: Agendamiento del Turno
        (31, op, ui, "31: Click btnAgendar", "SynchCall"),
        (32, ui, turno_be, "32: new TurnoBE_DNI101()", "SynchCall"),
        (33, ui, turno_bll, "33: AgendarTurno(turnoBE)", "SynchCall"),
        (34, turno_bll, turno_be, "34: GenerarCodigoUnico()", "SynchCall"),
        (35, turno_bll, state, "35: new TurnoSolicitadoState()", "SynchCall"),
        (36, turno_bll, turno_be, "36: CambiarEstado(state)", "SynchCall"),
        (37, turno_bll, turno_bll, "37: CalcularDVHorizontal(turno)", "SynchCall"),
        (38, turno_bll, dal, "38: GuardarTurno(turno)", "SynchCall"),
        (39, dal, sql, "39: INSERT INTO Turnos_DNI101", "SynchCall"),
        (40, sql, dal, "40: OK (idTurno)", "Return"),
        (41, dal, turno_bll, "41: idTurnoGenerado", "Return"),
        (42, turno_bll, dal, "42: ActualizarEstadoBloque(id, 'Ocupado')", "SynchCall"),
        (43, dal, sql, "43: UPDATE BloquesHorarios", "SynchCall"),
        (44, sql, dal, "44: OK", "Return"),
        (45, dal, turno_bll, "45: true", "Return"),
        
        # Fase 4: Dígito Verificador y Bitácora
        (46, turno_bll, dv_bll, "46: RecalcularYPersistir()", "SynchCall"),
        (47, dv_bll, dal, "47: ActualizarDV(firmas)", "SynchCall"),
        (48, dal, sql, "48: UPDATE dbo.DV", "SynchCall"),
        (49, sql, dal, "49: OK", "Return"),
        (50, dal, dv_bll, "50: true", "Return"),
        (51, dv_bll, turno_bll, "51: true", "Return"),
        
        (52, turno_bll, bit_bll, "52: RegistrarEvento('Turnos', 'Agendamiento', dni, 'Medio')", "SynchCall"),
        (53, bit_bll, dal, "53: GuardarBitacora(evento)", "SynchCall"),
        (54, dal, sql, "54: INSERT INTO Bitacora", "SynchCall"),
        (55, sql, dal, "55: OK", "Return"),
        (56, dal, bit_bll, "56: true", "Return"),
        (57, bit_bll, turno_bll, "57: true", "Return"),
        
        (58, turno_bll, ui, "58: true (Exito)", "Return"),
        (59, ui, op, "59: MostrarMensajeExito()", "SynchCall")
    ]
    
    for seq, src, dst, msg_name, subtype in msg_list:
        conn = src.Connectors.AddNew(msg_name, "Sequence")
        conn.SupplierID = dst.ElementID
        conn.SequenceNo = seq
        conn.SubType = subtype
        conn.DiagramID = 12
        conn.Update()
        src.Connectors.Refresh()
        
    diagram.Update()
    diagram.DiagramObjects.Refresh()
    
    # Export PNG
    project_interface = ea.GetProjectInterface()
    out_img = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\DSS\CU01 Registrar.png'
    try:
        project_interface.PutDiagramImageToFile(diagram.DiagramGUID, out_img, 1)
        print("Exported updated diagram image to:", out_img)
    except Exception as e:
        print("Export error:", e)
        
    ea.CloseFile()
    ea.Exit()
    print("Successfully synchronized CU01: Registrar diagram in TD.EAP!")

if __name__ == '__main__':
    sync_cu01_diagram_eap(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
