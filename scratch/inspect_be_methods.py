import win32com.client

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP")

for eid in [666, 777, 778, 779, 780, 781, 1026, 1027]:
    el = ea.GetElementByID(eid)
    print(f"\n=== ELEMENT {el.ElementID}: {el.Name} ({el.Type}) ===")
    print("Attributes:")
    for a in el.Attributes:
        print(f"  + {a.Name}: {a.Type}")
    print("Methods:")
    for m in el.Methods:
        print(f"  + {m.Name}({m.Parameters}) : {m.ReturnType}")

ea.CloseFile()
