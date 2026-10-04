import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

el = ea.GetElementByID(732)
print("Fragment 732 properties:")
print("Name:", el.Name)
print("Type:", el.Type)
print("Subtype:", el.Subtype)
print("Stereotype:", el.Stereotype)
print("MiscData:", [el.MiscData(i) for i in range(5)])

xml_str = ea.SQLQuery("SELECT * FROM t_object WHERE Object_ID = 732")
root = ET.fromstring(xml_str)
for r in root.findall('.//Row'):
    for elem in r:
        if elem.text:
            print(f"  {elem.tag}: {elem.text}")

ea.CloseFile()
ea.Exit()
