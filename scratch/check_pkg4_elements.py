import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
try:
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    pkg = ea.GetPackageByID(4)
    print("Package 4 Elements:")
    for el in pkg.Elements:
        if any(k in el.Name for k in ["Agenda", "Session", "DAL", "Turno", "Bitacora", "Modificar"]):
            print(f"ID={el.ElementID}, Name='{el.Name}', Type='{el.Type}'")
finally:
    ea.CloseFile()
    ea.Exit()
