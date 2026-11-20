import win32com.client
import os
import sys
import uuid
import xml.etree.ElementTree as ET

EAP_PATH = r"C:\Users\Danie\Desktop\GIT\Diagramas\TD.EAP"
EXPORT_DIR = r"C:\Users\Danie\Desktop\GIT\Diagramas\Exported"
SCRATCH_DIR = r"C:\Users\Danie\Desktop\GIT\scratch"

def log(msg):
    print(f"[INFO] {msg}", flush=True)

def get_or_create_package(parent_pkg, name):
    for i in range(parent_pkg.Packages.Count):
        p = parent_pkg.Packages.GetAt(i)
        if p.Name == name:
            return p
    new_pkg = parent_pkg.Packages.AddNew(name, "Package")
    new_pkg.Update()
    parent_pkg.Packages.Refresh()
    log(f"Created Package: {name} (ID: {new_pkg.PackageID})")
    return new_pkg

def get_or_create_element(ea, pkg, name, elem_type="Class", stereotype=""):
    for i in range(pkg.Elements.Count):
        el = pkg.Elements.GetAt(i)
        if el.Name == name and el.Type == elem_type:
            if stereotype is not None and el.Stereotype != stereotype:
                el.Stereotype = stereotype
                el.Update()
            return el
    el = pkg.Elements.AddNew(name, elem_type)
    if stereotype:
        el.Stereotype = stereotype
    el.Update()
    pkg.Elements.Refresh()
    log(f"Created Element [{elem_type}] '{name}' (ID: {el.ElementID})")
    return el

def populate_table_columns(el, cols):
    # Clear existing attributes and methods
    for i in range(el.Attributes.Count - 1, -1, -1):
        el.Attributes.Delete(i)
    el.Attributes.Refresh()
    for i in range(el.Methods.Count - 1, -1, -1):
        el.Methods.Delete(i)
    el.Methods.Refresh()

    for c_name, c_type, c_stereo in cols:
        attr = el.Attributes.AddNew(c_name, c_type)
        attr.Visibility = "Public"
        if c_stereo:
            attr.Stereotype = c_stereo
        attr.Update()
    el.Attributes.Refresh()
    el.Update()

def populate_unified_class(el, attrs, methods):
    for i in range(el.Attributes.Count - 1, -1, -1):
        el.Attributes.Delete(i)
    el.Attributes.Refresh()

    for a_name, a_type, a_vis in attrs:
        attr = el.Attributes.AddNew(a_name, a_type)
        attr.Visibility = a_vis
        attr.Update()
    el.Attributes.Refresh()

    for i in range(el.Methods.Count - 1, -1, -1):
        el.Methods.Delete(i)
    el.Methods.Refresh()

    for m_name, m_ret, m_params in methods:
        meth = el.Methods.AddNew(m_name, m_ret)
        meth.Visibility = "Public"
        meth.ReturnType = m_ret
        meth.Update()
        for p_name, p_type in m_params:
            param = meth.Parameters.AddNew(p_name, p_type)
            param.Update()
        meth.Parameters.Refresh()
        meth.Update()
    el.Methods.Refresh()
    el.Update()

def ensure_diagram_object(diag, el, left, top, right, bottom):
    for i in range(diag.DiagramObjects.Count):
        dobj = diag.DiagramObjects.GetAt(i)
        if dobj.ElementID == el.ElementID:
            dobj.left = left
            dobj.top = -top
            dobj.right = right
            dobj.bottom = -bottom
            dobj.Update()
            return dobj
    dobj = diag.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
    dobj.ElementID = el.ElementID
    dobj.left = left
    dobj.top = -top
    dobj.right = right
    dobj.bottom = -bottom
    dobj.Update()
    diag.DiagramObjects.Refresh()
    return dobj

