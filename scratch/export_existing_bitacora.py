import win32com.client
import os

def export_bitacora_diagrams():
    ea = win32com.client.Dispatch('EA.Repository')
    eap_path = r'C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP'
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()
    scratch = r'C:\Users\Danie\Desktop\GIT\scratch'

    diag_ids = [119, 120, 121, 122, 123]
    for did in diag_ids:
        try:
            diag = ea.GetDiagramByID(did)
            out_img = os.path.join(scratch, f'diag_{did}_{diag.Type}.png')
            proj.PutDiagramImageToFile(diag.DiagramGUID, out_img, 1)
            print(f'Exported {did}: {diag.Name} -> {out_img}')
        except Exception as e:
            print(f'Error exporting {did}: {e}')

    ea.CloseFile()
    ea.Exit()

if __name__ == '__main__':
    export_bitacora_diagrams()
