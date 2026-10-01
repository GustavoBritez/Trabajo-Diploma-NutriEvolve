import win32com.client
import os
import shutil

def build_pure_be_diagram():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    try:
        print("--- Building PURE BE Class Diagram (CUN 01 - CUN 05) ---")
        pkg = ea.GetPackageByID(7) # Package 7: Class Model

        # Helper to get or create element
        def get_or_create_element(name, el_type="Class"):
            for el in pkg.Elements:
                if el.Name == name and el.Type == el_type:
                    return el
            new_el = pkg.Elements.AddNew(name, el_type)
            new_el.Update()
            pkg.Elements.Refresh()
            return new_el

        # Helper to populate ONLY attributes (and delete all methods)
        def populate_attributes_only(el, attrs):
            # Clear all attributes
            for i in range(el.Attributes.Count - 1, -1, -1):
                el.Attributes.Delete(i)
            el.Attributes.Refresh()

            # Add attributes
            for a_name, a_type in attrs:
                attr = el.Attributes.AddNew(a_name, a_type)
                attr.Update()
            el.Attributes.Refresh()

            # Clear ALL methods (strictly no methods as requested by user)
            for i in range(el.Methods.Count - 1, -1, -1):
                el.Methods.Delete(i)
            el.Methods.Refresh()
            el.Update()

        # 1. TutorBE_DNI101
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
        populate_attributes_only(el_tutor, tutor_attrs)

        # 2. PacienteBE_DNI101
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
        el_paciente = get_or_create_element("PacienteBE_DNI101", "Class")
        populate_attributes_only(el_paciente, paciente_attrs)

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
            ("IdBloque_DNI101", "int?"),
            ("DV", "string")
        ]
        el_turno = get_or_create_element("TurnoBE_DNI101", "Class")
        populate_attributes_only(el_turno, turno_attrs)

        # 4. AgendaMedicaBE_DNI101
        agenda_attrs = [
            ("IdAgenda_DNI101", "int"),
            ("Fecha_DNI101", "DateTime"),
            ("EstadoAgenda_DNI101", "string"),
            ("DniNutricionista_DNI101", "int"),
            ("DV", "string")
        ]
        el_agenda = get_or_create_element("AgendaMedicaBE_DNI101", "Class")
        populate_attributes_only(el_agenda, agenda_attrs)

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
        el_bloque = get_or_create_element("BloqueHorarioBE_DNI101", "Class")
        populate_attributes_only(el_bloque, bloque_attrs)

        pkg.Elements.Refresh()

        be_elements = [el_tutor, el_paciente, el_turno, el_agenda, el_bloque]
        be_ids = [e.ElementID for e in be_elements]
        be_id_str = ",".join(str(i) for i in be_ids)

        # Clean connectors between these elements or connected to them
        ea.Execute(f"DELETE FROM t_connector WHERE Start_Object_ID IN ({be_id_str}) OR End_Object_ID IN ({be_id_str})")
        for el in be_elements:
            el.Connectors.Refresh()

        # Check and remove any unwanted elements like UsuarioBE or State elements from diagram 59
        diag = ea.GetDiagramByID(59)
        diag.Name = "Diagrama de Clases BE (CUN01 - CUN05)"
        diag.Notes = "Diagrama de Clases de la capa BE participantes en los Casos de Uso CUN01 al CUN05 (Atributos unicamente, sin metodos)."
        diag.Update()

        # Clear existing DiagramObjects in Diagram 59
        for i in range(diag.DiagramObjects.Count - 1, -1, -1):
            diag.DiagramObjects.Delete(i)
        diag.DiagramObjects.Refresh()

        # CREATE RELATIONSHIPS:
        # 1. Agregacion: TutorBE_DNI101 ◇---> PacienteBE_DNI101
        c_agg_tutor = el_tutor.Connectors.AddNew("", "Aggregation")
        c_agg_tutor.SupplierID = el_paciente.ElementID
        c_agg_tutor.Direction = "Source -> Destination"
        c_agg_tutor.ClientEnd.Cardinality = "1"
        c_agg_tutor.SupplierEnd.Cardinality = "1..*"
        c_agg_tutor.SupplierEnd.Role = "Pacientes_DNI101"
        c_agg_tutor.Update()
        el_tutor.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 1, DestIsAggregate = 0, SubType = 'Weak', Stereotype = null WHERE Connector_ID = {c_agg_tutor.ConnectorID}")

        # 2. Agregacion: PacienteBE_DNI101 ◇---> TurnoBE_DNI101
        c_agg_pac = el_paciente.Connectors.AddNew("", "Aggregation")
        c_agg_pac.SupplierID = el_turno.ElementID
        c_agg_pac.Direction = "Source -> Destination"
        c_agg_pac.ClientEnd.Cardinality = "1"
        c_agg_pac.SupplierEnd.Cardinality = "0..*"
        c_agg_pac.SupplierEnd.Role = "Turnos"
        c_agg_pac.Update()
        el_paciente.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 1, DestIsAggregate = 0, SubType = 'Weak', Stereotype = null WHERE Connector_ID = {c_agg_pac.ConnectorID}")

        # 3. Composicion: AgendaMedicaBE_DNI101 ◆---> BloqueHorarioBE_DNI101
        c_comp_agenda = el_agenda.Connectors.AddNew("", "Aggregation")
        c_comp_agenda.SupplierID = el_bloque.ElementID
        c_comp_agenda.Direction = "Source -> Destination"
        c_comp_agenda.ClientEnd.Cardinality = "1"
        c_comp_agenda.SupplierEnd.Cardinality = "1..*"
        c_comp_agenda.SupplierEnd.Role = "BloquesHorarios_DNI101"
        c_comp_agenda.Update()
        el_agenda.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 2, DestIsAggregate = 0, SubType = 'Strong', Stereotype = null WHERE Connector_ID = {c_comp_agenda.ConnectorID}")

        # 4. Uso: TurnoBE_DNI101 - - - > BloqueHorarioBE_DNI101 («use»)
        c_use_bloque = el_turno.Connectors.AddNew("", "Dependency")
        c_use_bloque.SupplierID = el_bloque.ElementID
        c_use_bloque.Stereotype = "use"
        c_use_bloque.Direction = "Source -> Destination"
        c_use_bloque.SupplierEnd.Role = "IdBloque_DNI101"
        c_use_bloque.Update()
        el_turno.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET Stereotype = 'use' WHERE Connector_ID = {c_use_bloque.ConnectorID}")

        # LAYOUT IN DIAGRAM 59:
        # Perfectly aligned 2-row layout with zero crossings
        # Row 1 (y: 60 to 320):
        #   TutorBE_DNI101 (x: 50 to 310) -> PacienteBE_DNI101 (x: 430 to 690) -> AgendaMedicaBE_DNI101 (x: 820 to 1080)
        # Row 2 (y: 390 to 670):
        #   TurnoBE_DNI101 (x: 430 to 690) -> BloqueHorarioBE_DNI101 (x: 820 to 1080)

        coords = [
            (el_tutor, 50, 60, 320, 310),       # Left, Top, Right, Bottom
            (el_paciente, 430, 60, 710, 330),
            (el_agenda, 830, 60, 1100, 240),
            (el_turno, 430, 420, 710, 700),
            (el_bloque, 830, 420, 1100, 640)
        ]

        for el, left, top, right, bottom in coords:
            do = diag.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag.DiagramObjects.Refresh()
        diag.Update()
        ea.SaveDiagram(diag.DiagramID)

        # Clear all diagram links styles to default direct orthogonal
        ea.Execute(f"UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 59")

        # Export diagram image
        scratch_dir = r"c:\Users\Danie\Desktop\GIT\TD\scratch"
        out_png = os.path.join(scratch_dir, "diagrama_clases_be_cun01_05.png")
        if os.path.exists(out_png):
            os.remove(out_png)

        proj.PutDiagramImageToFile(diag.DiagramGUID, out_png, 1)
        print("Diagram exported successfully to:", out_png)

        # Copy to artifacts directory
        artifact_dir = r"C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e"
        dest_png = os.path.join(artifact_dir, "diagrama_clases_be_cun01_05.png")
        shutil.copy2(out_png, dest_png)
        print("Copied to brain artifact:", dest_png)

    finally:
        ea.CloseFile()
        ea.Exit()
        print("EA closed.")

if __name__ == "__main__":
    build_pure_be_diagram()
