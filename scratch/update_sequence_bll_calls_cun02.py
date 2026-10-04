import os
import sys
import uuid
import win32com.client
import xml.etree.ElementTree as ET

def apply_bll_calls_cun02():
    eap_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP'
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    
    diag = ea.GetDiagramByID(12)
    print("Opened diagram:", diag.Name)
    
    # Clean existing connectors for Diagram 12
    ea.Execute("DELETE FROM t_diagramlinks WHERE DiagramID = 12")
    ea.Execute("DELETE FROM t_connector WHERE DiagramID = 12")
    
    # Elements map
    el_map = {}
    for do in diag.DiagramObjects:
        el = ea.GetElementByID(do.ElementID)
        el_map[el.Name.strip()] = el
        el.Connectors.Refresh()
        
    nutri = el_map["Nutricionista"]
    gui = el_map["GUI"]
    agenda_bll = el_map["AgendaMedicaBLL_DNI101"]
    pac_bll = el_map["PacienteBLL_DNI101"]
    cun02 = el_map["CUN02: Registrar Paciente"]
    turno_bll = el_map["TurnoBLL_DNI101"]
    turno_be = el_map["TurnoBE_DNI101"]
    turno_state = el_map["TurnoSolicitadoState_DNI101"]
    evento_bll = el_map["EventoBLL"]
    dv_bll = el_map["DigitoVerificadorBLL"]
    dal = el_map["DAL"]
    
    # Update coordinates of InteractionFragments
    # 732: Identificacion de Paciente (alt)
    el_732 = ea.GetElementByID(732)
    el_732.Name = "Identificacion de Paciente"
    el_732.Update()
    ea.Execute("UPDATE t_diagramobjects SET RectTop = -595, RectBottom = -825, RectLeft = 6, RectRight = 1361 WHERE Diagram_ID = 12 AND Object_ID = 732")
    
    # Partition descriptor for 732 (Bottom to Top in EA descriptor)
    part_desc_732 = "@PAR;Name=Paciente Registrado;Size=45;GUID={CFA9E348-3644-4f01-9E75-159A17873AB2};@ENDPAR;@PAR;Name=Paciente No Registrado [Punto de Extension CUN-02];Size=185;GUID={B584A820-20FD-4ba8-B15D-F637FC121C09};@ENDPAR;"
    ea.Execute(f"UPDATE t_xref SET Description = '{part_desc_732}' WHERE Client = '{el_732.ElementGUID}' AND Name = 'Partitions'")

    # 728: Superposicion de Turno (alt)
    ea.Execute("UPDATE t_diagramobjects SET RectTop = -1085, RectBottom = -1145, RectLeft = 65, RectRight = 1366 WHERE Diagram_ID = 12 AND Object_ID = 728")

    # 734: Campos obligatorios (alt)
    ea.Execute("UPDATE t_diagramobjects SET RectTop = -1160, RectBottom = -1780, RectLeft = 47, RectRight = 1328 WHERE Diagram_ID = 12 AND Object_ID = 734")
    part_desc_734 = "@PAR;Name=Campos Obligatorios No Cargados;Size=60;GUID={E8E05D84-17A7-4189-BA08-F5E1B4E22AB9};@ENDPAR;@PAR;Name=Campos Obligatorios Cargados;Size=560;GUID={40503C30-96B6-4297-B4C1-D247B7205CB8};@ENDPAR;"
    el_734 = ea.GetElementByID(734)
    ea.Execute(f"UPDATE t_xref SET Description = '{part_desc_734}' WHERE Client = '{el_734.ElementGUID}' AND Name = 'Partitions'")

    # Messages list: PacienteBLL directly activates CUN02
    messages = [
        # --- FASE 1: Consulta de Bloques y Disponibilidad ---
        (1, nutri, gui, "btn_Registrar_Turno()", "SynchCall", -151, "Synchronous", "retval=void;", "Call"),
        (2, gui, gui, "ConsultarDisponibilidad()", "SynchCall", -175, "Synchronous", "retval=void;", "Call"),
        (3, gui, agenda_bll, "ListarBloquesDisponibles(fecha, dniNutri)", "SynchCall", -205, "Synchronous", "retval=void;paramsDlg=fecha, dniNutri;", "Call"),
        (4, agenda_bll, dal, "ListarBloquesDisponibles(fecha, dniNutri)", "SynchCall", -245, "Synchronous", "retval=void;paramsDlg=fecha, dniNutri;", "Call"),
        (5, dal, agenda_bll, "", "Return", -280, "Synchronous", "retval=List<BloqueHorarioBE>;", "Call"),
        (6, agenda_bll, dal, "ObtenerHorariosOcupadosPorProfesional(dniNutri, fecha)", "SynchCall", -315, "Synchronous", "retval=void;paramsDlg=dniNutri, fecha;", "Call"),
        (7, dal, agenda_bll, "", "Return", -350, "Synchronous", "retval=List<Horarios>;", "Call"),
        (8, agenda_bll, gui, "", "Return", -385, "Synchronous", "retval=List<BloqueHorarioBE>;", "Call"),
        (9, gui, nutri, "", "Return", -420, "Synchronous", "retval=MostrarHorariosDisponibles;", "Call"),

        # --- FASE 2: Deteccion temprana en txtDniNiño_Leave y Punto de Extension CUN-02 disparado por PacienteBLL ---
        (10, nutri, gui, "txtDniNiño_Leave(dniNiño)", "SynchCall", -460, "Synchronous", "retval=void;paramsDlg=dniNiño;", "Call"),
        (11, gui, pac_bll, "ObtenerOAsegurarPaciente(dniNiño)", "SynchCall", -495, "Synchronous", "retval=PacienteBE_DNI101;paramsDlg=dniNiño;", "Call"),
        (12, pac_bll, dal, "ObtenerPacientePorDNI(dniNiño)", "SynchCall", -535, "Synchronous", "retval=PacienteBE_DNI101;paramsDlg=dniNiño;", "Call"),
        (13, dal, pac_bll, "", "Return", -565, "Synchronous", "retval=null;", "Call"),

        # [alt Identificacion de Paciente]
        # Top branch: [Paciente No Registrado [Punto de Extension CUN-02]]
        # PacienteBLL invoca directamente al Punto de Extension CUN02
        (14, pac_bll, cun02, "RegistrarPaciente(dniNiño)", "New", -625, "Synchronous", "retval=void;paramsDlg=dniNiño;", "Call"),
        (15, nutri, cun02, "IngresarDatosPaciente(apellido, nombre, dni, tel, obraSocial, email)", "SynchCall", -665, "Synchronous", "retval=void;", "Call"),
        (16, cun02, dal, "RegistrarPaciente(pacienteBE)", "SynchCall", -705, "Synchronous", "retval=int;paramsDlg=pacienteBE;", "Call"),
        (17, dal, cun02, "", "Return", -740, "Synchronous", "retval=idPacienteGenerado;", "Call"),
        (18, cun02, pac_bll, "", "Return", -775, "Synchronous", "retval=PacienteBE_DNI101;", "Call"),

        # [Retorno final de BLL a GUI e identificación al Nutricionista]
        (19, pac_bll, gui, "", "Return", -845, "Synchronous", "retval=PacienteBE_DNI101;", "Call"),
        (20, gui, nutri, "", "Return", -885, "Synchronous", "retval=lblPacienteInfo (Paciente identificado);", "Call"),

        # --- FASE 3: Confirmación y Registro de Turno en btnRegistrarTurno_Click ---
        (21, nutri, gui, "btnRegistrarTurno_Click()", "SynchCall", -930, "Synchronous", "retval=void;", "Call"),
        (22, gui, gui, "ValidarCamposObligatorios()", "SynchCall", -960, "Synchronous", "retval=void;", "Call"),
        (23, gui, turno_bll, "RegistrarTurno(dniNiño, fecha, horario, motivo, idBloque, dniNutri)", "SynchCall", -995, "Synchronous", "retval=TurnoBE_DNI101;paramsDlg=dniNiño, fecha, horario, motivo, idBloque, dniNutri;", "Call"),
        (24, turno_bll, dal, "ObtenerPacientePorDNI(dniNiño)", "SynchCall", -1030, "Synchronous", "retval=PacienteBE_DNI101;paramsDlg=dniNiño;", "Call"),
        (25, dal, turno_bll, "", "Return", -1060, "Synchronous", "retval=PacienteBE_DNI101;", "Call"),
        (26, turno_bll, dal, "ExisteTurnoParaProfesional(dniNutri, fecha, hora)", "SynchCall", -1095, "Synchronous", "retval=bool;paramsDlg=dniNutri, fecha, hora;", "Call"),
        (27, dal, turno_bll, "", "Return", -1125, "Synchronous", "retval=false (Sin superposicion);", "Call"),

        # [alt Campos obligatorios / Generación y Persistencia del Turno]
        (28, turno_bll, turno_be, "new TurnoBE_DNI101(dniNiño, fecha, horario, motivo)", "New", -1175, "Synchronous", "retval=void;", "Call"),
        (29, turno_bll, turno_be, "GenerarCodigoUnico()", "SynchCall", -1210, "Synchronous", "retval=string (TRN-...);", "Call"),
        (30, turno_bll, turno_state, "new TurnoSolicitadoState_DNI101()", "New", -1245, "Synchronous", "retval=void;", "Call"),
        (31, turno_bll, turno_be, "CambiarEstado(state)", "SynchCall", -1280, "Synchronous", "retval=void;paramsDlg=state;", "Call"),
        (32, turno_bll, turno_bll, "CalcularDV(cadenaDV)", "SynchCall", -1315, "Synchronous", "retval=string (hash);", "Call"),
        (33, turno_bll, dal, "RegistrarTurnoConBloque(turnoBE, idBloque)", "SynchCall", -1355, "Synchronous", "retval=int;paramsDlg=turnoBE, idBloque; [SqlTransaction atomica]", "Call"),
        (34, dal, turno_bll, "", "Return", -1390, "Synchronous", "retval=idTurnoGenerado;", "Call"),
        (35, turno_bll, evento_bll, "RegistrarEvento(1, Turno registrado..., dniNutri, TurneroNutricional)", "SynchCall", -1430, "Synchronous", "retval=bool;", "Call"),
        (36, evento_bll, dal, "GuardarBitacora(evento)", "SynchCall", -1465, "Synchronous", "retval=void;paramsDlg=evento;", "Call"),
        (37, dal, evento_bll, "", "Return", -1500, "Synchronous", "retval=true;", "Call"),
        (38, evento_bll, turno_bll, "", "Return", -1530, "Synchronous", "retval=true;", "Call"),
        (39, turno_bll, dv_bll, "RecalcularYPersistir()", "SynchCall", -1565, "Synchronous", "retval=void; [Batch Transaction]", "Call"),
        (40, dv_bll, dal, "PersistirIntegridadCompleta(resumen, filasDV)", "SynchCall", -1605, "Synchronous", "retval=void;paramsDlg=resumen, filasDV;", "Call"),
        (41, dal, dv_bll, "", "Return", -1640, "Synchronous", "retval=true;", "Call"),
        (42, dv_bll, turno_bll, "", "Return", -1670, "Synchronous", "retval=true;", "Call"),
        (43, turno_bll, gui, "", "Return", -1710, "Synchronous", "retval=TurnoBE_DNI101 (exito);", "Call"),
        (44, gui, nutri, "", "Return", -1750, "Synchronous", "retval=MostrarMensajeExito(CodigoTurno);", "Call")
    ]
    
    coords_x = {}
    for do in diag.DiagramObjects:
        coords_x[do.ElementID] = (do.left + do.right) // 2
        
    print("Inserting connectors with PacienteBLL calling CUN02...")
    for item in messages:
        seq, src, dst, msg_name, subtype, y, p1, p2, p3 = item
        clean_name = msg_name.replace("'", "''")
        conn = src.Connectors.AddNew(clean_name, "Sequence")
        conn.SupplierID = dst.ElementID
        conn.SequenceNo = seq
        conn.SubType = subtype
        conn.DiagramID = 12
        conn.Update()
        src.Connectors.Refresh()
        
        cid = conn.ConnectorID
        sx = coords_x.get(src.ElementID, 100)
        ex = coords_x.get(dst.ElementID, 200)
        
        # Self-message offset
        if src.ElementID == dst.ElementID:
            ex = sx + 25
            ey = y - 15
        else:
            ey = y
            
        p1_sql = p1.replace("'", "''")
        p2_sql = p2.replace("'", "''")
        p3_sql = p3.replace("'", "''")
        
        sql_update = f"""
        UPDATE t_connector 
        SET PtStartX = {sx}, PtEndX = {ex}, PtStartY = {y}, PtEndY = {ey},
            PDATA1 = '{p1_sql}', PDATA2 = '{p2_sql}', PDATA3 = '{p3_sql}',
            RouteStyle = 1, LineColor = -1
        WHERE Connector_ID = {cid}
        """
        ea.Execute(sql_update)
        
    diag.Update()
    diag.DiagramObjects.Refresh()
    
    # Export updated PNG
    proj = ea.GetProjectInterface()
    out_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\DSS\CUN01 Registrar Turno.png'
    proj.PutDiagramImageToFile(diag.DiagramGUID, out_path, 1)
    print("Exported updated diagram image to:", out_path)
    
    ea.CloseFile()
    ea.Exit()
    print("Successfully synchronized sequence diagram with PacienteBLL invoking CUN02!")

if __name__ == '__main__':
    apply_bll_calls_cun02()
