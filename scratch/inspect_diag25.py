import win32com.client

def inspect_diag25():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    try:
        diag = ea.GetDiagramByID(25)
        print(f"Diag 25: {diag.Name}, GUID: {diag.DiagramGUID}")
        print("Diagram Links in 25:")
        for dl in diag.DiagramLinks:
            c = ea.GetConnectorByID(dl.ConnectorID)
            src = ea.GetElementByID(c.ClientID)
            dest = ea.GetElementByID(c.SupplierID)
            print(f"  Conn {c.ConnectorID}: {src.Name} -> {dest.Name} | Type: {c.Type} | SubType: {c.Subtype} | Ster: {c.Stereotype}")
            
        print("\nPackages in Model:")
        for model in ea.Models:
            print(f"Model: {model.Name} (ID: {model.PackageID})")
            for pkg in model.Packages:
                print(f"  Pkg: {pkg.Name} (ID: {pkg.PackageID})")
                for subpkg in pkg.Packages:
                    print(f"    SubPkg: {subpkg.Name} (ID: {subpkg.PackageID})")

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    inspect_diag25()
