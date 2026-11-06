import win32com.client
import os

def create_be_diagram_pn2():
    ea = win32com.client.Dispatch('EA.Repository')
    eap_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP'
    ea.OpenFile(eap_path)
    
    try:
        pkg = ea.GetPackageByID(7) # Class Model
        print(f"Package: {pkg.Name} (ID: {pkg.PackageID})")
        
        diag_name = "Diagrama de Clases BE (CUN06 - CUN09)"
        diag_name_pn2 = "Proceso de Negocio 2 BE"
        
        # Check if already exists and delete to recreate cleanly
        for i in range(pkg.Diagrams.Count - 1, -1, -1):
            d = pkg.Diagrams.GetAt(i)
            if d.Name in [diag_name, diag_name_pn2]:
                print(f"Deleting existing diagram '{d.Name}'...")
                pkg.Diagrams.Delete(i)
        pkg.Diagrams.Refresh()
            
        # Layout positions (l, t, r, b):
        # Center: ConsultaNutricionalBE_DNI101 at (l=500, t=-60, r=900, b=-430)
        # Left: MedicionAntropometricaBE_DNI101 at (l=50, t=-60, r=420, b=-320)
        # Right: DiagnosticoNutricionalBE_DNI101 at (l=980, t=-60, r=1350, b=-300)
        # Bottom-Left: AlertaClinicaBE_DNI101 at (l=250, t=-490, r=640, b=-710)
        # Bottom-Right: PlanAlimentarioBE_DNI101 at (l=760, t=-490, r=1170, b=-760)
        layout = [
            (802, 500, -60, 900, -430),
            (803, 50, -60, 420, -320),
            (804, 980, -60, 1350, -300),
            (805, 250, -490, 640, -710),
            (806, 760, -490, 1170, -760),
        ]
        
        # 1. Create "Diagrama de Clases BE (CUN06 - CUN09)"
        new_diag = pkg.Diagrams.AddNew(diag_name, "Logical")
        new_diag.Notes = "Diagrama de Clases de la capa BE participantes exclusivamente en los Procesos de Negocio PN06 al PN09 / Casos de Uso CUN06 al CUN09 (Atributos únicamente, sin métodos)."
        new_diag.Orientation = "P"
        new_diag.cx = 1200
        new_diag.cy = 900
        new_diag.Scale = 80
        new_diag.ShowDetails = 0
        new_diag.Update()
        pkg.Diagrams.Refresh()
        print(f"Created Diagram '{new_diag.Name}' with ID: {new_diag.DiagramID}")
        
        for el_id, l, t, r, b in layout:
            el = ea.GetElementByID(el_id)
            d_obj = new_diag.DiagramObjects.AddNew(f"l={l};r={r};t={t};b={b};", "")
            d_obj.ElementID = el_id
            d_obj.Update()
            print(f"  Added {el.Name} to {diag_name}")
        new_diag.DiagramObjects.Refresh()
        
        # 2. Add associations from ConsultaNutricionalBE_DNI101 to the other 4 classes
        consulta_el = ea.GetElementByID(802)
        associations_to_add = [
            (803, "+Medicion", "1", "0..1"),
            (804, "+Diagnostico", "1", "0..1"),
            (805, "+Alertas", "1", "0..*"),
            (806, "+PlanAlimentario", "1", "0..1"),
        ]
        
        for item in associations_to_add:
            target_id = item[0]
            role = item[1]
            src_card = item[2]
            dst_card = item[3]
            target_el = ea.GetElementByID(target_id)
            
            exists = False
            for c in consulta_el.Connectors:
                if (c.ClientID == 802 and c.SupplierID == target_id) or (c.ClientID == target_id and c.SupplierID == 802):
                    exists = True
                    break
            if not exists:
                conn = consulta_el.Connectors.AddNew("", "Association")
                conn.SupplierID = target_id
                conn.Direction = "Source -> Destination"
                conn.ClientEnd.Cardinality = src_card
                conn.SupplierEnd.Cardinality = dst_card
                conn.SupplierEnd.Role = role
                conn.Update()
                consulta_el.Connectors.Refresh()
                print(f"  Created Association: ConsultaNutricionalBE -> {target_el.Name} ({dst_card})")
            else:
                print(f"  Association already exists: ConsultaNutricionalBE -> {target_el.Name}")
                
        new_diag.Update()
        ea.ReloadDiagram(new_diag.DiagramID)
        
        # 3. Create "Proceso de Negocio 2 BE" as well so both diagram titles exist
        new_diag_pn2 = pkg.Diagrams.AddNew(diag_name_pn2, "Logical")
        new_diag_pn2.Notes = "Diagrama de Clases BE del Proceso de Negocio 2 (Seguimiento Nutricional Pediátrico)."
        new_diag_pn2.Orientation = "P"
        new_diag_pn2.cx = 1200
        new_diag_pn2.cy = 900
        new_diag_pn2.Scale = 80
        new_diag_pn2.ShowDetails = 0
        new_diag_pn2.Update()
        pkg.Diagrams.Refresh()
        
        for el_id, l, t, r, b in layout:
            d_obj = new_diag_pn2.DiagramObjects.AddNew(f"l={l};r={r};t={t};b={b};", "")
            d_obj.ElementID = el_id
            d_obj.Update()
        new_diag_pn2.DiagramObjects.Refresh()
        new_diag_pn2.Update()
        ea.ReloadDiagram(new_diag_pn2.DiagramID)
        print(f"Created Diagram '{new_diag_pn2.Name}' with ID: {new_diag_pn2.DiagramID}")

        # Update attributes visibility via SQL query (Access uses True / False)
        for did in [new_diag.DiagramID, new_diag_pn2.DiagramID]:
            try:
                ea.Execute(f"UPDATE t_diagram SET AttPub = True, AttPri = True, AttPro = True WHERE Diagram_ID = {did}")
            except Exception as e:
                print(f"SQL update error for diag {did}: {e}")

        # Try to export diagram images
        out_img = os.path.abspath(r'c:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagrama_Clases_BE_CUN06_CUN09.png')
        try:
            proj = ea.GetProjectInterface()
            res = proj.PutDiagramImageToFile(new_diag.DiagramGUID, out_img, 1)
            print(f"Exported diagram image to: {out_img} (Result: {res})")
        except Exception as e:
            print("Could not export diagram image via EA API:", e)
            
        print("\nAll done successfully!")
    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == '__main__':
    create_be_diagram_pn2()
