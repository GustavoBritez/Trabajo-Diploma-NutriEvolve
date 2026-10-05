import win32com.client

def apply_pn1_planar():
    ea = win32com.client.Dispatch("EA.App")
    repo = ea.Repository
    repo.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    
    # Diagram 68: Proceso de Negocio 1 General
    diag = repo.GetDiagramByID(68)
    
    coords = {
        417: (50, 430, -50, -370),       # Paciente_DNI101
        418: (500, 970, -50, -880),      # Turno_DNI101
        426: (1040, 1390, -50, -420),    # Usuario
        424: (1460, 1870, -50, -400),    # AgendaMedica_DNI101
        425: (1460, 1800, -460, -760),   # BloqueHorario_DNI101
        419: (550, 1100, -950, -1080),   # IEstadoTurno_DNI101
        420: (50, 550, -1150, -1300),    # TurnoSolicitadoState_DNI101
        421: (580, 1080, -1150, -1300),  # TurnoConfirmadoState_DNI101
        422: (1110, 1610, -1150, -1300), # TurnoAsistioState_DNI101
        423: (1640, 2140, -1150, -1300), # TurnoCanceladoState_DNI101
    }
    
    for do in diag.DiagramObjects:
        if do.ElementID in coords:
            l, r, t, b = coords[do.ElementID]
            do.left = l
            do.right = r
            do.top = t
            do.bottom = b
            do.Update()
            
    diag.Update()
    
    # Refresh diagram links style
    repo.Execute("UPDATE t_diagramlinks SET Style = 'Mode=3;', Geometry = '' WHERE DiagramID = 68")
    
    pe = repo.GetProjectInterface()
    pe.PutDiagramImageToFile(diag, r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Clases\Proceso de Negocio 1 General.png", 1)
    print("Exported Proceso de Negocio 1 General.png")
    
    repo.CloseFile()

if __name__ == "__main__":
    apply_pn1_planar()
