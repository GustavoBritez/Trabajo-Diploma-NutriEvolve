import win32com.client
import os
import sys

EAP_PATH = r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP"
EXPORT_DIR = r"C:\Users\Danie\Desktop\GIT\Diagramas\Exported"

def log(msg):
    print(f"[INFO] {msg}", flush=True)

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

def refine_diagrams():
    ea = win32com.client.Dispatch("EA.Repository")
    if not ea.OpenFile(EAP_PATH):
        print("[ERROR] Could not open TD.EAP")
        sys.exit(1)

    try:
        # 1. DER GENERAL DEL SISTEMA (Diagram 67 in Package 5)
        pkg_domain = ea.GetPackageByID(5)
        t_els = {}
        for i in range(pkg_domain.Elements.Count):
            el = pkg_domain.Elements.GetAt(i)
            if el.Stereotype == "table" and el.ElementID >= 974:
                t_els[el.Name] = el

        diag_der = ea.GetDiagramByID(67)
        der_layout = [
            # LEFT COLUMN: Bloques, Agendas, Bitacora, DV
            ("BloquesHorarios_DNI101",                40,   30,  340,  210),
            ("AgendasMedicas_DNI101",                 40,  260,  340,  430),
            ("Bitacora",                              40,  500,  340,  700),
            ("DV",                                    40,  760,  340,  900),

            # LEFT-CENTER HUB: Turnos & Usuarios
            ("Turnos_DNI101",                        430,   20,  760,  280),
            ("Usuarios",                             430,  500,  760,  840),

            # CENTER HUB: Contactos 1:N, Pacientes, Pacientes_Cambios
            ("Paciente_Telefonos_DNI101",            850,   20, 1130,  190),
            ("Paciente_Emails_DNI101",              1170,   20, 1450,  190),
            ("Pacientes_DNI101",                     860,  340, 1190,  660),
            ("Pacientes_Cambios_DNI101",             860,  710, 1190, 1020),

            # RIGHT-CENTER HUB: ConsultasNutricionales & ConsultasNutricionales_Cambios
            ("ConsultasNutricionales_DNI101",       1310,  230, 1670,  570),
            ("ConsultasNutricionales_Cambios_DNI101",1310, 620, 1670, 1030),

            # FAR-RIGHT COLUMN: PN2 Child Tables
            ("MedicionesAntropometricas_DNI101",    1770,   30, 2130,  270),
            ("DiagnosticosNutricionales_DNI101",    1770,  310, 2130,  520),
            ("PlanesAlimentarios_DNI101",           1770,  560, 2130,  820),
            ("AlertasClinicas_DNI101",              1770,  860, 2130, 1080),

            # BOTTOM BAND: Perfiles Composite hierarchy (well below Consultas_Cambios)
            ("Perfil",                               430, 1090,  670, 1210),
            ("Perfil_Familia",                       740, 1090,  990, 1210),
            ("Familia",                             1060, 1090, 1300, 1210),
            ("Familia_Familia",                     1370, 1090, 1630, 1210),
            ("Perfil_Permiso",                       430, 1270,  680, 1390),
            ("Permiso_Familia",                      740, 1270,  990, 1390),
            ("Permiso",                             1060, 1270, 1300, 1390),
            ("Permiso_Boton",                       1370, 1270, 1630, 1390),
        ]

        for t_name, l, t, r, b in der_layout:
            ensure_diagram_object(diag_der, t_els[t_name], l, t, r, b)

        diag_der.Update()
        ea.ReloadDiagram(diag_der.DiagramID)

        proj = ea.GetProjectInterface()
        out_der_png = os.path.join(EXPORT_DIR, "DER_General_Sistema_NutriEvolve.png")
        res_der = proj.PutDiagramImageToFile(diag_der.DiagramGUID, out_der_png, 1)
        log(f"Exported polished DER General PNG: {out_der_png} (Result: {res_der})")

        # 2. DIAGRAMA DE CLASES GENERAL UNIFICADO (in Package 7)
        pkg_class_model = ea.GetPackageByID(7)
        c_els = {}
        for i in range(pkg_class_model.Elements.Count):
            el = pkg_class_model.Elements.GetAt(i)
            if el.ElementID >= 999:
                c_els[el.Name] = el

        diag_gen = None
        for i in range(pkg_class_model.Diagrams.Count):
            d = pkg_class_model.Diagrams.GetAt(i)
            if d.Name == "Diagrama de Clases General":
                diag_gen = d
                break

        clases_layout = [
            # TOP-LEFT: XML Serialization & Contacts 1:N
            ("ClienteXmlItem",                        40,   40,  310,  220),
            ("ClientesXmlContainer",                 360,   40,  660,  190),
            ("PacienteTelefono_DNI101",               40,  260,  360,  460),
            ("PacienteEmail_DNI101",                  40,  500,  360,  690),

            # TOP-CENTER: Paciente_DNI101
            ("Paciente_DNI101",                      430,  240,  930,  760),

            # TOP-RIGHT: Turno_DNI101, BloqueHorario, AgendaMedica & State Pattern (1 row of 4 states)
            ("Turno_DNI101",                        1040,   40, 1520,  490),
            ("BloqueHorario_DNI101",                1600,   40, 1940,  250),
            ("AgendaMedica_DNI101",                 1600,  300, 2020,  530),
            ("IEstadoTurno_DNI101",                 2380,   60, 2730,  220),
            ("TurnoSolicitadoState_DNI101",         2070,  320, 2380,  460),
            ("TurnoConfirmadoState_DNI101",         2410,  320, 2720,  460),
            ("TurnoAsistioState_DNI101",            2750,  320, 3060,  460),
            ("TurnoCanceladoState_DNI101",          3090,  320, 3400,  460),

            # MIDDLE & BOTTOM LEFT/CENTER: Criptografía Reversible, Irreversible, Bitácora Cambios, Seguridad
            ("ServicioCriptografiaAES",               40,  760,  410,  960),
            ("PacienteCambio_DNI101",                 40, 1030,  480, 1390),
            ("Bitacora",                             530,  830,  900, 1040),
            ("DigitoVerificador",                    530, 1100,  940, 1350),
            ("ServicioBcrypt",                       530, 1430,  930, 1640),
            ("Usuario",                             1020,  580, 1420,  990),
            ("ConsultaCambio_DNI101",               1000, 1060, 1460, 1500),
            ("Perfil",                              1030, 1560, 1390, 1770),

            # RIGHT/BOTTOM-RIGHT: PN2 ConsultaNutricional + 4 Child Classes in vertical stack + Strategy OMS
            ("ConsultaNutricional_DNI101",          1560,  600, 2070, 1090),
            ("MedicionAntropometrica_DNI101",       2220,  530, 2600,  760),
            ("DiagnosticoNutricional_DNI101",       2220,  790, 2600,  990),
            ("PlanAlimentario_DNI101",              2220, 1020, 2620, 1280),
            ("AlertaClinica_DNI101",                2220, 1310, 2600, 1520),

            ("CurvasOMSData_DNI101",                1560, 1170, 1970, 1350),
            ("EvaluadorNutricionalContext_DNI101",  1560, 1410, 1970, 1590),
            ("ResultadoEvaluacionOMS",              2700, 1180, 3040, 1430),
            ("IEvaluadorNutricionalStrategy_DNI101",2680, 1490, 3060, 1640),
            ("EvaluacionIMCEdadStrategy",           2320, 1710, 2640, 1850),
            ("EvaluacionPesoEdadStrategy",          2680, 1710, 3000, 1850),
            ("EvaluacionTallaEdadStrategy",         3040, 1710, 3360, 1850),
        ]

        for c_name, l, t, r, b in clases_layout:
            ensure_diagram_object(diag_gen, c_els[c_name], l, t, r, b)

        diag_gen.Update()
        ea.ReloadDiagram(diag_gen.DiagramID)

        out_cls_png = os.path.join(EXPORT_DIR, "Diagrama_Clases_General_Unificado_NutriEvolve.png")
        res_cls = proj.PutDiagramImageToFile(diag_gen.DiagramGUID, out_cls_png, 1)
        log(f"Exported polished Clases General PNG: {out_cls_png} (Result: {res_cls})")

    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    refine_diagrams()
