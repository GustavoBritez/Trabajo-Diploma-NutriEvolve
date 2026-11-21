import win32com.client

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP")

for pkg_id in [5, 7, 16, 17, 18]:
    pkg = ea.GetPackageByID(pkg_id)
    print(f"=== PKG {pkg.PackageID}: {pkg.Name} ===")
    for el in pkg.Elements:
        print(f"  ID={el.ElementID:<5} TYPE={el.Type:<10} STEREO={el.Stereotype:<10} NAME={el.Name}")

ea.CloseFile()
