import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

sql = "SELECT Diagram_ID, Package_ID, Name, Diagram_Type FROM t_diagram ORDER BY Diagram_ID"
xml = ea.SQLQuery(sql)
root = ET.fromstring(xml)

for row in root.findall('.//Row'):
    d_id = row.find('Diagram_ID').text if row.find('Diagram_ID') is not None else ''
    pkg_id = row.find('Package_ID').text if row.find('Package_ID') is not None else ''
    name = row.find('Name').text if row.find('Name') is not None else ''
    d_type = row.find('Diagram_Type').text if row.find('Diagram_Type') is not None else ''
    print(f"ID: {d_id:<4} Pkg: {pkg_id:<4} Type: {d_type:<12} Name: {name}")

ea.CloseFile()
ea.Exit()
