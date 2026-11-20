import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP')

sql = "SELECT Object_ID, Name, Object_Type, NType, Stereotype, Subtype FROM t_object WHERE Object_Type IN ('StateNode', 'Decision', 'Action', 'ActivityPartition')"
root = ET.fromstring(ea.SQLQuery(sql))
print("=== Activity Elements in TD.EAP ===")
for r in root.findall('.//Row')[:15]:
    print({c.tag: c.text for c in r})

ea.CloseFile()
ea.Exit()
