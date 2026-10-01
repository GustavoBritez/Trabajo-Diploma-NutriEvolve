import win32com.client

def build_cun03():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    try:
        print("--- Building CUN03: Diagrama de Secuencia ---")
        diag = ea.GetDiagramByID(13)
        diag.Name = "CUN03: Diagrama de Secuencia"
        diag.Update()
        diag_id = 13

        uc = ea.GetElementByID(13)

        # Clear existing connectors for Diagram 13
        ea.Execute("DELETE FROM t_connector WHERE DiagramID = 13")

        # 1. Nutricionista (Actor) - ID 266
        el_nutri = ea.GetElementByID(266)

        # 2. FrmReprogramarTurno_DNI101 (Object) - ID 719
        el_reprog = ea.GetElementByID(719)
        el_reprog.Name = "FrmReprogramarTurno_DNI101"
        el_reprog.Update()

        # 3. UsuarioBLL (Object) under UC 13
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

        el_usuario_bll = create_or_get_element(uc, "UsuarioBLL", "Object")

        # 4. AgendaMedicaBLL_DNI101 (Object) - ID 269
        el_agenda_bll = ea.GetElementByID(269)

        # 5. TurnoBLL_DNI101 (Object) - ID 268
        el_turno_bll = ea.GetElementByID(268)

        # 6. TurnoBE_DNI101 (Object) - ID 270
        el_be = ea.GetElementByID(270)

        # 7. TurnoConfirmadoState_DNI101 (Object) - ID 271
        el_state = ea.GetElementByID(271)

        # 8. EventoBLL (Object) - ID 274
        el_bitacora = ea.GetElementByID(274)

        # 9. DigitoVerificadorBLL (Object) - ID 273
        el_dv = ea.GetElementByID(273)

        # 10. DAL (Object) - ID 272
        el_dal = ea.GetElementByID(272)
        el_dal.Name = "DAL"
        el_dal.Update()

        # Interaction Fragments for CUN03:
        frag1_nobloques = create_or_get_element(uc, "[Flujo 6.1: Sin bloques disponibles]", "InteractionFragment", subtype=1)
        frag2_nohorario = create_or_get_element(uc, "[Flujo 6.1b: Horario no seleccionado]", "InteractionFragment", subtype=1)
        frag3_noestado = create_or_get_element(uc, "[Flujo 10.1: Estado no permite reprogramación (Asistió/Cancelado)]", "InteractionFragment", subtype=1)
        frag4_superposicion = create_or_get_element(uc, "[Superposición de Turno: ExisteTurnoParaProfesional]", "InteractionFragment", subtype=1)

        lifeline_bottom = -1850
        # (ElementID, Left, Right, CenterX)
        lifelines_info = [
            (el_nutri.ElementID,       20,   120,  70),
            (el_reprog.ElementID,      160,  320,  240),
            (el_usuario_bll.ElementID, 360,  480,  420),
            (el_agenda_bll.ElementID,  520,  660,  590),
            (el_turno_bll.ElementID,   700,  840,  770),
            (el_be.ElementID,          880,  1000, 940),
            (el_state.ElementID,       1040, 1200, 1120),
            (el_bitacora.ElementID,    1240, 1360, 1300),
            (el_dv.ElementID,          1400, 1540, 1470),
            (el_dal.ElementID,         1580, 1700, 1640),
            # Fragments
            (frag1_nobloques.ElementID,    15, 1680, 0),
            (frag2_nohorario.ElementID,    15, 1680, 0),
            (frag3_noestado.ElementID,     15, 1680, 0),
            (frag4_superposicion.ElementID, 15, 1680, 0)
        ]

        ea.Execute("DELETE FROM t_diagramobjects WHERE Diagram_ID = 13")
        diag.DiagramObjects.Refresh()

        for idx, item in enumerate(lifelines_info):
            eid = item[0]
            l = item[1]
            r = item[2]
            if eid == frag1_nobloques.ElementID:
                d_obj = diag.DiagramObjects.AddNew(f"l={l};r={r};t=-460;b=-520", "")
                d_obj.ElementID = eid
                d_obj.Sequence = 1
                d_obj.Update()
            elif eid == frag2_nohorario.ElementID:
                d_obj = diag.DiagramObjects.AddNew(f"l={l};r={r};t=-585;b=-675", "")
                d_obj.ElementID = eid
                d_obj.Sequence = 1
                d_obj.Update()
            elif eid == frag3_noestado.ElementID:
                d_obj = diag.DiagramObjects.AddNew(f"l={l};r={r};t=-805;b=-925", "")
                d_obj.ElementID = eid
                d_obj.Sequence = 1
                d_obj.Update()
            elif eid == frag4_superposicion.ElementID:
                d_obj = diag.DiagramObjects.AddNew(f"l={l};r={r};t=-1020;b=-1140", "")
                d_obj.ElementID = eid
                d_obj.Sequence = 1
                d_obj.Update()
            else:
                d_obj = diag.DiagramObjects.AddNew(f"l={l};r={r};t=-50;b={lifeline_bottom}", "")
                d_obj.ElementID = eid
                d_obj.Sequence = idx + 2
                d_obj.Update()

        diag.DiagramObjects.Refresh()
        diag.Update()

        centers = {eid: cx for eid, l, r, cx in lifelines_info if cx > 0}

        msg_defs = [
            # 1. Nutricionista abre la pantalla -> FrmReprogramarTurno_DNI101_Load
            (1,  el_nutri.ElementID, el_reprog.ElementID, "FrmReprogramarTurno_DNI101_Load(sender, e)", "SynchCall", False),
            (2,  el_reprog.ElementID, el_usuario_bll.ElementID, "ListarUsuarios()", "SynchCall", False),
            (3,  el_usuario_bll.ElementID, el_dal.ElementID, "ListarUsuarios()", "SynchCall", False),
            (4,  el_dal.ElementID, el_usuario_bll.ElementID, "", "Return", False),
            (5,  el_usuario_bll.ElementID, el_reprog.ElementID, "", "Return", False),

            # 2. CargarBloquesDisponibles()
            (6,  el_reprog.ElementID, el_reprog.ElementID, "CargarBloquesDisponibles()", "SynchCall", True),
            (7,  el_reprog.ElementID, el_agenda_bll.ElementID, "ListarBloquesDisponibles(fecha, dniNutricionista)", "SynchCall", False),
            (8,  el_agenda_bll.ElementID, el_dal.ElementID, "ListarBloquesDisponibles(fecha, dniNutricionista)", "SynchCall", False),
            (9,  el_dal.ElementID, el_agenda_bll.ElementID, "", "Return", False),
            (10, el_agenda_bll.ElementID, el_dal.ElementID, "ObtenerHorariosOcupadosPorProfesional(dniNutri, fecha)", "SynchCall", False),
            (11, el_dal.ElementID, el_agenda_bll.ElementID, "", "Return", False),
            (12, el_agenda_bll.ElementID, el_reprog.ElementID, "", "Return", False),

            # 3. Frag 1: [Flujo 6.1: Sin bloques disponibles]
            (13, el_reprog.ElementID, el_nutri.ElementID, "MostrarAlerta('⚠ Sin bloques disponibles para el profesional en la fecha')", "SynchCall", False),

            # 4. Confirmación por el Nutricionista
            (14, el_nutri.ElementID, el_reprog.ElementID, "btnConfirmar_Click(sender, e)", "SynchCall", False),

            # 5. Frag 2: [Flujo 6.1b: Horario no seleccionado]
            (15, el_reprog.ElementID, el_nutri.ElementID, "MostrarAdvertencia('No hay un bloque horario disponible seleccionado')", "SynchCall", False),
            (16, el_reprog.ElementID, el_reprog.ElementID, "cmbNuevoHorario.Focus()", "SynchCall", True),

            # 6. Invocación BLL
            (17, el_reprog.ElementID, el_turno_bll.ElementID, "ReprogramarTurno(codigoTurno, nuevaFecha, nuevaHora, nuevoIdBloque, dniNutri)", "SynchCall", False),
            (18, el_turno_bll.ElementID, el_dal.ElementID, "ObtenerPorCodigo(codigoTurno)", "SynchCall", False),
            (19, el_dal.ElementID, el_turno_bll.ElementID, "", "Return", False),

            # 7. Frag 3: [Flujo 10.1: Estado no permite reprogramación (Asistió/Cancelado)]
            (20, el_turno_bll.ElementID, el_reprog.ElementID, "throw InvalidOperationException('El turno se encuentra en estado no modificable')", "Return", False),
            (21, el_reprog.ElementID, el_nutri.ElementID, "MostrarError('El turno no permite reprogramación')", "SynchCall", False),
            (22, el_reprog.ElementID, el_reprog.ElementID, "CargarBloquesDisponibles()", "SynchCall", True),

            # 8. Anti-Superposición
            (23, el_turno_bll.ElementID, el_dal.ElementID, "ExisteTurnoParaProfesional(dniNutri, nuevaFecha, nuevaHora)", "SynchCall", False),
            (24, el_dal.ElementID, el_turno_bll.ElementID, "", "Return", False),

            # 9. Frag 4: [Superposición: ExisteTurnoParaProfesional]
            (25, el_turno_bll.ElementID, el_reprog.ElementID, "throw InvalidOperationException('El profesional ya cuenta con un turno asignado')", "Return", False),
            (26, el_reprog.ElementID, el_nutri.ElementID, "MostrarError('El profesional ya cuenta con un turno asignado')", "SynchCall", False),
            (27, el_reprog.ElementID, el_reprog.ElementID, "CargarBloquesDisponibles()", "SynchCall", True),

            # 10. Patrón State y DV (Flujo Normal)
            (28, el_turno_bll.ElementID, el_be.ElementID, "Reprogramar(nuevaFecha, nuevaHora, nuevoIdBloque)", "SynchCall", False),
            (29, el_be.ElementID, el_state.ElementID, "Reprogramar(this, nuevaFecha, nuevaHora, nuevoIdBloque)", "SynchCall", False),
            (30, el_state.ElementID, el_be.ElementID, "", "Return", False),
            (31, el_be.ElementID, el_turno_bll.ElementID, "", "Return", False),
            (32, el_turno_bll.ElementID, el_turno_bll.ElementID, "CalcularDV(cadenaDV)", "SynchCall", True),

            # 11. Persistencia y Liberación de Bloques en DAL
            (33, el_turno_bll.ElementID, el_dal.ElementID, "ReprogramarTurno(idTurno, nuevaFecha, nuevaHora, nuevoIdBloque, dniNutri, dv)", "SynchCall", False),
            (34, el_dal.ElementID, el_turno_bll.ElementID, "", "Return", False),
            (35, el_turno_bll.ElementID, el_dal.ElementID, "ActualizarEstadoBloque(idBloqueAnterior, 'Disponible')", "SynchCall", False),
            (36, el_dal.ElementID, el_turno_bll.ElementID, "", "Return", False),
            (37, el_turno_bll.ElementID, el_dal.ElementID, "ActualizarEstadoBloque(nuevoIdBloque, 'Ocupado')", "SynchCall", False),
            (38, el_dal.ElementID, el_turno_bll.ElementID, "", "Return", False),

            # 12. Auditoría y Dígito Verificador Global
            (39, el_turno_bll.ElementID, el_bitacora.ElementID, "RegistrarEvento(2, 'Turno reprogramado...', dniActual, 'TurneroNutricional')", "SynchCall", False),
            (40, el_bitacora.ElementID, el_dal.ElementID, "GuardarBitacora(evento)", "SynchCall", False),
            (41, el_turno_bll.ElementID, el_dv.ElementID, "RecalcularYPersistir()", "SynchCall", False),
            (42, el_dv.ElementID, el_dal.ElementID, "ActualizarDV(firmas)", "SynchCall", False),

            # 13. Retorno y Confirmación
            (43, el_turno_bll.ElementID, el_reprog.ElementID, "", "Return", False),
            (44, el_reprog.ElementID, el_nutri.ElementID, "MessageBox.Show('¡El turno ha sido reprogramado exitosamente!')", "SynchCall", False),
            (45, el_reprog.ElementID, el_reprog.ElementID, "Close()", "SynchCall", True),
            (46, el_reprog.ElementID, el_nutri.ElementID, "", "Return", False)
        ]

        y_coords = [
            -80,   # 1 FrmReprogramarTurno_DNI101_Load
            -115,  # 2 ListarUsuarios
            -145,  # 3 ListarUsuarios (DAL)
            -175,  # 4 return
            -205,  # 5 return to UI
            -245,  # 6 CargarBloquesDisponibles (self)
            -285,  # 7 ListarBloquesDisponibles
            -315,  # 8 ListarBloquesDisponibles (DAL)
            -345,  # 9 return
            -375,  # 10 ObtenerHorariosOcupadosPorProfesional (DAL)
            -405,  # 11 return
            -435,  # 12 return to UI
            # Frag 1 (-460 to -520)
            -485,  # 13 MostrarAlerta
            # Below Frag 1
            -560,  # 14 btnConfirmar_Click
            # Frag 2 (-585 to -675)
            -615,  # 15 MostrarAdvertencia
            -650,  # 16 cmbNuevoHorario.Focus (self)
            # Below Frag 2
            -720,  # 17 ReprogramarTurno
            -755,  # 18 ObtenerPorCodigo
            -785,  # 19 return
            # Frag 3 (-805 to -925)
            -835,  # 20 throw InvalidOperationException
            -865,  # 21 MostrarError
            -895,  # 22 CargarBloquesDisponibles (self)
            # Below Frag 3
            -970,  # 23 ExisteTurnoParaProfesional
            -1000, # 24 return
            # Frag 4 (-1020 to -1140)
            -1050, # 25 throw InvalidOperationException
            -1080, # 26 MostrarError
            -1110, # 27 CargarBloquesDisponibles (self)
            # Below Frag 4 (Normal flow)
            -1190, # 28 Reprogramar (BE)
            -1225, # 29 Reprogramar (State)
            -1255, # 30 return
            -1285, # 31 return
            -1320, # 32 CalcularDV (self)
            -1360, # 33 ReprogramarTurno (DAL)
            -1390, # 34 return
            -1420, # 35 ActualizarEstadoBloque (Disponible)
            -1450, # 36 return
            -1480, # 37 ActualizarEstadoBloque (Ocupado)
            -1510, # 38 return
            -1545, # 39 RegistrarEvento
            -1575, # 40 GuardarBitacora
            -1610, # 41 RecalcularYPersistir
            -1640, # 42 ActualizarDV
            -1675, # 43 return to UI
            -1710, # 44 MessageBox.Show
            -1745, # 45 Close (self)
            -1780  # 46 return to Nutri
        ]

        for idx, (seq, s_id, r_id, m_name, stype, is_self) in enumerate(msg_defs):
            sy = y_coords[idx]
            sx = centers[s_id]
            if is_self:
                ex = sx + 30
                ey = sy - 20
            else:
                ex = centers[r_id]
                ey = sy

            sender_el = ea.GetElementByID(s_id)
            conn = sender_el.Connectors.AddNew(m_name, "Sequence")
            conn.SupplierID = r_id
            conn.SubType = stype
            conn.Update()

            escaped_name = m_name.replace("'", "''") if m_name else ""
            sql = f"UPDATE t_connector SET DiagramID = {diag_id}, SeqNo = {seq}, Name = '{escaped_name}', PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey} WHERE Connector_ID = {conn.ConnectorID}"
            ea.Execute(sql)

        diag.Update()
        print("Diagram 13 (CUN03) updated successfully.")

        # Export image
        out_img = r"C:\Users\Danie\Desktop\GIT\TD\scratch\cun03_secuencia.png"
        proj.PutDiagramImageToFile(diag.DiagramGUID, out_img, 1)
        print("Exported CUN03 image to:", out_img)

    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    build_cun03()
