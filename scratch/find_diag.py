import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
try:
    xml_str = ea.SQLQuery("SELECT Diagram_ID, Name, Diagram_Type, ParentID, Package_ID FROM t_diagram WHERE ParentID = 12")
    root = ET.fromstring(xml_str)
    for row in root.findall(".//Row"):
        print({elem.tag: elem.text for elem in row})
finally:
    ea.CloseFile()
    ea.Exit()
