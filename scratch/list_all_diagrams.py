import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
try:
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    def dump_pkg(pkg):
        for d in pkg.Diagrams:
            print(f"Diagram ID={d.DiagramID}, Name='{d.Name}', Type='{d.Type}', Package='{pkg.Name}'")
        for sub in pkg.Packages:
            dump_pkg(sub)
    for model in ea.Models:
        dump_pkg(model)
finally:
    ea.CloseFile()
    ea.Exit()
