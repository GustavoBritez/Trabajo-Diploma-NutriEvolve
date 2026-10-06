import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

sql = "SELECT Diagram_ID, Package_ID, ParentID, Name, Diagram_Type FROM t_diagram WHERE Diagram_Type = 'Sequence' ORDER BY Diagram_ID"
xml = ea.SQLQuery(sql)
root = ET.fromstring(xml)
print("Sequence Diagrams in TD.EAP:")
for row in root.findall('.//Row'):
    d_id = row.find('Diagram_ID').text
    p_id = row.find('Package_ID').text
    par_id = row.find('ParentID').text if row.find('ParentID') is not None else '0'
    name = row.find('Name').text
    print(f"ID: {d_id:<4} Pkg: {p_id:<4} Parent: {par_id:<4} Name: {name}")

ea.CloseFile()
ea.Exit()
