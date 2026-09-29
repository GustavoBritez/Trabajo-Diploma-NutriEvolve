import win32com.client
import os

def create_or_get_element(ea, package_id, parent_id, name, elem_type):
    # Check if element exists under parent_id
    sql = f"SELECT Object_ID FROM t_object WHERE ParentID = {parent_id} AND Name = '{name}' AND Object_Type = '{elem_type}'"
    xml = ea.SQLQuery(sql)
    import xml.etree.ElementTree as ET
    root = ET.fromstring(xml)
    rows = root.findall('.//Row')
    if rows and len(rows) > 0:
        obj_id = int(rows[0].find('Object_ID').text)
        return ea.GetElementByID(obj_id)
    
    # Create new
    pkg = ea.GetPackageByID(package_id)
    el = pkg.Elements.AddNew(name, elem_type)
    el.ParentID = parent_id
    el.Update()
    return el

def create_or_get_diagram(ea, package_id, parent_id, diag_name, diag_type):
    sql = f"SELECT Diagram_ID FROM t_diagram WHERE ParentID = {parent_id} AND Name = '{diag_name}'"
    xml = ea.SQLQuery(sql)
    import xml.etree.ElementTree as ET
    root = ET.fromstring(xml)
    rows = root.findall('.//Row')
    if rows and len(rows) > 0:
        diag_id = int(rows[0].find('Diagram_ID').text)
        diag = ea.GetDiagramByID(diag_id)
        diag.Name = diag_name
        diag.Update()
        return diag
    
    pkg = ea.GetPackageByID(package_id)
    diag = pkg.Diagrams.AddNew(diag_name, diag_type)
    diag.ParentID = parent_id
    diag.Update()
    return diag

