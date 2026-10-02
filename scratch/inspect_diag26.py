import win32com.client
import os
import xml.etree.ElementTree as ET

def inspect_diag26():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    try:
        diag = ea.GetDiagramByID(26)
        print(f"Diag 26 Name: {diag.Name}, Type: {diag.Type}, PackageID: {diag.PackageID}, ParentID: {diag.parentID}")
        print("DiagramObjects in Diag 26:")
        for obj in diag.DiagramObjects:
            el = ea.GetElementByID(obj.ElementID)
            print(f"  El ID: {el.ElementID}, Name: {el.Name}, Type: {el.Type}, Stereotype: {el.Stereotype}")
            for a in el.Attributes:
                print(f"    Attr: {a.Name}, Type: {a.Type}, Ster: {a.Stereotype}, IsConst: {a.IsConst}")

        print("\nDiagramLinks in Diag 26:")
        for dl in diag.DiagramLinks:
            c = ea.GetConnectorByID(dl.ConnectorID)
            src = ea.GetElementByID(c.ClientID)
            dest = ea.GetElementByID(c.SupplierID)
            print(f"  Conn {c.ConnectorID}: {src.Name} -> {dest.Name} | Type: {c.Type} | SubType: {c.Subtype} | Ster: {c.Stereotype} | SrcCard: {c.ClientEnd.Cardinality} | DestCard: {c.SupplierEnd.Cardinality}")

        # Export current image
        out_png = r"c:\Users\Danie\Desktop\GIT\TD\scratch\diag26_current.png"
        proj.PutDiagramImageToFile(diag.DiagramGUID, out_png, 1)
        print("Exported current diag 26 to:", out_png)

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    inspect_diag26()
