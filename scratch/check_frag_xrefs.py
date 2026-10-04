import win32com.client
ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
el = ea.GetElementByID(734)
xml_str = ea.SQLQuery(f"SELECT Description FROM t_xref WHERE Client = '{el.ElementGUID}'")
print("Fragment 734 Xref:", xml_str)

el2 = ea.GetElementByID(728)
xml_str2 = ea.SQLQuery(f"SELECT Description FROM t_xref WHERE Client = '{el2.ElementGUID}'")
print("Fragment 728 Xref:", xml_str2)

ea.CloseFile()
ea.Exit()