def build_all_5_cuns():
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

    # =========================================================================
    # CUN01: Registrar Turno (UseCase ID: 12, Pkg: 4)
    # =========================================================================
    print("Updating CUN01: Diagrama Actividad...")
    diag1 = ea.GetDiagramByID(12)
    diag1.Name = "CUN01:Diagrama Actividad"
    diag1.Update()
    # (Diagram 12 already has the perfect masterpiece layout and connectors from update_cun01_masterpiece.py)

    # =========================================================================
    # CUN02: Registrar Paciente (UseCase ID: 38, Pkg: 4)
    # =========================================================================
    print("Building CUN02: Diagrama Actividad...")
    diag2 = create_or_get_diagram(ea, 4, 38, "CUN02:Diagrama Actividad", "Sequence")
    diag2_id = diag2.DiagramID
    ea.Execute(f"DELETE FROM t_connector WHERE DiagramID = {diag2_id}")

    # Elements for CUN02 under UseCase 38
    el_nutri2 = create_or_get_element(ea, 4, 38, "Nutricionista", "Actor")
    el_ui2 = create_or_get_element(ea, 4, 38, "RegistrarPaciente_DNI101", "Object")
    el_bll2 = create_or_get_element(ea, 4, 38, "PacienteBLL_DNI101", "Object")
    el_be2 = create_or_get_element(ea, 4, 38, "PacienteBE_DNI101", "Object")
    el_dv2 = create_or_get_element(ea, 4, 38, "DigitoVerificadorBLL", "Object")
    el_bit2 = create_or_get_element(ea, 4, 38, "EventoBLL", "Object")
    el_dal2 = create_or_get_element(ea, 4, 38, "DAL", "Object")

    diag2_lifelines = [
        (el_nutri2.ElementID, 30, 130, 80),
        (el_ui2.ElementID, 180, 320, 250),
        (el_bll2.ElementID, 370, 500, 435),
        (el_be2.ElementID, 540, 660, 600),
        (el_dv2.ElementID, 700, 830, 765),
        (el_bit2.ElementID, 870, 970, 920),
        (el_dal2.ElementID, 1010, 1110, 1060)
    ]

    lifeline_bottom2 = -1050
    # Add DiagramObjects
    for idx, (eid, l, r, cx) in enumerate(diag2_lifelines):
        sql_check = f"SELECT Diagram_ID FROM t_diagramobjects WHERE Diagram_ID = {diag2_id} AND Object_ID = {eid}"
        xml_chk = ea.SQLQuery(sql_check)
        import xml.etree.ElementTree as ET
        if not ET.fromstring(xml_chk).findall('.//Row'):
            d_obj = diag2.DiagramObjects.AddNew(f"l={l};r={r};t=-50;b={lifeline_bottom2}", "")
            d_obj.ElementID = eid
            d_obj.Update()
        else:
            ea.Execute(f"UPDATE t_diagramobjects SET RectLeft = {l}, RectRight = {r}, RectTop = -50, RectBottom = {lifeline_bottom2}, Sequence = {idx+1} WHERE Diagram_ID = {diag2_id} AND Object_ID = {eid}")

    diag2.DiagramObjects.Refresh()
    diag2.Update()

    # Messages for CUN02
    msgs2 = [
        (1,  el_nutri2.ElementID, el_ui2.ElementID, "btnGuardar_Click(nombre, apellido, dniNiño, telefono, email, obraSocial)", "SynchCall", 80, -95, 250, -95),
        (2,  el_ui2.ElementID, el_ui2.ElementID, "ValidarCamposObligatorios()", "SynchCall", 250, -130, 280, -150),
        (3,  el_ui2.ElementID, el_bll2.ElementID, "RegistrarPaciente(nombre, apellido, dniNiño, telefono, email, obraSocial)", "SynchCall", 250, -185, 435, -185),
        (4,  el_bll2.ElementID, el_bll2.ElementID, "ValidarReglasNegocio(dniNiño, obraSocial)", "SynchCall", 435, -220, 465, -240),
        (5,  el_bll2.ElementID, el_dal2.ElementID, "ObtenerPacientePorDNI(dniNiño)", "SynchCall", 435, -275, 1060, -275),
        (6,  el_dal2.ElementID, el_bll2.ElementID, "", "Return", 1060, -310, 435, -310),
        (7,  el_bll2.ElementID, el_be2.ElementID, "new PacienteBE_DNI101(nombre, apellido, dniNiño, telefono, email, obraSocial)", "SynchCall", 435, -350, 600, -350),
        (8,  el_bll2.ElementID, el_dal2.ElementID, "Guardar(pacienteBE)", "SynchCall", 435, -395, 1060, -395),
        (9,  el_dal2.ElementID, el_bll2.ElementID, "", "Return", 1060, -430, 435, -430),
        # Digito Verificador - NO RETURN
        (10, el_bll2.ElementID, el_dv2.ElementID, "RecalcularYPersistir()", "SynchCall", 435, -475, 765, -475),
        (11, el_dv2.ElementID, el_dal2.ElementID, "ActualizarDV(firmas)", "SynchCall", 765, -510, 1060, -510),
        # Bitacora - NO RETURN
        (12, el_bll2.ElementID, el_bit2.ElementID, "RegistrarEvento(1, 'Alta Paciente', dniUsuario, 'TurneroNutricional')", "SynchCall", 435, -555, 920, -555),
        (13, el_bit2.ElementID, el_dal2.ElementID, "GuardarBitacora(evento)", "SynchCall", 920, -590, 1060, -590),
        # Return to UI
        (14, el_bll2.ElementID, el_ui2.ElementID, "", "Return", 435, -635, 250, -635),
        (15, el_ui2.ElementID, el_nutri2.ElementID, "MostrarMensajeExito('Paciente registrado correctamente')", "SynchCall", 250, -680, 80, -680)
    ]

    for seq, s_id, r_id, m_name, stype, sx, sy, ex, ey in msgs2:
        sender_el = ea.GetElementByID(s_id)
        conn = sender_el.Connectors.AddNew(m_name, "Sequence")
        conn.SupplierID = r_id
        conn.SubType = stype
        conn.Update()
        sql = f"UPDATE t_connector SET DiagramID = {diag2_id}, SeqNo = {seq}, PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey} WHERE Connector_ID = {conn.ConnectorID}"
        ea.Execute(sql)

    diag2.Update()
    print("CUN02 built successfully.")

    # =========================================================================
    # CUN03: Reprogramar Turno (UseCase ID: 13, Pkg: 4)
    # =========================================================================
    print("Updating CUN03: Diagrama Actividad...")
    diag3 = ea.GetDiagramByID(13)
    diag3.Name = "CUN03:Diagrama Actividad"
    diag3.Update()

    ea.Execute(f"DELETE FROM t_connector WHERE DiagramID = 13")

    # Lifeline elements: 266, 267, 269, 268, 270, 271, 273, 274, 272
    lifeline_bottom3 = -1550
    coords3 = {
        266: (30, 130, -50, lifeline_bottom3, 1),
        267: (170, 290, -50, lifeline_bottom3, 2),
        269: (330, 470, -50, lifeline_bottom3, 3),
        268: (510, 630, -50, lifeline_bottom3, 4),
        270: (670, 790, -50, lifeline_bottom3, 5),
        271: (830, 1010, -50, lifeline_bottom3, 6),
        273: (1050, 1170, -50, lifeline_bottom3, 7),
        274: (1210, 1310, -50, lifeline_bottom3, 8),
        272: (1350, 1450, -50, lifeline_bottom3, 9)
    }

    for d_obj in diag3.DiagramObjects:
        if d_obj.ElementID in coords3:
            l, r, t, b, seq = coords3[d_obj.ElementID]
            d_obj.left = l
            d_obj.right = r
            d_obj.top = t
            d_obj.bottom = b
            d_obj.Sequence = seq
            d_obj.Update()

    diag3.DiagramObjects.Refresh()
    diag3.Update()

    # Messages for CUN03
    msgs3 = [
        # Fase 1: Clic en Reprogramar y Carga inicial
        (1,  266, 267, "btn_Reprogramar_Turno_Click(idTurno)", "SynchCall", 80, -95, 230, -95),
        (2,  267, 268, "ObtenerTurnoPorId(idTurno)", "SynchCall", 230, -135, 570, -135),
        (3,  268, 272, "ObtenerPorId(idTurno)", "SynchCall", 570, -170, 1400, -170),
        (4,  272, 268, "", "Return", 1400, -205, 570, -205),
        (5,  268, 267, "", "Return", 570, -240, 230, -240),
        (6,  267, 267, "AbrirFrmReprogramar(turno)", "SynchCall", 230, -275, 260, -295),
        # Fase 2: Consultar disponibilidad (CUN-07)
        (7,  266, 267, "SeleccionarNuevaFecha(nuevaFecha, dniNutri)", "SynchCall", 80, -330, 230, -330),
        (8,  267, 269, "ListarBloquesDisponibles(nuevaFecha, dniNutri)", "SynchCall", 230, -365, 400, -365),
        (9,  269, 272, "ListarBloquesDisponibles(nuevaFecha, dniNutri)", "SynchCall", 400, -400, 1400, -400),
        (10, 272, 269, "", "Return", 1400, -435, 400, -435),
        (11, 269, 267, "", "Return", 400, -470, 230, -470),
        (12, 267, 267, "CargarSelectorHorariosDisponibles()", "SynchCall", 230, -505, 260, -525),
        # Fase 3: Confirmar reprogramación
        (13, 266, 267, "btnConfirmar_Click(nuevoIdBloque)", "SynchCall", 80, -560, 230, -560),
        (14, 267, 268, "ReprogramarTurno(idTurno, nuevaFecha, nuevaHora, nuevoIdBloque)", "SynchCall", 230, -600, 570, -600),
        # State pattern
        (15, 268, 270, "Reprogramar(nuevaFecha, nuevaHora, nuevoIdBloque)", "SynchCall", 570, -640, 730, -640),
        (16, 270, 271, "Reprogramar(turno, nuevaFecha, nuevaHora, nuevoIdBloque)", "SynchCall", 730, -675, 920, -675),
        (17, 271, 270, "", "Return", 920, -710, 730, -710),
        (18, 270, 268, "", "Return", 730, -745, 570, -745),
        (19, 268, 268, "CalcularDV(cadenaDV)", "SynchCall", 570, -780, 600, -800),
        # Persistencia en DAL
        (20, 268, 272, "ReprogramarTurno(idTurno, nuevaFecha, nuevaHora, nuevoIdBloque, dniNutri, dv)", "SynchCall", 570, -840, 1400, -840),
        (21, 272, 268, "", "Return", 1400, -875, 570, -875),
        # Actualización de bloques
        (22, 268, 272, "ActualizarEstadoBloque(idBloqueAnterior, 'Disponible')", "SynchCall", 570, -915, 1400, -915),
        (23, 272, 268, "", "Return", 1400, -950, 570, -950),
        (24, 268, 272, "ActualizarEstadoBloque(nuevoIdBloque, 'Ocupado')", "SynchCall", 570, -990, 1400, -990),
        (25, 272, 268, "", "Return", 1400, -1025, 570, -1025),
        # DV - NO RETURN
        (26, 268, 273, "RecalcularYPersistir()", "SynchCall", 570, -1065, 1110, -1065),
        (27, 273, 272, "ActualizarDV(firmas)", "SynchCall", 1110, -1100, 1400, -1100),
        # Bitacora - NO RETURN
        (28, 268, 274, "RegistrarEvento(2, 'Turno Reprogramado', dniNutri, 'TurneroNutricional')", "SynchCall", 570, -1140, 1260, -1140),
        (29, 274, 272, "GuardarBitacora(evento)", "SynchCall", 1260, -1175, 1400, -1175),
        # Return to UI
        (30, 268, 267, "", "Return", 570, -1215, 230, -1215),
        (31, 267, 267, "CargarTurnos()", "SynchCall", 230, -1250, 260, -1270),
        (32, 267, 266, "MostrarMensajeExito('Turno reprogramado exitosamente')", "SynchCall", 230, -1310, 80, -1310)
    ]

    for seq, s_id, r_id, m_name, stype, sx, sy, ex, ey in msgs3:
        sender_el = ea.GetElementByID(s_id)
        conn = sender_el.Connectors.AddNew(m_name, "Sequence")
        conn.SupplierID = r_id
        conn.SubType = stype
        conn.Update()
        sql = f"UPDATE t_connector SET DiagramID = 13, SeqNo = {seq}, PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey} WHERE Connector_ID = {conn.ConnectorID}"
        ea.Execute(sql)

    diag3.Update()
    print("CUN03 built successfully.")

    # =========================================================================
    # CUN04: Modificar Turno (UseCase ID: 39, Pkg: 4)
    # =========================================================================
    print("Building CUN04: Diagrama Actividad...")
    diag4 = create_or_get_diagram(ea, 4, 39, "CUN04:Diagrama Actividad", "Sequence")
    diag4_id = diag4.DiagramID
    ea.Execute(f"DELETE FROM t_connector WHERE DiagramID = {diag4_id}")

    # Elements for CUN04 under UseCase 39
    el_nutri4 = create_or_get_element(ea, 4, 39, "Nutricionista", "Actor")
    el_turnero4 = create_or_get_element(ea, 4, 39, "FormTurnero_DNI101", "Object")
    el_ui4 = create_or_get_element(ea, 4, 39, "frmModificarTurno_DNI101", "Object")
    el_bll4 = create_or_get_element(ea, 4, 39, "TurnoBLL_DNI101", "Object")
    el_be4 = create_or_get_element(ea, 4, 39, "TurnoBE_DNI101", "Object")
    el_state4 = create_or_get_element(ea, 4, 39, "IEstadoTurno_DNI101", "Object")
    el_dv4 = create_or_get_element(ea, 4, 39, "DigitoVerificadorBLL", "Object")
    el_bit4 = create_or_get_element(ea, 4, 39, "EventoBLL", "Object")
    el_dal4 = create_or_get_element(ea, 4, 39, "DAL", "Object")

    diag4_lifelines = [
        (el_nutri4.ElementID, 30, 130, 80),
        (el_turnero4.ElementID, 170, 290, 230),
        (el_ui4.ElementID, 330, 480, 405),
        (el_bll4.ElementID, 520, 640, 580),
        (el_be4.ElementID, 680, 800, 740),
        (el_state4.ElementID, 840, 980, 910),
        (el_dv4.ElementID, 1020, 1140, 1080),
        (el_bit4.ElementID, 1180, 1280, 1230),
        (el_dal4.ElementID, 1320, 1420, 1370)
    ]

    lifeline_bottom4 = -1450
    for idx, (eid, l, r, cx) in enumerate(diag4_lifelines):
        sql_check = f"SELECT Diagram_ID FROM t_diagramobjects WHERE Diagram_ID = {diag4_id} AND Object_ID = {eid}"
        xml_chk = ea.SQLQuery(sql_check)
        import xml.etree.ElementTree as ET
        if not ET.fromstring(xml_chk).findall('.//Row'):
            d_obj = diag4.DiagramObjects.AddNew(f"l={l};r={r};t=-50;b={lifeline_bottom4}", "")
            d_obj.ElementID = eid
            d_obj.Update()
        else:
            ea.Execute(f"UPDATE t_diagramobjects SET RectLeft = {l}, RectRight = {r}, RectTop = -50, RectBottom = {lifeline_bottom4}, Sequence = {idx+1} WHERE Diagram_ID = {diag4_id} AND Object_ID = {eid}")

    diag4.DiagramObjects.Refresh()
    diag4.Update()

    msgs4 = [
        (1,  el_nutri4.ElementID, el_turnero4.ElementID, "btn_Modificar_Turno_Click()", "SynchCall", 80, -95, 230, -95),
        (2,  el_turnero4.ElementID, el_ui4.ElementID, "ShowDialog(turnoSeleccionado)", "SynchCall", 230, -135, 405, -135),
        (3,  el_nutri4.ElementID, el_ui4.ElementID, "btnBuscarCodigo_Click(codigoTurno)", "SynchCall", 80, -175, 405, -175),
        (4,  el_ui4.ElementID, el_bll4.ElementID, "ObtenerPorCodigo(codigoTurno)", "SynchCall", 405, -210, 580, -210),
        (5,  el_bll4.ElementID, el_dal4.ElementID, "ObtenerPorCodigo(codigoTurno)", "SynchCall", 580, -245, 1370, -245),
        (6,  el_dal4.ElementID, el_bll4.ElementID, "", "Return", 1370, -280, 580, -280),
        (7,  el_bll4.ElementID, el_ui4.ElementID, "", "Return", 580, -315, 405, -315),
        (8,  el_ui4.ElementID, el_ui4.ElementID, "MostrarDatosTurno(turno)", "SynchCall", 405, -350, 435, -370),
        (9,  el_nutri4.ElementID, el_ui4.ElementID, "btnGuardar_Click(nuevoMotivo, nuevoEstado)", "SynchCall", 80, -410, 405, -410),
        (10, el_ui4.ElementID, el_bll4.ElementID, "ModificarTurno(codigoTurno, nuevoMotivo, nuevoEstado)", "SynchCall", 405, -450, 580, -450),
        (11, el_bll4.ElementID, el_dal4.ElementID, "ObtenerPorCodigo(codigoTurno)", "SynchCall", 580, -490, 1370, -490),
        (12, el_dal4.ElementID, el_bll4.ElementID, "", "Return", 1370, -525, 580, -525),
        # State Pattern
        (13, el_bll4.ElementID, el_be4.ElementID, "Atender() / Cancelar(motivo) / ConfigurarEstado(estado)", "SynchCall", 580, -565, 740, -565),
        (14, el_be4.ElementID, el_state4.ElementID, "Atender(turno) / Cancelar(turno, motivo)", "SynchCall", 740, -600, 910, -600),
        (15, el_state4.ElementID, el_be4.ElementID, "", "Return", 910, -635, 740, -635),
        (16, el_be4.ElementID, el_bll4.ElementID, "", "Return", 740, -670, 580, -670),
        (17, el_bll4.ElementID, el_bll4.ElementID, "CalcularDV(cadenaDV)", "SynchCall", 580, -705, 610, -725),
        # Persistencia en DAL
        (18, el_bll4.ElementID, el_dal4.ElementID, "ModificarTurnoEstadoYMotivo(idTurno, nuevoMotivo, nuevoEstado, dv)", "SynchCall", 580, -765, 1370, -765),
        (19, el_dal4.ElementID, el_bll4.ElementID, "", "Return", 1370, -800, 580, -800),
        # DV - NO RETURN
        (20, el_bll4.ElementID, el_dv4.ElementID, "RecalcularYPersistir()", "SynchCall", 580, -840, 1080, -840),
        (21, el_dv4.ElementID, el_dal4.ElementID, "ActualizarDV(firmas)", "SynchCall", 1080, -875, 1370, -875),
        # Bitacora - NO RETURN
        (22, el_bll4.ElementID, el_bit4.ElementID, "RegistrarEvento(2, 'Turno Modificado', dniActual, 'TurneroNutricional')", "SynchCall", 580, -915, 1230, -915),
        (23, el_bit4.ElementID, el_dal4.ElementID, "GuardarBitacora(evento)", "SynchCall", 1230, -950, 1370, -950),
        # Returns and refresh
        (24, el_bll4.ElementID, el_ui4.ElementID, "", "Return", 580, -990, 405, -990),
        (25, el_ui4.ElementID, el_turnero4.ElementID, "DialogResult.OK", "Return", 405, -1025, 230, -1025),
        (26, el_turnero4.ElementID, el_turnero4.ElementID, "CargarTurnos()", "SynchCall", 230, -1065, 260, -1085),
        (27, el_turnero4.ElementID, el_nutri4.ElementID, "MostrarMensajeExito('Turno modificado exitosamente')", "SynchCall", 230, -1125, 80, -1125)
    ]

    for seq, s_id, r_id, m_name, stype, sx, sy, ex, ey in msgs4:
        sender_el = ea.GetElementByID(s_id)
        conn = sender_el.Connectors.AddNew(m_name, "Sequence")
        conn.SupplierID = r_id
        conn.SubType = stype
        conn.Update()
        sql = f"UPDATE t_connector SET DiagramID = {diag4_id}, SeqNo = {seq}, PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey} WHERE Connector_ID = {conn.ConnectorID}"
        ea.Execute(sql)

    diag4.Update()
    print("CUN04 built successfully.")

    # =========================================================================
    # CUN05: Cancelar Turno (UseCase ID: 37, Pkg: 4)
    # =========================================================================
    print("Building CUN05: Diagrama Actividad...")
    diag5 = create_or_get_diagram(ea, 4, 37, "CUN05:Diagrama Actividad", "Sequence")
    diag5_id = diag5.DiagramID
    ea.Execute(f"DELETE FROM t_connector WHERE DiagramID = {diag5_id}")

    # Elements for CUN05 under UseCase 37
    el_nutri5 = create_or_get_element(ea, 4, 37, "Nutricionista", "Actor")
    el_turnero5 = create_or_get_element(ea, 4, 37, "FormTurnero_DNI101", "Object")
    el_bll5 = create_or_get_element(ea, 4, 37, "TurnoBLL_DNI101", "Object")
    el_be5 = create_or_get_element(ea, 4, 37, "TurnoBE_DNI101", "Object")
    el_state5 = create_or_get_element(ea, 4, 37, "TurnoCanceladoState_DNI101", "Object")
    el_dv5 = create_or_get_element(ea, 4, 37, "DigitoVerificadorBLL", "Object")
    el_bit5 = create_or_get_element(ea, 4, 37, "EventoBLL", "Object")
    el_dal5 = create_or_get_element(ea, 4, 37, "DAL", "Object")

    diag5_lifelines = [
        (el_nutri5.ElementID, 30, 130, 80),
        (el_turnero5.ElementID, 170, 290, 230),
        (el_bll5.ElementID, 350, 470, 410),
        (el_be5.ElementID, 520, 640, 580),
        (el_state5.ElementID, 690, 870, 780),
        (el_dv5.ElementID, 920, 1040, 980),
        (el_bit5.ElementID, 1080, 1180, 1130),
        (el_dal5.ElementID, 1220, 1320, 1270)
    ]

    lifeline_bottom5 = -1350
    for idx, (eid, l, r, cx) in enumerate(diag5_lifelines):
        sql_check = f"SELECT Diagram_ID FROM t_diagramobjects WHERE Diagram_ID = {diag5_id} AND Object_ID = {eid}"
        xml_chk = ea.SQLQuery(sql_check)
        import xml.etree.ElementTree as ET
        if not ET.fromstring(xml_chk).findall('.//Row'):
            d_obj = diag5.DiagramObjects.AddNew(f"l={l};r={r};t=-50;b={lifeline_bottom5}", "")
            d_obj.ElementID = eid
            d_obj.Update()
        else:
            ea.Execute(f"UPDATE t_diagramobjects SET RectLeft = {l}, RectRight = {r}, RectTop = -50, RectBottom = {lifeline_bottom5}, Sequence = {idx+1} WHERE Diagram_ID = {diag5_id} AND Object_ID = {eid}")

    diag5.DiagramObjects.Refresh()
    diag5.Update()

    msgs5 = [
        (1,  el_nutri5.ElementID, el_turnero5.ElementID, "btn_Cancelar_Turno_Click()", "SynchCall", 80, -95, 230, -95),
        (2,  el_turnero5.ElementID, el_turnero5.ElementID, "ObtenerTurnoSeleccionado()", "SynchCall", 230, -130, 260, -150),
        (3,  el_turnero5.ElementID, el_nutri5.ElementID, "SolicitarConfirmacion(codigoTurno)", "SynchCall", 230, -185, 80, -185),
        (4,  el_nutri5.ElementID, el_turnero5.ElementID, "Confirmar(DialogResult.Yes)", "SynchCall", 80, -220, 230, -220),
        (5,  el_turnero5.ElementID, el_bll5.ElementID, "CancelarTurno(codigoTurno)", "SynchCall", 230, -260, 410, -260),
        (6,  el_bll5.ElementID, el_dal5.ElementID, "ObtenerPorCodigo(codigoTurno)", "SynchCall", 410, -300, 1270, -300),
        (7,  el_dal5.ElementID, el_bll5.ElementID, "", "Return", 1270, -335, 410, -335),
        # State pattern
        (8,  el_bll5.ElementID, el_be5.ElementID, "Cancelar(motivo)", "SynchCall", 410, -375, 580, -375),
        (9,  el_be5.ElementID, el_state5.ElementID, "new TurnoCanceladoState_DNI101()", "SynchCall", 580, -410, 780, -410),
        (10, el_be5.ElementID, el_bll5.ElementID, "", "Return", 580, -445, 410, -445),
        (11, el_bll5.ElementID, el_bll5.ElementID, "CalcularDV(cadenaDV)", "SynchCall", 410, -480, 440, -500),
        # Persistir en DAL
        (12, el_bll5.ElementID, el_dal5.ElementID, "CancelarTurno(idTurno, motivo, dv)", "SynchCall", 410, -540, 1270, -540),
        (13, el_dal5.ElementID, el_bll5.ElementID, "", "Return", 1270, -575, 410, -575),
        # Liberar bloque en agenda
        (14, el_bll5.ElementID, el_dal5.ElementID, "ActualizarEstadoBloque(idBloque, 'Disponible')", "SynchCall", 410, -615, 1270, -615),
        (15, el_dal5.ElementID, el_bll5.ElementID, "", "Return", 1270, -650, 410, -650),
        # DV - NO RETURN
        (16, el_bll5.ElementID, el_dv5.ElementID, "RecalcularYPersistir()", "SynchCall", 410, -690, 980, -690),
        (17, el_dv5.ElementID, el_dal5.ElementID, "ActualizarDV(firmas)", "SynchCall", 980, -725, 1270, -725),
        # Bitacora - NO RETURN
        (18, el_bll5.ElementID, el_bit5.ElementID, "RegistrarEvento(2, 'Turno Cancelado', dniActual, 'TurneroNutricional')", "SynchCall", 410, -765, 1130, -765),
        (19, el_bit5.ElementID, el_dal5.ElementID, "GuardarBitacora(evento)", "SynchCall", 1130, -800, 1270, -800),
        # Return to UI
        (20, el_bll5.ElementID, el_turnero5.ElementID, "", "Return", 410, -840, 230, -840),
        (21, el_turnero5.ElementID, el_turnero5.ElementID, "CargarTurnos()", "SynchCall", 230, -880, 260, -900),
        (22, el_turnero5.ElementID, el_nutri5.ElementID, "MostrarMensajeExito('Turno cancelado exitosamente')", "SynchCall", 230, -940, 80, -940)
    ]

    for seq, s_id, r_id, m_name, stype, sx, sy, ex, ey in msgs5:
        sender_el = ea.GetElementByID(s_id)
        conn = sender_el.Connectors.AddNew(m_name, "Sequence")
        conn.SupplierID = r_id
        conn.SubType = stype
        conn.Update()
        sql = f"UPDATE t_connector SET DiagramID = {diag5_id}, SeqNo = {seq}, PtStartX = {sx}, PtStartY = {sy}, PtEndX = {ex}, PtEndY = {ey} WHERE Connector_ID = {conn.ConnectorID}"
        ea.Execute(sql)

    diag5.Update()
    print("CUN05 built successfully.")

    # Export images to scratch for verification
    project = ea.GetProjectInterface()
    for d_id, name in [(12, "cun01"), (diag2_id, "cun02"), (13, "cun03"), (diag4_id, "cun04"), (diag5_id, "cun05")]:
        d = ea.GetDiagramByID(d_id)
        img_path = rf"C:\Users\Danie\Desktop\GIT\TD\scratch\{name}_actividad.png"
        project.PutDiagramImageToFile(d.DiagramGUID, img_path, 1)
        print(f"Exported {img_path}")

    ea.CloseFile()
    ea.Exit()
    print("ALL 5 DIAGRAMS SUCCESSFULLY BUILT AND SYNCHRONIZED IN TD.EAP!")

if __name__ == '__main__':
    build_all_5_cuns()
