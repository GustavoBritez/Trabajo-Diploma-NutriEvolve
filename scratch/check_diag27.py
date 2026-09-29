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

print("--- ELEMENTS IN DIAGRAM 27 ---")
dobjs = q("SELECT do.Diagram_ID, do.Object_ID, o.Name, o.Object_Type FROM t_diagramobjects do INNER JOIN t_object o ON do.Object_ID = o.Object_ID WHERE do.Diagram_ID = 27")
for d in dobjs:
    print(d)

print("\n--- CONNECTORS IN DIAGRAM 27 ---")
conns = q("SELECT c.Connector_ID, c.Name, c.Connector_Type, c.Start_Object_ID, c.End_Object_ID FROM t_connector c WHERE c.DiagramID = 27")
for c in conns:
    print(c)
