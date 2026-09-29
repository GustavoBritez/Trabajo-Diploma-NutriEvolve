import win32com.client
import sys

def update_secuencia_general(eap_path):
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    
    # 1. Find diagram
    diagram = ea.GetDiagramByID(33)
    print(f"Diagram: {diagram.Name} (ID: {diagram.DiagramID})")
    
    # Check if system object exists in diagram or create one in package 4
    pkg = ea.GetPackageByID(diagram.PackageID)
    
    # Let's find or create ':Sistema NutriEvolve' object
    system_obj = None
    for obj in pkg.Elements:
        if obj.Name == "Sistema (NutriEvolve)" or obj.Name == ":Sistema NutriEvolve" or obj.Name == "Sistema NutriEvolve":
            system_obj = obj
            break
            
    if not system_obj:
        print("Creating ':Sistema NutriEvolve' boundary/lifeline object...")
        system_obj = pkg.Elements.AddNew("Sistema (NutriEvolve)", "Boundary")
        system_obj.Update()
        pkg.Elements.Refresh()
        
    # Get Paciente (643) and Nutricionista (644)
    paciente_obj = ea.GetElementByID(643)
    nutri_obj = ea.GetElementByID(644)
    
    print(f"Paciente: {paciente_obj.Name} (ID: {paciente_obj.ElementID})")
    print(f"Nutricionista: {nutri_obj.Name} (ID: {nutri_obj.ElementID})")
    print(f"Sistema: {system_obj.Name} (ID: {system_obj.ElementID})")
    
    # Ensure system_obj is in the diagram
    has_system_in_diag = False
    for do in diagram.DiagramObjects:
        if do.ElementID == system_obj.ElementID:
            has_system_in_diag = True
            break
            
    if not has_system_in_diag:
        print("Adding Sistema to diagram...")
        new_do = diagram.DiagramObjects.AddNew("l=550;r=670;t=-50;b=-1300;", "")
        new_do.ElementID = system_obj.ElementID
        new_do.Update()
        diagram.DiagramObjects.Refresh()
        
    # Position objects nicely
    # Paciente: left 80, right 170
    # Nutricionista: left 320, right 410
    # Sistema: left 560, right 650
    for do in diagram.DiagramObjects:
        if do.ElementID == paciente_obj.ElementID:
            do.left = 80
            do.right = 170
            do.top = -50
            do.bottom = -1300
            do.Update()
        elif do.ElementID == nutri_obj.ElementID:
            do.left = 320
            do.right = 410
            do.top = -50
            do.bottom = -1300
            do.Update()
        elif do.ElementID == system_obj.ElementID:
            do.left = 560
            do.right = 650
            do.top = -50
            do.bottom = -1300
            do.Update()
            
    diagram.Update()
    
    ea.CloseFile()
    ea.Exit()
    print("Done inspecting/updating diagram objects.")

if __name__ == '__main__':
    update_secuencia_general(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
