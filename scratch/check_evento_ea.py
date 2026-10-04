import win32com.client

repo = win32com.client.Dispatch('EA.Repository')
repo.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

sql = "SELECT Object_ID, Object_Type, Name FROM t_object WHERE Name LIKE '%Bitacora%'"
xml = repo.SQLQuery(sql)
print('t_object with Bitacora:\n', xml)

repo.CloseFile()
repo.Exit()
