import win32com.client
import os

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
try:
    diag = ea.GetDiagramByID(25)
    proj = ea.GetProjectInterface()
    out_png = r"c:\Users\Danie\Desktop\GIT\TD\scratch\diag25_current.png"
    proj.PutDiagramImageToFile(diag.DiagramGUID, out_png, 1)
    print("Exported diag 25 to", out_png)
finally:
    ea.CloseFile()
    ea.Exit()
