import win32com.client
import xml.etree.ElementTree as ET

def inspect_bitacora_details():
    ea = win32com.client.Dispatch('EA.Repository')
    eap_path = r'C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP'
    ea.OpenFile(eap_path)

    try:
        for did in [119, 120, 121]:
            diag = ea.GetDiagramByID(did)
            print(f'==============================')
            print(f'DIAGRAM {did}: "{diag.Name}" (Type: {diag.Type})')
            print(f'==============================')
            print(f'Objects ({diag.DiagramObjects.Count}):')
            for i in range(diag.DiagramObjects.Count):
                dobj = diag.DiagramObjects.GetAt(i)
                el = ea.GetElementByID(dobj.ElementID)
                print(f'  - [{el.ElementID}] {el.Name} ({el.Type}, Stereo: {el.Stereotype}) at ({dobj.left}, {dobj.top}, {dobj.right}, {dobj.bottom})')
            
            print(f'Links / Messages ({diag.DiagramLinks.Count}):')
            if diag.Type == 'Sequence':
                # Query sequence messages
                sql_msg = f'''
                SELECT Connector_ID, Name, ActionTag, SequenceNo, SourceCard, DestCard
                FROM t_connector
                WHERE DiagramID = {did}
                ORDER BY SequenceNo
                '''
                xml_msg = ea.SQLQuery(sql_msg)
                root = ET.fromstring(xml_msg)
                for row in root.findall('.//Row'):
                    cid = row.find('Connector_ID').text
                    name = row.find('Name').text
                    seq = row.find('SequenceNo').text
                    print(f'    Msg #{seq} [ID {cid}]: {name}')
            else:
                for j in range(diag.DiagramLinks.Count):
                    dl = diag.DiagramLinks.GetAt(j)
                    conn = ea.GetConnectorByID(dl.ConnectorID)
                    src = ea.GetElementByID(conn.ClientID)
                    tgt = ea.GetElementByID(conn.SupplierID)
                    print(f'    Link [{conn.Type}] {src.Name} -> {tgt.Name} ({conn.Name})')
    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == '__main__':
    inspect_bitacora_details()
