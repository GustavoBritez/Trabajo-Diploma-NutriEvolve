import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
eap_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP'
ea.OpenFile(eap_path)

xml = ea.SQLQuery("SELECT Object_ID, Name, Object_Type, Stereotype, Package_ID FROM t_object WHERE Object_Type IN ('Class', 'Interface', 'Table') ORDER BY Object_Type, Name")
root = ET.fromstring(xml)
print("=== ALL CLASSES / INTERFACES / TABLES IN TD.EAP ===")
for row in root.iter('Row'):
    vals = {c.tag: c.text for c in row}
    print(f"ID: {vals.get('Object_ID')}, Name: {vals.get('Name')}, Type: {vals.get('Object_Type')}, Stereo: {vals.get('Stereotype')}, Pkg: {vals.get('Package_ID')}")

ea.CloseFile()
ea.Exit()
