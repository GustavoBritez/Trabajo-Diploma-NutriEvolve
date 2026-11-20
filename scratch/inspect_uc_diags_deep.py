import win32com.client
import xml.etree.ElementTree as ET

def inspect_uc_diagrams():
    ea = win32com.client.Dispatch('EA.Repository')
    eap_path = r'C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP'
    ea.OpenFile(eap_path)

    try:
        sql = "SELECT Diagram_ID, Name, Package_ID FROM t_diagram WHERE Diagram_Type = 'Use Case' OR Name LIKE '%Caso%'"
        xml_str = ea.SQLQuery(sql)
        root = ET.fromstring(xml_str)
        for row in root.findall('.//Row'):
            d_id = int(row.find('Diagram_ID').text)
            d_name = row.find('Name').text
            diag = ea.GetDiagramByID(d_id)
            print(f'=== Diagram {d_id}: "{d_name}" ===')
            for i in range(diag.DiagramObjects.Count):
                do = diag.DiagramObjects.GetAt(i)
                el = ea.GetElementByID(do.ElementID)
                print(f'   Elem {el.ElementID} [{el.Type}] "{el.Name}" - Rect: ({do.left}, {do.top}, {do.right}, {do.bottom})')
            for j in range(diag.DiagramLinks.Count):
                dl = diag.DiagramLinks.GetAt(j)
                conn = ea.GetConnectorByID(dl.ConnectorID)
                source = ea.GetElementByID(conn.ClientID)
                target = ea.GetElementByID(conn.SupplierID)
                print(f'   Link {conn.ConnectorID} [{conn.Type}] "{source.Name}" -> "{target.Name}" (Stereotype: {conn.Stereotype})')
    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == '__main__':
    inspect_uc_diagrams()
