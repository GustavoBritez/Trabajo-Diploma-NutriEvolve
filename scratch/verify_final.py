import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP')

ids = [125, 121, 119, 120, 126, 128, 127, 129, 131, 130]
print('=== FINAL VERIFICATION OF REQUESTED DIAGRAMS IN TD.EAP ===')
for did in ids:
    diag = ea.GetDiagramByID(did)
    pkg = ea.GetPackageByID(diag.PackageID)
    print(f'ID {diag.DiagramID:3d} | [{diag.Type:9s}] | Package: {pkg.Name:26s} | Objects: {diag.DiagramObjects.Count:2d} | Name: "{diag.Name}"')

ea.CloseFile()
ea.Exit()
