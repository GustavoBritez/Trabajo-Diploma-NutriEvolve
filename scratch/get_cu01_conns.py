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

obj_ids = [219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230]
obj_ids_str = ",".join(str(x) for x in obj_ids)

conns = query_xml(ea, f"""
    SELECT c.Connector_ID, c.Name, c.Connector_Type, c.SubType, c.PtX, c.PtY, c.SeqNo, c.DiagramID,
           c.Start_Object_ID, o1.Name as SourceName, 
           c.End_Object_ID, o2.Name as DestName,
           c.Notes, c.ReturnValue
    FROM t_connector c
    LEFT JOIN t_object o1 ON c.Start_Object_ID = o1.Object_ID
    LEFT JOIN t_object o2 ON c.End_Object_ID = o2.Object_ID
    WHERE c.Start_Object_ID IN ({obj_ids_str}) AND c.End_Object_ID IN ({obj_ids_str})
    ORDER BY c.PtX, c.SeqNo, c.Connector_ID
""")

print(f"Found {len(conns)} connectors between CU01 lifelines:")
for i, c in enumerate(conns):
    print(f"{i+1}. [ID:{c.get('Connector_ID')}, SeqNo:{c.get('SeqNo')}, PtX:{c.get('PtX')}, SubType:{c.get('SubType')}] '{c.get('SourceName')}' -> '{c.get('DestName')}' : '{c.get('Name')}'")

with open(r'C:\Users\Danie\Desktop\GIT\TD\scratch\cu01_messages_full.json', 'w', encoding='utf-8') as f:
    json.dump(conns, f, indent=2, ensure_ascii=False)

ea.CloseFile()
ea.Exit()
