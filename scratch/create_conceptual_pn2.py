import win32com.client

def create_conceptual_pn2(eap_path):
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(eap_path)
    
    # Usaremos el paquete 7: Class Model
    pkg = ea.GetPackageByID(7)
    
    def get_or_create_class(name, attributes):
        element = None
        for el in pkg.Elements:
            if el.Name == name and el.Type == "Class":
                element = el
                break
        if not element:
            element = pkg.Elements.AddNew(name, "Class")
            element.Update()
        
        # Eliminar atributos viejos
        for i in range(element.Attributes.Count - 1, -1, -1):
            element.Attributes.Delete(i)
        element.Attributes.Refresh()
        
        # Eliminar métodos (solo BE atributos)
        for i in range(element.Methods.Count - 1, -1, -1):
            element.Methods.Delete(i)
        element.Methods.Refresh()

        # Agregar atributos
        for attr_name, attr_type in attributes:
            attr = element.Attributes.AddNew(attr_name, attr_type)
            attr.Update()
        
        element.Update()
        return element

    # 1. ConsultaNutricionalBE_DNI101
    consulta_attrs = [
        ("IdConsulta_DNI101", "int"),
        ("IdPaciente_DNI101", "int"),
        ("DniNutricionista_DNI101", "int"),
        ("IdTurno_DNI101", "int"),
        ("FechaControl_DNI101", "DateTime"),
        ("EdadMeses_DNI101", "int"),
        ("TipoLactancia_DNI101", "string"),
        ("AlimentacionComplementaria_DNI101", "string"),
        ("Alergias_DNI101", "string"),
        ("AntecedentesFamiliares_DNI101", "string"),
        ("Observaciones_DNI101", "string"),
        ("DV", "string")
    ]
    consulta = get_or_create_class("ConsultaNutricionalBE_DNI101", consulta_attrs)

    # 2. MedicionAntropometricaBE_DNI101
    medicion_attrs = [
        ("IdMedicion_DNI101", "int"),
        ("IdConsulta_DNI101", "int"),
        ("PesoKg_DNI101", "double"),
        ("TallaCm_DNI101", "double"),
        ("PerimetroCefalicoCm_DNI101", "double"),
        ("CircunferenciaCinturaCm_DNI101", "double"),
        ("IMC_DNI101", "double"),
        ("DV", "string")
    ]
    medicion = get_or_create_class("MedicionAntropometricaBE_DNI101", medicion_attrs)

    # 3. ZScoreResultadoBE_DNI101
    zscore_attrs = [
        ("IdZScore_DNI101", "int"),
        ("IdConsulta_DNI101", "int"),
        ("ZPesoEdad_DNI101", "double"),
        ("ZTallaEdad_DNI101", "double"),
        ("ZIMCEdad_DNI101", "double"),
        ("ZPCEdad_DNI101", "double"),
        ("PercentilPeso_DNI101", "double"),
        ("PercentilTalla_DNI101", "double"),
        ("PercentilIMC_DNI101", "double"),
        ("DV", "string")
    ]
    zscore = get_or_create_class("ZScoreResultadoBE_DNI101", zscore_attrs)

    # 4. DiagnosticoNutricionalBE_DNI101
    diag_attrs = [
        ("IdDiagnostico_DNI101", "int"),
        ("IdConsulta_DNI101", "int"),
        ("ClasificacionOMS_DNI101", "string"),
        ("DetallesClinicos_DNI101", "string"),
        ("RequiereAlerta_DNI101", "bool"),
        ("DV", "string")
    ]
    diagnostico = get_or_create_class("DiagnosticoNutricionalBE_DNI101", diag_attrs)

    # 5. AlertaClinicaBE_DNI101
    alerta_attrs = [
        ("IdAlerta_DNI101", "int"),
        ("IdConsulta_DNI101", "int"),
        ("TipoAlerta_DNI101", "string"),
        ("Severidad_DNI101", "string"),
        ("MensajeAlerta_DNI101", "string"),
        ("FechaGeneracion_DNI101", "DateTime"),
        ("DV", "string")
    ]
    alerta = get_or_create_class("AlertaClinicaBE_DNI101", alerta_attrs)

    # 6. PlanAlimentarioBE_DNI101
    plan_attrs = [
        ("IdPlan_DNI101", "int"),
        ("IdConsulta_DNI101", "int"),
        ("RequerimientoCalorico_DNI101", "double"),
        ("PctCarbohidratos_DNI101", "double"),
        ("PctProteinas_DNI101", "double"),
        ("PctGrasas_DNI101", "double"),
        ("PautasFamiliares_DNI101", "string"),
        ("MetasSalud_DNI101", "string"),
        ("DV", "string")
    ]
    plan = get_or_create_class("PlanAlimentarioBE_DNI101", plan_attrs)

    # 7. Recordatorio24hBE_DNI101
    rec_attrs = [
        ("IdRecordatorio_DNI101", "int"),
        ("IdConsulta_DNI101", "int"),
        ("Desayuno_DNI101", "string"),
        ("Almuerzo_DNI101", "string"),
        ("Merienda_DNI101", "string"),
        ("Cena_DNI101", "string"),
        ("Colaciones_DNI101", "string"),
        ("FrecuenciaAlimentos_DNI101", "string"),
        ("DV", "string")
    ]
    recordatorio = get_or_create_class("Recordatorio24hBE_DNI101", rec_attrs)

    # Reutilizar PacienteBE_DNI101, UsuarioBE, TurnoBE_DNI101
    paciente = None
    usuario = None
    turno = None
    for el in pkg.Elements:
        if el.Name == "PacienteBE_DNI101": paciente = el
        if el.Name == "UsuarioBE": usuario = el
        if el.Name == "TurnoBE_DNI101": turno = el

    pkg.Elements.Refresh()

    # Limpiar conectores existentes de las nuevas clases de PN2
    pn2_classes = [consulta, medicion, zscore, diagnostico, alerta, plan, recordatorio]
    for cls in pn2_classes:
        for i in range(cls.Connectors.Count - 1, -1, -1):
            cls.Connectors.Delete(i)
        cls.Connectors.Refresh()

    # Helper para crear relaciones:
    # 1. Agregación (Weak Aggregation - Rombo blanco en Source)
    def create_aggregation(src, tgt, src_card, tgt_card, tgt_role):
        conn = src.Connectors.AddNew("", "Aggregation")
        conn.SupplierID = tgt.ElementID
        conn.Direction = "Source -> Destination"
        conn.ClientEnd.Cardinality = src_card
        conn.SupplierEnd.Cardinality = tgt_card
        conn.SupplierEnd.Role = tgt_role
        conn.Update()
        src.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 1, DestIsAggregate = 0, SubType = 'Weak', Stereotype = null WHERE Connector_ID = {conn.ConnectorID}")
        return conn

    # 2. Composición (Composite Aggregation - Rombo negro en Source)
    def create_composition(src, tgt, src_card, tgt_card, tgt_role):
        conn = src.Connectors.AddNew("", "Aggregation")
        conn.SupplierID = tgt.ElementID
        conn.Direction = "Source -> Destination"
        conn.ClientEnd.Cardinality = src_card
        conn.SupplierEnd.Cardinality = tgt_card
        conn.SupplierEnd.Role = tgt_role
        conn.Update()
        src.Connectors.Refresh()
        ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 2, DestIsAggregate = 0, SubType = 'Strong', Stereotype = null WHERE Connector_ID = {conn.ConnectorID}")
        return conn

    # 3. Uso (Dependency <<use>> - Línea punteada con flecha)
    def create_usage(src, tgt, role):
        conn = src.Connectors.AddNew("", "Dependency")
        conn.SupplierID = tgt.ElementID
        conn.Stereotype = "use"
        conn.Direction = "Source -> Destination"
        conn.SupplierEnd.Role = role
        conn.Update()
        src.Connectors.Refresh()
        return conn

    # CREACIÓN DE RELACIONES:
    # A) Agregaciones desde los actores/titulares hacia la Consulta:
    # Paciente agrega sus consultas históricas (rombo blanco en Paciente)
    if paciente:
        create_aggregation(paciente, consulta, "1", "0..*", "historialConsultas")
    
    # Nutricionista (UsuarioBE) realiza/agrega consultas (rombo blanco en UsuarioBE)
    if usuario:
        create_aggregation(usuario, consulta, "1", "0..*", "consultasRealizadas")

    # B) Uso: La consulta médica hace uso / referencia del Turno asignado
    if turno:
        create_usage(consulta, turno, "turnoOrigen")

    # C) Composiciones: ConsultaNutricionalBE_DNI101 compone todas las partes de la evaluación
    create_composition(consulta, medicion, "1", "1", "medicion")
    create_composition(consulta, zscore, "1", "1", "zscores")
    create_composition(consulta, diagnostico, "1", "1", "diagnostico")
    create_composition(consulta, alerta, "1", "0..1", "alertaClinica")
    create_composition(consulta, plan, "1", "1", "planAlimentario")
    create_composition(consulta, recordatorio, "1", "0..1", "recordatorio24h")

    # DIAGRAMA: "Diagrama Conceptual PN2"
    diag_name = "Diagrama Conceptual PN2"
    diagram = None
    for d in pkg.Diagrams:
        if d.Name == diag_name:
            diagram = d
            break
    if not diagram:
        diagram = pkg.Diagrams.AddNew(diag_name, "Logical")
        diagram.Update()

    # Limpiar diagrama
    for i in range(diagram.DiagramObjects.Count - 1, -1, -1):
        diagram.DiagramObjects.Delete(i)
    diagram.DiagramObjects.Refresh()

    def add_to_diagram(element, left, top, right, bottom):
        do = diagram.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
        do.ElementID = element.ElementID
        do.Update()

    # LAYOUT ARQUITECTÓNICO DEL DIAGRAMA CONCEPTUAL PN2:
    # Fila Superior: PacienteBE_DNI101 (izq) | TurnoBE_DNI101 (centro) | UsuarioBE (der)
    # Fila Media: ConsultaNutricionalBE_DNI101 (centro, abajo de Turno)
    # Fila Inferior 1 (Composiciones directas):
    #   MedicionAntropometrica (izq) | ZScoreResultado (centro) | DiagnosticoNutricional (der)
    # Fila Inferior 2 (Composiciones complementarias):
    #   AlertaClinica (izq) | PlanAlimentario (centro) | Recordatorio24h (der)

    if paciente:
        add_to_diagram(paciente, 60, -50, 310, -280)
    if turno:
        add_to_diagram(turno, 450, -50, 690, -280)
    if usuario:
        add_to_diagram(usuario, 830, -50, 1050, -240)

    # ConsultaNutricional en el centro
    add_to_diagram(consulta, 430, -360, 710, -640)

    # Fila Inferior 1
    add_to_diagram(medicion, 70, -720, 320, -910)
    add_to_diagram(zscore, 430, -720, 710, -950)
    add_to_diagram(diagnostico, 820, -720, 1070, -890)

    # Fila Inferior 2
    add_to_diagram(alerta, 70, -1000, 320, -1170)
    add_to_diagram(plan, 430, -1000, 710, -1200)
    add_to_diagram(recordatorio, 820, -1000, 1070, -1190)

    diagram.Update()

    # Exportar imagen para validación
    out_img = r'C:\Users\Danie\Desktop\GIT\TD\scratch\diagrama_conceptual_pn2.png'
    try:
        ea.GetProjectInterface().PutDiagramImageToFile(diagram.DiagramGUID, out_img, 1)
        print("Exported diagram to:", out_img)
    except Exception as e:
        print("Error exporting image:", e)

    ea.CloseFile()
    ea.Exit()
    print("Diagrama Conceptual PN2 created successfully in TD.EAP.")

if __name__ == '__main__':
    create_conceptual_pn2(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
