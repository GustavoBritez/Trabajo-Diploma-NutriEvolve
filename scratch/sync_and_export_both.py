import win32com.client
import os

def sync_and_export():
    ea = win32com.client.Dispatch('EA.Repository')
    try:
        ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
        proj = ea.GetProjectInterface()

        # 1. Diagram 57 -> CUN04: Diagrama de Secuencia
        diag57 = ea.GetDiagramByID(57)
        diag57.Name = "CUN04: Diagrama de Secuencia"
        diag57.Update()
        out57 = r"c:\Users\Danie\Desktop\GIT\TD\scratch\cun04_secuencia.png"
        proj.PutDiagramImageToFile(diag57.DiagramGUID, out57, 1)
        print("Diagram 57 updated and exported to", out57)

        # 2. Diagram 58 -> CUN05: Diagrama de Secuencia
        diag58 = ea.GetDiagramByID(58)
        diag58.Name = "CUN05: Diagrama de Secuencia"
        diag58.Update()
        out58 = r"c:\Users\Danie\Desktop\GIT\TD\scratch\cun05_secuencia.png"
        proj.PutDiagramImageToFile(diag58.DiagramGUID, out58, 1)
        print("Diagram 58 updated and exported to", out58)

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    sync_and_export()
