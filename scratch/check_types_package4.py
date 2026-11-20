import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP')

sql = "SELECT DISTINCT Object_Type FROM t_object"
root = ET.fromstring(ea.SQLQuery(sql))
print("=== Distinct Object Types in TD.EAP ===")
for r in root.findall('.//Row'):
    print({c.tag: c.text for c in r})

# Check objects in Package 4
sql2 = "SELECT Object_ID, Name, Object_Type, Stereotype FROM t_object WHERE Package_ID = 4"
root2 = ET.fromstring(ea.SQLQuery(sql2))
print("=== Package 4 Objects ===")
for r in root2.findall('.//Row')[:15]:
    print({c.tag: c.text for c in r})

ea.CloseFile()
ea.Exit()
