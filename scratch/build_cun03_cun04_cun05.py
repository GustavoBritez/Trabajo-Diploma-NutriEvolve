import win32com.client
import os
import shutil

def build_cun03_04_05():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()
    scratch_dir = r"c:\Users\Danie\Desktop\GIT\TD\scratch"
    artifact_dir = r"C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e"

    try:
        print("=== BUILDING DER & CLASES FOR CUN03, CUN04, CUN05 ===")

        # Fetch existing elements
        # Tables (DER)
        el_paciente_tab = ea.GetElementByID(430)
        el_usuario_tab = ea.GetElementByID(431)
        el_agenda_tab = ea.GetElementByID(432)
        el_bloque_tab = ea.GetElementByID(433)
        el_turno_tab = ea.GetElementByID(434)
        el_dv_tab = ea.GetElementByID(435)
        el_bitacora_tab = ea.GetElementByID(436)

        # Classes
        pkg7 = ea.GetPackageByID(7)
        def get_class(name):
            for el in pkg7.Elements:
                if el.Name == name and el.Type == "Class":
                    return el
            return None

        # BE
        el_paciente_be = get_class("PacienteBE_DNI101")
        el_turno_be = get_class("TurnoBE_DNI101")
        el_bloque_be = get_class("BloqueHorarioBE_DNI101")
        el_agenda_be = get_class("AgendaMedicaBE_DNI101")

        # BLL
        el_paciente_bll = get_class("PacienteBLL_DNI101")
        el_turno_bll = get_class("TurnoBLL_DNI101")
        el_agenda_bll = get_class("AgendaMedicaBLL_DNI101")
        el_evento_bll = get_class("EventoBLL")
        el_dv_bll = get_class("DigitoVerificadorBLL")

        # DAL
        el_paciente_dal = get_class("PacienteDAL_DNI101")
        el_turno_dal = get_class("TurnoDAL_DNI101")
        el_agenda_dal = get_class("AgendaMedicaDAL_DNI101")
        el_evento_dal = get_class("EventoDAL")
        el_dv_dal = get_class("DigitoVerificadorDAL")

        # Helper to ensure dependency connector exists
        def ensure_use_dependency(source_el, target_el, role=""):
            for c in source_el.Connectors:
                if c.SupplierID == target_el.ElementID and c.Type == "Dependency":
                    return c
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

        # Ensure all required «use» connectors exist
        ensure_use_dependency(el_turno_bll, el_turno_be)
        ensure_use_dependency(el_turno_bll, el_agenda_bll)
        ensure_use_dependency(el_turno_bll, el_evento_bll)
        ensure_use_dependency(el_turno_bll, el_dv_bll)
        ensure_use_dependency(el_turno_bll, el_turno_dal)
        ensure_use_dependency(el_agenda_bll, el_bloque_be)
        ensure_use_dependency(el_agenda_bll, el_agenda_be)
        ensure_use_dependency(el_agenda_bll, el_agenda_dal)
        ensure_use_dependency(el_evento_bll, el_evento_dal)
        ensure_use_dependency(el_dv_bll, el_dv_dal)

        # Helper to get or create diagram under a UseCase element
        def get_or_create_diagram(uc_id, name, diag_type="Logical"):
            uc_el = ea.GetElementByID(uc_id)
            for d in uc_el.Diagrams:
                if d.Name == name and d.Type == diag_type:
                    return d
            new_d = uc_el.Diagrams.AddNew(name, diag_type)
            new_d.Update()
            uc_el.Diagrams.Refresh()
            ea.Execute(f"UPDATE t_diagram SET ParentID = {uc_id}, Package_ID = 4 WHERE Diagram_ID = {new_d.DiagramID}")
            return new_d

        # Helper to apply standard styles to diagram
        def apply_styles(diag_id, is_der=False):
            t_conn = "TConnectorNotation=Information Engineering;" if is_der else "TConnectorNotation=UML 2.1;"
            style_ex = f"{t_conn}ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;"
            pdata = "HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=0;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;"
            ea.Execute(f"""
UPDATE t_diagram
SET StyleEx = '{style_ex}',
    PDATA = '{pdata}',
    ShowForeign = 0,
    ShowPackageContents = 0
WHERE Diagram_ID = {diag_id}
            """)
            ea.Execute(f"UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = {diag_id}")

        def populate_diagram(diag, coords):
            for i in range(diag.DiagramObjects.Count - 1, -1, -1):
                diag.DiagramObjects.Delete(i)
            diag.DiagramObjects.Refresh()

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

        # Standard 7-table DER coordinates
        der_7_coords = [
            (el_paciente_tab, 50, 50, 360, 350),
            (el_dv_tab, 50, 430, 360, 550),
            (el_bitacora_tab, 440, 50, 770, 250),
            (el_usuario_tab, 440, 330, 770, 640),
            (el_turno_tab, 440, 720, 770, 1030),
            (el_agenda_tab, 850, 50, 1170, 240),
            (el_bloque_tab, 850, 340, 1170, 560)
        ]

        # Standard 4-component Class diagram layout for Turnos (Turno, Agenda, Evento, DV)
        class_coords_full = [
            # Tier 1: BE
            (el_paciente_be, 50, 50, 420, 330),
            (el_turno_be, 480, 50, 950, 330),
            (el_bloque_be, 1010, 50, 1350, 330),
            (el_agenda_be, 1410, 50, 1750, 330),
            # Tier 2: BLL
            (el_turno_bll, 50, 410, 640, 650),
            (el_agenda_bll, 700, 410, 1120, 650),
            (el_evento_bll, 1180, 410, 1580, 650),
            (el_dv_bll, 1640, 410, 2080, 650),
            # Tier 3: DAL
            (el_turno_dal, 50, 730, 640, 1030),
            (el_agenda_dal, 700, 730, 1120, 900),
            (el_evento_dal, 1180, 730, 1580, 950),
            (el_dv_dal, 1640, 730, 2080, 950)
        ]

        # ==============================================================
        # CUN03: REPROGRAMAR TURNO (UC 13)
        # ==============================================================
        print("--- Building CUN03 Diagrams ---")
        # 1. CUN03 Diagrama de Clases
        d_c03_class = get_or_create_diagram(13, "CUN03: Diagrama de Clases (BE, BLL y DAL)", "Logical")
        d_c03_class.Notes = "Diagrama de Clases de CUN03 (Reprogramar Turno) con arquitectura de 3 capas: BE, BLL y DAL. Únicamente BLL mantiene relaciones de uso («use»). Métodos y atributos sincronizados con código fuente C#."
        d_c03_class.Update()
        populate_diagram(d_c03_class, class_coords_full)
        apply_styles(d_c03_class.DiagramID, is_der=False)
        out_png = os.path.join(scratch_dir, "cun03_clases_bll_dal_be.png")
        proj.PutDiagramImageToFile(d_c03_class.DiagramGUID, out_png, 1)
        shutil.copy2(out_png, os.path.join(artifact_dir, "cun03_clases_bll_dal_be.png"))
        print("CUN03 Class Diagram exported.")

        # 2. CUN03 DER
        d_c03_der = get_or_create_diagram(13, "CUN03: DER (Modelo Relacional)", "Logical")
        d_c03_der.Notes = "Diagrama Entidad-Relación (DER) del CUN03 (Reprogramar Turno). Tablas participantes: Turnos_DNI101, BloquesHorarios_DNI101, AgendasMedicas_DNI101, Pacientes_DNI101, Usuarios, Bitacora y DV."
        d_c03_der.Update()
        populate_diagram(d_c03_der, der_7_coords)
        apply_styles(d_c03_der.DiagramID, is_der=True)
        out_png = os.path.join(scratch_dir, "cun03_der.png")
        proj.PutDiagramImageToFile(d_c03_der.DiagramGUID, out_png, 1)
        shutil.copy2(out_png, os.path.join(artifact_dir, "cun03_der.png"))
        print("CUN03 DER exported.")

        # ==============================================================
        # CUN04: MODIFICAR TURNO (UC 39)
        # ==============================================================
        print("--- Building CUN04 Diagrams ---")
        # In CUN04 (Modificar Turno), patient, turno, block, audit and DV participate
        class_coords_c04 = [
            # Tier 1: BE
            (el_paciente_be, 50, 50, 460, 330),
            (el_turno_be, 520, 50, 1050, 330),
            (el_bloque_be, 1110, 50, 1550, 330),
            # Tier 2: BLL
            (el_turno_bll, 50, 410, 640, 650),
            (el_evento_bll, 720, 410, 1140, 650),
            (el_dv_bll, 1220, 410, 1680, 650),
            # Tier 3: DAL
            (el_turno_dal, 50, 730, 640, 1030),
            (el_evento_dal, 720, 730, 1140, 950),
            (el_dv_dal, 1220, 730, 1680, 950)
        ]

        der_c04_coords = [
            (el_paciente_tab, 50, 50, 360, 350),
            (el_dv_tab, 50, 430, 360, 550),
            (el_bitacora_tab, 440, 50, 770, 250),
            (el_usuario_tab, 440, 330, 770, 640),
            (el_turno_tab, 440, 720, 770, 1030),
            (el_bloque_tab, 850, 340, 1170, 560)
        ]

        # 1. CUN04 Diagrama de Clases
        d_c04_class = get_or_create_diagram(39, "CUN04: Diagrama de Clases (BE, BLL y DAL)", "Logical")
        d_c04_class.Notes = "Diagrama de Clases de CUN04 (Modificar Turno) con arquitectura de 3 capas: BE, BLL y DAL. Métodos y atributos sincronizados con código fuente C#."
        d_c04_class.Update()
        populate_diagram(d_c04_class, class_coords_c04)
        apply_styles(d_c04_class.DiagramID, is_der=False)
        out_png = os.path.join(scratch_dir, "cun04_clases_bll_dal_be.png")
        proj.PutDiagramImageToFile(d_c04_class.DiagramGUID, out_png, 1)
        shutil.copy2(out_png, os.path.join(artifact_dir, "cun04_clases_bll_dal_be.png"))
        print("CUN04 Class Diagram exported.")

        # 2. CUN04 DER
        d_c04_der = get_or_create_diagram(39, "CUN04: DER (Modelo Relacional)", "Logical")
        d_c04_der.Notes = "Diagrama Entidad-Relación (DER) del CUN04 (Modificar Turno). Tablas participantes: Turnos_DNI101, BloquesHorarios_DNI101, Pacientes_DNI101, Usuarios, Bitacora y DV."
        d_c04_der.Update()
        populate_diagram(d_c04_der, der_c04_coords)
        apply_styles(d_c04_der.DiagramID, is_der=True)
        out_png = os.path.join(scratch_dir, "cun04_der.png")
        proj.PutDiagramImageToFile(d_c04_der.DiagramGUID, out_png, 1)
        shutil.copy2(out_png, os.path.join(artifact_dir, "cun04_der.png"))
        print("CUN04 DER exported.")

        # ==============================================================
        # CUN05: CANCELAR TURNO (UC 37)
        # ==============================================================
        print("--- Building CUN05 Diagrams ---")
        # 1. CUN05 Diagrama de Clases (In CUN05, Turno, Agenda, Evento, DV participate to cancel and release block)
        d_c05_class = get_or_create_diagram(37, "CUN05: Diagrama de Clases (BE, BLL y DAL)", "Logical")
        d_c05_class.Notes = "Diagrama de Clases de CUN05 (Cancelar Turno) con arquitectura de 3 capas: BE, BLL y DAL. Incluye liberación de bloque en AgendaMedicaBLL_DNI101 y registro en Bitácora y DV."
        d_c05_class.Update()
        populate_diagram(d_c05_class, class_coords_full)
        apply_styles(d_c05_class.DiagramID, is_der=False)
        out_png = os.path.join(scratch_dir, "cun05_clases_bll_dal_be.png")
        proj.PutDiagramImageToFile(d_c05_class.DiagramGUID, out_png, 1)
        shutil.copy2(out_png, os.path.join(artifact_dir, "cun05_clases_bll_dal_be.png"))
        print("CUN05 Class Diagram exported.")

        # 2. CUN05 DER
        d_c05_der = get_or_create_diagram(37, "CUN05: DER (Modelo Relacional)", "Logical")
        d_c05_der.Notes = "Diagrama Entidad-Relación (DER) del CUN05 (Cancelar Turno). Tablas participantes: Turnos_DNI101, BloquesHorarios_DNI101, AgendasMedicas_DNI101, Pacientes_DNI101, Usuarios, Bitacora y DV."
        d_c05_der.Update()
        populate_diagram(d_c05_der, der_7_coords)
        apply_styles(d_c05_der.DiagramID, is_der=True)
        out_png = os.path.join(scratch_dir, "cun05_der.png")
        proj.PutDiagramImageToFile(d_c05_der.DiagramGUID, out_png, 1)
        shutil.copy2(out_png, os.path.join(artifact_dir, "cun05_der.png"))
        print("CUN05 DER exported.")

        print("=== ALL DIAGRAMS FOR CUN03, CUN04, CUN05 BUILT SUCCESSFULLY ===")

    finally:
        ea.CloseFile()
        ea.Exit()
        print("EA closed.")

if __name__ == "__main__":
    build_cun03_04_05()
