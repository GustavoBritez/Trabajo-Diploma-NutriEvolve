import win32com.client
import xml.etree.ElementTree as ET

def inspect_eap():
    ea = win32com.client.Dispatch('EA.Repository')
    eap_path = r'C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP'
    ea.OpenFile(eap_path)

    try:
        sql = '''
        SELECT o.Object_ID, o.Name, o.Object_Type, o.Package_ID, o.ParentID
        FROM t_object o
        WHERE o.Object_Type = 'UseCase' OR o.Object_Type = 'Actor'
        ORDER BY o.Package_ID, o.Object_Type, o.Name
        '''
        xml_str = ea.SQLQuery(sql)
        root = ET.fromstring(xml_str)
        print('--- Actors & Use Cases in TD.EAP ---')
        for row in root.findall('.//Row'):
            o_id = row.find('Object_ID').text
            name = row.find('Name').text
            o_type = row.find('Object_Type').text
            pkg_id = row.find('Package_ID').text
            parent_id = row.find('ParentID').text
            print(f'[{o_type}] ID {o_id}: "{name}" (Pkg: {pkg_id}, Parent: {parent_id})')

        print('\n--- Use Case Diagrams (Diagram_Type = "Use Case") ---')
        sql_uc = "SELECT Diagram_ID, Name, Package_ID, ParentID FROM t_diagram WHERE Diagram_Type = 'Use Case'"
        xml_uc = ea.SQLQuery(sql_uc)
        root_uc = ET.fromstring(xml_uc)
        for row in root_uc.findall('.//Row'):
            d_id = row.find('Diagram_ID').text
            d_name = row.find('Name').text
            pkg_id = row.find('Package_ID').text
            parent_id = row.find('ParentID').text
            print(f'Diagram {d_id}: "{d_name}" (Pkg: {pkg_id}, Parent: {parent_id})')
            diag = ea.GetDiagramByID(int(d_id))
            print(f'   -> Objects: {diag.DiagramObjects.Count}, Links: {diag.DiagramLinks.Count}')
            for i in range(diag.DiagramObjects.Count):
                do = diag.DiagramObjects.GetAt(i)
                el = ea.GetElementByID(do.ElementID)
                print(f'      Elem {el.ElementID}: {el.Name} ({el.Type})')

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == '__main__':
    inspect_eap()
