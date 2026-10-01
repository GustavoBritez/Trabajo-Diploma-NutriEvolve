using System;

namespace BE
{
    public class BloqueHorarioBE_DNI101
    {
        public int IdBloque_DNI101 { get; set; }
        public int IdAgenda_DNI101 { get; set; }
        public TimeSpan HoraInicio_DNI101 { get; set; }
        public TimeSpan HoraFin_DNI101 { get; set; }
        public string EstadoBloque_DNI101 { get; set; } = "Disponible"; // Disponible, Ocupado, Bloqueado
        public string? DV { get; set; }

        public string DescripcionHorario => $"{HoraInicio_DNI101:hh\\:mm} - {HoraFin_DNI101:hh\\:mm}";

        public override string ToString()
        {
            return $"{DescripcionHorario} ({EstadoBloque_DNI101})";
        }
    }
}
