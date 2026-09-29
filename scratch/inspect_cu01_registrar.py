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

# Diagram 12 details
diagram = query_xml(ea, "SELECT * FROM t_diagram WHERE Diagram_ID = 12")[0]
print("=== DIAGRAM 12 DETAILS ===")
print("Name:", diagram.get('Name'))
print("Type:", diagram.get('Diagram_Type'))
print("Package_ID:", diagram.get('Package_ID'))
print("ParentID:", diagram.get('ParentID'))
print("Notes:", diagram.get('Notes'))

# Diagram objects (Lifelines)
print("\n=== LIFELINES (Diagram Objects) ===")
objs = query_xml(ea, """
    SELECT do.Diagram_ID, do.Object_ID, o.Name, o.Object_Type, o.Classifier, o.Stereotype, do.RectTop, do.RectLeft, do.RectBottom, do.RectRight, c.Name as ClassifierName
    FROM (t_diagramobjects do INNER JOIN t_object o ON do.Object_ID = o.Object_ID)
    LEFT JOIN t_object c ON o.Classifier = c.Object_ID
    WHERE do.Diagram_ID = 12
    ORDER BY do.RectLeft ASC
""")
for o in objs:
    print(f"Lifeline: '{o.get('Name')}' | Type: {o.get('Object_Type')} | Classifier: {o.get('ClassifierName')} (ID: {o.get('Classifier')}) | Stereotype: {o.get('Stereotype')} | Left: {o.get('RectLeft')}, Right: {o.get('RectRight')}")

# Diagram connectors / messages
print("\n=== SEQUENCE MESSAGES ===")
messages = query_xml(ea, """
    SELECT c.Connector_ID, c.Name, c.Connector_Type, c.SubType, c.PtX, c.PtY, c.SeqNo, 
           o1.Name as SourceName, o1.Object_Type as SourceType, 
           o2.Name as DestName, o2.Object_Type as DestType,
           c.Notes, c.ReturnValue, c.IsReturn
    FROM (t_diagramlinks dl INNER JOIN t_connector c ON dl.ConnectorID = c.Connector_ID)
    LEFT JOIN t_object o1 ON c.Start_Object_ID = o1.Object_ID
    LEFT JOIN t_object o2 ON c.End_Object_ID = o2.Object_ID
    WHERE dl.DiagramID = 12
    ORDER BY c.PtX, c.SeqNo, c.Connector_ID
""")
if not messages:
    messages = query_xml(ea, """
        SELECT c.Connector_ID, c.Name, c.Connector_Type, c.SubType, c.PtX, c.PtY, c.SeqNo, 
               o1.Name as SourceName, o1.Object_Type as SourceType, 
               o2.Name as DestName, o2.Object_Type as DestType,
               c.Notes, c.ReturnValue
        FROM t_connector c
        LEFT JOIN t_object o1 ON c.Start_Object_ID = o1.Object_ID
        LEFT JOIN t_object o2 ON c.End_Object_ID = o2.Object_ID
        WHERE c.DiagramID = 12 AND c.Connector_Type = 'Sequence'
        ORDER BY c.PtX, c.SeqNo, c.Connector_ID
    """)

for i, m in enumerate(messages):
    print(f"{i+1}. [SeqNo: {m.get('SeqNo')}, PtX: {m.get('PtX')}] '{m.get('SourceName')}' -> '{m.get('DestName')}' : {m.get('Name')} (SubType: {m.get('SubType')})")

with open(r'C:\Users\Danie\Desktop\GIT\TD\scratch\cu01_diagram_dump.json', 'w', encoding='utf-8') as f:
    json.dump({"diagram": diagram, "lifelines": objs, "messages": messages}, f, indent=2, ensure_ascii=False)

ea.CloseFile()
ea.Exit()
