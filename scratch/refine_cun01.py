import win32com.client

def refine_cun01():
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    
    diag = ea.GetDiagramByID(12)
    
    # 1. Update lifelines positions and sequence
    lifeline_bottom = -1580
    
    coords = {
        219: (30, 130, -50, lifeline_bottom, 2),
        220: (170, 290, -50, lifeline_bottom, 3),
        221: (330, 460, -50, lifeline_bottom, 4),
        222: (490, 610, -50, lifeline_bottom, 5),
        225: (820, 940, -50, lifeline_bottom, 8),
        226: (970, 1080, -50, lifeline_bottom, 9),
        227: (1110, 1240, -50, lifeline_bottom, 10),
        229: (1270, 1400, -50, lifeline_bottom, 12),
        230: (1430, 1540, -50, lifeline_bottom, 13),
        228: (1570, 1670, -50, lifeline_bottom, 11),
        # InteractionFragment 232: Bottom at -675 so messages 16..34 are outside
        232: (20, 790, -520, -680, 1),
        # CUN02 oval: Sequence = 6, Top = -590, Bottom = -700
        38:  (640, 780, -590, -700, 6)
    }
    
    for d_obj in diag.DiagramObjects:
        if d_obj.ElementID in coords:
            l, r, t, b, seq = coords[d_obj.ElementID]
            d_obj.left = l
            d_obj.right = r
            d_obj.top = t
            d_obj.bottom = b
            d_obj.Sequence = seq
            d_obj.Update()
            
    diag.DiagramObjects.Refresh()
    diag.Update()
    
    # 2. Make TurnoBE and TurnoSolicitadoState creation use SynchCall so they stay in top header
    # Message 18 (new TurnoBE_DNI101) and Message 20 (new TurnoSolicitadoState)
    # Let's find their Connector_IDs and update SubType = 'SynchCall'
    ea.Execute("UPDATE t_connector SET SubType = 'SynchCall' WHERE DiagramID = 12 AND SeqNo IN (18, 20)")
    
    diag.Update()
    out_img = r'C:\Users\Danie\Desktop\GIT\TD\scratch\cun01_sequence_refined.png'
    ea.GetProjectInterface().PutDiagramImageToFile(diag.DiagramGUID, out_img, 1)
    print(f"Exported refined image to {out_img}")
    
    ea.CloseFile()
    ea.Exit()
    print("Refined successfully")

if __name__ == '__main__':
    refine_cun01()
