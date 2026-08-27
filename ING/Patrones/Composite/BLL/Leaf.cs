
using BE;

namespace BLL
{
    public class Leaf : IComponent
    {
        private Usuario usuario;
        private List<Permiso> permisos;

        public int Id => usuario.Id;
        public string Nombre => usuario.Nombre;

        public Leaf(Usuario usuario)
        {
            this.usuario = usuario;
            this.permisos = new List<Permiso>(usuario.Permisos);
        }

        public void AgregarPermiso(Permiso permiso)
        {
            if (!permisos.Contains(permiso))
            {
                permisos.Add(permiso);
            }

            permisos.Add(permiso);
            usuario.Permisos.Add(permiso);
        }

        public void EliminarPermiso(Permiso permiso)
        {
            permisos.Remove(permiso);
            usuario.Permisos.Remove(permiso);
        }

        public void AgregarComponente(IComponent permiso)
        {
            throw new NotSupportedException("Una Hoja no puede contener componentes");
        }

        public void EliminarComponente(IComponent permiso)
        {
            throw new NotSupportedException("Una Hoja no puede contener componentes");
        }

        public List<Permiso> ObtenerPermisos()
        {
            return new List<Permiso>(permisos);
        }

        public void Execute()
        {
            Console.WriteLine($"👤 Usuario: {usuario.Nombre} ");
            Console.WriteLine($"   Permisos: [{string.Join(", ", permisos.Select(p => p.Id))}]");
        }
    }  
}
