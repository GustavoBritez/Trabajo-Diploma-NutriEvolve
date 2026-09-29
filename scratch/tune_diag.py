import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

# Set Sequence = 6 for object 38, RectBottom = -770
ea.Execute("UPDATE t_diagramobjects SET Sequence = 6, RectTop = -650, RectBottom = -770 WHERE Diagram_ID = 12 AND Object_ID = 38")

# Set fragment 232 RectBottom = -780
ea.Execute("UPDATE t_diagramobjects SET RectTop = -560, RectBottom = -780, RectLeft = 20, RectRight = 790 WHERE Diagram_ID = 12 AND Object_ID = 232")

# Set c16 (Delete) coordinates right at the end of CUN02 lifeline inside the box
# PtStartY = -760, PtEndY = -760
ea.Execute("UPDATE t_connector SET PtStartX = 230, PtStartY = -760, PtEndX = 710, PtEndY = -760, SeqNo = 16, SubType = 'Delete', StyleEx = NULL WHERE Connector_ID = 1314")

diag = ea.GetDiagramByID(12)
diag.Update()

ea.GetProjectInterface().PutDiagramImageToFile(diag.DiagramGUID, r'C:\Users\Danie\Desktop\GIT\TD\scratch\cun01_sequence_tuned.png', 1)

ea.CloseFile()
ea.Exit()
print("Tuned and exported cun01_sequence_tuned.png")
