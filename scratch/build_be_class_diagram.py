import win32com.client
import uuid
import os

def build_be_class_diagram():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    try:
        print("--- Building BE Class Diagram (CUN01 - CUN05) ---")
        pkg = ea.GetPackageByID(7) # Package 7: Class Model

        # 1. Helper to create or get class/interface
        def get_or_create_element(name, el_type="Class"):
            for el in pkg.Elements:
                if el.Name == name and el.Type == el_type:
                    return el
            new_el = pkg.Elements.AddNew(name, el_type)
            new_el.Update()
            pkg.Elements.Refresh()
            return new_el

        # Helper to set attributes and methods
        def populate_element(el, attrs, methods):
            # Clear old attributes
            for i in range(el.Attributes.Count - 1, -1, -1):
                el.Attributes.Delete(i)
            el.Attributes.Refresh()

            for a_name, a_type in attrs:
                attr = el.Attributes.AddNew(a_name, a_type)
                attr.Update()
            el.Attributes.Refresh()

            # Clear old methods
            for i in range(el.Methods.Count - 1, -1, -1):
                el.Methods.Delete(i)
            el.Methods.Refresh()

            for m_name, m_ret, m_params in methods:
                meth = el.Methods.AddNew(m_name, m_ret)
                meth.Update()
                for p_name, p_type in m_params:
                    param = meth.Parameters.AddNew(p_name, p_type)
                    param.Update()
                meth.Parameters.Refresh()
                meth.Update()
            el.Methods.Refresh()
            el.Update()

        # Elements definition
        # 1. PacienteBE_DNI101
        paciente_attrs = [
            ("IdPaciente_DNI101", "int"),
            ("DNINiño_DNI101", "string"),
            ("Nombre_DNI101", "string"),
            ("Apellido_DNI101", "string"),
            ("Telefono_DNI101", "string"),
            ("Email_DNI101", "string"),
            ("FechaNacimiento_DNI101", "DateTime"),
            ("Sexo_DNI101", "string"),
            ("ObraSocial_DNI101", "string"),
            ("NombreCompleto", "string"),
            ("DV", "string")
        ]
        paciente_methods = [
            ("ObtenerEdadMeses", "int", [])
        ]
        el_paciente = get_or_create_element("PacienteBE_DNI101", "Class")
        populate_element(el_paciente, paciente_attrs, paciente_methods)

        # 2. TutorBE_DNI101
        tutor_attrs = [
            ("IdTutor_DNI101", "int"),
            ("DniTutor_DNI101", "string"),
            ("Nombre_DNI101", "string"),
            ("Apellido_DNI101", "string"),
            ("Telefono_DNI101", "string"),
            ("Email_DNI101", "string"),
            ("Parentesco_DNI101", "string"),
            ("FechaRegistro_DNI101", "DateTime"),
            ("NombreCompleto", "string"),
            ("DV", "string")
        ]
        el_tutor = get_or_create_element("TutorBE_DNI101", "Class")
        populate_element(el_tutor, tutor_attrs, [])

        # 3. TurnoBE_DNI101
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
        turno_methods = [
            ("CambiarEstado", "void", [("nuevoEstado", "IEstadoTurno_DNI101")]),
            ("ConfigurarEstadoPorNombre", "void", [("nombreEstado", "string")]),
            ("Atender", "void", []),
            ("Cancelar", "void", [("motivo", "string")]),
            ("Reprogramar", "void", [("nuevaFecha", "DateTime"), ("nuevaHora", "TimeSpan"), ("nuevoIdBloque", "int")])
        ]
        el_turno = get_or_create_element("TurnoBE_DNI101", "Class")
        populate_element(el_turno, turno_attrs, turno_methods)

        # 4. AgendaMedicaBE_DNI101
        agenda_attrs = [
            ("IdAgenda_DNI101", "int"),
            ("Fecha_DNI101", "DateTime"),
            ("EstadoAgenda_DNI101", "string"),
            ("DniNutricionista_DNI101", "int"),
            ("DV", "string")
        ]
        el_agenda = get_or_create_element("AgendaMedicaBE_DNI101", "Class")
        populate_element(el_agenda, agenda_attrs, [])

        # 5. BloqueHorarioBE_DNI101
        bloque_attrs = [
            ("IdBloque_DNI101", "int"),
            ("IdAgenda_DNI101", "int"),
            ("HoraInicio_DNI101", "TimeSpan"),
            ("HoraFin_DNI101", "TimeSpan"),
            ("EstadoBloque_DNI101", "string"),
            ("DescripcionHorario", "string"),
            ("DV", "string")
        ]
        bloque_methods = [
            ("ToString", "string", [])
        ]
        el_bloque = get_or_create_element("BloqueHorarioBE_DNI101", "Class")
        populate_element(el_bloque, bloque_attrs, bloque_methods)

        # 6. UsuarioBE
        usuario_attrs = [
            ("DNI", "int"),
            ("Nombre", "string"),
            ("Apellido", "string"),
            ("NombreUsuario", "string"),
            ("Email", "string"),
            ("Rol", "string"),
            ("DV", "string")
        ]
        el_usuario = get_or_create_element("UsuarioBE", "Class")
        populate_element(el_usuario, usuario_attrs, [])

        # 7. IEstadoTurno_DNI101 (Interface)
        state_attrs = [
            ("NombreEstado", "string")
        ]
        state_methods = [
            ("Atender", "void", [("turno", "TurnoBE_DNI101")]),
            ("Cancelar", "void", [("turno", "TurnoBE_DNI101"), ("motivo", "string")]),
            ("Reprogramar", "void", [("turno", "TurnoBE_DNI101"), ("nuevaFecha", "DateTime"), ("nuevaHora", "TimeSpan"), ("nuevoIdBloque", "int")])
        ]
        el_state_intf = get_or_create_element("IEstadoTurno_DNI101", "Interface")
        populate_element(el_state_intf, state_attrs, state_methods)

        # 8-11 Concrete States
        concrete_states = [
            "TurnoSolicitadoState_DNI101",
            "TurnoConfirmadoState_DNI101",
            "TurnoAsistioState_DNI101",
            "TurnoCanceladoState_DNI101"
        ]
        state_elements = []
        for sname in concrete_states:
            s_el = get_or_create_element(sname, "Class")
            populate_element(s_el, state_attrs, state_methods)
            state_elements.append(s_el)

        pkg.Elements.Refresh()

        # All classes in this model
        all_elements = [el_paciente, el_tutor, el_turno, el_agenda, el_bloque, el_usuario, el_state_intf] + state_elements

        # Clean existing connectors between these elements
        all_ids = [e.ElementID for e in all_elements]
        id_list_str = ",".join(str(i) for i in all_ids)
        ea.Execute(f"DELETE FROM t_connector WHERE Start_Object_ID IN ({id_list_str}) AND End_Object_ID IN ({id_list_str})")

        for el in all_elements:
            el.Connectors.Refresh()

        # CREATE RELATIONSHIPS:
        # A. COMPOSICIÓN (Composition): AgendaMedicaBE_DNI101 ◆---> BloqueHorarioBE_DNI101
        # Agenda is the whole, BloqueHorario is the part
        c_comp_agenda = el_agenda.Connectors.AddNew("", "Aggregation")
        c_comp_agenda.SupplierID = el_bloque.ElementID
        c_comp_agenda.Direction = "Source -> Destination"
        c_comp_agenda.ClientEnd.Cardinality = "1"
        c_comp_agenda.SupplierEnd.Cardinality = "1..*"
        c_comp_agenda.SupplierEnd.Role = "BloquesHorarios_DNI101"
        c_comp_agenda.Update()
        el_agenda.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 2, DestIsAggregate = 0, SubType = 'Strong', Stereotype = null WHERE Connector_ID = {c_comp_agenda.ConnectorID}")

        # B. COMPOSICIÓN (State Pattern): TurnoBE_DNI101 ◆---> IEstadoTurno_DNI101
        # Turno manages and contains its current state instance
        c_comp_state = el_turno.Connectors.AddNew("", "Aggregation")
        c_comp_state.SupplierID = el_state_intf.ElementID
        c_comp_state.Direction = "Source -> Destination"
        c_comp_state.ClientEnd.Cardinality = "1"
        c_comp_state.SupplierEnd.Cardinality = "1"
        c_comp_state.SupplierEnd.Role = "EstadoActual_DNI101"
        c_comp_state.Update()
        el_turno.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 2, DestIsAggregate = 0, SubType = 'Strong', Stereotype = null WHERE Connector_ID = {c_comp_state.ConnectorID}")

        # C. AGREGACIÓN: TutorBE_DNI101 ◇---> PacienteBE_DNI101
        # Tutor has patients (children), but they have independent lifecycles
        c_agg_tutor = el_tutor.Connectors.AddNew("", "Aggregation")
        c_agg_tutor.SupplierID = el_paciente.ElementID
        c_agg_tutor.Direction = "Source -> Destination"
        c_agg_tutor.ClientEnd.Cardinality = "1"
        c_agg_tutor.SupplierEnd.Cardinality = "1..*"
        c_agg_tutor.SupplierEnd.Role = "Pacientes_DNI101"
        c_agg_tutor.Update()
        el_tutor.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 1, DestIsAggregate = 0, SubType = 'Weak', Stereotype = null WHERE Connector_ID = {c_agg_tutor.ConnectorID}")

        # D. AGREGACIÓN: PacienteBE_DNI101 ◇---> TurnoBE_DNI101
        # Paciente aggregates appointments
        c_agg_pac = el_paciente.Connectors.AddNew("", "Aggregation")
        c_agg_pac.SupplierID = el_turno.ElementID
        c_agg_pac.Direction = "Source -> Destination"
        c_agg_pac.ClientEnd.Cardinality = "1"
        c_agg_pac.SupplierEnd.Cardinality = "0..*"
        c_agg_pac.SupplierEnd.Role = "Turnos"
        c_agg_pac.Update()
        el_paciente.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 1, DestIsAggregate = 0, SubType = 'Weak', Stereotype = null WHERE Connector_ID = {c_agg_pac.ConnectorID}")

        # E. AGREGACIÓN: UsuarioBE (Nutricionista) ◇---> AgendaMedicaBE_DNI101
        c_agg_user = el_usuario.Connectors.AddNew("", "Aggregation")
        c_agg_user.SupplierID = el_agenda.ElementID
        c_agg_user.Direction = "Source -> Destination"
        c_agg_user.ClientEnd.Cardinality = "1"
        c_agg_user.SupplierEnd.Cardinality = "0..*"
        c_agg_user.SupplierEnd.Role = "Agendas"
        c_agg_user.Update()
        el_usuario.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 1, DestIsAggregate = 0, SubType = 'Weak', Stereotype = null WHERE Connector_ID = {c_agg_user.ConnectorID}")

        # F. USO (Dependency <<use>>): TurnoBE_DNI101 ---> BloqueHorarioBE_DNI101
        c_use_bloque = el_turno.Connectors.AddNew("", "Dependency")
        c_use_bloque.SupplierID = el_bloque.ElementID
        c_use_bloque.Stereotype = "use"
        c_use_bloque.Direction = "Source -> Destination"
        c_use_bloque.SupplierEnd.Role = "IdBloque_DNI101"
        c_use_bloque.Update()
        el_turno.Connectors.Refresh()

        # G. USO (Dependency <<use>>): TurnoBE_DNI101 ---> UsuarioBE (Nutricionista asignado)
        c_use_nutri = el_turno.Connectors.AddNew("", "Dependency")
        c_use_nutri.SupplierID = el_usuario.ElementID
        c_use_nutri.Stereotype = "use"
        c_use_nutri.Direction = "Source -> Destination"
        c_use_nutri.SupplierEnd.Role = "DniNutricionista"
        c_use_nutri.Update()
        el_turno.Connectors.Refresh()

        # H. Realización (Realization): Concrete States implement IEstadoTurno_DNI101
        for s_el in state_elements:
            u_guid = "{" + str(uuid.uuid4()).upper() + "}"
            sql = f"INSERT INTO t_connector (Connector_Type, Direction, Start_Object_ID, End_Object_ID, ea_guid) VALUES ('Realisation', 'Source -> Destination', {s_el.ElementID}, {el_state_intf.ElementID}, '{u_guid}')"
            ea.Execute(sql)
            s_el.Connectors.Refresh()
        el_state_intf.Connectors.Refresh()

        # DIAGRAM SETUP
        diag_name = "Diagrama de Clases BE (CUN01 - CUN05)"
        diagram = None
        for d in pkg.Diagrams:
            if d.Name == diag_name:
                diagram = d
                break

        if not diagram:
            diagram = pkg.Diagrams.AddNew(diag_name, "Logical")
            diagram.Update()

        # Clear objects in diagram
        for i in range(diagram.DiagramObjects.Count - 1, -1, -1):
            diagram.DiagramObjects.Delete(i)
        diagram.DiagramObjects.Refresh()

        # LAYOUT:
        # Row 1 (Top): TutorBE (left) -> PacienteBE (center) -> UsuarioBE (right) -> AgendaMedicaBE (far-right)
        # Row 2 (Mid-upper): TurnoBE (center) -> BloqueHorarioBE (right)
        # Row 3 (Mid-lower): IEstadoTurno_DNI101 (center, directly below TurnoBE)
        # Row 4 (Bottom): 4 Concrete States under IEstadoTurno
        
        layout = [
            (el_tutor,         60,   -40,  320, -280),
            (el_paciente,     440,   -40,  740, -320),
            (el_usuario,      860,   -40, 1100, -250),
            (el_agenda,      1220,   -40, 1480, -230),
            (el_turno,        440,  -380,  760, -710),
            (el_bloque,       980,  -380, 1240, -590),
            (el_state_intf,   460,  -760,  740, -920),
            (state_elements[0],   40,  -980,  320, -1160), # Solicitado
            (state_elements[1],  360,  -980,  640, -1160), # Confirmado
            (state_elements[2],  680,  -980,  960, -1160), # Asistió
            (state_elements[3], 1000,  -980, 1280, -1160), # Cancelado
        ]

        for el, l, t, r, b in layout:
            d_obj = diagram.DiagramObjects.AddNew(f"l={l};r={r};t={t};b={b};", "")
            d_obj.ElementID = el.ElementID
            d_obj.left = l
            d_obj.right = r
            d_obj.top = t
            d_obj.bottom = b
            d_obj.Update()

        diagram.DiagramObjects.Refresh()
        diagram.Update()

        print(f"Diagram '{diag_name}' created/updated successfully with ID {diagram.DiagramID}.")

        # Export image
        out_img = r"C:\Users\Danie\Desktop\GIT\TD\scratch\diagrama_clases_be_cun01_05.png"
        proj = ea.GetProjectInterface()
        proj.PutDiagramImageToFile(diagram.DiagramGUID, out_img, 1)
        print("Exported diagram image to:", out_img)

    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    build_be_class_diagram()
