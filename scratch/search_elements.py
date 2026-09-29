import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
try:
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    sql = "SELECT Element_ID, Name, Element_Type, Package_ID FROM t_object WHERE Name LIKE '%DAL%' OR Name LIKE '%Session%' OR Name LIKE '%Bitacora%'"
    print(ea.SQLQuery(sql))
finally:
    ea.CloseFile()
    ea.Exit()
