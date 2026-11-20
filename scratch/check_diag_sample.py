import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP')

sql = "SELECT Diagram_ID, RectTop, RectBottom, RectLeft, RectRight FROM t_diagramobjects WHERE Object_ID = 443"
root = ET.fromstring(ea.SQLQuery(sql))
print("=== Object 443 in diagrams ===")
for r in root.findall('.//Row'):
    print({c.tag: c.text for c in r})

# Get diagram name
if root.findall('.//Row'):
    diag_id = root.findall('.//Row')[0].find('Diagram_ID').text
    sql2 = f"SELECT Diagram_ID, Name, Diagram_Type FROM t_diagram WHERE Diagram_ID = {diag_id}"
    root2 = ET.fromstring(ea.SQLQuery(sql2))
    for r in root2.findall('.//Row'):
        print("Diagram info:", {c.tag: c.text for c in r})

    # Let's inspect all objects and connectors in this diagram!
    sql_objs = f"""
    SELECT do.Object_ID, o.Name, o.Object_Type, o.Stereotype, do.RectTop, do.RectBottom, do.RectLeft, do.RectRight
    FROM t_diagramobjects do
    JOIN t_object o ON do.Object_ID = o.Object_ID
    WHERE do.Diagram_ID = {diag_id}
    ORDER BY do.RectTop DESC
    """
    root_o = ET.fromstring(ea.SQLQuery(sql_objs))
    print(f"=== Objects in diagram {diag_id} ===")
    for r in root_o.findall('.//Row'):
        print({c.tag: c.text for c in r})

    sql_c = f"""
    SELECT c.Connector_ID, c.Connector_Type, c.Name, o1.Name as Src, o2.Name as Dst
    FROM t_connector c
    JOIN t_object o1 ON c.Start_Object_ID = o1.Object_ID
    JOIN t_object o2 ON c.End_Object_ID = o2.Object_ID
    WHERE c.Start_Object_ID IN (SELECT Object_ID FROM t_diagramobjects WHERE Diagram_ID = {diag_id})
    """
    root_c = ET.fromstring(ea.SQLQuery(sql_c))
    print(f"=== Connectors in diagram {diag_id} ===")
    for r in root_c.findall('.//Row')[:15]:
        print({c.tag: c.text for c in r})

ea.CloseFile()
ea.Exit()
