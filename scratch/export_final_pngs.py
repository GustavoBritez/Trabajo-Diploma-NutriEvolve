import win32com.client
import os

EXPORT_DIR = r"C:\Users\Danie\Desktop\GIT\Diagramas\Exported"
SCRATCH_DIR = r"C:\Users\Danie\Desktop\GIT\scratch"
EAP_PATH = r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP"

diagrams = [
    (125, 'Diagrama_de_Caso_de_uso_Bitacora_de_Cambios'),
    (121, 'Diagrama_Secuencia_Bitacora_de_Cambios'),
    (119, 'Diagrama_Clases_Bitacora_de_Cambios'),
    (120, 'Diagrama_Base_de_datos_Bitacora_de_Cambios'),
    (126, 'Diagrama_de_Caso_de_uso_Serializacion'),
    (128, 'Diagrama_Secuencia_Serializacion'),
    (127, 'Diagrama_Clases_Serializacion'),
    (129, 'Diagrama_de_Caso_de_uso_Deserializacion'),
    (131, 'Diagrama_Secuencia_Deserializacion'),
    (130, 'Diagrama_Clases_Deserializacion')
]

def export_all():
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(EAP_PATH)
    proj = ea.GetProjectInterface()
    os.makedirs(EXPORT_DIR, exist_ok=True)
    os.makedirs(SCRATCH_DIR, exist_ok=True)

    for did, clean_name in diagrams:
        diag = ea.GetDiagramByID(did)
        f_exp = os.path.join(EXPORT_DIR, f"{clean_name}.png")
        f_scr = os.path.join(SCRATCH_DIR, f"{clean_name}.png")
        proj.PutDiagramImageToFile(diag.DiagramGUID, f_exp, 1)
        proj.PutDiagramImageToFile(diag.DiagramGUID, f_scr, 1)
        print(f"Exported [ID {did}] '{diag.Name}' -> {f_exp} ({os.path.getsize(f_exp)} bytes)")

    ea.CloseFile()
    ea.Exit()

if __name__ == '__main__':
    export_all()
