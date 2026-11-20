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

    # Find the diagram
    diag = None
    for i in range(pkg.Diagrams.Count):
        d = pkg.Diagrams.GetAt(i)
        if d.Name == diag_name:
            diag = d
            break

    if not diag:
        log("Diagram not found!")
        sys.exit(1)

    # 1. Delete orphan element 'Fin_Bitacora' (ID 966) if exists
    for i in range(pkg.Elements.Count - 1, -1, -1):
        el = pkg.Elements.GetAt(i)
        if el.Name == "Fin_Bitacora":
            log(f"Deleting orphan element: {el.Name} (ID: {el.ElementID})")
            pkg.Elements.Delete(i)
            pkg.Elements.Refresh()
            break

    # Also make sure no orphan diagram objects remain in diagram
    for j in range(diag.DiagramObjects.Count - 1, -1, -1):
        dobj = diag.DiagramObjects.GetAt(j)
        try:
            el = ea.GetElementByID(dobj.ElementID)
            if el.Name == "Fin_Bitacora":
                diag.DiagramObjects.Delete(j)
                diag.DiagramObjects.Refresh()
        except:
            pass

    # Helper to find element by name
    def find_element(name):
        for i in range(pkg.Elements.Count):
            el = pkg.Elements.GetAt(i)
            if el.Name == name:
                return el
        return None

    p_admin = find_element("Administrador / Usuario")
    p_sys = find_element("Sistema NutriEvolve")
    n_start = find_element("Inicio_Bitacora")
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
    n_end_user = find_element("Fin_Consulta")
    n_end_sys = find_element("Fin_Restauracion")

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

    # Adjust layout
    # Swimlanes
    set_diagram_object(p_admin, 40, 440, -10, -970, seq=1)
    set_diagram_object(p_sys, 460, 880, -10, -970, seq=1)

    # Administrador lane
    set_diagram_object(n_start, 228, 252, -40, -64)
    set_diagram_object(a_open, 120, 360, -95, -145)
    set_diagram_object(a_filter, 100, 380, -185, -240)
    set_diagram_object(a_search, 130, 350, -270, -315)
    set_diagram_object(d_revert, 160, 320, -355, -425)

    # Branches from d_revert
    set_diagram_object(a_exit, 60, 220, -475, -530)
    set_diagram_object(n_end_user, 128, 152, -585, -609)

    set_diagram_object(a_select_rev, 250, 430, -475, -535)
    set_diagram_object(d_confirm, 270, 410, -580, -645)

    # Sistema NutriEvolve lane
    set_diagram_object(a_load, 530, 810, -95, -145)
    set_diagram_object(a_show, 530, 810, -270, -320)
    set_diagram_object(a_ask_confirm, 530, 810, -475, -530)
    set_diagram_object(a_restore_data, 520, 820, -670, -725)
    set_diagram_object(a_audit_dv, 510, 830, -755, -815)
    set_diagram_object(a_notify, 530, 810, -845, -900)
    set_diagram_object(n_end_sys, 658, 682, -925, -949)

    diag.DiagramObjects.Refresh()
    diag.Update()

    # Reconnect d_confirm cleanly:
    # Remove old connectors on d_confirm
    for k in range(d_confirm.Connectors.Count - 1, -1, -1):
        d_confirm.Connectors.Delete(k)
    d_confirm.Connectors.Refresh()

    # Remove old connectors on a_exit
    for k in range(a_exit.Connectors.Count - 1, -1, -1):
        a_exit.Connectors.Delete(k)
    a_exit.Connectors.Refresh()

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

    add_flow(a_exit, n_end_user)
    add_flow(d_confirm, n_end_user, "[No - Cancelar]")
    add_flow(d_confirm, a_restore_data, "[Sí - Confirmar]")

    diag.Update()
    ea.ReloadDiagram(diag.DiagramID)

    # Export diagram image
    project = ea.GetProjectInterface()
    out_png_1 = os.path.join(EXPORT_DIR, "Diagrama_de_Actividad_Bitacora_de_Cambios.png")
    out_png_2 = os.path.join(SCRATCH_DIR, "Diagrama_de_Actividad_Bitacora_de_Cambios.png")

    res1 = project.PutDiagramImageToFile(diag.DiagramGUID, out_png_1, 1)
    res2 = project.PutDiagramImageToFile(diag.DiagramGUID, out_png_2, 1)
    log(f"Exported PNG 1: {out_png_1} (result: {res1})")
    log(f"Exported PNG 2: {out_png_2} (result: {res2})")

    ea.CloseFile()
    ea.Exit()
    log("Cleaned and re-exported successfully!")

if __name__ == "__main__":
    main()
