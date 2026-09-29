import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
pkg = ea.GetPackageByID(4)

test_diag = pkg.Diagrams.AddNew('Test Clean X', 'Sequence')
test_diag.Update()

do1 = test_diag.DiagramObjects.AddNew('l=100;r=200;t=-50;b=-600;', '')
do1.ElementID = 220
do1.Update()

do2 = test_diag.DiagramObjects.AddNew('l=350;r=470;t=-150;b=-600;', '')
do2.ElementID = 38
do2.Update()
test_diag.Update()

el1 = ea.GetElementByID(220)
el2 = ea.GetElementByID(38)

# Form -> UC
c1 = el1.Connectors.AddNew('1: Invocacion()', 'Sequence')
c1.SupplierID = 38
c1.SubType = 'SynchCall'
c1.Update()

# UC -> Form
c2 = el2.Connectors.AddNew('2: PacienteRegistradoOK()', 'Sequence')
c2.SupplierID = 220
c2.SubType = 'Return'
c2.Update()

# Delete marker
c3 = el1.Connectors.AddNew('', 'Sequence')
c3.SupplierID = 38
c3.SubType = 'Delete'
c3.Update()

ea.Execute(f"UPDATE t_connector SET DiagramID = {test_diag.DiagramID}, SeqNo = 1, PtStartX = 150, PtStartY = -190, PtEndX = 410, PtEndY = -190 WHERE Connector_ID = {c1.ConnectorID}")
ea.Execute(f"UPDATE t_connector SET DiagramID = {test_diag.DiagramID}, SeqNo = 2, PtStartX = 410, PtStartY = -250, PtEndX = 150, PtEndY = -250 WHERE Connector_ID = {c2.ConnectorID}")
ea.Execute(f"UPDATE t_connector SET DiagramID = {test_diag.DiagramID}, SeqNo = 3, PtStartX = 410, PtStartY = -270, PtEndX = 410, PtEndY = -270, StyleEx = 'hidden=1;' WHERE Connector_ID = {c3.ConnectorID}")

test_diag.Update()
ea.GetProjectInterface().PutDiagramImageToFile(test_diag.DiagramGUID, r'C:\Users\Danie\Desktop\GIT\TD\scratch\test_clean_x.png', 1)

for i in range(pkg.Diagrams.Count - 1, -1, -1):
    if pkg.Diagrams.GetAt(i).Name == 'Test Clean X':
        pkg.Diagrams.Delete(i)
pkg.Diagrams.Refresh()

ea.CloseFile()
ea.Exit()
print('Done clean x')
