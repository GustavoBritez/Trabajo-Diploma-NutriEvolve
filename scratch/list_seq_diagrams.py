import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
xml = ea.SQLQuery("SELECT Diagram_ID, Name, Diagram_Type FROM t_diagram WHERE Diagram_Type = 'Sequence'")
root = ET.fromstring(xml)
for r in root.iter('Row'):
    did = r.find('Diagram_ID').text if r.find('Diagram_ID') is not None else ''
    name = r.find('Name').text if r.find('Name') is not None else ''
    print(did, name)
ea.CloseFile()
