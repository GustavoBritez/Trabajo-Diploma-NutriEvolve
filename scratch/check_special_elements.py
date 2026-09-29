import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
xml = ea.SQLQuery("SELECT Object_ID, Name, Object_Type, Stereotype FROM t_object WHERE Object_Type IN ('Text', 'StateNode', 'MessageEndpoint')")
root = ET.fromstring(xml)
for r in root.iter('Row'):
    oid = r.find('Object_ID').text if r.find('Object_ID') is not None else ''
    ot = r.find('Object_Type').text if r.find('Object_Type') is not None else ''
    nm = r.find('Name').text if r.find('Name') is not None else ''
    st = r.find('Stereotype').text if r.find('Stereotype') is not None else ''
    print(oid, ot, nm, st)
ea.CloseFile()
ea.Exit()
