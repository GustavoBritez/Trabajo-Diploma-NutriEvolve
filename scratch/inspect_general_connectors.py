import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch("EA.Repository")
ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP")

for did in [67, 68, 85, 86, 119, 120]:
    d = ea.GetDiagramByID(did)
    print(f"\n=== Diagram {did}: '{d.Name}' (StyleEx='{d.StyleEx}', ExtendedStyle='{d.ExtendedStyle}') ===")
    obj_ids = [str(d.DiagramObjects.GetAt(i).ElementID) for i in range(d.DiagramObjects.Count)]
    if obj_ids:
        id_str = ",".join(obj_ids)
        sql = f"""
        SELECT c.Connector_ID, c.Connector_Type, c.Name, c.Stereotype, c.SubType, c.Direction,
               c.SourceCard, c.DestCard, c.SourceRole, c.DestRole, c.SourceIsAggregate, c.DestIsAggregate,
               o1.Name as Src, o2.Name as Dst
        FROM (t_connector c
        INNER JOIN t_object o1 ON c.Start_Object_ID = o1.Object_ID)
        INNER JOIN t_object o2 ON c.End_Object_ID = o2.Object_ID
        WHERE c.Start_Object_ID IN ({id_str}) AND c.End_Object_ID IN ({id_str})
        """
        root = ET.fromstring(ea.SQLQuery(sql))
        for r in root.findall(".//Row"):
            print("  Conn:", {c.tag: c.text for c in r})

ea.CloseFile()
ea.Exit()
