import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
eap_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP'
ea.OpenFile(eap_path)

print('=== DIAGRAM 5 OBJECTS ===')
d5 = ea.GetDiagramByID(5)
print(f'Diagram 5 Name: {d5.Name}, Count: {d5.DiagramObjects.Count}')
for do in d5.DiagramObjects:
    el = ea.GetElementByID(do.ElementID)
    print(f'  ID: {el.ElementID}, Name: {el.Name}, Type: {el.Type}, Stereo: {el.Stereotype}')

print('=== DIAGRAM 7 OBJECTS ===')
d7 = ea.GetDiagramByID(7)
print(f'Diagram 7 Name: {d7.Name}, Count: {d7.DiagramObjects.Count}')
for do in d7.DiagramObjects:
    el = ea.GetElementByID(do.ElementID)
    print(f'  ID: {el.ElementID}, Name: {el.Name}, Type: {el.Type}, Stereo: {el.Stereotype}')

print('=== ALL TABLE ELEMENTS IN REPOSITORY ===')
xml = ea.SQLQuery("SELECT Object_ID, Name, Stereotype, Package_ID FROM t_object WHERE Stereotype = 'table' OR Object_Type = 'Table'")
root = ET.fromstring(xml)
for row in root.iter('Row'):
    vals = {c.tag: c.text for c in row}
    print(f"Table ID: {vals.get('Object_ID')}, Name: {vals.get('Name')}, Stereo: {vals.get('Stereotype')}, Pkg: {vals.get('Package_ID')}")

print('=== ALL CLASSES IN PACKAGE 7 ===')
pkg7 = ea.GetPackageByID(7)
for el in pkg7.Elements:
    print(f"ID: {el.ElementID}, Name: {el.Name}, Type: {el.Type}, Stereo: {el.Stereotype}")

ea.CloseFile()
ea.Exit()
