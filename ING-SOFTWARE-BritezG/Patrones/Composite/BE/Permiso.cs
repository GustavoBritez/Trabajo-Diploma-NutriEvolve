namespace BE
{
    public class Permiso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public Permiso(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        public override bool Equals(object? obj)
        {
            return obj is Permiso permiso &&
                   Id == permiso.Id &&
                   Nombre == permiso.Nombre;
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
