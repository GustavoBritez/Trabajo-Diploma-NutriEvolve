import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

diag = ea.GetDiagramByID(12)
proj = ea.GetProjectInterface()

out_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\DSS\CUN01 Registrar Turno.png'
res = proj.PutDiagramImageToFile(diag.DiagramGUID, out_path, 1)
print("Export result:", res)

ea.CloseFile()
ea.Exit()
