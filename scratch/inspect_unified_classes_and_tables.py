import win32com.client

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP")

for eid in [417, 418, 419, 420, 424, 425, 426, 430, 431, 432, 433, 434, 435, 436, 797, 798, 799, 800, 801, 868, 869, 870, 891]:
    try:
        el = ea.GetElementByID(eid)
        print(f"=== [{el.ElementID}] {el.Name} (Type={el.Type}, Stereo={el.Stereotype}, Pkg={el.PackageID}) ===")
        for a in el.Attributes:
            print(f"  Attr: {a.Visibility} {a.Name} : {a.Type} [{a.Stereotype}]")
        for m in el.Methods:
            params = ", ".join(f"{p.Name}:{p.Type}" for p in m.Parameters)
            print(f"  Meth: {m.Visibility} {m.Name}({params}) : {m.ReturnType} [{m.Stereotype}]")
    except Exception as e:
        print(f"Error on {eid}: {e}")

ea.CloseFile()
ea.Exit()
