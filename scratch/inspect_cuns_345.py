import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

def dump_diag_summary(diag_id):
    d = ea.GetDiagramByID(diag_id)
    print(f'=== Diagram {diag_id}: {d.Name} ===')
    for dobj in d.DiagramObjects:
        el = ea.GetElementByID(dobj.ElementID)
        print(f'  ElID: {el.ElementID}, Name: "{el.Name}", Type: {el.Type}, Subtype: {el.Subtype}, Stereotype: "{el.Stereotype}"')
    
    xml_conn = ea.SQLQuery(f'SELECT c.Connector_ID, c.DiagramID, c.Name, c.Connector_Type, c.SubType, c.PtX, c.SeqNo, c.Start_Object_ID, o1.Name as SourceName, c.End_Object_ID, o2.Name as DestName, c.PDATA4 FROM t_connector c LEFT JOIN t_object o1 ON c.Start_Object_ID = o1.Object_ID LEFT JOIN t_object o2 ON c.End_Object_ID = o2.Object_ID WHERE c.DiagramID = {diag_id} ORDER BY c.SeqNo, c.Connector_ID')
    root_conn = ET.fromstring(xml_conn)
    for row in root_conn.findall('.//Row'):
        cid = row.find('Connector_ID').text
        seq = row.find('SeqNo').text if row.find('SeqNo') is not None else ''
        sname = row.find('SourceName').text if row.find('SourceName') is not None else ''
        dname = row.find('DestName').text if row.find('DestName') is not None else ''
        name = row.find('Name').text if row.find('Name') is not None else ''
        pd4 = row.find('PDATA4').text if row.find('PDATA4') is not None else ''
        print(f'    Seq {seq:<3}: {sname} -> {dname} : "{name}" (PDATA4={pd4})')

dump_diag_summary(57) # CUN04
dump_diag_summary(58) # CUN05
dump_diag_summary(13) # CUN03

ea.CloseFile()
ea.Exit()
