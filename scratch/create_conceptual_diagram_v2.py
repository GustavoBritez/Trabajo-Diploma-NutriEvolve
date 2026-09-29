import win32com.client
import os

def create_conceptual_diagram_v2(eap_path):
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    
    # Paquete 7: Class Model
    pkg = ea.GetPackageByID(7)
    
    # 1. Eliminar clases anteriores creadas con sufijo _PN1
    old_names = ["PacienteBE_PN1", "TurnoBE_PN1", "AgendaMedicaBE_PN1", "BloqueHorarioBE_PN1", "UsuarioBE_PN1"]
    for i in range(pkg.Elements.Count - 1, -1, -1):
        el = pkg.Elements.GetAt(i)
        if el.Name in old_names:
            # Eliminar sus conectores primero
            for c_idx in range(el.Connectors.Count - 1, -1, -1):
                el.Connectors.Delete(c_idx)
            el.Connectors.Refresh()
            pkg.Elements.Delete(i)
    pkg.Elements.Refresh()

    # Helper para crear clases con atributos
    def get_or_create_class(name, attributes):
        element = None
        for el in pkg.Elements:
            if el.Name == name and el.Type == "Class":
                element = el
                break
        if not element:
            element = pkg.Elements.AddNew(name, "Class")
            element.Update()
        
        # Eliminar atributos viejos
        for i in range(element.Attributes.Count - 1, -1, -1):
            element.Attributes.Delete(i)
        element.Attributes.Refresh()
        
        # Eliminar métodos por si acaso (el diagrama conceptual es solo de atributos)
        for i in range(element.Methods.Count - 1, -1, -1):
            element.Methods.Delete(i)
        element.Methods.Refresh()

        # Agregar atributos
        for attr_name, attr_type in attributes:
            attr = element.Attributes.AddNew(attr_name, attr_type)
            attr.Update()
        
        element.Update()
        return element

    # Definición de Clases y Atributos según el Bloc de Notas (Promp Para Turnos.txt)
    # PacienteBE_DNI101 (con Tutor unificado)
    paciente_attrs = [
        ("IdPaciente_DNI101", "int"),
        ("DniNiño_DNI101", "string"),
        ("Nombre_DNI101", "string"),
        ("Apellido_DNI101", "string"),
        ("FechaNacimiento_DNI101", "DateTime"),
        ("Sexo_DNI101", "string"),
        ("ObraSocial_DNI101", "string"),
        ("DniTutor_DNI101", "string"),
        ("NombreTutor_DNI101", "string"),
        ("ApellidoTutor_DNI101", "string"),
        ("TelefonoTutor_DNI101", "string"),
        ("EmailTutor_DNI101", "string"),
        ("Parentesco_DNI101", "string"),
        ("DV", "string")
    ]
    paciente = get_or_create_class("PacienteBE_DNI101", paciente_attrs)

    # TurnoBE_DNI101
    turno_attrs = [
        ("IdTurno_DNI101", "int"),
        ("CodigoTurno_DNI101", "string"),
        ("FechaTurno_DNI101", "DateTime"),
        ("HoraTurno_DNI101", "TimeSpan"),
        ("MotivoConsulta_DNI101", "string"),
        ("EstadoTurno_DNI101", "string"),
        ("IdPaciente_DNI101", "int"),
        ("DniNutricionista_DNI101", "int"),
        ("IdBloque_DNI101", "int"),
        ("DV", "string")
    ]
    turno = get_or_create_class("TurnoBE_DNI101", turno_attrs)

    # AgendaMedicaBE_DNI101
    agenda_attrs = [
        ("IdAgenda_DNI101", "int"),
        ("Fecha_DNI101", "DateTime"),
        ("EstadoAgenda_DNI101", "string"),
        ("DniNutricionista_DNI101", "int"),
        ("DV", "string")
    ]
    agenda = get_or_create_class("AgendaMedicaBE_DNI101", agenda_attrs)

    # BloqueHorarioBE_DNI101
    bloque_attrs = [
        ("IdBloque_DNI101", "int"),
        ("IdAgenda_DNI101", "int"),
        ("HoraInicio_DNI101", "TimeSpan"),
        ("HoraFin_DNI101", "TimeSpan"),
        ("EstadoBloque_DNI101", "string"),
        ("DV", "string")
    ]
    bloque = get_or_create_class("BloqueHorarioBE_DNI101", bloque_attrs)

    # UsuarioBE (Clase existente en capa BE del proyecto base - Reutilizada)
    usuario_attrs = [
        ("DNI", "int"),
        ("Nombre", "string"),
        ("Apellido", "string"),
        ("NombreUsuario", "string"),
        ("Email", "string"),
        ("Rol", "string"),
        ("DV", "string")
    ]
    usuario = get_or_create_class("UsuarioBE", usuario_attrs)

    pkg.Elements.Refresh()

    # Limpiar conectores existentes en estas 5 clases
    for cls in [paciente, turno, agenda, bloque, usuario]:
        for i in range(cls.Connectors.Count - 1, -1, -1):
            cls.Connectors.Delete(i)
        cls.Connectors.Refresh()

    # CREACIÓN DE RELACIONES UML:
    # 1. COMPOSICIÓN: AgendaMedicaBE_DNI101 compone BloqueHorarioBE_DNI101
    # AgendaMedica es el todo, BloqueHorario es la parte (rombo negro)
    c_comp = agenda.Connectors.AddNew("", "Aggregation")
    c_comp.SupplierID = bloque.ElementID
    c_comp.Direction = "Source -> Destination"
    c_comp.ClientEnd.Cardinality = "1"
    c_comp.SupplierEnd.Cardinality = "1..*"
    c_comp.SupplierEnd.Role = "bloquesHorarios"
    c_comp.Update()
    agenda.Connectors.Refresh()
    ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 2, DestIsAggregate = 0, SubType = 'Strong', Stereotype = null WHERE Connector_ID = {c_comp.ConnectorID}")

    # 2. AGREGACIÓN: PacienteBE_DNI101 agrega TurnoBE_DNI101
    # Paciente posee turnos agendados, pero tienen ciclos de vida independientes (rombo blanco)
    c_agg_paciente = paciente.Connectors.AddNew("", "Aggregation")
    c_agg_paciente.SupplierID = turno.ElementID
    c_agg_paciente.Direction = "Source -> Destination"
    c_agg_paciente.ClientEnd.Cardinality = "1"
    c_agg_paciente.SupplierEnd.Cardinality = "0..*"
    c_agg_paciente.SupplierEnd.Role = "turnos"
    c_agg_paciente.Update()
    paciente.Connectors.Refresh()
    ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 1, DestIsAggregate = 0, SubType = 'Weak', Stereotype = null WHERE Connector_ID = {c_agg_paciente.ConnectorID}")

    # 3. AGREGACIÓN: UsuarioBE (Nutricionista) agrega AgendaMedicaBE_DNI101
    # El nutricionista administra agendas médicas (rombo blanco)
    c_agg_usuario = usuario.Connectors.AddNew("", "Aggregation")
    c_agg_usuario.SupplierID = agenda.ElementID
    c_agg_usuario.Direction = "Source -> Destination"
    c_agg_usuario.ClientEnd.Cardinality = "1"
    c_agg_usuario.SupplierEnd.Cardinality = "0..*"
    c_agg_usuario.SupplierEnd.Role = "agendas"
    c_agg_usuario.Update()
    usuario.Connectors.Refresh()
    ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 1, DestIsAggregate = 0, SubType = 'Weak', Stereotype = null WHERE Connector_ID = {c_agg_usuario.ConnectorID}")

    # 4. USO (Dependency <<use>>): TurnoBE_DNI101 usa BloqueHorarioBE_DNI101
    # El turno reserva / hace uso del bloque de horario disponible
    c_use_bloque = turno.Connectors.AddNew("", "Dependency")
    c_use_bloque.SupplierID = bloque.ElementID
    c_use_bloque.Stereotype = "use"
    c_use_bloque.Direction = "Source -> Destination"
    c_use_bloque.SupplierEnd.Role = "bloqueAsignado"
    c_use_bloque.Update()
    turno.Connectors.Refresh()

    # 5. USO (Dependency <<use>>): TurnoBE_DNI101 usa UsuarioBE (Nutricionista)
    # El turno referencia al profesional nutricionista que atenderá
    c_use_nutri = turno.Connectors.AddNew("", "Dependency")
    c_use_nutri.SupplierID = usuario.ElementID
    c_use_nutri.Stereotype = "use"
    c_use_nutri.Direction = "Source -> Destination"
    c_use_nutri.SupplierEnd.Role = "nutricionista"
    c_use_nutri.Update()
    turno.Connectors.Refresh()

    # Crear o recuperar el Diagrama
    diagram_name = "Diagrama Conceptual PN1"
    diagram = None
    for d in pkg.Diagrams:
        if d.Name == diagram_name:
            diagram = d
            break
            
    if not diagram:
        diagram = pkg.Diagrams.AddNew(diagram_name, "Logical")
        diagram.Update()
    
    # Limpiar objetos en el diagrama
    for i in range(diagram.DiagramObjects.Count - 1, -1, -1):
        diagram.DiagramObjects.Delete(i)
    diagram.DiagramObjects.Refresh()
    
    # Posicionar objetos en el diagrama con dimensiones cómodas y legibles
    def add_to_diagram(element, left, top, right, bottom):
        do = diagram.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
        do.ElementID = element.ElementID
        do.Update()

    # Layout ordenado sin cruce de líneas:
    # Paciente (izq) -> Turno (centro) -> Usuario (arriba der) -> Agenda (der)
    # Turno -> Bloque (abajo der)
    # Agenda -> Bloque (abajo der)
    add_to_diagram(paciente, 60, -60, 300, -380)
    add_to_diagram(turno, 380, -60, 610, -320)
    add_to_diagram(usuario, 700, -40, 920, -230)
    add_to_diagram(agenda, 1000, -40, 1230, -210)
    add_to_diagram(bloque, 700, -300, 930, -480)
    
    diagram.Update()
    
    # Exportar imagen del diagrama para validación visual
    project_interface = ea.GetProjectInterface()
    out_img = r'C:\Users\Danie\Desktop\GIT\TD\scratch\diagrama_conceptual_pn1.png'
    try:
        project_interface.PutDiagramImageToFile(diagram.DiagramGUID, out_img, 1)
        print("Diagram image exported to:", out_img)
    except Exception as e:
        print("Error exporting image:", e)

    ea.CloseFile()
    ea.Exit()
    print("Diagrama Conceptual PN1 updated successfully with correct DNI101 naming, composition, aggregation, and use.")

if __name__ == '__main__':
    create_conceptual_diagram_v2(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
