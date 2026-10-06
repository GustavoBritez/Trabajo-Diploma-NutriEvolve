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

sql = "SELECT Diagram_ID, Name, StyleEx, PDATA, ShowPackageContents, ShowForeign FROM t_diagram WHERE Diagram_ID IN (30, 93)"
for r in parse_xml(ea.SQLQuery(sql)):
    print(f"\n--- Diagram {r.get('Diagram_ID')}: {r.get('Name')} ---")
    print("StyleEx:", r.get('StyleEx'))
    print("PDATA:", r.get('PDATA'))
    print("ShowPackageContents:", r.get('ShowPackageContents'))
    print("ShowForeign:", r.get('ShowForeign'))

ea.CloseFile()
