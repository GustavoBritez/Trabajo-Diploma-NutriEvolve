import win32com.client

def inspect():
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    try:
        for diag_id in [56, 30, 31]:
            diag = ea.GetDiagramByID(diag_id)
            print(f"\n================ Diagram {diag_id}: {diag.Name} (Type: {diag.Type}, ParentID: {diag.parentID}) ================")
            print("--- DiagramObjects ---")
            for obj in diag.DiagramObjects:
                el = ea.GetElementByID(obj.ElementID)
                print(f"  ElID: {el.ElementID}, Name: '{el.Name}', Type: '{el.Type}', Subtype: {el.Subtype}, Pos: (l={obj.left}, r={obj.right}, t={obj.top}, b={obj.bottom})")
    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    inspect()
