using DAL;
using DAL.Perfiles;
using Services;
using Services.Perfiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class PerfilBLL
    {
        private readonly PerfilDAL _perfilDAL = new();
        private readonly PatenteDAL _patenteDAL = new();

        #region Agregar
        public void AgregarFamiliaAlPerfil(int idPerfil, int idFamilia, string nombrePerfil, string nombreFamilia)
        {
            // CORRECCIÓN 1: El método en DAL se llama ExisteRelacionFamiliaPerfil
            if (_perfilDAL.ExisteRelacionFamiliaPerfil(idPerfil, idFamilia))
            {
                throw new ArgumentException($"La familia '{nombreFamilia}' ya se encuentra asignada directamente al perfil '{nombrePerfil}'.");
            }

            // (Asumo que este método sí lo tenés en tu clase PatenteDAL)
            List<PatenteServices> permisosActualesPerfil = _patenteDAL.ObtenerPermisosDePerfil(idPerfil);

            // CORRECCIÓN 2: El método lo creaste adentro de PerfilDAL, no en PatenteDAL
            List<PatenteServices> permisosNuevaFamilia = _perfilDAL.ObtenerPermisosDeFamilia(idFamilia);

            List<PatenteServices> permisosDuplicados = permisosActualesPerfil
                .Where(pActual => permisosNuevaFamilia.Any(pNueva => pNueva.Id == pActual.Id))
                .ToList();

            if (permisosDuplicados.Any())
            {
                string detallesPermisos = string.Join(", ", permisosDuplicados.Select(p => $"'{p.Nombre}'"));
                throw new ArgumentException($"No se puede agregar la familia '{nombreFamilia}' al perfil '{nombrePerfil}' porque generaría permisos duplicados: {detallesPermisos}.");
            }

            // CORRECCIÓN 3: Llamamos al método de guardar (te dejo el código de este método más abajo)
            _perfilDAL.AgregarFamiliaAlPerfil(idPerfil, idFamilia);

            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Asignación exitosa de Familia: '{nombreFamilia}' al Perfil '{nombrePerfil}'";
            bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Perfiles");
        }

        public void AgregarPermisoAFamilia(int idPerfil, int idPermiso, string nombrePermiso)
        {
            if (_perfilDAL.ExisteRelacionPermisoPerfil(idPerfil, idPermiso))
            {
                throw new ArgumentException($"El permiso '{nombrePermiso}' ya existe en esta familia.");
            }

            _perfilDAL.InsertarPermisoAFamilia(idPerfil, idPermiso);

            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Asignar Permiso a Perfil";
            bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Permisos");
        }
        #endregion 

        #region Eliminar
        public void EliminarPermisoAPerfil(int idPerfil, int idPermiso, string nombrePermiso, string nombrePerfil)
        {
            // 1. Validación: ¿El permiso está realmente asignado?
            if (!_perfilDAL.ExistePermisoEnPerfil(idPerfil, idPermiso))
            {
                throw new ArgumentException($"El permiso '{nombrePermiso}' no está asignado al perfil '{nombrePerfil}', por lo tanto no se puede eliminar.");
            }

            // 2. Ejecución
            _perfilDAL.EliminarPermisoAPerfil(idPerfil, idPermiso);

            // 3. Bitácora
            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Eliminación de Permiso: '{nombrePermiso}' del Perfil '{nombrePerfil}'";
            bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Perfiles");
        }
        public void EliminarPerfil(int idPerfil, string nombrePerfil)
        {
            // Frenamos si hay gente usándolo y lanzamos tu mensaje personalizado
            if (_perfilDAL.PerfilTieneUsuarios(idPerfil))
            {
                throw new ArgumentException("No pudimos eliminar el perfil por que los usuarios no pueden quedar sin rol");
            }

            _perfilDAL.EliminarPerfilDefinitivo(idPerfil);

            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Eliminacion Perfil";
            bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Permisos");
        }

        public void EliminarFamiliaDePerfil(int idPerfil, int idFamilia, string nombrePerfil, string nombreFamilia)
        {
            if (!_perfilDAL.ExisteRelacionFamiliaPerfil(idPerfil, idFamilia))
            {
                throw new ArgumentException($"La familia '{nombreFamilia}' no está asignada directamente al perfil '{nombrePerfil}'. \n\nEs probable que la esté heredando a través de otra familia contenedora (como se ve en el árbol). Para quitarla, debe desvincular la familia principal.");
            }

            _perfilDAL.EliminarFamiliaDePerfil(idPerfil, idFamilia);

            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Desvincular Familia '{nombreFamilia}' del Perfil '{nombrePerfil}'";
            bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Permisos");
        }
        #endregion

        #region Solo lo usamos para cargar las 3 grillas
        public List<Perfil> ObtenerComponentesTotales()
        {
            return _perfilDAL.ObtenerComponentesTotales();
        }

        public List<Perfil> ObtenerFamiliasPerfil()
        {
            return _perfilDAL.ObtenerComponentesTotales()
                             .Where(c => c.EsCompuesto())
                             .ToList();
        }

        public List<Perfil> ObtenerPermisosPerfil()
        {
            return _perfilDAL.ObtenerComponentesTotales()
                             .Where(c => !c.EsCompuesto())
                             .ToList();
        }
        #endregion

        public void CrearNuevoPerfil(string nombrePerfil)
        {
            if (string.IsNullOrWhiteSpace(nombrePerfil))
            {
                throw new ArgumentException("El nombre del perfil no puede estar vacío.");
            }

            if (_perfilDAL.ExistePerfilPorNombre(nombrePerfil))
            {
                throw new ArgumentException($"Ya existe un perfil registrado con el nombre '{nombrePerfil}'. Por favor, elija un nombre diferente.");
            }

            _perfilDAL.InsertarPerfilNuevo(nombrePerfil);

            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Creación de Perfil";
            bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Permisos");
        }

        public int ObtenerIdPerfilPorNombre(string nombreRol)
        {
            if (string.IsNullOrWhiteSpace(nombreRol))
            {
                throw new ArgumentException("El nombre del rol no puede estar vacío.");
            }
            return _perfilDAL.ObtenerIdPerfilPorNombre(nombreRol);
        }

        public List<Perfil> ObtenerPerfiles()
        {
            return _perfilDAL.ObtenerPerfiles();
        }

        public FamiliaServices ObtenerArbolPerfil(int idPerfil)
        {
            return _perfilDAL.ObtenerArbolPerfil(idPerfil);
        }
        public void AgregarPermisoAPerfil(int idPerfil, int idPermiso, string nombrePermiso, string nombrePerfil)
        {
            // 1. Obtenemos TODOS los permisos del perfil (Directos + Heredados por Familias)
            // Asegurate de tener instanciada _patenteDAL en tu BLL
            List<PatenteServices> permisosTotalesDelPerfil = _patenteDAL.ObtenerPermisosDePerfil(idPerfil);

            // 2. Buscamos si el ID del permiso que intentan agregar ya existe en esa lista completa
            bool yaTieneElPermiso = permisosTotalesDelPerfil.Any(p => p.Id == idPermiso);

            if (yaTieneElPermiso)
            {
                // 3. Frenamos todo si ya lo tiene, avisando al usuario en la UI
                throw new ArgumentException($"El permiso '{nombrePermiso}' ya está asignado al perfil '{nombrePerfil}' (de forma directa o heredado a través de una Familia).");
            }

            // 4. Si pasó la validación, insertamos en la tabla Perfil_Permiso
            // (Asegurate de que este método en tu DAL haga el INSERT INTO Perfil_Permiso)
            _perfilDAL.AgregarPermisoAPerfil(idPerfil, idPermiso);

            // 5. Dejamos el registro en la Bitácora
            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Asignación de Permiso: '{nombrePermiso}' al Perfil '{nombrePerfil}'";
            bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Perfiles");
        }
    }
}