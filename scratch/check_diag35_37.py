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

print("--- DIAGRAM 35 OBJECTS ---")
sql35 = "SELECT do.Diagram_ID, o.Object_ID, o.Name, o.Object_Type FROM t_diagramobjects do INNER JOIN t_object o ON do.Object_ID = o.Object_ID WHERE do.Diagram_ID = 35"
for r in parse_xml(ea.SQLQuery(sql35)):
    print(r)

print("\n--- DIAGRAM 37 OBJECTS ---")
sql37 = "SELECT do.Diagram_ID, o.Object_ID, o.Name, o.Object_Type FROM t_diagramobjects do INNER JOIN t_object o ON do.Object_ID = o.Object_ID WHERE do.Diagram_ID = 37"
for r in parse_xml(ea.SQLQuery(sql37)):
    print(r)

ea.CloseFile()
