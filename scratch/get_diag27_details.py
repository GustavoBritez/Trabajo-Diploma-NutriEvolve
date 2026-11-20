import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP')

sql = '''
SELECT do.Diagram_ID, do.Object_ID, o.Name, o.Object_Type, o.Stereotype, do.RectTop, do.RectLeft, do.RectBottom, do.RectRight
FROM t_diagramobjects do
JOIN t_object o ON do.Object_ID = o.Object_ID
WHERE do.Diagram_ID = 27
'''
root = ET.fromstring(ea.SQLQuery(sql))
print("=== Diagram 27 Objects ===")
for r in root.findall('.//Row'):
    print({c.tag: c.text for c in r})

sql_conn = '''
SELECT c.Connector_ID, c.Connector_Type, c.Name, o1.Name as Src, o2.Name as Dst
FROM t_connector c
JOIN t_object o1 ON c.Start_Object_ID = o1.Object_ID
JOIN t_object o2 ON c.End_Object_ID = o2.Object_ID
WHERE c.DiagramID = 27
'''
root_c = ET.fromstring(ea.SQLQuery(sql_conn))
print("=== Diagram 27 Connectors ===")
for r in root_c.findall('.//Row'):
    print({c.tag: c.text for c in r})

ea.CloseFile()
ea.Exit()
