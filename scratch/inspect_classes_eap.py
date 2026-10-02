import win32com.client

def inspect_eap():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    try:
        print("=== Searching for all Classes in TD.EAP ===")
        # Query t_object for classes
        xml_res = ea.SQLQuery("SELECT Object_ID, Name, Object_Type, Package_ID FROM t_object WHERE Object_Type = 'Class'")
        print(xml_res[:2000])

        print("\n=== Diagram 25 details ===")
        diag = ea.GetDiagramByID(25)
        print(f"Diag 25 Name: {diag.Name}, Type: {diag.Type}, PackageID: {diag.PackageID}, ParentID: {diag.parentID}")
        print("DiagramObjects in Diag 25:")
        for obj in diag.DiagramObjects:
            el = ea.GetElementByID(obj.ElementID)
            print(f"  El ID: {el.ElementID}, Name: {el.Name}, Type: {el.Type}")

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    inspect_eap()
