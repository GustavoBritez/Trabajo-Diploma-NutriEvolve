import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP")

sql_pkgs = "SELECT Package_ID, Name, Parent_ID FROM t_package ORDER BY Package_ID"
root_p = ET.fromstring(ea.SQLQuery(sql_pkgs))
print("=== All Packages in TD.EAP ===")
for r in root_p.findall(".//Row"):
    print({c.tag: c.text for c in r})

sql_diags = "SELECT Diagram_ID, Name, Diagram_Type, Package_ID FROM t_diagram ORDER BY Package_ID, Diagram_ID"
root_d = ET.fromstring(ea.SQLQuery(sql_diags))
print("=== All Diagrams in TD.EAP ===")
for r in root_d.findall(".//Row"):
    print({c.tag: c.text for c in r})

ea.CloseFile()
ea.Exit()
