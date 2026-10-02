import win32com.client

def create_all_missing_elements():
    ea = win32com.client.Dispatch('EA.Repository')
    eap_path = r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP'
    ea.OpenFile(eap_path)

    pkg4 = ea.GetPackageByID(4)
    pkg7 = ea.GetPackageByID(7)

    # 1. CREATE MISSING TABLES IN PKG 4
    tables_def = {
        "Perfil": [
            ("ID_Perfil", "INT", True),
            ("Nombre", "VARCHAR(50)", False)
        ],
        "Familia": [
            ("ID_Familia", "INT", True),
            ("Nombre", "VARCHAR(50)", False)
        ],
        "Permiso": [
            ("ID_Permiso", "INT", True),
            ("Nombre", "VARCHAR(50)", False)
        ],
        "Perfil_Familia": [
            ("ID_Perfil", "INT", True),
            ("ID_Familia", "INT", True)
        ],
        "Perfil_Permiso": [
            ("ID_Perfil", "INT", True),
            ("ID_Permiso", "INT", True)
        ],
        "Familia_Familia": [
            ("ID_FamiliaPadre", "INT", True),
            ("ID_FamiliaHija", "INT", True)
        ],
        "Permiso_Familia": [
            ("ID_Permiso", "INT", True),
            ("ID_Familia", "INT", True)
        ],
        "Permiso_Boton": [
            ("ID_PermisoBoton", "INT", True),
            ("NombreFormulario", "VARCHAR(100)", False),
            ("NombreControl", "VARCHAR(100)", False),
            ("NombrePatente", "VARCHAR(100)", False)
        ]
    }

    created_tables = {}
    for tname, cols in tables_def.items():
        found = None
        for el in pkg4.Elements:
            if el.Name == tname and el.Stereotype == "table":
                found = el
                break
        if not found:
            el = pkg4.Elements.AddNew(tname, "Class")
            el.Stereotype = "table"
            el.Update()
            pkg4.Elements.Refresh()
            found = el
            print(f"Created table {tname} (ID: {el.ElementID})")
        else:
            print(f"Table {tname} already exists (ID: {found.ElementID})")
        
        # Add columns if not present
        existing_cols = {a.Name: a for a in found.Attributes}
        for cname, ctype, is_pk in cols:
            if cname not in existing_cols:
                att = found.Attributes.AddNew(cname, ctype)
                att.IsOrdered = is_pk
                att.Update()
        found.Attributes.Refresh()
        created_tables[tname] = found

    # Ensure Usuarios has ID_Perfil
    el_usuarios = ea.GetElementByID(431)
    user_cols = {a.Name: a for a in el_usuarios.Attributes}
    if "ID_Perfil" not in user_cols:
        att = el_usuarios.Attributes.AddNew("ID_Perfil", "INT")
        att.Update()
        el_usuarios.Attributes.Refresh()
        print("Added ID_Perfil column to Usuarios table")

    # 2. CREATE MISSING CLASSES IN PKG 7
    # BE Classes
    be_classes_def = {
        "EventoBE": {
            "atts": [
                ("Criticidad", "int"),
                ("Descripcion", "string"),
                ("Dni", "int"),
                ("Fecha", "DateTime"),
                ("Id_Evento", "int"),
                ("Modulo", "string")
            ],
            "meths": []
        },
        "Idioma": {
            "atts": [
                ("Nombre", "string"),
                ("Codigo", "string"),
                ("ArchivoJson", "string")
            ],
            "meths": []
        },
        "Perfil": {
            "atts": [
                ("Id", "int"),
                ("Nombre", "string")
            ],
            "meths": [
                ("EsCompuesto", "bool", [])
            ],
            "abstract": True
        },
        "FamiliaServices": {
            "atts": [
                ("Hijos", "List<Perfil>")
            ],
            "meths": [
                ("Agregar", "void", [("c", "Perfil")]),
                ("EsCompuesto", "bool", [])
            ]
        },
        "PatenteServices": {
            "atts": [],
            "meths": [
                ("EsCompuesto", "bool", [])
            ]
        }
    }

    # BLL Classes
    bll_classes_def = {
        "UsuarioBLL": {
            "meths": [
                ("CambiarEstado", "void", [("usuario", "UsuarioBE")]),
                ("CambiarContraseña", "void", [("usuario", "UsuarioBE")]),
                ("CrearUsuario", "void", [("usuario", "UsuarioBE")]),
                ("ListarUsuarios", "List<UsuarioBE>", []),
                ("Login", "bool", [("nombreDeUsuario", "string"), ("contraseñaPlana", "string")]),
                ("ObtenerIntentosFallidos", "int", [("nombreDeUsuario", "string")]),
                ("LogOut", "void", [("usuario", "UsuarioBE")]),
                ("ModificarUsuario", "void", [("usuario", "UsuarioBE")]),
                ("BuscarUsuario", "UsuarioBE", [("nombreDeUsuario", "string")]),
                ("Desbloquear", "void", [("user", "UsuarioBE")]),
                ("CambioDeIdiomaUser", "void", [("user", "UsuarioBE")]),
                ("GenerarCadenaParaDV", "string", [("usuario", "UsuarioBE")])
            ]
        },
        "IdiomaBLL": {
            "meths": [
                ("ObtenerIdiomas", "List<Idioma>", []),
                ("Traducir", "string", [("clave", "string")])
            ]
        },
        "BackupBLL": {
            "meths": [
                ("RealizarBackup", "void", [("ruta", "string")]),
                ("RealizarRestore", "void", [("ruta", "string")])
            ]
        },
        "PerfilBLL": {
            "meths": [
                ("CrearNuevoPerfil", "void", [("nombrePerfil", "string")]),
                ("ObtenerPerfiles", "List<Perfil>", []),
                ("ObtenerArbolPerfil", "FamiliaServices", [("idPerfil", "int")]),
                ("AgregarFamiliaAlPerfil", "void", [("idPerfil", "int"), ("idFamilia", "int"), ("nombrePerfil", "string"), ("nombreFamilia", "string")]),
                ("AgregarPermisoAFamilia", "void", [("idPerfil", "int"), ("idPermiso", "int"), ("nombrePermiso", "string")]),
                ("AgregarPermisoAPerfil", "void", [("idPerfil", "int"), ("idPermiso", "int"), ("nombrePermiso", "string"), ("nombrePerfil", "string")]),
                ("EliminarPerfil", "void", [("idPerfil", "int"), ("nombrePerfil", "string")]),
                ("EliminarPermisoAPerfil", "void", [("idPerfil", "int"), ("idPermiso", "int"), ("nombrePermiso", "string"), ("nombrePerfil", "string")]),
                ("EliminarFamiliaDePerfil", "void", [("idPerfil", "int"), ("idFamilia", "int"), ("nombrePerfil", "string"), ("nombreFamilia", "string")]),
                ("ObtenerComponentesTotales", "List<Perfil>", []),
                ("ObtenerFamiliasPerfil", "List<Perfil>", []),
                ("ObtenerPermisosPerfil", "List<Perfil>", []),
                ("ObtenerIdPerfilPorNombre", "int", [("nombreRol", "string")])
            ]
        },
        "FamiliaBLL": {
            "meths": [
                ("CrearNuevaFamilia", "int", [("nombreFamilia", "string")]),
                ("ObtenerTodasLasFamilias", "List<FamiliaServices>", []),
                ("ObtenerArbolFamiliar", "FamiliaServices", [("idFamiliaRaiz", "int")]),
                ("AgregarFamiliaAPerfil", "void", [("idPerfil", "int"), ("idFamilia", "int")]),
                ("AgregarFamiliaAFamilia", "void", [("idFamiliaPadre", "int"), ("idFamiliaHija", "int"), ("nombrePadre", "string"), ("nombreHija", "string")]),
                ("AgregarPermisoAFamilia", "void", [("idFamilia", "int"), ("idPermiso", "int"), ("nombrePermiso", "string"), ("nombreFamilia", "string")]),
                ("EliminarFamilia", "void", [("idFamilia", "int"), ("nombreFamilia", "string")]),
                ("EliminarPermisoFamilia", "void", [("idFamilia", "int"), ("idPermiso", "int"), ("nombreFamilia", "string"), ("nombrePermiso", "string")]),
                ("EliminarFamiliaDeFamilia", "void", [("idFamiliaPadre", "int"), ("idFamiliaHija", "int"), ("nombrePadre", "string"), ("nombreHija", "string")]),
                ("ObtenerFamiliasPerfil", "List<Perfil>", [])
            ]
        },
        "PatenteBLL": {
            "meths": [
                ("CrearNuevoPermiso", "void", [("nombrePermiso", "string")]),
                ("EliminarPermiso", "void", [("idPermiso", "int"), ("nombrePermiso", "string")]),
                ("AgregarPermisoAPerfil", "void", [("idPerfil", "int"), ("idPermiso", "int")]),
                ("EliminarPermisoPerfil", "void", [("idPerfil", "int"), ("idPermiso", "int")]),
                ("ObtenerPermisosDePerfil", "List<PatenteServices>", [("idPerfil", "int")]),
                ("ObtenerPermisosPerfil", "List<Perfil>", []),
                ("VincularPermisoABoton", "void", [("nombreFormulario", "string"), ("nombreBoton", "string"), ("nombrePermiso", "string")]),
                ("ObtenerControlesRestringidos", "Dictionary<string, string>", [("nombreFormulario", "string")]),
                ("ObtenerComponentesTotales", "List<Perfil>", [])
            ]
        }
    }

    # DAL Classes
    dal_classes_def = {
        "UsuarioDAL": {
            "meths": [
                ("CrearUsuario", "void", [("usuario", "UsuarioBE")]),
                ("CambiarContraseña", "void", [("usuario", "UsuarioBE")]),
                ("ObtenerUsuario", "UsuarioBE", [("nombreDeUsuario", "string")]),
                ("BuscarUsuario", "UsuarioBE", [("dni", "int")]),
                ("ModificarUsuario", "void", [("usuario", "UsuarioBE")]),
                ("CambioEstado", "void", [("usuario", "UsuarioBE")]),
                ("ListaUsuarios", "List<UsuarioBE>", []),
                ("Desbloquear", "void", [("usuario", "UsuarioBE")]),
                ("CambiarIdiomaUsuario", "void", [("usuario", "UsuarioBE")])
            ]
        },
        "IdiomaDAL": {
            "meths": [
                ("ObtenerIdiomas", "List<Idioma>", []),
                ("Traducir", "string", [("clave", "string")]),
                ("Traducir", "string", [("clave", "string"), ("idioma", "Idioma")])
            ]
        },
        "BackupDAL": {
            "meths": [
                ("CrearBackup", "void", [("ruta", "string")]),
                ("RestaurarBackup", "void", [("ruta", "string")])
            ]
        },
        "PerfilDAL": {
            "meths": [
                ("InsertarPerfilNuevo", "void", [("nombrePerfil", "string")]),
                ("AgregarFamiliaAlPerfil", "void", [("idPerfil", "int"), ("idFamilia", "int")]),
                ("ExisteRelacionFamiliaPerfil", "bool", [("idPerfil", "int"), ("idFamilia", "int")]),
                ("ExisteRelacionPermisoPerfil", "bool", [("idPerfil", "int"), ("idPermiso", "int")]),
                ("ObtenerPermisosDeFamilia", "List<PatenteServices>", [("idFamilia", "int")]),
                ("InsertarPermisoAFamilia", "void", [("idFamilia", "int"), ("idPermiso", "int")]),
                ("EliminarPermisoAPerfil", "void", [("idPerfil", "int"), ("idPermiso", "int")]),
                ("EliminarPerfilDefinitivo", "void", [("idPerfil", "int")]),
                ("PerfilTieneUsuarios", "bool", [("idPerfil", "int")]),
                ("EliminarFamiliaDePerfil", "void", [("idPerfil", "int"), ("idFamilia", "int")]),
                ("ObtenerIdPerfilPorNombre", "int", [("nombreRol", "string")]),
                ("AgregarPermisoAPerfil", "void", [("idPerfil", "int"), ("idPermiso", "int")]),
                ("ObtenerComponentesTotales", "List<Perfil>", []),
                ("ObtenerPerfiles", "List<Perfil>", []),
                ("ExistePermisoEnPerfil", "bool", [("idPerfil", "int"), ("idPermiso", "int")]),
                ("ObtenerArbolPerfil", "FamiliaServices", [("idPerfil", "int")]),
                ("ExistePerfilPorNombre", "bool", [("nombrePerfil", "string")])
            ]
        },
        "FamiliaDAL": {
            "meths": [
                ("InsertarFamiliaNueva", "int", [("nombreFamilia", "string")]),
                ("ObtenerArbolFamiliar", "FamiliaServices", [("idFamiliaRaiz", "int")]),
                ("ObtenerPerfilesDeFamilia", "List<string>", [("idFamilia", "int")]),
                ("ExisteRelacionPermisoFamilia", "bool", [("idFamilia", "int"), ("idPermiso", "int")]),
                ("ExisteFamiliaPorNombre", "bool", [("nombreFamilia", "string")]),
                ("ExisteRelacionFamiliaFamilia", "bool", [("idFamiliaPadre", "int"), ("idFamiliaHija", "int")]),
                ("ExisteRelacionFamiliaPerfil", "bool", [("idPerfil", "int"), ("idFamilia", "int")]),
                ("InsertarPermisoFamilia", "void", [("idFamilia", "int"), ("idPermiso", "int")]),
                ("EliminarPermisoFamilia", "void", [("idFamilia", "int"), ("idPermiso", "int")]),
                ("EliminarFamilia", "void", [("idFamilia", "int")]),
                ("EliminarFamiliaDeFamilia", "void", [("idFamiliaPadre", "int"), ("idFamiliaHija", "int")]),
                ("InsertarFamiliaAFamilia", "void", [("idFamiliaPadre", "int"), ("idFamiliaHija", "int")]),
                ("ObtenerTodasLasFamilias", "List<FamiliaServices>", [])
            ]
        },
        "PatenteDAL": {
            "meths": [
                ("InsertarPatenteNueva", "void", [("nombrePermiso", "string")]),
                ("EliminarPermisoDefinitivo", "void", [("idPermiso", "int")]),
                ("InsertarPermisoPerfil", "void", [("idFamilia", "int"), ("idPermiso", "int")]),
                ("EliminarPermisoPerfil", "void", [("idFamilia", "int"), ("idPermiso", "int")]),
                ("InsertarFamiliaPerfil", "void", [("idFamiliaPadre", "int"), ("idFamiliaHija", "int")]),
                ("EliminarFamiliaPerfil", "void", [("idFamiliaPadre", "int"), ("idFamiliaHija", "int")]),
                ("EliminarPermisoAPerfil", "void", [("idPerfil", "int"), ("idPermiso", "int")]),
                ("ObtenerControlesRestringidos", "Dictionary<string, string>", [("nombreFormulario", "string")]),
                ("ExistePermisoABoton", "bool", [("nombreFormulario", "string"), ("nombreBoton", "string")]),
                ("ExistePermisoPorNombre", "bool", [("nombrePermiso", "string")]),
                ("ObtenerFamiliasPerfil", "List<Perfil>", []),
                ("ObtenerPermisosPerfil", "List<Perfil>", []),
                ("ObtenerPermisosDePerfil", "List<PatenteServices>", [("idPerfil", "int")]),
                ("ObtenerComponentesTotales", "List<Perfil>", []),
                ("VincularPermisoABoton", "void", [("nombreFormulario", "string"), ("nombreBoton", "string"), ("nombrePatente", "string")])
            ]
        },
        "Conexion": {
            "meths": [
                ("AbrirConexion", "bool", []),
                ("CerrarConexion", "bool", []),
                ("ExecuteNonQuery", "void", [("stringQuery", "string"), ("parametros", "SqlParameter[]")]),
                ("ExecuteReader", "DataTable", [("stringQuery", "string"), ("parametros", "SqlParameter[]")]),
                ("ExecuteNonQueryMaster", "void", [("stringQuery", "string"), ("parametros", "SqlParameter[]")])
            ]
        }
    }

    def ensure_class(cname, info, is_abstract=False):
        found = None
        for el in pkg7.Elements:
            if el.Name == cname and el.Type == "Class":
                found = el
                break
        if not found:
            el = pkg7.Elements.AddNew(cname, "Class")
            if is_abstract:
                el.Abstract = "1"
            el.Update()
            pkg7.Elements.Refresh()
            found = el
            print(f"Created class {cname} (ID: {el.ElementID})")
        else:
            print(f"Class {cname} already exists (ID: {found.ElementID})")

        # Attributes
        if "atts" in info:
            existing_atts = {a.Name: a for a in found.Attributes}
            for aname, atype in info["atts"]:
                if aname not in existing_atts:
                    att = found.Attributes.AddNew(aname, atype)
                    att.Update()
            found.Attributes.Refresh()

        # Methods
        if "meths" in info:
            existing_meths = {m.Name: m for m in found.Methods}
            for mitem in info["meths"]:
                mname, mret, mparams = mitem
                if mname not in existing_meths:
                    meth = found.Methods.AddNew(mname, mret)
                    meth.ReturnType = mret
                    meth.Update()
                    for pname, ptype in mparams:
                        param = meth.Parameters.AddNew(pname, ptype)
                        param.Type = ptype
                        param.Update()
                    meth.Parameters.Refresh()
                    meth.Update()
            found.Methods.Refresh()
        return found

    for cname, info in be_classes_def.items():
        ensure_class(cname, info, info.get("abstract", False))

    for cname, info in bll_classes_def.items():
        ensure_class(cname, info)

    for cname, info in dal_classes_def.items():
        ensure_class(cname, info)

    ea.CloseFile()
    ea.Exit()
    print("ALL MISSING ELEMENTS CREATED SUCCESSFULLY!")

if __name__ == "__main__":
    create_all_missing_elements()
