import win32com.client
import xml.etree.ElementTree as ET
import json

def get_eap_messages(eap_path):
    ea = win32com.client.Dispatch('EA.Repository')
    print(f"Opening {eap_path}...")
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
        
    diagrams = ea.SQLQuery("SELECT Diagram_ID, Name, Diagram_Type FROM t_diagram")
    root_d = ET.fromstring(diagrams)
    diag_list = []
    for row in root_d.findall('.//Row'):
        r = {}
        for elem in row:
            r[elem.tag] = elem.text
        diag_list.append(r)
        
    ea.CloseFile()
    ea.Exit()
    return {"messages": rows, "diagrams": diag_list}

dss_data = get_eap_messages(r'C:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\dssDeCampo.EAP')
monit_data = get_eap_messages(r'C:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\Monitoreo Nutricional.EAP')

with open(r'C:\Users\Danie\Desktop\GIT\TD\scratch\dss_and_monit_dump.json', 'w', encoding='utf-8') as f:
    json.dump({"dss": dss_data, "monit": monit_data}, f, indent=2, ensure_ascii=False)

print(f"DSS messages: {len(dss_data['messages'])} | Monit messages: {len(monit_data['messages'])}")
print("\n--- DSS DIAGRAMS ---")
for d in dss_data['diagrams']:
    print(d)

print("\n--- MONIT DIAGRAMS ---")
for d in monit_data['diagrams']:
    print(d)
