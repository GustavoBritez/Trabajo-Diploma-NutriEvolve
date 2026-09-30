import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
try:
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    
    print("--- DIAGRAM 56 (ParentID 38: CUN02) ---")
    diag56 = ea.GetDiagramByID(56)
    print("Name:", diag56.Name, "Type:", diag56.Type)
    for obj in diag56.DiagramObjects:
        el = ea.GetElementByID(obj.ElementID)
        print(f"  Obj: ID={obj.ElementID}, Name='{el.Name}', Type='{el.Type}'")
    xml56 = ea.SQLQuery("SELECT Connector_ID, Name, SeqNo, Start_Object_ID, End_Object_ID FROM t_connector WHERE DiagramID = 56 ORDER BY SeqNo")
    print("Connectors in 56:\n", xml56)

    print("\n--- DIAGRAM 13 (ParentID 13: CUN03) ---")
    diag13 = ea.GetDiagramByID(13)
    print("Name:", diag13.Name, "Type:", diag13.Type)
    for obj in diag13.DiagramObjects:
        el = ea.GetElementByID(obj.ElementID)
        print(f"  Obj: ID={obj.ElementID}, Name='{el.Name}', Type='{el.Type}'")
    xml13 = ea.SQLQuery("SELECT Connector_ID, Name, SeqNo, Start_Object_ID, End_Object_ID FROM t_connector WHERE DiagramID = 13 ORDER BY SeqNo")
    print("Connectors in 13:\n", xml13)

finally:
    ea.CloseFile()
    ea.Exit()
