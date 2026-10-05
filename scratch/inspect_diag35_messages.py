import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

diag35 = ea.GetDiagramByID(35)
print(f"=== Messages in Diagram 35 ({diag35.Name}) ===")
for link in diag35.DiagramLinks:
    conn = ea.GetConnectorByID(link.ConnectorID)
    print(f"Conn ID: {conn.ConnectorID}, Name: '{conn.Name}', Type: '{conn.Type}', Subtype: '{conn.SubType}', Client: {conn.ClientID}, Supplier: {conn.SupplierID}")

ea.CloseFile()
