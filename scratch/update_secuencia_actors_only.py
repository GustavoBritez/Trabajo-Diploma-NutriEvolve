import win32com.client
import os

def update_secuencia_general_actors_only(eap_path):
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    
    diagram = ea.GetDiagramByID(33)
    pkg = ea.GetPackageByID(diagram.PackageID)
    
    paciente = ea.GetElementByID(643)  # Paciente (Actor)
    nutri = ea.GetElementByID(644)     # Nutricionista (Actor)
    
    # 1. Clean connectors and diagram links for diagram 33
    try:
        ea.Execute("DELETE FROM t_diagramlinks WHERE DiagramID = 33")
    except Exception as e:
        print("Diagramlinks delete:", e)
        
    try:
        ea.Execute("DELETE FROM t_connector WHERE DiagramID = 33")
    except Exception as e:
        print("Connector delete:", e)
    
    # Remove any extra objects (like boundary/system) from diagram 33 diagramobjects
    try:
        ea.Execute(f"DELETE FROM t_diagramobjects WHERE Diagram_ID = 33 AND Object_ID NOT IN ({paciente.ElementID}, {nutri.ElementID})")
    except Exception as e:
        print("Diagramobjects delete:", e)
    
    diagram.DiagramObjects.Refresh()
    
    # Position the two lifelines
    for do in diagram.DiagramObjects:
        if do.ElementID == paciente.ElementID:
            do.left = 120
            do.right = 220
            do.top = -50
            do.bottom = -1100
            do.Update()
        elif do.ElementID == nutri.ElementID:
            do.left = 480
            do.right = 580
            do.top = -50
            do.bottom = -1100
            do.Update()
            
    diagram.Update()
    diagram.DiagramObjects.Refresh()
    
    # 2. Add Messages directly between Paciente and Nutricionista
    messages = [
        # Fase 1: Solicitud de Turno
        (1, paciente, nutri, "1: SolicitarTurno(dniNiño, datosTutor, fechaPreferencia)", "SynchCall"),
        (2, nutri, paciente, "2: OfrecerHorariosDisponibles(listaBloquesLibres)", "SynchCall"),
        (3, paciente, nutri, "3: SeleccionarHorarioPreferido(fecha, hora)", "SynchCall"),
        
        # Fase 2: Registro de Paciente Nuevo (alt / condicional)
        (4, nutri, paciente, "4: [Paciente Nuevo] SolicitarDatosFiliatorios()", "SynchCall"),
        (5, paciente, nutri, "5: EntregarDatos(nombreNiño, fechaNac, datosTutor)", "Return"),
        
        # Fase 3: Confirmación y Entrega de Turno
        (6, nutri, paciente, "6: EntregarComprobanteTurno(codigoTurno, fecha, hora)", "SynchCall"),
        
        # Fase 4: Alternativas Posteriores (Asistencia vs Reprogramación vs Cancelación)
        # Alt 1: Asistencia (Flujo Normal a PN2)
        (7, paciente, nutri, "7: [alt: Asistencia] PresentarseEnConsultorio(codigoTurno)", "SynchCall"),
        (8, nutri, paciente, "8: IniciarConsultaNutricional() [Deriva a PN2]", "SynchCall"),
        
        # Alt 2: Reprogramación
        (9, paciente, nutri, "9: [alt: Reprogramar] SolicitarReprogramacion(codigoTurno, nuevaFecha)", "SynchCall"),
        (10, nutri, paciente, "10: EntregarNuevoComprobante(codigoTurno, nuevaFecha, nuevaHora)", "SynchCall"),
        
        # Alt 3: Cancelación
        (11, paciente, nutri, "11: [alt: Cancelar] SolicitarCancelacion(codigoTurno, motivo)", "SynchCall"),
        (12, nutri, paciente, "12: NotificarCancelacionExitosa(codigoTurno)", "SynchCall")
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
    print("Successfully updated 2-actor Sequence Diagram in TD.EAP!")

if __name__ == '__main__':
    update_secuencia_general_actors_only(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
