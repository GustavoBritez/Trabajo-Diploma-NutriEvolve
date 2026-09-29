import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
try:
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    diag = ea.GetDiagramByID(58)
    print('Diagram ID:', diag.DiagramID, 'Name:', diag.Name, 'PackageID:', diag.PackageID)
    pkg = ea.GetPackageByID(diag.PackageID)
    print('Package Name:', pkg.Name)
    print('--- Diagram Objects in Diagram 58 ---')
    for obj in diag.DiagramObjects:
        el = ea.GetElementByID(obj.ElementID)
        print(f'Obj ID={obj.ElementID}, Name="{el.Name}", Type="{el.Type}", Left={obj.left}, Right={obj.right}')
    print('--- Package Elements ---')
    for el in pkg.Elements:
        print(f'Pkg El ID={el.ElementID}, Name="{el.Name}", Type="{el.Type}"')
finally:
    ea.CloseFile()
    ea.Exit()
