import win32com.client

def inspect_pn1_general():
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    try:
        for did in [68, 69, 70, 71]:
            diag = ea.GetDiagramByID(did)
            print(f"\n=== Diagram {did}: {diag.Name} (Type: {diag.Type}) ===")
            print("Objects:")
            for o in diag.DiagramObjects:
                el = ea.GetElementByID(o.ElementID)
                print(f"  El {el.ElementID}: '{el.Name}' ({el.Type})")
            print("Links in t_diagramlinks:")
            res = ea.SQLQuery(f"SELECT ConnectorID FROM t_diagramlinks WHERE DiagramID = {did}")
            print(res)
    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    inspect_pn1_general()
