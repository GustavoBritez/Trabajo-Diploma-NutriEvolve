import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

def parse_xml(xml_str):
    root = ET.fromstring(xml_str)
    rows = []
    for row in root.findall('.//Row'):
        r = {}
        for col in row:
            r[col.tag] = col.text
        rows.append(r)
    return rows

sql = "SELECT DISTINCT Connector_Type, SubType, Stereotype FROM t_connector"
for r in parse_xml(ea.SQLQuery(sql)):
    print(r)

ea.CloseFile()
