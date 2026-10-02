import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
eap_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP'
ea.OpenFile(eap_path)

tables = [430, 431, 432, 433, 434, 435, 436]
for tid in tables:
    el = ea.GetElementByID(tid)
    print(f"\nTable: {el.Name} (ID: {el.ElementID})")
    for a in el.Attributes:
        print(f"  Col: {a.Name} ({a.Type}) isPK={a.IsOrdered}")

ea.CloseFile()
ea.Exit()
