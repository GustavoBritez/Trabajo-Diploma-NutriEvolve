import win32com.client

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP")

table_ids = [987, 988, 989, 990, 991, 992, 993, 994, 995, 996, 997]

print("=== CHECKING CONNECTORS FOR SERVICES TABLES ===")
seen = set()
for tid in table_ids:
    el = ea.GetElementByID(tid)
    for conn in el.Connectors:
        cid = conn.ConnectorID
        if cid not in seen:
            seen.add(cid)
            cl = ea.GetElementByID(conn.ClientID)
            cr = ea.GetElementByID(conn.SupplierID)
            print(f"CONN {cid}: [{cl.Name}] --({conn.Type} / {conn.Stereotype})--> [{cr.Name}]")

ea.CloseFile()
