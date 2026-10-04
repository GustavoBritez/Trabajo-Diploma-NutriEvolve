import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

xml_str = ea.SQLQuery("SELECT TOP 5 * FROM t_connector WHERE DiagramID = 12 ORDER BY SeqNo")
root = ET.fromstring(xml_str)
for row in root.findall('.//Row'):
    print("--- CONNECTOR ---")
    for elem in row:
        if elem.text:
            print(f"  {elem.tag}: {elem.text}")

ea.CloseFile()
ea.Exit()
