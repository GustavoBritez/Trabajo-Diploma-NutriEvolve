import win32com.client
import os

def build_secuencia_general_full_alt(eap_path):
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    
    diagram = ea.GetDiagramByID(33)
    pkg = ea.GetPackageByID(diagram.PackageID)
    
    paciente = ea.GetElementByID(643)  # Paciente (Actor)
    nutri = ea.GetElementByID(644)     # Nutricionista (Actor)
    
    # 1. Clean diagram 33 connectors and fragments
    ea.Execute("DELETE FROM t_diagramlinks WHERE DiagramID = 33")
    ea.Execute("DELETE FROM t_connector WHERE DiagramID = 33")
    ea.Execute(f"DELETE FROM t_diagramobjects WHERE Diagram_ID = 33 AND Object_ID NOT IN ({paciente.ElementID}, {nutri.ElementID})")
    
    diagram.DiagramObjects.Refresh()
    
    # Position lifelines nicely
    for do in diagram.DiagramObjects:
        if do.ElementID == paciente.ElementID:
            do.left = 120
            do.right = 220
            do.top = -50
            do.bottom = -1350
            do.Update()
        elif do.ElementID == nutri.ElementID:
            do.left = 480
            do.right = 580
            do.top = -50
            do.bottom = -1350
            do.Update()
            
    diagram.Update()
    diagram.DiagramObjects.Refresh()
    
    # 2. Add Messages with exact numbering and guards
    messages = [
        # Fase 1: Solicitud inicial
        (1, paciente, nutri, "1: SolicitarTurno(dniNiño, datosTutor, fechaPreferencia)", "SynchCall"),
        (2, nutri, paciente, "2: OfrecerHorariosDisponibles(listaBloquesLibres)", "SynchCall"),
        (3, paciente, nutri, "3: SeleccionarHorarioPreferido(fecha, hora)", "SynchCall"),
        
        # Bloque ALT 1: Padrón
        (4, nutri, paciente, "4: [Paciente Nuevo] SolicitarDatosFiliatorios()", "SynchCall"),
        (5, paciente, nutri, "5: EntregarDatos(nombreNiño, fechaNac, datosTutor)", "Return"),
        (6, nutri, paciente, "6: [Paciente Registrado] ConfirmarDatosExistentes()", "SynchCall"),
        (7, paciente, nutri, "7: ValidarDatos()", "Return"),
        
        # Fase 3: Emisión Comprobante
        (8, nutri, paciente, "8: EntregarComprobanteTurno(codigoTurno, fecha, hora)", "SynchCall"),
        
        # Bloque ALT 2: Eventos Posteriores
        (9, paciente, nutri, "9: [Caso 1: Asistencia] PresentarseEnConsultorio(codigoTurno)", "SynchCall"),
        (10, nutri, paciente, "10: IniciarConsultaNutricional() [Deriva a PN2]", "SynchCall"),
        (11, paciente, nutri, "11: [Caso 2: Reprogramar] SolicitarReprogramacion(codigoTurno, nuevaFecha)", "SynchCall"),
        (12, nutri, paciente, "12: EntregarNuevoComprobante(codigoTurno, nuevaFecha, nuevaHora)", "SynchCall"),
        (13, paciente, nutri, "13: [Caso 3: Cancelar] SolicitarCancelacion(codigoTurno, motivo)", "SynchCall"),
        (14, nutri, paciente, "14: NotificarCancelacionExitosa(codigoTurno)", "SynchCall")
    ]
    
    paciente.Connectors.Refresh()
    nutri.Connectors.Refresh()
    
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
    
    # 3. Add Combined Fragments (ALT)
    # Fragment 1: ALT Padrón de Pacientes
    frag1 = pkg.Elements.AddNew("alt Validación de Padrón", "InteractionFragment")
    frag1.Notes = "[Paciente Nuevo]\n--\n[Paciente Registrado]"
    frag1.Update()
    do_f1 = diagram.DiagramObjects.AddNew("l=70;r=630;t=-280;b=-550;", "")
    do_f1.ElementID = frag1.ElementID
    do_f1.Update()
    
    # Fragment 2: ALT Eventos Posteriores
    frag2 = pkg.Elements.AddNew("alt Eventos Posteriores (Ciclo de Vida)", "InteractionFragment")
    frag2.Notes = "[Caso 1: Asistencia a Consulta - Flujo Normal]\n--\n[Caso 2: Reprogramación de Turno]\n--\n[Caso 3: Cancelación de Turno]"
    frag2.Update()
    do_f2 = diagram.DiagramObjects.AddNew("l=70;r=630;t=-700;b=-1280;", "")
    do_f2.ElementID = frag2.ElementID
    do_f2.Update()
    
    diagram.Update()
    diagram.DiagramObjects.Refresh()
    
    # Export PNG
    project_interface = ea.GetProjectInterface()
    out_img = r'C:\Users\Danie\Desktop\GIT\TD\scratch\secuencia_general.png'
    try:
        project_interface.PutDiagramImageToFile(diagram.DiagramGUID, out_img, 1)
        print("Exported updated diagram image to:", out_img)
    except Exception as e:
        print("Export error:", e)
        
    ea.CloseFile()
    ea.Exit()
    print("Successfully built Secuencia General with ALT blocks in TD.EAP!")

if __name__ == '__main__':
    build_secuencia_general_full_alt(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
