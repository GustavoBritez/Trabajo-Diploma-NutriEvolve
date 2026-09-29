import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
xml = ea.SQLQuery('SELECT Object_ID, RectLeft, RectRight, RectTop, RectBottom FROM t_diagramobjects WHERE Diagram_ID = 12')
root = ET.fromstring(xml)
for r in root.iter('Row'):
    oid = r.find('Object_ID').text or ''
    el = ea.GetElementByID(int(oid))
    print(f"{oid} ({el.Name}): Left={r.find('RectLeft').text}, Right={r.find('RectRight').text}, Top={r.find('RectTop').text}, Bottom={r.find('RectBottom').text}")
ea.CloseFile()
ea.Exit()
