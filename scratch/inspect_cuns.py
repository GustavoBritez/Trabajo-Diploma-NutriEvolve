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

print("--- ALL USE CASES ---")
ucs = q("SELECT Object_ID, Name, Package_ID, ParentID FROM t_object WHERE Object_Type = 'UseCase'")
for u in ucs:
    print(f"UseCase ID: {u.get('Object_ID')}, Name: {u.get('Name')}, Pkg: {u.get('Package_ID')}, Parent: {u.get('ParentID')}")

print("\n--- ALL DIAGRAMS ---")
diags = q("SELECT Diagram_ID, Name, Diagram_Type, Package_ID, ParentID FROM t_diagram")
for d in diags:
    print(f"Diag ID: {d.get('Diagram_ID')}, Name: {d.get('Name')}, Type: {d.get('Diagram_Type')}, Pkg: {d.get('Package_ID')}, Parent: {d.get('ParentID')}")

print("\n--- ELEMENTS UNDER USE CASES ---")
children = q("SELECT Object_ID, Name, Object_Type, ParentID FROM t_object WHERE ParentID IN (SELECT Object_ID FROM t_object WHERE Object_Type = 'UseCase')")
for c in children:
    print(f"Child ID: {c.get('Object_ID')}, Name: {c.get('Name')}, Type: {c.get('Object_Type')}, Parent: {c.get('ParentID')}")
