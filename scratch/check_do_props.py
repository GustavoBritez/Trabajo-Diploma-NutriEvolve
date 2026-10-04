import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

diag = ea.GetDiagramByID(12)
for do in diag.DiagramObjects:
    el = ea.GetElementByID(do.ElementID)
    if el.ElementID in [38, 225, 754, 222]:
        print(f"ID={el.ElementID}, Name={el.Name}, Left={do.left}, Right={do.right}, Top={do.top}, Bottom={do.bottom}, Sequence={do.Sequence}")

ea.CloseFile()
ea.Exit()
