import win32com.client

def inspect_details():
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    try:
        # Diagram 56 connectors
        print("=== Diagram 56 Connectors ===")
        diag56 = ea.GetDiagramByID(56)
        conns_56 = ea.GetElementSet("SELECT * FROM t_connector WHERE DiagramID = 56 ORDER BY SeqNo", 2)
        for c in conns_56:
            src = ea.GetElementByID(c.ClientID)
            dst = ea.GetElementByID(c.SupplierID)
            print(f"Seq {c.SequenceNo}: {src.Name} -> {dst.Name} | Msg: '{c.Name}' | SubType: '{c.SubType}' | PDATA4: '{c.CustomProperties.Item('PDATA4').Value if hasattr(c, 'CustomProperties') else ''}'")

        # Query direct SQL for t_connector in Diagram 56
        res = ea.SQLQuery("SELECT Connector_ID, SeqNo, Name, SubType, PtStartX, PtEndX, PtStartY, PtEndY, PDATA1, PDATA2, PDATA3, PDATA4 FROM t_connector WHERE DiagramID = 56 ORDER BY SeqNo")
        print("\nSQL t_connector Diagram 56:\n", res[:2000])

        # Diagram 30 connectors
        print("\n=== Diagram 30 Connectors (from t_diagramlinks) ===")
        dl_res = ea.SQLQuery("SELECT dl.DiagramID, dl.ConnectorID, c.Name, c.Connector_Type, c.SubType, c.Stereotype, c.Start_Object_ID, c.End_Object_ID FROM t_diagramlinks dl INNER JOIN t_connector c ON dl.ConnectorID = c.Connector_ID WHERE dl.DiagramID = 30")
        print("Diagram 30 links:\n", dl_res)

        # Diagram 31 connectors
        print("\n=== Diagram 31 Connectors (from t_diagramlinks) ===")
        dl_res31 = ea.SQLQuery("SELECT dl.DiagramID, dl.ConnectorID, c.Name, c.Connector_Type, c.SubType, c.Stereotype, c.Start_Object_ID, c.End_Object_ID FROM t_diagramlinks dl INNER JOIN t_connector c ON dl.ConnectorID = c.Connector_ID WHERE dl.DiagramID = 31")
        print("Diagram 31 links:\n", dl_res31)

    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    inspect_details()
