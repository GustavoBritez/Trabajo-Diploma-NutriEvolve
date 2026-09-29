import win32com.client
import xml.etree.ElementTree as ET
import json

def query_xml(ea, sql):
    xml_str = ea.SQLQuery(sql)
    root = ET.fromstring(xml_str)
    rows = []
    for row in root.findall('.//Row'):
        r = {}
        for elem in row:
            r[elem.tag] = elem.text
        rows.append(r)
    return rows

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

# 1. Inspect "Secuencia General" diagram (ID 33)
print("=== DIAGRAM 'Secuencia General' ===")
diagram_rows = query_xml(ea, "SELECT * FROM t_diagram WHERE Name = 'Secuencia General' OR Diagram_ID = 33")
print("Diagram info:", diagram_rows)

diag_objs = query_xml(ea, "SELECT do.Diagram_ID, do.Object_ID, o.Name, o.Object_Type, o.Classifier, o.Stereotype, do.RectTop, do.RectLeft, do.RectBottom, do.RectRight FROM t_diagramobjects do INNER JOIN t_object o ON do.Object_ID = o.Object_ID WHERE do.Diagram_ID = 33")
print("\nDiagram Objects (Lifelines):")
for obj in diag_objs:
    print(obj)

diag_links = query_xml(ea, "SELECT c.Connector_ID, c.Name, c.Connector_Type, c.SubType, c.PtX, c.SeqNo, c.Start_Object_ID, o1.Name as Source, c.End_Object_ID, o2.Name as Dest, c.Notes FROM (t_diagramlinks dl INNER JOIN t_connector c ON dl.ConnectorID = c.Connector_ID) LEFT JOIN t_object o1 ON c.Start_Object_ID = o1.Object_ID LEFT JOIN t_object o2 ON c.End_Object_ID = o2.Object_ID WHERE dl.DiagramID = 33 ORDER BY c.SeqNo, c.PtX")
print(f"\nDiagram Links/Messages ({len(diag_links)} found):")
for link in diag_links:
    print(link)

# 2. Inspect all connectors with Connector_Type = 'Sequence'
all_seq = query_xml(ea, "SELECT c.Connector_ID, c.DiagramID, c.Name, c.SubType, c.SeqNo, c.PtX, o1.Name as Source, o2.Name as Dest FROM t_connector c LEFT JOIN t_object o1 ON c.Start_Object_ID = o1.Object_ID LEFT JOIN t_object o2 ON c.End_Object_ID = o2.Object_ID WHERE c.Connector_Type = 'Sequence' ORDER BY c.DiagramID, c.SeqNo, c.PtX")
print(f"\nAll Sequence Messages in Project ({len(all_seq)}):")
for s in all_seq:
    if s.get('DiagramID') == '33' or s.get('DiagramID') is None or s.get('DiagramID') == '0':
        print("  ->", s)

# 3. Inspect PN1 and PN2 packages / elements / diagrams
print("\n=== PN1 & PN2 INSPECTION ===")
pn_diagrams = query_xml(ea, "SELECT Diagram_ID, Name, Diagram_Type, Package_ID FROM t_diagram WHERE Name LIKE '%PN%' OR Name LIKE '%Proceso%' OR Name LIKE '%N01%' OR Name LIKE '%N02%'")
for pnd in pn_diagrams:
    print(pnd)

pn_packages = query_xml(ea, "SELECT Package_ID, Name, Parent_ID FROM t_package WHERE Name LIKE '%PN%' OR Name LIKE '%Proceso%' OR Name LIKE '%N01%' OR Name LIKE '%N02%' OR Name LIKE '%Turno%' OR Name LIKE '%Nutri%'")
for pnp in pn_packages:
    print(pnp)

# 4. Activity elements in PN1
pn1_activities = query_xml(ea, "SELECT Object_ID, Name, Object_Type, Stereotype, Note, Package_ID FROM t_object WHERE Object_Type IN ('Activity', 'Action', 'Decision', 'StateNode', 'ActivityPartition') AND (Package_ID = 15 OR Name LIKE '%PN%')")
print(f"\nPN1 Activity Elements ({len(pn1_activities)}):")
for act in pn1_activities:
    print(act)

ea.CloseFile()
ea.Exit()
