import win32com.client
import xml.etree.ElementTree as ET
import json
import os

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

def full_deep_eap_analysis(eap_path):
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    
    analysis = {}
    
    # 1. Packages
    analysis['packages'] = query_xml(ea, "SELECT Package_ID, Name, Parent_ID, Notes FROM t_package ORDER BY Package_ID")
    
    # 2. Diagrams with elements and links
    diagrams = query_xml(ea, "SELECT Diagram_ID, Name, Diagram_Type, Package_ID, Notes, Author, Version, CreatedDate, ModifiedDate FROM t_diagram ORDER BY Diagram_Type, Name")
    for d in diagrams:
        d_id = d.get('Diagram_ID')
        # Diagram objects
        d_objs = query_xml(ea, f"SELECT do.Diagram_ID, do.Object_ID, o.Name, o.Object_Type, o.Stereotype, do.RectTop, do.RectLeft, do.RectBottom, do.RectRight FROM t_diagramobjects do INNER JOIN t_object o ON do.Object_ID = o.Object_ID WHERE do.Diagram_ID = {d_id}")
        d['objects'] = d_objs
        
        # Diagram links / connectors
        d_links = query_xml(ea, f"SELECT dl.DiagramID, dl.ConnectorID, c.Name as ConnectorName, c.Connector_Type, c.SourceRole, c.DestRole, o1.Name as SourceObj, o2.Name as DestObj FROM (t_diagramlinks dl INNER JOIN t_connector c ON dl.ConnectorID = c.Connector_ID) LEFT JOIN t_object o1 ON c.Start_Object_ID = o1.Object_ID LEFT JOIN t_object o2 ON c.End_Object_ID = o2.Object_ID WHERE dl.DiagramID = {d_id}")
        d['links'] = d_links
    analysis['diagrams'] = diagrams
    
    # 3. Use Cases
    use_cases = query_xml(ea, "SELECT Object_ID, Name, Note, Status, Phase, Complexity FROM t_object WHERE Object_Type = 'UseCase' ORDER BY Name")
    analysis['use_cases'] = use_cases
    
    # 4. Actors
    actors = query_xml(ea, "SELECT Object_ID, Name, Note FROM t_object WHERE Object_Type = 'Actor' ORDER BY Name")
    analysis['actors'] = actors
    
    # 5. Classes with attributes and operations
    classes = query_xml(ea, "SELECT Object_ID, Name, Stereotype, Note, Package_ID FROM t_object WHERE Object_Type IN ('Class', 'Interface') ORDER BY Name")
    for c in classes:
        c_id = c.get('Object_ID')
        attrs = query_xml(ea, f"SELECT Name, Type, Scope, Notes FROM t_attribute WHERE Object_ID = {c_id} ORDER BY Pos, Name")
        ops = query_xml(ea, f"SELECT OperationID, Name, Type, Scope, Notes FROM t_operation WHERE Object_ID = {c_id} ORDER BY Pos, Name")
        for op in ops:
            op_id = op.get('OperationID')
            params = query_xml(ea, f"SELECT Name, Type, Kind, Pos FROM t_operationparams WHERE OperationID = {op_id} ORDER BY Pos")
            op['params'] = params
        c['attributes'] = attrs
        c['operations'] = ops
    analysis['classes'] = classes
    
    # 6. Sequence Messages (DSS)
    seq_messages = query_xml(ea, "SELECT c.Connector_ID, c.DiagramID, c.Name as MessageName, c.Connector_Type, c.PtX as SeqNum, o1.Name as SourceName, o2.Name as DestName, c.Notes FROM t_connector c LEFT JOIN t_object o1 ON c.Start_Object_ID = o1.Object_ID LEFT JOIN t_object o2 ON c.End_Object_ID = o2.Object_ID WHERE c.Connector_Type = 'Sequence' ORDER BY c.DiagramID, c.PtX, c.SeqNo")
    analysis['sequence_messages'] = seq_messages
    
    # 7. Activities and Actions
    activities = query_xml(ea, "SELECT Object_ID, Name, Object_Type, Stereotype, Note, ParentID FROM t_object WHERE Object_Type IN ('Activity', 'Action', 'Decision', 'StateNode', 'ActivityPartition') ORDER BY ParentID, Object_Type, Name")
    analysis['activity_elements'] = activities
    
    # 8. All Connectors / Relationships
    connectors = query_xml(ea, "SELECT c.Connector_ID, c.Name, c.Connector_Type, c.Stereotype, o1.Name as SourceName, o1.Object_Type as SourceType, o2.Name as DestName, o2.Object_Type as DestType, c.Notes FROM t_connector c LEFT JOIN t_object o1 ON c.Start_Object_ID = o1.Object_ID LEFT JOIN t_object o2 ON c.End_Object_ID = o2.Object_ID ORDER BY c.Connector_Type, c.Name")
    analysis['all_connectors'] = connectors
    
    ea.CloseFile()
    ea.Exit()
    return analysis

if __name__ == '__main__':
    res = full_deep_eap_analysis(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    with open(r'C:\Users\Danie\Desktop\GIT\TD\scratch\eap_detailed_analysis.json', 'w', encoding='utf-8') as f:
        json.dump(res, f, indent=2, ensure_ascii=False)
    print("Detailed analysis saved successfully!")
    print(f"Total Use Cases: {len(res['use_cases'])}")
    print(f"Total Actors: {len(res['actors'])}")
    print(f"Total Classes: {len(res['classes'])}")
    print(f"Total Sequence Messages: {len(res['sequence_messages'])}")