# ==============================================================================
# 1. BUILD DER GENERAL DEL SISTEMA
# ==============================================================================
def build_der_general(ea):
    log("=== BUILDING DER GENERAL DEL SISTEMA ===")
    pkg_domain = ea.GetPackageByID(5) # Domain Model
    pkg_der = get_or_create_package(pkg_domain, "DER General Completo")

    # Define all 24 tables with exact columns and PK/FK/Criptografía stereotypes
    tables_spec = {
        "Pacientes_DNI101": [
            ("IdPaciente_DNI101", "INT", "PK"),
            ("DniNiño_DNI101", "VARCHAR(100)", "UNIQUE, AES-256"),
            ("Nombre_DNI101", "VARCHAR(100)", "NOT NULL"),
            ("Apellido_DNI101", "VARCHAR(100)", "NOT NULL"),
            ("FechaNacimiento_DNI101", "DATETIME2(7)", "NOT NULL"),
            ("Sexo_DNI101", "VARCHAR(10)", "NULL"),
            ("ObraSocial_DNI101", "VARCHAR(100)", "NULL"),
            ("Telefono_DNI101", "VARCHAR(50)", "NULL"),
            ("Email_DNI101", "VARCHAR(100)", "NULL"),
            ("Direccion_DNI101", "VARCHAR(200)", "NULL"),
            ("Activo_DNI101", "BIT", "NOT NULL"),
            ("DV", "VARCHAR(255)", "DVH")
        ],
        "Paciente_Emails_DNI101": [
            ("IdEmail", "INT", "PK"),
            ("IdPaciente", "INT", "FK"),
            ("Email", "VARCHAR(150)", "NOT NULL"),
            ("EsPrincipal", "BIT", "NOT NULL"),
            ("Activo", "BIT", "NOT NULL")
        ],
        "Paciente_Telefonos_DNI101": [
            ("IdTelefono", "INT", "PK"),
            ("IdPaciente", "INT", "FK"),
            ("Numero", "VARCHAR(50)", "NOT NULL"),
            ("Tipo", "VARCHAR(20)", "NOT NULL"),
            ("EsPrincipal", "BIT", "NOT NULL"),
            ("Activo", "BIT", "NOT NULL")
        ],
        "Pacientes_Cambios_DNI101": [
            ("IdCambio", "INT", "PK"),
            ("IdPaciente_DNI101", "INT", "FK"),
            ("DniNiño_DNI101", "VARCHAR(100)", "AES-256"),
            ("Nombre_DNI101", "VARCHAR(100)", "NOT NULL"),
            ("Apellido_DNI101", "VARCHAR(100)", "NOT NULL"),
            ("Telefono_DNI101", "VARCHAR(50)", "NULL"),
            ("Email_DNI101", "VARCHAR(100)", "NULL"),
            ("Direccion_DNI101", "VARCHAR(200)", "NULL"),
            ("Activo_DNI101", "BIT", "NOT NULL"),
            ("FechaCambio", "DATETIME2(7)", "NOT NULL"),
            ("UsuarioDni", "INT", "FK"),
            ("TipoOperacion", "VARCHAR(20)", "NOT NULL"),
            ("Act", "BIT", "NOT NULL")
        ],
        "Turnos_DNI101": [
            ("IdTurno_DNI101", "INT", "PK"),
            ("CodigoTurno_DNI101", "VARCHAR(50)", "UNIQUE"),
            ("FechaTurno_DNI101", "DATETIME2(7)", "NOT NULL"),
            ("HoraTurno_DNI101", "TIME(7)", "NOT NULL"),
            ("MotivoConsulta_DNI101", "VARCHAR(255)", "NOT NULL"),
            ("EstadoTurno_DNI101", "VARCHAR(50)", "NOT NULL"),
            ("IdPaciente_DNI101", "INT", "FK"),
            ("DniNutricionista_DNI101", "INT", "FK"),
            ("IdBloque_DNI101", "INT", "FK"),
            ("DV", "VARCHAR(255)", "DVH")
        ],
        "BloquesHorarios_DNI101": [
            ("IdBloque_DNI101", "INT", "PK"),
            ("IdAgenda_DNI101", "INT", "FK"),
            ("HoraInicio_DNI101", "TIME(7)", "NOT NULL"),
            ("HoraFin_DNI101", "TIME(7)", "NOT NULL"),
            ("EstadoBloque_DNI101", "VARCHAR(50)", "NOT NULL"),
            ("DV", "VARCHAR(255)", "DVH")
        ],
        "AgendasMedicas_DNI101": [
            ("IdAgenda_DNI101", "INT", "PK"),
            ("Fecha_DNI101", "DATE", "NOT NULL"),
            ("EstadoAgenda_DNI101", "VARCHAR(50)", "NOT NULL"),
            ("DniNutricionista_DNI101", "INT", "FK"),
            ("DV", "VARCHAR(255)", "DVH")
        ],
        "ConsultasNutricionales_DNI101": [
            ("IdConsulta_DNI101", "INT", "PK"),
            ("IdPaciente_DNI101", "INT", "FK"),
            ("DniNutricionista_DNI101", "INT", "FK"),
            ("IdTurno_DNI101", "INT", "FK"),
            ("FechaControl_DNI101", "DATETIME2(7)", "NOT NULL"),
            ("EdadMeses_DNI101", "INT", "NOT NULL"),
            ("TipoLactancia_DNI101", "VARCHAR(50)", "NULL"),
            ("AlimentacionComplementaria_DNI101", "VARCHAR(255)", "NULL"),
            ("Alergias_DNI101", "VARCHAR(255)", "NULL"),
            ("AntecedentesFamiliares_DNI101", "VARCHAR(255)", "NULL"),
            ("Observaciones_DNI101", "VARCHAR(500)", "NULL"),
            ("DV", "VARCHAR(255)", "DVH")
        ],
        "ConsultasNutricionales_Cambios_DNI101": [
            ("IdCambio", "INT", "PK"),
            ("IdConsulta_DNI101", "INT", "FK"),
            ("IdPaciente_DNI101", "INT", "FK"),
            ("DniNutricionista_DNI101", "INT", "FK"),
            ("FechaControl_DNI101", "DATETIME2(7)", "NOT NULL"),
            ("EdadMeses_DNI101", "INT", "NOT NULL"),
            ("TipoLactancia_DNI101", "VARCHAR(50)", "NULL"),
            ("AlimentacionComplementaria_DNI101", "VARCHAR(255)", "NULL"),
            ("Alergias_DNI101", "VARCHAR(255)", "NULL"),
            ("AntecedentesFamiliares_DNI101", "VARCHAR(255)", "NULL"),
            ("Observaciones_DNI101", "VARCHAR(MAX)", "NULL"),
            ("PesoKg_DNI101", "DECIMAL(5,2)", "NULL"),
            ("TallaCm_DNI101", "DECIMAL(5,2)", "NULL"),
            ("PerimetroCefalicoCm_DNI101", "DECIMAL(5,2)", "NULL"),
            ("IMC_DNI101", "DECIMAL(5,2)", "NULL"),
            ("ClasificacionOMS_DNI101", "VARCHAR(100)", "NULL"),
            ("FechaCambio", "DATETIME2(7)", "NOT NULL"),
            ("UsuarioDni", "INT", "FK"),
            ("TipoOperacion", "VARCHAR(20)", "NOT NULL"),
            ("Act", "BIT", "NOT NULL")
        ],
        "MedicionesAntropometricas_DNI101": [
            ("IdMedicion_DNI101", "INT", "PK"),
            ("IdConsulta_DNI101", "INT", "FK"),
            ("PesoKg_DNI101", "DECIMAL(5,2)", "NOT NULL"),
            ("TallaCm_DNI101", "DECIMAL(5,2)", "NOT NULL"),
            ("PerimetroCefalicoCm_DNI101", "DECIMAL(5,2)", "NULL"),
            ("CircunferenciaCinturaCm_DNI101", "DECIMAL(5,2)", "NULL"),
            ("IMC_DNI101", "DECIMAL(5,2)", "NOT NULL"),
            ("DV", "VARCHAR(255)", "DVH")
        ],
        "DiagnosticosNutricionales_DNI101": [
            ("IdDiagnostico_DNI101", "INT", "PK"),
            ("IdConsulta_DNI101", "INT", "FK"),
            ("ClasificacionOMS_DNI101", "VARCHAR(100)", "NOT NULL"),
            ("DetallesClinicos_DNI101", "VARCHAR(500)", "NULL"),
            ("RequiereAlerta_DNI101", "BIT", "NOT NULL"),
            ("DV", "VARCHAR(255)", "DVH")
        ],
        "AlertasClinicas_DNI101": [
            ("IdAlerta_DNI101", "INT", "PK"),
            ("IdConsulta_DNI101", "INT", "FK"),
            ("TipoAlerta_DNI101", "VARCHAR(50)", "NOT NULL"),
            ("Severidad_DNI101", "VARCHAR(20)", "NOT NULL"),
            ("MensajeAlerta_DNI101", "VARCHAR(255)", "NOT NULL"),
            ("FechaGeneracion_DNI101", "DATETIME2(7)", "NOT NULL"),
            ("DV", "VARCHAR(255)", "DVH")
        ],
        "PlanesAlimentarios_DNI101": [
            ("IdPlan_DNI101", "INT", "PK"),
            ("IdConsulta_DNI101", "INT", "FK"),
            ("RequerimientoCalorico_DNI101", "DECIMAL(6,2)", "NOT NULL"),
            ("PctCarbohidratos_DNI101", "DECIMAL(4,2)", "NOT NULL"),
            ("PctProteinas_DNI101", "DECIMAL(4,2)", "NOT NULL"),
            ("PctGrasas_DNI101", "DECIMAL(4,2)", "NOT NULL"),
            ("PautasFamiliares_DNI101", "VARCHAR(1000)", "NULL"),
            ("MetasSalud_DNI101", "VARCHAR(500)", "NULL"),
            ("DV", "VARCHAR(255)", "DVH")
        ],
        "Usuarios": [
            ("DNI", "INT", "PK"),
            ("Nombre", "VARCHAR(50)", "NOT NULL"),
            ("Apellido", "VARCHAR(50)", "NOT NULL"),
            ("NombreDeUsuario", "VARCHAR(50)", "UNIQUE"),
            ("Contraseña", "VARCHAR(255)", "BCrypt Irreversible"),
            ("Email", "VARCHAR(100)", "NOT NULL"),
            ("Id_Perfil", "INT", "FK"),
            ("Bloqueado", "BIT", "NOT NULL"),
            ("Estado", "BIT", "NOT NULL"),
            ("Intentos", "INT", "NOT NULL"),
            ("Nombre_Idioma", "VARCHAR(25)", "NOT NULL"),
            ("DV", "VARCHAR(255)", "DVH")
        ],
        "Bitacora": [
            ("Numero", "INT", "PK"),
            ("Fecha", "DATETIME2(7)", "NOT NULL"),
            ("DNI", "INT", "FK"),
            ("Modulo", "VARCHAR(50)", "NOT NULL"),
            ("Descripcion", "VARCHAR(255)", "NOT NULL"),
            ("Criticidad", "INT", "NOT NULL")
        ],
        "DV": [
            ("Tabla", "VARCHAR(100)", "PK"),
            ("DVH", "VARCHAR(255)", "NOT NULL"),
            ("DVV", "VARCHAR(255)", "NOT NULL")
        ],
        "Perfil": [
            ("Id_Perfil", "INT", "PK"),
            ("Nombre", "VARCHAR(50)", "NOT NULL")
        ],
        "Perfil_Familia": [
            ("Id_Perfil", "INT", "PK, FK"),
            ("Id_Familia", "INT", "PK, FK")
        ],
        "Perfil_Permiso": [
            ("Id_Perfil", "INT", "PK, FK"),
            ("Id_Permiso", "INT", "PK, FK")
        ],
        "Familia": [
            ("Id_Familia", "INT", "PK"),
            ("Nombre", "VARCHAR(50)", "NOT NULL")
        ],
        "Familia_Familia": [
            ("Id_Familia_Padre", "INT", "PK, FK"),
            ("Id_Familia_Hijo", "INT", "PK, FK")
        ],
        "Permiso_Familia": [
            ("Id_Familia", "INT", "PK, FK"),
            ("Id_Permiso", "INT", "PK, FK")
        ],
        "Permiso": [
            ("Id_Permiso", "INT", "PK"),
            ("Nombre", "VARCHAR(50)", "NOT NULL")
        ],
        "Permiso_Boton": [
            ("Id_Permiso", "INT", "PK, FK"),
            ("Nombre_Boton", "VARCHAR(100)", "PK")
        ]
    }

    t_els = {}
    for t_name, cols in tables_spec.items():
        el = get_or_create_element(ea, pkg_der, t_name, "Class", "table")
        populate_table_columns(el, cols)
        t_els[t_name] = el

    # Clear existing connectors among these table elements
    all_t_ids = ",".join(str(e.ElementID) for e in t_els.values())
    ea.Execute(f"DELETE FROM t_connector WHERE Start_Object_ID IN ({all_t_ids}) AND End_Object_ID IN ({all_t_ids})")
    for e in t_els.values():
        e.Connectors.Refresh()

    # Add FK Associations (Crow's Foot)
    fk_relations = [
        ("Pacientes_DNI101", "Paciente_Emails_DNI101", "1", "0..*", "FK_Emails_Paciente"),
        ("Pacientes_DNI101", "Paciente_Telefonos_DNI101", "1", "0..*", "FK_Telefonos_Paciente"),
        ("Pacientes_DNI101", "Pacientes_Cambios_DNI101", "1", "0..*", "FK_Cambios_Paciente"),
        ("Usuarios", "Pacientes_Cambios_DNI101", "1", "0..*", "FK_CambiosPac_Usuario"),
        ("Pacientes_DNI101", "Turnos_DNI101", "1", "0..*", "FK_Turnos_Paciente"),
        ("Usuarios", "Turnos_DNI101", "1", "0..*", "FK_Turnos_Nutricionista"),
        ("BloquesHorarios_DNI101", "Turnos_DNI101", "1", "0..1", "FK_Turnos_Bloque"),
        ("AgendasMedicas_DNI101", "BloquesHorarios_DNI101", "1", "1..*", "FK_Bloques_Agenda"),
        ("Usuarios", "AgendasMedicas_DNI101", "1", "0..*", "FK_Agendas_Nutricionista"),
        ("Pacientes_DNI101", "ConsultasNutricionales_DNI101", "1", "0..*", "FK_Consultas_Paciente"),
        ("Usuarios", "ConsultasNutricionales_DNI101", "1", "0..*", "FK_Consultas_Nutricionista"),
        ("Turnos_DNI101", "ConsultasNutricionales_DNI101", "0..1", "0..1", "FK_Consultas_Turno"),
        ("ConsultasNutricionales_DNI101", "ConsultasNutricionales_Cambios_DNI101", "1", "0..*", "FK_Cambios_Consulta"),
        ("Pacientes_DNI101", "ConsultasNutricionales_Cambios_DNI101", "1", "0..*", "FK_CambiosCons_Paciente"),
        ("Usuarios", "ConsultasNutricionales_Cambios_DNI101", "1", "0..*", "FK_CambiosCons_Usuario"),
        ("ConsultasNutricionales_DNI101", "MedicionesAntropometricas_DNI101", "1", "1", "FK_Medicion_Consulta"),
        ("ConsultasNutricionales_DNI101", "DiagnosticosNutricionales_DNI101", "1", "1", "FK_Diagnostico_Consulta"),
        ("ConsultasNutricionales_DNI101", "AlertasClinicas_DNI101", "1", "0..*", "FK_Alertas_Consulta"),
        ("ConsultasNutricionales_DNI101", "PlanesAlimentarios_DNI101", "1", "0..1", "FK_Plan_Consulta"),
        ("Usuarios", "Bitacora", "1", "0..*", "FK_Bitacora_Usuario"),
        ("Perfil", "Usuarios", "1", "0..*", "FK_Usuarios_Perfil"),
        ("Perfil", "Perfil_Familia", "1", "0..*", "FK_PerfilFam_Perfil"),
        ("Familia", "Perfil_Familia", "1", "0..*", "FK_PerfilFam_Familia"),
        ("Perfil", "Perfil_Permiso", "1", "0..*", "FK_PerfilPerm_Perfil"),
        ("Permiso", "Perfil_Permiso", "1", "0..*", "FK_PerfilPerm_Permiso"),
        ("Familia", "Familia_Familia", "1", "0..*", "FK_FamFam_Padre"),
        ("Familia", "Permiso_Familia", "1", "0..*", "FK_PermFam_Familia"),
        ("Permiso", "Permiso_Familia", "1", "0..*", "FK_PermFam_Permiso"),
        ("Permiso", "Permiso_Boton", "1", "0..*", "FK_PermBoton_Permiso")
    ]

    for src_name, dst_name, scard, dcard, cname in fk_relations:
        src_el = t_els[src_name]
        dst_el = t_els[dst_name]
        c = src_el.Connectors.AddNew(cname, "Association")
        c.SupplierID = dst_el.ElementID
        c.Stereotype = "FK"
        c.Direction = "Source -> Destination"
        c.ClientEnd.Cardinality = scard
        c.SupplierEnd.Cardinality = dcard
        c.Update()
        src_el.Connectors.Refresh()

    # Update Diagram 67 ('DER General del Sistema' in Package 5) AND create/update 'DER General del Sistema' in Package 7
    diag_der_67 = ea.GetDiagramByID(67)
    diag_der_67.Name = "DER General del Sistema"
    diag_der_67.Notes = "Modelo Entidad-Relación (DER) General Completo: incluye PN1, PN2, Bitácora de Cambios, Contactos 1:N, Criptografía Reversible (AES-256) e Irreversible (BCrypt), Auditoría, DV y Perfiles."
    diag_der_67.Update()

    for i in range(diag_der_67.DiagramObjects.Count - 1, -1, -1):
        diag_der_67.DiagramObjects.Delete(i)
    diag_der_67.DiagramObjects.Refresh()

    # Layout for 24 tables
    der_layout = [
        # Zone 1: PN1 & Contactos 1:N
        ("Paciente_Telefonos_DNI101",              40,   40,  320,  210),
        ("Paciente_Emails_DNI101",                 40,  260,  320,  420),
        ("Pacientes_DNI101",                      400,   40,  730,  360),
        ("Turnos_DNI101",                         810,   40, 1130,  330),
        ("BloquesHorarios_DNI101",               1210,   40, 1510,  220),
        ("AgendasMedicas_DNI101",                1590,   40, 1890,  210),

        # Zone 3: Bitacora de Cambios
        ("Pacientes_Cambios_DNI101",               40,  480,  350,  830),
        ("ConsultasNutricionales_Cambios_DNI101", 400,  430,  740,  900),

        # Zone 2: PN2 Seguimiento Nutricional
        ("ConsultasNutricionales_DNI101",         810,  400, 1170,  740),
        ("MedicionesAntropometricas_DNI101",     1250,  280, 1610,  520),
        ("DiagnosticosNutricionales_DNI101",     1670,  280, 2020,  490),
        ("PlanesAlimentarios_DNI101",            1250,  570, 1610,  830),
        ("AlertasClinicas_DNI101",               1670,  570, 2020,  790),

        # Zone 4: Seguridad, Irreversible, Auditoría, DV y Perfiles Composite
        ("DV",                                     40,  940,  320, 1080),
        ("Bitacora",                              400,  960,  710, 1160),
        ("Usuarios",                              810,  820, 1140, 1160),
        ("Perfil",                               1230,  900, 1470, 1020),
        ("Perfil_Familia",                       1530,  880, 1780, 1000),
        ("Familia",                              1840,  880, 2080, 1000),
        ("Familia_Familia",                      2140,  880, 2400, 1000),
        ("Perfil_Permiso",                       1230, 1070, 1480, 1190),
        ("Permiso_Familia",                      1530, 1070, 1780, 1190),
        ("Permiso",                              1840, 1070, 2080, 1190),
        ("Permiso_Boton",                        2140, 1070, 2400, 1190),
    ]

    for t_name, l, t, r, b in der_layout:
        ensure_diagram_object(diag_der_67, t_els[t_name], l, t, r, b)

    # Set Information Engineering (Crow's Foot) notation
    ea.Execute(f"UPDATE t_diagram SET StyleEx = 'TConnectorNotation=Information Engineering;ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;', AttPub = True, AttPri = True, AttPro = True WHERE Diagram_ID = {diag_der_67.DiagramID}")
    diag_der_67.Update()
    ea.ReloadDiagram(diag_der_67.DiagramID)

    proj = ea.GetProjectInterface()
    os.makedirs(EXPORT_DIR, exist_ok=True)
    out_der_png = os.path.join(EXPORT_DIR, "DER_General_Sistema_NutriEvolve.png")
    res_der = proj.PutDiagramImageToFile(diag_der_67.DiagramGUID, out_der_png, 1)
    log(f"Exported DER General PNG: {out_der_png} (Result: {res_der})")


