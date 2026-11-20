import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP')

sql = "SELECT Object_ID, Name, Object_Type, NType, Stereotype, Package_ID FROM t_object WHERE Object_Type = 'StateNode'"
root = ET.fromstring(ea.SQLQuery(sql))
print("=== StateNodes in TD.EAP ===")
for r in root.findall('.//Row'):
    print({c.tag: c.text for c in r})

sql_action = "SELECT TOP 5 Object_ID, Name, Object_Type, NType, Stereotype, Package_ID FROM t_object WHERE Object_Type = 'Action'"
root_a = ET.fromstring(ea.SQLQuery(sql_action))
print("=== Sample Actions in TD.EAP ===")
for r in root_a.findall('.//Row'):
    print({c.tag: c.text for c in r})

sql_dec = "SELECT TOP 5 Object_ID, Name, Object_Type, NType, Stereotype, Package_ID FROM t_object WHERE Object_Type = 'Decision'"
root_d = ET.fromstring(ea.SQLQuery(sql_dec))
print("=== Sample Decisions in TD.EAP ===")
for r in root_d.findall('.//Row'):
    print({c.tag: c.text for c in r})

ea.CloseFile()
ea.Exit()
