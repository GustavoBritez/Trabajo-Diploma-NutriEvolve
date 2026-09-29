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

print("--- DIAGRAM 12 (CUN01: Registrar Turno) ---")
d12 = q("SELECT Diagram_ID, Name, Diagram_Type, ParentID FROM t_diagram WHERE Diagram_ID = 12")
print(d12)

print("\n--- CONNECTORS IN DIAGRAM 12 ---")
c12 = q("SELECT Connector_ID, Name, Connector_Type, SubType, Start_Object_ID, End_Object_ID, SeqNo FROM t_connector WHERE DiagramID = 12 ORDER BY SeqNo")
for c in c12:
    print(c)

print("\n--- DIAGRAM 27 (CU01: Diagrama de Actividad) ---")
d27 = q("SELECT Diagram_ID, Name, Diagram_Type, ParentID FROM t_diagram WHERE Diagram_ID = 27")
print(d27)
