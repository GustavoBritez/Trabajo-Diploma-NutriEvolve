import win32com.client
import os
import shutil
import uuid

def rebuild_cun02_all():
    eap_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP'
    scratch_dir = r'c:\Users\Danie\Desktop\GIT\TD\scratch'
    artifact_dir = r'C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e'

    print("Connecting to Enterprise Architect...")
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    try:
        # =====================================================================
        # 1. DIAGRAM 56: CUN02 DIAGRAMA DE SECUENCIA
        # =====================================================================
        print("\n=== 1. REBUILDING DIAGRAM 56 (CUN02: Diagrama de Secuencia) ===")
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
        # In EA, bottom partition is listed first with its size, top partition is listed second
        part_desc = f"@PAR;Name=[DNI No Duplicado: pacienteExistente == null];Size=400;GUID={{{guid2}}};@ENDPAR;@PAR;Name=[DNI Duplicado: pacienteExistente != null];Size=135;GUID={{{guid1}}};@ENDPAR;"
        ea.Execute(f"DELETE FROM t_xref WHERE Client = '{el_frag.ElementGUID}' AND Name = 'Partitions'")
        ea.Execute(f"INSERT INTO t_xref (XrefID, Name, Type, Visibility, Partition, Description, Client) VALUES ('{{{str(uuid.uuid4()).upper()}}}', 'Partitions', 'element property', 'Public', '0', '{part_desc}', '{el_frag.ElementGUID}')")

        diag56.DiagramObjects.Refresh()

        # Messages definition for CUN02:
        # (seq, src, dst, msg_name, subtype, y, pdata1, pdata2, pdata3, pdata4, override_ex)
        seq_messages = [
            # 1. Nutricionista presiona botón Registrar Paciente
            (1, el_nutri, el_gui, "btnRegistrarPaciente_Click()", "SynchCall", -110, "Synchronous", "", "Call", "", None),
            # 2. UI valida campos obligatorios en memoria
            (2, el_gui, el_gui, "ValidarCamposObligatorios()", "SynchCall", -145, "Synchronous", "", "Call", "", None),
            # 3. UI invoca BLL
            (3, el_gui, el_pac_bll, "RegistrarPaciente(nombre, apellido, dniNiño, telefono, email, obraSocial)", "SynchCall", -180, "Synchronous", "", "Call", "", None),
            # 4. BLL consulta duplicados en DAL
            (4, el_pac_bll, el_dal, "ObtenerPacientePorDNI(dniNiño)", "SynchCall", -215, "Synchronous", "", "Call", "", None),
            # 5. DAL retorna a BLL
            (5, el_dal, el_pac_bll, "", "Return", -245, "Synchronous", "", "Call", "1", None),

            # --- ALT TOP PARTITION: [DNI Duplicado: pacienteExistente != null] (-270 a -405) ---
            # 6. BLL retorna error/excepción a UI
            (6, el_pac_bll, el_gui, "", "Return", -295, "Synchronous", "", "Call", "1", None),
            # 7. UI muestra mensaje de duplicado
            (7, el_gui, el_nutri, "MostrarMensaje(msg_dni_duplicado)", "SynchCall", -330, "Synchronous", "", "Call", "", None),
            # 8. Retorno a UI
            (8, el_nutri, el_gui, "", "Return", -365, "Synchronous", "", "Call", "1", None),

            # --- ALT BOTTOM PARTITION: [DNI No Duplicado: pacienteExistente == null] (-405 a -805) ---
            # 9. Creación dinámica de PacienteBE_DNI101 (arrow lands right on left edge 660 at -435)
            (9, el_pac_bll, el_pac_be, "new PacienteBE_DNI101(nombre, apellido, dniNiño, tel, email, obraSocial)", "New", -435, "Synchronous", "", "Call", "", 660),
            # 10. BLL persiste en DAL con datos escalares
            (10, el_pac_bll, el_dal, "Guardar(dniNiño, nombre, apellido, tel, email, fechaNac, sexo, os, dv)", "SynchCall", -475, "Synchronous", "", "Call", "", None),
            # 11. DAL retorna ID generado a BLL
            (11, el_dal, el_pac_bll, "", "Return", -510, "Synchronous", "", "Call", "1", None),
            # 12. BLL registra en Bitácora
            (12, el_pac_bll, el_bit_bll, "RegistrarBitacora(1, Registro de Paciente Pediátrico..., dniActual, TurneroNutricional)", "SynchCall", -545, "Synchronous", "", "Call", "", None),
            # 13. BitacoraBLL persiste en DAL
            (13, el_bit_bll, el_dal, "GuardarBitacora(bitacora)", "SynchCall", -580, "Synchronous", "", "Call", "", None),
            # 14. DAL retorna a BitacoraBLL
            (14, el_dal, el_bit_bll, "", "Return", -610, "Synchronous", "", "Call", "1", None),
            # 15. BitacoraBLL retorna a PacienteBLL
            (15, el_bit_bll, el_pac_bll, "", "Return", -640, "Synchronous", "", "Call", "1", None),
            # 16. BLL solicita recálculo de integridad a DigitoVerificadorBLL
            (16, el_pac_bll, el_dv_bll, "RecalcularYPersistir()", "SynchCall", -675, "Synchronous", "", "Call", "", None),
            # 17. DigitoVerificadorBLL persiste en DAL
            (17, el_dv_bll, el_dal, "PersistirIntegridadCompleta(resumen, filasDV)", "SynchCall", -710, "Synchronous", "", "Call", "", None),
            # 18. DAL retorna a DigitoVerificadorBLL
            (18, el_dal, el_dv_bll, "", "Return", -740, "Synchronous", "", "Call", "1", None),
            # 19. DigitoVerificadorBLL retorna a PacienteBLL
            (19, el_dv_bll, el_pac_bll, "", "Return", -770, "Synchronous", "", "Call", "1", None),

            # --- POST-FRAGMENT: Retorno exitoso a UI y Usuario ---
            # 20. BLL retorna a UI
            (20, el_pac_bll, el_gui, "", "Return", -830, "Synchronous", "", "Call", "1", None),
            # 21. UI muestra mensaje de confirmación
            (21, el_gui, el_nutri, "MostrarMensaje(msg_paciente_registrado_det)", "SynchCall", -865, "Synchronous", "", "Call", "", None),
            # 22. Retorno de confirmación a UI
            (22, el_nutri, el_gui, "", "Return", -895, "Synchronous", "", "Call", "1", None),
            # 23. UI se cierra
            (23, el_gui, el_gui, "Close()", "SynchCall", -925, "Synchronous", "", "Call", "", None)
        ]

        print(f"Inserting {len(seq_messages)} connectors into Diagram 56...")
        for item in seq_messages:
            seq, src, dst, msg_name, subtype, y, p1, p2, p3, p4, override_ex = item
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
            if override_ex is not None:
                ex = override_ex
            else:
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
        print("Diagram 56 exported successfully to:", out_dss1)

        # =====================================================================
        # 2. DIAGRAM 30: CUN02 DIAGRAMA DE CLASES (BE, BLL y DAL)
        # =====================================================================
        print("\n=== 2. REBUILDING DIAGRAM 30 (CUN02: Diagrama de Clases) ===")
        def populate_attributes_only(el, attrs):
            for i in range(el.Attributes.Count - 1, -1, -1):
                el.Attributes.Delete(i)
            el.Attributes.Refresh()

            for a_name, a_type in attrs:
                attr = el.Attributes.AddNew(a_name, a_type)
                attr.Visibility = "Private"
                attr.Update()
            el.Attributes.Refresh()

            for i in range(el.Methods.Count - 1, -1, -1):
                el.Methods.Delete(i)
            el.Methods.Refresh()
            el.Update()

        def populate_methods_only(el, methods):
            for i in range(el.Attributes.Count - 1, -1, -1):
                el.Attributes.Delete(i)
            el.Attributes.Refresh()

            for i in range(el.Methods.Count - 1, -1, -1):
                el.Methods.Delete(i)
            el.Methods.Refresh()
            el.Update()

            for m_name, m_ret, m_params in methods:
                meth = el.Methods.AddNew(m_name, m_ret)
                meth.Visibility = "Public"
                meth.Update()
                pos = 0
                for p_name, p_type in m_params:
                    param = meth.Parameters.AddNew(p_name, p_type)
                    param.Position = pos
                    param.Update()
                    pos += 1
                meth.Parameters.Refresh()
                meth.Update()
            el.Methods.Refresh()
            el.Update()

        # Tier 1 (BE): PacienteBE_DNI101 (662)
        el_pac_be_cls = ea.GetElementByID(662)
        pac_attrs = [
            ("IdPaciente_DNI101", "int"),
            ("DNINiño_DNI101", "string"),
            ("Nombre_DNI101", "string"),
            ("Apellido_DNI101", "string"),
            ("Telefono_DNI101", "string?"),
            ("Email_DNI101", "string?"),
            ("FechaNacimiento_DNI101", "DateTime"),
            ("Sexo_DNI101", "string?"),
            ("ObraSocial_DNI101", "string?"),
            ("NombreCompleto", "string"),
            ("DV", "string?")
        ]
        populate_attributes_only(el_pac_be_cls, pac_attrs)

        # Tier 2 (BLL): PacienteBLL_DNI101 (757), BitacoraBLL (762), DigitoVerificadorBLL (763)
        el_pac_bll_cls = ea.GetElementByID(757)
        pac_bll_methods = [
            ("ListarPacientes", "List<PacienteBE_DNI101>", []),
            ("ModificarPaciente", "bool", [("paciente", "PacienteBE_DNI101")]),
            ("ObtenerOAsegurarPaciente", "PacienteBE_DNI101", [("dniNiño", "string"), ("callbackUI", "Func<string, bool>")]),
            ("ObtenerPacientePorDNI", "PacienteBE_DNI101", [("dniNiño", "string")]),
            ("ObtenerPorId", "PacienteBE_DNI101", [("idPaciente", "int")]),
            ("RegistrarPaciente", "int", [("nombre", "string"), ("apellido", "string"), ("dniNiño", "string"), ("telefono", "string?"), ("email", "string?"), ("obraSocial", "string?")])
        ]
        populate_methods_only(el_pac_bll_cls, pac_bll_methods)

        el_bit_bll_cls = ea.GetElementByID(762)
        if el_bit_bll_cls.Name != "BitacoraBLL":
            el_bit_bll_cls.Name = "BitacoraBLL"
            el_bit_bll_cls.Update()
        bit_bll_methods = [
            ("BuscarBitacoras", "List<BitacoraBE>", [("desde", "DateTime"), ("hasta", "DateTime")]),
            ("RegistrarBitacora", "bool", [("criticidad", "int"), ("descripcion", "string"), ("dni", "int"), ("modulo", "string")]),
            ("VerBitacoras", "List<BitacoraBE>", [])
        ]
        populate_methods_only(el_bit_bll_cls, bit_bll_methods)

        el_dv_bll_cls = ea.GetElementByID(763)
        dv_bll_methods = [
            ("ActualizarDVIndividualesUsuarios", "void", []),
            ("ObtenerUsuariosCorruptos", "List<string>", []),
            ("RecalcularYPersistir", "void", []),
            ("VerificarBaseDatos", "bool", [])
        ]
        populate_methods_only(el_dv_bll_cls, dv_bll_methods)

        # Tier 3 (DAL): PacienteDAL_DNI101 (760), BitacoraDAL (764), DigitoVerificadorDAL (765)
        # Decoupled from BE: scalar arguments, returns DataTable / int / bool
        el_pac_dal_cls = ea.GetElementByID(760)
        pac_dal_methods = [
            ("Guardar", "int", [("dniNiño", "string"), ("nombre", "string"), ("apellido", "string"), ("tel", "string?"), ("email", "string?"), ("fechaNac", "DateTime"), ("sexo", "string?"), ("os", "string?"), ("dv", "string?")]),
            ("ListarTodos", "DataTable", []),
            ("Modificar", "bool", [("idPaciente", "int"), ("dniNiño", "string"), ("nombre", "string"), ("apellido", "string"), ("tel", "string?"), ("email", "string?"), ("fechaNac", "DateTime"), ("sexo", "string?"), ("os", "string?"), ("dv", "string?")]),
            ("ObtenerPacientePorDNI", "DataTable", [("dniNiño", "string")]),
            ("ObtenerPorId", "DataTable", [("idPaciente", "int")])
        ]
        populate_methods_only(el_pac_dal_cls, pac_dal_methods)

        el_bit_dal_cls = ea.GetElementByID(764)
        if el_bit_dal_cls.Name != "BitacoraDAL":
            el_bit_dal_cls.Name = "BitacoraDAL"
            el_bit_dal_cls.Update()
        bit_dal_methods = [
            ("FiltrarBitacora", "DataTable", [("desde", "DateTime"), ("hasta", "DateTime")]),
            ("GuardarBitacora", "int", [("criticidad", "int"), ("descripcion", "string"), ("dni", "int"), ("modulo", "string")]),
            ("ObtenerBitacora", "DataTable", [])
        ]
        populate_methods_only(el_bit_dal_cls, bit_dal_methods)

        el_dv_dal_cls = ea.GetElementByID(765)
        dv_dal_methods = [
            ("ActualizarDVRegistro", "void", [("schema", "string"), ("tabla", "string"), ("columnaId", "string"), ("valorId", "object"), ("dvCalculado", "string")]),
            ("ExisteTablaDV", "bool", []),
            ("ObtenerDatosTabla", "DataTable", [("schema", "string"), ("tabla", "string")]),
            ("ObtenerResumenPersistido", "DataTable", []),
            ("ObtenerTablasPersistentes", "List<(string, string)>", []),
            ("PersistirIntegridadCompleta", "void", [("resumen", "object"), ("filasDV", "object")])
        ]
        populate_methods_only(el_dv_dal_cls, dv_dal_methods)

        # Clear existing connectors between these 7 elements
        c02_cls_elements = [el_pac_be_cls, el_pac_bll_cls, el_bit_bll_cls, el_dv_bll_cls, el_pac_dal_cls, el_bit_dal_cls, el_dv_dal_cls]
        c02_cls_ids = [e.ElementID for e in c02_cls_elements]
        id_str = ",".join(str(i) for i in c02_cls_ids)
        ea.Execute(f"DELETE FROM t_connector WHERE Start_Object_ID IN ({id_str}) AND End_Object_ID IN ({id_str})")
        for el in c02_cls_elements:
            el.Connectors.Refresh()

        def add_use(src, dst):
            conn = src.Connectors.AddNew("", "Dependency")
            conn.SupplierID = dst.ElementID
            conn.Stereotype = "use"
            conn.Direction = "Source -> Destination"
            conn.Update()
            src.Connectors.Refresh()
            ea.Execute(f"UPDATE t_connector SET Stereotype = 'use' WHERE Connector_ID = {conn.ConnectorID}")
            return conn

        # BLL to BE:
        add_use(el_pac_bll_cls, el_pac_be_cls)

        # BLL to DAL:
        add_use(el_pac_bll_cls, el_pac_dal_cls)
        add_use(el_bit_bll_cls, el_bit_dal_cls)
        add_use(el_dv_bll_cls, el_dv_dal_cls)

        # BLL to BLL:
        add_use(el_pac_bll_cls, el_bit_bll_cls)
        add_use(el_pac_bll_cls, el_dv_bll_cls)

        # Layout Diagram 30
        diag30 = ea.GetDiagramByID(30)
        diag30.Name = "CUN02: Diagrama de Clases (BE, BLL y DAL)"
        diag30.Notes = "Diagrama de Clases CUN02 (Registrar Paciente Pediátrico): BLL orquesta y usa DAL y BE; DAL no tiene dependencias hacia BE."
        diag30.parentID = 4 # CUN02
        diag30.Update()

        for i in range(diag30.DiagramObjects.Count - 1, -1, -1):
            diag30.DiagramObjects.Delete(i)
        diag30.DiagramObjects.Refresh()

        coords30 = [
            # Tier 1: BE
            (el_pac_be_cls,  50,   50,  540,  330),

            # Tier 2: BLL
            (el_pac_bll_cls, 50,   410, 540,  660),
            (el_bit_bll_cls, 600,  410, 1040, 660),
            (el_dv_bll_cls,  1100, 410, 1680, 660),

            # Tier 3: DAL
            (el_pac_dal_cls, 50,   740, 540,  1040),
            (el_bit_dal_cls, 600,  740, 1040, 1040),
            (el_dv_dal_cls,  1100, 740, 1680, 1040)
        ]

        for el, left, top, right, bottom in coords30:
            do = diag30.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag30.DiagramObjects.Refresh()
        diag30.Update()
        ea.SaveDiagram(30)

        ea.Execute("""
UPDATE t_diagram
SET ShowForeign = 0,
    ShowPackageContents = 0,
    StyleEx = 'ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TConnectorNotation=UML 2.1;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;',
    PDATA = 'HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=0;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;'
WHERE Diagram_ID = 30
        """)
        ea.Execute("UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = 30")

        out_dc1 = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Clases\CUN02 Registrar Paciente.png'
        out_dc2 = os.path.join(scratch_dir, "cun02_clases_bll_dal_be.png")
        out_dc3 = os.path.join(artifact_dir, "cun02_clases_bll_dal_be.png")
        proj.PutDiagramImageToFile(diag30.DiagramGUID, out_dc1, 1)
        shutil.copy2(out_dc1, out_dc2)
        shutil.copy2(out_dc1, out_dc3)
        print("Diagram 30 exported successfully to:", out_dc1)

        # =====================================================================
        # 3. DIAGRAM 31: CUN02 DER (MODELO RELACIONAL)
        # =====================================================================
        print("\n=== 3. REBUILDING DIAGRAM 31 (CUN02: DER Modelo Relacional) ===")
        diag31 = ea.GetDiagramByID(31)
        diag31.Name = "CUN02: DER (Modelo Relacional)"
        diag31.Notes = "Diagrama Entidad-Relación (DER) CUN02 (Registrar Paciente Pediátrico): Pacientes_DNI101, Usuarios, Bitacora y DV con notación Information Engineering (patas de gallo)."
        diag31.parentID = 4 # CUN02
        diag31.Update()

        for i in range(diag31.DiagramObjects.Count - 1, -1, -1):
            diag31.DiagramObjects.Delete(i)
        diag31.DiagramObjects.Refresh()

        el_pac_tab = ea.GetElementByID(430)
        el_usr_tab = ea.GetElementByID(431)
        el_dv_tab = ea.GetElementByID(435)
        el_bit_tab = ea.GetElementByID(436)

        coords31 = [
            (el_pac_tab, 50,  40,  420, 360),
            (el_dv_tab,  50,  430, 420, 560),
            (el_usr_tab, 520, 40,  880, 350),
            (el_bit_tab, 520, 430, 880, 640)
        ]

        for el, left, top, right, bottom in coords31:
            do = diag31.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag31.DiagramObjects.Refresh()
        diag31.Update()
        ea.SaveDiagram(31)

        ea.Execute("""
UPDATE t_diagram
SET StyleEx = 'TConnectorNotation=Information Engineering;ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;',
    PDATA = 'HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=0;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;',
    ShowForeign = 0,
    ShowPackageContents = 0
WHERE Diagram_ID = 31
        """)

        ea.Execute("DELETE FROM t_diagramlinks WHERE DiagramID = 31")
        ea.Execute("INSERT INTO t_diagramlinks (DiagramID, ConnectorID, Geometry, Style, Hidden) VALUES (31, 895, NULL, 'Mode=3;', 0)")

        out_der1 = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Entidad Relacion\CU02 DER (Modelo Relacional).png'
        out_der2 = os.path.join(scratch_dir, "cun02_der.png")
        out_der3 = os.path.join(artifact_dir, "cun02_der.png")
        proj.PutDiagramImageToFile(diag31.DiagramGUID, out_der1, 1)
        shutil.copy2(out_der1, out_der2)
        shutil.copy2(out_der1, out_der3)
        print("Diagram 31 exported successfully to:", out_der1)

        print("\nAll 3 CUN02 diagrams rebuilt and exported successfully!")

    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass
        print("EA session closed cleanly.")

if __name__ == '__main__':
    rebuild_cun02_all()
