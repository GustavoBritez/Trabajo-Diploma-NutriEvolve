import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP')

sql = "SELECT Diagram_ID, Name, Diagram_Type, Package_ID FROM t_diagram WHERE Diagram_Type = 'Activity'"
root = ET.fromstring(ea.SQLQuery(sql))
print("=== All Activity Diagrams ===")
for r in root.findall('.//Row'):
    print({c.tag: c.text for c in r})

# Find any Activity diagram or partition
sql2 = "SELECT o.Object_ID, o.Name, o.Object_Type, o.Package_ID FROM t_object o WHERE o.Object_Type IN ('ActivityPartition', 'Action') LIMIT 10"
# In Access SQL LIMIT might not work, so just query top 10
sql2 = "SELECT TOP 10 o.Object_ID, o.Name, o.Object_Type, o.Package_ID FROM t_object o WHERE o.Object_Type IN ('ActivityPartition', 'Action')"
root2 = ET.fromstring(ea.SQLQuery(sql2))
print("=== Sample Activity Elements ===")
for r in root2.findall('.//Row'):
    print({c.tag: c.text for c in r})

ea.CloseFile()
ea.Exit()
