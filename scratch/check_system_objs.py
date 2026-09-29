import win32com.client
import xml.etree.ElementTree as ET

def query_xml(ea, sql):
    xml_str = ea.SQLQuery(sql)
    root = ET.fromstring(xml_str)
    rows = []
    for row in root.findall('.//Row'):
        r = {}
        for elem in row:
            r[elem.tag] = elem.text
        rows.append(r)
    return rows

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

# Look for System boundary or objects
systems = query_xml(ea, "SELECT Object_ID, Name, Object_Type, Stereotype, Package_ID FROM t_object WHERE Name LIKE '%Sistema%' OR Name LIKE '%Nutri%' OR Object_Type IN ('Boundary', 'GUIElement', 'Actor')")
print("Objects matching System/Actor/Boundary:")
for s in systems:
    print(s)

ea.CloseFile()
ea.Exit()