# ==============================================================================
# 2. BUILD DIAGRAMA DE CLASES GENERAL (UNIFICADO SIN BE/BLL/DAL SEPARADAS)
# ==============================================================================
def build_clases_general(ea):
    log("=== BUILDING DIAGRAMA DE CLASES GENERAL UNIFICADO ===")
    pkg_class_model = ea.GetPackageByID(7) # Class Model
    pkg_unificado = get_or_create_package(pkg_class_model, "Modelo de Clases General Unificado")

    # Define all 28 unified domain/service classes (attributes + methods combined)
    classes_spec = [
        # --- PN1: Pacientes, Contactos 1:N, XML, Turnos, Estados, Agenda, Bloques ---
        ("Paciente_DNI101", "Class", "", [
            ("IdPaciente_DNI101", "int", "Private"),
            ("DniNiño_DNI101", "string", "Private"),
            ("Nombre_DNI101", "string", "Private"),
            ("Apellido_DNI101", "string", "Private"),
            ("Telefono_DNI101", "string", "Private"),
            ("Email_DNI101", "string", "Private"),
            ("Direccion_DNI101", "string", "Private"),
            ("FechaNacimiento_DNI101", "DateTime", "Private"),
            ("Sexo_DNI101", "string", "Private"),
            ("ObraSocial_DNI101", "string", "Private"),
            ("Activo_DNI101", "bool", "Private"),
            ("Emails_DNI101", "List<PacienteEmail_DNI101>", "Private"),
            ("Telefonos_DNI101", "List<PacienteTelefono_DNI101>", "Private"),
            ("DV", "string", "Private")
        ], [
            ("ObtenerEdadMeses", "int", []),
            ("RegistrarPaciente", "int", [("nombre", "string"), ("apellido", "string"), ("dniNiño", "string"), ("telefono", "string"), ("email", "string"), ("obraSocial", "string")]),
            ("ModificarPaciente", "bool", [("paciente", "Paciente_DNI101")]),
            ("ObtenerPacientePorDNI", "Paciente_DNI101", [("dniNiño", "string")]),
            ("ObtenerPorId", "Paciente_DNI101", [("idPaciente", "int")]),
            ("ListarPacientes", "List<Paciente_DNI101>", []),
            ("GuardarMaestro", "int", [("dni", "string"), ("apellidos", "string"), ("nombres", "string"), ("email", "string"), ("celular", "string"), ("direccion", "string")]),
            ("ModificarMaestro", "bool", [("idPaciente", "int"), ("dni", "string"), ("apellidos", "string"), ("nombres", "string"), ("email", "string"), ("celular", "string"), ("direccion", "string")]),
            ("EliminarLogico", "bool", [("idPaciente", "int")]),
            ("DesbloquearLogico", "bool", [("idPaciente", "int")]),
            ("AgregarEmail1N", "void", [("idPaciente", "int"), ("email", "string"), ("esPrincipal", "bool")]),
            ("EliminarEmailLogico1N", "void", [("idEmail", "int")]),
            ("AgregarTelefono1N", "void", [("idPaciente", "int"), ("numero", "string"), ("tipo", "string"), ("esPrincipal", "bool")]),
            ("EliminarTelefonoLogico1N", "void", [("idTelefono", "int")]),
            ("ImportarClientesDesdeXml", "void", [("clientes", "List<ClienteXmlItem>")]),
            ("CifrarDniAES256", "string", [("dni", "string")]),
            ("DescifrarDniAES256", "string", [("dniCifrado", "string")])
        ]),

        ("PacienteEmail_DNI101", "Class", "", [
            ("IdEmail", "int", "Private"),
            ("IdPaciente", "int", "Private"),
            ("Email", "string", "Private"),
            ("EsPrincipal", "bool", "Private"),
            ("Activo", "bool", "Private")
        ], [
            ("ObtenerEmails1N", "List<PacienteEmail_DNI101>", [("idPaciente", "int")]),
            ("AgregarEmail1N", "void", [("idPaciente", "int"), ("email", "string"), ("esPrincipal", "bool")]),
            ("EliminarEmailLogico1N", "void", [("idEmail", "int")])
        ]),

        ("PacienteTelefono_DNI101", "Class", "", [
            ("IdTelefono", "int", "Private"),
            ("IdPaciente", "int", "Private"),
            ("Numero", "string", "Private"),
            ("Tipo", "string", "Private"),
            ("EsPrincipal", "bool", "Private"),
            ("Activo", "bool", "Private")
        ], [
            ("ObtenerTelefonos1N", "List<PacienteTelefono_DNI101>", [("idPaciente", "int")]),
            ("AgregarTelefono1N", "void", [("idPaciente", "int"), ("numero", "string"), ("tipo", "string"), ("esPrincipal", "bool")]),
            ("EliminarTelefonoLogico1N", "void", [("idTelefono", "int")])
        ]),

        ("ClientesXmlContainer", "Class", "Serializable XML", [
            ("Clientes", "List<ClienteXmlItem>", "Public")
        ], [
            ("SerializarAXml", "void", [("rutaArchivo", "string")]),
            ("DeserializarDesdeXml", "ClientesXmlContainer", [("rutaArchivo", "string")])
        ]),

        ("ClienteXmlItem", "Class", "Serializable XML", [
            ("DNI", "string", "Public"),
            ("APELLIDOS", "string", "Public"),
            ("NOMBRES", "string", "Public"),
            ("EMAIL", "string", "Public"),
            ("CELULAR", "string", "Public"),
            ("DIRECCION", "string", "Public")
        ], []),

        ("Turno_DNI101", "Class", "Context", [
            ("IdTurno_DNI101", "int", "Private"),
            ("CodigoTurno_DNI101", "string", "Private"),
            ("FechaTurno_DNI101", "DateTime", "Private"),
            ("HoraTurno_DNI101", "TimeSpan", "Private"),
            ("MotivoConsulta_DNI101", "string", "Private"),
            ("EstadoTurno_DNI101", "string", "Private"),
            ("IdPaciente_DNI101", "int", "Private"),
            ("DniNutricionista_DNI101", "int", "Private"),
            ("IdBloque_DNI101", "int", "Private"),
            ("EstadoActual_DNI101", "IEstadoTurno_DNI101", "Private"),
            ("DV", "string", "Private")
        ], [
            ("CambiarEstado", "void", [("nuevoEstado", "IEstadoTurno_DNI101")]),
            ("Atender", "void", []),
            ("Cancelar", "void", [("motivo", "string")]),
            ("Reprogramar", "void", [("nuevaFecha", "DateTime"), ("nuevaHora", "TimeSpan"), ("nuevoIdBloque", "int")]),
            ("AgendarTurno", "bool", [("turno", "Turno_DNI101")]),
            ("ConfirmarTurno", "bool", [("idTurno", "int")]),
            ("ReprogramarTurno", "bool", [("idTurno", "int"), ("nuevaFecha", "DateTime"), ("nuevaHora", "TimeSpan"), ("nuevoIdBloque", "int")]),
            ("CancelarTurno", "bool", [("idTurno", "int"), ("motivo", "string")]),
            ("MarcarAsistencia", "bool", [("idTurno", "int")]),
            ("ListarTurnos", "List<Turno_DNI101>", [("fecha", "DateTime?"), ("estado", "string?")]),
            ("ObtenerPorId", "Turno_DNI101", [("idTurno", "int")])
        ]),

        ("IEstadoTurno_DNI101", "Interface", "State", [
            ("NombreEstado", "string", "Public")
        ], [
            ("Atender", "void", [("turno", "Turno_DNI101")]),
            ("Cancelar", "void", [("turno", "Turno_DNI101"), ("motivo", "string")]),
            ("Reprogramar", "void", [("turno", "Turno_DNI101"), ("nuevaFecha", "DateTime"), ("nuevaHora", "TimeSpan"), ("nuevoIdBloque", "int")])
        ]),

        ("TurnoSolicitadoState_DNI101", "Class", "ConcreteState", [], [
            ("Atender", "void", [("turno", "Turno_DNI101")]),
            ("Cancelar", "void", [("turno", "Turno_DNI101"), ("motivo", "string")]),
            ("Reprogramar", "void", [("turno", "Turno_DNI101"), ("nuevaFecha", "DateTime"), ("nuevaHora", "TimeSpan"), ("nuevoIdBloque", "int")])
        ]),

        ("TurnoConfirmadoState_DNI101", "Class", "ConcreteState", [], [
            ("Atender", "void", [("turno", "Turno_DNI101")]),
            ("Cancelar", "void", [("turno", "Turno_DNI101"), ("motivo", "string")]),
            ("Reprogramar", "void", [("turno", "Turno_DNI101"), ("nuevaFecha", "DateTime"), ("nuevaHora", "TimeSpan"), ("nuevoIdBloque", "int")])
        ]),

        ("TurnoAsistioState_DNI101", "Class", "ConcreteState", [], [
            ("Atender", "void", [("turno", "Turno_DNI101")]),
            ("Cancelar", "void", [("turno", "Turno_DNI101"), ("motivo", "string")]),
            ("Reprogramar", "void", [("turno", "Turno_DNI101"), ("nuevaFecha", "DateTime"), ("nuevaHora", "TimeSpan"), ("nuevoIdBloque", "int")])
        ]),

        ("TurnoCanceladoState_DNI101", "Class", "ConcreteState", [], [
            ("Atender", "void", [("turno", "Turno_DNI101")]),
            ("Cancelar", "void", [("turno", "Turno_DNI101"), ("motivo", "string")]),
            ("Reprogramar", "void", [("turno", "Turno_DNI101"), ("nuevaFecha", "DateTime"), ("nuevaHora", "TimeSpan"), ("nuevoIdBloque", "int")])
        ]),

        ("AgendaMedica_DNI101", "Class", "", [
            ("IdAgenda_DNI101", "int", "Private"),
            ("Fecha_DNI101", "DateTime", "Private"),
            ("EstadoAgenda_DNI101", "string", "Private"),
            ("DniNutricionista_DNI101", "int", "Private"),
            ("BloquesHorarios_DNI101", "List<BloqueHorario_DNI101>", "Private"),
            ("DV", "string", "Private")
        ], [
            ("GuardarAgenda", "int", [("agenda", "AgendaMedica_DNI101")]),
            ("ObtenerAgendaPorFecha", "AgendaMedica_DNI101", [("fecha", "DateTime"), ("dniNutricionista", "int")]),
            ("ActualizarEstadoBloque", "bool", [("idBloque", "int"), ("nuevoEstado", "string")]),
            ("ListarBloquesDisponibles", "List<BloqueHorario_DNI101>", [("fecha", "DateTime"), ("dniNutricionista", "int")])
        ]),

        ("BloqueHorario_DNI101", "Class", "", [
            ("IdBloque_DNI101", "int", "Private"),
            ("IdAgenda_DNI101", "int", "Private"),
            ("HoraInicio_DNI101", "TimeSpan", "Private"),
            ("HoraFin_DNI101", "TimeSpan", "Private"),
            ("EstadoBloque_DNI101", "string", "Private"),
            ("DescripcionHorario", "string", "Private"),
            ("DV", "string", "Private")
        ], [
            ("ToString", "string", [])
        ]),

        # --- BITÁCORA DE CAMBIOS (PACIENTES Y CONSULTAS) ---
        ("PacienteCambio_DNI101", "Class", "BitacoraCambios", [
            ("IdCambio", "int", "Private"),
            ("IdPaciente", "int", "Private"),
            ("Dni", "string", "Private"),
            ("Nombre", "string", "Private"),
            ("Apellido", "string", "Private"),
            ("Telefono", "string", "Private"),
            ("Email", "string", "Private"),
            ("Direccion", "string", "Private"),
            ("Activo", "bool", "Private"),
            ("FechaCambio", "DateTime", "Private"),
            ("UsuarioDni", "int", "Private"),
            ("TipoOperacion", "string", "Private"),
            ("Act", "bool", "Private")
        ], [
            ("ListarCambiosPacientes", "List<PacienteCambio_DNI101>", [("idPaciente", "int?"), ("dni", "string?"), ("nombre", "string?"), ("fechaIni", "DateTime?"), ("fechaFin", "DateTime?"), ("tipoOperacion", "string?")]),
            ("RevertirCambioPaciente", "bool", [("idCambio", "int"), ("usuarioDni", "int")]),
            ("NormalizarOperacion", "string", [("op", "string")])
        ]),

        ("ConsultaCambio_DNI101", "Class", "BitacoraCambios", [
            ("IdCambio", "int", "Private"),
            ("IdConsulta", "int", "Private"),
            ("IdPaciente", "int", "Private"),
            ("DniNutricionista", "int", "Private"),
            ("FechaControl", "DateTime", "Private"),
            ("EdadMeses", "int", "Private"),
            ("TipoLactancia", "string", "Private"),
            ("AlimentacionComplementaria", "string", "Private"),
            ("Alergias", "string", "Private"),
            ("AntecedentesFamiliares", "string", "Private"),
            ("Observaciones", "string", "Private"),
            ("PesoKg", "decimal?", "Private"),
            ("TallaCm", "decimal?", "Private"),
            ("PerimetroCefalico", "decimal?", "Private"),
            ("IMC", "decimal?", "Private"),
            ("ClasificacionOMS", "string", "Private"),
            ("FechaCambio", "DateTime", "Private"),
            ("UsuarioDni", "int", "Private"),
            ("TipoOperacion", "string", "Private"),
            ("Act", "bool", "Private")
        ], [
            ("ListarCambios", "List<ConsultaCambio_DNI101>", [("idConsulta", "int?"), ("nombrePaciente", "string?"), ("fechaIni", "DateTime?"), ("fechaFin", "DateTime?")]),
            ("ActivarVersionHistorica", "bool", [("idCambio", "int"), ("usuarioDni", "int")]),
            ("RevertirCambio", "bool", [("idCambio", "int"), ("usuarioDni", "int")])
        ]),

        # --- PN2: Seguimiento Nutricional Pediátrico, Antropometría, Planes, Strategy OMS ---
        ("ConsultaNutricional_DNI101", "Class", "", [
            ("IdConsulta_DNI101", "int", "Private"),
            ("IdPaciente_DNI101", "int", "Private"),
            ("DniNutricionista_DNI101", "int", "Private"),
            ("IdTurno_DNI101", "int?", "Private"),
            ("FechaControl_DNI101", "DateTime", "Private"),
            ("EdadMeses_DNI101", "int", "Private"),
            ("TipoLactancia_DNI101", "string", "Private"),
            ("AlimentacionComplementaria_DNI101", "string", "Private"),
            ("Alergias_DNI101", "string", "Private"),
            ("AntecedentesFamiliares_DNI101", "string", "Private"),
            ("Observaciones_DNI101", "string", "Private"),
            ("Medicion", "MedicionAntropometrica_DNI101", "Private"),
            ("Diagnostico", "DiagnosticoNutricional_DNI101", "Private"),
            ("Alertas", "List<AlertaClinica_DNI101>", "Private"),
            ("PlanAlimentario", "PlanAlimentario_DNI101", "Private"),
            ("DV", "string", "Private")
        ], [
            ("CalcularIMC", "decimal", [("pesoKg", "decimal"), ("tallaCm", "decimal")]),
            ("EvaluarEstadoNutricional", "ResultadoEvaluacionOMS", [("imc", "decimal"), ("edadMeses", "int"), ("sexo", "string")]),
            ("EvaluarPeso", "ResultadoEvaluacionOMS", [("pesoKg", "decimal"), ("edadMeses", "int"), ("sexo", "string")]),
            ("EvaluarTalla", "ResultadoEvaluacionOMS", [("tallaCm", "decimal"), ("edadMeses", "int"), ("sexo", "string")]),
            ("RegistrarConsulta", "int", [("idPaciente", "int"), ("edadMeses", "int"), ("sexo", "string"), ("pesoKg", "decimal"), ("tallaCm", "decimal"), ("perimetroCefalico", "decimal"), ("fechaControl", "DateTime")]),
            ("ModificarConsulta", "bool", [("idConsulta", "int"), ("idPaciente", "int"), ("edadMeses", "int"), ("sexo", "string"), ("pesoKg", "decimal"), ("tallaCm", "decimal")]),
            ("ObtenerHistorialPorPaciente", "List<ConsultaNutricional_DNI101>", [("idPaciente", "int")]),
            ("ObtenerUltimaConsultaPorPaciente", "ConsultaNutricional_DNI101", [("idPaciente", "int")])
        ]),

        ("MedicionAntropometrica_DNI101", "Class", "", [
            ("IdMedicion_DNI101", "int", "Private"),
            ("IdConsulta_DNI101", "int", "Private"),
            ("PesoKg_DNI101", "decimal", "Private"),
            ("TallaCm_DNI101", "decimal", "Private"),
            ("PerimetroCefalicoCm_DNI101", "decimal", "Private"),
            ("CircunferenciaCinturaCm_DNI101", "decimal", "Private"),
            ("IMC_DNI101", "decimal", "Private"),
            ("DV", "string", "Private")
        ], [
            ("CalcularIMC", "decimal", [])
        ]),

        ("DiagnosticoNutricional_DNI101", "Class", "", [
            ("IdDiagnostico_DNI101", "int", "Private"),
            ("IdConsulta_DNI101", "int", "Private"),
            ("ClasificacionOMS_DNI101", "string", "Private"),
            ("DetallesClinicos_DNI101", "string", "Private"),
            ("RequiereAlerta_DNI101", "bool", "Private"),
            ("DV", "string", "Private")
        ], [
            ("ObtenerResumenDiagnostico", "string", [])
        ]),

        ("AlertaClinica_DNI101", "Class", "", [
            ("IdAlerta_DNI101", "int", "Private"),
            ("IdConsulta_DNI101", "int", "Private"),
            ("TipoAlerta_DNI101", "string", "Private"),
            ("Severidad_DNI101", "string", "Private"),
            ("MensajeAlerta_DNI101", "string", "Private"),
            ("FechaGeneracion_DNI101", "DateTime", "Private"),
            ("DV", "string", "Private")
        ], []),

        ("PlanAlimentario_DNI101", "Class", "", [
            ("IdPlan_DNI101", "int", "Private"),
            ("IdConsulta_DNI101", "int", "Private"),
            ("RequerimientoCalorico_DNI101", "decimal", "Private"),
            ("PctCarbohidratos_DNI101", "decimal", "Private"),
            ("PctProteinas_DNI101", "decimal", "Private"),
            ("PctGrasas_DNI101", "decimal", "Private"),
            ("PautasFamiliares_DNI101", "string", "Private"),
            ("MetasSalud_DNI101", "string", "Private"),
            ("DV", "string", "Private")
        ], [
            ("PrescribirPlan", "int", [("idConsulta", "int"), ("requerimientoCalorico", "decimal"), ("pctCarbohidratos", "decimal"), ("pctProteinas", "decimal"), ("pctGrasas", "decimal"), ("pautasFamiliares", "string"), ("metasSalud", "string")]),
            ("ObtenerPlanPorConsulta", "PlanAlimentario_DNI101", [("idConsulta", "int")]),
            ("ObtenerUltimoPlanPorPaciente", "PlanAlimentario_DNI101", [("idPaciente", "int")])
        ]),

        ("CurvasOMSData_DNI101", "Class", "", [], [
            ("ObtenerCurvasOMS", "Dictionary<string, List<PointF>>", [("indicador", "string"), ("sexo", "string")]),
            ("ObtenerValoresReferencia", "(decimal, decimal, decimal)", [("indicador", "string"), ("edadMeses", "int"), ("sexo", "string")])
        ]),

        ("EvaluadorNutricionalContext_DNI101", "Class", "Context Strategy", [
            ("_estrategia", "IEvaluadorNutricionalStrategy_DNI101", "Private")
        ], [
            ("SetEstrategia", "void", [("estrategia", "IEvaluadorNutricionalStrategy_DNI101")]),
            ("EjecutarEvaluacion", "ResultadoEvaluacionOMS", [("valorMedido", "decimal"), ("edadMeses", "int"), ("sexo", "string")])
        ]),

        ("IEvaluadorNutricionalStrategy_DNI101", "Interface", "Strategy", [], [
            ("Evaluar", "ResultadoEvaluacionOMS", [("valorMedido", "decimal"), ("edadMeses", "int"), ("sexo", "string")])
        ]),

        ("EvaluacionIMCEdadStrategy", "Class", "ConcreteStrategy", [], [
            ("Evaluar", "ResultadoEvaluacionOMS", [("valorMedido", "decimal"), ("edadMeses", "int"), ("sexo", "string")])
        ]),

        ("EvaluacionPesoEdadStrategy", "Class", "ConcreteStrategy", [], [
            ("Evaluar", "ResultadoEvaluacionOMS", [("valorMedido", "decimal"), ("edadMeses", "int"), ("sexo", "string")])
        ]),

        ("EvaluacionTallaEdadStrategy", "Class", "ConcreteStrategy", [], [
            ("Evaluar", "ResultadoEvaluacionOMS", [("valorMedido", "decimal"), ("edadMeses", "int"), ("sexo", "string")])
        ]),

        ("ResultadoEvaluacionOMS", "Class", "", [
            ("ValorMedido", "decimal", "Public"),
            ("ZScoreEstimado", "decimal", "Public"),
            ("PercentilEstimado", "decimal", "Public"),
            ("Clasificacion", "string", "Public"),
            ("Detalles", "string", "Public"),
            ("RequiereAlerta", "bool", "Public"),
            ("SeveridadAlerta", "string", "Public"),
            ("MensajeAlerta", "string", "Public")
        ], []),

        # --- CRIPTOGRAFÍA REVERSIBLE E IRREVERSIBLE, SEGURIDAD, AUDITORÍA Y DV ---
        ("ServicioCriptografiaAES", "Class", "Reversible AES-256", [
            ("Clave256", "byte[]", "Private"),
            ("Iv128", "byte[]", "Private")
        ], [
            ("CifrarAES256", "string", [("textoPlano", "string")]),
            ("DescifrarAES256", "string", [("textoCifradoBase64", "string")]),
            ("EsTextoCifrado", "bool", [("texto", "string")])
        ]),

        ("ServicioBcrypt", "Class", "Irreversible BCrypt", [
            ("WorkFactor", "int", "Private")
        ], [
            ("HashearContraseña", "string", [("contraseña", "string")]),
            ("ValidarContraseña", "bool", [("contraseñaPlana", "string"), ("hashGuardado", "string")]),
            ("CalcularDV", "string", [("datos", "string")]),
            ("ValidarDV", "bool", [("datos", "string"), ("dvGuardado", "string")])
        ]),

        ("Usuario", "Class", "Seguridad", [
            ("DNI", "int", "Private"),
            ("Nombre", "string", "Private"),
            ("Apellido", "string", "Private"),
            ("NombreDeUsuario", "string", "Private"),
            ("Contraseña", "string", "Private"),
            ("Email", "string", "Private"),
            ("IdPerfil", "int", "Private"),
            ("Bloqueado", "bool", "Private"),
            ("Estado", "bool", "Private"),
            ("Intentos", "int", "Private"),
            ("Idioma", "string", "Private"),
            ("DV", "string", "Private")
        ], [
            ("Login", "bool", [("nombreDeUsuario", "string"), ("contraseñaPlana", "string")]),
            ("LogOut", "void", [("usuario", "Usuario")]),
            ("CrearUsuario", "void", [("usuario", "Usuario")]),
            ("ModificarUsuario", "void", [("usuario", "Usuario")]),
            ("CambiarContraseña", "void", [("usuario", "Usuario")]),
            ("CambiarEstado", "void", [("usuario", "Usuario")]),
            ("Desbloquear", "void", [("usuario", "Usuario")]),
            ("BuscarUsuario", "Usuario", [("nombreDeUsuario", "string")]),
            ("ListarUsuarios", "List<Usuario>", [])
        ]),

        ("Bitacora", "Class", "Auditoria", [
            ("Numero", "int", "Private"),
            ("Criticidad", "int", "Private"),
            ("Descripcion", "string", "Private"),
            ("DNI", "int", "Private"),
            ("Fecha", "DateTime", "Private"),
            ("Modulo", "string", "Private")
        ], [
            ("RegistrarBitacora", "bool", [("criticidad", "int"), ("descripcion", "string"), ("dni", "int"), ("modulo", "string")]),
            ("BuscarBitacora", "List<Bitacora>", [("desde", "DateTime"), ("hasta", "DateTime")]),
            ("VerBitacora", "List<Bitacora>", [])
        ]),

        ("DigitoVerificador", "Class", "Integridad DVH/DVV", [
            ("Tabla", "string", "Private"),
            ("DVH", "string", "Private"),
            ("DVV", "string", "Private"),
            ("NombreTablaGlobal", "string", "Private")
        ], [
            ("RecalcularYPersistir", "void", []),
            ("VerificarBaseDatos", "bool", []),
            ("ObtenerTablasAlteradas", "List<string>", []),
            ("ObtenerFilasCorruptas", "List<string>", [("tabla", "string")]),
            ("ObtenerDetalleCorrupcionCompleto", "Dictionary<string, List<string>>", [])
        ]),

        ("Perfil", "Class", "Composite Permisos", [
            ("IdPerfil", "int", "Private"),
            ("Nombre", "string", "Private"),
            ("Permisos", "List<string>", "Private")
        ], [
            ("CrearPerfil", "int", [("nombre", "string")]),
            ("AsignarFamilia", "void", [("idPerfil", "int"), ("idFamilia", "int")]),
            ("AsignarPatente", "void", [("idPerfil", "int"), ("idPermiso", "int")]),
            ("ObtenerPermisosUsuario", "List<string>", [("idPerfil", "int")])
        ])
    ]

    c_els = {}
    for c_name, c_type, c_stereo, attrs, methods in classes_spec:
        el = get_or_create_element(ea, pkg_unificado, c_name, c_type, c_stereo)
        populate_unified_class(el, attrs, methods)
        c_els[c_name] = el

    # Clear existing connectors among these unified classes
    all_c_ids = ",".join(str(e.ElementID) for e in c_els.values())
    ea.Execute(f"DELETE FROM t_connector WHERE Start_Object_ID IN ({all_c_ids}) AND End_Object_ID IN ({all_c_ids})")
    for e in c_els.values():
        e.Connectors.Refresh()

    # Helper to add UML relationship
    def add_rel(src_name, dst_name, rel_type, name="", scard="", dcard="", stereo="", agg_mode=0):
        src_el = c_els[src_name]
        dst_el = c_els[dst_name]
        if rel_type == "Realisation":
            u_guid = "{" + str(uuid.uuid4()).upper() + "}"
            ea.Execute(f"INSERT INTO t_connector (Connector_Type, Direction, Start_Object_ID, End_Object_ID, ea_guid) VALUES ('Realisation', 'Source -> Destination', {src_el.ElementID}, {dst_el.ElementID}, '{u_guid}')")
            src_el.Connectors.Refresh()
            return
        c = src_el.Connectors.AddNew(name, rel_type)
        c.SupplierID = dst_el.ElementID
        c.Direction = "Source -> Destination"
        if stereo:
            c.Stereotype = stereo
        if scard:
            c.ClientEnd.Cardinality = scard
        if dcard:
            c.SupplierEnd.Cardinality = dcard
        c.Update()
        src_el.Connectors.Refresh()
        if agg_mode == 2: # Composition (Strong)
            ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 2, DestIsAggregate = 0, SubType = 'Strong', Stereotype = null WHERE Connector_ID = {c.ConnectorID}")
        elif agg_mode == 1: # Aggregation (Weak)
            ea.Execute(f"UPDATE t_connector SET SourceIsAggregate = 1, DestIsAggregate = 0, SubType = 'Weak', Stereotype = null WHERE Connector_ID = {c.ConnectorID}")

    # 1. PN1 Relationships
    add_rel("Paciente_DNI101", "PacienteEmail_DNI101", "Aggregation", "posee 1:N", "1", "0..*", agg_mode=2)
    add_rel("Paciente_DNI101", "PacienteTelefono_DNI101", "Aggregation", "posee 1:N", "1", "0..*", agg_mode=2)
    add_rel("ClientesXmlContainer", "ClienteXmlItem", "Aggregation", "contiene", "1", "0..*", agg_mode=2)
    add_rel("Paciente_DNI101", "ClientesXmlContainer", "Dependency", "serializa/deserializa", stereo="use")
    add_rel("Paciente_DNI101", "Turno_DNI101", "Aggregation", "solicita", "1", "0..*", agg_mode=1)
    add_rel("Turno_DNI101", "BloqueHorario_DNI101", "Association", "reserva", "0..1", "1")
    add_rel("AgendaMedica_DNI101", "BloqueHorario_DNI101", "Aggregation", "compuesta por", "1", "1..*", agg_mode=2)
    add_rel("Usuario", "AgendaMedica_DNI101", "Aggregation", "gestiona", "1", "0..*", agg_mode=1)
    add_rel("Usuario", "Turno_DNI101", "Association", "atiende", "1", "0..*")
    add_rel("Turno_DNI101", "IEstadoTurno_DNI101", "Aggregation", "estadoActual", "1", "1", agg_mode=2)
    for st in ["TurnoSolicitadoState_DNI101", "TurnoConfirmadoState_DNI101", "TurnoAsistioState_DNI101", "TurnoCanceladoState_DNI101"]:
        add_rel(st, "IEstadoTurno_DNI101", "Realisation")

    # 2. Bitácora de Cambios Relationships
    add_rel("Paciente_DNI101", "PacienteCambio_DNI101", "Association", "registra historial", "1", "0..*")
    add_rel("ConsultaNutricional_DNI101", "ConsultaCambio_DNI101", "Association", "registra historial", "1", "0..*")
    add_rel("Usuario", "PacienteCambio_DNI101", "Association", "audita/revierte", "1", "0..*")
    add_rel("Usuario", "ConsultaCambio_DNI101", "Association", "audita/revierte", "1", "0..*")

    # 3. PN2 Relationships
    add_rel("Paciente_DNI101", "ConsultaNutricional_DNI101", "Aggregation", "historial clinico", "1", "0..*", agg_mode=1)
    add_rel("Usuario", "ConsultaNutricional_DNI101", "Association", "evalúa", "1", "0..*")
    add_rel("ConsultaNutricional_DNI101", "MedicionAntropometrica_DNI101", "Aggregation", "+Medicion", "1", "1", agg_mode=2)
    add_rel("ConsultaNutricional_DNI101", "DiagnosticoNutricional_DNI101", "Aggregation", "+Diagnostico", "1", "1", agg_mode=2)
    add_rel("ConsultaNutricional_DNI101", "AlertaClinica_DNI101", "Aggregation", "+Alertas", "1", "0..*", agg_mode=2)
    add_rel("ConsultaNutricional_DNI101", "PlanAlimentario_DNI101", "Aggregation", "+PlanAlimentario", "1", "0..1", agg_mode=2)
    add_rel("ConsultaNutricional_DNI101", "EvaluadorNutricionalContext_DNI101", "Dependency", "evalúa OMS", stereo="use")
    add_rel("ConsultaNutricional_DNI101", "CurvasOMSData_DNI101", "Dependency", "consulta curvas", stereo="use")
    add_rel("EvaluadorNutricionalContext_DNI101", "IEvaluadorNutricionalStrategy_DNI101", "Aggregation", "estrategia", "1", "1", agg_mode=1)
    add_rel("EvaluadorNutricionalContext_DNI101", "ResultadoEvaluacionOMS", "Dependency", "genera", stereo="use")
    add_rel("IEvaluadorNutricionalStrategy_DNI101", "ResultadoEvaluacionOMS", "Dependency", "retorna", stereo="use")
    for st_oms in ["EvaluacionIMCEdadStrategy", "EvaluacionPesoEdadStrategy", "EvaluacionTallaEdadStrategy"]:
        add_rel(st_oms, "IEvaluadorNutricionalStrategy_DNI101", "Realisation")

    # 4. Criptografía Reversible (AES-256), Irreversible (BCrypt), Auditoría, DV y Perfiles Relationships
    add_rel("Paciente_DNI101", "ServicioCriptografiaAES", "Dependency", "cifra/descifra DNI (Reversible)", stereo="use")
    add_rel("PacienteCambio_DNI101", "ServicioCriptografiaAES", "Dependency", "cifra/descifra DNI", stereo="use")
    add_rel("Usuario", "ServicioBcrypt", "Dependency", "hashea clave (Irreversible)", stereo="use")
    add_rel("DigitoVerificador", "ServicioBcrypt", "Dependency", "calcula/valida hash DV", stereo="use")
    add_rel("Usuario", "Perfil", "Association", "tiene asignado", "0..*", "1")
    add_rel("Usuario", "Bitacora", "Association", "registra eventos", "1", "0..*")
    add_rel("Paciente_DNI101", "Bitacora", "Dependency", "audita", stereo="use")
    add_rel("ConsultaNutricional_DNI101", "Bitacora", "Dependency", "audita", stereo="use")
    add_rel("Paciente_DNI101", "DigitoVerificador", "Dependency", "recalcula DV", stereo="use")
    add_rel("ConsultaNutricional_DNI101", "DigitoVerificador", "Dependency", "recalcula DV", stereo="use")
    add_rel("Usuario", "DigitoVerificador", "Dependency", "recalcula DV", stereo="use")

    # Create or update 'Diagrama de Clases General' in Package 7 (Class Model)
    diag_name = "Diagrama de Clases General"
    diag_gen = None
    for i in range(pkg_class_model.Diagrams.Count):
        d = pkg_class_model.Diagrams.GetAt(i)
        if d.Name == diag_name:
            diag_gen = d
            break
    if not diag_gen:
        diag_gen = pkg_class_model.Diagrams.AddNew(diag_name, "Logical")
        diag_gen.Update()
        pkg_class_model.Diagrams.Refresh()

    diag_gen.Notes = "Diagrama de Clases General Unificado (sin separar BE, BLL y DAL): incluye PN1, PN2, Bitácora de Cambios, Criptografía Reversible (AES-256) e Irreversible (BCrypt), Serialización XML, Patrones State y Strategy, Auditoría y Dígito Verificador."
    diag_gen.Update()

    for i in range(diag_gen.DiagramObjects.Count - 1, -1, -1):
        diag_gen.DiagramObjects.Delete(i)
    diag_gen.DiagramObjects.Refresh()

    # Layout for the 28 unified classes
    clases_layout = [
        # ROW 1: PN1 (XML, Contactos 1:N, Paciente, Turno, Bloque, Agenda)
        ("ClienteXmlItem",                        40,   40,  310,  220),
        ("ClientesXmlContainer",                  40,  270,  340,  410),
        ("PacienteTelefono_DNI101",              380,   40,  710,  240),
        ("PacienteEmail_DNI101",                 380,  280,  710,  470),
        ("Paciente_DNI101",                      770,   40, 1270,  560),
        ("Turno_DNI101",                        1340,   40, 1820,  490),
        ("BloqueHorario_DNI101",                1890,   40, 2230,  260),
        ("AgendaMedica_DNI101",                 1890,  310, 2310,  550),

        # ROW 2: Bitácora de Cambios & Patrón State (PN1)
        ("PacienteCambio_DNI101",                 40,  530,  490,  890),
        ("ConsultaCambio_DNI101",                530,  610,  990, 1050),
        ("IEstadoTurno_DNI101",                 1430,  550, 1760,  700),
        ("TurnoSolicitadoState_DNI101",         1060,  760, 1380,  900),
        ("TurnoConfirmadoState_DNI101",         1410,  760, 1730,  900),
        ("TurnoAsistioState_DNI101",            1760,  760, 2080,  900),
        ("TurnoCanceladoState_DNI101",          2110,  760, 2430,  900),

        # ROW 3: Seguridad, Criptografía Reversible (AES-256) e Irreversible (BCrypt), Usuario, Bitácora, DV, Perfil
        ("ServicioCriptografiaAES",               40,  950,  440, 1150),
        ("ServicioBcrypt",                        40, 1200,  440, 1410),
        ("DigitoVerificador",                     40, 1460,  460, 1710),
        ("Usuario",                              520, 1110,  920, 1520),
        ("Bitacora",                             520, 1560,  900, 1770),
        ("Perfil",                               950, 1560, 1310, 1770),

        # ROW 3-4 Right: PN2 (Consulta, Medición, Diagnóstico, Plan, Alerta, Strategy OMS, Curvas)
        ("ConsultaNutricional_DNI101",           980,  970, 1490, 1460),
        ("MedicionAntropometrica_DNI101",       1560,  960, 1930, 1210),
        ("DiagnosticoNutricional_DNI101",       1980,  960, 2340, 1180),
        ("PlanAlimentario_DNI101",              1560, 1260, 1960, 1540),
        ("AlertaClinica_DNI101",                2000, 1260, 2350, 1490),
        ("CurvasOMSData_DNI101",                2400,  960, 2810, 1140),
        ("ResultadoEvaluacionOMS",              2860,  960, 3200, 1210),
        ("EvaluadorNutricionalContext_DNI101",  2400, 1200, 2810, 1380),
        ("IEvaluadorNutricionalStrategy_DNI101",2860, 1260, 3230, 1400),
        ("EvaluacionIMCEdadStrategy",           2380, 1460, 2700, 1600),
        ("EvaluacionPesoEdadStrategy",          2730, 1460, 3050, 1600),
        ("EvaluacionTallaEdadStrategy",         3080, 1460, 3400, 1600),
    ]

    for c_name, l, t, r, b in clases_layout:
        ensure_diagram_object(diag_gen, c_els[c_name], l, t, r, b)

    ea.Execute(f"UPDATE t_diagram SET AttPub = True, AttPri = True, AttPro = True WHERE Diagram_ID = {diag_gen.DiagramID}")
    diag_gen.Update()
    ea.ReloadDiagram(diag_gen.DiagramID)

    proj = ea.GetProjectInterface()
    out_cls_png = os.path.join(EXPORT_DIR, "Diagrama_Clases_General_Unificado_NutriEvolve.png")
    res_cls = proj.PutDiagramImageToFile(diag_gen.DiagramGUID, out_cls_png, 1)
    log(f"Exported Clases General PNG: {out_cls_png} (Result: {res_cls})")

def main():
    ea = win32com.client.Dispatch("EA.Repository")
    opened = ea.OpenFile(EAP_PATH)
    if not opened:
        print("[ERROR] Could not open EA repository.")
        sys.exit(1)

    try:
        build_der_general(ea)
        build_clases_general(ea)
        log("ALL GENERAL DIAGRAMS BUILT AND EXPORTED SUCCESSFULLY!")
    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    main()
