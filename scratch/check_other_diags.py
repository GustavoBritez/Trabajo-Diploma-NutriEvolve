import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
try:
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    pkg = ea.GetPackageByID(4)
    print("Diagrams in Package 4:")
    for d in pkg.Diagrams:
        print(f"Diagram ID={d.DiagramID}, Name='{d.Name}', Type='{d.Type}'")
        for obj in d.DiagramObjects:
            el = ea.GetElementByID(obj.ElementID)
            print(f"   Obj ID={obj.ElementID}, Name='{el.Name}', Type='{el.Type}'")
finally:
    ea.CloseFile()
    ea.Exit()
