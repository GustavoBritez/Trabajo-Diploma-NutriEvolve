import win32com.client
import xml.etree.ElementTree as ET

def check_usecases_diagrams():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)

    try:
        sql = """
        SELECT d.Diagram_ID, d.Name, d.Diagram_Type, d.ParentID, o.Name as ParentName
        FROM t_diagram d
        LEFT JOIN t_object o ON d.ParentID = o.Object_ID
        WHERE d.Package_ID = 4 OR d.ParentID IN (12, 38, 13, 39, 37)
        """
        xml_str = ea.SQLQuery(sql)
        root = ET.fromstring(xml_str)
        print("=== Diagrams in Package 4 / Use Cases ===")
        for row in root.findall(".//Row"):
            print({elem.tag: elem.text for elem in row})

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    check_usecases_diagrams()
