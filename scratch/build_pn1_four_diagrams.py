import win32com.client
import os
import shutil

def build_pn1_diagrams():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()
    scratch_dir = r"c:\Users\Danie\Desktop\GIT\TD\scratch"
    artifact_dir = r"C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e"

    try:
        print("=== REFINING 4 PN1 DIAGRAMS ===")
        pkg7 = ea.GetPackageByID(7)

        def apply_styles(diag_id):
            style_ex = "TConnectorNotation=UML 2.1;ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;"
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

        # =============================================================
        # 1. DIAGRAMA 1: "Proceso de Negocio 1 General"
        # =============================================================
        el_p4_pac = ea.GetElementByID(417)
        el_p4_turno = ea.GetElementByID(418)
        el_p4_agenda = ea.GetElementByID(424)
        el_p4_bloque = ea.GetElementByID(425)
        el_p4_usuario = ea.GetElementByID(426)
        el_p4_istate = ea.GetElementByID(419)
        el_p4_st_sol = ea.GetElementByID(420)
        el_p4_st_conf = ea.GetElementByID(421)
        el_p4_st_asist = ea.GetElementByID(422)
        el_p4_st_canc = ea.GetElementByID(423)

        d_pn1_gen = get_or_create_diagram(pkg7, "Proceso de Negocio 1 General")
        clear_diagram(d_pn1_gen)
        # Row 1: Paciente, Turno, BloqueHorario, AgendaMedica, Usuario
        add_do(d_pn1_gen, el_p4_pac.ElementID, 50, -60, 480, -450)
        add_do(d_pn1_gen, el_p4_turno.ElementID, 560, -60, 1140, -560)
        add_do(d_pn1_gen, el_p4_bloque.ElementID, 1220, -60, 1540, -280)
        add_do(d_pn1_gen, el_p4_agenda.ElementID, 1620, -60, 2060, -320)
        add_do(d_pn1_gen, el_p4_usuario.ElementID, 2140, -60, 2480, -340)

        # State pattern under Turno with generous spacing
        add_do(d_pn1_gen, el_p4_istate.ElementID, 680, -620, 1020, -730)
        add_do(d_pn1_gen, el_p4_st_sol.ElementID, 50, -780, 480, -890)
        add_do(d_pn1_gen, el_p4_st_conf.ElementID, 530, -780, 960, -890)
        add_do(d_pn1_gen, el_p4_st_asist.ElementID, 1010, -780, 1440, -890)
        add_do(d_pn1_gen, el_p4_st_canc.ElementID, 1490, -780, 1920, -890)

        d_pn1_gen.DiagramObjects.Refresh()
        d_pn1_gen.Update()
        apply_styles(d_pn1_gen.DiagramID)

        # =============================================================
        # 2. DIAGRAMA 2: "Proceso de Negocio 1 BE"
        # =============================================================
        be_pac = ea.GetElementByID(662)
        be_turno = ea.GetElementByID(663)
        be_agenda = ea.GetElementByID(664)
        be_bloque = ea.GetElementByID(665)
        be_usuario = ea.GetElementByID(666)
        be_istate = ea.GetElementByID(749)
        be_st_sol = ea.GetElementByID(750)
        be_st_conf = ea.GetElementByID(751)
        be_st_asist = ea.GetElementByID(752)
        be_st_canc = ea.GetElementByID(753)

        d_pn1_be = get_or_create_diagram(pkg7, "Proceso de Negocio 1 BE")
        clear_diagram(d_pn1_be)
        add_do(d_pn1_be, be_pac.ElementID, 50, -60, 400, -360)
        add_do(d_pn1_be, be_turno.ElementID, 480, -60, 880, -360)
        add_do(d_pn1_be, be_bloque.ElementID, 960, -60, 1220, -280)
        add_do(d_pn1_be, be_agenda.ElementID, 1300, -60, 1580, -250)
        add_do(d_pn1_be, be_usuario.ElementID, 1660, -60, 1940, -300)

        # State pattern
        add_do(d_pn1_be, be_istate.ElementID, 530, -420, 830, -530)
        add_do(d_pn1_be, be_st_sol.ElementID, 100, -570, 480, -680)
        add_do(d_pn1_be, be_st_conf.ElementID, 520, -570, 900, -680)
        add_do(d_pn1_be, be_st_asist.ElementID, 940, -570, 1320, -680)
        add_do(d_pn1_be, be_st_canc.ElementID, 1360, -570, 1740, -680)

        d_pn1_be.DiagramObjects.Refresh()
        d_pn1_be.Update()
        apply_styles(d_pn1_be.DiagramID)

        # =============================================================
        # 3. DIAGRAMA 3: "Proceso de Negocio 1 BLL" (2 Rows: clean routing)
        # =============================================================
        bll_pac = ea.GetElementByID(757)
        bll_turno = ea.GetElementByID(756)
        bll_agenda = ea.GetElementByID(758)
        bll_evento = ea.GetElementByID(762)
        bll_dv = ea.GetElementByID(763)

        d_pn1_bll = get_or_create_diagram(pkg7, "Proceso de Negocio 1 BLL")
        clear_diagram(d_pn1_bll)
        # Row 1: Core Business BLL
        add_do(d_pn1_bll, bll_pac.ElementID, 50, -60, 450, -260)
        add_do(d_pn1_bll, bll_turno.ElementID, 530, -60, 1000, -390)
        add_do(d_pn1_bll, bll_agenda.ElementID, 1080, -60, 1580, -240)
        # Row 2: Transversal Services BLL (Bitacora & DV)
        add_do(d_pn1_bll, bll_evento.ElementID, 530, -460, 980, -620)
        add_do(d_pn1_bll, bll_dv.ElementID, 1080, -460, 1580, -650)

        d_pn1_bll.DiagramObjects.Refresh()
        d_pn1_bll.Update()
        apply_styles(d_pn1_bll.DiagramID)

        # =============================================================
        # 4. DIAGRAMA 4: "Proceso de Negocio 1 DAL"
        # =============================================================
        dal_pac = ea.GetElementByID(760)
        dal_turno = ea.GetElementByID(759)
        dal_agenda = ea.GetElementByID(761)
        dal_evento = ea.GetElementByID(764)
        dal_dv = ea.GetElementByID(765)
        dal_conn = ea.GetElementByID(794)

        d_pn1_dal = get_or_create_diagram(pkg7, "Proceso de Negocio 1 DAL")
        clear_diagram(d_pn1_dal)
        # Row 1: Core DALs
        add_do(d_pn1_dal, dal_pac.ElementID, 50, -60, 420, -250)
        add_do(d_pn1_dal, dal_turno.ElementID, 490, -60, 930, -460)
        add_do(d_pn1_dal, dal_agenda.ElementID, 1010, -60, 1490, -230)
        add_do(d_pn1_dal, dal_evento.ElementID, 1570, -60, 1910, -210)
        add_do(d_pn1_dal, dal_dv.ElementID, 1990, -60, 2400, -270)
        # Row 2: Conexion centered underneath
        add_do(d_pn1_dal, dal_conn.ElementID, 1010, -540, 1490, -720)

        d_pn1_dal.DiagramObjects.Refresh()
        d_pn1_dal.Update()
        apply_styles(d_pn1_dal.DiagramID)

        # =============================================================
        # EXPORT ALL 4 DIAGRAMS TO PNG
        # =============================================================
        print("Exporting diagrams to PNG...")
        diag_exports = [
            (d_pn1_gen, "pn1_general.png"),
            (d_pn1_be, "pn1_be.png"),
            (d_pn1_bll, "pn1_bll.png"),
            (d_pn1_dal, "pn1_dal.png")
        ]

        for d, fname in diag_exports:
            f_scratch = os.path.join(scratch_dir, fname)
            f_art = os.path.join(artifact_dir, fname)
            proj.PutDiagramImageToFile(d.DiagramGUID, f_scratch, 1)
            shutil.copyfile(f_scratch, f_art)
            print(f"Exported: {fname} -> {f_scratch}")

        print("=== ALL 4 PN1 DIAGRAMS REFINED SUCCESSFULLY! ===")

    except Exception as ex:
        print(f"ERROR: {ex}")
        import traceback
        traceback.print_exc()

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    build_pn1_diagrams()
