import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
try:
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    sql = "SELECT Object_ID, Name, Object_Type, Package_ID FROM t_object WHERE Object_ID >= 680"
    print(ea.SQLQuery(sql))
finally:
    ea.CloseFile()
    ea.Exit()
