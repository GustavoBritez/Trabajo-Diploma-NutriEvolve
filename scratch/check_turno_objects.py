import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
try:
    sql = "SELECT Object_ID, Name, Object_Type, Package_ID FROM t_object WHERE Name LIKE '*Turno*'"
    xml_str = ea.SQLQuery(sql)
    root = ET.fromstring(xml_str)
    for row in root.findall(".//Row"):
        print({elem.tag: elem.text for elem in row})
finally:
    ea.CloseFile()
    ea.Exit()
