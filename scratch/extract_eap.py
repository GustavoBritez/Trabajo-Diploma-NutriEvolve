import win32com.client
import xml.etree.ElementTree as ET
import json

def analyze_ea_project(eap_path):
    ea = win32com.client.Dispatch('EA.Repository')
    print(f"Opening {eap_path}...")
    ea.OpenFile(eap_path)
    
    result = {
        "models": [],
        "packages": [],
        "diagrams": [],
        "element_counts_by_type": {},
        "elements_summary": [],
        "connectors_summary": []
    }
    
    # Run SQL Queries for fast & comprehensive overview
    # 1. Diagram List
    xml_diagrams = ea.SQLQuery("SELECT d.Diagram_ID, d.Name, d.Diagram_Type, d.Package_ID, p.Name as PackageName, d.Notes FROM t_diagram d LEFT JOIN t_package p ON d.Package_ID = p.Package_ID ORDER BY d.Diagram_Type, d.Name")
    root = ET.fromstring(xml_diagrams)
    for row in root.findall('.//Row'):
        d_dict = {}
        for elem in row:
            d_dict[elem.tag] = elem.text
        result["diagrams"].append(d_dict)
        
    # 2. Package Hierarchy
    xml_packages = ea.SQLQuery("SELECT Package_ID, Name, Parent_ID, Notes FROM t_package ORDER BY Parent_ID, Name")
    root = ET.fromstring(xml_packages)
    for row in root.findall('.//Row'):
        p_dict = {}
        for elem in row:
            p_dict[elem.tag] = elem.text
        result["packages"].append(p_dict)
        
    # 3. Element Counts by Type
    xml_counts = ea.SQLQuery("SELECT Object_Type, COUNT(*) as Total FROM t_object GROUP BY Object_Type ORDER BY COUNT(*) DESC")
    root = ET.fromstring(xml_counts)
    for row in root.findall('.//Row'):
        obj_type = row.find('Object_Type').text if row.find('Object_Type') is not None else 'Unknown'
        total = row.find('Total').text if row.find('Total') is not None else '0'
        result["element_counts_by_type"][obj_type] = int(total)
        
    # 4. Elements Detail (UseCases, Classes, Components, Actors, Activities, Tables, etc.)
    xml_elements = ea.SQLQuery("SELECT o.Object_ID, o.Object_Type, o.Name, o.Stereotype, o.Package_ID, p.Name as PackageName, o.Note FROM t_object o LEFT JOIN t_package p ON o.Package_ID = p.Package_ID ORDER BY o.Object_Type, o.Name")
    root = ET.fromstring(xml_elements)
    for row in root.findall('.//Row'):
        e_dict = {}
        for elem in row:
            e_dict[elem.tag] = elem.text
        result["elements_summary"].append(e_dict)
        
    # 5. Connector Counts
    xml_conn_counts = ea.SQLQuery("SELECT Connector_Type, COUNT(*) as Total FROM t_connector GROUP BY Connector_Type ORDER BY COUNT(*) DESC")
    root = ET.fromstring(xml_conn_counts)
    conn_counts = {}
    for row in root.findall('.//Row'):
        c_type = row.find('Connector_Type').text if row.find('Connector_Type') is not None else 'Unknown'
        total = row.find('Total').text if row.find('Total') is not None else '0'
        conn_counts[c_type] = int(total)
    result["connector_counts"] = conn_counts

    ea.CloseFile()
    ea.Exit()
    return result

if __name__ == '__main__':
    data = analyze_ea_project(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    with open(r'C:\Users\Danie\Desktop\GIT\TD\scratch\ea_analysis.json', 'w', encoding='utf-8') as f:
        json.dump(data, f, indent=2, ensure_ascii=False)
    print(f"Total Diagrams: {len(data['diagrams'])}")
    print(f"Total Packages: {len(data['packages'])}")
    print("Element Counts:", data["element_counts_by_type"])
    print("Connector Counts:", data["connector_counts"])
