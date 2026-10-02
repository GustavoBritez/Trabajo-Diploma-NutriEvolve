import win32com.client
import os
import shutil

def build_cun02_all():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    try:
        print("=== BUILDING CUN02 CLASS DIAGRAM & DER ===")
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

        # Helper to add «use» dependency
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

        # ==============================================================
        # 1. PREPARE CLASSES FOR CUN02 CLASS DIAGRAM
        # ==============================================================
        # BE: TutorBE_DNI101 & PacienteBE_DNI101
        el_tutor_be = get_or_create_element("TutorBE_DNI101", "Class")
        el_paciente_be = get_or_create_element("PacienteBE_DNI101", "Class")

        # BLL: PacienteBLL_DNI101
        el_paciente_bll = get_or_create_element("PacienteBLL_DNI101", "Class")

        # BLL Security: EventoBLL & DigitoVerificadorBLL
        el_evento_bll = get_or_create_element("EventoBLL", "Class")
        evento_bll_methods = [
            ("RegistrarEvento", "bool", [
                ("criticidad", "int"),
                ("descripcion", "string"),
                ("dniUsuario", "int"),
                ("modulo", "string")
            ])
        ]
        populate_methods_only(el_evento_bll, evento_bll_methods)

        el_dv_bll = get_or_create_element("DigitoVerificadorBLL", "Class")
        dv_bll_methods = [
            ("RecalcularYPersistir", "void", []),
            ("ValidarIntegridad", "bool", [])
        ]
        populate_methods_only(el_dv_bll, dv_bll_methods)

        # DAL: PacienteDAL_DNI101
        el_paciente_dal = get_or_create_element("PacienteDAL_DNI101", "Class")

        # DAL Security: EventoDAL & DigitoVerificadorDAL
        el_evento_dal = get_or_create_element("EventoDAL", "Class")
        evento_dal_methods = [
            ("GuardarEvento", "bool", [
                ("criticidad", "int"),
                ("descripcion", "string"),
                ("dni", "int"),
                ("modulo", "string")
            ])
        ]
        populate_methods_only(el_evento_dal, evento_dal_methods)

        el_dv_dal = get_or_create_element("DigitoVerificadorDAL", "Class")
        dv_dal_methods = [
            ("ActualizarDV", "bool", [
                ("tabla", "string"),
                ("dvv", "string")
            ]),
            ("ObtenerDV", "string", [
                ("tabla", "string")
            ])
        ]
        populate_methods_only(el_dv_dal, dv_dal_methods)

        pkg.Elements.Refresh()

        cun02_classes = [
            el_tutor_be, el_paciente_be,
            el_paciente_bll, el_evento_bll, el_dv_bll,
            el_paciente_dal, el_evento_dal, el_dv_dal
        ]
        c_ids = [e.ElementID for e in cun02_classes]
        c_id_str = ",".join(str(i) for i in c_ids)

        # Connectors for CUN02 Class Diagram:
        # We ensure no duplicate connectors between security classes
        sec_ids = [el_evento_bll.ElementID, el_dv_bll.ElementID, el_evento_dal.ElementID, el_dv_dal.ElementID]
        sec_id_str = ",".join(str(i) for i in sec_ids)
        ea.Execute(f"DELETE FROM t_connector WHERE Start_Object_ID IN ({sec_id_str}) OR End_Object_ID IN ({sec_id_str})")
        for el in cun02_classes:
            el.Connectors.Refresh()

        # Connectors:
        # 1. Agregacion: TutorBE_DNI101 ◇---> PacienteBE_DNI101 (already exists or add if not)
        has_agg = False
        for c in el_tutor_be.Connectors:
            if c.SupplierID == el_paciente_be.ElementID and c.Type == "Aggregation":
                has_agg = True
                break
        if not has_agg:
            c_agg = el_tutor_be.Connectors.AddNew("", "Aggregation")
            c_agg.SupplierID = el_paciente_be.ElementID
            c_agg.Direction = "Source -> Destination"
            c_agg.ClientEnd.Cardinality = "1"
            c_agg.SupplierEnd.Cardinality = "1..*"
            c_agg.SupplierEnd.Role = "Pacientes_DNI101"
            c_agg.Update()
            el_tutor_be.Connectors.Refresh()
            ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 1, DestIsAggregate = 0, SubType = 'Weak', Stereotype = null WHERE Connector_ID = {c_agg.ConnectorID}")

        # 2. ONLY BLL HAS RELATION OF «use» WITH BE AND DAL!
        # PacienteBLL -> PacienteBE & TutorBE
        add_use_dependency(el_paciente_bll, el_paciente_be)
        add_use_dependency(el_paciente_bll, el_tutor_be)

        # PacienteBLL -> PacienteDAL
        add_use_dependency(el_paciente_bll, el_paciente_dal)

        # PacienteBLL -> EventoBLL & DigitoVerificadorBLL
        add_use_dependency(el_paciente_bll, el_evento_bll)
        add_use_dependency(el_paciente_bll, el_dv_bll)

        # EventoBLL -> EventoDAL
        add_use_dependency(el_evento_bll, el_evento_dal)

        # DigitoVerificadorBLL -> DigitoVerificadorDAL
        add_use_dependency(el_dv_bll, el_dv_dal)

        # NOTE: DAL has ZERO relations to BE!

        # ==============================================================
        # 2. BUILD DIAGRAM 30: CUN02 CLASS DIAGRAM
        # ==============================================================
        diag30 = ea.GetDiagramByID(30)
        diag30.Name = "CUN02: Diagrama de Clases (BE, BLL y DAL)"
        diag30.Notes = "Diagrama de Clases de CUN02 (Registrar Paciente Pediátrico) con separación de capas BE (Entidades), BLL (Lógica y Seguridad) y DAL (Acceso a Datos). Únicamente BLL mantiene relaciones de uso («use»)."
        diag30.parentID = 38 # Move to UseCase 38 (CUN02)
        diag30.Update()
        ea.Execute("UPDATE t_diagram SET ParentID = 38 WHERE Diagram_ID = 30")

        # Clear existing DiagramObjects in Diagram 30
        for i in range(diag30.DiagramObjects.Count - 1, -1, -1):
            diag30.DiagramObjects.Delete(i)
        diag30.DiagramObjects.Refresh()

        # Layout for Diagram 30:
        # Tier 1 (TOP): BE (y: 50 .. 340)
        #   TutorBE_DNI101:    Left = 50,  Right = 330 (width: 280)
        #   PacienteBE_DNI101: Left = 430, Right = 730 (width: 300)
        #
        # Tier 2 (MIDDLE): BLL (y: 430 .. 630)
        #   PacienteBLL_DNI101:   Left = 140, Right = 640 (width: 500)
        #   EventoBLL:            Left = 740, Right = 1120 (width: 380)
        #   DigitoVerificadorBLL: Left = 1180, Right = 1520 (width: 340)
        #
        # Tier 3 (BOTTOM): DAL (y: 720 .. 920)
        #   PacienteDAL_DNI101:   Left = 140, Right = 640 (width: 500)
        #   EventoDAL:            Left = 740, Right = 1120 (width: 380)
        #   DigitoVerificadorDAL: Left = 1180, Right = 1520 (width: 340)

        coords30 = [
            # BE
            (el_tutor_be, 50, 50, 330, 340),
            (el_paciente_be, 430, 50, 730, 340),
            # BLL
            (el_paciente_bll, 140, 430, 640, 630),
            (el_evento_bll, 740, 430, 1120, 630),
            (el_dv_bll, 1180, 430, 1520, 630),
            # DAL
            (el_paciente_dal, 140, 720, 640, 920),
            (el_evento_dal, 740, 720, 1120, 920),
            (el_dv_dal, 1180, 720, 1520, 920)
        ]

        for el, left, top, right, bottom in coords30:
            do = diag30.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag30.DiagramObjects.Refresh()
        diag30.Update()
        ea.SaveDiagram(diag30.DiagramID)
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 30")

        # Export Diagram 30 image
        scratch_dir = r"c:\Users\Danie\Desktop\GIT\TD\scratch"
        out_png30 = os.path.join(scratch_dir, "cun02_clases_bll_dal_be.png")
        if os.path.exists(out_png30):
            os.remove(out_png30)
        proj.PutDiagramImageToFile(diag30.DiagramGUID, out_png30, 1)
        print("Diagram 30 exported to:", out_png30)

        artifact_dir = r"C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e"
        dest_png30 = os.path.join(artifact_dir, "cun02_clases_bll_dal_be.png")
        shutil.copy2(out_png30, dest_png30)
        print("Copied to brain artifact:", dest_png30)

        # ==============================================================
        # 3. BUILD DIAGRAM 31: CUN02 DER (MODELO RELACIONAL)
        # ==============================================================
        diag31 = ea.GetDiagramByID(31)
        diag31.Name = "CUN02: DER (Modelo Relacional)"
        diag31.Notes = "Diagrama Entidad-Relación (DER) / Modelo Relacional del CUN02 (Registrar Paciente Pediátrico) con las tablas involucradas: Tutores_DNI101, Pacientes_DNI101, Usuarios, Bitacora y DV."
        diag31.parentID = 38 # Move to UseCase 38 (CUN02)
        diag31.Update()
        ea.Execute("UPDATE t_diagram SET ParentID = 38 WHERE Diagram_ID = 31")

        # Clear existing DiagramObjects in Diagram 31
        for i in range(diag31.DiagramObjects.Count - 1, -1, -1):
            diag31.DiagramObjects.Delete(i)
        diag31.DiagramObjects.Refresh()

        el_tutor_tab = ea.GetElementByID(429)
        el_paciente_tab = ea.GetElementByID(430)
        el_usuario_tab = ea.GetElementByID(431)
        el_bitacora_tab = ea.GetElementByID(436)
        el_dv_tab = ea.GetElementByID(435)

        # Layout for Diagram 31 (Focused on CUN02 tables):
        # Left column (Patient Domain):
        #   Tutores_DNI101:   Left = 50,  Right = 360, Top = 50,  Bottom = 310
        #   Pacientes_DNI101: Left = 50,  Right = 360, Top = 400, Bottom = 680
        # Right column (Security / Audit Domain):
        #   Bitacora:         Left = 480, Right = 810, Top = 50,  Bottom = 250
        #   Usuarios:         Left = 480, Right = 810, Top = 340, Bottom = 650
        # Bottom-Center (Integrity):
        #   DV:               Left = 280, Right = 580, Top = 740, Bottom = 860

        coords31 = [
            (el_tutor_tab, 50, 50, 360, 310),
            (el_paciente_tab, 50, 400, 360, 680),
            (el_bitacora_tab, 480, 50, 810, 250),
            (el_usuario_tab, 480, 340, 810, 650),
            (el_dv_tab, 280, 740, 580, 860)
        ]

        for el, left, top, right, bottom in coords31:
            do = diag31.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag31.DiagramObjects.Refresh()
        diag31.Update()
        ea.SaveDiagram(diag31.DiagramID)
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 31")

        # Export Diagram 31 image
        out_png31 = os.path.join(scratch_dir, "cun02_der.png")
        if os.path.exists(out_png31):
            os.remove(out_png31)
        proj.PutDiagramImageToFile(diag31.DiagramGUID, out_png31, 1)
        print("Diagram 31 exported to:", out_png31)

        dest_png31 = os.path.join(artifact_dir, "cun02_der.png")
        shutil.copy2(out_png31, dest_png31)
        print("Copied to brain artifact:", dest_png31)

        print("=== BOTH CUN02 DIAGRAMS BUILT SUCCESSFULLY ===")

    finally:
        ea.CloseFile()
        ea.Exit()
        print("EA closed.")

if __name__ == "__main__":
    build_cun02_all()
