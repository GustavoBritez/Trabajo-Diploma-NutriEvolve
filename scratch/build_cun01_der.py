import win32com.client
import os
import shutil

def build_cun01_der():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    try:
        print("--- Building CUN01 DER (Diagram 26) ---")
        diag = ea.GetDiagramByID(26)
        diag.Name = "CUN01: DER (Modelo Relacional)"
        diag.Notes = "Diagrama Entidad-Relación (DER) / Modelo Relacional del CUN01 (Registrar Turno) con todas las tablas del dominio, claves primarias (PK), foráneas (FK), tipos de datos y cardinalidades."
        diag.Update()

        # Elements in Diagram 26:
        # El ID 429: Tutores_DNI101
        # El ID 430: Pacientes_DNI101
        # El ID 431: Usuarios
        # El ID 432: AgendasMedicas_DNI101
        # El ID 433: BloquesHorarios_DNI101
        # El ID 434: Turnos_DNI101
        # El ID 435: DV
        # El ID 436: Bitacora

        el_tutor = ea.GetElementByID(429)
        el_paciente = ea.GetElementByID(430)
        el_usuario = ea.GetElementByID(431)
        el_agenda = ea.GetElementByID(432)
        el_bloque = ea.GetElementByID(433)
        el_turno = ea.GetElementByID(434)
        el_dv = ea.GetElementByID(435)
        el_bitacora = ea.GetElementByID(436)

        # Fix encodings on attributes if needed
        for a in el_paciente.Attributes:
            if "Ni" in a.Name:
                a.Name = "DniNiño_DNI101"
                a.Update()
        el_paciente.Attributes.Refresh()

        for a in el_usuario.Attributes:
            if "Contrase" in a.Name:
                a.Name = "Contraseña"
                a.Update()
        el_usuario.Attributes.Refresh()

        # Clear existing DiagramObjects
        for i in range(diag.DiagramObjects.Count - 1, -1, -1):
            diag.DiagramObjects.Delete(i)
        diag.DiagramObjects.Refresh()

        # Balanced 3-column Layout:
        # Left column (x: 50 .. 360, width 310):
        #   Tutores_DNI101:   Top = 50,  Bottom = 310
        #   Pacientes_DNI101: Top = 390, Bottom = 670
        #   DV:               Top = 750, Bottom = 870
        #
        # Center column (x: 440 .. 780, width 340):
        #   Bitacora:         Top = 50,  Bottom = 250
        #   Usuarios:         Top = 330, Bottom = 650
        #   Turnos_DNI101:    Top = 730, Bottom = 1030
        #
        # Right column (x: 860 .. 1170, width 310):
        #   AgendasMedicas_DNI101:  Top = 50,  Bottom = 230
        #   BloquesHorarios_DNI101: Top = 330, Bottom = 550

        coords = [
            # Left column
            (el_tutor, 50, 50, 360, 310),
            (el_paciente, 50, 390, 360, 670),
            (el_dv, 50, 750, 360, 870),
            # Center column
            (el_bitacora, 440, 50, 780, 250),
            (el_usuario, 440, 330, 780, 650),
            (el_turno, 440, 730, 780, 1030),
            # Right column
            (el_agenda, 860, 50, 1170, 230),
            (el_bloque, 860, 330, 1170, 550)
        ]

        for el, left, top, right, bottom in coords:
            do = diag.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag.DiagramObjects.Refresh()
        diag.Update()
        ea.SaveDiagram(diag.DiagramID)

        # Orthogonal direct style for diagram links
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 26")

        # Export diagram image
        scratch_dir = r"c:\Users\Danie\Desktop\GIT\TD\scratch"
        out_png = os.path.join(scratch_dir, "cun01_der.png")
        if os.path.exists(out_png):
            os.remove(out_png)

        proj.PutDiagramImageToFile(diag.DiagramGUID, out_png, 1)
        print("Diagram 26 exported successfully to:", out_png)

        # Copy to brain artifact directory
        artifact_dir = r"C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e"
        dest_png = os.path.join(artifact_dir, "cun01_der.png")
        shutil.copy2(out_png, dest_png)
        print("Copied to brain artifact:", dest_png)

    finally:
        ea.CloseFile()
        ea.Exit()
        print("EA closed.")

if __name__ == "__main__":
    build_cun01_der()
