import win32com.client
import os
import shutil

def rebuild_cun01_exact():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    try:
        print("--- Rebuilding CUN01 Class Diagram (Exact Architecture: BE Top, BLL Mid, DAL Bottom) ---")
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
            ("ActualizarEstadoBloque", "bool", [
                ("idBloque", "int"),
                ("nuevoEstado", "string")
            ]),
            ("ActualizarEstado", "bool", [
                ("idTurno", "int"),
                ("nuevoEstado", "string")
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
            ("ActualizarEstadoBloque", "bool", [
                ("idBloque", "int"),
                ("nuevoEstado", "string")
            ]),
            ("ListarBloquesDisponibles", "List<BloqueHorarioBE_DNI101>", [
                ("fecha", "DateTime"),
                ("dniNutricionista", "int")
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
        # Helper to create «use» dependency
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

        # A. ASSOCIATIONS BETWEEN BEs ONLY
        # 1. Agregacion: TutorBE_DNI101 ◇---> PacienteBE_DNI101
        c_agg_tutor = el_tutor_be.Connectors.AddNew("", "Aggregation")
        c_agg_tutor.SupplierID = el_paciente_be.ElementID
        c_agg_tutor.Direction = "Source -> Destination"
        c_agg_tutor.ClientEnd.Cardinality = "1"
        c_agg_tutor.SupplierEnd.Cardinality = "1..*"
        c_agg_tutor.SupplierEnd.Role = "Pacientes_DNI101"
        c_agg_tutor.Update()
        el_tutor_be.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 1, DestIsAggregate = 0, SubType = 'Weak', Stereotype = null WHERE Connector_ID = {c_agg_tutor.ConnectorID}")

        # 2. Agregacion: PacienteBE_DNI101 ◇---> TurnoBE_DNI101
        c_agg_pac = el_paciente_be.Connectors.AddNew("", "Aggregation")
        c_agg_pac.SupplierID = el_turno_be.ElementID
        c_agg_pac.Direction = "Source -> Destination"
        c_agg_pac.ClientEnd.Cardinality = "1"
        c_agg_pac.SupplierEnd.Cardinality = "0..*"
        c_agg_pac.SupplierEnd.Role = "Turnos"
        c_agg_pac.Update()
        el_paciente_be.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 1, DestIsAggregate = 0, SubType = 'Weak', Stereotype = null WHERE Connector_ID = {c_agg_pac.ConnectorID}")

        # 3. Composicion: AgendaMedicaBE_DNI101 ◆---> BloqueHorarioBE_DNI101
        c_comp_agenda = el_agenda_be.Connectors.AddNew("", "Aggregation")
        c_comp_agenda.SupplierID = el_bloque_be.ElementID
        c_comp_agenda.Direction = "Source -> Destination"
        c_comp_agenda.ClientEnd.Cardinality = "1"
        c_comp_agenda.SupplierEnd.Cardinality = "1..*"
        c_comp_agenda.SupplierEnd.Role = "BloquesHorarios_DNI101"
        c_comp_agenda.Update()
        el_agenda_be.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 2, DestIsAggregate = 0, SubType = 'Strong', Stereotype = null WHERE Connector_ID = {c_comp_agenda.ConnectorID}")

        # 4. TurnoBE_DNI101 - - - > BloqueHorarioBE_DNI101 («use»)
        add_use_dependency(el_turno_be, el_bloque_be, "IdBloque_DNI101")

        # B. ONLY BLL HAS RELATION OF «use» WITH BE AND DAL!
        # 1) BLL -> BE:
        add_use_dependency(el_turno_bll, el_turno_be)
        add_use_dependency(el_paciente_bll, el_paciente_be)
        add_use_dependency(el_paciente_bll, el_tutor_be)
        add_use_dependency(el_agenda_bll, el_bloque_be)
        add_use_dependency(el_agenda_bll, el_agenda_be)

        # 2) BLL -> DAL:
        add_use_dependency(el_turno_bll, el_turno_dal)
        add_use_dependency(el_turno_bll, el_paciente_dal)
        add_use_dependency(el_turno_bll, el_agenda_dal)
        add_use_dependency(el_paciente_bll, el_paciente_dal)
        add_use_dependency(el_agenda_bll, el_agenda_dal)

        # 3) BLL -> BLL (Orchestration):
        add_use_dependency(el_turno_bll, el_paciente_bll)
        add_use_dependency(el_turno_bll, el_agenda_bll)

        # NOTE: DAL has ZERO «use» relations to BE as instructed!

        # ==============================================================
        # 5. UPDATE DIAGRAM 25 (CU01: Diagrama de Clases)
        # ==============================================================
        diag = ea.GetDiagramByID(25)
        diag.Name = "CUN01: Diagrama de Clases (BE, BLL y DAL)"
        diag.Notes = "Diagrama de Clases de CUN01 (Registrar Turno) con arquitectura de 3 capas: BE (Entidades), BLL (Lógica de Negocio) y DAL (Acceso a Datos). Únicamente BLL mantiene relaciones de uso («use») con BE y DAL."
        diag.Update()

        # Clear existing DiagramObjects in Diagram 25
        for i in range(diag.DiagramObjects.Count - 1, -1, -1):
            diag.DiagramObjects.Delete(i)
        diag.DiagramObjects.Refresh()

        # LAYOUT:
        # Tier 1 (TOP): BE (Business Entities)
        #   TutorBE:         Left = 40,   Right = 310  (width: 270)
        #   PacienteBE:      Left = 380,  Right = 680  (width: 300)
        #   TurnoBE:         Left = 850,  Right = 1170 (width: 320)
        #   BloqueHorarioBE: Left = 1250, Right = 1530 (width: 280)
        #   AgendaMedicaBE:  Left = 1610, Right = 1890 (width: 280)
        #
        # Tier 2 (MIDDLE): BLL (Business Logic Layer)
        #   PacienteBLL:     Left = 240,  Right = 680  (width: 440)
        #   TurnoBLL:        Left = 760,  Right = 1260 (width: 500)
        #   AgendaMedicaBLL: Left = 1340, Right = 1800 (width: 460)
        #
        # Tier 3 (BOTTOM): DAL (Data Access Layer)
        #   PacienteDAL:     Left = 240,  Right = 680  (width: 440)
        #   TurnoDAL:        Left = 760,  Right = 1260 (width: 500)
        #   AgendaMedicaDAL: Left = 1340, Right = 1800 (width: 460)

        coords = [
            # Tier 1: BE (y: 50 .. 340)
            (el_tutor_be, 40, 50, 310, 340),
            (el_paciente_be, 380, 50, 680, 340),
            (el_turno_be, 850, 50, 1170, 340),
            (el_bloque_be, 1250, 50, 1530, 340),
            (el_agenda_be, 1610, 50, 1890, 340),

            # Tier 2: BLL (y: 430 .. 630)
            (el_paciente_bll, 240, 430, 680, 630),
            (el_turno_bll, 760, 430, 1260, 630),
            (el_agenda_bll, 1340, 430, 1800, 630),

            # Tier 3: DAL (y: 720 .. 940)
            (el_paciente_dal, 240, 720, 680, 940),
            (el_turno_dal, 760, 720, 1260, 940),
            (el_agenda_dal, 1340, 720, 1800, 940)
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
        out_png = os.path.join(scratch_dir, "cun01_clases_bll_dal_be_exact.png")
        if os.path.exists(out_png):
            os.remove(out_png)

        proj.PutDiagramImageToFile(diag.DiagramGUID, out_png, 1)
        print("Diagram 25 exported successfully to:", out_png)

        # Copy to brain artifact directory
        artifact_dir = r"C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e"
        dest_png = os.path.join(artifact_dir, "cun01_clases_bll_dal_be_exact.png")
        shutil.copy2(out_png, dest_png)
        print("Copied to brain artifact:", dest_png)

    finally:
        ea.CloseFile()
        ea.Exit()
        print("EA closed.")

if __name__ == "__main__":
    rebuild_cun01_exact()
