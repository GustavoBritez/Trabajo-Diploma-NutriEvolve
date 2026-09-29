import win32com.client
import xml.etree.ElementTree as ET
import json

def get_cu01_messages(eap_path):
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    
    xml_str = ea.SQLQuery("""
        SELECT c.Connector_ID, c.DiagramID, c.Name, c.Connector_Type, c.SubType, c.PtX, c.SeqNo,
               c.Start_Object_ID, o1.Name as SourceName, o1.Object_Type as SourceType,
               c.End_Object_ID, o2.Name as DestName, o2.Object_Type as DestType,
               c.Notes, c.ReturnValue
        FROM t_connector c
        LEFT JOIN t_object o1 ON c.Start_Object_ID = o1.Object_ID
        LEFT JOIN t_object o2 ON c.End_Object_ID = o2.Object_ID
        WHERE c.DiagramID = 12
        ORDER BY c.PtX, c.SeqNo, c.Connector_ID
    """)
    root = ET.fromstring(xml_str)
    rows = []
    for row in root.findall('.//Row'):
        r = {}
        for elem in row:
            r[elem.tag] = elem.text
        rows.append(r)
        
    ea.CloseFile()
    ea.Exit()
    return rows

cu01_msgs = get_cu01_messages(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
print(f"Total CU01 messages: {len(cu01_msgs)}")
for i, m in enumerate(cu01_msgs):
    print(f"{i+1}. [Seq:{m.get('SeqNo')}, SubType:{m.get('SubType')}] '{m.get('SourceName')}' -> '{m.get('DestName')}' : {m.get('Name')}")

with open(r'C:\Users\Danie\Desktop\GIT\TD\scratch\cu01_exact_messages.json', 'w', encoding='utf-8') as f:
    json.dump(cu01_msgs, f, indent=2, ensure_ascii=False)
