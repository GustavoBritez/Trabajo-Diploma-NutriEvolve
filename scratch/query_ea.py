import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
eap_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP'
ea.OpenFile(eap_path)

xml = ea.SQLQuery("SELECT Object_ID, Name, Object_Type, Stereotype, Package_ID FROM t_object WHERE Name LIKE '%Perfil%' OR Name LIKE '%Permiso%' OR Name LIKE '%Familia%' OR Name LIKE '%Consulta%'")
root = ET.fromstring(xml)
for row in root.iter('Row'):
    vals = {c.tag: c.text for c in row}
    print(vals)

print("--- Diagram 36 (Diagrama Conceptual PN1) ---")
d36 = ea.GetDiagramByID(36)
print(f"Name: {d36.Name}, Objects: {d36.DiagramObjects.Count}")
for do in d36.DiagramObjects:
    el = ea.GetElementByID(do.ElementID)
    print(f"  ID: {el.ElementID}, Name: {el.Name}, Type: {el.Type}, Stereo: {el.Stereotype}")

print("--- Diagram 37 (Diagrama Conceptual PN2) ---")
d37 = ea.GetDiagramByID(37)
print(f"Name: {d37.Name}, Objects: {d37.DiagramObjects.Count}")
for do in d37.DiagramObjects:
    el = ea.GetElementByID(do.ElementID)
    print(f"  ID: {el.ElementID}, Name: {el.Name}, Type: {el.Type}, Stereo: {el.Stereotype}")

ea.CloseFile()
ea.Exit()
