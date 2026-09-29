import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
xml = ea.SQLQuery("SELECT * FROM t_connector WHERE Connector_ID = 1314")
root = ET.fromstring(xml)
for row in root.iter('Row'):
    for child in row:
        if child.text:
            print(f'{child.tag}: {child.text}')
ea.CloseFile()
ea.Exit()
