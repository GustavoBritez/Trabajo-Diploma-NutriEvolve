import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

xml_str = ea.SQLQuery("SELECT * FROM t_xref WHERE Client = '{C3C383D5-B27A-4f14-8A2C-BF52259EF8CF}'")
root = ET.fromstring(xml_str)
for r in root.findall('.//Row'):
    for elem in r:
        if elem.text:
            print(f"  {elem.tag}: {elem.text}")

ea.CloseFile()
ea.Exit()
