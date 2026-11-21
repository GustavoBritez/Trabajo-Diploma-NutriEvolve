import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP")

sql = """
SELECT Object_ID, Object_Type, Name, Stereotype, Package_ID 
FROM t_object 
WHERE Name LIKE '%Usuario%' 
   OR Name LIKE '%Bitacora%' 
   OR Name LIKE '%Digito%' 
   OR Name LIKE '%DV%' 
   OR Name LIKE '%Idioma%' 
   OR Name LIKE '%Backup%' 
   OR Name LIKE '%Perfil%' 
   OR Name LIKE '%Familia%' 
   OR Name LIKE '%Patente%' 
   OR Name LIKE '%Permiso%' 
   OR Name LIKE '%Bcrypt%' 
   OR Name LIKE '%Cripto%' 
   OR Name LIKE '%Session%'
ORDER BY Package_ID, Object_Type, Name
"""
res = ea.SQLQuery(sql)
ea.CloseFile()

root = ET.fromstring(res)
for row in root.findall(".//Row"):
    oid = row.find("Object_ID").text if row.find("Object_ID") is not None else ""
    otype = row.find("Object_Type").text if row.find("Object_Type") is not None else ""
    name = row.find("Name").text if row.find("Name").text is not None else ""
    stereo = row.find("Stereotype").text if row.find("Stereotype") is not None else ""
    pkg = row.find("Package_ID").text if row.find("Package_ID") is not None else ""
    print(f"ID={oid:<5} PKG={pkg:<4} TYPE={otype:<12} STEREO={stereo:<12} NAME={name}")
