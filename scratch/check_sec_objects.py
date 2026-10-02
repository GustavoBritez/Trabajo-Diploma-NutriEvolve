import win32com.client
import xml.etree.ElementTree as ET

def check_security_classes():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)

    try:
        sql = "SELECT Object_ID, Name, Object_Type, Package_ID FROM t_object WHERE Name LIKE '*Evento*' OR Name LIKE '*Digito*'"
        xml_str = ea.SQLQuery(sql)
        root = ET.fromstring(xml_str)
        print("=== Objects matching Evento / Digito ===")
        for row in root.findall(".//Row"):
            print({elem.tag: elem.text for elem in row})

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    check_security_classes()
