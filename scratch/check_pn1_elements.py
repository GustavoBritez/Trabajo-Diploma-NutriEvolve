import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
eap_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP'
ea.OpenFile(eap_path)

print("=== CHECKING ELEMENTS IN PACKAGE 4 ===")
for el_id in [417, 418, 419, 420, 421, 422, 423, 424, 425, 426]:
    el = ea.GetElementByID(el_id)
    atts = [a.Name for a in el.Attributes]
    meths = [m.Name for m in el.Methods]
    print(f"\nID: {el.ElementID}, Name: {el.Name}, Type: {el.Type}, Stereo: {el.Stereotype}")
    print(f"  Attributes ({len(atts)}): {atts}")
    print(f"  Methods ({len(meths)}): {meths}")

print("\n=== CHECKING DIAGRAMS IN PACKAGE 15 (N01.1 Proceso #1: Turnero Nutricional) & PACKAGE 4 & 7 ===")
for pid in [4, 7, 15]:
    pkg = ea.GetPackageByID(pid)
    print(f"Package: {pkg.Name} (ID: {pkg.PackageID})")
    for d in pkg.Diagrams:
        print(f"  Diagram: {d.Name} (ID: {d.DiagramID}, Type: {d.Type})")

ea.CloseFile()
ea.Exit()
