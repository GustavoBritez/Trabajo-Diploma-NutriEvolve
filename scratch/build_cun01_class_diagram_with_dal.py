import win32com.client
import os
import shutil

def build_cun01_class_diagram_with_dal():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    try:
        print("--- Building CUN01 Class Diagram (BLL + DAL + BE) ---")
        pkg = ea.GetPackageByID(7) # Package 7: Class Model

        # Helper to get or create element in Package 7
        def get_or_create_element(name, el_type="Class"):
            for el in pkg.Elements:
                if el.Name == name and el.Type == el_type:
                    return el
            new_el = pkg.Elements.AddNew(name, el_type)
            new_el.Update()
            pkg.Elements.Refresh()
            return new_el

        # Helper for BE classes: populate ONLY attributes, delete all methods
        def populate_attributes_only(el, attrs):
            for i in range(el.Attributes.Count - 1, -1, -1):
                el.Attributes.Delete(i)
            el.Attributes.Refresh()

            for a_name, a_type in attrs:
                attr = el.Attributes.AddNew(a_name, a_type)
                attr.Visibility = "Private"
                attr.Update()
            el.Attributes.Refresh()

            for i in range(el.Methods.Count - 1, -1, -1):
                el.Methods.Delete(i)
            el.Methods.Refresh()
            el.Update()

        # Helper for BLL and DAL classes: populate ONLY methods, delete all attributes
        def populate_methods_only(el, methods):
            for i in range(el.Attributes.Count - 1, -1, -1):
                el.Attributes.Delete(i)
            el.Attributes.Refresh()

            for i in range(el.Methods.Count - 1, -1, -1):
                el.Methods.Delete(i)
            el.Methods.Refresh()

            for m_name, m_ret, m_params in methods:
                meth = el.Methods.AddNew(m_name, m_ret)
                meth.Visibility = "Public"
                meth.Update()
                pos = 0
                for p_name, p_type in m_params:
                    param = meth.Parameters.AddNew(p_name, p_type)
                    param.Position = pos
                    param.Update()
                    pos += 1
                meth.Parameters.Refresh()
                meth.Update()
            el.Methods.Refresh()
            el.Update()

        # ==============================================================
        # 1. BE ENTITIES (Attributes only)
        # ==============================================================
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
        el_tutor_be = get_or_create_element("TutorBE_DNI101", "Class")
        populate_attributes_only(el_tutor_be, tutor_attrs)

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
        el_paciente_be = get_or_create_element("PacienteBE_DNI101", "Class")
        populate_attributes_only(el_paciente_be, paciente_attrs)

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
        el_turno_be = get_or_create_element("TurnoBE_DNI101", "Class")
        populate_attributes_only(el_turno_be, turno_attrs)

        agenda_attrs = [
            ("IdAgenda_DNI101", "int"),
            ("Fecha_DNI101", "DateTime"),
            ("EstadoAgenda_DNI101", "string"),
            ("DniNutricionista_DNI101", "int"),
            ("DV", "string")
        ]
        el_agenda_be = get_or_create_element("AgendaMedicaBE_DNI101", "Class")
        populate_attributes_only(el_agenda_be, agenda_attrs)

        bloque_attrs = [
            ("IdBloque_DNI101", "int"),
            ("IdAgenda_DNI101", "int"),
            ("HoraInicio_DNI101", "TimeSpan"),
            ("HoraFin_DNI101", "TimeSpan"),
            ("EstadoBloque_DNI101", "string"),
            ("DescripcionHorario", "string"),
            ("DV", "string")
        ]
        el_bloque_be = get_or_create_element("BloqueHorarioBE_DNI101", "Class")
        populate_attributes_only(el_bloque_be, bloque_attrs)

        # ==============================================================
        # 2. BLL LOGIC CLASSES (Methods only)
        # ==============================================================
        turno_bll_methods = [
            ("RegistrarTurno", "TurnoBE_DNI101", [
                ("dniNiño", "string"),
                ("fecha", "DateTime"),
                ("horario", "string"),
                ("motivo", "string"),
                ("idBloque", "int?"),
                ("dniNutricionista", "int?")
            ]),
            ("ExisteSuperposicion", "bool", [
                ("dniNutricionista", "int"),
                ("fecha", "DateTime"),
                ("hora", "TimeSpan")
            ]),
            ("ListarTurnos", "List<TurnoBE_DNI101>", []),
            ("ListarTurnosPorFecha", "List<TurnoBE_DNI101>", [
                ("fecha", "DateTime")
            ]),
            ("ObtenerPorCodigo", "TurnoBE_DNI101", [
                ("codigo", "string")
            ])
        ]
        el_turno_bll = get_or_create_element("TurnoBLL_DNI101", "Class")
        populate_methods_only(el_turno_bll, turno_bll_methods)

        paciente_bll_methods = [
            ("ObtenerPacientePorDNI", "PacienteBE_DNI101", [
                ("dniNiño", "string")
            ]),
            ("RegistrarPaciente", "int", [
                ("nombre", "string"),
                ("apellido", "string"),
                ("dniNiño", "string"),
                ("telefono", "string"),
                ("email", "string"),
                ("obraSocial", "string")
            ]),
            ("ListarPacientes", "List<PacienteBE_DNI101>", []),
            ("ObtenerPorId", "PacienteBE_DNI101", [
                ("idPaciente", "int")
            ]),
            ("ModificarPaciente", "bool", [
                ("paciente", "PacienteBE_DNI101")
            ]),
            ("RegistrarTutor", "int", [
                ("tutor", "TutorBE_DNI101")
            ]),
            ("ObtenerTutorPorDNI", "TutorBE_DNI101", [
                ("dniTutor", "string")
            ])
        ]
        el_paciente_bll = get_or_create_element("PacienteBLL_DNI101", "Class")
        populate_methods_only(el_paciente_bll, paciente_bll_methods)

        agenda_bll_methods = [
            ("ListarBloquesDisponibles", "List<BloqueHorarioBE_DNI101>", [
                ("fecha", "DateTime"),
                ("dniNutricionista", "int")
            ]),
            ("ActualizarEstadoBloque", "bool", [
                ("idBloque", "int"),
                ("nuevoEstado", "string")
            ]),
            ("ObtenerAgendaPorFecha", "AgendaMedicaBE_DNI101", [
                ("fecha", "DateTime"),
                ("dniNutricionista", "int")
            ])
        ]
        el_agenda_bll = get_or_create_element("AgendaMedicaBLL_DNI101", "Class")
        populate_methods_only(el_agenda_bll, agenda_bll_methods)

        # ==============================================================
        # 3. DAL DATA ACCESS CLASSES (Methods only)
        # ==============================================================
        turno_dal_methods = [
            ("Guardar", "int", [
                ("turno", "TurnoBE_DNI101")
            ]),
            ("ExisteTurnoParaProfesional", "bool", [
                ("dniNutricionista", "int"),
                ("fecha", "DateTime"),
                ("hora", "TimeSpan")
            ]),
            ("ObtenerPorId", "TurnoBE_DNI101", [
                ("idTurno", "int")
            ]),
            ("ListarTurnosPorFecha", "List<TurnoBE_DNI101>", [
                ("fecha", "DateTime")
            ]),
            ("ListarTodos", "List<TurnoBE_DNI101>", [])
        ]
        el_turno_dal = get_or_create_element("TurnoDAL_DNI101", "Class")
        populate_methods_only(el_turno_dal, turno_dal_methods)

        paciente_dal_methods = [
            ("Guardar", "int", [
                ("paciente", "PacienteBE_DNI101")
            ]),
            ("ObtenerPacientePorDNI", "PacienteBE_DNI101", [
                ("dniNiño", "string")
            ]),
            ("ObtenerPorId", "PacienteBE_DNI101", [
                ("idPaciente", "int")
            ]),
            ("Modificar", "bool", [
                ("paciente", "PacienteBE_DNI101")
            ]),
            ("ListarTodos", "List<PacienteBE_DNI101>", [])
        ]
        el_paciente_dal = get_or_create_element("PacienteDAL_DNI101", "Class")
        populate_methods_only(el_paciente_dal, paciente_dal_methods)

        agenda_dal_methods = [
            ("ListarBloquesDisponibles", "List<BloqueHorarioBE_DNI101>", [
                ("fecha", "DateTime"),
                ("dniNutricionista", "int")
            ]),
            ("ActualizarEstadoBloque", "bool", [
                ("idBloque", "int"),
                ("nuevoEstado", "string")
            ])
        ]
        el_agenda_dal = get_or_create_element("AgendaMedicaDAL_DNI101", "Class")
        populate_methods_only(el_agenda_dal, agenda_dal_methods)

        pkg.Elements.Refresh()

        all_elements = [
            el_tutor_be, el_paciente_be, el_turno_be, el_agenda_be, el_bloque_be,
            el_turno_bll, el_paciente_bll, el_agenda_bll,
            el_turno_dal, el_paciente_dal, el_agenda_dal
        ]
        all_ids = [e.ElementID for e in all_elements]
        id_str = ",".join(str(i) for i in all_ids)

        # Delete existing connectors between these specific elements to rebuild fresh
        ea.Execute(f"DELETE FROM t_connector WHERE Start_Object_ID IN ({id_str}) AND End_Object_ID IN ({id_str})")
        for el in all_elements:
            el.Connectors.Refresh()

        # ==============================================================
        # 4. CREATE UML CONNECTORS
        # ==============================================================
        # A. AGREGACIÓN (Weak Aggregation ◇)
        # 1. TutorBE_DNI101 ◇---> PacienteBE_DNI101
        c_agg_tutor = el_tutor_be.Connectors.AddNew("", "Aggregation")
        c_agg_tutor.SupplierID = el_paciente_be.ElementID
        c_agg_tutor.Direction = "Source -> Destination"
        c_agg_tutor.ClientEnd.Cardinality = "1"
        c_agg_tutor.SupplierEnd.Cardinality = "1..*"
        c_agg_tutor.SupplierEnd.Role = "Pacientes_DNI101"
        c_agg_tutor.Update()
        el_tutor_be.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 1, DestIsAggregate = 0, SubType = 'Weak', Stereotype = null WHERE Connector_ID = {c_agg_tutor.ConnectorID}")

        # 2. PacienteBE_DNI101 ◇---> TurnoBE_DNI101
        c_agg_pac = el_paciente_be.Connectors.AddNew("", "Aggregation")
        c_agg_pac.SupplierID = el_turno_be.ElementID
        c_agg_pac.Direction = "Source -> Destination"
        c_agg_pac.ClientEnd.Cardinality = "1"
        c_agg_pac.SupplierEnd.Cardinality = "0..*"
        c_agg_pac.SupplierEnd.Role = "Turnos"
        c_agg_pac.Update()
        el_paciente_be.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 1, DestIsAggregate = 0, SubType = 'Weak', Stereotype = null WHERE Connector_ID = {c_agg_pac.ConnectorID}")

        # B. COMPOSICIÓN (Strong Composite ◆)
        # 3. AgendaMedicaBE_DNI101 ◆---> BloqueHorarioBE_DNI101
        c_comp_agenda = el_agenda_be.Connectors.AddNew("", "Aggregation")
        c_comp_agenda.SupplierID = el_bloque_be.ElementID
        c_comp_agenda.Direction = "Source -> Destination"
        c_comp_agenda.ClientEnd.Cardinality = "1"
        c_comp_agenda.SupplierEnd.Cardinality = "1..*"
        c_comp_agenda.SupplierEnd.Role = "BloquesHorarios_DNI101"
        c_comp_agenda.Update()
        el_agenda_be.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 2, DestIsAggregate = 0, SubType = 'Strong', Stereotype = null WHERE Connector_ID = {c_comp_agenda.ConnectorID}")

        # C. USO («use» / Dependency)
        def add_use_dependency(source_el, target_el, role=""):
            conn = source_el.Connectors.AddNew("", "Dependency")
            conn.SupplierID = target_el.ElementID
            conn.Stereotype = "use"
            conn.Direction = "Source -> Destination"
            if role:
                conn.SupplierEnd.Role = role
            conn.Update()
            source_el.Connectors.Refresh()
            ea.Execute(f"UPDATE t_connector SET Stereotype = 'use' WHERE Connector_ID = {conn.ConnectorID}")
            return conn

        # 4. TurnoBE_DNI101 - - - > BloqueHorarioBE_DNI101 («use»)
        add_use_dependency(el_turno_be, el_bloque_be, "IdBloque_DNI101")

        # 5. BLL -> BLL Orchestration Dependencies («use»)
        add_use_dependency(el_turno_bll, el_paciente_bll)
        add_use_dependency(el_turno_bll, el_agenda_bll)

        # 6. BLL -> DAL Dependencies («use»)
        add_use_dependency(el_turno_bll, el_turno_dal)
        add_use_dependency(el_turno_bll, el_paciente_dal)
        add_use_dependency(el_paciente_bll, el_paciente_dal)
        add_use_dependency(el_agenda_bll, el_agenda_dal)

        # 7. DAL -> BE Dependencies («use»)
        add_use_dependency(el_turno_dal, el_turno_be)
        add_use_dependency(el_paciente_dal, el_paciente_be)
        add_use_dependency(el_paciente_dal, el_tutor_be)
        add_use_dependency(el_agenda_dal, el_bloque_be)
        add_use_dependency(el_agenda_dal, el_agenda_be)

        # ==============================================================
        # 5. UPDATE DIAGRAM 25 (CU01: Diagrama de Clases)
        # ==============================================================
        diag = ea.GetDiagramByID(25)
        diag.Name = "CUN01: Diagrama de Clases (BLL, DAL y BE)"
        diag.Notes = "Diagrama de Clases de CUN01 (Registrar Turno) con arquitectura en 3 capas: BLL (Lógica de Negocio), DAL (Acceso a Datos) y BE (Entidades), y sus relaciones de Agregación, Composición y Uso."
        diag.Update()

        # Clear existing DiagramObjects in Diagram 25
        for i in range(diag.DiagramObjects.Count - 1, -1, -1):
            diag.DiagramObjects.Delete(i)
        diag.DiagramObjects.Refresh()

        # 3-TIER LAYOUT COORDINATES:
        # Tier 1 (BLL): y = 50 .. 250 (height: 200)
        #   PacienteBLL:     Left = 240,  Right = 680   (width: 440)
        #   TurnoBLL:        Left = 760,  Right = 1260  (width: 500)
        #   AgendaMedicaBLL: Left = 1340, Right = 1800  (width: 460)
        #
        # Tier 2 (DAL): y = 330 .. 500 (height: 170)
        #   PacienteDAL:     Left = 280,  Right = 640   (width: 360)
        #   TurnoDAL:        Left = 800,  Right = 1220  (width: 420)
        #   AgendaMedicaDAL: Left = 1360, Right = 1780  (width: 420)
        #
        # Tier 3 (BE):  y = 590 .. 890 (height: 300)
        #   TutorBE:         Left = 40,   Right = 310   (width: 270)
        #   PacienteBE:      Left = 380,  Right = 680   (width: 300)
        #   TurnoBE:         Left = 850,  Right = 1170  (width: 320)
        #   BloqueHorarioBE: Left = 1250, Right = 1530  (width: 280)
        #   AgendaMedicaBE:  Left = 1610, Right = 1890  (width: 280)

        coords = [
            # Tier 1: BLL
            (el_paciente_bll, 240, 50, 680, 250),
            (el_turno_bll, 760, 50, 1260, 250),
            (el_agenda_bll, 1340, 50, 1800, 250),
            # Tier 2: DAL
            (el_paciente_dal, 280, 330, 640, 500),
            (el_turno_dal, 800, 330, 1220, 500),
            (el_agenda_dal, 1360, 330, 1780, 500),
            # Tier 3: BE
            (el_tutor_be, 40, 590, 310, 890),
            (el_paciente_be, 380, 590, 680, 890),
            (el_turno_be, 850, 590, 1170, 890),
            (el_bloque_be, 1250, 590, 1530, 890),
            (el_agenda_be, 1610, 590, 1890, 890)
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

        # Direct orthogonal style
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 25")

        # Export diagram image
        scratch_dir = r"c:\Users\Danie\Desktop\GIT\TD\scratch"
        out_png = os.path.join(scratch_dir, "cun01_clases_bll_dal_be.png")
        if os.path.exists(out_png):
            os.remove(out_png)

        proj.PutDiagramImageToFile(diag.DiagramGUID, out_png, 1)
        print("Diagram 25 exported successfully to:", out_png)

        # Copy to brain artifact directory
        artifact_dir = r"C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e"
        dest_png = os.path.join(artifact_dir, "cun01_clases_bll_dal_be.png")
        shutil.copy2(out_png, dest_png)
        print("Copied to brain artifact:", dest_png)

    finally:
        ea.CloseFile()
        ea.Exit()
        print("EA closed.")

if __name__ == "__main__":
    build_cun01_class_diagram_with_dal()
