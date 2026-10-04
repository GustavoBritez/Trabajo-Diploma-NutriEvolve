import win32com.client

def refine_cun01():
    eap_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP'
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    
    diag = ea.GetDiagramByID(12)
    
    # 1. Update Fragment 732 Name to 'Identificacion de Paciente'
    el_732 = ea.GetElementByID(732)
    el_732.Name = "Identificacion de Paciente"
    el_732.Update()
    
    # 2. Update partition heights and names (Bottom-to-Top in EA descriptor)
    part_desc = "@PAR;Name=Paciente Registrado;Size=60;GUID={CFA9E348-3644-4f01-9E75-159A17873AB2};@ENDPAR;@PAR;Name=Paciente No Registrado [Punto de Extension CUN-02];Size=270;GUID={B584A820-20FD-4ba8-B15D-F637FC121C09};@ENDPAR;"
    ea.Execute(f"UPDATE t_xref SET Description = '{part_desc}' WHERE Client = '{el_732.ElementGUID}' AND Name = 'Partitions'")
    
    # 3. Adjust Y coordinate of Fragment 732
    ea.Execute("UPDATE t_diagramobjects SET RectTop = -615, RectBottom = -955, RectLeft = 6, RectRight = 1361 WHERE Diagram_ID = 12 AND Object_ID = 732")
    
    diag.Update()
    diag.DiagramObjects.Refresh()
    
    # Export PNG
    proj = ea.GetProjectInterface()
    out_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\DSS\CUN01 Registrar Turno.png'
    proj.PutDiagramImageToFile(diag.DiagramGUID, out_path, 1)
    print("Exported updated diagram image to:", out_path)
    
    ea.CloseFile()
    ea.Exit()
    print("Refinement completed!")

if __name__ == '__main__':
    refine_cun01()
