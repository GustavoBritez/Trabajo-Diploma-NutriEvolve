import win32com.client

def check_uc39_37():
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    try:
        for ucid in [39, 37]:
            uc = ea.GetElementByID(ucid)
            print(f"\nUseCase {ucid}: {uc.Name}")
            for el in uc.Elements:
                print(f"  El {el.ElementID}: '{el.Name}' ({el.Type}, Subtype: {el.Subtype})")
    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    check_uc39_37()
