import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

sql = "SELECT c.Connector_ID, c.SeqNo, c.Name, c.SubType, c.PtStartY, c.Start_Object_ID, c.End_Object_ID, c.PDATA1, c.PDATA2, c.PDATA3, c.PDATA4, c.PDATA5 FROM t_connector c WHERE c.DiagramID = 12 ORDER BY c.SeqNo"
xml_str = ea.SQLQuery(sql)
root = ET.fromstring(xml_str)
rows = root.findall('.//Row')
print(f"Total messages: {len(rows)}")

with open(r'c:\Users\Danie\Desktop\GIT\TD\scratch\diag12_full_dump.txt', 'w', encoding='utf-8') as f:
    for r in rows:
        seq = r.find('SeqNo').text or ''
        cid = r.find('Connector_ID').text or ''
        name = r.find('Name').text or ''
        subtype = r.find('SubType').text or ''
        y = r.find('PtStartY').text or ''
        s_id = r.find('Start_Object_ID').text or ''
        d_id = r.find('End_Object_ID').text or ''
        pdata1 = r.find('PDATA1').text or ''
        pdata2 = r.find('PDATA2').text or ''
        pdata3 = r.find('PDATA3').text or ''
        
        src_name = ea.GetElementByID(int(s_id)).Name if s_id else '?'
        dst_name = ea.GetElementByID(int(d_id)).Name if d_id else '?'
        
        line = f"Seq {seq:>2}: [{subtype:<9}] {src_name} ({s_id}) -> {dst_name} ({d_id}) : '{name}' (Y={y}) | P1={pdata1} | P2={pdata2} | P3={pdata3}"
        print(line)
        f.write(line + '\n')

ea.CloseFile()
ea.Exit()
