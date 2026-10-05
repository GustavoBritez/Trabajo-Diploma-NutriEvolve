import win32com.client

def inspect_all():
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    try:
        diag_ids = [13, 60, 61, 57, 62, 63, 58, 64, 65, 68]
        for did in diag_ids:
            diag = ea.GetDiagramByID(did)
            print(f"\n================ Diagram {did}: {diag.Name} (Type: {diag.Type}, Package: {diag.PackageID}, Parent: {diag.parentID}) ================")
            print("--- Objects ---")
            for obj in diag.DiagramObjects:
                el = ea.GetElementByID(obj.ElementID)
                print(f"  ElID: {el.ElementID}, Name: '{el.Name}', Type: '{el.Type}', Pos: (l={obj.left}, r={obj.right}, t={obj.top}, b={obj.bottom})")
    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    inspect_all()
