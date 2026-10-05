import win32com.client

def check_uc13():
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    try:
        uc = ea.GetElementByID(13) # UseCase 13
        print(f"UseCase 13: {uc.Name}")
        for el in uc.Elements:
            print(f"  El {el.ElementID}: '{el.Name}' ({el.Type}, Subtype: {el.Subtype})")
        # Check Package 4 elements
        pkg4 = ea.GetPackageByID(4)
        print(f"Package 4: {pkg4.Name}")
        for el in pkg4.Elements:
            if "03" in el.Name or "Reprogramar" in el.Name:
                print(f"  Pkg4 El {el.ElementID}: '{el.Name}' ({el.Type})")
    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    check_uc13()
