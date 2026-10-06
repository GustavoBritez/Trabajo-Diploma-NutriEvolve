import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

def parse_xml(xml_str):
    root = ET.fromstring(xml_str)
    rows = []
    for row in root.findall('.//Row'):
        r = {}
        for col in row:
            r[col.tag] = col.text
        rows.append(r)
    return rows

print("--- ELEMENTS WITH PN2 OR CONSULTA / PLAN / MEDICION / DIAGNOSTICO / PACIENTE ---")
sql = """
SELECT Object_ID, Name, Object_Type, Classifier_guid, Package_ID, ParentID, Stereotype 
FROM t_object 
WHERE Name LIKE '%Consulta%' 
   OR Name LIKE '%Plan%' 
   OR Name LIKE '%Medicion%' 
   OR Name LIKE '%Diagnostico%' 
   OR Name LIKE '%Nutri%' 
   OR Name LIKE '%Alerta%' 
   OR Name LIKE '%OMS%'
   OR Name LIKE '%Evaluad%'
   OR Name LIKE '%Paciente%'
ORDER BY Object_ID
"""
xml = ea.SQLQuery(sql)
for r in parse_xml(xml):
    print(r)

print("\n--- TABLES (Stereotype = 'table') ---")
sql_tables = "SELECT Object_ID, Name, Object_Type, Stereotype FROM t_object WHERE Stereotype = 'table' ORDER BY Object_ID"
for r in parse_xml(ea.SQLQuery(sql_tables)):
    print(r)

ea.CloseFile()
