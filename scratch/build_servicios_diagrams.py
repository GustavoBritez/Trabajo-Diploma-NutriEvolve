import win32com.client
import os
import sys

EAP_PATH = r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP"
EXPORT_DIR = r"C:\Users\Danie\Desktop\GIT\Diagramas\Exported"

def log(msg):
    print(f"[INFO] {msg}", flush=True)

def apply_styles(ea, diag_id, is_der=False):
    t_conn = "TConnectorNotation=Information Engineering;" if is_der else "TConnectorNotation=UML 2.1;"
    style_ex = f"{t_conn}ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=0;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;"
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

def get_or_create_element(pkg, name, etype, stereotype=""):
    for i in range(pkg.Elements.Count):
        el = pkg.Elements.GetAt(i)
        if el.Name.lower() == name.lower() and el.Type.lower() == etype.lower():
            return el
    el = pkg.Elements.AddNew(name, etype)
    if stereotype:
        el.Stereotype = stereotype
    el.Update()
    pkg.Elements.Refresh()
    return el

def add_attribute_if_missing(el, name, atype, scope="Private"):
    for i in range(el.Attributes.Count):
        a = el.Attributes.GetAt(i)
        if a.Name.lower() == name.lower():
            return a
    a = el.Attributes.AddNew(name, atype)
    a.Visibility = scope
    a.Update()
    el.Attributes.Refresh()
    return a

def add_method_if_missing(el, name, rtype="void", params=None, scope="Public"):
    for i in range(el.Methods.Count):
        m = el.Methods.GetAt(i)
        if m.Name.lower() == name.lower():
            return m
    m = el.Methods.AddNew(name, rtype)
    m.Visibility = scope
    m.ReturnType = rtype
    m.Update()
    if params:
        for pname, ptype in params:
            param = m.Parameters.AddNew(pname, ptype)
            param.Update()
        m.Parameters.Refresh()
    el.Methods.Refresh()
    return m

def ensure_connector(el_source, el_target, conn_type, name="", stereotype=""):
    for i in range(el_source.Connectors.Count):
        c = el_source.Connectors.GetAt(i)
        if c.SupplierID == el_target.ElementID and c.Type.lower() == conn_type.lower():
            return c
    c = el_source.Connectors.AddNew(name, conn_type)
    c.SupplierID = el_target.ElementID
    if stereotype:
        c.Stereotype = stereotype
    c.Update()
    el_source.Connectors.Refresh()
    return c

