import win32com.client
import xml.etree.ElementTree as ET

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

# Diagram 28: N01.1: Diagrama de Proceso Macro (PN1)
print("=== DIAGRAM 28 (PN1 Macro Process) ===")
diag_objs = query_xml(ea, "SELECT do.Diagram_ID, do.Object_ID, o.Name, o.Object_Type, o.Stereotype, do.RectTop, do.RectLeft, do.RectBottom, do.RectRight FROM t_diagramobjects do INNER JOIN t_object o ON do.Object_ID = o.Object_ID WHERE do.Diagram_ID = 28 ORDER BY do.RectTop DESC")
for o in diag_objs:
    print(f"[{o.get('Object_Type')}] {o.get('Name')} (Stereotype: {o.get('Stereotype')}) | Top: {o.get('RectTop')}, Left: {o.get('RectLeft')}")

print("\n--- CONTROL FLOWS IN DIAGRAM 28 ---")
flows = query_xml(ea, "SELECT c.Connector_ID, c.Name, c.Connector_Type, o1.Name as Source, o2.Name as Dest FROM (t_diagramlinks dl INNER JOIN t_connector c ON dl.ConnectorID = c.Connector_ID) LEFT JOIN t_object o1 ON c.Start_Object_ID = o1.Object_ID LEFT JOIN t_object o2 ON c.End_Object_ID = o2.Object_ID WHERE dl.DiagramID = 28")
for f in flows:
    print(f"Flow: {f.get('Source')}  --->  [{f.get('Name') or ''}]  --->  {f.get('Dest')}")

ea.CloseFile()
ea.Exit()
