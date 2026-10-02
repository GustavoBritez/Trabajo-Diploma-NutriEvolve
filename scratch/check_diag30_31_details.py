import win32com.client

def check_diag30_31_details():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)

    try:
        for did in [30, 31]:
            diag = ea.GetDiagramByID(did)
            print(f"Diag {did}: Name='{diag.Name}', Type='{diag.Type}', GUID='{diag.DiagramGUID}', ParentID={diag.parentID}, PackageID={diag.PackageID}")
    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    check_diag30_31_details()
