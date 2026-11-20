import win32com.client
import os
import sys
import xml.etree.ElementTree as ET

EAP_PATH = r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP"
EXPORT_DIR = r"C:\Users\Danie\Desktop\GIT\Diagramas\Exported"
SCRATCH_DIR = r"C:\Users\Danie\Desktop\GIT\scratch"

def log(msg):
    print(f"[INFO] {msg}", flush=True)

def get_or_create_package(parent_pkg, name):
    for i in range(parent_pkg.Packages.Count):
        p = parent_pkg.Packages.GetAt(i)
        if p.Name == name or name.lower() in p.Name.lower():
            return p
    new_pkg = parent_pkg.Packages.AddNew(name, "Package")
    new_pkg.Update()
    parent_pkg.Packages.Refresh()
    log(f"Created Package: {name} (ID: {new_pkg.PackageID})")
    return new_pkg

def get_or_create_element(ea, pkg, name, elem_type, stereotype=""):
    clean_name = name.replace("'", "''")
    sql = f"SELECT Object_ID FROM t_object WHERE Package_ID = {pkg.PackageID} AND Name = '{clean_name}' AND Object_Type = '{elem_type}'"
    xml_str = ea.SQLQuery(sql)
    root = ET.fromstring(xml_str)
    rows = root.findall(".//Row")
    if rows:
        obj_id = int(rows[0].find("Object_ID").text)
        el = ea.GetElementByID(obj_id)
        if stereotype and el.Stereotype != stereotype:
            el.Stereotype = stereotype
            el.Update()
        return el

    el = pkg.Elements.AddNew(name, elem_type)
    if stereotype:
        el.Stereotype = stereotype
    el.Update()
    pkg.Elements.Refresh()
    log(f"Created Element [{elem_type}] '{name}' (ID: {el.ElementID})")
    return el

def get_or_create_diagram(ea, pkg, name, diag_type):
    clean_name = name.replace("'", "''")
    sql = f"SELECT Diagram_ID FROM t_diagram WHERE Package_ID = {pkg.PackageID} AND Name = '{clean_name}'"
    xml_str = ea.SQLQuery(sql)
    root = ET.fromstring(xml_str)
    rows = root.findall(".//Row")
    if rows:
        d_id = int(rows[0].find("Diagram_ID").text)
        return ea.GetDiagramByID(d_id)

    diag = pkg.Diagrams.AddNew(name, diag_type)
    diag.Update()
    pkg.Diagrams.Refresh()
    log(f"Created Diagram [{diag_type}] '{name}' (ID: {diag.DiagramID})")
    return diag

def add_attribute_if_not_exists(el, name, att_type, stereotype=""):
    for a in el.Attributes:
        if a.Name == name:
            return a
    a = el.Attributes.AddNew(name, att_type)
    if stereotype:
        a.Stereotype = stereotype
    a.Update()
    el.Attributes.Refresh()
    return a

def add_method_if_not_exists(el, name, return_type):
    for m in el.Methods:
        if m.Name == name:
            return m
    m = el.Methods.AddNew(name, return_type)
    m.ReturnType = return_type
    m.Update()
    el.Methods.Refresh()
    return m

def ensure_diagram_object(diag, el, left, top, right, bottom):
    for i in range(diag.DiagramObjects.Count):
        dobj = diag.DiagramObjects.GetAt(i)
        if dobj.ElementID == el.ElementID:
            dobj.left = left
            dobj.top = -top
            dobj.right = right
            dobj.bottom = -bottom
            dobj.Update()
            return dobj

    dobj = diag.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
    dobj.ElementID = el.ElementID
    dobj.left = left
    dobj.top = -top
    dobj.right = right
    dobj.bottom = -bottom
    dobj.Update()
    diag.DiagramObjects.Refresh()
    return dobj

def get_or_create_connector(ea, src, tgt, conn_type, name="", stereotype="", subtype=""):
    for c in src.Connectors:
        if c.SupplierID == tgt.ElementID and c.Type == conn_type and (not name or c.Name == name):
            if stereotype and c.Stereotype != stereotype:
                c.Stereotype = stereotype
                c.Update()
            return c
    c = src.Connectors.AddNew(name, conn_type)
    c.SupplierID = tgt.ElementID
    if stereotype:
        c.Stereotype = stereotype
    if subtype:
        c.SubType = subtype
    c.Update()
    src.Connectors.Refresh()
    return c

def create_sequence_message(ea, diag_id, seq, src, dst, name, subtype, y, x_coords):
    clean_name = name.replace("'", "''")
    conn = src.Connectors.AddNew(clean_name, "Sequence")
    conn.SupplierID = dst.ElementID
    conn.SequenceNo = seq
    conn.SubType = subtype
    conn.DiagramID = diag_id
    conn.Update()
    src.Connectors.Refresh()

    cid = conn.ConnectorID
    sx = x_coords.get(src.ElementID, 100)
    ex = x_coords.get(dst.ElementID, 200)

    if src.ElementID == dst.ElementID:
        ex = sx + 30
        ey = y - 15
    else:
        ey = y

    action_tag = "Call" if subtype == "SynchCall" else ("Return" if subtype == "Return" else "Call")
    sql_update = f"""
    UPDATE t_connector
    SET PtStartX = {sx}, PtEndX = {ex}, PtStartY = {y}, PtEndY = {ey},
        PDATA1 = '{action_tag}',
        RouteStyle = 1, LineColor = -1
    WHERE Connector_ID = {cid}
    """
    ea.Execute(sql_update)
    return conn

