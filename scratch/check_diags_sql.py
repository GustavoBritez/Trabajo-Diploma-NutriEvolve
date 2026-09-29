import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
try:
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    xml = ea.SQLQuery("SELECT Diagram_ID, Name, Diagram_Type, Package_ID, ParentID FROM t_diagram WHERE Diagram_ID >= 50")
    print(xml)
finally:
    ea.CloseFile()
    ea.Exit()
