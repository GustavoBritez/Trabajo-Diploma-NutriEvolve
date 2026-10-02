import win32com.client

def inspect_diag30_31():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)

    try:
        for diag_id in [30, 31]:
            diag = ea.GetDiagramByID(diag_id)
            print(f"\n=== Diagram {diag_id}: {diag.Name} (Type: {diag.Type}, ParentID: {diag.parentID}) ===")
            print("DiagramObjects:")
            for obj in diag.DiagramObjects:
                el = ea.GetElementByID(obj.ElementID)
                print(f"  El ID: {el.ElementID}, Name: {el.Name}, Type: {el.Type}, Ster: {el.Stereotype}")
            print("DiagramLinks:")
            for dl in diag.DiagramLinks:
                c = ea.GetConnectorByID(dl.ConnectorID)
                src = ea.GetElementByID(c.ClientID)
                dest = ea.GetElementByID(c.SupplierID)
                print(f"  Conn {c.ConnectorID}: {src.Name} -> {dest.Name} | Type: {c.Type}")

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    inspect_diag30_31()
