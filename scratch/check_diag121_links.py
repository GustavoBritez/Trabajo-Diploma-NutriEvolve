import win32com.client

def check_diag121():
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP')
    diag = ea.GetDiagramByID(121)
    print('Diagram 121 Links Count:', diag.DiagramLinks.Count)
    for i in range(diag.DiagramLinks.Count):
        dl = diag.DiagramLinks.GetAt(i)
        c = ea.GetConnectorByID(dl.ConnectorID)
        s = ea.GetElementByID(c.ClientID)
        t = ea.GetElementByID(c.SupplierID)
        print(f'{i+1}. [ConnID {c.ConnectorID}] "{c.Name}" | {s.Name} -> {t.Name} (Seq: {c.SequenceNo}, Type: {c.Type}, SubType: {c.SubType})')
    ea.CloseFile()
    ea.Exit()

if __name__ == '__main__':
    check_diag121()
