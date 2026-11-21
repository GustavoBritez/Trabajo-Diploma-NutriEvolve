import win32com.client

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP")

dal_ids = [788, 764, 765, 789, 790, 791, 792, 793, 794]

print("=== CHECKING DAL CLASSES FOR SERVICIOS ===")
for did in dal_ids:
    el = ea.GetElementByID(did)
    print(f"\nID={el.ElementID} | NAME={el.Name} | STEREO={el.Stereotype}")
    print("Methods:")
    for m in el.Methods:
        print(f"  + {m.Name} : {m.ReturnType}")

ea.CloseFile()
ea.Exit()
