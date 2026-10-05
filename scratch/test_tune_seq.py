import win32com.client
import os
import shutil
import uuid

def test_tune_seq():
    eap_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP'
    scratch_dir = r'c:\Users\Danie\Desktop\GIT\TD\scratch'
    artifact_dir = r'C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e'

    print("Connecting to Enterprise Architect...")
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    try:
        diag56 = ea.GetDiagramByID(56)
        diag56.Name = "CUN02: Diagrama de Secuencia"
        diag56.Notes = "Diagrama de Secuencia de CUN02 (Registrar Paciente Pediátrico): UI valida campos en memoria, BLL orquesta reglas y verificación de duplicados, creación dinámica de PacienteBE_DNI101, DAL persiste y retorna datos crudos, auditoría en BitacoraBLL y recálculo de integridad en DigitoVerificadorBLL. Retornos limpios y sin texto (PDATA4='1')."
        diag56.Update()

        # Clean existing connectors for Diagram 56
        ea.Execute("DELETE FROM t_diagramlinks WHERE DiagramID = 56")
        ea.Execute("DELETE FROM t_connector WHERE DiagramID = 56")

        el_nutri = ea.GetElementByID(677)
        el_gui = ea.GetElementByID(766)
        el_pac_bll = ea.GetElementByID(679)
        el_pac_be = ea.GetElementByID(680)
        el_bit_bll = ea.GetElementByID(682)
        if el_bit_bll.Name != "BitacoraBLL":
            el_bit_bll.Name = "BitacoraBLL"
            el_bit_bll.Update()

        el_dv_bll = ea.GetElementByID(681)
        el_dal = ea.GetElementByID(683)
        if el_dal.Name != "DAL":
            el_dal.Name = "DAL"
            el_dal.Update()

        el_frag = ea.GetElementByID(736)

        lifelines_layout = [
            (el_nutri,   20,   120,  -50,  -980),
            (el_gui,     150,  250,  -50,  -980),
            (el_pac_bll, 290,  430,  -50,  -980),
            (el_pac_be,  660,  800,  -435, -980), # Dynamic lifeline top at -435
            (el_bit_bll, 880,  1020, -50,  -980),
            (el_dv_bll,  1080, 1220, -50,  -980),
            (el_dal,     1280, 1420, -50,  -980),
        ]

        # Reset diagram objects in Diagram 56
        for i in range(diag56.DiagramObjects.Count - 1, -1, -1):
            diag56.DiagramObjects.Delete(i)
        diag56.DiagramObjects.Refresh()

        coords_x = {}
        for el, left, right, top, bottom in lifelines_layout:
            do = diag56.DiagramObjects.AddNew(f"l={left};r={right};t={abs(top)};b={abs(bottom)};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = top
            do.bottom = bottom
            do.Update()
            coords_x[el.ElementID] = (left + right) // 2

        # Configure Fragment 736 (alt)
        # RectTop = -270, RectBottom = -805 (Total height = 535)
        # Partition 1 (top): DNI Duplicado -> height from top = 135
        # Partition 2 (bottom): DNI No Duplicado -> height = 400
        do_frag = diag56.DiagramObjects.AddNew("l=10;r=1440;t=270;b=805;", "")
        do_frag.ElementID = el_frag.ElementID
        do_frag.left = 10
        do_frag.right = 1440
        do_frag.top = -270
        do_frag.bottom = -805
        do_frag.Sequence = 1
        do_frag.Update()

        guid1 = str(uuid.uuid4()).upper()
        guid2 = str(uuid.uuid4()).upper()
        # In EA, bottom partition is listed first with its size, top partition is listed second!
        part_desc = f"@PAR;Name=[DNI No Duplicado: pacienteExistente == null];Size=400;GUID={{{guid2}}};@ENDPAR;@PAR;Name=[DNI Duplicado: pacienteExistente != null];Size=135;GUID={{{guid1}}};@ENDPAR;"
        ea.Execute(f"DELETE FROM t_xref WHERE Client = '{el_frag.ElementGUID}' AND Name = 'Partitions'")
        ea.Execute(f"INSERT INTO t_xref (XrefID, Name, Type, Visibility, Partition, Description, Client) VALUES ('{{{str(uuid.uuid4()).upper()}}}', 'Partitions', 'element property', 'Public', '0', '{part_desc}', '{el_frag.ElementGUID}')")

        diag56.DiagramObjects.Refresh()

        seq_messages = [
            # 1. Nutricionista presiona botón Registrar Paciente
            (1, el_nutri, el_gui, "btnRegistrarPaciente_Click()", "SynchCall", -110, "Synchronous", "", "Call", ""),
            # 2. UI valida campos obligatorios en memoria
            (2, el_gui, el_gui, "ValidarCamposObligatorios()", "SynchCall", -145, "Synchronous", "", "Call", ""),
            # 3. UI invoca BLL
            (3, el_gui, el_pac_bll, "RegistrarPaciente(nombre, apellido, dniNiño, telefono, email, obraSocial)", "SynchCall", -180, "Synchronous", "", "Call", ""),
            # 4. BLL consulta duplicados en DAL
            (4, el_pac_bll, el_dal, "ObtenerPacientePorDNI(dniNiño)", "SynchCall", -215, "Synchronous", "", "Call", ""),
            # 5. DAL retorna a BLL
            (5, el_dal, el_pac_bll, "", "Return", -245, "Synchronous", "", "Call", "1"),

            # --- ALT TOP PARTITION: [DNI Duplicado: pacienteExistente != null] (-270 a -405) ---
            # 6. BLL retorna error/excepción a UI
            (6, el_pac_bll, el_gui, "", "Return", -295, "Synchronous", "", "Call", "1"),
            # 7. UI muestra mensaje de duplicado
            (7, el_gui, el_nutri, "MostrarMensaje(msg_dni_duplicado)", "SynchCall", -330, "Synchronous", "", "Call", ""),
            # 8. Retorno a UI
            (8, el_nutri, el_gui, "", "Return", -365, "Synchronous", "", "Call", "1"),

            # --- ALT BOTTOM PARTITION: [DNI No Duplicado: pacienteExistente == null] (-405 a -805) ---
            # 9. Creación dinámica de PacienteBE_DNI101 (arrow lands on top of BE box at -435)
            (9, el_pac_bll, el_pac_be, "new PacienteBE_DNI101(nombre, apellido, dniNiño, tel, email, obraSocial)", "New", -435, "Synchronous", "", "Call", ""),
            # 10. BLL persiste en DAL con datos escalares
            (10, el_pac_bll, el_dal, "Guardar(dniNiño, nombre, apellido, tel, email, fechaNac, sexo, os, dv)", "SynchCall", -475, "Synchronous", "", "Call", ""),
            # 11. DAL retorna ID generado a BLL
            (11, el_dal, el_pac_bll, "", "Return", -510, "Synchronous", "", "Call", "1"),
            # 12. BLL registra en Bitácora
            (12, el_pac_bll, el_bit_bll, "RegistrarBitacora(1, Registro de Paciente Pediátrico..., dniActual, TurneroNutricional)", "SynchCall", -545, "Synchronous", "", "Call", ""),
            # 13. BitacoraBLL persiste en DAL
            (13, el_bit_bll, el_dal, "GuardarBitacora(bitacora)", "SynchCall", -580, "Synchronous", "", "Call", ""),
            # 14. DAL retorna a BitacoraBLL
            (14, el_dal, el_bit_bll, "", "Return", -610, "Synchronous", "", "Call", "1"),
            # 15. BitacoraBLL retorna a PacienteBLL
            (15, el_bit_bll, el_pac_bll, "", "Return", -640, "Synchronous", "", "Call", "1"),
            # 16. BLL solicita recálculo de integridad a DigitoVerificadorBLL
            (16, el_pac_bll, el_dv_bll, "RecalcularYPersistir()", "SynchCall", -675, "Synchronous", "", "Call", ""),
            # 17. DigitoVerificadorBLL persiste en DAL
            (17, el_dv_bll, el_dal, "PersistirIntegridadCompleta(resumen, filasDV)", "SynchCall", -710, "Synchronous", "", "Call", ""),
            # 18. DAL retorna a DigitoVerificadorBLL
            (18, el_dal, el_dv_bll, "", "Return", -740, "Synchronous", "", "Call", "1"),
            # 19. DigitoVerificadorBLL retorna a PacienteBLL
            (19, el_dv_bll, el_pac_bll, "", "Return", -770, "Synchronous", "", "Call", "1"),

            # --- POST-FRAGMENT: Retorno exitoso a UI y Usuario ---
            # 20. BLL retorna a UI
            (20, el_pac_bll, el_gui, "", "Return", -830, "Synchronous", "", "Call", "1"),
            # 21. UI muestra mensaje de confirmación
            (21, el_gui, el_nutri, "MostrarMensaje(msg_paciente_registrado_det)", "SynchCall", -865, "Synchronous", "", "Call", ""),
            # 22. Retorno de confirmación a UI
            (22, el_nutri, el_gui, "", "Return", -895, "Synchronous", "", "Call", "1"),
            # 23. UI se cierra
            (23, el_gui, el_gui, "Close()", "SynchCall", -925, "Synchronous", "", "Call", "")
        ]

        print(f"Inserting {len(seq_messages)} connectors into Diagram 56...")
        for item in seq_messages:
            seq, src, dst, msg_name, subtype, y, p1, p2, p3, p4 = item
            clean_name = msg_name.replace("'", "''")
            conn = src.Connectors.AddNew(clean_name, "Sequence")
            conn.SupplierID = dst.ElementID
            conn.SequenceNo = seq
            conn.SubType = subtype
            conn.DiagramID = 56
            conn.Update()
            src.Connectors.Refresh()

            cid = conn.ConnectorID
            sx = coords_x.get(src.ElementID, 100)
            ex = coords_x.get(dst.ElementID, 200)

            if src.ElementID == dst.ElementID:
                ex = sx + 25
                ey = y - 15
            else:
                ey = y

            p1_sql = p1.replace("'", "''")
            p2_sql = p2.replace("'", "''")
            p3_sql = p3.replace("'", "''")
            p4_sql = p4.replace("'", "''")

            sql_update = f"""
            UPDATE t_connector 
            SET PtStartX = {sx}, PtEndX = {ex}, PtStartY = {y}, PtEndY = {ey},
                PDATA1 = '{p1_sql}', PDATA2 = '{p2_sql}', PDATA3 = '{p3_sql}', PDATA4 = '{p4_sql}',
                RouteStyle = 1, LineColor = -1
            WHERE Connector_ID = {cid}
            """
            ea.Execute(sql_update)

        diag56.Update()
        diag56.DiagramObjects.Refresh()
        ea.SaveDiagram(56)

        out_dss1 = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\DSS\CUN02 Registrar Paciente.png'
        out_dss2 = os.path.join(scratch_dir, "cun02_sequence.png")
        out_dss3 = os.path.join(artifact_dir, "cun02_sequence.png")
        proj.PutDiagramImageToFile(diag56.DiagramGUID, out_dss1, 1)
        shutil.copy2(out_dss1, out_dss2)
        shutil.copy2(out_dss1, out_dss3)
        print("Diagram 56 re-exported successfully to:", out_dss1)

    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == '__main__':
    test_tune_seq()
