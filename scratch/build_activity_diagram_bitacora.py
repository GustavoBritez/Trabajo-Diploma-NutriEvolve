import win32com.client
import os
import sys

EAP_PATH = r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP"
EXPORT_DIR = r"C:\Users\Danie\Desktop\GIT\Diagramas\Exported"
SCRATCH_DIR = r"C:\Users\Danie\Desktop\GIT\scratch"

def log(msg):
    print(f"[INFO] {msg}", flush=True)

def main():
    ea = win32com.client.Dispatch("EA.Repository")
    opened = ea.OpenFile(EAP_PATH)
    if not opened:
        print("[ERROR] Could not open EA repository.")
        sys.exit(1)

    log("Opened EA Repository.")
    pkg = ea.GetPackageByID(16)
    log(f"Working in Package 16: '{pkg.Name}'")

    diag_name = 'Diagrama de Actividad "Bitacora de Cambios"'

    existing_diag_id = None
    for i in range(pkg.Diagrams.Count):
        d = pkg.Diagrams.GetAt(i)
        if d.Name == diag_name:
            existing_diag_id = d.DiagramID
            break

    if existing_diag_id:
        diag = ea.GetDiagramByID(existing_diag_id)
    else:
        diag = pkg.Diagrams.AddNew(diag_name, "Activity")
        diag.Update()
        pkg.Diagrams.Refresh()

    def get_or_create_element(name, elem_type, stereotype="", ntype=0):
        for i in range(pkg.Elements.Count):
            el = pkg.Elements.GetAt(i)
            if el.Name == name and el.Type == elem_type:
                if stereotype and el.Stereotype != stereotype:
                    el.Stereotype = stereotype
                    el.Update()
                return el
        el = pkg.Elements.AddNew(name, elem_type)
        if stereotype:
            el.Stereotype = stereotype
        if ntype:
            el.NType = ntype
        el.Update()
        pkg.Elements.Refresh()
        log(f"Created Element [{elem_type}] '{name}' (ID: {el.ElementID})")
        return el

    # Create ActivityPartitions (Swimlanes)
    p_admin = get_or_create_element("Administrador / Usuario", "ActivityPartition")
    p_sys = get_or_create_element("Sistema NutriEvolve", "ActivityPartition")

    # Create Nodes
    n_start = get_or_create_element("Inicio_Bitacora", "StateNode", stereotype="ActivityInitial")
    n_start.Name = ""
    n_start.Update()

    a_open = get_or_create_element("Abrir pantalla 'Bitácora de Cambios'", "Action")
    a_load = get_or_create_element("Cargar historial de modificaciones y listar registros", "Action")
    a_filter = get_or_create_element("Seleccionar filtros (Paciente, Fechas, Tipo de Operación)", "Action")
    a_search = get_or_create_element("Presionar botón 'Buscar / Filtrar'", "Action")
    a_show = get_or_create_element("Filtrar y presentar versiones históricas en grilla", "Action")
    d_revert = get_or_create_element("¿Desea restaurar una versión anterior?", "Decision")
    a_exit = get_or_create_element("Presionar botón 'Salir / Cerrar'", "Action")
    a_select_rev = get_or_create_element("Seleccionar versión histórica y presionar 'Activar Versión'", "Action")
    a_ask_confirm = get_or_create_element("Solicitar confirmación al usuario para revertir", "Action")
    d_confirm = get_or_create_element("¿Confirma la restauración?", "Decision")
    a_restore_data = get_or_create_element("Restaurar datos del paciente y contactos asociados", "Action")
    a_audit_dv = get_or_create_element("Registrar evento en auditoría y recalcular Dígito Verificador", "Action")
    a_notify = get_or_create_element("Mostrar mensaje de éxito y refrescar listado", "Action")

    n_end_user = get_or_create_element("Fin_Consulta", "StateNode", stereotype="ActivityFinal")
    n_end_user.Name = ""
    n_end_user.Update()

    n_end_sys = get_or_create_element("Fin_Restauracion", "StateNode", stereotype="ActivityFinal")
    n_end_sys.Name = ""
    n_end_sys.Update()

    # Clean old fin if present in diagram
    for i in range(pkg.Elements.Count):
        el = pkg.Elements.GetAt(i)
        if el.Name == "Fin_Bitacora":
            # remove from diagram
            for j in range(diag.DiagramObjects.Count - 1, -1, -1):
                dobj = diag.DiagramObjects.GetAt(j)
                if dobj.ElementID == el.ElementID:
                    diag.DiagramObjects.Delete(j)
                    diag.DiagramObjects.Refresh()
            break

    def set_diagram_object(el, left, right, top, bottom, seq=0):
        found = None
        for i in range(diag.DiagramObjects.Count):
            dobj = diag.DiagramObjects.GetAt(i)
            if dobj.ElementID == el.ElementID:
                found = dobj
                break
        if not found:
            found = diag.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            found.ElementID = el.ElementID

        found.left = left
        found.right = right
        found.top = top
        found.bottom = bottom
        if seq:
            found.Sequence = seq
        found.Update()
        return found

    # 1. Swimlanes at back
    set_diagram_object(p_admin, 40, 440, -10, -980, seq=1)
    set_diagram_object(p_sys, 460, 880, -10, -980, seq=1)

    # 2. Nodes in Administrador Lane (X ~ 50 to 430)
    set_diagram_object(n_start, 228, 252, -40, -64)
    set_diagram_object(a_open, 120, 360, -95, -145)
    set_diagram_object(a_filter, 100, 380, -185, -240)
    set_diagram_object(a_search, 130, 350, -270, -315)
    set_diagram_object(d_revert, 160, 320, -355, -425)

    # Branches from d_revert
    set_diagram_object(a_exit, 60, 220, -475, -530)
    set_diagram_object(n_end_user, 128, 152, -580, -604)

    set_diagram_object(a_select_rev, 250, 430, -475, -535)
    set_diagram_object(d_confirm, 275, 410, -580, -645)

    # 3. Nodes in Sistema NutriEvolve Lane (X ~ 470 to 870)
    set_diagram_object(a_load, 530, 810, -95, -145)
    set_diagram_object(a_show, 530, 810, -270, -320)
    set_diagram_object(a_ask_confirm, 530, 810, -475, -530)
    set_diagram_object(a_restore_data, 520, 820, -670, -725)
    set_diagram_object(a_audit_dv, 510, 830, -755, -815)
    set_diagram_object(a_notify, 530, 810, -845, -900)
    set_diagram_object(n_end_sys, 658, 682, -930, -954)

    diag.DiagramObjects.Refresh()
    diag.Update()

    # Clear old connectors of n_start, a_exit, a_notify to avoid obsolete paths
    def remove_connectors(el):
        for k in range(el.Connectors.Count - 1, -1, -1):
            el.Connectors.Delete(k)
        el.Connectors.Refresh()

    remove_connectors(a_exit)
    remove_connectors(a_notify)
    remove_connectors(d_confirm)

    def add_flow(src, dst, label=""):
        for c in src.Connectors:
            if c.SupplierID == dst.ElementID and c.Type == "ControlFlow" and c.Name == label:
                return c
        conn = src.Connectors.AddNew(label, "ControlFlow")
        conn.SupplierID = dst.ElementID
        conn.Update()
        src.Connectors.Refresh()
        log(f"Connected: '{src.Name or src.Stereotype}' --> '{dst.Name or dst.Stereotype}' [{label}]")
        return conn

    # Connectors
    add_flow(n_start, a_open)
    add_flow(a_open, a_load)
    add_flow(a_load, a_filter)
    add_flow(a_filter, a_search)
    add_flow(a_search, a_show)
    add_flow(a_show, d_revert)

    # Decision revert
    add_flow(d_revert, a_exit, "[No]")
    add_flow(d_revert, a_select_rev, "[Sí]")

    add_flow(a_exit, n_end_user)

    add_flow(a_select_rev, a_ask_confirm)
    add_flow(a_ask_confirm, d_confirm)

    # Decision confirm
    add_flow(d_confirm, a_exit, "[No - Cancelar]")
    add_flow(d_confirm, a_restore_data, "[Sí - Confirmar]")

    add_flow(a_restore_data, a_audit_dv)
    add_flow(a_audit_dv, a_notify)
    add_flow(a_notify, n_end_sys)

    diag.Update()
    ea.ReloadDiagram(diag.DiagramID)

    # Export diagram image
    project = ea.GetProjectInterface()
    os.makedirs(EXPORT_DIR, exist_ok=True)
    out_png_1 = os.path.join(EXPORT_DIR, "Diagrama_de_Actividad_Bitacora_de_Cambios.png")
    out_png_2 = os.path.join(SCRATCH_DIR, "Diagrama_de_Actividad_Bitacora_de_Cambios.png")

    res1 = project.PutDiagramImageToFile(diag.DiagramGUID, out_png_1, 1)
    res2 = project.PutDiagramImageToFile(diag.DiagramGUID, out_png_2, 1)
    log(f"Exported PNG 1: {out_png_1} (result: {res1})")
    log(f"Exported PNG 2: {out_png_2} (result: {res2})")

    ea.CloseFile()
    ea.Exit()
    log("Complete successfully!")

if __name__ == "__main__":
    main()
