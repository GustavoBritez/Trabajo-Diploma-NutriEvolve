import os
import sys
import uuid
import win32com.client
import xml.etree.ElementTree as ET

def apply_cun01_sequence():
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
    # 732: Paciente Registrado (alt)
    ea.Execute("UPDATE t_diagramobjects SET RectTop = -615, RectBottom = -945, RectLeft = 6, RectRight = 1361 WHERE Diagram_ID = 12 AND Object_ID = 732")
    # 728: Superposicion de Turno (alt)
    ea.Execute("UPDATE t_diagramobjects SET RectTop = -1190, RectBottom = -1255, RectLeft = 65, RectRight = 1366 WHERE Diagram_ID = 12 AND Object_ID = 728")
    # 734: Campos obligatorios (alt)
    ea.Execute("UPDATE t_diagramobjects SET RectTop = -1265, RectBottom = -1885, RectLeft = 47, RectRight = 1328 WHERE Diagram_ID = 12 AND Object_ID = 734")
    
    # Update partition heights for fragment 732
    part_desc = "@PAR;Name=Paciente No Registrado [CUN-02];Size=250;GUID={B584A820-20FD-4ba8-B15D-F637FC121C09};@ENDPAR;@PAR;Name=Paciente Registrado;Size=80;GUID={CFA9E348-3644-4f01-9E75-159A17873AB2};@ENDPAR;"
    ea.Execute(f"UPDATE t_xref SET Description = '{part_desc}' WHERE Client = '{{C3C383D5-B27A-4f14-8A2C-BF52259EF8CF}}' AND Name = 'Partitions'")

    # Full list of messages according to refactored code:
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

        # --- FASE 2: Deteccion temprana en txtDniNiño_Leave y Punto de Extension CUN-02 ---
        (10, nutri, gui, "txtDniNiño_Leave(dniNiño)", "SynchCall", -460, "Synchronous", "retval=void;paramsDlg=dniNiño;", "Call"),
        (11, gui, gui, "AsegurarPacienteConCUN02(dniNiño)", "SynchCall", -490, "Synchronous", "retval=void;", "Call"),
        (12, gui, pac_bll, "ObtenerOAsegurarPaciente(dniNiño, callbackUI)", "SynchCall", -525, "Synchronous", "retval=PacienteBE_DNI101;paramsDlg=dniNiño, callbackUI;", "Call"),
        (13, pac_bll, dal, "ObtenerPacientePorDNI(dniNiño)", "SynchCall", -560, "Synchronous", "retval=PacienteBE_DNI101;paramsDlg=dniNiño;", "Call"),
        (14, dal, pac_bll, "", "Return", -595, "Synchronous", "retval=null;", "Call"),

        # [alt Paciente No Registrado - Punto de Extensión CUN-02]
        (15, pac_bll, gui, "callback: solicitarRegistroUI(dniNiño)", "SynchCall", -635, "Synchronous", "retval=bool;paramsDlg=dniNiño;", "Call"),
        (16, gui, cun02, "RegistrarPaciente(dniNiño)", "New", -670, "Synchronous", "retval=void;paramsDlg=dniNiño;", "Call"),
        (17, nutri, cun02, "IngresarDatosPaciente(apellido, nombre, dni, tel, obraSocial, email)", "SynchCall", -705, "Synchronous", "retval=void;", "Call"),
        (18, cun02, dal, "RegistrarPaciente(pacienteBE)", "SynchCall", -740, "Synchronous", "retval=int;paramsDlg=pacienteBE;", "Call"),
        (19, dal, cun02, "", "Return", -775, "Synchronous", "retval=idPacienteGenerado;", "Call"),
        (20, cun02, gui, "", "Return", -810, "Synchronous", "retval=DialogResult.OK;", "Call"),
        (21, gui, pac_bll, "", "Return", -845, "Synchronous", "retval=true (registro completado);", "Call"),
        (22, pac_bll, dal, "ObtenerPacientePorDNI(dniNiño)", "SynchCall", -880, "Synchronous", "retval=PacienteBE_DNI101;paramsDlg=dniNiño;", "Call"),
        (23, dal, pac_bll, "", "Return", -915, "Synchronous", "retval=PacienteBE_DNI101;", "Call"),

        # [Retorno final de BLL a GUI e identificación al Nutricionista]
        (24, pac_bll, gui, "", "Return", -955, "Synchronous", "retval=PacienteBE_DNI101;", "Call"),
        (25, gui, nutri, "", "Return", -990, "Synchronous", "retval=lblPacienteInfo (Paciente identificado);", "Call"),

        # --- FASE 3: Confirmación y Registro de Turno en btnRegistrarTurno_Click ---
        (26, nutri, gui, "btnRegistrarTurno_Click()", "SynchCall", -1035, "Synchronous", "retval=void;", "Call"),
        (27, gui, gui, "ValidarCamposObligatorios()", "SynchCall", -1065, "Synchronous", "retval=void;", "Call"),
        (28, gui, turno_bll, "RegistrarTurno(dniNiño, fecha, horario, motivo, idBloque, dniNutri)", "SynchCall", -1100, "Synchronous", "retval=TurnoBE_DNI101;paramsDlg=dniNiño, fecha, horario, motivo, idBloque, dniNutri;", "Call"),
        (29, turno_bll, dal, "ObtenerPacientePorDNI(dniNiño)", "SynchCall", -1135, "Synchronous", "retval=PacienteBE_DNI101;paramsDlg=dniNiño;", "Call"),
        (30, dal, turno_bll, "", "Return", -1165, "Synchronous", "retval=PacienteBE_DNI101;", "Call"),
        (31, turno_bll, dal, "ExisteTurnoParaProfesional(dniNutri, fecha, hora)", "SynchCall", -1200, "Synchronous", "retval=bool;paramsDlg=dniNutri, fecha, hora;", "Call"),
        (32, dal, turno_bll, "", "Return", -1230, "Synchronous", "retval=false (Sin superposicion);", "Call"),

        # [alt Campos obligatorios / Generación y Persistencia del Turno]
        (33, turno_bll, turno_be, "new TurnoBE_DNI101(dniNiño, fecha, horario, motivo)", "New", -1280, "Synchronous", "retval=void;", "Call"),
        (34, turno_bll, turno_be, "GenerarCodigoUnico()", "SynchCall", -1315, "Synchronous", "retval=string (TRN-...);", "Call"),
        (35, turno_bll, turno_state, "new TurnoSolicitadoState_DNI101()", "New", -1350, "Synchronous", "retval=void;", "Call"),
        (36, turno_bll, turno_be, "CambiarEstado(state)", "SynchCall", -1385, "Synchronous", "retval=void;paramsDlg=state;", "Call"),
        (37, turno_bll, turno_bll, "CalcularDV(cadenaDV)", "SynchCall", -1420, "Synchronous", "retval=string (hash);", "Call"),
        (38, turno_bll, dal, "RegistrarTurnoConBloque(turnoBE, idBloque)", "SynchCall", -1460, "Synchronous", "retval=int;paramsDlg=turnoBE, idBloque; [SqlTransaction atomica]", "Call"),
        (39, dal, turno_bll, "", "Return", -1495, "Synchronous", "retval=idTurnoGenerado;", "Call"),
        (40, turno_bll, evento_bll, "RegistrarEvento(1, Turno registrado..., dniNutri, TurneroNutricional)", "SynchCall", -1535, "Synchronous", "retval=bool;", "Call"),
        (41, evento_bll, dal, "GuardarBitacora(evento)", "SynchCall", -1570, "Synchronous", "retval=void;paramsDlg=evento;", "Call"),
        (42, dal, evento_bll, "", "Return", -1605, "Synchronous", "retval=true;", "Call"),
        (43, evento_bll, turno_bll, "", "Return", -1635, "Synchronous", "retval=true;", "Call"),
        (44, turno_bll, dv_bll, "RecalcularYPersistir()", "SynchCall", -1670, "Synchronous", "retval=void; [Batch Transaction]", "Call"),
        (45, dv_bll, dal, "PersistirIntegridadCompleta(resumen, filasDV)", "SynchCall", -1710, "Synchronous", "retval=void;paramsDlg=resumen, filasDV;", "Call"),
        (46, dal, dv_bll, "", "Return", -1745, "Synchronous", "retval=true;", "Call"),
        (47, dv_bll, turno_bll, "", "Return", -1775, "Synchronous", "retval=true;", "Call"),
        (48, turno_bll, gui, "", "Return", -1815, "Synchronous", "retval=TurnoBE_DNI101 (exito);", "Call"),
        (49, gui, nutri, "", "Return", -1855, "Synchronous", "retval=MostrarMensajeExito(CodigoTurno);", "Call")
    ]
    
    # Calculate lifelines center X
    coords_x = {}
    for do in diag.DiagramObjects:
        coords_x[do.ElementID] = (do.left + do.right) // 2
        
    print("Inserting refactored connectors...")
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
    print("CUN01 Sequence Diagram successfully updated in TD.EAP!")

if __name__ == '__main__':
    apply_cun01_sequence()
