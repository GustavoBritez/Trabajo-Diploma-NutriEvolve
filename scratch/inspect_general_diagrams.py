import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP")

for did in [7, 67, 68, 72, 85, 86, 120, 123, 124]:
    d = ea.GetDiagramByID(did)
    print(f"\n=== Diagram {did}: '{d.Name}' (Package {d.PackageID}) ===")
    for i in range(d.DiagramObjects.Count):
        dobj = d.DiagramObjects.GetAt(i)
        el = ea.GetElementByID(dobj.ElementID)
        print(f"  - [{el.ElementID}] {el.Name} (Type={el.Type}, Stereo={el.Stereotype}) pos=({dobj.left}, {dobj.top}, {dobj.right}, {dobj.bottom})")

ea.CloseFile()
ea.Exit()
