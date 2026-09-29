import win32com.client

def create_conceptual_diagram(eap_path):
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    
    # Usaremos el paquete 7 (Class Model) para poner las clases y el diagrama
    pkg = ea.GetPackageByID(7)
    
    # Helper para crear clases y atributos
    def create_class_with_attributes(name, attributes):
        # Buscar si ya existe la clase en el paquete
        element = None
        for el in pkg.Elements:
            if el.Name == name and el.Type == "Class":
                element = el
                break
        
        if not element:
            element = pkg.Elements.AddNew(name, "Class")
            element.Update()
        
        # Eliminar atributos existentes si estamos actualizando
        for i in range(element.Attributes.Count - 1, -1, -1):
            element.Attributes.Delete(i)
        element.Attributes.Refresh()
        
        # Crear nuevos atributos
        for attr_name, attr_type in attributes:
            attr = element.Attributes.AddNew(attr_name, attr_type)
            attr.Update()
        
        element.Update()
        return element

    # 1. Definir atributos para PacienteBE (Tutor fusionado)
    paciente_attrs = [
        ("IdPaciente", "int"),
        ("DniNiño", "string"),
        ("Nombre", "string"),
        ("Apellido", "string"),
        ("FechaNacimiento", "DateTime"),
        ("Sexo", "string"),
        ("ObraSocial", "string"),
        ("DniTutor", "string"),
        ("NombreTutor", "string"),
        ("ApellidoTutor", "string"),
        ("TelefonoTutor", "string"),
        ("EmailTutor", "string"),
        ("Parentesco", "string"),
        ("DV", "string")
    ]
    paciente = create_class_with_attributes("PacienteBE_PN1", paciente_attrs)
    
    # 2. TurnoBE
    turno_attrs = [
        ("IdTurno", "int"),
        ("CodigoTurno", "string"),
        ("FechaTurno", "DateTime"),
        ("HoraTurno", "TimeSpan"),
        ("MotivoConsulta", "string"),
        ("EstadoTurno", "string"),
        ("DV", "string")
    ]
    turno = create_class_with_attributes("TurnoBE_PN1", turno_attrs)
    
    # 3. AgendaMedicaBE
    agenda_attrs = [
        ("IdAgenda", "int"),
        ("Fecha", "DateTime"),
        ("EstadoAgenda", "string"),
        ("DV", "string")
    ]
    agenda = create_class_with_attributes("AgendaMedicaBE_PN1", agenda_attrs)
    
    # 4. BloqueHorarioBE
    bloque_attrs = [
        ("IdBloque", "int"),
        ("HoraInicio", "TimeSpan"),
        ("HoraFin", "TimeSpan"),
        ("EstadoBloque", "string"),
        ("DV", "string")
    ]
    bloque = create_class_with_attributes("BloqueHorarioBE_PN1", bloque_attrs)
    
    # 5. UsuarioBE (Nutricionista)
    usuario_attrs = [
        ("DNI", "int"),
        ("Nombre", "string"),
        ("Apellido", "string"),
        ("NombreUsuario", "string"),
        ("Email", "string"),
        ("Rol", "string")
    ]
    usuario = create_class_with_attributes("UsuarioBE_PN1", usuario_attrs)
    
    pkg.Elements.Refresh()

    # Eliminar conectores previos de estas clases para hacer limpieza
    for cls in [paciente, turno, agenda, bloque, usuario]:
        for i in range(cls.Connectors.Count - 1, -1, -1):
            cls.Connectors.Delete(i)
        cls.Connectors.Refresh()

    # Relaciones (Asociaciones)
    def create_association(src, tgt, src_role, tgt_role, src_mult, tgt_mult):
        conn = src.Connectors.AddNew("", "Association")
        conn.SupplierID = tgt.ElementID
        conn.Direction = "Source -> Destination"
        conn.ClientEnd.Role = src_role
        conn.ClientEnd.Cardinality = src_mult
        conn.SupplierEnd.Role = tgt_role
        conn.SupplierEnd.Cardinality = tgt_mult
        conn.Update()
        src.Connectors.Refresh()

    create_association(paciente, turno, "", "turnos", "1", "0..*")
    create_association(usuario, agenda, "", "agendas", "1", "0..*")
    create_association(agenda, bloque, "", "bloques", "1", "1..*")
    create_association(turno, bloque, "", "bloqueAsignado", "0..1", "1")
    create_association(turno, usuario, "", "profesional", "0..*", "1")

    # Crear el Diagrama
    diagram_name = "Diagrama Conceptual PN1"
    diagram = None
    for d in pkg.Diagrams:
        if d.Name == diagram_name:
            diagram = d
            break
            
    if not diagram:
        diagram = pkg.Diagrams.AddNew(diagram_name, "Logical")
        diagram.Update()
    
    # Limpiar diagrama
    for i in range(diagram.DiagramObjects.Count - 1, -1, -1):
        diagram.DiagramObjects.Delete(i)
    diagram.DiagramObjects.Refresh()
    
    # Posicionar objetos en el diagrama
    def add_to_diagram(element, left, top, right, bottom):
        do = diagram.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
        do.ElementID = element.ElementID
        do.Update()
        
    add_to_diagram(usuario, 50, -50, 200, -180)
    add_to_diagram(agenda, 300, -50, 450, -150)
    add_to_diagram(bloque, 550, -50, 700, -180)
    add_to_diagram(turno, 300, -250, 450, -420)
    add_to_diagram(paciente, 50, -250, 250, -500)
    
    diagram.Update()
    
    # Configurar el diagrama
    diagram.Update()
    
    ea.CloseFile()
    ea.Exit()
    print("Conceptual Diagram created.")

if __name__ == '__main__':
    create_conceptual_diagram(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
