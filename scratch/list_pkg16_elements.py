import win32com.client

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP")
pkg = ea.GetPackageByID(16)
print(f"Elements in Package 16 ({pkg.Elements.Count}):")
for i in range(pkg.Elements.Count):
    el = pkg.Elements.GetAt(i)
    print(f"Index {i}: ID={el.ElementID}, Name='{el.Name}', Type='{el.Type}', Stereotype='{el.Stereotype}'")
ea.CloseFile()
ea.Exit()
