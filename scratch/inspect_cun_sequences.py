import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

def inspect_diag(diag_id):
    d = ea.GetDiagramByID(diag_id)
    print(f'=== Diagram {diag_id}: {d.Name} ===')
    for dobj in d.DiagramObjects:
        el = ea.GetElementByID(dobj.ElementID)
        print(f'  ObjID: {dobj.InstanceID}, ElID: {el.ElementID}, Name: "{el.Name}", Type: {el.Type}, Subtype: {el.Subtype}, Left: {dobj.left}, Right: {dobj.right}, Top: {dobj.top}, Bottom: {dobj.bottom}')
    
    xml = ea.SQLQuery(f'SELECT Connector_ID, Start_Object_ID, End_Object_ID, Name, Connector_Type, Subtype, Direction, SequenceNo, PtStartX, PtStartY, PtEndX, PtEndY, PDATA4, PDATA1, PDATA2, PDATA3, PDATA5 FROM t_connector WHERE DiagramID = {diag_id} ORDER BY SequenceNo')
    root = ET.fromstring(xml)
    print('  --- Connectors ---')
    for row in root.findall('.//Row'):
        cid = row.find('Connector_ID').text
        sid = row.find('Start_Object_ID').text
        eid = row.find('End_Object_ID').text
        name = row.find('Name').text if row.find('Name') is not None else ''
        seq = row.find('SequenceNo').text if row.find('SequenceNo') is not None else ''
        pd1 = row.find('PDATA1').text if row.find('PDATA1') is not None else ''
        pd4 = row.find('PDATA4').text if row.find('PDATA4') is not None else ''
        s_el = ea.GetElementByID(int(sid))
        e_el = ea.GetElementByID(int(eid))
        print(f'    Seq {seq:<3} ({cid}): {s_el.Name} -> {e_el.Name} : "{name}" (PDATA1={pd1}, PDATA4={pd4})')

print("--- INSPECTING CUN01 (12) ---")
inspect_diag(12)
print("--- INSPECTING CUN02 (56) ---")
inspect_diag(56)
print("--- INSPECTING CUN03 (13) ---")
inspect_diag(13)

ea.CloseFile()
ea.Exit()
