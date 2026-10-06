import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

sql = "SELECT Diagram_ID, Package_ID, ParentID, Name, Diagram_Type FROM t_diagram WHERE ParentID IN (42, 46, 674, 675) ORDER BY ParentID, Diagram_ID"
xml = ea.SQLQuery(sql)
root = ET.fromstring(xml)
print("Diagrams under PN2 Use Cases (42, 46, 674, 675):")
for row in root.findall('.//Row'):
    d_id = row.find('Diagram_ID').text
    p_id = row.find('Package_ID').text
    par_id = row.find('ParentID').text
    name = row.find('Name').text
    dtype = row.find('Diagram_Type').text
    print(f"Parent: {par_id:<4} ID: {d_id:<4} Type: {dtype:<10} Name: {name}")

ea.CloseFile()
ea.Exit()
