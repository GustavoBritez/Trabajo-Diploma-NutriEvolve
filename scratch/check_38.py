import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
xml = ea.SQLQuery("SELECT Connector_ID, Name, DiagramID, Start_Object_ID, End_Object_ID, SeqNo, SubType, PtStartY FROM t_connector WHERE Start_Object_ID = 38 OR End_Object_ID = 38")
root = ET.fromstring(xml)
for r in root.iter('Row'):
    cid = r.find('Connector_ID').text if r.find('Connector_ID') is not None else ''
    name = r.find('Name').text if r.find('Name') is not None else ''
    did = r.find('DiagramID').text if r.find('DiagramID') is not None else ''
    so = r.find('Start_Object_ID').text if r.find('Start_Object_ID') is not None else ''
    eo = r.find('End_Object_ID').text if r.find('End_Object_ID') is not None else ''
    seq = r.find('SeqNo').text if r.find('SeqNo') is not None else ''
    st = r.find('SubType').text if r.find('SubType') is not None else ''
    py = r.find('PtStartY').text if r.find('PtStartY') is not None else ''
    print(f'ID:{cid}, Diag:{did}, Start:{so}, End:{eo}, Seq:{seq}, Sub:{st}, Y:{py}, Name:{name}')
ea.CloseFile()
