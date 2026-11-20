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

    pkg = ea.GetPackageByID(16)
    diag_name = 'Diagrama de Actividad "Bitacora de Cambios"'

    diag = None
    for i in range(pkg.Diagrams.Count):
        d = pkg.Diagrams.GetAt(i)
        if d.Name == diag_name:
            diag = d
            break

    if not diag:
        log("Diagram not found!")
        sys.exit(1)

    # Clean out all StateNodes currently in Package 16 to start fresh with StateNodes
    for i in range(pkg.Elements.Count - 1, -1, -1):
        el = pkg.Elements.GetAt(i)
        if el.Type == "StateNode":
            log(f"Deleting StateNode: ID={el.ElementID}")
            pkg.Elements.Delete(i)
    pkg.Elements.Refresh()

    # Clear diagram objects for StateNodes
    for j in range(diag.DiagramObjects.Count - 1, -1, -1):
        dobj = diag.DiagramObjects.GetAt(j)
        try:
            el = ea.GetElementByID(dobj.ElementID)
            if el.Type == "StateNode":
                diag.DiagramObjects.Delete(j)
        except:
            diag.DiagramObjects.Delete(j)
    diag.DiagramObjects.Refresh()

    # Helper to find existing element
    def find_element(name):
        for i in range(pkg.Elements.Count):
            el = pkg.Elements.GetAt(i)
            if el.Name == name:
                return el
        return None

    # Get elements
    p_admin = find_element("Administrador / Usuario")
    p_sys = find_element("Sistema NutriEvolve")
    a_open = find_element("Abrir pantalla 'Bitácora de Cambios'")
    a_load = find_element("Cargar historial de modificaciones y listar registros")
    a_filter = find_element("Seleccionar filtros (Paciente, Fechas, Tipo de Operación)")
    a_search = find_element("Presionar botón 'Buscar / Filtrar'")
    a_show = find_element("Filtrar y presentar versiones históricas en grilla")
    d_revert = find_element("¿Desea restaurar una versión anterior?")
    a_exit = find_element("Presionar botón 'Salir / Cerrar'")
    a_select_rev = find_element("Seleccionar versión histórica y presionar 'Activar Versión'")
    a_ask_confirm = find_element("Solicitar confirmación al usuario para revertir")
    d_confirm = find_element("¿Confirma la restauración?")
    a_restore_data = find_element("Restaurar datos del paciente y contactos asociados")
    a_audit_dv = find_element("Registrar evento en auditoría y recalcular Dígito Verificador")
    a_notify = find_element("Mostrar mensaje de éxito y refrescar listado")

    # Create fresh StateNodes
    n_start = pkg.Elements.AddNew("", "StateNode")
    n_start.Stereotype = "ActivityInitial"
    n_start.Update()

    n_end_user = pkg.Elements.AddNew("", "StateNode")
    n_end_user.Stereotype = "ActivityFinal"
    n_end_user.Update()

    n_end_sys = pkg.Elements.AddNew("", "StateNode")
    n_end_sys.Stereotype = "ActivityFinal"
    n_end_sys.Update()
    pkg.Elements.Refresh()

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

    # Layout Coordinates
    # Swimlanes
    set_diagram_object(p_admin, 40, 440, -10, -960, seq=1)
    set_diagram_object(p_sys, 460, 880, -10, -960, seq=1)

    # Administrador Lane
    set_diagram_object(n_start, 228, 252, -40, -64)
    set_diagram_object(a_open, 120, 360, -95, -145)
    set_diagram_object(a_filter, 100, 380, -185, -240)
    set_diagram_object(a_search, 130, 350, -270, -315)
    set_diagram_object(d_revert, 160, 320, -355, -425)

    set_diagram_object(a_exit, 60, 220, -475, -530)
    set_diagram_object(n_end_user, 128, 152, -580, -604)

    set_diagram_object(a_select_rev, 250, 430, -475, -535)
    set_diagram_object(d_confirm, 270, 410, -580, -645)

    # Sistema NutriEvolve Lane
    set_diagram_object(a_load, 530, 810, -95, -145)
    set_diagram_object(a_show, 530, 810, -270, -320)
    set_diagram_object(a_ask_confirm, 530, 810, -475, -530)
    set_diagram_object(a_restore_data, 520, 820, -670, -725)
    set_diagram_object(a_audit_dv, 510, 830, -755, -815)
    set_diagram_object(a_notify, 530, 810, -845, -900)
    set_diagram_object(n_end_sys, 658, 682, -920, -944)

    diag.DiagramObjects.Refresh()
    diag.Update()

    # Re-wire connectors cleanly
    for el in [n_start, a_open, a_load, a_filter, a_search, a_show, d_revert, a_exit, a_select_rev, a_ask_confirm, d_confirm, a_restore_data, a_audit_dv, a_notify]:
        for k in range(el.Connectors.Count - 1, -1, -1):
            el.Connectors.Delete(k)
        el.Connectors.Refresh()

    def add_flow(src, dst, label=""):
        conn = src.Connectors.AddNew(label, "ControlFlow")
        conn.SupplierID = dst.ElementID
        conn.Update()
        src.Connectors.Refresh()
        log(f"Connected: '{src.Name or src.Stereotype}' --> '{dst.Name or dst.Stereotype}' [{label}]")
        return conn

    add_flow(n_start, a_open)
    add_flow(a_open, a_load)
    add_flow(a_load, a_filter)
    add_flow(a_filter, a_search)
    add_flow(a_search, a_show)
    add_flow(a_show, d_revert)

    # Decisions
    add_flow(d_revert, a_exit, "[No]")
    add_flow(d_revert, a_select_rev, "[Sí]")

    add_flow(a_exit, n_end_user)

    add_flow(a_select_rev, a_ask_confirm)
    add_flow(a_ask_confirm, d_confirm)

    add_flow(d_confirm, n_end_user, "[No - Cancelar]")
    add_flow(d_confirm, a_restore_data, "[Sí - Confirmar]")

    add_flow(a_restore_data, a_audit_dv)
    add_flow(a_audit_dv, a_notify)
    add_flow(a_notify, n_end_sys)

    diag.Update()
    ea.ReloadDiagram(diag.DiagramID)

    # Export diagram images
    project = ea.GetProjectInterface()
    out_png_1 = os.path.join(EXPORT_DIR, "Diagrama_de_Actividad_Bitacora_de_Cambios.png")
    out_png_2 = os.path.join(SCRATCH_DIR, "Diagrama_de_Actividad_Bitacora_de_Cambios.png")

    res1 = project.PutDiagramImageToFile(diag.DiagramGUID, out_png_1, 1)
    res2 = project.PutDiagramImageToFile(diag.DiagramGUID, out_png_2, 1)
    log(f"Exported PNG 1: {out_png_1} (result: {res1})")
    log(f"Exported PNG 2: {out_png_2} (result: {res2})")

    ea.CloseFile()
    ea.Exit()
    log("Diagram perfectly rebuilt and exported!")

if __name__ == "__main__":
    main()
