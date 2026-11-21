import win32com.client

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP")

for eid in [666, 777, 778, 779, 780, 781, 1026, 1027]:
    el = ea.GetElementByID(eid)
    print(f"\n=== CONNECTORS FOR {el.Name} (ID {el.ElementID}) ===")
    for conn in el.Connectors:
        cl = ea.GetElementByID(conn.ClientID)
        cr = ea.GetElementByID(conn.SupplierID)
        print(f"  ID={conn.ConnectorID}: [{cl.Name}] --({conn.Type} / {conn.Stereotype})--> [{cr.Name}]")

ea.CloseFile()
