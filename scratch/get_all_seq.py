import win32com.client
import xml.etree.ElementTree as ET
import json

def get_all_sequence_messages(eap_path):
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    
    xml_str = ea.SQLQuery("""
        SELECT c.Connector_ID, c.DiagramID, c.Name, c.Connector_Type, c.SubType, c.PtX, c.SeqNo,
               c.Start_Object_ID, o1.Name as SourceName, o1.Object_Type as SourceType,
               c.End_Object_ID, o2.Name as DestName, o2.Object_Type as DestType,
               c.Notes
        FROM t_connector c
        LEFT JOIN t_object o1 ON c.Start_Object_ID = o1.Object_ID
        LEFT JOIN t_object o2 ON c.End_Object_ID = o2.Object_ID
        WHERE c.Connector_Type = 'Sequence'
        ORDER BY c.DiagramID, c.SeqNo, c.PtX, c.Connector_ID
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

conns_eap = get_all_sequence_messages(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
conns_bak = get_all_sequence_messages(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP.bak')

print(f"Total Sequence in TD.EAP: {len(conns_eap)}")
print(f"Total Sequence in TD.EAP.bak: {len(conns_bak)}")

with open(r'C:\Users\Danie\Desktop\GIT\TD\scratch\cu01_all_seq_messages.json', 'w', encoding='utf-8') as f:
    json.dump({"eap": conns_eap, "bak": conns_bak}, f, indent=2, ensure_ascii=False)

print("\n--- FIRST 35 MESSAGES IN BAK ---")
for i, m in enumerate(conns_bak[:35]):
    print(f"{i+1}. [Diag:{m.get('DiagramID')}, Seq:{m.get('SeqNo')}, SubType:{m.get('SubType')}] '{m.get('SourceName')}' -> '{m.get('DestName')}' : {m.get('Name')}")
