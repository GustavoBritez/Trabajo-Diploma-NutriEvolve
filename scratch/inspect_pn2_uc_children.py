import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

for ucid in [42, 46, 674, 675]:
    uc = ea.GetElementByID(ucid)
    print(f"\nUse Case {ucid}: {uc.Name}")
    for el in uc.Elements:
        print(f"  Child ID: {el.ElementID}, Name: '{el.Name}', Type: '{el.Type}', Stereo: '{el.Stereotype}'")

ea.CloseFile()
ea.Exit()
