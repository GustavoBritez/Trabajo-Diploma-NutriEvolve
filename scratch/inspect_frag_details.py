import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

for el_id in [732, 728, 734]:
    el = ea.GetElementByID(el_id)
    print(f"ID={el.ElementID} | Name='{el.Name}' | Type={el.Type} | SubType={el.Subtype} | Notes='{el.Notes}'")

ea.CloseFile()
ea.Exit()
