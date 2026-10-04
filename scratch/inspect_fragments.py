import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

diag = ea.GetDiagramByID(12)
print("Diagram:", diag.Name)
for do in diag.DiagramObjects:
    el = ea.GetElementByID(do.ElementID)
    print(f"ID={el.ElementID} | Name='{el.Name}' | Type={el.Type} | Rect=({do.left}, {do.right}, {do.top}, {do.bottom})")

ea.CloseFile()
ea.Exit()
