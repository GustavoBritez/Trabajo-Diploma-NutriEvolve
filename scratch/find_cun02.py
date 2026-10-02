import win32com.client
import xml.etree.ElementTree as ET

def find_cun02():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)

    try:
        # Find UseCase for CUN02
        sql = "SELECT Object_ID, Name, Object_Type, Package_ID FROM t_object WHERE Name LIKE '*CUN02*' OR Name LIKE '*CU02*'"
        xml_str = ea.SQLQuery(sql)
        root = ET.fromstring(xml_str)
        print("=== Objects matching CUN02 / CU02 ===")
        for row in root.findall(".//Row"):
            print({elem.tag: elem.text for elem in row})

        # Find diagrams matching CUN02 / CU02
        sql_diag = "SELECT Diagram_ID, Name, Diagram_Type, ParentID, Package_ID FROM t_diagram WHERE Name LIKE '*CUN02*' OR Name LIKE '*CU02*'"
        xml_diag = ea.SQLQuery(sql_diag)
        root_diag = ET.fromstring(xml_diag)
        print("\n=== Diagrams matching CUN02 / CU02 ===")
        for row in root_diag.findall(".//Row"):
            print({elem.tag: elem.text for elem in row})

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    find_cun02()
