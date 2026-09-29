import win32com.client
import os

def build_full_pn1_sequence_diagram(eap_path):
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    
    diagram = ea.GetDiagramByID(33)
    pkg = ea.GetPackageByID(diagram.PackageID)
    
    # 1. Lifelines
    paciente = ea.GetElementByID(643)  # Paciente (Actor)
    nutri = ea.GetElementByID(644)     # Nutricionista (Actor)
    
    # Find or create Sistema NutriEvolve
    system_obj = None
    for obj in pkg.Elements:
        if obj.Name == "Sistema (NutriEvolve)" or obj.Name == ":Sistema NutriEvolve" or obj.Name == "Sistema NutriEvolve":
            system_obj = obj
            break
    if not system_obj:
        system_obj = pkg.Elements.AddNew("Sistema (NutriEvolve)", "Boundary")
        system_obj.Update()
        pkg.Elements.Refresh()

    # Ensure diagram has the 3 lifelines
    element_ids = [paciente.ElementID, nutri.ElementID, system_obj.ElementID]
    existing_do_ids = [do.ElementID for do in diagram.DiagramObjects]
    
    for eid in element_ids:
        if eid not in existing_do_ids:
            ndo = diagram.DiagramObjects.AddNew("l=100;r=200;t=-50;b=-1400;", "")
            ndo.ElementID = eid
            ndo.Update()
            
    diagram.DiagramObjects.Refresh()
    
    # Reposition lifelines
    for do in diagram.DiagramObjects:
        if do.ElementID == paciente.ElementID:
            do.left = 80
            do.right = 180
            do.top = -50
            do.bottom = -1400
            do.Update()
        elif do.ElementID == nutri.ElementID:
            do.left = 340
            do.right = 440
            do.top = -50
            do.bottom = -1400
            do.Update()
        elif do.ElementID == system_obj.ElementID:
            do.left = 600
            do.right = 720
            do.top = -50
            do.bottom = -1400
            do.Update()
            
    # Delete previous connectors for diagram 33 via SQL/EA to keep it clean
    ea.Execute("DELETE FROM t_diagramlinks WHERE DiagramID = 33")
    ea.Execute(f"DELETE FROM t_connector WHERE DiagramID = 33 OR (Start_Object_ID IN ({paciente.ElementID}, {nutri.ElementID}, {system_obj.ElementID}) AND End_Object_ID IN ({paciente.ElementID}, {nutri.ElementID}, {system_obj.ElementID}) AND Connector_Type = 'Sequence')")
    
    paciente.Connectors.Refresh()
    nutri.Connectors.Refresh()
    system_obj.Connectors.Refresh()
    
    # Define Messages based on PN1 Macro Process
    # (seq_num, source, dest, name, subtype)
    messages = [
        # Fase 1: Consulta y Disponibilidad
        (1, paciente, nutri, "1: SolicitarTurno(dniNiño, fechaDeseada, datosTutor)", "SynchCall"),
        (2, nutri, system_obj, "2: ConsultarDisponibilidad(fecha, profesional)", "SynchCall"),
        (3, system_obj, nutri, "3: MostrarBloquesDisponibles(listaBloques)", "Return"),
        
        # Fase 2: Verificación y Registro de Paciente / Tutor (CU08)
        (4, nutri, system_obj, "4: BuscarPacientePorDNI(dniNiño)", "SynchCall"),
        (5, system_obj, nutri, "5: PacienteNoRegistrado()", "Return"),
        (6, nutri, paciente, "6: SolicitarDatosFiliatorios()", "SynchCall"),
        (7, paciente, nutri, "7: EntregarDatos(nombreNiño, fechaNac, datosTutor)", "Return"),
        (8, nutri, system_obj, "8: RegistrarPacienteYTutor(datosNiño, datosTutor)", "SynchCall"),
        (9, system_obj, nutri, "9: PacienteRegistradoExitosamente()", "Return"),
        
        # Fase 3: Agendamiento y Confirmación del Turno (CU01)
        (10, nutri, system_obj, "10: AgendarTurno(idPaciente, idBloque, motivo)", "SynchCall"),
        (11, system_obj, nutri, "11: TurnoAgendadoConfirmado(codigoTurno, fecha, hora)", "Return"),
        (12, nutri, paciente, "12: InformarTurnoConfirmado(codigoTurno, fecha, hora)", "SynchCall"),
        
        # Fase 4: Eventos Posteriores (Reprogramación, Asistencia)
        (13, paciente, nutri, "13: SolicitarReprogramacion(codigoTurno, nuevaFecha)", "SynchCall"),
        (14, nutri, system_obj, "14: ReprogramarTurno(codigoTurno, nuevoIdBloque)", "SynchCall"),
        (15, system_obj, nutri, "15: TurnoReprogramadoExitosamente()", "Return"),
        (16, nutri, paciente, "16: NotificarNuevoHorario(codigoTurno, nuevaFecha, hora)", "SynchCall"),
        
        (17, paciente, nutri, "17: PresentarseEnConsultorio(codigoTurno)", "SynchCall"),
        (18, nutri, system_obj, "18: MarcarAsistencia(codigoTurno)", "SynchCall"),
        (19, system_obj, nutri, "19: HabilitarAtencionNutricional()", "Return"),
        (20, nutri, paciente, "20: IniciarConsultaNutricional() [Deriva a PN2]", "SynchCall")
    ]
    
    for seq, src, dst, msg_name, subtype in messages:
        conn = src.Connectors.AddNew(msg_name, "Sequence")
        conn.SupplierID = dst.ElementID
        conn.SequenceNo = seq
        conn.SubType = subtype
        conn.DiagramID = 33
        conn.Update()
        src.Connectors.Refresh()
        
    diagram.Update()
    diagram.DiagramObjects.Refresh()
    
    # Also export diagram image to scratch/secuencia_general.png
    project_interface = ea.GetProjectInterface()
    out_img = r'C:\Users\Danie\Desktop\GIT\TD\scratch\secuencia_general.png'
    try:
        project_interface.SaveDiagramImageToFile(diagram.DiagramGUID, out_img, 1)
        print("Image exported:", out_img)
    except Exception as e:
        print("Export image exception:", e)
    
    ea.CloseFile()
    ea.Exit()
    print("Successfully built PN1 Sequence Diagram in TD.EAP!")

if __name__ == '__main__':
    build_full_pn1_sequence_diagram(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
