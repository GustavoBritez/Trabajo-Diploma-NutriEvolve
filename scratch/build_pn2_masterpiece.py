import win32com.client
import os
import shutil
import uuid

def build_pn2_complete():
    ea = win32com.client.Dispatch("EA.Repository")
    eap_path = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP"
    ea.OpenFile(eap_path)
    proj = ea.GetProjectInterface()

    scratch_dir = r"C:\Users\Danie\Desktop\GIT\TD\scratch"
    artifact_dir = r"C:\Users\Danie\.gemini\antigravity-ide\brain\e96ed8c6-c21c-4649-a731-718a961e1c2e"
    dss_dir = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\DSS"
    clases_dir = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Clases"
    der_dir = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Entidad Relacion"

    os.makedirs(scratch_dir, exist_ok=True)
    os.makedirs(artifact_dir, exist_ok=True)
    os.makedirs(dss_dir, exist_ok=True)
    os.makedirs(clases_dir, exist_ok=True)
    os.makedirs(der_dir, exist_ok=True)

    try:
        print("=======================================================")
        print("=== BUILDING PN2 COMPLETE: DIAGRAMS & ARCHITECTURE ===")
        print("=======================================================")

        pkg2 = ea.GetPackageByID(2) # Use Case Model
        pkg4 = ea.GetPackageByID(4) # Primary Use Cases
        pkg7 = ea.GetPackageByID(7) # Class Model

        # Standardize Use Case Names
        uc06 = ea.GetElementByID(42)
        uc06.Name = "CUN06: Seleccionar Paciente"
        uc06.Update()

        uc07 = ea.GetElementByID(46)
        uc07.Name = "CUN07: Graficar Diagnostico"
        uc07.Update()

        uc08 = ea.GetElementByID(674)
        uc08.Name = "CUN08: Registrar Consulta"
        uc08.Update()

        uc09 = ea.GetElementByID(675)
        uc09.Name = "CUN09: Prescribir Plan"
        uc09.Update()

        # Helper functions
        def get_or_create_element(pkg, name, el_type, stereo="", parent_id=0):
            for el in pkg.Elements:
                if el.Name == name and el.Type == el_type and el.ParentID == parent_id:
                    if stereo and el.Stereotype != stereo:
                        el.Stereotype = stereo
                        el.Update()
                    return el
            el = pkg.Elements.AddNew(name, el_type)
            if stereo:
                el.Stereotype = stereo
            if parent_id > 0:
                el.ParentID = parent_id
            el.Update()
            pkg.Elements.Refresh()
            return el

        def get_or_create_child_object(parent_el, name, el_type="Object", subtype=0):
            for el in parent_el.Elements:
                if el.Name == name and el.Type == el_type:
                    el.Subtype = subtype
                    el.Update()
                    return el
            new_el = parent_el.Elements.AddNew(name, el_type)
            new_el.Subtype = subtype
            new_el.Update()
            parent_el.Elements.Refresh()
            return new_el

        def get_or_create_diagram(pkg, name, d_type, parent_id=0):
            for d in pkg.Diagrams:
                if d.Name == name and d.ParentID == parent_id:
                    return d
            d = pkg.Diagrams.AddNew(name, d_type)
            d.ParentID = parent_id
            d.Update()
            pkg.Diagrams.Refresh()
            return d

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

        def populate_table_attributes(el, columns):
            for i in range(el.Attributes.Count - 1, -1, -1):
                el.Attributes.Delete(i)
            el.Attributes.Refresh()

            for c_name, c_type, is_pk, is_fk in columns:
                attr = el.Attributes.AddNew(c_name, c_type)
                if is_pk:
                    attr.Stereotype = "PK"
                    attr.IsOrdered = True
                elif is_fk:
                    attr.Stereotype = "FK"
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

        def add_use_dependency(src, dst):
            for c in src.Connectors:
                if c.SupplierID == dst.ElementID and c.Type == "Dependency":
                    return c
            conn = src.Connectors.AddNew("", "Dependency")
            conn.SupplierID = dst.ElementID
            conn.Stereotype = "use"
            conn.Direction = "Source -> Destination"
            conn.Update()
            src.Connectors.Refresh()
            ea.Execute(f"UPDATE t_connector SET Stereotype = 'use' WHERE Connector_ID = {conn.ConnectorID}")
            return conn

        def add_realization(src, dst):
            sql_check = f"SELECT Connector_ID FROM t_connector WHERE Start_Object_ID = {src.ElementID} AND End_Object_ID = {dst.ElementID} AND Connector_Type = 'Realisation'"
            res = ea.SQLQuery(sql_check)
            if "<Row>" not in res:
                guid = "{" + str(uuid.uuid4()).upper() + "}"
                ea.Execute(f"INSERT INTO t_connector (ea_guid, Connector_Type, Start_Object_ID, End_Object_ID, Direction, SeqNo, RouteStyle, LineColor) VALUES ('{guid}', 'Realisation', {src.ElementID}, {dst.ElementID}, 'Source -> Destination', 0, 1, -1)")
                src.Connectors.Refresh()
                dst.Connectors.Refresh()
            return None

        def add_association(src, dst, name="", s_card="", d_card="", s_role="", d_role=""):
            for c in src.Connectors:
                if c.SupplierID == dst.ElementID and c.Type == "Association":
                    if name and not c.Name:
                        c.Name = name
                        c.Update()
                    return c
            conn = src.Connectors.AddNew(name, "Association")
            conn.SupplierID = dst.ElementID
            if s_card:
                conn.ClientEnd.Cardinality = s_card
            if d_card:
                conn.SupplierEnd.Cardinality = d_card
            if s_role:
                conn.ClientEnd.Role = s_role
            if d_role:
                conn.SupplierEnd.Role = d_role
            conn.Update()
            src.Connectors.Refresh()
            return conn

        def apply_class_diagram_style(diag_id):
            ea.Execute(f"""
UPDATE t_diagram
SET ShowForeign = 0,
    ShowPackageContents = 0,
    StyleEx = 'ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TConnectorNotation=UML 2.1;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;',
    PDATA = 'HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=1;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;'
WHERE Diagram_ID = {diag_id}
            """)
            ea.Execute(f"UPDATE t_diagramlinks SET Geometry = null, Style = 'Mode=3;' WHERE DiagramID = {diag_id}")

        def apply_der_style(diag_id):
            ea.Execute(f"""
UPDATE t_diagram
SET StyleEx = 'TConnectorNotation=Information Engineering;ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;',
    PDATA = 'HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=0;PPgs.cy=0;PSize=9;ShowIcons=1;SuppCN=1;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;',
    ShowForeign = 0,
    ShowPackageContents = 0
WHERE Diagram_ID = {diag_id}
            """)

        def clear_diagram_objects(diag):
            for i in range(diag.DiagramObjects.Count - 1, -1, -1):
                diag.DiagramObjects.Delete(i)
            diag.DiagramObjects.Refresh()
            diag.Update()

        def export_diagram(diag, out_files):
            ea.ReloadDiagram(diag.DiagramID)
            primary = out_files[0]
            proj.PutDiagramImageToFile(diag.DiagramGUID, primary, 1)
            for copy_dst in out_files[1:]:
                shutil.copy2(primary, copy_dst)
            print(f"Exported diagram '{diag.Name}' (ID {diag.DiagramID}) -> {primary}")

        # =====================================================================
        # 1. CREATE/UPDATE PN2 TABLES
        # =====================================================================
        print("\n--- 1. Creating/Updating Database Tables for PN2 ---")
        tab_paciente = ea.GetElementByID(430)
        tab_usuario = ea.GetElementByID(431)
        tab_dv = ea.GetElementByID(435)
        tab_bitacora = ea.GetElementByID(436)

        tab_consultas = get_or_create_element(pkg4, "ConsultasNutricionales_DNI101", "Class", stereo="table")
        tab_mediciones = get_or_create_element(pkg4, "MedicionesAntropometricas_DNI101", "Class", stereo="table")
        tab_diagnosticos = get_or_create_element(pkg4, "DiagnosticosNutricionales_DNI101", "Class", stereo="table")
        tab_alertas = get_or_create_element(pkg4, "AlertasClinicas_DNI101", "Class", stereo="table")
        tab_planes = get_or_create_element(pkg4, "PlanesAlimentarios_DNI101", "Class", stereo="table")

        cols_consultas = [
            ("IdConsulta_DNI101", "int", True, False),
            ("IdPaciente_DNI101", "int", False, True),
            ("DniNutricionista_DNI101", "int", False, True),
            ("IdTurno_DNI101", "int", False, True),
            ("FechaControl_DNI101", "datetime2", False, False),
            ("EdadMeses_DNI101", "int", False, False),
            ("TipoLactancia_DNI101", "varchar(50)", False, False),
            ("AlimentacionComplementaria_DNI101", "varchar(255)", False, False),
            ("Alergias_DNI101", "varchar(255)", False, False),
            ("AntecedentesFamiliares_DNI101", "varchar(255)", False, False),
            ("Observaciones_DNI101", "varchar(500)", False, False),
            ("DV", "varchar(255)", False, False)
        ]
        populate_table_attributes(tab_consultas, cols_consultas)

        cols_mediciones = [
            ("IdMedicion_DNI101", "int", True, False),
            ("IdConsulta_DNI101", "int", False, True),
            ("PesoKg_DNI101", "decimal(5,2)", False, False),
            ("TallaCm_DNI101", "decimal(5,2)", False, False),
            ("PerimetroCefalicoCm_DNI101", "decimal(5,2)", False, False),
            ("CircunferenciaCinturaCm_DNI101", "decimal(5,2)", False, False),
            ("IMC_DNI101", "decimal(5,2)", False, False),
            ("DV", "varchar(255)", False, False)
        ]
        populate_table_attributes(tab_mediciones, cols_mediciones)

        cols_diagnosticos = [
            ("IdDiagnostico_DNI101", "int", True, False),
            ("IdConsulta_DNI101", "int", False, True),
            ("ClasificacionOMS_DNI101", "varchar(100)", False, False),
            ("DetallesClinicos_DNI101", "varchar(500)", False, False),
            ("RequiereAlerta_DNI101", "bit", False, False),
            ("DV", "varchar(255)", False, False)
        ]
        populate_table_attributes(tab_diagnosticos, cols_diagnosticos)

        cols_alertas = [
            ("IdAlerta_DNI101", "int", True, False),
            ("IdConsulta_DNI101", "int", False, True),
            ("TipoAlerta_DNI101", "varchar(50)", False, False),
            ("Severidad_DNI101", "varchar(20)", False, False),
            ("MensajeAlerta_DNI101", "varchar(255)", False, False),
            ("FechaGeneracion_DNI101", "datetime2", False, False),
            ("DV", "varchar(255)", False, False)
        ]
        populate_table_attributes(tab_alertas, cols_alertas)

        cols_planes = [
            ("IdPlan_DNI101", "int", True, False),
            ("IdConsulta_DNI101", "int", False, True),
            ("RequerimientoCalorico_DNI101", "decimal(6,2)", False, False),
            ("PctCarbohidratos_DNI101", "decimal(4,2)", False, False),
            ("PctProteinas_DNI101", "decimal(4,2)", False, False),
            ("PctGrasas_DNI101", "decimal(4,2)", False, False),
            ("PautasFamiliares_DNI101", "varchar(1000)", False, False),
            ("MetasSalud_DNI101", "varchar(500)", False, False),
            ("DV", "varchar(255)", False, False)
        ]
        populate_table_attributes(tab_planes, cols_planes)

        # Table Foreign Keys
        add_association(tab_paciente, tab_consultas, "FK_Consultas_Pacientes", "1", "0..*")
        add_association(tab_usuario, tab_consultas, "FK_Consultas_Usuarios", "1", "0..*")
        add_association(tab_consultas, tab_mediciones, "FK_Mediciones_Consultas", "1", "1")
        add_association(tab_consultas, tab_diagnosticos, "FK_Diagnosticos_Consultas", "1", "1")
        add_association(tab_consultas, tab_alertas, "FK_Alertas_Consultas", "1", "0..1")
        add_association(tab_consultas, tab_planes, "FK_Planes_Consultas", "1", "0..1")

        # =====================================================================
        # 2. CREATE/UPDATE PN2 CLASSES (BE, BLL, DAL, STRATEGY)
        # =====================================================================
        print("\n--- 2. Creating/Updating Classes for PN2 ---")
        el_pac_be = ea.GetElementByID(662)
        el_pac_bll = ea.GetElementByID(757)
        el_pac_dal = ea.GetElementByID(760)
        el_bit_bll = ea.GetElementByID(762)
        el_dv_bll = ea.GetElementByID(763)
        el_bit_dal = ea.GetElementByID(764)
        el_dv_dal = ea.GetElementByID(765)

        # BEs
        el_consulta_be = get_or_create_element(pkg7, "ConsultaNutricionalBE_DNI101", "Class")
        populate_attributes_only(el_consulta_be, [
            ("IdConsulta_DNI101", "int"),
            ("IdPaciente_DNI101", "int"),
            ("DniNutricionista_DNI101", "int"),
            ("FechaControl_DNI101", "DateTime"),
            ("EdadMeses_DNI101", "int"),
            ("TipoLactancia_DNI101", "string?"),
            ("AlimentacionComplementaria_DNI101", "string?"),
            ("Alergias_DNI101", "string?"),
            ("AntecedentesFamiliares_DNI101", "string?"),
            ("Observaciones_DNI101", "string?"),
            ("Medicion", "MedicionAntropometricaBE_DNI101?"),
            ("Diagnostico", "DiagnosticoNutricionalBE_DNI101?"),
            ("Alerta", "AlertaClinicaBE_DNI101?"),
            ("DV", "string?")
        ])

        el_medicion_be = get_or_create_element(pkg7, "MedicionAntropometricaBE_DNI101", "Class")
        populate_attributes_only(el_medicion_be, [
            ("IdMedicion_DNI101", "int"),
            ("IdConsulta_DNI101", "int"),
            ("PesoKg_DNI101", "decimal"),
            ("TallaCm_DNI101", "decimal"),
            ("PerimetroCefalicoCm_DNI101", "decimal"),
            ("CircunferenciaCinturaCm_DNI101", "decimal?"),
            ("IMC_DNI101", "decimal"),
            ("DV", "string?")
        ])

        el_diag_be = get_or_create_element(pkg7, "DiagnosticoNutricionalBE_DNI101", "Class")
        populate_attributes_only(el_diag_be, [
            ("IdDiagnostico_DNI101", "int"),
            ("IdConsulta_DNI101", "int"),
            ("ClasificacionOMS_DNI101", "string"),
            ("DetallesClinicos_DNI101", "string"),
            ("RequiereAlerta_DNI101", "bool"),
            ("DV", "string?")
        ])

        el_alerta_be = get_or_create_element(pkg7, "AlertaClinicaBE_DNI101", "Class")
        populate_attributes_only(el_alerta_be, [
            ("IdAlerta_DNI101", "int"),
            ("IdConsulta_DNI101", "int"),
            ("TipoAlerta_DNI101", "string"),
            ("Severidad_DNI101", "string"),
            ("MensajeAlerta_DNI101", "string"),
            ("FechaGeneracion_DNI101", "DateTime"),
            ("DV", "string?")
        ])

        el_plan_be = get_or_create_element(pkg7, "PlanAlimentarioBE_DNI101", "Class")
        populate_attributes_only(el_plan_be, [
            ("IdPlan_DNI101", "int"),
            ("IdConsulta_DNI101", "int"),
            ("RequerimientoCalorico_DNI101", "decimal"),
            ("PctCarbohidratos_DNI101", "decimal"),
            ("PctProteinas_DNI101", "decimal"),
            ("PctGrasas_DNI101", "decimal"),
            ("PautasFamiliares_DNI101", "string"),
            ("MetasSalud_DNI101", "string"),
            ("DV", "string?")
        ])

        # BLLs
        el_consulta_bll = get_or_create_element(pkg7, "ConsultaNutricionalBLL_DNI101", "Class")
        populate_methods_only(el_consulta_bll, [
            ("CalcularIMC", "decimal", [("pesoKg", "decimal"), ("tallaCm", "decimal")]),
            ("ObtenerHistorialCompleto", "List<ConsultaNutricionalBE_DNI101>", [("idPaciente", "int")]),
            ("RegistrarConsultaCompleta", "int", [
                ("idPaciente", "int"), ("dniNutricionista", "int"), ("edadMeses", "int"),
                ("pesoKg", "decimal"), ("tallaCm", "decimal"), ("pcCm", "decimal"),
                ("ccCm", "decimal?"), ("lactancia", "string?"), ("alimentacion", "string?"),
                ("alergias", "string?"), ("antecedentes", "string?"), ("observaciones", "string?"),
                ("sexo", "string")
            ])
        ])

        el_curvas_oms = get_or_create_element(pkg7, "CurvasOMSData_DNI101", "Class")
        populate_methods_only(el_curvas_oms, [
            ("ObtenerPuntosCurva", "double[]", [("tipoCurva", "string"), ("percentil", "string"), ("sexo", "string")]),
            ("ObtenerMesesReferencia", "double[]", []),
            ("InterpolarPercentilAproximado", "string", [("edadMeses", "int"), ("valor", "decimal"), ("tipoCurva", "string"), ("sexo", "string")])
        ])

        el_plan_bll = get_or_create_element(pkg7, "PlanAlimentarioBLL_DNI101", "Class")
        populate_methods_only(el_plan_bll, [
            ("ObtenerPlanPorConsulta", "PlanAlimentarioBE_DNI101?", [("idConsulta", "int")]),
            ("PrescribirPlan", "int", [
                ("idConsulta", "int"), ("calorias", "decimal"), ("pctCarb", "decimal"),
                ("pctProt", "decimal"), ("pctGrasas", "decimal"), ("pautas", "string"),
                ("metas", "string"), ("dniNutricionista", "int")
            ])
        ])

        # Strategy Pattern
        el_strategy_iface = get_or_create_element(pkg7, "IEvaluadorNutricionalStrategy_DNI101", "Interface")
        populate_methods_only(el_strategy_iface, [
            ("Evaluar", "ResultadoEvaluacionOMS", [
                ("meses", "int"), ("sexo", "string"), ("peso", "decimal"), ("talla", "decimal"), ("imc", "decimal")
            ])
        ])

        el_strat_imc = get_or_create_element(pkg7, "EvaluacionIMCEdadStrategy", "Class")
        populate_methods_only(el_strat_imc, [
            ("Evaluar", "ResultadoEvaluacionOMS", [
                ("meses", "int"), ("sexo", "string"), ("peso", "decimal"), ("talla", "decimal"), ("imc", "decimal")
            ])
        ])
        add_realization(el_strat_imc, el_strategy_iface)

        el_strat_peso = get_or_create_element(pkg7, "EvaluacionPesoEdadStrategy", "Class")
        populate_methods_only(el_strat_peso, [
            ("Evaluar", "ResultadoEvaluacionOMS", [
                ("meses", "int"), ("sexo", "string"), ("peso", "decimal"), ("talla", "decimal"), ("imc", "decimal")
            ])
        ])
        add_realization(el_strat_peso, el_strategy_iface)

        el_strat_talla = get_or_create_element(pkg7, "EvaluacionTallaEdadStrategy", "Class")
        populate_methods_only(el_strat_talla, [
            ("Evaluar", "ResultadoEvaluacionOMS", [
                ("meses", "int"), ("sexo", "string"), ("peso", "decimal"), ("talla", "decimal"), ("imc", "decimal")
            ])
        ])
        add_realization(el_strat_talla, el_strategy_iface)

        el_context_eval = get_or_create_element(pkg7, "EvaluadorNutricionalContext_DNI101", "Class")
        populate_methods_only(el_context_eval, [
            ("SetStrategy", "void", [("strategy", "IEvaluadorNutricionalStrategy_DNI101")]),
            ("EjecutarEvaluacion", "ResultadoEvaluacionOMS", [
                ("meses", "int"), ("sexo", "string"), ("peso", "decimal"), ("talla", "decimal"), ("imc", "decimal")
            ])
        ])
        add_association(el_context_eval, el_strategy_iface, "evalua mediante", "1", "1")

        el_resultado_oms = get_or_create_element(pkg7, "ResultadoEvaluacionOMS", "Class")
        populate_attributes_only(el_resultado_oms, [
            ("ZScore", "decimal"),
            ("PercentilAproximado", "string"),
            ("Clasificacion", "string"),
            ("EsAlerta", "bool"),
            ("TipoAlerta", "string"),
            ("SeveridadAlerta", "string"),
            ("MensajeAlerta", "string")
        ])

        # DALs (Decoupled: strictly scalar arguments, returns DataTable / int / bool, NO references to BE)
        el_consulta_dal = get_or_create_element(pkg7, "ConsultaNutricionalDAL_DNI101", "Class")
        populate_methods_only(el_consulta_dal, [
            ("GuardarConsultaCompleta", "int", [
                ("idPaciente", "int"), ("dniNutri", "int"), ("idTurno", "int?"),
                ("fecha", "DateTime"), ("edadMeses", "int"), ("lactancia", "string?"),
                ("alimentacion", "string?"), ("alergias", "string?"), ("antecedentes", "string?"),
                ("obs", "string?"), ("dvConsulta", "string?"), ("peso", "decimal"),
                ("talla", "decimal"), ("pc", "decimal"), ("cc", "decimal?"),
                ("imc", "decimal"), ("dvMedicion", "string?"), ("clasifOMS", "string"),
                ("detallesClinicos", "string"), ("requiereAlerta", "bool"), ("dvDiag", "string?"),
                ("tipoAlerta", "string?"), ("severidad", "string?"), ("msjAlerta", "string?"),
                ("dvAlerta", "string?")
            ]),
            ("ObtenerHistorialConsultasPorPaciente", "DataTable", [("idPaciente", "int")]),
            ("ObtenerMedicionPorConsulta", "DataTable", [("idConsulta", "int")]),
            ("ObtenerDiagnosticoPorConsulta", "DataTable", [("idConsulta", "int")]),
            ("ObtenerAlertaPorConsulta", "DataTable", [("idConsulta", "int")])
        ])

        el_plan_dal = get_or_create_element(pkg7, "PlanAlimentarioDAL_DNI101", "Class")
        populate_methods_only(el_plan_dal, [
            ("GuardarPlanAlimentario", "int", [
                ("idConsulta", "int"), ("calorias", "decimal"), ("pctCarb", "decimal"),
                ("pctProt", "decimal"), ("pctGrasas", "decimal"), ("pautas", "string"),
                ("metas", "string"), ("dvPlan", "string?")
            ]),
            ("ObtenerPlanPorConsulta", "DataTable", [("idConsulta", "int")])
        ])

        # Dependencies for Class Diagrams
        add_use_dependency(el_consulta_bll, el_consulta_be)
        add_use_dependency(el_consulta_bll, el_medicion_be)
        add_use_dependency(el_consulta_bll, el_diag_be)
        add_use_dependency(el_consulta_bll, el_alerta_be)
        add_use_dependency(el_consulta_bll, el_consulta_dal)
        add_use_dependency(el_consulta_bll, el_curvas_oms)
        add_use_dependency(el_consulta_bll, el_context_eval)
        add_use_dependency(el_consulta_bll, el_bit_bll)
        add_use_dependency(el_consulta_bll, el_dv_bll)

        add_use_dependency(el_plan_bll, el_plan_be)
        add_use_dependency(el_plan_bll, el_plan_dal)
        add_use_dependency(el_plan_bll, el_bit_bll)
        add_use_dependency(el_plan_bll, el_dv_bll)

        add_use_dependency(el_context_eval, el_resultado_oms)
        add_use_dependency(el_strategy_iface, el_resultado_oms)

        # =====================================================================
        # 3. BUILD SEQUENCE DIAGRAMS (CUN06, CUN07, CUN08, CUN09)
        # =====================================================================
        print("\n--- 3. Building Sequence Diagrams (DSS) for PN2 ---")
        el_nutri_actor = ea.GetElementByID(266)

        def build_sequence_diagram(uc_el, diag_name, notes, lifelines_data, messages_data, out_pngs, lifeline_bottom=-1000):
            diag = get_or_create_diagram(pkg4, diag_name, "Sequence", parent_id=uc_el.ElementID)
            diag.Notes = notes
            diag.Update()
            diag_id = diag.DiagramID

            ea.Execute(f"DELETE FROM t_connector WHERE DiagramID = {diag_id}")
            clear_diagram_objects(diag)

            centers = {}
            for idx, (el, left, right, cx) in enumerate(lifelines_data):
                centers[el.ElementID] = cx
                d_obj = diag.DiagramObjects.AddNew(f"l={left};r={right};t=-50;b={lifeline_bottom}", "")
                d_obj.ElementID = el.ElementID
                d_obj.Sequence = idx + 1
                d_obj.Update()

            diag.DiagramObjects.Refresh()
            diag.Update()

            for seq, s_el, r_el, m_name, stype, y, p1, p2, p3, p4 in messages_data:
                s_id = s_el.ElementID
                r_id = r_el.ElementID
                sx = centers[s_id]
                is_self = (s_id == r_id)
                if is_self:
                    ex = sx + 25
                    ey = y - 15
                else:
                    ex = centers[r_id]
                    ey = y

                conn = s_el.Connectors.AddNew(m_name, "Sequence")
                conn.SupplierID = r_id
                conn.SubType = stype
                conn.SequenceNo = seq
                conn.DiagramID = diag_id
                conn.Update()
                s_el.Connectors.Refresh()

                clean_name = m_name.replace("'", "''")
                ea.Execute(f"""
                UPDATE t_connector
                SET DiagramID = {diag_id}, SeqNo = {seq}, Name = '{clean_name}',
                    PtStartX = {sx}, PtStartY = {y}, PtEndX = {ex}, PtEndY = {ey},
                    PDATA1 = '{p1}', PDATA2 = '{p2}', PDATA3 = '{p3}', PDATA4 = '{p4}',
                    RouteStyle = 1, LineColor = -1
                WHERE Connector_ID = {conn.ConnectorID}
                """)

            export_diagram(diag, out_pngs)

        def get_or_create_boundary(parent_el, name="GUI"):
            for el in parent_el.Elements:
                if el.Name == name and (el.Type == "Sequence" or el.Type == "Boundary"):
                    if el.Stereotype != "boundary":
                        el.Stereotype = "boundary"
                        el.Update()
                        ea.Execute(f"UPDATE t_object SET Stereotype = 'boundary' WHERE Object_ID = {el.ElementID}")
                    return el
            el = parent_el.Elements.AddNew(name, "Sequence")
            el.Stereotype = "boundary"
            el.Update()
            parent_el.Elements.Refresh()
            ea.Execute(f"UPDATE t_object SET Stereotype = 'boundary' WHERE Object_ID = {el.ElementID}")
            return el

        # --- DSS CUN06: Seleccionar Paciente ---
        el_gui_c06 = get_or_create_boundary(uc06, "GUI")
        el_obj_pac_bll = get_or_create_child_object(uc06, "PacienteBLL_DNI101", "Object")
        el_obj_dal6 = get_or_create_child_object(uc06, "DAL", "Object")

        lifelines_c06 = [
            (el_nutri_actor,      20,   120,  70),
            (el_gui_c06,          160,  240,  200),
            (el_obj_pac_bll,      290,  450,  370),
            (el_obj_dal6,         500,  640,  570)
        ]

        messages_c06 = [
            (1, el_nutri_actor, el_gui_c06, "btnSeguimientoNutricional_Click()", "SynchCall", -90, "Synchronous", "", "Call", ""),
            (2, el_gui_c06, el_gui_c06, "CargarPacientes()", "SynchCall", -130, "Synchronous", "", "Call", ""),
            (3, el_gui_c06, el_obj_pac_bll, "ListarPacientes()", "SynchCall", -170, "Synchronous", "", "Call", ""),
            (4, el_obj_pac_bll, el_obj_dal6, "ListarTodos()", "SynchCall", -210, "Synchronous", "", "Call", ""),
            (5, el_obj_dal6, el_obj_pac_bll, "", "Return", -250, "Synchronous", "", "Call", "1"),
            (6, el_obj_pac_bll, el_gui_c06, "", "Return", -290, "Synchronous", "", "Call", "1"),
            (7, el_gui_c06, el_nutri_actor, "", "Return", -330, "Synchronous", "", "Call", "1"),
            (8, el_nutri_actor, el_gui_c06, "dgvPacientes_CellClick(paciente)", "SynchCall", -370, "Synchronous", "", "Call", ""),
            (9, el_gui_c06, el_gui_c06, "MostrarOpciones('Graficar Diagnostico', 'Registrar Consulta', 'Prescribir Plan')", "SynchCall", -410, "Synchronous", "", "Call", ""),
            (10, el_gui_c06, el_nutri_actor, "", "Return", -450, "Synchronous", "", "Call", "1")
        ]

        build_sequence_diagram(
            uc06, "CUN06: Diagrama de Secuencia",
            "Diagrama de Secuencia CUN06 (Seleccionar Paciente): UI modelada como Boundary (GUI). Nutricionista accede a Seguimiento Nutricional, GUI dispara ListarPacientes hacia BLL -> DAL genérica (1 solo DB hit). Al seleccionar paciente, GUI despliega opciones interactivas.",
            lifelines_c06, messages_c06,
            [
                os.path.join(dss_dir, "CUN06 Seleccionar Paciente.png"),
                os.path.join(scratch_dir, "cun06_secuencia.png"),
                os.path.join(artifact_dir, "cun06_secuencia.png")
            ],
            lifeline_bottom=-510
        )

        # --- DSS CUN07: Graficar Diagnóstico ---
        el_gui_c07 = get_or_create_boundary(uc07, "GUI")
        el_obj_cons_bll7 = get_or_create_child_object(uc07, "ConsultaNutricionalBLL_DNI101", "Object")
        el_obj_oms_data = get_or_create_child_object(uc07, "CurvasOMSData_DNI101", "Object")
        el_obj_dal7 = get_or_create_child_object(uc07, "DAL", "Object")

        lifelines_c07 = [
            (el_nutri_actor,    20,   120,  70),
            (el_gui_c07,        160,  240,  200),
            (el_obj_cons_bll7,  290,  490,  390),
            (el_obj_oms_data,   540,  710,  625),
            (el_obj_dal7,       760,  900,  830)
        ]

        messages_c07 = [
            (1, el_nutri_actor, el_gui_c07, "btnGraficarDiagnostico_Click(paciente)", "SynchCall", -90, "Synchronous", "", "Call", ""),
            (2, el_gui_c07, el_gui_c07, "InicializarGraficoScottPlot()", "SynchCall", -130, "Synchronous", "", "Call", ""),
            (3, el_gui_c07, el_obj_cons_bll7, "ObtenerHistorialCompleto(idPaciente)", "SynchCall", -170, "Synchronous", "", "Call", ""),
            (4, el_obj_cons_bll7, el_obj_dal7, "ObtenerHistorialConsultasPorPaciente(idPaciente)", "SynchCall", -210, "Synchronous", "", "Call", ""),
            (5, el_obj_dal7, el_obj_cons_bll7, "", "Return", -250, "Synchronous", "", "Call", "1"),
            (6, el_obj_cons_bll7, el_gui_c07, "", "Return", -290, "Synchronous", "", "Call", "1"),
            (7, el_gui_c07, el_obj_oms_data, "ObtenerCurva(tipoCurva, sexo)", "SynchCall", -330, "Synchronous", "", "Call", ""),
            (8, el_obj_oms_data, el_gui_c07, "", "Return", -370, "Synchronous", "", "Call", "1"),
            (9, el_gui_c07, el_gui_c07, "GraficarCurvasYConsultas(curvasOMS, historialPaciente)", "SynchCall", -410, "Synchronous", "", "Call", ""),
            (10, el_gui_c07, el_nutri_actor, "", "Return", -450, "Synchronous", "", "Call", "1")
        ]

        build_sequence_diagram(
            uc07, "CUN07: Diagrama de Secuencia",
            "Diagrama de Secuencia CUN07 (Graficar Diagnóstico): UI modelada como Boundary (GUI). Nutricionista selecciona graficar, GUI solicita historial a BLL -> DAL genérica (1 único hit), obtiene patrones de referencia OMS y grafica curvas ScottPlot de alta fidelidad.",
            lifelines_c07, messages_c07,
            [
                os.path.join(dss_dir, "CUN07 Graficar Diagnostico.png"),
                os.path.join(scratch_dir, "cun07_secuencia.png"),
                os.path.join(artifact_dir, "cun07_secuencia.png")
            ],
            lifeline_bottom=-510
        )

        # --- DSS CUN08: Registrar Consulta ---
        el_gui_c08 = get_or_create_boundary(uc08, "GUI")
        el_obj_cons_bll8 = get_or_create_child_object(uc08, "ConsultaNutricionalBLL_DNI101", "Object")
        el_obj_strat_ctx = get_or_create_child_object(uc08, "EvaluadorNutricionalContext_DNI101", "Object")
        el_obj_strat = get_or_create_child_object(uc08, "IEvaluadorNutricionalStrategy_DNI101", "Object")
        el_obj_bit8 = get_or_create_child_object(uc08, "BitacoraBLL", "Object")
        el_obj_dv8 = get_or_create_child_object(uc08, "DigitoVerificadorBLL", "Object")
        el_obj_dal8 = get_or_create_child_object(uc08, "DAL", "Object")

        lifelines_c08 = [
            (el_nutri_actor,    20,   120,  70),
            (el_gui_c08,        160,  240,  200),
            (el_obj_cons_bll8,  290,  490,  390),
            (el_obj_strat_ctx,  530,  730,  630),
            (el_obj_strat,      770,  990,  880),
            (el_obj_bit8,       1030, 1160, 1095),
            (el_obj_dv8,        1200, 1350, 1275),
            (el_obj_dal8,       1390, 1510, 1450)
        ]

        messages_c08 = [
            # Evaluación en vivo durante carga
            (1, el_nutri_actor, el_gui_c08, "txtMediciones_TextChanged(peso, talla, pc)", "SynchCall", -90, "Synchronous", "", "Call", ""),
            (2, el_gui_c08, el_obj_cons_bll8, "CalcularIMC(peso, talla)", "SynchCall", -125, "Synchronous", "", "Call", ""),
            (3, el_obj_cons_bll8, el_gui_c08, "", "Return", -155, "Synchronous", "", "Call", "1"),
            (4, el_gui_c08, el_obj_strat_ctx, "EjecutarEvaluacion(edadMeses, sexo, peso, talla, imc)", "SynchCall", -190, "Synchronous", "", "Call", ""),
            (5, el_obj_strat_ctx, el_obj_strat, "Evaluar(edadMeses, sexo, peso, talla, imc)", "SynchCall", -225, "Synchronous", "", "Call", ""),
            (6, el_obj_strat, el_obj_strat_ctx, "", "Return", -255, "Synchronous", "", "Call", "1"),
            (7, el_obj_strat_ctx, el_gui_c08, "", "Return", -285, "Synchronous", "", "Call", "1"),
            (8, el_gui_c08, el_nutri_actor, "", "Return", -315, "Synchronous", "", "Call", "1"),

            # Guardado y persistencia atómica en DAL genérica
            (9, el_nutri_actor, el_gui_c08, "btnGuardarConsulta_Click()", "SynchCall", -355, "Synchronous", "", "Call", ""),
            (10, el_gui_c08, el_gui_c08, "ValidarCamposObligatorios()", "SynchCall", -390, "Synchronous", "", "Call", ""),
            (11, el_gui_c08, el_obj_cons_bll8, "RegistrarConsultaCompleta(idPaciente, dniNutri, edad, peso, talla, pc, ...)", "SynchCall", -425, "Synchronous", "", "Call", ""),
            (12, el_obj_cons_bll8, el_obj_cons_bll8, "ValidarRangosAntropometricos()", "SynchCall", -460, "Synchronous", "", "Call", ""),
            (13, el_obj_cons_bll8, el_obj_cons_bll8, "CalcularDVConsulta()", "SynchCall", -495, "Synchronous", "", "Call", ""),
            (14, el_obj_cons_bll8, el_obj_dal8, "GuardarConsultaCompleta(...) [1 DB Hit: Transacción Atómica]", "SynchCall", -530, "Synchronous", "", "Call", ""),
            (15, el_obj_dal8, el_obj_cons_bll8, "", "Return", -565, "Synchronous", "", "Call", "1"),
            (16, el_obj_cons_bll8, el_obj_bit8, "RegistrarEvento(1, 'Registro Consulta Nutricional...', dniNutri, 'SeguimientoNutricional')", "SynchCall", -600, "Synchronous", "", "Call", ""),
            (17, el_obj_bit8, el_obj_dal8, "GuardarBitacora(bitacora)", "SynchCall", -635, "Synchronous", "", "Call", ""),
            (18, el_obj_dal8, el_obj_bit8, "", "Return", -665, "Synchronous", "", "Call", "1"),
            (19, el_obj_bit8, el_obj_cons_bll8, "", "Return", -695, "Synchronous", "", "Call", "1"),
            (20, el_obj_cons_bll8, el_obj_dv8, "RecalcularYPersistir()", "SynchCall", -730, "Synchronous", "", "Call", ""),
            (21, el_obj_dv8, el_obj_dal8, "PersistirIntegridadCompleta(resumen, filasDV)", "SynchCall", -765, "Synchronous", "", "Call", ""),
            (22, el_obj_dal8, el_obj_dv8, "", "Return", -795, "Synchronous", "", "Call", "1"),
            (23, el_obj_dv8, el_obj_cons_bll8, "", "Return", -825, "Synchronous", "", "Call", "1"),
            (24, el_obj_cons_bll8, el_gui_c08, "", "Return", -855, "Synchronous", "", "Call", "1"),
            (25, el_gui_c08, el_gui_c08, "Close()", "SynchCall", -885, "Synchronous", "", "Call", ""),
            (26, el_gui_c08, el_nutri_actor, "", "Return", -915, "Synchronous", "", "Call", "1")
        ]

        build_sequence_diagram(
            uc08, "CUN08: Diagrama de Secuencia",
            "Diagrama de Secuencia CUN08 (Registrar Consulta): UI modelada como Boundary (GUI). Nutricionista ingresa mediciones con cálculo en vivo vía BLL y Strategy Pattern (OMS). Al presionar Guardar, GUI dispara hacia BLL -> persistencia atómica en DAL con 1 único hit transaccional, auditoría en BitacoraBLL hacia DAL y recálculo de integridad en DigitoVerificadorBLL hacia DAL.",
            lifelines_c08, messages_c08,
            [
                os.path.join(dss_dir, "CUN08 Registrar Consulta.png"),
                os.path.join(scratch_dir, "cun08_secuencia.png"),
                os.path.join(artifact_dir, "cun08_secuencia.png")
            ],
            lifeline_bottom=-980
        )

        # --- DSS CUN09: Prescribir Plan ---
        el_gui_c09 = get_or_create_boundary(uc09, "GUI")
        el_obj_plan_bll9 = get_or_create_child_object(uc09, "PlanAlimentarioBLL_DNI101", "Object")
        el_obj_bit9 = get_or_create_child_object(uc09, "BitacoraBLL", "Object")
        el_obj_dv9 = get_or_create_child_object(uc09, "DigitoVerificadorBLL", "Object")
        el_obj_dal9 = get_or_create_child_object(uc09, "DAL", "Object")

        lifelines_c09 = [
            (el_nutri_actor,    20,   120,  70),
            (el_gui_c09,        160,  240,  200),
            (el_obj_plan_bll9,  290,  490,  390),
            (el_obj_bit9,       530,  660,  595),
            (el_obj_dv9,        700,  850,  775),
            (el_obj_dal9,       890,  1010, 950)
        ]

        messages_c09 = [
            # Cálculo de macronutrientes interactivo
            (1, el_nutri_actor, el_gui_c09, "numMacronutrientes_ValueChanged(pctCarb, pctProt, pctGrasas)", "SynchCall", -90, "Synchronous", "", "Call", ""),
            (2, el_gui_c09, el_gui_c09, "CalcularGramosYValidarSuma100()", "SynchCall", -125, "Synchronous", "", "Call", ""),
            (3, el_gui_c09, el_nutri_actor, "", "Return", -155, "Synchronous", "", "Call", "1"),

            # Guardado de plan alimentario en DAL genérica
            (4, el_nutri_actor, el_gui_c09, "btnGuardarPlan_Click()", "SynchCall", -195, "Synchronous", "", "Call", ""),
            (5, el_gui_c09, el_gui_c09, "ValidarCamposObligatorios()", "SynchCall", -230, "Synchronous", "", "Call", ""),
            (6, el_gui_c09, el_obj_plan_bll9, "PrescribirPlan(idConsulta, calorias, pctCarb, pctProt, pctGrasas, pautas, metas)", "SynchCall", -265, "Synchronous", "", "Call", ""),
            (7, el_obj_plan_bll9, el_obj_plan_bll9, "ValidarDistribucionCalorica()", "SynchCall", -300, "Synchronous", "", "Call", ""),
            (8, el_obj_plan_bll9, el_obj_plan_bll9, "CalcularDVPlan()", "SynchCall", -335, "Synchronous", "", "Call", ""),
            (9, el_obj_plan_bll9, el_obj_dal9, "GuardarPlanAlimentario(...) [1 DB Hit]", "SynchCall", -370, "Synchronous", "", "Call", ""),
            (10, el_obj_dal9, el_obj_plan_bll9, "", "Return", -405, "Synchronous", "", "Call", "1"),
            (11, el_obj_plan_bll9, el_obj_bit9, "RegistrarEvento(1, 'Prescripción de Plan Alimentario...', dniNutri, 'SeguimientoNutricional')", "SynchCall", -440, "Synchronous", "", "Call", ""),
            (12, el_obj_bit9, el_obj_dal9, "GuardarBitacora(bitacora)", "SynchCall", -475, "Synchronous", "", "Call", ""),
            (13, el_obj_dal9, el_obj_bit9, "", "Return", -505, "Synchronous", "", "Call", "1"),
            (14, el_obj_bit9, el_obj_plan_bll9, "", "Return", -535, "Synchronous", "", "Call", "1"),
            (15, el_obj_plan_bll9, el_obj_dv9, "RecalcularYPersistir()", "SynchCall", -570, "Synchronous", "", "Call", ""),
            (16, el_obj_dv9, el_obj_dal9, "PersistirIntegridadCompleta(resumen, filasDV)", "SynchCall", -605, "Synchronous", "", "Call", ""),
            (17, el_obj_dal9, el_obj_dv9, "", "Return", -635, "Synchronous", "", "Call", "1"),
            (18, el_obj_dv9, el_obj_plan_bll9, "", "Return", -665, "Synchronous", "", "Call", "1"),
            (19, el_obj_plan_bll9, el_gui_c09, "", "Return", -695, "Synchronous", "", "Call", "1"),
            (20, el_gui_c09, el_gui_c09, "Close()", "SynchCall", -725, "Synchronous", "", "Call", ""),
            (21, el_gui_c09, el_nutri_actor, "", "Return", -755, "Synchronous", "", "Call", "1")
        ]

        build_sequence_diagram(
            uc09, "CUN09: Diagrama de Secuencia",
            "Diagrama de Secuencia CUN09 (Prescribir Plan): UI modelada como Boundary (GUI). Validación de macronutrientes interactiva (100%), GUI dispara hacia PlanAlimentarioBLL -> persistencia en DAL genérica con 1 solo hit, registro en BitacoraBLL hacia DAL y recálculo en DigitoVerificadorBLL hacia DAL.",
            lifelines_c09, messages_c09,
            [
                os.path.join(dss_dir, "CUN09 Prescribir Plan.png"),
                os.path.join(scratch_dir, "cun09_secuencia.png"),
                os.path.join(artifact_dir, "cun09_secuencia.png")
            ],
            lifeline_bottom=-820
        )

        # =====================================================================
        # 4. BUILD CLASS DIAGRAMS (CUN06, CUN07, CUN08, CUN09)
        # =====================================================================
        print("\n--- 4. Building Class Diagrams for CUN06, CUN07, CUN08, CUN09 ---")

        def build_class_diagram(uc_el, diag_name, notes, coords_list, out_pngs):
            diag = get_or_create_diagram(pkg4, diag_name, "Logical", parent_id=uc_el.ElementID)
            diag.Notes = notes
            diag.Update()

            clear_diagram_objects(diag)
            for el, left, top, right, bottom in coords_list:
                do = diag.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
                do.ElementID = el.ElementID
                do.left = left
                do.right = right
                do.top = -top
                do.bottom = -bottom
                do.Update()

            diag.DiagramObjects.Refresh()
            diag.Update()
            apply_class_diagram_style(diag.DiagramID)
            export_diagram(diag, out_pngs)

        # CUN06 Class Diagram
        build_class_diagram(
            uc06, "CUN06: Diagrama de Clases (BE, BLL y DAL)",
            "Diagrama de Clases CUN06 (Seleccionar Paciente): 3 capas estrictas (BE, BLL, DAL). DAL desacoplada de BE.",
            [
                (el_pac_be,  50, 40,  500, 300),
                (el_pac_bll, 50, 370, 500, 600),
                (el_pac_dal, 50, 670, 500, 930)
            ],
            [
                os.path.join(clases_dir, "CUN06 Seleccionar Paciente.png"),
                os.path.join(scratch_dir, "cun06_clases.png"),
                os.path.join(artifact_dir, "cun06_clases.png")
            ]
        )

        # CUN07 Class Diagram
        build_class_diagram(
            uc07, "CUN07: Diagrama de Clases (BE, BLL y DAL)",
            "Diagrama de Clases CUN07 (Graficar Diagnóstico): BEs de diagnóstico y mediciones, BLL orquesta con CurvasOMSData, DAL desacoplada.",
            [
                # Tier 1: BEs
                (el_consulta_be, 50,   40,  480,  320),
                (el_medicion_be, 540,  40,  920,  280),
                (el_diag_be,     980,  40,  1340, 240),
                (el_alerta_be,   1400, 40,  1760, 240),
                # Tier 2: BLLs
                (el_consulta_bll, 50,  380, 800,  600),
                (el_curvas_oms,   900, 380, 1500, 540),
                # Tier 3: DALs
                (el_consulta_dal, 50,  670, 1250, 930)
            ],
            [
                os.path.join(clases_dir, "CUN07 Graficar Diagnostico.png"),
                os.path.join(scratch_dir, "cun07_clases.png"),
                os.path.join(artifact_dir, "cun07_clases.png")
            ]
        )

        # CUN08 Class Diagram
        build_class_diagram(
            uc08, "CUN08: Diagrama de Clases (BE, BLL y DAL)",
            "Diagrama de Clases CUN08 (Registrar Consulta): 3 capas completas con Bitácora y Dígito Verificador. DAL desacoplada.",
            [
                # Tier 1: BEs
                (el_consulta_be, 50,   40,  480,  320),
                (el_medicion_be, 540,  40,  920,  280),
                (el_diag_be,     980,  40,  1340, 240),
                (el_alerta_be,   1400, 40,  1760, 240),
                # Tier 2: BLLs
                (el_consulta_bll, 50,   380, 800,  620),
                (el_bit_bll,      1350, 380, 1850, 580),
                (el_dv_bll,       1950, 380, 2450, 580),
                # Tier 3: DALs
                (el_consulta_dal, 50,   690, 1250, 970),
                (el_bit_dal,      1350, 690, 1850, 940),
                (el_dv_dal,       1950, 690, 2500, 970)
            ],
            [
                os.path.join(clases_dir, "CUN08 Registrar Consulta.png"),
                os.path.join(scratch_dir, "cun08_clases.png"),
                os.path.join(artifact_dir, "cun08_clases.png")
            ]
        )

        # CUN09 Class Diagram
        build_class_diagram(
            uc09, "CUN09: Diagrama de Clases (BE, BLL y DAL)",
            "Diagrama de Clases CUN09 (Prescribir Plan): Capas BE, BLL, DAL con auditoría de Bitácora y Dígito Verificador.",
            [
                # Tier 1: BE
                (el_plan_be,  50,   40,  500,  290),
                # Tier 2: BLLs
                (el_plan_bll, 50,   360, 520,  580),
                (el_bit_bll,  570,  360, 1000, 560),
                (el_dv_bll,   1050, 360, 1500, 560),
                # Tier 3: DALs
                (el_plan_dal, 50,   660, 520,  880),
                (el_bit_dal,  570,  660, 1000, 900),
                (el_dv_dal,   1050, 660, 1550, 930)
            ],
            [
                os.path.join(clases_dir, "CUN09 Prescribir Plan.png"),
                os.path.join(scratch_dir, "cun09_clases.png"),
                os.path.join(artifact_dir, "cun09_clases.png")
            ]
        )

        # =====================================================================
        # 5. BUILD DER DIAGRAMS (CUN06, CUN07, CUN08, CUN09, PN2 GENERAL DER)
        # =====================================================================
        print("\n--- 5. Building DER Diagrams for PN2 ---")

        def build_der_diagram(pkg_target, diag_name, notes, parent_id, coords_list, out_pngs):
            diag = get_or_create_diagram(pkg_target, diag_name, "Logical", parent_id=parent_id)
            diag.Notes = notes
            diag.Update()

            clear_diagram_objects(diag)
            for el, left, top, right, bottom in coords_list:
                do = diag.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
                do.ElementID = el.ElementID
                do.left = left
                do.right = right
                do.top = -top
                do.bottom = -bottom
                do.Update()

            diag.DiagramObjects.Refresh()
            diag.Update()
            apply_der_style(diag.DiagramID)
            export_diagram(diag, out_pngs)

        # DER CUN06
        build_der_diagram(
            pkg4, "CUN06: DER (Modelo Relacional)",
            "DER CUN06 (Seleccionar Paciente): Pacientes_DNI101, Usuarios, Bitacora, DV con notación Information Engineering.",
            uc06.ElementID,
            [
                (tab_paciente, 50,  40,  420, 360),
                (tab_dv,       50,  430, 420, 560),
                (tab_usuario,  520, 40,  880, 350),
                (tab_bitacora, 520, 430, 880, 640)
            ],
            [
                os.path.join(der_dir, "CUN06 DER (Modelo Relacional).png"),
                os.path.join(scratch_dir, "cun06_der.png"),
                os.path.join(artifact_dir, "cun06_der.png")
            ]
        )

        # DER CUN07
        build_der_diagram(
            pkg4, "CUN07: DER (Modelo Relacional)",
            "DER CUN07 (Graficar Diagnóstico): Pacientes_DNI101, ConsultasNutricionales_DNI101, MedicionesAntropometricas_DNI101, DiagnosticosNutricionales_DNI101 y AlertasClinicas_DNI101.",
            uc07.ElementID,
            [
                (tab_paciente,     50,  40,  400, 360),
                (tab_consultas,    470, 40,  840, 380),
                (tab_mediciones,   900, 40,  1260,330),
                (tab_diagnosticos, 470, 440, 840, 680),
                (tab_alertas,      900, 440, 1260,680)
            ],
            [
                os.path.join(der_dir, "CUN07 DER (Modelo Relacional).png"),
                os.path.join(scratch_dir, "cun07_der.png"),
                os.path.join(artifact_dir, "cun07_der.png")
            ]
        )

        # DER CUN08
        build_der_diagram(
            pkg4, "CUN08: DER (Modelo Relacional)",
            "DER CUN08 (Registrar Consulta): ConsultasNutricionales_DNI101 y sus tablas dependientes, más Pacientes_DNI101, Usuarios, Bitacora y DV.",
            uc08.ElementID,
            [
                (tab_paciente,     50,  40,  400, 360),
                (tab_consultas,    460, 40,  840, 380),
                (tab_mediciones,   900, 40,  1260,330),
                (tab_diagnosticos, 900, 390, 1260,630),
                (tab_alertas,      1320,390, 1680,630),
                (tab_usuario,      460, 440, 820, 720),
                (tab_bitacora,     50,  440, 400, 650),
                (tab_dv,           50,  700, 400, 830)
            ],
            [
                os.path.join(der_dir, "CUN08 DER (Modelo Relacional).png"),
                os.path.join(scratch_dir, "cun08_der.png"),
                os.path.join(artifact_dir, "cun08_der.png")
            ]
        )

        # DER CUN09
        build_der_diagram(
            pkg4, "CUN09: DER (Modelo Relacional)",
            "DER CUN09 (Prescribir Plan): PlanesAlimentarios_DNI101 vinculado a ConsultasNutricionales_DNI101 y Pacientes_DNI101, con Usuarios, Bitacora y DV.",
            uc09.ElementID,
            [
                (tab_paciente,  50,  40,  400, 360),
                (tab_consultas, 460, 40,  840, 380),
                (tab_planes,    900, 40,  1280,360),
                (tab_usuario,   460, 440, 820, 720),
                (tab_bitacora,  50,  440, 400, 650),
                (tab_dv,        50,  700, 400, 830)
            ],
            [
                os.path.join(der_dir, "CUN09 DER (Modelo Relacional).png"),
                os.path.join(scratch_dir, "cun09_der.png"),
                os.path.join(artifact_dir, "cun09_der.png")
            ]
        )

        # DER General de PN2 (en Package 7)
        build_der_diagram(
            pkg7, "Proceso de Negocio 2 DER",
            "Diagrama Entidad-Relación General de PN2 (Seguimiento Nutricional Pediátrico): Integración de Pacientes, Consultas, Mediciones, Diagnósticos, Alertas y Planes, con seguridad (Usuarios, Bitácora, DV).",
            0,
            [
                (tab_paciente,     50,   40,  400,  360),
                (tab_consultas,    470,  40,  850,  380),
                (tab_mediciones,   920,  40,  1280, 330),
                (tab_planes,       1350, 40,  1730, 360),
                (tab_diagnosticos, 920,  390, 1280, 630),
                (tab_alertas,      1350, 390, 1730, 630),
                (tab_usuario,      470,  450, 830,  740),
                (tab_bitacora,     50,   450, 400,  670),
                (tab_dv,           50,   720, 400,  850)
            ],
            [
                os.path.join(der_dir, "Proceso de Negocio 2 DER.png"),
                os.path.join(scratch_dir, "pn2_der.png"),
                os.path.join(artifact_dir, "pn2_der.png")
            ]
        )

        # =====================================================================
        # 6. BUILD GENERAL CLASS DIAGRAM FOR PN2 (WITH STRATEGY PATTERN)
        # =====================================================================
        print("\n--- 6. Building Diagrama de Clases General de PN2 (Strategy Pattern) ---")
        diag_pn2_gen = get_or_create_diagram(pkg7, "Proceso de Negocio 2 General", "Logical", parent_id=0)
        diag_pn2_gen.Notes = "Diagrama de Clases General del Proceso de Negocio 2 (Seguimiento Nutricional Pediátrico): Arquitectura multicapa con patrón Strategy (IEvaluadorNutricionalStrategy_DNI101, EvaluacionIMCEdadStrategy, EvaluacionPesoEdadStrategy, EvaluacionTallaEdadStrategy), entidades de dominio, servicios BLL y persistencia desacoplada en DAL."
        diag_pn2_gen.Update()

        clear_diagram_objects(diag_pn2_gen)

        coords_pn2_gen = [
            # Top Band: Strategy Pattern & OMS Evaluation
            (el_strategy_iface, 800,  40,  1280, 200),
            (el_context_eval,   240,  40,  700,  200),
            (el_resultado_oms,  1380, 40,  1760, 240),
            (el_strat_imc,      400,  270, 780,  400),
            (el_strat_peso,     840,  270, 1220, 400),
            (el_strat_talla,    1280, 270, 1660, 400),

            # Mid-Upper Band: Domain Entities (BEs)
            (el_pac_be,        50,   460, 450,  720),
            (el_consulta_be,   500,  460, 920,  740),
            (el_medicion_be,   970,  460, 1340, 700),
            (el_diag_be,       1390, 460, 1740, 680),
            (el_alerta_be,     1790, 460, 2140, 680),
            (el_plan_be,       2190, 460, 2580, 710),

            # Mid-Lower Band: Business Logic Layer (BLLs)
            (el_pac_bll,       50,   800, 460,  1030),
            (el_consulta_bll,  520,  800, 1250, 1040),
            (el_curvas_oms,    1350, 800, 1850, 980),
            (el_plan_bll,      1950, 800, 2450, 1020),
            (el_bit_bll,       2550, 800, 3000, 1000),
            (el_dv_bll,        3100, 800, 3600, 1000),

            # Bottom Band: Data Access Layer (DALs)
            (el_pac_dal,       50,   1110, 480,  1370),
            (el_consulta_dal,  520,  1110, 1250, 1380),
            (el_plan_dal,      1950, 1110, 2450, 1330),
            (el_bit_dal,       2550, 1110, 3000, 1320),
            (el_dv_dal,        3100, 1110, 3600, 1380)
        ]

        for el, left, top, right, bottom in coords_pn2_gen:
            do = diag_pn2_gen.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
            do.ElementID = el.ElementID
            do.left = left
            do.right = right
            do.top = -top
            do.bottom = -bottom
            do.Update()

        diag_pn2_gen.DiagramObjects.Refresh()
        diag_pn2_gen.Update()
        apply_class_diagram_style(diag_pn2_gen.DiagramID)
        export_diagram(diag_pn2_gen, [
            os.path.join(clases_dir, "Proceso de Negocio 2 General.png"),
            os.path.join(scratch_dir, "pn2_general.png"),
            os.path.join(artifact_dir, "pn2_general.png")
        ])

        print("\n=======================================================")
        print("=== PN2 COMPLETE SCRIPT EXECUTED SUCCESSFULLY! ===")
        print("=======================================================")

    except Exception as ex:
        print(f"\n[FATAL ERROR]: {ex}")
        import traceback
        traceback.print_exc()

    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    build_pn2_complete()