# ==============================================================================
# 1. BITACORA DE CAMBIOS DIAGRAMS
# ==============================================================================
def build_bitacora(ea, pkg_primary):
    log("=== 1. BUILDING BITACORA DE CAMBIOS DIAGRAMS ===")
    pkg_bitacora = get_or_create_package(pkg_primary, "T06b - Bitacora de Cambios")

    # Diagram 1: Diagrama de Caso de uso "Bitacora de Cambios"
    diag_uc = get_or_create_diagram(ea, pkg_bitacora, 'Diagrama de Caso de uso "Bitacora de Cambios"', "Use Case")
    diag_uc.Notes = "Diagrama de Casos de Uso para el módulo de Bitácora y Control de Cambios (T06b / Auditoría NutriEvolve)."
    diag_uc.Update()

    for i in range(diag_uc.DiagramObjects.Count - 1, -1, -1):
        diag_uc.DiagramObjects.Delete(i)
    diag_uc.DiagramObjects.Refresh()

    actor_admin = get_or_create_element(ea, pkg_bitacora, "Administrador", "Actor")
    boundary = get_or_create_element(ea, pkg_bitacora, "Sistema NutriEvolve - Bitacora y Control de Cambios", "Boundary")
    uc_main = get_or_create_element(ea, pkg_bitacora, "Consultar Bitacora de Cambios", "UseCase")
    uc_hist = get_or_create_element(ea, pkg_bitacora, "Visualizar Historial y Versiones de la Entidad", "UseCase")
    uc_filtro = get_or_create_element(ea, pkg_bitacora, "Filtrar Cambios por Entidad, Fechas y Operacion", "UseCase")
    uc_revertir = get_or_create_element(ea, pkg_bitacora, "Recomponer Estado Anterior (Rollback / Activar Version)", "UseCase")
    uc_audit = get_or_create_element(ea, pkg_bitacora, "Auditar Reversion y Recalcular Digito Verificador", "UseCase")

    get_or_create_connector(ea, actor_admin, uc_main, "Association", "")
    get_or_create_connector(ea, actor_admin, uc_revertir, "Association", "")
    get_or_create_connector(ea, uc_main, uc_hist, "Dependency", "", "include")
    get_or_create_connector(ea, uc_main, uc_filtro, "Dependency", "", "include")
    get_or_create_connector(ea, uc_revertir, uc_main, "Dependency", "", "extend")
    get_or_create_connector(ea, uc_revertir, uc_audit, "Dependency", "", "include")

    ensure_diagram_object(diag_uc, boundary, 220, 40, 780, 520)
    ensure_diagram_object(diag_uc, actor_admin, 50, 180, 110, 290)
    ensure_diagram_object(diag_uc, uc_main, 280, 100, 470, 170)
    ensure_diagram_object(diag_uc, uc_filtro, 540, 70, 740, 140)
    ensure_diagram_object(diag_uc, uc_hist, 540, 160, 740, 230)
    ensure_diagram_object(diag_uc, uc_revertir, 280, 290, 490, 370)
    ensure_diagram_object(diag_uc, uc_audit, 540, 380, 750, 460)

    diag_uc.Update()
    ea.SaveDiagram(diag_uc.DiagramID)

    # Standardize names of existing diagrams in Package 16
    diag119 = ea.GetDiagramByID(119)
    if diag119:
        diag119.Name = 'Diagrama Clases Bitacora de Cambios'
        diag119.Notes = "Diagrama de Clases de 4 Capas (UI, BE, BLL, DAL) y Criptografía para el Control de Cambios."
        diag119.Update()
        ea.SaveDiagram(119)

    diag120 = ea.GetDiagramByID(120)
    if diag120:
        diag120.Name = 'Diagrama Base de datos Bitacora de Cambios'
        diag120.Notes = "Diagrama de Base de Datos / DER para el Control y Bitácora de Cambios con Tablas Históricas y Notación Crows Foot."
        diag120.Update()
        ea.SaveDiagram(120)

    diag121 = ea.GetDiagramByID(121)
    if diag121:
        diag121.Name = 'Diagrama Secuencia Bitacora de Cambios'
        diag121.Notes = "Diagrama de Secuencia para Auditoría, Criptografía Reversible AES-256 y Reversión Histórica de Cambios."
        diag121.Update()
        ea.SaveDiagram(121)

    log("Bitácora de Cambios diagrams synchronized.")

