import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
try:
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    diag = ea.GetDiagramByID(58)
    print("Diagram 58 Name:", diag.Name)
    print("Diagram 58 Objects:")
    for o in diag.DiagramObjects:
        el = ea.GetElementByID(o.ElementID)
        print(f"InstanceID={o.InstanceID}, ElementID={o.ElementID}, Name='{el.Name}', Type='{el.Type}'")
finally:
    ea.CloseFile()
    ea.Exit()
