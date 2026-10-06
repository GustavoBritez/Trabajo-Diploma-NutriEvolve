import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

xml = ea.SQLQuery('SELECT * FROM t_object WHERE Object_ID IN (754, 766, 767)')
root = ET.fromstring(xml)
for row in root.findall('.//Row'):
    for child in row:
        if child.text:
            print(f'{child.tag}: {child.text}')
    print('-'*40)

# Also let's check connectors for Diagram 56 (CUN02) and Diagram 12 (CUN01)
xml_conn = ea.SQLQuery('SELECT Connector_ID, Start_Object_ID, End_Object_ID, Name, Connector_Type, Subtype, Direction, SequenceNo, PtStartX, PtStartY, PtEndX, PtEndY, PDATA4, PDATA1, PDATA2, PDATA3, PDATA5 FROM t_connector WHERE DiagramID = 56 ORDER BY SequenceNo')
root_conn = ET.fromstring(xml_conn)
print('CONNECTORS DIAGRAM 56 (CUN02):')
for row in root_conn.findall('.//Row'):
    cid = row.find('Connector_ID').text
    sid = row.find('Start_Object_ID').text
    eid = row.find('End_Object_ID').text
    name = row.find('Name').text if row.find('Name') is not None else ''
    seq = row.find('SequenceNo').text if row.find('SequenceNo') is not None else ''
    pd1 = row.find('PDATA1').text if row.find('PDATA1') is not None else ''
    pd4 = row.find('PDATA4').text if row.find('PDATA4') is not None else ''
    s_el = ea.GetElementByID(int(sid))
    e_el = ea.GetElementByID(int(eid))
    print(f'Seq {seq:<3} ({cid}): {s_el.Name} -> {e_el.Name} : "{name}" (PDATA1={pd1}, PDATA4={pd4})')

# Also check Diagram 12 (CUN01) connectors
xml_conn12 = ea.SQLQuery('SELECT Connector_ID, Start_Object_ID, End_Object_ID, Name, Connector_Type, Subtype, Direction, SequenceNo, PtStartX, PtStartY, PtEndX, PtEndY, PDATA4, PDATA1, PDATA2, PDATA3, PDATA5 FROM t_connector WHERE DiagramID = 12 ORDER BY SequenceNo')
root_conn12 = ET.fromstring(xml_conn12)
print('CONNECTORS DIAGRAM 12 (CUN01):')
for row in root_conn12.findall('.//Row'):
    cid = row.find('Connector_ID').text
    sid = row.find('Start_Object_ID').text
    eid = row.find('End_Object_ID').text
    name = row.find('Name').text if row.find('Name') is not None else ''
    seq = row.find('SequenceNo').text if row.find('SequenceNo') is not None else ''
    pd1 = row.find('PDATA1').text if row.find('PDATA1') is not None else ''
    pd4 = row.find('PDATA4').text if row.find('PDATA4') is not None else ''
    s_el = ea.GetElementByID(int(sid))
    e_el = ea.GetElementByID(int(eid))
    print(f'Seq {seq:<3} ({cid}): {s_el.Name} -> {e_el.Name} : "{name}" (PDATA1={pd1}, PDATA4={pd4})')

ea.CloseFile()
ea.Exit()
