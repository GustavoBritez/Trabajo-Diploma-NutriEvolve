import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
try:
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    xml = ea.SQLQuery("SELECT Connector_ID, Name, SubType, Start_Object_ID, End_Object_ID, SeqNo, PtStartX, PtStartY, PtEndX, PtEndY FROM t_connector WHERE DiagramID = 57 AND SeqNo >= 23 ORDER BY SeqNo")
    print(xml)
finally:
    ea.CloseFile()
    ea.Exit()