# ==============================================================================
# 2. SERIALIZACION DIAGRAMS
# ==============================================================================
def build_serializacion(ea, pkg_primary):
    log("=== 2. BUILDING SERIALIZACION DIAGRAMS ===")
    pkg_ser = get_or_create_package(pkg_primary, "Serializacion")

    # ----------------------------------------------------
    # A) Diagrama de Caso de uso "Serializacion"
    # ----------------------------------------------------
    diag_uc = get_or_create_diagram(ea, pkg_ser, 'Diagrama de Caso de uso "Serializacion"', "Use Case")
    diag_uc.Notes = "Diagrama de Caso de Uso para la Serialización de Pacientes a archivo XML estándar (Requisito A03)."
    diag_uc.Update()

    for i in range(diag_uc.DiagramObjects.Count - 1, -1, -1):
        diag_uc.DiagramObjects.Delete(i)
    diag_uc.DiagramObjects.Refresh()

    actor_nutri = get_or_create_element(ea, pkg_ser, "Nutricionista", "Actor")
    boundary = get_or_create_element(ea, pkg_ser, "Sistema NutriEvolve - Serializacion XML de Pacientes", "Boundary")
    uc_main = get_or_create_element(ea, pkg_ser, "Serializar Pacientes a XML", "UseCase")
    uc_path = get_or_create_element(ea, pkg_ser, "Seleccionar Ruta de Destino en Disco", "UseCase")
    uc_val = get_or_create_element(ea, pkg_ser, "Validar Registros en Matriz de Clientes", "UseCase")
    uc_aes = get_or_create_element(ea, pkg_ser, "Descifrar Datos Sensibles Reversibles (AES-256)", "UseCase")
    uc_struct = get_or_create_element(ea, pkg_ser, "Estructurar Contenedor XML ([XmlRoot], [XmlElement])", "UseCase")
    uc_write = get_or_create_element(ea, pkg_ser, "Escribir Flujo UTF-8 en Archivo XML", "UseCase")

    get_or_create_connector(ea, actor_nutri, uc_main, "Association", "")
    get_or_create_connector(ea, uc_main, uc_path, "Dependency", "", "include")
    get_or_create_connector(ea, uc_main, uc_val, "Dependency", "", "include")
    get_or_create_connector(ea, uc_aes, uc_main, "Dependency", "", "extend")
    get_or_create_connector(ea, uc_main, uc_struct, "Dependency", "", "include")
    get_or_create_connector(ea, uc_main, uc_write, "Dependency", "", "include")

    ensure_diagram_object(diag_uc, boundary, 220, 30, 820, 520)
    ensure_diagram_object(diag_uc, actor_nutri, 40, 180, 100, 290)
    ensure_diagram_object(diag_uc, uc_main, 270, 160, 460, 240)
    ensure_diagram_object(diag_uc, uc_path, 560, 50, 780, 120)
    ensure_diagram_object(diag_uc, uc_val, 560, 140, 780, 210)
    ensure_diagram_object(diag_uc, uc_aes, 270, 350, 480, 430)
    ensure_diagram_object(diag_uc, uc_struct, 560, 240, 790, 320)
    ensure_diagram_object(diag_uc, uc_write, 560, 360, 780, 440)

    diag_uc.Update()
    ea.SaveDiagram(diag_uc.DiagramID)

    # ----------------------------------------------------
    # B) Diagrama Clases Serializacion
    # ----------------------------------------------------
    diag_cl = get_or_create_diagram(ea, pkg_ser, 'Diagrama Clases Serializacion', "Logical")
    diag_cl.Notes = "Diagrama de Clases para la Serialización de Pacientes a XML con DTOs, Servicios Criptográficos y Framework XML."
    diag_cl.Update()

    for i in range(diag_cl.DiagramObjects.Count - 1, -1, -1):
        diag_cl.DiagramObjects.Delete(i)
    diag_cl.DiagramObjects.Refresh()

    # Classes
    el_gui = get_or_create_element(ea, pkg_ser, "frmMaestroClientes_DNI101", "Class", "GUI")
    add_attribute_if_not_exists(el_gui, "_pacienteBLL", "PacienteBLL_DNI101", "private")
    add_attribute_if_not_exists(el_gui, "_dniCifradoActivo", "bool", "private")
    add_attribute_if_not_exists(el_gui, "txtRutaSerializar", "TextBox", "private")
    add_attribute_if_not_exists(el_gui, "dgvClientes", "DataGridView", "private")
    add_method_if_not_exists(el_gui, "btnExaminarSerializar_Click(object, EventArgs)", "void")
    add_method_if_not_exists(el_gui, "btnSerializar_Click(object, EventArgs)", "void")
    add_method_if_not_exists(el_gui, "ObtenerValorCelda(DataGridViewRow, string, string)", "string")
    add_method_if_not_exists(el_gui, "AgregarMensaje(string)", "void")

    el_cont = get_or_create_element(ea, pkg_ser, "ClientesXmlContainer", "Class", "serializable")
    add_attribute_if_not_exists(el_cont, "Clientes", "List<ClienteXmlItem>", "public")

    el_item = get_or_create_element(ea, pkg_ser, "ClienteXmlItem", "Class", "serializable")
    add_attribute_if_not_exists(el_item, "DNI", "string", "public")
    add_attribute_if_not_exists(el_item, "APELLIDOS", "string", "public")
    add_attribute_if_not_exists(el_item, "NOMBRES", "string", "public")
    add_attribute_if_not_exists(el_item, "EMAIL", "string", "public")
    add_attribute_if_not_exists(el_item, "CELULAR", "string", "public")
    add_attribute_if_not_exists(el_item, "DIRECCION", "string", "public")

    el_bll = get_or_create_element(ea, pkg_ser, "PacienteBLL_DNI101", "Class", "BLL")
    add_method_if_not_exists(el_bll, "DescifrarDniAES256(string)", "string")
    add_method_if_not_exists(el_bll, "CifrarDniAES256(string)", "string")

    el_aes = get_or_create_element(ea, pkg_ser, "ServicioCriptografiaAES", "Class", "Service")
    add_method_if_not_exists(el_aes, "CifrarAES256(string)", "string")
    add_method_if_not_exists(el_aes, "DescifrarAES256(string)", "string")
    add_method_if_not_exists(el_aes, "EsTextoCifrado(string)", "bool")

    el_ser_fw = get_or_create_element(ea, pkg_ser, "XmlSerializer", "Class", "framework")
    add_method_if_not_exists(el_ser_fw, "Serialize(TextWriter, object)", "void")
    add_method_if_not_exists(el_ser_fw, "Deserialize(TextReader)", "object")

    el_sw = get_or_create_element(ea, pkg_ser, "StreamWriter", "Class", "framework")
    add_method_if_not_exists(el_sw, "Write(string)", "void")
    add_method_if_not_exists(el_sw, "Close()", "void")

    # Connectors
    get_or_create_connector(ea, el_gui, el_cont, "Dependency", "instancia")
    get_or_create_connector(ea, el_cont, el_item, "Aggregation", "contiene", "", "Strong")
    get_or_create_connector(ea, el_gui, el_bll, "Dependency", "invoca")
    get_or_create_connector(ea, el_bll, el_aes, "Dependency", "utiliza")
    get_or_create_connector(ea, el_gui, el_ser_fw, "Dependency", "ejecuta")
    get_or_create_connector(ea, el_gui, el_sw, "Dependency", "escribe")
    get_or_create_connector(ea, el_ser_fw, el_sw, "Dependency", "vuelca datos")

    # Layout: 2 rows balanced
    ensure_diagram_object(diag_cl, el_gui, 40, 50, 360, 250)
    ensure_diagram_object(diag_cl, el_cont, 440, 50, 680, 160)
    ensure_diagram_object(diag_cl, el_item, 760, 50, 1000, 240)
    ensure_diagram_object(diag_cl, el_bll, 40, 320, 320, 440)
    ensure_diagram_object(diag_cl, el_aes, 40, 490, 320, 610)
    ensure_diagram_object(diag_cl, el_ser_fw, 440, 260, 680, 390)
    ensure_diagram_object(diag_cl, el_sw, 760, 300, 980, 420)

    diag_cl.Update()
    ea.SaveDiagram(diag_cl.DiagramID)

    # ----------------------------------------------------
    # C) Diagrama Secuencia Serializacion
    # ----------------------------------------------------
    diag_seq = get_or_create_diagram(ea, pkg_ser, 'Diagrama Secuencia Serializacion', "Sequence")
    diag_seq.Notes = "Diagrama de Secuencia que detalla el flujo de serialización XML con cifrado AES reversible."
    diag_seq.Update()

    for i in range(diag_seq.DiagramObjects.Count - 1, -1, -1):
        diag_seq.DiagramObjects.Delete(i)
    diag_seq.DiagramObjects.Refresh()

    life_nutri = get_or_create_element(ea, pkg_ser, "Nutricionista", "Actor")
    life_gui = get_or_create_element(ea, pkg_ser, "gui: frmMaestroClientes_DNI101", "Sequence", "boundary")
    life_aes = get_or_create_element(ea, pkg_ser, "aes: ServicioCriptografiaAES", "Sequence", "")
    life_dto = get_or_create_element(ea, pkg_ser, "dto: ClientesXmlContainer", "Sequence", "entity")
    life_ser = get_or_create_element(ea, pkg_ser, "ser: XmlSerializer", "Sequence", "")
    life_fs = get_or_create_element(ea, pkg_ser, "fs: StreamWriter", "Sequence", "")

    lifeline_bottom = 980
    ensure_diagram_object(diag_seq, life_nutri, 40, 50, 120, lifeline_bottom)
    ensure_diagram_object(diag_seq, life_gui, 200, 50, 330, lifeline_bottom)
    ensure_diagram_object(diag_seq, life_aes, 410, 50, 560, lifeline_bottom)
    ensure_diagram_object(diag_seq, life_dto, 630, 50, 780, lifeline_bottom)
    ensure_diagram_object(diag_seq, life_ser, 850, 50, 980, lifeline_bottom)
    ensure_diagram_object(diag_seq, life_fs, 1050, 50, 1180, lifeline_bottom)

    diag_seq.Update()

    ea.Execute(f"DELETE FROM t_connector WHERE DiagramID = {diag_seq.DiagramID}")

    x_map = {
        life_nutri.ElementID: 80,
        life_gui.ElementID: 265,
        life_aes.ElementID: 485,
        life_dto.ElementID: 705,
        life_ser.ElementID: 915,
        life_fs.ElementID: 1115
    }

    messages = [
        (1, life_nutri, life_gui, "btnExaminarSerializar_Click()", "SynchCall", -100),
        (2, life_gui, life_gui, "SaveFileDialog.ShowDialog() : string rutaXml", "SynchCall", -135),
        (3, life_nutri, life_gui, "btnSerializar_Click()", "SynchCall", -180),
        (4, life_gui, life_gui, "Validar dgvClientes.Rows.Count > 0", "SynchCall", -215),
        (5, life_gui, life_dto, "new ClientesXmlContainer()", "SynchCall", -260),
        (6, life_gui, life_gui, "loop [Por cada fila en dgvClientes]", "SynchCall", -300),
        (7, life_gui, life_aes, "EsTextoCifrado(dniVal) : bool", "SynchCall", -340),
        (8, life_aes, life_gui, "retorna true/false", "Return", -375),
        (9, life_gui, life_aes, "DescifrarAES256(dniVal) : string dniPlano", "SynchCall", -415),
        (10, life_aes, life_gui, "retorna dniPlano", "Return", -450),
        (11, life_gui, life_dto, "Clientes.Add(new ClienteXmlItem { DNI, APELLIDOS, ... })", "SynchCall", -495),
        (12, life_gui, life_ser, "new XmlSerializer(typeof(ClientesXmlContainer))", "SynchCall", -550),
        (13, life_gui, life_fs, "new StreamWriter(rutaXml, false, UTF8)", "SynchCall", -600),
        (14, life_gui, life_ser, "Serialize(writer, container)", "SynchCall", -650),
        (15, life_ser, life_fs, "Write Xml Declaration & Elements <Clientes>...", "SynchCall", -690),
        (16, life_ser, life_gui, "void (Serialización completada)", "Return", -730),
        (17, life_gui, life_fs, "Close() / Dispose()", "SynchCall", -775),
        (18, life_gui, life_gui, "AgregarMensaje('SERIALIZACIÓN EXITOSA: {count} clientes...')", "SynchCall", -820),
        (19, life_gui, life_nutri, "MessageBox.Show('Serialización XML Exitosa')", "Return", -865),
    ]

    for seq, src, dst, mname, stype, y in messages:
        create_sequence_message(ea, diag_seq.DiagramID, seq, src, dst, mname, stype, y, x_map)

    diag_seq.Update()
    ea.SaveDiagram(diag_seq.DiagramID)
    log("Serialización diagrams synchronized.")

