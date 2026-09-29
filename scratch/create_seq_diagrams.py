import win32com.client

def create_sequence_diagrams(eap_path):
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    
    # We will use Package 4 (Primary Use Cases)
    pkg = ea.GetPackageByID(4)
    
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

    def create_diagram(name, messages):
        # Create Diagram
        diagram = pkg.Diagrams.AddNew(name, "Sequence")
        diagram.Update()
        
        # Add Lifelines
        for idx, eid in enumerate([paciente.ElementID, nutri.ElementID, system_obj.ElementID]):
            do = diagram.DiagramObjects.AddNew(f"l={80 + idx*260};r={180 + idx*260};t=-50;b=-1400;", "")
            do.ElementID = eid
            do.Update()
        diagram.DiagramObjects.Refresh()
        
        # Add Messages
        for seq, src, dst, msg_name, subtype in messages:
            conn = src.Connectors.AddNew(msg_name, "Sequence")
            conn.SupplierID = dst.ElementID
            conn.SequenceNo = seq
            conn.SubType = subtype
            conn.DiagramID = diagram.DiagramID
            conn.Update()
            src.Connectors.Refresh()
            
        diagram.Update()
        return diagram

    # PN1 Messages
    # Entradas: 4, Comportamientos: 5, Salidas: 4
    # This mapping is approximate based on user description
    pn1_messages = [
        # Entradas (4)
        (1, paciente, nutri, "1: EnviarDatosTurno(Nombre, Apellido, DNI, etc)", "SynchCall"),
        (2, nutri, system_obj, "2: CargarDatosTurno()", "SynchCall"),
        (3, nutri, system_obj, "3: RegistrarPacienteSiNoExiste()", "SynchCall"),
        (4, paciente, nutri, "4: SolicitarCambiosTurno()", "SynchCall"),
        
        # Comportamientos (5)
        (5, nutri, system_obj, "5: ReprogramarPorSuperposicion()", "SynchCall"),
        (6, system_obj, nutri, "6: OfrecerHorariosAlternativos()", "Return"),
        (7, nutri, system_obj, "7: SeleccionarTurnoAgenda()", "SynchCall"),
        (8, nutri, system_obj, "8: ModificarTurno()", "SynchCall"),
        (9, nutri, system_obj, "9: RegistrarEstadoFinal(Asistio/Ausente/Cancelado)", "SynchCall"),
        
        # Salidas (4)
        (10, system_obj, nutri, "10: AgendaActualizada(Ocupado)", "Return"),
        (11, system_obj, nutri, "11: AgendaActualizada(Disponible)", "Return"),
        (12, system_obj, nutri, "12: EstadoConsultaRegistrado()", "Return"),
        (13, nutri, paciente, "13: TurnoConfirmado()", "SynchCall")
    ]
    
    # PN2 Messages
    pn2_messages = [
        (1, nutri, system_obj, "1: SeleccionarPaciente()", "SynchCall"),
        (2, system_obj, nutri, "2: DesplegarHistoriaClinica()", "Return"),
        (3, nutri, system_obj, "3: RegistrarAntropometria()", "SynchCall"),
        (4, system_obj, nutri, "4: CalcularIMC_ZScores()", "Return"),
        (5, system_obj, nutri, "5: GenerarPatronCrecimientoOMS()", "Return"),
        (6, system_obj, nutri, "6: GenerarDiagnosticoNutricional()", "Return"),
        (7, system_obj, nutri, "7: EmitirAlertaClinica()", "Return"),
        (8, nutri, system_obj, "8: RegistrarAntecedentesYR24h()", "SynchCall"),
        (9, nutri, system_obj, "9: PrescribirPlanAlimentario()", "SynchCall"),
        (10, system_obj, nutri, "10: GuardarRegistroHistoriaClinica()", "Return")
    ]
    
    create_diagram("Diagrama de Secuencia de Roles PN1", pn1_messages)
    create_diagram("Diagrama de Secuencia de Roles PN2", pn2_messages)
    
    ea.CloseFile()
    ea.Exit()
    print("Sequence diagrams created.")

if __name__ == '__main__':
    create_sequence_diagrams(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
