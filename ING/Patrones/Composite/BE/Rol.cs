namespace BE
{
    public class Rol : Usuario
    {
        public List<Permiso> Permisos { get; set; }

        public Rol()
        {
            Permisos = new List<Permiso>();
        }

        public Rol(int id, string nombre) : this()
        {
            Permisos = new List<Permiso>();
        }
    }
}
