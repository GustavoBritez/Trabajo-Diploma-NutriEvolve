import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

diag = ea.GetDiagramByID(12)
print("Lifeline positions on Diagram 12:")
for do in diag.DiagramObjects:
    el = ea.GetElementByID(do.ElementID)
    if el.Type != 'InteractionFragment':
        centerX = (do.left + do.right) // 2
        print(f"ID={el.ElementID:4d} | Name='{el.Name:<28}' | Left={do.left:4d}, Right={do.right:4d}, CenterX={centerX:4d}")

ea.CloseFile()
ea.Exit()
