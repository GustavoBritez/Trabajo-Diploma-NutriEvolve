import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
res = ea.SQLQuery("SELECT TOP 20 Element_ID, Name, Object_Type FROM t_object")
print(res)
ea.CloseFile()
