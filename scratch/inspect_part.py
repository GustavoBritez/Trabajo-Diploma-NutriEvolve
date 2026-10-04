import win32com.client
ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
xml_str = ea.SQLQuery("SELECT Description FROM t_xref WHERE Client = '{C3C383D5-B27A-4f14-8A2C-BF52259EF8CF}'")
print(xml_str)
ea.CloseFile()
ea.Exit()
