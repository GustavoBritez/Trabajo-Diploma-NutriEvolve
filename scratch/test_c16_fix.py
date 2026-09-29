import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

ea.Execute("UPDATE t_connector SET PtStartX = 230, PtStartY = -760, PtEndX = 710, PtEndY = -760, StyleEx = NULL WHERE Connector_ID = 1314")

diag = ea.GetDiagramByID(12)
diag.Update()
ea.GetProjectInterface().PutDiagramImageToFile(diag.DiagramGUID, r'C:\Users\Danie\Desktop\GIT\TD\scratch\test_c16.png', 1)

ea.CloseFile()
ea.Exit()
print("Exported test_c16.png")
