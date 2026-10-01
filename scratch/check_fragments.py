import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
try:
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    for eid in [716, 718, 648, 649, 650, 653]:
        try:
            el = ea.GetElementByID(eid)
            print(f'ID={el.ElementID}, Name="{el.Name}", Type={el.Type}, Subtype={el.Subtype}, Notes="{el.Notes}"')
        except Exception as e:
            print(f'ID={eid} error: {e}')
finally:
    ea.CloseFile()
    try: ea.Exit()
    except: pass
