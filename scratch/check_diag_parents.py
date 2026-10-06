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

print("--- DIAGRAMS UNDER USE CASES (ParentID > 0) ---")
sql = "SELECT Diagram_ID, Name, Diagram_Type, Package_ID, ParentID FROM t_diagram WHERE ParentID > 0 ORDER BY Diagram_ID"
for r in parse_xml(ea.SQLQuery(sql)):
    print(r)

print("\n--- ACTORS IN USE CASE MODEL ---")
sql_act = "SELECT Object_ID, Name, Object_Type, Stereotype FROM t_object WHERE Object_Type = 'Actor' ORDER BY Object_ID"
for r in parse_xml(ea.SQLQuery(sql_act)):
    print(r)

ea.CloseFile()
