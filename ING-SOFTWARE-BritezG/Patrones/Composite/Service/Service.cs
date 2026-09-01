using BE;
using BLL;

namespace Service
{
    public class Service
    {

        private IComponent _rolActivo;

        public void IniciarRol(Rol rol)
        {
            _rolActivo = new Composite(rol);
        }

        public void AsignarUsuarioRolActivo(Usuario user )
        {
            IComponent hojaUsuario = new Leaf(user);
            _rolActivo.AgregarComponente(hojaUsuario);
        }

        public void AgregarPermisoARolActivo(Permiso permiso)
        {
            _rolActivo.AgregarPermiso(permiso);
        }

        public List<Permiso> ObtenerPermisosDelRolActivo()
        {
            return _rolActivo.ObtenerPermisos() ?? new List<Permiso>();
        }

        public void EjecutarRolActivo()
        {
            _rolActivo.Execute();
        }   

        public void AgregarPermiso(IComponent componente, Permiso permiso)

        {
            componente.AgregarPermiso(permiso);
        }

        public void EliminarPermiso(IComponent componente, Permiso permiso)
        {
            componente.EliminarPermiso(permiso);
        }

        public void AgregarComponente(IComponent componente, IComponent nuevoComponente)
        {
            componente.AgregarComponente(nuevoComponente);
        }

        public void EliminarComponente(IComponent componente, IComponent componenteAEliminar)
        {
            componente.EliminarComponente(componenteAEliminar);
        }

         public List<Permiso> ObtenerPermisos(IComponent componente)
        {
            return componente.ObtenerPermisos();
        }

         public void Ejecutar(IComponent componente)
        {
            componente.Execute();
        }
    }
}
