import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

# Set Sequence = 999999 for object 38, RectTop = -650, RectBottom = -770
ea.Execute("UPDATE t_diagramobjects SET Sequence = 999999, RectTop = -650, RectBottom = -770, RectLeft = 640, RectRight = 780 WHERE Diagram_ID = 12 AND Object_ID = 38")

# Fragment 232: Box bounds: Left=20, Right=790, Top=-560, Bottom=-790
ea.Execute("UPDATE t_diagramobjects SET RectTop = -560, RectBottom = -790, RectLeft = 20, RectRight = 790 WHERE Diagram_ID = 12 AND Object_ID = 232")

diag = ea.GetDiagramByID(12)
diag.Update()

ea.GetProjectInterface().PutDiagramImageToFile(diag.DiagramGUID, r'C:\Users\Danie\Desktop\GIT\TD\scratch\cun01_sequence_oval_test.png', 1)

ea.CloseFile()
ea.Exit()
print("Exported cun01_sequence_oval_test.png")
