import win32com.client

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP")

pkg7 = ea.GetPackageByID(7)
diag = None
for d in pkg7.Diagrams:
    if d.Name == "Diagrama Clases Servicios General":
        diag = d
        break

print(f"=== DIAGRAM {diag.DiagramID}: {diag.Name} ===")
for i in range(diag.DiagramObjects.Count):
    dobj = diag.DiagramObjects.GetAt(i)
    el = ea.GetElementByID(dobj.ElementID)
    w = dobj.right - dobj.left
    h = dobj.top - dobj.bottom
    print(f"[{el.Name:<25}] pos=({dobj.left:>4}, {dobj.top:>5}, {dobj.right:>4}, {dobj.bottom:>5}) size=({w:>4}x{h:<4})")

ea.CloseFile()
ea.Exit()