# ==============================================================================
# 3. DESERIALIZACION DIAGRAMS
# ==============================================================================
def build_deserializacion(ea, pkg_primary):
    log("=== 3. BUILDING DESERIALIZACION DIAGRAMS ===")
    pkg_deser = get_or_create_package(pkg_primary, "Deserializacion")

    # ----------------------------------------------------
    # A) Diagrama de Caso de uso "Deserializacion"
    # ----------------------------------------------------
    diag_uc = get_or_create_diagram(ea, pkg_deser, 'Diagrama de Caso de uso "Deserializacion"', "Use Case")
    diag_uc.Notes = "Diagrama de Caso de Uso para la Deserialización y Sincronización de Pacientes desde archivo XML (Requisito A03)."
    diag_uc.Update()

    for i in range(diag_uc.DiagramObjects.Count - 1, -1, -1):
        diag_uc.DiagramObjects.Delete(i)
    diag_uc.DiagramObjects.Refresh()

    actor_nutri = get_or_create_element(ea, pkg_deser, "Nutricionista", "Actor")
    boundary = get_or_create_element(ea, pkg_deser, "Sistema NutriEvolve - Deserializacion y Sincronizacion XML", "Boundary")
    uc_main = get_or_create_element(ea, pkg_deser, "Deserializar y Sincronizar Pacientes desde XML", "UseCase")
    uc_path = get_or_create_element(ea, pkg_deser, "Seleccionar Archivo XML Origen en Disco", "UseCase")
    uc_val = get_or_create_element(ea, pkg_deser, "Validar Integridad y Esquema del Archivo XML", "UseCase")
    uc_aes = get_or_create_element(ea, pkg_deser, "Descifrar Identificadores Sensibles (AES-256)", "UseCase")
    uc_persist = get_or_create_element(ea, pkg_deser, "Persistir Pacientes en Base de Datos (Transaccional)", "UseCase")
    uc_audit = get_or_create_element(ea, pkg_deser, "Auditar Evento de Sincronizacion en Bitacora", "UseCase")
    uc_dv = get_or_create_element(ea, pkg_deser, "Recalcular Integridad con Digito Verificador (DVH/DVV)", "UseCase")
    uc_refresh = get_or_create_element(ea, pkg_deser, "Actualizar Grilla de Clientes en UI", "UseCase")

    get_or_create_connector(ea, actor_nutri, uc_main, "Association", "")
    get_or_create_connector(ea, uc_main, uc_path, "Dependency", "", "include")
    get_or_create_connector(ea, uc_main, uc_val, "Dependency", "", "include")
    get_or_create_connector(ea, uc_aes, uc_main, "Dependency", "", "extend")
    get_or_create_connector(ea, uc_main, uc_persist, "Dependency", "", "include")
    get_or_create_connector(ea, uc_main, uc_audit, "Dependency", "", "include")
    get_or_create_connector(ea, uc_main, uc_dv, "Dependency", "", "include")
    get_or_create_connector(ea, uc_main, uc_refresh, "Dependency", "", "include")

    ensure_diagram_object(diag_uc, boundary, 220, 20, 840, 580)
    ensure_diagram_object(diag_uc, actor_nutri, 40, 200, 100, 310)
    ensure_diagram_object(diag_uc, uc_main, 270, 190, 480, 270)
    ensure_diagram_object(diag_uc, uc_path, 580, 40, 800, 110)
    ensure_diagram_object(diag_uc, uc_val, 580, 130, 800, 200)
    ensure_diagram_object(diag_uc, uc_aes, 270, 360, 480, 440)
    ensure_diagram_object(diag_uc, uc_persist, 580, 220, 810, 290)
    ensure_diagram_object(diag_uc, uc_audit, 580, 310, 810, 380)
    ensure_diagram_object(diag_uc, uc_dv, 580, 400, 810, 470)
    ensure_diagram_object(diag_uc, uc_refresh, 580, 490, 810, 560)

    diag_uc.Update()
    ea.SaveDiagram(diag_uc.DiagramID)

    # ----------------------------------------------------
    # B) Diagrama Clases Deserializacion
    # ----------------------------------------------------
    diag_cl = get_or_create_diagram(ea, pkg_deser, 'Diagrama Clases Deserializacion', "Logical")
    diag_cl.Notes = "Diagrama de Clases para Des-serialización XML, Persistencia Transaccional, Auditoría y Dígito Verificador."
    diag_cl.Update()

    for i in range(diag_cl.DiagramObjects.Count - 1, -1, -1):
        diag_cl.DiagramObjects.Delete(i)
    diag_cl.DiagramObjects.Refresh()

    # Classes
    el_gui = get_or_create_element(ea, pkg_deser, "frmMaestroClientes_DNI101", "Class", "GUI")
    add_method_if_not_exists(el_gui, "btnExaminarDesSerializar_Click(object, EventArgs)", "void")
    add_method_if_not_exists(el_gui, "btnDesSerializar_Click(object, EventArgs)", "void")
    add_method_if_not_exists(el_gui, "CargarGrillaClientes()", "void")

    el_cont = get_or_create_element(ea, pkg_deser, "ClientesXmlContainer", "Class", "serializable")
    add_attribute_if_not_exists(el_cont, "Clientes", "List<ClienteXmlItem>", "public")

    el_item = get_or_create_element(ea, pkg_deser, "ClienteXmlItem", "Class", "serializable")
    add_attribute_if_not_exists(el_item, "DNI", "string", "public")
    add_attribute_if_not_exists(el_item, "APELLIDOS", "string", "public")
    add_attribute_if_not_exists(el_item, "NOMBRES", "string", "public")
    add_attribute_if_not_exists(el_item, "EMAIL", "string", "public")
    add_attribute_if_not_exists(el_item, "CELULAR", "string", "public")
    add_attribute_if_not_exists(el_item, "DIRECCION", "string", "public")

    el_bll = get_or_create_element(ea, pkg_deser, "PacienteBLL_DNI101", "Class", "BLL")
    add_method_if_not_exists(el_bll, "ImportarClientesDesdeXml(List<ClienteTuple>)", "void")
    add_method_if_not_exists(el_bll, "DescifrarDniAES256(string)", "string")

    el_dal = get_or_create_element(ea, pkg_deser, "PacienteDAL_DNI101", "Class", "DAL")
    add_method_if_not_exists(el_dal, "ImportarClientesDesdeXml(List<ClienteTuple>, int)", "void")

    el_bit_bll = get_or_create_element(ea, pkg_deser, "BitacoraBLL", "Class", "BLL")
    add_method_if_not_exists(el_bit_bll, "RegistrarBitacora(int, string, int, string)", "void")

    el_bit_dal = get_or_create_element(ea, pkg_deser, "BitacoraDAL", "Class", "DAL")
    add_method_if_not_exists(el_bit_dal, "RegistrarBitacora(int, string, int, string)", "void")

    el_dv_bll = get_or_create_element(ea, pkg_deser, "DigitoVerificadorBLL", "Class", "BLL")
    add_method_if_not_exists(el_dv_bll, "RecalcularYPersistir()", "void")

    el_dv_dal = get_or_create_element(ea, pkg_deser, "DigitoVerificadorDAL", "Class", "DAL")
    add_method_if_not_exists(el_dv_dal, "RecalcularDVH()", "void")
    add_method_if_not_exists(el_dv_dal, "RecalcularDVV()", "void")

    el_aes = get_or_create_element(ea, pkg_deser, "ServicioCriptografiaAES", "Class", "Service")
    add_method_if_not_exists(el_aes, "DescifrarAES256(string)", "string")
    add_method_if_not_exists(el_aes, "EsTextoCifrado(string)", "bool")

    el_ser_fw = get_or_create_element(ea, pkg_deser, "XmlSerializer", "Class", "framework")
    add_method_if_not_exists(el_ser_fw, "Deserialize(TextReader)", "object")

    el_sr = get_or_create_element(ea, pkg_deser, "StreamReader", "Class", "framework")
    add_method_if_not_exists(el_sr, "ReadToEnd()", "string")
    add_method_if_not_exists(el_sr, "Close()", "void")

    # Connectors
    get_or_create_connector(ea, el_gui, el_ser_fw, "Dependency", "invoca")
    get_or_create_connector(ea, el_gui, el_sr, "Dependency", "lee archivo")
    get_or_create_connector(ea, el_gui, el_cont, "Dependency", "recibe")
    get_or_create_connector(ea, el_cont, el_item, "Aggregation", "contiene", "", "Strong")
    get_or_create_connector(ea, el_gui, el_bll, "Dependency", "delega")
    get_or_create_connector(ea, el_bll, el_dal, "Dependency", "persiste")
    get_or_create_connector(ea, el_bll, el_bit_bll, "Dependency", "audita")
    get_or_create_connector(ea, el_bll, el_dv_bll, "Dependency", "recalcula")
    get_or_create_connector(ea, el_bit_bll, el_bit_dal, "Dependency", "graba")
    get_or_create_connector(ea, el_dv_bll, el_dv_dal, "Dependency", "persiste DV")
    get_or_create_connector(ea, el_dal, el_aes, "Dependency", "descifra AES")

    # Layout: 3 columns grid
    ensure_diagram_object(diag_cl, el_gui, 40, 50, 320, 220)
    ensure_diagram_object(diag_cl, el_ser_fw, 380, 50, 600, 160)
    ensure_diagram_object(diag_cl, el_sr, 660, 50, 880, 160)

    ensure_diagram_object(diag_cl, el_cont, 940, 50, 1160, 160)
    ensure_diagram_object(diag_cl, el_item, 940, 220, 1160, 420)

    ensure_diagram_object(diag_cl, el_bll, 40, 290, 320, 410)
    ensure_diagram_object(diag_cl, el_dal, 380, 290, 660, 410)
    ensure_diagram_object(diag_cl, el_aes, 720, 290, 900, 410)

    ensure_diagram_object(diag_cl, el_bit_bll, 40, 480, 320, 590)
    ensure_diagram_object(diag_cl, el_bit_dal, 380, 480, 640, 590)

    ensure_diagram_object(diag_cl, el_dv_bll, 40, 660, 320, 770)
    ensure_diagram_object(diag_cl, el_dv_dal, 380, 660, 640, 770)

    diag_cl.Update()
    ea.SaveDiagram(diag_cl.DiagramID)

    # ----------------------------------------------------
    # C) Diagrama Secuencia Deserializacion
    # ----------------------------------------------------
    diag_seq = get_or_create_diagram(ea, pkg_deser, 'Diagrama Secuencia Deserializacion', "Sequence")
    diag_seq.Notes = "Diagrama de Secuencia que detalla la deserialización XML, inserción transaccional, auditoría y recálculo de integridad."
    diag_seq.Update()

    for i in range(diag_seq.DiagramObjects.Count - 1, -1, -1):
        diag_seq.DiagramObjects.Delete(i)
    diag_seq.DiagramObjects.Refresh()

    life_nutri = get_or_create_element(ea, pkg_deser, "Nutricionista", "Actor")
    life_gui = get_or_create_element(ea, pkg_deser, "gui: frmMaestroClientes_DNI101", "Sequence", "boundary")
    life_ser = get_or_create_element(ea, pkg_deser, "ser: XmlSerializer", "Sequence", "")
    life_fs = get_or_create_element(ea, pkg_deser, "fs: StreamReader", "Sequence", "")
    life_bll = get_or_create_element(ea, pkg_deser, "bll: PacienteBLL_DNI101", "Sequence", "")
    life_dal = get_or_create_element(ea, pkg_deser, "dal: PacienteDAL_DNI101", "Sequence", "")
    life_db = get_or_create_element(ea, pkg_deser, "db: SQL Server (Transaccion)", "Sequence", "database")
    life_bit = get_or_create_element(ea, pkg_deser, "bit: BitacoraBLL", "Sequence", "")
    life_dv = get_or_create_element(ea, pkg_deser, "dv: DigitoVerificadorBLL", "Sequence", "")

    lifeline_bottom = 1250
    ensure_diagram_object(diag_seq, life_nutri, 30, 50, 110, lifeline_bottom)
    ensure_diagram_object(diag_seq, life_gui, 160, 50, 290, lifeline_bottom)
    ensure_diagram_object(diag_seq, life_ser, 340, 50, 470, lifeline_bottom)
    ensure_diagram_object(diag_seq, life_fs, 520, 50, 650, lifeline_bottom)
    ensure_diagram_object(diag_seq, life_bll, 700, 50, 840, lifeline_bottom)
    ensure_diagram_object(diag_seq, life_dal, 890, 50, 1030, lifeline_bottom)
    ensure_diagram_object(diag_seq, life_db, 1080, 50, 1220, lifeline_bottom)
    ensure_diagram_object(diag_seq, life_bit, 1270, 50, 1380, lifeline_bottom)
    ensure_diagram_object(diag_seq, life_dv, 1430, 50, 1560, lifeline_bottom)

    diag_seq.Update()

    ea.Execute(f"DELETE FROM t_connector WHERE DiagramID = {diag_seq.DiagramID}")

    x_map = {
        life_nutri.ElementID: 70,
        life_gui.ElementID: 225,
        life_ser.ElementID: 405,
        life_fs.ElementID: 585,
        life_bll.ElementID: 770,
        life_dal.ElementID: 960,
        life_db.ElementID: 1150,
        life_bit.ElementID: 1325,
        life_dv.ElementID: 1495
    }

    messages = [
        (1, life_nutri, life_gui, "btnExaminarDesSerializar_Click()", "SynchCall", -100),
        (2, life_gui, life_gui, "OpenFileDialog.ShowDialog() : string rutaXml", "SynchCall", -135),
        (3, life_nutri, life_gui, "btnDesSerializar_Click()", "SynchCall", -180),
        (4, life_gui, life_gui, "Validar File.Exists(rutaXml)", "SynchCall", -215),
        (5, life_gui, life_ser, "new XmlSerializer(typeof(ClientesXmlContainer))", "SynchCall", -260),
        (6, life_gui, life_fs, "new StreamReader(rutaXml, Encoding.UTF8)", "SynchCall", -300),
        (7, life_gui, life_ser, "Deserialize(reader)", "SynchCall", -345),
        (8, life_ser, life_fs, "Read XML Stream", "SynchCall", -380),
        (9, life_ser, life_gui, "retorna container (ClientesXmlContainer)", "Return", -420),
        (10, life_gui, life_fs, "Close() / Dispose()", "SynchCall", -460),
        (11, life_gui, life_gui, "Validar container.Clientes.Count > 0", "SynchCall", -495),
        (12, life_gui, life_gui, "Preparar lista tuplas (dni, ape, nom, email, cel, dir)", "SynchCall", -535),
        (13, life_gui, life_bll, "ImportarClientesDesdeXml(listaClientes)", "SynchCall", -580),
        (14, life_bll, life_bll, "ObtenerDniUsuarioActual() : int dniActual", "SynchCall", -615),
        (15, life_bll, life_dal, "ImportarClientesDesdeXml(clientes, dniActual)", "SynchCall", -660),
        (16, life_dal, life_db, "ExecuteTransaction(tx)", "SynchCall", -700),
        (17, life_dal, life_db, "EXEC sp_set_session_context @key=N'UsuarioDni', @value=dniActual", "SynchCall", -740),
        (18, life_dal, life_dal, "loop [Por cada cliente] -> DescifrarAES256 si viene cifrado", "SynchCall", -780),
        (19, life_dal, life_db, "UPSERT Pacientes_DNI101, Telefonos, Emails", "SynchCall", -820),
        (20, life_db, life_dal, "Commit Transaction", "Return", -860),
        (21, life_dal, life_bll, "void (Sincronización Exitosa)", "Return", -895),
        (22, life_bll, life_bit, "RegistrarBitacora(2, 'Des-serialización XML: sincronizados...', dniActual, 'MaestroClientes')", "SynchCall", -940),
        (23, life_bit, life_bll, "void (Auditoría grabada)", "Return", -975),
        (24, life_bll, life_dv, "RecalcularYPersistir()", "SynchCall", -1015),
        (25, life_dv, life_bll, "void (Integridad verificada)", "Return", -1050),
        (26, life_bll, life_gui, "void", "Return", -1085),
        (27, life_gui, life_gui, "CargarGrillaClientes()", "SynchCall", -1120),
        (28, life_gui, life_gui, "AgregarMensaje('DES-SERIALIZACIÓN EXITOSA: {count} registros...')", "SynchCall", -1155),
        (29, life_gui, life_nutri, "MessageBox.Show('Des-serialización XML Exitosa')", "Return", -1190),
    ]

    for seq, src, dst, mname, stype, y in messages:
        create_sequence_message(ea, diag_seq.DiagramID, seq, src, dst, mname, stype, y, x_map)

    diag_seq.Update()
    ea.SaveDiagram(diag_seq.DiagramID)
    log("Deserialización diagrams synchronized.")

