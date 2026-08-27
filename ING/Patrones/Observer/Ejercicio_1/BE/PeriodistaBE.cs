namespace BE
{
    public class PeriodistaBE
    {
        public string? Nombre { get; set; }
        public string? Noticia { get; set; }

        public PeriodistaBE()
        {
        }

        public PeriodistaBE(string nombre)
        {
            Nombre = nombre;
        }
    }
}
