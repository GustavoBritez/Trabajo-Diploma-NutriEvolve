namespace BE
{
    public class BitacoraBE
    {
        private int Criticidad;
        private string Descripcion;
        private int Dni;
        private DateTime Fecha;
        private int Id_Bitacora;
        private string Modulo;

        public BitacoraBE()
        {

        }
        public BitacoraBE(int criticidad, string descripcion, int dni, DateTime fecha, string modulo)
        {
            Criticidad = criticidad;
            Descripcion = descripcion;
            Dni = dni;
            Fecha = fecha;
            Id_Bitacora = 0; // no lo toquen dejenlo asi se arregla en la BD
            Modulo = modulo;
        }

        public BitacoraBE(int criticidad, string descripcion, int dni, DateTime fecha, string modulo, int id_bitacora)
        {
            Criticidad = criticidad;
            Descripcion = descripcion;
            Dni = dni;
            Fecha = fecha;
            Id_Bitacora = id_bitacora;
            Modulo = modulo;
        }

        public int _Criticidad { get => Criticidad; set => Criticidad = value; }
        public string _Descripcion { get => Descripcion; set => Descripcion = value; }
        public int _Dni { get => Dni; set => Dni = value; }
        public DateTime _Fecha { get => Fecha; set => Fecha = value; }
        public int _Id_Bitacora { get => Id_Bitacora; set => Id_Bitacora = value; }
        public int _Id_Evento { get => Id_Bitacora; set => Id_Bitacora = value; }
        public string _Modulo { get => Modulo; set => Modulo = value; }
    }

    [Obsolete("Usar BitacoraBE")]
    public class EventoBE : BitacoraBE
    {
        public EventoBE() : base() { }
        public EventoBE(int criticidad, string descripcion, int dni, DateTime fecha, string modulo) : base(criticidad, descripcion, dni, fecha, modulo) { }
        public EventoBE(int criticidad, string descripcion, int dni, DateTime fecha, string modulo, int id_evento) : base(criticidad, descripcion, dni, fecha, modulo, id_evento) { }
    }
}
