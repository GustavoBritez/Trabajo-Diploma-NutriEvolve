import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

diag37 = ea.GetDiagramByID(37)
print(f"=== Elements & Links in Diagram 37 ({diag37.Name}) ===")
for do in diag37.DiagramObjects:
    el = ea.GetElementByID(do.ElementID)
    print(f"Element ID: {el.ElementID}, Name: {el.Name}, Type: {el.Type}")

for link in diag37.DiagramLinks:
    conn = ea.GetConnectorByID(link.ConnectorID)
    print(f"Conn ID: {conn.ConnectorID}, Name: '{conn.Name}', Type: '{conn.Type}', Client: {conn.ClientID}, Supplier: {conn.SupplierID}")

ea.CloseFile()
