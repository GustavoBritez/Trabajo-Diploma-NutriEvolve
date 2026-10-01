import win32com.client

def rebuild_cun02():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    try:
        print("--- Rebuilding CUN02: Diagrama de Secuencia ---")
        diag2 = ea.GetDiagramByID(56)
        diag2.Name = "CUN02: Diagrama de Secuencia"
        diag2.Update()
        diag2_id = 56

        uc38 = ea.GetElementByID(38)

        # Clear old connectors
        ea.Execute(f"DELETE FROM t_connector WHERE DiagramID = {diag2_id}")

        # Lifelines in UC 38:
        el_nutri2 = ea.GetElementByID(677)
        el_ui2 = ea.GetElementByID(678)
        el_bll2 = ea.GetElementByID(679)
        el_be2 = ea.GetElementByID(680)
        el_bit2 = ea.GetElementByID(682)
        el_dv2 = ea.GetElementByID(681)
        el_dal2 = ea.GetElementByID(683)
        el_dal2.Name = "DAL"
        el_dal2.Update()

        # Fragment for CUN02 (Only 1 fragment: DNI Duplicado)
        def create_or_get_element(parent, name, el_type, subtype=0):
            for el in parent.Elements:
                if el.Name == name and el.Type == el_type:
                    el.Subtype = subtype
                    el.Update()
                    return el
            new_el = parent.Elements.AddNew(name, el_type)
            new_el.Subtype = subtype
            new_el.Update()
            parent.Elements.Refresh()
            return new_el

        frag2_dni = create_or_get_element(uc38, "[DNI Duplicado: pacienteExistente != null]", "InteractionFragment", subtype=1)

        lifeline_bottom2 = -820
        # (ElementID, Left, Right, CenterX)
        lifelines2_info = [
            (el_nutri2.ElementID,  20,   120,  70),
            (el_ui2.ElementID,     180,  320,  250),
            (el_bll2.ElementID,    380,  520,  450),
            (el_be2.ElementID,     580,  720,  650),
            (el_bit2.ElementID,    780,  920,  850),
            (el_dv2.ElementID,     980,  1120, 1050),
            (el_dal2.ElementID,    1180, 1300, 1240),
            # Fragment box:
            (frag2_dni.ElementID,  15,   1320, 0)
        ]

        ea.Execute(f"DELETE FROM t_diagramobjects WHERE Diagram_ID = {diag2_id}")
        diag2.DiagramObjects.Refresh()

        for idx, item in enumerate(lifelines2_info):
            eid = item[0]
            l = item[1]
            r = item[2]
            if eid == frag2_dni.ElementID:
                d_obj = diag2.DiagramObjects.AddNew(f"l={l};r={r};t=-225;b=-305", "")
                d_obj.ElementID = eid
                d_obj.Sequence = 1
                d_obj.Update()
            else:
                d_obj = diag2.DiagramObjects.AddNew(f"l={l};r={r};t=-50;b={lifeline_bottom2}", "")
                d_obj.ElementID = eid
                d_obj.Sequence = idx + 2
                d_obj.Update()

        diag2.DiagramObjects.Refresh()
        diag2.Update()

        centers2 = {eid: cx for eid, l, r, cx in lifelines2_info if cx > 0}

        msg2_defs = [
            # 1. Nutricionista presiona Registrar Paciente en UI
            (1,  el_nutri2.ElementID, el_ui2.ElementID, "btnRegistrarPaciente_Click(sender, e)", "SynchCall", False),

            # 2. Llamada directa a RegistrarPaciente en BLL
            (2,  el_ui2.ElementID, el_bll2.ElementID, "RegistrarPaciente(nombre, apellido, dniNiño, telefono, email, obraSocial)", "SynchCall", False),

            # 3. Consulta de duplicados en DAL
            (3,  el_bll2.ElementID, el_dal2.ElementID, "ObtenerPacientePorDNI(dniNiño)", "SynchCall", False),
            (4,  el_dal2.ElementID, el_bll2.ElementID, "", "Return", False),

            # 4. Alternancia: [DNI Duplicado: pacienteExistente != null] (Inside Frag)
            (5,  el_bll2.ElementID, el_ui2.ElementID, "throw InvalidOperationException('El paciente con DNI ya se encuentra registrado')", "Return", False),
            (6,  el_ui2.ElementID, el_nutri2.ElementID, "MostrarError('El paciente con DNI ya se encuentra registrado')", "SynchCall", False),

            # 5. Flujo Normal de Creación y Persistencia (Below Frag)
            (7,  el_bll2.ElementID, el_be2.ElementID, "new PacienteBE_DNI101(nombre, apellido, dniNiño, telefono, email, obraSocial)", "SynchCall", False),
            (8,  el_bll2.ElementID, el_dal2.ElementID, "Guardar(nuevoPaciente)", "SynchCall", False),
            (9,  el_dal2.ElementID, el_bll2.ElementID, "", "Return", False),

            # 6. Auditoría y Dígitos Verificadores
            (10, el_bll2.ElementID, el_bit2.ElementID, "RegistrarEvento(1, 'Registro de Paciente Pediátrico...', dniActual, 'TurneroNutricional')", "SynchCall", False),
            (11, el_bit2.ElementID, el_dal2.ElementID, "GuardarBitacora(evento)", "SynchCall", False),
            (12, el_bll2.ElementID, el_dv2.ElementID, "RecalcularYPersistir()", "SynchCall", False),
            (13, el_dv2.ElementID, el_dal2.ElementID, "ActualizarDV(firmas)", "SynchCall", False),

            # 7. Retorno a UI y Notificación Exitosa
            (14, el_bll2.ElementID, el_ui2.ElementID, "", "Return", False),
            (15, el_ui2.ElementID, el_nutri2.ElementID, "MessageBox.Show('El paciente fue registrado exitosamente')", "SynchCall", False),
            (16, el_ui2.ElementID, el_ui2.ElementID, "Close()", "SynchCall", True),
            (17, el_ui2.ElementID, el_nutri2.ElementID, "", "Return", False)
        ]

        y_coords2 = [
            -80,  # 1: btnRegistrarPaciente_Click
            -125, # 2: RegistrarPaciente
            -170, # 3: ObtenerPacientePorDNI
            -205, # 4: Return
            # Inside Frag (-225 to -335):
            -260, # 5: throw InvalidOperationException
            -300, # 6: MostrarError
            # Below Frag (clean gap!):
            -380, # 7: new PacienteBE_DNI101
            -420, # 8: Guardar
            -455, # 9: Return
            -495, # 10: RegistrarEvento
            -530, # 11: GuardarBitacora
            -570, # 12: RecalcularYPersistir
            -605, # 13: ActualizarDV
            -645, # 14: Return to UI
            -685, # 15: MessageBox.Show
            -725, # 16: Close (self)
            -765  # 17: Return to Nutri
        ]

        for idx, (seq, s_id, r_id, m_name, stype, is_self) in enumerate(msg2_defs):
            sy = y_coords2[idx]
            sx = centers2[s_id]
            if is_self:
                ex = sx + 30
                ey = sy - 20
            else:
                ex = centers2[r_id]
                ey = sy

            sender_el = ea.GetElementByID(s_id)
            conn = sender_el.Connectors.AddNew(m_name, "Sequence")
            conn.SupplierID = r_id
            conn.SubType = stype
            conn.Update()

            escaped_name = m_name.replace("'", "''") if m_name else ""
            sql = f"UPDATE t_connector SET DiagramID = {diag2_id}, SeqNo = {seq}, Name = '{escaped_name}', PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey} WHERE Connector_ID = {conn.ConnectorID}"
            ea.Execute(sql)

        diag2.Update()
        print("CUN02 Diagram updated successfully.")
        out_img2 = r"C:\Users\Danie\Desktop\GIT\TD\scratch\cun02_secuencia.png"
        proj.PutDiagramImageToFile(diag2.DiagramGUID, out_img2, 1)
        print("Exported CUN02 image to:", out_img2)

    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    rebuild_cun02()
