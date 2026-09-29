import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

diag = ea.GetDiagramByID(12)

# Update Fragment 232 via COM object
for d in diag.DiagramObjects:
    if d.ElementID == 232:
        d.top = -560
        d.bottom = -780
        d.left = 20
        d.right = 790
        d.Update()
    elif d.ElementID == 38:
        d.top = -650
        d.bottom = -770
        d.left = 640
        d.right = 780
        d.Update()

diag.DiagramObjects.Refresh()
diag.Update()

ea.GetProjectInterface().PutDiagramImageToFile(diag.DiagramGUID, r'C:\Users\Danie\Desktop\GIT\TD\scratch\test_box_height.png', 1)

ea.CloseFile()
ea.Exit()
print("Exported test_box_height.png")
