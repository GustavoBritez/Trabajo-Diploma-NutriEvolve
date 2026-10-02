import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
eap_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP'
ea.OpenFile(eap_path)

pkg7 = ea.GetPackageByID(7)
print("=== PACKAGE 7 CLASSES ATTRIBUTES & METHODS ===")
for el in pkg7.Elements:
    if el.Type in ('Class', 'Interface'):
        atts = [a.Name for a in el.Attributes]
        meths = [m.Name for m in el.Methods]
        print(f"[{el.Type}] {el.Name} (ID: {el.ElementID})")
        print(f"  Attributes ({len(atts)}): {', '.join(atts[:10])}")
        print(f"  Methods ({len(meths)}): {', '.join(meths[:10])}")

ea.CloseFile()
ea.Exit()
