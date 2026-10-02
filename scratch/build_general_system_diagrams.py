import win32com.client
import os
import shutil

def build_general_diagrams():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()
    scratch_dir = r"c:\Users\Danie\Desktop\GIT\TD\scratch"
    artifact_dir = r"C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e"

    try:
        print("=== APPLYING PERFECT GEOMETRY FOR GENERAL CLASS DIAGRAM & DER ===")

        # Elements Fetch
        # BE
        be_paciente = ea.GetElementByID(662)
        be_turno = ea.GetElementByID(663)
        be_agenda = ea.GetElementByID(664)
        be_bloque = ea.GetElementByID(665)
        be_usuario = ea.GetElementByID(666)
        be_evento = ea.GetElementByID(777)
        be_idioma = ea.GetElementByID(778)
        be_perfil = ea.GetElementByID(779)
        be_familia_svc = ea.GetElementByID(780)
        be_patente_svc = ea.GetElementByID(781)
        be_istate = ea.GetElementByID(749)
        be_state_sol = ea.GetElementByID(750)
        be_state_conf = ea.GetElementByID(751)
        be_state_asist = ea.GetElementByID(752)
        be_state_canc = ea.GetElementByID(753)

        # BLL
        bll_paciente = ea.GetElementByID(757)
        bll_turno = ea.GetElementByID(756)
        bll_agenda = ea.GetElementByID(758)
        bll_evento = ea.GetElementByID(762)
        bll_dv = ea.GetElementByID(763)
        bll_usuario = ea.GetElementByID(782)
        bll_idioma = ea.GetElementByID(783)
        bll_backup = ea.GetElementByID(784)
        bll_perfil = ea.GetElementByID(785)
        bll_familia = ea.GetElementByID(786)
        bll_patente = ea.GetElementByID(787)

        # DAL
        dal_paciente = ea.GetElementByID(760)
        dal_turno = ea.GetElementByID(759)
        dal_agenda = ea.GetElementByID(761)
        dal_evento = ea.GetElementByID(764)
        dal_dv = ea.GetElementByID(765)
        dal_usuario = ea.GetElementByID(788)
        dal_idioma = ea.GetElementByID(789)
        dal_backup = ea.GetElementByID(790)
        dal_perfil = ea.GetElementByID(791)
        dal_familia = ea.GetElementByID(792)
        dal_patente = ea.GetElementByID(793)
        dal_conexion = ea.GetElementByID(794)

        # DER Tables
        tab_paciente = ea.GetElementByID(430)
        tab_usuario = ea.GetElementByID(431)
        tab_agenda = ea.GetElementByID(432)
        tab_bloque = ea.GetElementByID(433)
        tab_turno = ea.GetElementByID(434)
        tab_dv = ea.GetElementByID(435)
        tab_bitacora = ea.GetElementByID(436)
        tab_perfil = ea.GetElementByID(769)
        tab_familia = ea.GetElementByID(770)
        tab_permiso = ea.GetElementByID(771)
        tab_perfil_fam = ea.GetElementByID(772)
        tab_perfil_perm = ea.GetElementByID(773)
        tab_fam_fam = ea.GetElementByID(774)
        tab_perm_fam = ea.GetElementByID(775)
        tab_perm_btn = ea.GetElementByID(776)

        def apply_styles(diag_id, is_der=False):
            t_conn = "TConnectorNotation=Information Engineering;" if is_der else "TConnectorNotation=UML 2.1;"
            style_ex = f"{t_conn}ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;"
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

        # -------------------------------------------------------------
        # CLASS DIAGRAM REFINEMENT
        # -------------------------------------------------------------
        pkg7 = ea.GetPackageByID(7)
        d_gen_clases = None
        for d in pkg7.Diagrams:
            if d.Name == "Diagrama General de Clases (BE - BLL - DAL)":
                d_gen_clases = d
                break
        if not d_gen_clases:
            d_gen_clases = pkg7.Diagrams.AddNew("Diagrama General de Clases (BE - BLL - DAL)", "Logical")
            d_gen_clases.Update()
            pkg7.Diagrams.Refresh()

        d7 = ea.GetDiagramByID(7)

        def populate_class_diagram(d):
            clear_diagram(d)

            # === TIER 1: BE (Top: Y = -60 to -820) ===
            # Negocio BE
            add_do(d, be_paciente.ElementID, 50, -60, 400, -360)
            add_do(d, be_turno.ElementID, 480, -60, 920, -360)
            add_do(d, be_bloque.ElementID, 1000, -60, 1260, -280)
            add_do(d, be_agenda.ElementID, 1340, -60, 1620, -250)

            # Seguridad BE
            add_do(d, be_usuario.ElementID, 1700, -60, 1980, -300)
            add_do(d, be_evento.ElementID, 2060, -60, 2340, -270)
            add_do(d, be_idioma.ElementID, 2420, -60, 2660, -190)

            # Composite RBAC Entities
            add_do(d, be_perfil.ElementID, 2740, -60, 2980, -210)
            add_do(d, be_familia_svc.ElementID, 3060, -60, 3320, -230)
            add_do(d, be_patente_svc.ElementID, 3400, -60, 3660, -190)

            # State Pattern for Turno (under TurnoBE)
            add_do(d, be_istate.ElementID, 550, -410, 850, -520)
            # 2x2 grid for States
            add_do(d, be_state_sol.ElementID, 300, -550, 680, -660)
            add_do(d, be_state_conf.ElementID, 720, -550, 1100, -660)
            add_do(d, be_state_asist.ElementID, 300, -690, 680, -800)
            add_do(d, be_state_canc.ElementID, 720, -690, 1100, -800)

            # === TIER 2: BLL (Middle: Y = -920 to -1620) ===
            # Core BLL
            add_do(d, bll_paciente.ElementID, 50, -920, 420, -1120)
            add_do(d, bll_turno.ElementID, 480, -920, 880, -1250)
            add_do(d, bll_agenda.ElementID, 960, -920, 1420, -1100)
            add_do(d, bll_usuario.ElementID, 1500, -920, 1950, -1280)
            add_do(d, bll_evento.ElementID, 2030, -920, 2370, -1080)
            add_do(d, bll_dv.ElementID, 2450, -920, 2850, -1120)
            add_do(d, bll_idioma.ElementID, 2930, -920, 3210, -1050)
            add_do(d, bll_backup.ElementID, 3290, -920, 3570, -1050)

            # RBAC BLL
            add_do(d, bll_perfil.ElementID, 2450, -1180, 2900, -1580)
            add_do(d, bll_familia.ElementID, 2980, -1180, 3420, -1540)
            add_do(d, bll_patente.ElementID, 3500, -1180, 3940, -1520)

            # === TIER 3: DAL (Bottom: Y = -1700 to -2400) ===
            # Core DAL
            add_do(d, dal_paciente.ElementID, 50, -1700, 420, -1900)
            add_do(d, dal_turno.ElementID, 480, -1700, 880, -2100)
            add_do(d, dal_agenda.ElementID, 960, -1700, 1420, -1860)
            add_do(d, dal_usuario.ElementID, 1500, -1700, 1950, -2000)
            add_do(d, dal_evento.ElementID, 2030, -1700, 2370, -1850)
            add_do(d, dal_dv.ElementID, 2450, -1700, 2850, -1940)
            add_do(d, dal_idioma.ElementID, 2930, -1700, 3210, -1860)
            add_do(d, dal_backup.ElementID, 3290, -1700, 3570, -1840)

            # RBAC DAL & Conexion
            add_do(d, dal_conexion.ElementID, 1500, -2060, 1950, -2250)
            add_do(d, dal_perfil.ElementID, 2450, -1990, 2900, -2440)
            add_do(d, dal_familia.ElementID, 2980, -1990, 3420, -2420)
            add_do(d, dal_patente.ElementID, 3500, -1990, 3940, -2440)

            d.DiagramObjects.Refresh()
            d.Update()
            apply_styles(d.DiagramID, is_der=False)

        populate_class_diagram(d_gen_clases)
        populate_class_diagram(d7)

        # -------------------------------------------------------------
        # DER REFINEMENT
        # -------------------------------------------------------------
        pkg5 = ea.GetPackageByID(5)
        d_gen_der = None
        for d in pkg5.Diagrams:
            if d.Name == "DER General del Sistema":
                d_gen_der = d
                break
        if not d_gen_der:
            d_gen_der = pkg5.Diagrams.AddNew("DER General del Sistema", "Logical")
            d_gen_der.Update()
            pkg5.Diagrams.Refresh()

        d5 = ea.GetDiagramByID(5)

        def populate_der_diagram(d):
            clear_diagram(d)

            # Negocio Tables (Top Band: Y = -60 to -380)
            add_do(d, tab_paciente.ElementID, 50, -60, 340, -360)
            add_do(d, tab_turno.ElementID, 420, -60, 720, -360)
            add_do(d, tab_bloque.ElementID, 800, -60, 1070, -260)
            add_do(d, tab_agenda.ElementID, 1150, -60, 1430, -250)

            # Transversal / Seguridad Tables (Middle Band: Y = -440 to -780)
            add_do(d, tab_dv.ElementID, 50, -450, 340, -590)
            add_do(d, tab_bitacora.ElementID, 420, -450, 700, -660)
            add_do(d, tab_usuario.ElementID, 800, -380, 1100, -730)

            # RBAC Permisos / Familias Tables (Right/Lower Band: Y = -360 to -780)
            add_do(d, tab_perfil.ElementID, 1200, -420, 1440, -540)
            add_do(d, tab_perfil_fam.ElementID, 1530, -340, 1770, -460)
            add_do(d, tab_perfil_perm.ElementID, 1530, -550, 1770, -670)
            add_do(d, tab_familia.ElementID, 1860, -340, 2100, -460)
            add_do(d, tab_fam_fam.ElementID, 2190, -340, 2450, -470)
            add_do(d, tab_perm_fam.ElementID, 1860, -550, 2120, -670)
            add_do(d, tab_permiso.ElementID, 2190, -550, 2430, -670)
            add_do(d, tab_perm_btn.ElementID, 2520, -550, 2820, -700)

            d.DiagramObjects.Refresh()
            d.Update()
            apply_styles(d.DiagramID, is_der=True)

        populate_der_diagram(d_gen_der)
        populate_der_diagram(d5)

        # -------------------------------------------------------------
        # EXPORT IMAGES
        # -------------------------------------------------------------
        print("Exporting diagrams to PNG...")
        img_clases_scratch = os.path.join(scratch_dir, "general_clases_bll_dal_be.png")
        img_clases_artifact = os.path.join(artifact_dir, "general_clases_bll_dal_be.png")
        proj.PutDiagramImageToFile(d_gen_clases.DiagramGUID, img_clases_scratch, 1)
        shutil.copyfile(img_clases_scratch, img_clases_artifact)
        print(f"Exported Class Diagram: {img_clases_scratch}")

        img_der_scratch = os.path.join(scratch_dir, "general_der.png")
        img_der_artifact = os.path.join(artifact_dir, "general_der.png")
        proj.PutDiagramImageToFile(d_gen_der.DiagramGUID, img_der_scratch, 1)
        shutil.copyfile(img_der_scratch, img_der_artifact)
        print(f"Exported DER Diagram: {img_der_scratch}")

        proj.PutDiagramImageToFile(d7.DiagramGUID, os.path.join(scratch_dir, "diagram_7_class_model.png"), 1)
        proj.PutDiagramImageToFile(d5.DiagramGUID, os.path.join(scratch_dir, "diagram_5_domain_model.png"), 1)

        print("=== PERFECT GEOMETRY COMPLETED SUCCESSFULLY! ===")

    except Exception as ex:
        print(f"ERROR: {ex}")
        import traceback
        traceback.print_exc()

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    build_general_diagrams()
