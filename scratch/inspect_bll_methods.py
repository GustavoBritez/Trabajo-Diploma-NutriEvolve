import win32com.client

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP")

for eid in [782, 762, 763, 783, 784, 785, 786, 787]:
    el = ea.GetElementByID(eid)
    print(f"\n=== ELEMENT {el.ElementID}: {el.Name} ({el.Type}) ===")
    print("Attributes:")
    for a in el.Attributes:
        print(f"  + {a.Name}: {a.Type}")
    print("Methods:")
    for m in el.Methods:
        print(f"  + {m.Name} : {m.ReturnType}")

ea.CloseFile()
