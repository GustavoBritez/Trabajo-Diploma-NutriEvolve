import win32com.client

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP")

bll_ids = [782, 762, 763, 783, 784, 785, 786, 787]

print("=== CHECKING CONNECTORS FOR BLL CLASSES ===")
seen = set()
for bid in bll_ids:
    el = ea.GetElementByID(bid)
    for conn in el.Connectors:
        cid = conn.ConnectorID
        if cid not in seen:
            seen.add(cid)
            cl = ea.GetElementByID(conn.ClientID)
            cr = ea.GetElementByID(conn.SupplierID)
            print(f"CONN {cid}: [{cl.Name}] --({conn.Type} / {conn.Stereotype})--> [{cr.Name}]")

ea.CloseFile()
ea.Exit()
