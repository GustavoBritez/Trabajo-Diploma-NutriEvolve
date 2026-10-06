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

sql = "SELECT Object_ID, Name, Package_ID, ParentID, Stereotype FROM t_object WHERE Object_ID IN (429, 430, 431, 432, 433, 434, 435, 436)"
for r in parse_xml(ea.SQLQuery(sql)):
    print(r)

ea.CloseFile()
