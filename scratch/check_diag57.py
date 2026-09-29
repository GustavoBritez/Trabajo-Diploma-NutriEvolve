import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
try:
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    diag = ea.GetDiagramByID(57)
    print("Diagram ID:", diag.DiagramID, "Name:", diag.Name, "Type:", diag.Type, "ParentID:", diag.ParentID)
    print("--- Diagram Objects in Diagram 57 ---")
    for obj in diag.DiagramObjects:
        el = ea.GetElementByID(obj.ElementID)
        print(f"Obj ID={obj.ElementID}, Name='{el.Name}', Type='{el.Type}', Left={obj.left}, Right={obj.right}")
    
    xml = ea.SQLQuery("SELECT Connector_ID, Name, Connector_Type, SubType, Start_Object_ID, End_Object_ID, SeqNo FROM t_connector WHERE DiagramID = 57 ORDER BY SeqNo")
    print("--- Connectors in Diagram 57 ---")
    print(xml)
finally:
    ea.CloseFile()
    ea.Exit()
