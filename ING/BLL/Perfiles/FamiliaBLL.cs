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
    public class FamiliaBLL
    {
        private PatenteDAL _patenteDAL = new();
        private FamiliaDAL _familiaDAL = new();

        public FamiliaBLL() { }

        public int CrearNuevaFamilia(string nombreFamilia)
        {
            if (string.IsNullOrWhiteSpace(nombreFamilia))
            {
                throw new ArgumentException("Familia No Puede ser Creada Vacia");
            }

            if (_familiaDAL.ExisteFamiliaPorNombre(nombreFamilia))
            {
                throw new ArgumentException($"Ya existe una familia registrada con el nombre '{nombreFamilia}'. Por favor, elija un nombre diferente.");
            }

            int idNuevaFamilia = _familiaDAL.InsertarFamiliaNueva(nombreFamilia);

            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Creacion de Familia";
            bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Permisos");

            return idNuevaFamilia;
        }

        public void AgregarFamiliaAPerfil(int idPerfil, int idFamilia)
        {
            _patenteDAL.InsertarFamiliaPerfil(idPerfil, idFamilia);
        }

        public void AgregarFamiliaAFamilia(int idFamiliaPadre, int idFamiliaHija, string nombrePadre, string nombreHija)
        {
            if (idFamiliaPadre == idFamiliaHija)
            {
                throw new ArgumentException("Error de recursividad: Una familia no puede contenerse a sí misma.");
            }

            if (_familiaDAL.ExisteRelacionFamiliaFamilia(idFamiliaPadre, idFamiliaHija))
            {
                throw new ArgumentException($"La familia '{nombreHija}' ya se encuentra dentro de la familia '{nombrePadre}'.");
            }

            if (_familiaDAL.ExisteRelacionFamiliaFamilia(idFamiliaHija, idFamiliaPadre))
            {
                throw new ArgumentException($"Bucle detectado: No puede agregar '{nombreHija}' dentro de '{nombrePadre}' porque '{nombrePadre}' ya está adentro de '{nombreHija}'.");
            }

            _familiaDAL.InsertarFamiliaAFamilia(idFamiliaPadre, idFamiliaHija);

            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Familia '{nombreHija}' asignada como hija de Familia '{nombrePadre}'";
            bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Permisos");
        }

        public void EliminarPermisoFamilia(int idFamilia, int idPermiso, string nombreFamilia, string nombrePermiso)
        {
            // 1. Validamos que exista la relación usando el método ExisteRelacionPermisoFamilia que armamos en el paso anterior
            if (!_familiaDAL.ExisteRelacionPermisoFamilia(idFamilia, idPermiso))
            {
                throw new ArgumentException($"El permiso '{nombrePermiso}' no se encuentra asignado a la familia '{nombreFamilia}'.");
            }

            // 2. Si existe, lo borramos
            _familiaDAL.EliminarPermisoFamilia(idFamilia, idPermiso);

            // 3. Bitácora
            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Desvincular Permiso '{nombrePermiso}' de Familia '{nombreFamilia}'";
            bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Permisos");
        }

        public void EliminarFamiliaDeFamilia(int idFamiliaPadre, int idFamiliaHija, string nombrePadre, string nombreHija)
        {
            // 1. Validamos defensivamente que exista el vínculo antes de intentar borrarlo
            if (!_familiaDAL.ExisteRelacionFamiliaFamilia(idFamiliaPadre, idFamiliaHija))
            {
                throw new ArgumentException($"No se puede desvincular: La familia '{nombreHija}' no se encuentra asignada dentro de '{nombrePadre}'.");
            }

            // 2. Si la relación existe, procedemos al borrado
            _familiaDAL.EliminarFamiliaDeFamilia(idFamiliaPadre, idFamiliaHija);

            // 3. Registramos la acción en la Bitácora
            EventoBLL bitacoraBLL = new EventoBLL();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Desvincular Familia '{nombreHija}' de la Familia Padre '{nombrePadre}'";
            bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Permisos");
        }

        public void EliminarFamilia(int idFamilia, string nombreFamilia)
        {
            // Ejecutamos el borrado en cascada
            _familiaDAL.EliminarFamilia(idFamilia);

            // Dejamos registro en la bitácora
            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Eliminacion de Familia";
            bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Permisos");
        }

        public void AgregarPermisoAFamilia(int idFamilia, int idPermiso, string nombrePermiso, string nombreFamilia)
        {
            if (_familiaDAL.ExisteRelacionPermisoFamilia(idFamilia, idPermiso))
            {
                throw new ArgumentException($"El permiso '{nombrePermiso}' ya se encuentra dentro de la familia '{nombreFamilia}'.");
            }

            _familiaDAL.InsertarPermisoFamilia(idFamilia, idPermiso);

            EventoBLL bitacoraBLL = new();
            int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
            string descripcion = $"Asignar Permiso '{nombrePermiso}' a Familia '{nombreFamilia}'";
            bitacoraBLL.RegistrarEvento(3, descripcion, dniActual, "Permisos");
        }

        public FamiliaServices ObtenerArbolFamiliar(int idFamiliaRaiz)
        {
            return _familiaDAL.ObtenerArbolFamiliar(idFamiliaRaiz);
        }

        public List<Perfil> ObtenerFamiliasPerfil() => _patenteDAL.ObtenerFamiliasPerfil();

        public List<FamiliaServices> ObtenerTodasLasFamilias()
        {
            try
            {
                return _familiaDAL.ObtenerTodasLasFamilias();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de familias: " + ex.Message);
            }
        }

    }

}
