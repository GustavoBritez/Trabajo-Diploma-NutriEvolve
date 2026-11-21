import win32com.client

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP")

pkg7 = ea.GetPackageByID(7)

target_names = [
    "Diagrama Clases Servicios BE",
    "Diagrama Clases Servicios BLL",
    "Diagrama Clases Servicios DER"
]

for dname in target_names:
    diag = None
    for d in pkg7.Diagrams:
        if d.Name == dname:
            diag = d
            break
    if not diag:
        print(f"NOT FOUND: {dname}")
        continue
    print(f"\n=======================================================")
    print(f"DIAGRAM: {diag.Name} (ID: {diag.DiagramID}, Type: {diag.Type})")
    print(f"=======================================================")
    for i in range(diag.DiagramObjects.Count):
        dobj = diag.DiagramObjects.GetAt(i)
        el = ea.GetElementByID(dobj.ElementID)
        w = dobj.right - dobj.left
        h = dobj.top - dobj.bottom
        print(f"  [{el.Name:<28}] pos=({dobj.left:>4}, {dobj.top:>4}, {dobj.right:>4}, {dobj.bottom:>4}) size=({w:>4}x{h:<4})")

ea.CloseFile()
ea.Exit()
