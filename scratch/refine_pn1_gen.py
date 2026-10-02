import win32com.client
import os
import shutil

def refine_pn1_general():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()
    scratch_dir = r"c:\Users\Danie\Desktop\GIT\TD\scratch"
    artifact_dir = r"C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e"

    try:
        pkg7 = ea.GetPackageByID(7)
        d_pn1_gen = None
        for d in pkg7.Diagrams:
            if d.Name == "Proceso de Negocio 1 General":
                d_pn1_gen = d
                break

        el_p4_pac = ea.GetElementByID(417)
        el_p4_turno = ea.GetElementByID(418)
        el_p4_agenda = ea.GetElementByID(424)
        el_p4_bloque = ea.GetElementByID(425)
        el_p4_usuario = ea.GetElementByID(426)
        el_p4_istate = ea.GetElementByID(419)
        el_p4_st_sol = ea.GetElementByID(420)
        el_p4_st_conf = ea.GetElementByID(421)
        el_p4_st_asist = ea.GetElementByID(422)
        el_p4_st_canc = ea.GetElementByID(423)

        count = d_pn1_gen.DiagramObjects.Count
        for i in range(count - 1, -1, -1):
            d_pn1_gen.DiagramObjects.Delete(i)
        d_pn1_gen.DiagramObjects.Refresh()

        def add_do(d, el_id, l, t, r, b):
            obj = d.DiagramObjects.AddNew("", "")
            obj.ElementID = el_id
            obj.left = l
            obj.top = t
            obj.right = r
            obj.bottom = b
            obj.Update()
            return obj

        # Row 1: Paciente, Turno, BloqueHorario, AgendaMedica, Usuario
        add_do(d_pn1_gen, el_p4_pac.ElementID, 50, -60, 480, -450)
        add_do(d_pn1_gen, el_p4_turno.ElementID, 560, -60, 1140, -560)
        add_do(d_pn1_gen, el_p4_bloque.ElementID, 1220, -60, 1540, -280)
        add_do(d_pn1_gen, el_p4_agenda.ElementID, 1620, -60, 2060, -320)
        add_do(d_pn1_gen, el_p4_usuario.ElementID, 2140, -60, 2480, -340)

        # State pattern: IEstadoTurno
        add_do(d_pn1_gen, el_p4_istate.ElementID, 650, -630, 1050, -740)

        # 2x2 Grid of States (620px wide each!)
        add_do(d_pn1_gen, el_p4_st_sol.ElementID, 360, -790, 1000, -910)
        add_do(d_pn1_gen, el_p4_st_conf.ElementID, 1060, -790, 1700, -910)
        add_do(d_pn1_gen, el_p4_st_asist.ElementID, 360, -950, 1000, -1070)
        add_do(d_pn1_gen, el_p4_st_canc.ElementID, 1060, -950, 1700, -1070)

        d_pn1_gen.DiagramObjects.Refresh()
        d_pn1_gen.Update()

        f_scratch = os.path.join(scratch_dir, "pn1_general.png")
        f_art = os.path.join(artifact_dir, "pn1_general.png")
        proj.PutDiagramImageToFile(d_pn1_gen.DiagramGUID, f_scratch, 1)
        shutil.copyfile(f_scratch, f_art)
        print("Updated pn1_general.png")

    finally:
        ea.CloseFile()
        ea.Exit()

if __name__ == "__main__":
    refine_pn1_general()
