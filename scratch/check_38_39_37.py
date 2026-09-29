import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

def q(sql):
    xml_str = ea.SQLQuery(sql)
    root = ET.fromstring(xml_str)
    rows = []
    for row in root.findall('.//Row'):
        r = {elem.tag: elem.text for elem in row}
        rows.append(r)
    return rows

print("--- OBJECTS UNDER 38, 39, 37 ---")
rows = q("SELECT Object_ID, Name, Object_Type, ParentID FROM t_object WHERE ParentID IN (38, 39, 37)")
for r in rows:
    print(r)

print("\n--- DIAGRAMS UNDER 38, 39, 37 ---")
diags = q("SELECT Diagram_ID, Name, Diagram_Type, ParentID FROM t_diagram WHERE ParentID IN (38, 39, 37)")
for d in diags:
    print(d)
