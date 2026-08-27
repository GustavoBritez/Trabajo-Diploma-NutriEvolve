namespace BE
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public List<Permiso> Permisos;

        public Usuario()
        {
            
        }

        public Usuario(int id, string nombre) : this()
        {
            this.Id = id;
            this.Nombre = nombre;

            Permisos = new();
        }
    }
}
