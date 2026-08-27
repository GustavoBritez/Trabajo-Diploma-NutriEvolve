using DAL.Perfiles;
using Services;
using Services.Perfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Perfiles
{
    public class PatenteBLL
    {
        private PatenteDAL _patenteDAL = new();

        public void AgregarPermisoAPerfil(int idPerfil, int idPermiso)
        {
            _patenteDAL.InsertarPermisoPerfil(idPerfil, idPermiso);
        }
        public List<PatenteServices> ObtenerPermisosDePerfil(int idPerfil)
        {
            // Aquí podés agregar validaciones si lo necesitás, y delegás la llamada
            return _patenteDAL.ObtenerPermisosDePerfil(idPerfil);
        }
        public void VincularPermisoABoton(string nombreFormulario, string nombreBoton, string nombrePermiso)
        {
            if (string.IsNullOrWhiteSpace(nombreFormulario) || string.IsNullOrWhiteSpace(nombreBoton) || string.IsNullOrWhiteSpace(nombrePermiso))
            {
                throw new ArgumentException("Ninguno de los campos para la vinculación puede estar vacío.");
            }

            if (_patenteDAL.ExistePermisoABoton(nombreFormulario, nombreBoton))
            {
                throw new ArgumentException($"El control '{nombreBoton}' ya tiene un permiso asignado en el formulario '{nombreFormulario}'.");
            }

            // 1. Guardamos la vinculación en la base de datos
            _patenteDAL.VincularPermisoABoton(nombreFormulario, nombreBoton, nombrePermiso);

            // 2. Registramos en la bitácora siguiendo tu excelente patrón de diseño
            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Se vinculó el Permiso '{nombrePermiso}' al control '{nombreBoton}' en la pantalla '{nombreFormulario}'";
            bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Permisos");
        }
        public void EliminarPermisoPerfil(int idPerfil, int idPermiso)
        {
            _patenteDAL.EliminarPermisoPerfil(idPerfil, idPermiso);
        }

        public void CrearNuevoPermiso(string nombrePermiso)
        {
            if (string.IsNullOrWhiteSpace(nombrePermiso))
            {
                throw new ArgumentException("El nombre del permiso no puede estar vacío.");
            }

            if (_patenteDAL.ExistePermisoPorNombre(nombrePermiso))
            {
                throw new ArgumentException($"Ya existe un permiso registrado con el nombre '{nombrePermiso}'. Por favor, elija un nombre diferente.");
            }

            _patenteDAL.InsertarPatenteNueva(nombrePermiso);

            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Creacion de Patente";
            bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Permisos");
        }

        public List<Perfil> ObtenerComponentesTotales() => _patenteDAL.ObtenerComponentesTotales();

        public List<Perfil> ObtenerPermisosPerfil() => _patenteDAL.ObtenerPermisosPerfil();

        public Dictionary<string, string> ObtenerControlesRestringidos(string nombreFormulario)
        {
            return _patenteDAL.ObtenerControlesRestringidos(nombreFormulario);
        }

        public void EliminarPermiso(int idPermiso, string nombrePermiso)
        {
            _patenteDAL.EliminarPermisoDefinitivo(idPermiso);

            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Eliminacion de Patente";
            bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Permisos");
        }


    }
}
