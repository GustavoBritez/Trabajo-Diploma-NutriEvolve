import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
try:
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    xml = ea.SQLQuery("SELECT Connector_ID, Name, Direction, SubType, StyleEx, SourceCard, DestCard FROM t_connector WHERE Connector_ID = 2031")
    print(xml)
finally:
    ea.CloseFile()
    ea.Exit()
