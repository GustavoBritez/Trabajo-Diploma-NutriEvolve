import win32com.client

def inspect_diag13():
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    
    diag = ea.GetDiagramByID(13)
    print("=== DIAGRAM 13 ===")
    print("Name:", diag.Name, "PackageID:", diag.PackageID)
    
    print("\n=== DIAGRAM OBJECTS ===")
    for do in diag.DiagramObjects:
        el = ea.GetElementByID(do.ElementID)
        print(f"ID: {el.ElementID:3d} | Type: {el.Type:18s} | Name: {el.Name:30s} | Left: {do.left:4d}, Right: {do.right:4d}, Top: {do.top:4d}, Bottom: {do.bottom:4d}, Seq: {do.Sequence}")

    print("\n=== CONNECTORS IN DIAGRAM 13 ===")
    xml = ea.SQLQuery("SELECT Connector_ID, Name, Connector_Type, SubType, SeqNo, PtStartX, PtStartY, PtEndX, PtEndY, Start_Object_ID, End_Object_ID FROM t_connector WHERE DiagramID = 13 ORDER BY SeqNo, PtStartY DESC")
    print(xml)

    print("\n=== ALL INTERACTION FRAGMENTS IN TD.EAP ===")
    xml2 = ea.SQLQuery("SELECT Object_ID, Name, Object_Type, Subtype, Notes FROM t_object WHERE Object_Type = 'InteractionFragment'")
    print(xml2)
    
    ea.CloseFile()
    ea.Exit()

if __name__ == '__main__':
    inspect_diag13()
