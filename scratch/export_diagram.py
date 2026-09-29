import win32com.client

def export_diagram_image():
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    diagram = ea.GetDiagramByID(33)
    project_interface = ea.GetProjectInterface()
    out_img = r'C:\Users\Danie\Desktop\GIT\TD\scratch\secuencia_general.png'
    try:
        project_interface.PutDiagramImageToFile(diagram.DiagramGUID, out_img, 1)
        print("Exported with PutDiagramImageToFile to:", out_img)
    except Exception as e:
        print("Error PutDiagramImageToFile:", e)
    ea.CloseFile()
    ea.Exit()

if __name__ == '__main__':
    export_diagram_image()
