import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
try:
    sql = "SELECT Connector_ID, Connector_Type, SubType, Stereotype, SourceIsAggregate, DestIsAggregate, Direction, SourceCard, DestCard, SourceRole, DestRole FROM t_connector WHERE Connector_ID IN (876, 877, 880, 881, 882, 887)"
    xml_str = ea.SQLQuery(sql)
    root = ET.fromstring(xml_str)
    for row in root.findall(".//Row"):
        print({elem.tag: elem.text for elem in row})
finally:
    ea.CloseFile()
    ea.Exit()
