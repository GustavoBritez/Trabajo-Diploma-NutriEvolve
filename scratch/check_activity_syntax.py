import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP')

sql = '''
SELECT o.Object_ID, o.Name, o.Object_Type, o.NType, o.Stereotype
FROM t_object o
WHERE o.Package_ID = 4 AND o.Object_Type IN ('StateNode', 'Action', 'ActivityPartition', 'Decision', 'Synchronization')
'''
xml_str = ea.SQLQuery(sql)
root = ET.fromstring(xml_str)
for row in root.findall('.//Row')[:15]:
    print({child.tag: child.text for child in row})

sql_conn = '''
SELECT c.Connector_ID, c.Connector_Type, c.Name, c.Stereotype
FROM t_connector c
WHERE c.DiagramID = 27
'''
xml_c = ea.SQLQuery(sql_conn)
root_c = ET.fromstring(xml_c)
print('--- Connectors in Diagram 27 ---')
for row in root_c.findall('.//Row')[:10]:
    print({child.tag: child.text for child in row})

ea.CloseFile()
ea.Exit()
