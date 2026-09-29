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

print("--- DIAGRAMS FOR CUN01 to CUN05 ---")
sql = """
SELECT d.Diagram_ID, d.Name, d.Diagram_Type, d.ParentID, o.Name as ParentName
FROM t_diagram d
LEFT JOIN t_object o ON d.ParentID = o.Object_ID
WHERE d.ParentID IN (12, 38, 13, 39, 37) OR d.Diagram_ID IN (12, 13, 27, 32)
"""
for row in q(sql):
    print(row)

print("\n--- OBJECTS INSIDE CUN01 (ID 12) ---")
sql_c1 = "SELECT Object_ID, Name, Object_Type, Classifier_guid FROM t_object WHERE ParentID = 12"
for row in q(sql_c1):
    print(row)

print("\n--- OBJECTS INSIDE CUN03 (ID 13) ---")
sql_c3 = "SELECT Object_ID, Name, Object_Type, Classifier_guid FROM t_object WHERE ParentID = 13"
for row in q(sql_c3):
    print(row)
