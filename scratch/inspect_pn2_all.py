import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

def parse_xml(xml_str):
    root = ET.fromstring(xml_str)
    rows = []
    for row in root.findall('.//Row'):
        r = {}
        for col in row:
            r[col.tag] = col.text
        rows.append(r)
    return rows

print('--- PACKAGES ---')
xml_pkg = ea.SQLQuery('SELECT Package_ID, Name, Parent_ID FROM t_package ORDER BY Package_ID')
for r in parse_xml(xml_pkg):
    print(r)

print('\n--- ALL DIAGRAMS ---')
xml_diag = ea.SQLQuery('SELECT Diagram_ID, Name, Diagram_Type, Package_ID, ParentID FROM t_diagram ORDER BY Diagram_ID')
for r in parse_xml(xml_diag):
    print(r)

print('\n--- USE CASES ---')
xml_uc = ea.SQLQuery("SELECT Object_ID, Name, Object_Type, Package_ID, ParentID FROM t_object WHERE Object_Type = 'UseCase' ORDER BY Object_ID")
for r in parse_xml(xml_uc):
    print(r)

ea.CloseFile()