def build_servicios_diagrams():
    ea = win32com.client.Dispatch("EA.Repository")
    if not ea.OpenFile(EAP_PATH):
        print("[ERROR] Could not open TD.EAP")
        sys.exit(1)

    try:
        log("=== BUILDING DIAGRAMAS DE SERVICIOS ===")
        pkg7 = ea.GetPackageByID(7) # Class Model
        pkg5 = ea.GetPackageByID(5) # Domain Model

        # -------------------------------------------------------------
        # 1. PREPARE SERVICES BE ELEMENTS
        # -------------------------------------------------------------
        log("Preparing Services BE elements...")
        be_usuario = ea.GetElementByID(666) # UsuarioBE
        be_bitacora = ea.GetElementByID(777) # EventoBE / BitacoraBE
        be_bitacora.Name = "BitacoraBE"
        be_bitacora.Alias = "EventoBE"
        be_bitacora.Update()

        be_idioma = ea.GetElementByID(778) # Idioma
        be_perfil = ea.GetElementByID(779) # Perfil
        be_familia_svc = ea.GetElementByID(780) # FamiliaServices
        be_patente_svc = ea.GetElementByID(781) # PatenteServices
        be_aes = ea.GetElementByID(1026) # ServicioCriptografiaAES
        be_bcrypt = ea.GetElementByID(1027) # ServicioBcrypt

        # Create IIdiomaObservable if missing
        be_observable = get_or_create_element(pkg7, "IIdiomaObservable", "Interface")
        add_method_if_missing(be_observable, "Suscribir", "void", [("observer", "IIdiomaObserver")])
        add_method_if_missing(be_observable, "Desuscribir", "void", [("observer", "IIdiomaObserver")])
        add_method_if_missing(be_observable, "Notificar", "void")

        # Create IIdiomaObserver if missing
        be_observer = get_or_create_element(pkg7, "IIdiomaObserver", "Interface")
        add_method_if_missing(be_observer, "ActualizarIdioma", "void")

        # Create ServicesSessionManager if missing
        be_session = get_or_create_element(pkg7, "ServicesSessionManager", "Class", "Singleton")
        add_attribute_if_missing(be_session, "_instancia", "ServicesSessionManager", "Private")
        add_attribute_if_missing(be_session, "Instancia", "ServicesSessionManager", "Public")
        add_attribute_if_missing(be_session, "usuarioActivo", "UsuarioBE", "Private")
        add_attribute_if_missing(be_session, "idiomaActual", "Idioma", "Private")
        add_attribute_if_missing(be_session, "observadores", "List<IIdiomaObserver>", "Private")
        add_attribute_if_missing(be_session, "_permisosUsuario", "List<PatenteServices>", "Private")
        add_attribute_if_missing(be_session, "BaseDatosCorruptaDetectada", "bool", "Public")

        add_method_if_missing(be_session, "IniciarSesion", "void", [("usuario", "UsuarioBE")])
        add_method_if_missing(be_session, "CerrarSesion", "void")
        add_method_if_missing(be_session, "ObtenerUsuario", "UsuarioBE")
        add_method_if_missing(be_session, "CambiarIdioma", "void", [("idioma", "Idioma")])
        add_method_if_missing(be_session, "Suscribir", "void", [("observer", "IIdiomaObserver")])
        add_method_if_missing(be_session, "Desuscribir", "void", [("observer", "IIdiomaObserver")])
        add_method_if_missing(be_session, "Notificar", "void")

        # Connectors for BE
        ensure_connector(be_session, be_observable, "Realisation")
        ensure_connector(be_observable, be_observer, "Dependency", "notifica", "use")
        ensure_connector(be_session, be_observer, "Aggregation", "observadores")
        ensure_connector(be_session, be_usuario, "Aggregation", "usuarioActivo")
        ensure_connector(be_session, be_idioma, "Aggregation", "idiomaActual")
        ensure_connector(be_session, be_patente_svc, "Aggregation", "permisos")
        ensure_connector(be_usuario, be_bcrypt, "Dependency", "hashea", "use")
        ensure_connector(be_usuario, be_aes, "Dependency", "cifra", "use")
        ensure_connector(be_bitacora, be_usuario, "Association", "audita")

        # -------------------------------------------------------------
        # DIAGRAM 1: Diagrama Clases Servicios BE
        # -------------------------------------------------------------
        log("Generating Diagrama Clases Servicios BE...")
        diag_be = get_or_create_diagram(pkg7, "Diagrama Clases Servicios BE", "Logical")
        clear_diagram(diag_be)

        # Layout Planar BE:
        # Left column: Observer Pattern (Internationalization)
        # Center-left: Session Manager & Idioma
        # Center: UsuarioBE & BitacoraBE
        # Right column: RBAC Composite (Perfil, FamiliaServices, PatenteServices)
        # Bottom-right: Criptografía (Bcrypt & AES)

        be_layout = [
            # Top-Left: Observer interfaces
            (be_observable.ElementID,       40,   40,  360,  180),
            (be_observer.ElementID,         40,  260,  360,  380),

            # Center-Left: Singleton ServicesSessionManager & Idioma
            (be_session.ElementID,         440,   40,  880,  360),
            (be_idioma.ElementID,          440,  440,  780,  590),

            # Center: UsuarioBE & BitacoraBE
            (be_usuario.ElementID,         960,   40, 1380,  380),
            (be_bitacora.ElementID,        960,  460, 1380,  680),

            # Right: Composite Pattern (Perfil, FamiliaServices, PatenteServices)
            (be_perfil.ElementID,         1480,   40, 1800,  200),
            (be_familia_svc.ElementID,    1420,  270, 1750,  450),
            (be_patente_svc.ElementID,    1820,  270, 2120,  430),

            # Bottom-Right: Cryptography Services
            (be_bcrypt.ElementID,         1420,  520, 1800,  710),
            (be_aes.ElementID,            1850,  520, 2250,  710),
        ]

        for el_id, l, t, r, b in be_layout:
            ensure_diagram_object(diag_be, el_id, l, t, r, b)

        diag_be.Update()
        apply_styles(ea, diag_be.DiagramID, is_der=False)
        ea.ReloadDiagram(diag_be.DiagramID)

        # -------------------------------------------------------------
        # DIAGRAM 2: Diagrama Clases Servicios BLL
        # -------------------------------------------------------------
        log("Generating Diagrama Clases Servicios BLL...")
        diag_bll = get_or_create_diagram(pkg7, "Diagrama Clases Servicios BLL", "Logical")
        clear_diagram(diag_bll)

        bll_usuario = ea.GetElementByID(782)
        bll_bitacora = ea.GetElementByID(762)
        bll_dv = ea.GetElementByID(763)
        bll_idioma = ea.GetElementByID(783)
        bll_backup = ea.GetElementByID(784)
        bll_perfil = ea.GetElementByID(785)
        bll_familia = ea.GetElementByID(786)
        bll_patente = ea.GetElementByID(787)

        # Connectors for BLL
        ensure_connector(bll_usuario, bll_bitacora, "Dependency", "audita", "use")
        ensure_connector(bll_usuario, bll_dv, "Dependency", "verifica", "use")
        ensure_connector(bll_usuario, bll_perfil, "Dependency", "autoriza", "use")
        ensure_connector(bll_perfil, bll_bitacora, "Dependency", "audita", "use")
        ensure_connector(bll_familia, bll_bitacora, "Dependency", "audita", "use")
        ensure_connector(bll_patente, bll_bitacora, "Dependency", "audita", "use")
        ensure_connector(bll_perfil, bll_familia, "Dependency", "administra", "use")
        ensure_connector(bll_perfil, bll_patente, "Dependency", "administra", "use")
        ensure_connector(bll_familia, bll_patente, "Dependency", "asigna", "use")
        ensure_connector(bll_backup, bll_bitacora, "Dependency", "audita", "use")

        # BLL Layout:
        # Tier 1 (Top): UsuarioBLL & PerfilBLL
        # Tier 2 (Middle): BitacoraBLL, DigitoVerificadorBLL, FamiliaBLL, PatenteBLL
        # Tier 3 (Bottom): BackupBLL & IdiomaBLL

        bll_layout = [
            # Top row: Core User & Profile Managers
            (bll_usuario.ElementID,        550,   40,  990,  380),
            (bll_perfil.ElementID,        1140,   40, 1580,  400),

            # Middle row: Bitacora, DV, FamiliaBLL, PatenteBLL
            (bll_bitacora.ElementID,        40,  460,  460,  670),
            (bll_dv.ElementID,             550,  460,  990,  690),
            (bll_familia.ElementID,       1080,  460, 1510,  700),
            (bll_patente.ElementID,       1580,  460, 2030,  710),

            # Bottom row: Transversal Services
            (bll_backup.ElementID,          40,  750,  460,  920),
            (bll_idioma.ElementID,         550,  750,  990,  910),
        ]

        for el_id, l, t, r, b in bll_layout:
            ensure_diagram_object(diag_bll, el_id, l, t, r, b)

        diag_bll.Update()
        apply_styles(ea, diag_bll.DiagramID, is_der=False)
        ea.ReloadDiagram(diag_bll.DiagramID)

        # -------------------------------------------------------------
        # DIAGRAM 3: Diagrama Clases Servicios DER
        # -------------------------------------------------------------
        log("Generating Diagrama Clases Servicios DER...")
        diag_der = get_or_create_diagram(pkg7, "Diagrama Clases Servicios DER", "Logical")
        clear_diagram(diag_der)

        tab_usuarios = ea.GetElementByID(987)
        tab_bitacora = ea.GetElementByID(988)
        tab_dv = ea.GetElementByID(989)
        tab_perfil = ea.GetElementByID(990)
        tab_perfil_fam = ea.GetElementByID(991)
        tab_perfil_perm = ea.GetElementByID(992)
        tab_familia = ea.GetElementByID(993)
        tab_fam_fam = ea.GetElementByID(994)
        tab_perm_fam = ea.GetElementByID(995)
        tab_permiso = ea.GetElementByID(996)
        tab_perm_btn = ea.GetElementByID(997)

        # DER Layout: completely planar, clean spacing, Crow's foot
        # Column 1 (Left): Bitacora & DV
        # Column 2 (Center-Left): Usuarios
        # Column 3 (Center): Perfil
        # Column 4 (Center-Right): Perfil_Familia & Perfil_Permiso
        # Column 5 (Right): Familia, Familia_Familia, Permiso_Familia, Permiso, Permiso_Boton

        der_layout = [
            # Auditoria e Integridad
            (tab_bitacora.ElementID,        40,   40,  340,  240),
            (tab_dv.ElementID,              40,  310,  340,  460),

            # Operador del sistema
            (tab_usuarios.ElementID,       420,   40,  740,  380),

            # Perfil base
            (tab_perfil.ElementID,         850,  150, 1100,  280),

            # Tablas intermedias de Perfiles
            (tab_perfil_fam.ElementID,    1200,   40, 1450,  170),
            (tab_perfil_perm.ElementID,   1200,  270, 1450,  400),

            # Familias y Familias compuestas
            (tab_familia.ElementID,       1550,   40, 1800,  170),
            (tab_fam_fam.ElementID,       1900,   40, 2180,  180),

            # Permisos y Permisos-Familias
            (tab_perm_fam.ElementID,      1550,  270, 1820,  400),
            (tab_permiso.ElementID,       1920,  270, 2170,  400),
            (tab_perm_btn.ElementID,      1920,  460, 2250,  620),
        ]

        for el_id, l, t, r, b in der_layout:
            ensure_diagram_object(diag_der, el_id, l, t, r, b)

        diag_der.Update()
        apply_styles(ea, diag_der.DiagramID, is_der=True)
        ea.ReloadDiagram(diag_der.DiagramID)

        # -------------------------------------------------------------
        # EXPORT PNG IMAGES
        # -------------------------------------------------------------
        proj = ea.GetProjectInterface()
        os.makedirs(EXPORT_DIR, exist_ok=True)

        img_be = os.path.join(EXPORT_DIR, "Diagrama_Clases_Servicios_BE.png")
        img_bll = os.path.join(EXPORT_DIR, "Diagrama_Clases_Servicios_BLL.png")
        img_der = os.path.join(EXPORT_DIR, "Diagrama_Clases_Servicios_DER.png")

        res_be = proj.PutDiagramImageToFile(diag_be.DiagramGUID, img_be, 1)
        res_bll = proj.PutDiagramImageToFile(diag_bll.DiagramGUID, img_bll, 1)
        res_der = proj.PutDiagramImageToFile(diag_der.DiagramGUID, img_der, 1)

        log(f"Exported BE Diagram to {img_be} (Result: {res_be})")
        log(f"Exported BLL Diagram to {img_bll} (Result: {res_bll})")
        log(f"Exported DER Diagram to {img_der} (Result: {res_der})")

        print("=== ALL THREE SERVICIOS DIAGRAMS BUILT SUCCESSFULLY ===")

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
    build_servicios_diagrams()
