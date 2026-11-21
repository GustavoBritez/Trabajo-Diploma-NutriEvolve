import win32com.client

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP")

dal_ids = [788, 764, 765, 789, 790, 791, 792, 793, 794]
seen = set()
for did in dal_ids:
    el = ea.GetElementByID(did)
    for conn in el.Connectors:
        if conn.ConnectorID not in seen:
            seen.add(conn.ConnectorID)
            cl = ea.GetElementByID(conn.ClientID)
            cr = ea.GetElementByID(conn.SupplierID)
            print(f"CONN {conn.ConnectorID}: [{cl.Name}] --({conn.Type} / {conn.Stereotype})--> [{cr.Name}]")

ea.CloseFile()
ea.Exit()
