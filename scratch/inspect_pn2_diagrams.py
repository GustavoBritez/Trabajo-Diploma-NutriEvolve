import win32com.client

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

def inspect_diag(diag_id):
    diag = ea.GetDiagramByID(diag_id)
    print(f"\n=== DIAGRAM {diag_id}: {diag.Name} ({diag.Type}) ===")
    for do in diag.DiagramObjects:
        el = ea.GetElementByID(do.ElementID)
        print(f"  Element ID: {el.ElementID}, Name: '{el.Name}', Type: '{el.Type}', Stereotype: '{el.Stereotype}'")
        
inspect_diag(35)
inspect_diag(37)
inspect_diag(3)

ea.CloseFile()
