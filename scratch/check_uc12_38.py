import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
try:
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    for uid in [12, 38]:
        uc = ea.GetElementByID(uid)
        print(f'=== UseCase {uid}: {uc.Name} ===')
        for el in uc.Elements:
            print(f'  Child ID={el.ElementID}, Name="{el.Name}", Type={el.Type}, Subtype={el.Subtype}')
finally:
    ea.CloseFile()
    try: ea.Exit()
    except: pass
