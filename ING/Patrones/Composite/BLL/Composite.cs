using BE;


namespace BLL
{
    public class Composite : IComponent
    {
        private Rol rol;
        private List<Permiso> permisos;
        private List<IComponent> children;

        public int Id => rol.Id;
        public string Nombre => rol.Nombre;

        public Composite(Rol rol)
        {
            this.rol = rol;
            this.permisos = new List<Permiso>(rol.Permisos);
            this.children = new List<IComponent>();
        }

        public void AgregarPermiso(Permiso permiso)
        {
            if (permisos.Contains(permiso))
            {
                throw new InvalidOperationException("El permiso ya existe en el rol");
            }

            permisos.Add(permiso);
            rol.Permisos.Add(permiso);
        }

        public void EliminarPermiso(Permiso permiso)
        {
            permisos.Remove(permiso);
            rol.Permisos.Remove(permiso);
        }

        public void AgregarComponente(IComponent componente)
        {

            children.Add(componente);
            var permisosComponentes = componente.ObtenerPermisos();

            foreach ( var permiso in permisosComponentes)
            {
                try
                {
                    AgregarPermiso(permiso);
                }
                catch (InvalidOperationException)
                {
                    // Si el permiso ya existe, simplemente lo ignoramos
                }
            }
                
        }

        public void EliminarComponente(IComponent componente)
        {
            children.Remove(componente);
        }

        public List<IComponent> ObtenerChildren()
        {
            return new List<IComponent>(children);
        }

        public List<Permiso> ObtenerPermisos()
        {
            List<Permiso> permisosTotales = new(permisos);

            foreach (var child in children)
            {
                var permisosChild = child.ObtenerPermisos();
                foreach (var permiso in permisosChild)
                {
                    if (!permisos.Contains(permiso))
                    {
                        permisos.Add(permiso);
                    }
                }
            }
            return permisosTotales;
        }

        public void Execute()
        {
            Console.WriteLine($"👤 Rol: {rol.Nombre}");
            Console.WriteLine($"   Permisos: [{string.Join(", ", permisos.Select(p => p.Id))}]");
            foreach (var child in children)
            {
                child.Execute();
            }
        }
    }        
}
