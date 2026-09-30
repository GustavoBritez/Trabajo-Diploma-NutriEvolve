import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
try:
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    diag56 = ea.GetDiagramByID(56)
    print("Name:", diag56.Name, "Type:", diag56.Type)
    for obj in diag56.DiagramObjects:
        el = ea.GetElementByID(obj.ElementID)
        print(f"  Obj: ID={obj.ElementID}, Name='{el.Name}', Type='{el.Type}'")
    xml56 = ea.SQLQuery("SELECT Connector_ID, Name, SeqNo, Start_Object_ID, End_Object_ID FROM t_connector WHERE DiagramID = 56 ORDER BY SeqNo")
    print("Connectors in 56:\n", xml56)
finally:
    ea.CloseFile()
    ea.Exit()
