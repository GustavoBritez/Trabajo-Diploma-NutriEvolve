import win32com.client
import os
import shutil
import uuid

def polish():
    eap_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP'
    scratch_dir = r'c:\Users\Danie\Desktop\GIT\TD\scratch'
    artifact_dir = r'C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e'

    dss_dir = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\DSS'
    dc_dir = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Clases'
    der_dir = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Entidad Relacion'

    print("Connecting to Enterprise Architect for visual polish...")
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    def export_and_copy(diag, base_name, dest_folder):
        out_path = os.path.join(dest_folder, base_name + ".png")
        if os.path.exists(out_path):
            try:
                os.remove(out_path)
            except:
                pass
        proj.PutDiagramImageToFile(diag.DiagramGUID, out_path, 1)
        shutil.copy2(out_path, os.path.join(scratch_dir, base_name + ".png"))
        shutil.copy2(out_path, os.path.join(artifact_dir, base_name + ".png"))
        print(f"Exported: {out_path}")

    try:
        # =====================================================================
        # 1. POLISH CUN03 SEQUENCE (Diagram 13)
        # =====================================================================
        print("Polishing Diagram 13 (Fragment box)...")
        diag13 = ea.GetDiagramByID(13)
        el_frag13 = ea.GetElementByID(741)

        # Update fragment bounds: top: -520, bottom: -980 (height = 460)
        # partition 1 (top): size = 130
        # partition 2 (bottom): size = 330
        for obj in diag13.DiagramObjects:
            if obj.ElementID == el_frag13.ElementID:
                obj.top = -520
                obj.bottom = -980
                obj.left = 10
                obj.right = 1450
                obj.Update()

        guid1 = str(uuid.uuid4()).upper()
        guid2 = str(uuid.uuid4()).upper()
        part_desc13 = f"@PAR;Name=[Datos Validos y Disponibles];Size=330;GUID={{{guid2}}};@ENDPAR;@PAR;Name=[Estado Invalido o Superposicion: Asistio / Cancelado / Ocupado];Size=130;GUID={{{guid1}}};@ENDPAR;"
        ea.Execute(f"UPDATE t_xref SET Description = '{part_desc13}' WHERE Client = '{el_frag13.ElementGUID}' AND Name = 'Partitions'")

        diag13.DiagramObjects.Refresh()
        diag13.Update()
        ea.SaveDiagram(13)
        export_and_copy(diag13, "CUN03 Reprogramar Turno", dss_dir)

        # =====================================================================
        # 2. POLISH DER DIAGRAMS (61, 63, 65) - CLEAN UN-CROSSED LAYOUT
        # =====================================================================
        print("Polishing DER diagrams 61, 63, 65...")
        el_tab_turno = ea.GetElementByID(434)
        el_tab_bloque = ea.GetElementByID(433)
        el_tab_agenda = ea.GetElementByID(432)
        el_tab_usr = ea.GetElementByID(431)
        el_tab_bit = ea.GetElementByID(436)
        el_tab_dv = ea.GetElementByID(435)

        # DER 61 (CUN03)
        # Column 1: AgendasMedicas (top), BloquesHorarios (mid), Turnos (bottom)
        # Column 2: Usuarios (top), Bitacora (mid), DV (bottom)
        # This way:
        # Usuarios -> AgendasMedicas (left) : horizontal
        # Usuarios -> Bitacora (down) : vertical
        # Usuarios -> Turnos (down-left) : clean
        # Agendas -> Bloques (down) : vertical
        # Bloques -> Turnos (down) : vertical
        # ZERO line crossings!
        diag61 = ea.GetDiagramByID(61)
        for i in range(diag61.DiagramObjects.Count - 1, -1, -1):
            diag61.DiagramObjects.Delete(i)
        diag61.DiagramObjects.Refresh()

        coords61_clean = [
            (el_tab_agenda, 50,  40,  410, 240),
            (el_tab_bloque, 50,  320, 410, 540),
            (el_tab_turno,  50,  620, 410, 930),
            (el_tab_usr,    520, 40,  880, 350),
            (el_tab_bit,    520, 430, 880, 640),
            (el_tab_dv,     520, 720, 880, 850),
        ]
        for el, left, top, right, bottom in coords61_clean:
            do = diag61.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()
        diag61.DiagramObjects.Refresh()
        diag61.Update()
        ea.SaveDiagram(61)
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 61")
        export_and_copy(diag61, "CUN03 DER (Modelo Relacional)", der_dir)

        # DER 63 (CUN04)
        # Column 1: Turnos (top), BloquesHorarios (bottom)
        # Column 2: Usuarios (top), Bitacora (mid), DV (bottom)
        diag63 = ea.GetDiagramByID(63)
        for i in range(diag63.DiagramObjects.Count - 1, -1, -1):
            diag63.DiagramObjects.Delete(i)
        diag63.DiagramObjects.Refresh()

        coords63_clean = [
            (el_tab_turno,  50,  40,  410, 350),
            (el_tab_bloque, 50,  430, 410, 650),
            (el_tab_usr,    520, 40,  880, 350),
            (el_tab_bit,    520, 430, 880, 640),
            (el_tab_dv,     520, 720, 880, 850),
        ]
        for el, left, top, right, bottom in coords63_clean:
            do = diag63.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()
        diag63.DiagramObjects.Refresh()
        diag63.Update()
        ea.SaveDiagram(63)
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 63")
        export_and_copy(diag63, "CUN04 DER (Modelo Relacional)", der_dir)

        # DER 65 (CUN05)
        # Column 1: Turnos (top), BloquesHorarios (bottom)
        # Column 2: Usuarios (top), Bitacora (mid), DV (bottom)
        diag65 = ea.GetDiagramByID(65)
        for i in range(diag65.DiagramObjects.Count - 1, -1, -1):
            diag65.DiagramObjects.Delete(i)
        diag65.DiagramObjects.Refresh()

        coords65_clean = [
            (el_tab_turno,  50,  40,  410, 350),
            (el_tab_bloque, 50,  430, 410, 650),
            (el_tab_usr,    520, 40,  880, 350),
            (el_tab_bit,    520, 430, 880, 640),
            (el_tab_dv,     520, 720, 880, 850),
        ]
        for el, left, top, right, bottom in coords65_clean:
            do = diag65.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()
        diag65.DiagramObjects.Refresh()
        diag65.Update()
        ea.SaveDiagram(65)
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 65")
        export_and_copy(diag65, "CUN05 DER (Modelo Relacional)", der_dir)

        # =====================================================================
        # 3. POLISH DIAGRAM 68 (Proceso de Negocio 1 General)
        # =====================================================================
        print("Polishing Diagram 68 (PN1 General Class Diagram)...")
        diag68 = ea.GetDiagramByID(68)
        el_pac_dom = ea.GetElementByID(417)
        el_tur_dom = ea.GetElementByID(418)
        el_blo_dom = ea.GetElementByID(425)
        el_age_dom = ea.GetElementByID(424)
        el_usr_dom = ea.GetElementByID(426)

        el_state_iface = ea.GetElementByID(419)
        el_state_sol = ea.GetElementByID(420)
        el_state_conf = ea.GetElementByID(421)
        el_state_asis = ea.GetElementByID(422)
        el_state_canc = ea.GetElementByID(423)

        for i in range(diag68.DiagramObjects.Count - 1, -1, -1):
            diag68.DiagramObjects.Delete(i)
        diag68.DiagramObjects.Refresh()

        # Layout for Diagram 68 with ample space for state classes:
        # Row 1: Core Domain Entities
        # Row 2: State Interface
        # Row 3: 4 Concrete State classes with 520px width each
        coords68_polish = [
            # Row 1 & Domain Cycle
            (el_pac_dom,     50,   40,   450,  380),
            (el_tur_dom,     520,  40,   1020, 850),
            (el_usr_dom,     1100, 40,   1480, 420),
            (el_age_dom,     1560, 40,   1980, 400),
            (el_blo_dom,     1560, 470,  1920, 770),

            # Row 2: State Interface (under Turno)
            (el_state_iface, 600,  930,  940,  1060),

            # Row 3: 4 States (width ~490px each, zero overlaps)
            (el_state_sol,   50,   1140, 540,  1280),
            (el_state_conf,  570,  1140, 1060, 1280),
            (el_state_asis,  1090, 1140, 1580, 1280),
            (el_state_canc,  1610, 1140, 2100, 1280),
        ]

        for el, left, top, right, bottom in coords68_polish:
            do = diag68.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag68.DiagramObjects.Refresh()
        diag68.Update()
        ea.SaveDiagram(68)
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 68")
        export_and_copy(diag68, "Proceso de Negocio 1 General", dc_dir)

        print("\nVisual polish completed successfully!")

    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == '__main__':
    polish()
