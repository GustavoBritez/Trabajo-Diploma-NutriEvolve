namespace BE
{
    public class EventoBE
    {
        private int Criticidad;
        private string Descripcion;
        private int Dni;
        private DateTime Fecha;
        private int Id_Evento;
        private string Modulo;

        public EventoBE()
        {

        }
        public EventoBE(int criticidad, string descripcion, int dni, DateTime fecha, string modulo)
        {
            Criticidad = criticidad;
            Descripcion = descripcion;
            Dni = dni;
            Fecha = fecha;
            Id_Evento = 0; // no lo toquen dejenlo asi se arregla en la BD
            Modulo = modulo;
        }


        public EventoBE(int criticidad, string descripcion, int dni, DateTime fecha, string modulo, int id_evento)
        {
            Criticidad = criticidad;
            Descripcion = descripcion;
            Dni = dni;
            Fecha = fecha;
            Id_Evento = id_evento;
            Modulo = modulo;
        }

        public int _Criticidad { get => Criticidad; set => Criticidad = value; }
        public string _Descripcion { get => Descripcion; set => Descripcion = value; }
        public int _Dni { get => Dni; set => Dni = value; }
        public DateTime _Fecha { get => Fecha; set => Fecha = value; }
        public int _Id_Evento { get => Id_Evento; set => Id_Evento = value; }
        public string _Modulo { get => Modulo; set => Modulo = value; }
    }
}   