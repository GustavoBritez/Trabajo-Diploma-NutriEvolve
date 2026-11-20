import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP')

sql = "SELECT Diagram_ID, Name, Diagram_Type, Package_ID FROM t_diagram WHERE Name LIKE '%Actividad%'"
root = ET.fromstring(ea.SQLQuery(sql))
print("=== Activity Diagrams matching 'Actividad' ===")
for r in root.findall('.//Row'):
    print({c.tag: c.text for c in r})

# Also check t_diagramobjects for diagram 27 vs all diagram IDs
sql2 = "SELECT TOP 5 Diagram_ID, Object_ID, RectTop, RectLeft, RectBottom, RectRight FROM t_diagramobjects"
root2 = ET.fromstring(ea.SQLQuery(sql2))
print("=== Sample DiagramObjects ===")
for r in root2.findall('.//Row'):
    print({c.tag: c.text for c in r})

ea.CloseFile()
ea.Exit()
