import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

def print_messages(diag_id, title):
    print(f"\n==================================================")
    print(f"DIAGRAM {diag_id}: {title}")
    print(f"==================================================")
    d = ea.GetDiagramByID(diag_id)
    print("Lifelines in diagram:")
    for dobj in d.DiagramObjects:
        el = ea.GetElementByID(dobj.ElementID)
        print(f"  ElID: {el.ElementID}, Name: '{el.Name}', Type: '{el.Type}', Stereo: '{el.Stereotype}'")

    sql = f"""
        SELECT Connector_ID, Name, SeqNo, PtStartY, Start_Object_ID, End_Object_ID, PDATA1, PDATA4
        FROM t_connector
        WHERE DiagramID = {diag_id}
        ORDER BY SeqNo
    """
    xml = ea.SQLQuery(sql)
    root = ET.fromstring(xml)
    print("Messages:")
    for row in root.findall('.//Row'):
        seq = row.find('SeqNo').text if row.find('SeqNo') is not None else ''
        name = row.find('Name').text if row.find('Name') is not None else ''
        y = row.find('PtStartY').text if row.find('PtStartY') is not None else ''
        pd1 = row.find('PDATA1').text if row.find('PDATA1') is not None else ''
        pd4 = row.find('PDATA4').text if row.find('PDATA4') is not None else ''
        sid = int(row.find('Start_Object_ID').text)
        eid = int(row.find('End_Object_ID').text)
        s_el = ea.GetElementByID(sid)
        e_el = ea.GetElementByID(eid)
        print(f"  Seq {seq:>2} (y={y}): {s_el.Name} -> {e_el.Name} : '{name}' [PDATA1={pd1}, PDATA4={pd4}]")

print_messages(57, "CUN04: Diagrama de Secuencia")
print_messages(58, "CUN05: Diagrama de Secuencia")

ea.CloseFile()
ea.Exit()