# ==============================================================================
# 4. EXPORT ALL REQUESTED DIAGRAMS
# ==============================================================================
def export_all(ea):
    log("=== 4. EXPORTING ALL REQUESTED DIAGRAMS TO PNG ===")
    proj = ea.GetProjectInterface()
    os.makedirs(EXPORT_DIR, exist_ok=True)
    os.makedirs(SCRATCH_DIR, exist_ok=True)

    target_diagrams = [
        # Bitacora
        'Diagrama de Caso de uso "Bitacora de Cambios"',
        'Diagrama Secuencia Bitacora de Cambios',
        'Diagrama Clases Bitacora de Cambios',
        'Diagrama Base de datos Bitacora de Cambios',
        # Serializacion
        'Diagrama de Caso de uso "Serializacion"',
        'Diagrama Secuencia Serializacion',
        'Diagrama Clases Serializacion',
        # Deserializacion
        'Diagrama de Caso de uso "Deserializacion"',
        'Diagrama Secuencia Deserializacion',
        'Diagrama Clases Deserializacion',
    ]

    for d_name in target_diagrams:
        clean_name = d_name.replace('"', '').replace(' ', '_').replace(':', '')
        sql = f"SELECT Diagram_ID, GUID FROM t_diagram WHERE Name = '{d_name.replace("'", "''")}'"
        xml_str = ea.SQLQuery(sql)
        root = ET.fromstring(xml_str)
        rows = root.findall(".//Row")
        if not rows:
            log(f"WARNING: Diagram '{d_name}' not found!")
            continue

        d_id = int(rows[0].find("Diagram_ID").text)
        guid = rows[0].find("GUID").text

        file_exported = os.path.join(EXPORT_DIR, f"{clean_name}.png")
        file_scratch = os.path.join(SCRATCH_DIR, f"{clean_name}.png")

        proj.PutDiagramImageToFile(guid, file_exported, 1)
        proj.PutDiagramImageToFile(guid, file_scratch, 1)
        log(f"Exported [Diagram {d_id}] '{d_name}' -> {file_exported}")

def main():
    ea = win32com.client.Dispatch("EA.Repository")
    opened = ea.OpenFile(EAP_PATH)
    if not opened:
        print("[ERROR] Could not open TD.EAP", file=sys.stderr)
        sys.exit(1)

    try:
        log("Opened TD.EAP successfully.")
        pkg_primary = ea.GetPackageByID(4) # Primary Use Cases

        build_bitacora(ea, pkg_primary)
        build_serializacion(ea, pkg_primary)
        build_deserializacion(ea, pkg_primary)
        export_all(ea)

        log("ALL REQUESTED DIAGRAMS BUILT AND EXPORTED SUCCESSFULLY!")
    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    main()
