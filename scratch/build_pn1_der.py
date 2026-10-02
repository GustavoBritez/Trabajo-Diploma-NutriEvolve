import win32com.client
import os
import shutil

def build_pn1_der():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()
    scratch_dir = r"c:\Users\Danie\Desktop\GIT\TD\scratch"
    artifact_dir = r"C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e"

    try:
        print("=== BUILDING DER DEL PN1 ===")
        pkg7 = ea.GetPackageByID(7)

        # Helper to ensure connector
        def ensure_connector(src, dst, ctype, stereo="", name="", s_card="", d_card="", s_role="", d_role=""):
            for c in src.Connectors:
                if c.SupplierID == dst.ElementID and c.Type == ctype and (not stereo or c.Stereotype == stereo):
                    if name and not c.Name:
                        c.Name = name
                        c.Update()
                    return c
            conn = src.Connectors.AddNew(name, ctype)
            conn.SupplierID = dst.ElementID
            if stereo:
                conn.Stereotype = stereo
            if s_card:
                conn.ClientEnd.Cardinality = s_card
            if d_card:
                conn.SupplierEnd.Cardinality = d_card
            if s_role:
                conn.ClientEnd.Role = s_role
            if d_role:
                conn.SupplierEnd.Role = d_role
            conn.Update()
            src.Connectors.Refresh()
            if stereo:
                ea.Execute(f"UPDATE t_connector SET Stereotype = '{stereo}' WHERE Connector_ID = {conn.ConnectorID}")
            return conn

        def apply_styles(diag_id):
            style_ex = "TConnectorNotation=Information Engineering;ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;"
            pdata = "HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=0;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;"
            ea.Execute(f"""
UPDATE t_diagram
SET StyleEx = '{style_ex}',
    PDATA = '{pdata}',
    ShowPackageContents = 0,
    ShowForeign = 0
WHERE Diagram_ID = {diag_id}
""")

        def clear_diagram(d):
            count = d.DiagramObjects.Count
            for i in range(count - 1, -1, -1):
                d.DiagramObjects.Delete(i)
            d.DiagramObjects.Refresh()
            d.Update()

        def add_do(d, el_id, l, t, r, b):
            obj = d.DiagramObjects.AddNew("", "")
            obj.ElementID = el_id
            obj.left = l
            obj.top = t
            obj.right = r
            obj.bottom = b
            obj.Update()
            return obj

        def get_or_create_diagram(pkg, name):
            for d in pkg.Diagrams:
                if d.Name == name:
                    return d
            d = pkg.Diagrams.AddNew(name, "Logical")
            d.Update()
            pkg.Diagrams.Refresh()
            return d

        # Fetch Tables for PN1
        tab_paciente = ea.GetElementByID(430)
        tab_usuario = ea.GetElementByID(431)
        tab_agenda = ea.GetElementByID(432)
        tab_bloque = ea.GetElementByID(433)
        tab_turno = ea.GetElementByID(434)
        tab_dv = ea.GetElementByID(435)
        tab_bitacora = ea.GetElementByID(436)

        # Connectors for PN1 DER
        ensure_connector(tab_paciente, tab_turno, "Association", "", "FK_Turnos_Pacientes", "1", "0..*")
        ensure_connector(tab_agenda, tab_bloque, "Association", "", "FK_Bloques_Agendas", "1", "1..*")
        ensure_connector(tab_bloque, tab_turno, "Association", "", "FK_Turnos_Bloques", "0..1", "0..1")
        ensure_connector(tab_usuario, tab_turno, "Association", "", "FK_Turnos_Usuarios", "1", "0..*")
        ensure_connector(tab_usuario, tab_agenda, "Association", "", "FK_Agendas_Usuarios", "1", "0..*")
        ensure_connector(tab_usuario, tab_bitacora, "Association", "", "FK_Bitacora_Usuarios", "1", "0..*")

        # Create Diagram: "Proceso de Negocio 1 DER" in Package 7
        d_pn1_der = get_or_create_diagram(pkg7, "Proceso de Negocio 1 DER")
        clear_diagram(d_pn1_der)

        # Top Band: Negocio Core
        add_do(d_pn1_der, tab_paciente.ElementID, 50, -60, 340, -360)
        add_do(d_pn1_der, tab_turno.ElementID, 430, -60, 740, -370)
        add_do(d_pn1_der, tab_bloque.ElementID, 830, -60, 1110, -260)
        add_do(d_pn1_der, tab_agenda.ElementID, 1200, -60, 1480, -250)

        # Bottom Band: Transversal Seguridad, Auditoria y Usuario (Nutricionista)
        add_do(d_pn1_der, tab_dv.ElementID, 50, -450, 340, -590)
        add_do(d_pn1_der, tab_bitacora.ElementID, 430, -450, 720, -670)
        add_do(d_pn1_der, tab_usuario.ElementID, 830, -380, 1140, -730)

        d_pn1_der.DiagramObjects.Refresh()
        d_pn1_der.Update()
        apply_styles(d_pn1_der.DiagramID)

        # Export PNG
        f_scratch = os.path.join(scratch_dir, "pn1_der.png")
        f_art = os.path.join(artifact_dir, "pn1_der.png")
        proj.PutDiagramImageToFile(d_pn1_der.DiagramGUID, f_scratch, 1)
        shutil.copyfile(f_scratch, f_art)
        print(f"Exported PN1 DER: {f_scratch}")
        print("=== DER DEL PN1 BUILT SUCCESSFULLY! ===")

    except Exception as ex:
        print(f"ERROR: {ex}")
        import traceback
        traceback.print_exc()

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    build_pn1_der()
