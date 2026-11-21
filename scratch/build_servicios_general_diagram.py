import win32com.client
import os
import sys

EAP_PATH = r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP"
EXPORT_DIR = r"C:\Users\Danie\Desktop\GIT\Diagramas\Exported"

def log(msg):
    print(f"[INFO] {msg}", flush=True)

def apply_styles(ea, diag_id):
    style_ex = "TConnectorNotation=UML 2.1;ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=0;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;"
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

def ensure_diagram_object(diag, el_id, left, top, right, bottom):
    for i in range(diag.DiagramObjects.Count):
        dobj = diag.DiagramObjects.GetAt(i)
        if dobj.ElementID == el_id:
            dobj.left = left
            dobj.top = -top
            dobj.right = right
            dobj.bottom = -bottom
            dobj.Update()
            return dobj
    dobj = diag.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
    dobj.ElementID = el_id
    dobj.left = left
    dobj.top = -top
    dobj.right = right
    dobj.bottom = -bottom
    dobj.Update()
    diag.DiagramObjects.Refresh()
    return dobj

def get_or_create_diagram(pkg, name, dtype="Logical"):
    for i in range(pkg.Diagrams.Count):
        d = pkg.Diagrams.GetAt(i)
        if d.Name == name:
            return d
    d = pkg.Diagrams.AddNew(name, dtype)
    d.Update()
    pkg.Diagrams.Refresh()
    return d

def build_servicios_general():
    ea = win32com.client.Dispatch("EA.Repository")
    if not ea.OpenFile(EAP_PATH):
        print("[ERROR] Could not open TD.EAP")
        sys.exit(1)

    try:
        log("=== BUILDING DIAGRAMA CLASES SERVICIOS GENERAL ===")
        pkg7 = ea.GetPackageByID(7) # Class Model

        # Fetch Elements
        # Tier 1: BE & Base Services
        be_observable = None
        be_observer = None
        be_session = None
        for el in pkg7.Elements:
            if el.Name == "IIdiomaObservable": be_observable = el
            elif el.Name == "IIdiomaObserver": be_observer = el
            elif el.Name == "ServicesSessionManager": be_session = el

        be_idioma = ea.GetElementByID(778)
        be_usuario = ea.GetElementByID(666)
        be_bitacora = ea.GetElementByID(777)
        be_perfil = ea.GetElementByID(779)
        be_familia_svc = ea.GetElementByID(780)
        be_patente_svc = ea.GetElementByID(781)
        be_bcrypt = ea.GetElementByID(1027)
        be_aes = ea.GetElementByID(1026)

        # Tier 2: BLL
        bll_idioma = ea.GetElementByID(783)
        bll_bitacora = ea.GetElementByID(762)
        bll_dv = ea.GetElementByID(763)
        bll_backup = ea.GetElementByID(784)
        bll_usuario = ea.GetElementByID(782)
        bll_perfil = ea.GetElementByID(785)
        bll_familia = ea.GetElementByID(786)
        bll_patente = ea.GetElementByID(787)

        # Tier 3: DAL & Conexion
        dal_idioma = ea.GetElementByID(789)
        dal_bitacora = ea.GetElementByID(764)
        dal_dv = ea.GetElementByID(765)
        dal_backup = ea.GetElementByID(790)
        dal_usuario = ea.GetElementByID(788)
        dal_perfil = ea.GetElementByID(791)
        dal_familia = ea.GetElementByID(792)
        dal_patente = ea.GetElementByID(793)
        dal_conexion = ea.GetElementByID(794)

        diag = get_or_create_diagram(pkg7, "Diagrama Clases Servicios General", "Logical")
        clear_diagram(diag)

        # Perfect Symmetrical 3-Tier Layout
        layout = [
            # ==============================================================
            # TIER 1: BE & SERVICIOS BASE (Y = 40 to 860)
            # ==============================================================
            # Col 1: Internacionalización & Sesión
            (be_observable.ElementID,       40,   40,  380,  170),
            (be_observer.ElementID,         40,  200,  380,  310),
            (be_session.ElementID,          40,  340,  440,  660),
            (be_idioma.ElementID,           40,  690,  380,  830),

            # Col 2: Auditoría
            (be_bitacora.ElementID,        500,   40,  880,  260),

            # Col 3: Integridad & Criptografía
            (be_bcrypt.ElementID,          940,   40, 1340,  230),
            (be_aes.ElementID,             940,  260, 1340,  450),

            # Col 4: Usuarios & Autenticación
            (be_usuario.ElementID,        1400,   40, 1820,  380),

            # Col 5: Perfiles (Base Composite)
            (be_perfil.ElementID,         1880,   40, 2240,  200),

            # Col 6 & 7: Familias & Patentes (Composite RBAC)
            (be_familia_svc.ElementID,    2320,   40, 2680,  220),
            (be_patente_svc.ElementID,    2740,   40, 3080,  200),

            # ==============================================================
            # TIER 2: BLL (LÓGICA DE NEGOCIO) (Y = 920 to 1340)
            # ==============================================================
            (bll_idioma.ElementID,          40,  920,  400, 1090),
            (bll_bitacora.ElementID,       500,  920,  880, 1130),
            (bll_dv.ElementID,             940,  920, 1340, 1150),
            (bll_backup.ElementID,         940, 1180, 1340, 1340),
            (bll_usuario.ElementID,       1400,  920, 1820, 1260),
            (bll_perfil.ElementID,        1880,  920, 2280, 1320),
            (bll_familia.ElementID,       2320,  920, 2740, 1200),
            (bll_patente.ElementID,       2800,  920, 3220, 1220),

            # ==============================================================
            # TIER 3: DAL & CONEXION (PERSISTENCIA) (Y = 1420 to 1920)
            # ==============================================================
            (dal_idioma.ElementID,          40, 1420,  400, 1600),
            (dal_bitacora.ElementID,       500, 1420,  880, 1600),
            (dal_dv.ElementID,             940, 1420, 1340, 1600),
            (dal_backup.ElementID,         940, 1640, 1340, 1800),
            (dal_usuario.ElementID,       1400, 1420, 1820, 1660),
            (dal_conexion.ElementID,      1400, 1720, 1820, 1920),
            (dal_perfil.ElementID,        1880, 1420, 2280, 1780),
            (dal_familia.ElementID,       2320, 1420, 2740, 1740),
            (dal_patente.ElementID,       2800, 1420, 3220, 1760),
        ]

        for el_id, l, t, r, b in layout:
            ensure_diagram_object(diag, el_id, l, t, r, b)

        diag.Update()
        apply_styles(ea, diag.DiagramID)
        ea.ReloadDiagram(diag.DiagramID)

        # Export high-res PNG
        proj = ea.GetProjectInterface()
        os.makedirs(EXPORT_DIR, exist_ok=True)
        img_general = os.path.join(EXPORT_DIR, "Diagrama_Clases_Servicios_General.png")
        res = proj.PutDiagramImageToFile(diag.DiagramGUID, img_general, 1)

        log(f"Exported Diagrama_Clases_Servicios_General to {img_general} (Result: {res})")
        print("=== DIAGRAMA CLASES SERVICIOS GENERAL COMPLETED SUCCESSFULLY ===")

    except Exception as ex:
        print(f"[ERROR] {ex}")
        import traceback
        traceback.print_exc()

    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    build_servicios_general()
