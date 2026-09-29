import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

# Set message 14 (ID 1312) to SubType = 'New' or ActionFlags='Lifecycle=New;'
# And Sequence = 6 for object 38
ea.Execute("UPDATE t_connector SET SubType = 'New' WHERE Connector_ID = 1312")
ea.Execute("UPDATE t_diagramobjects SET Sequence = 6 WHERE Diagram_ID = 12 AND Object_ID = 38")

diag = ea.GetDiagramByID(12)
diag.Update()

ea.GetProjectInterface().PutDiagramImageToFile(diag.DiagramGUID, r'C:\Users\Danie\Desktop\GIT\TD\scratch\test_new_subtype.png', 1)

ea.CloseFile()
ea.Exit()
print("Exported test_new_subtype.png")
