using System;
using System.Collections.Generic;

namespace BE
{
    public class AgendaMedicaBE_DNI101
    {
        public int IdAgenda_DNI101 { get; set; }
        public DateTime Fecha_DNI101 { get; set; } = DateTime.Today;
        public string EstadoAgenda_DNI101 { get; set; } = "Abierta"; // Abierta, Cerrada
        public int DniNutricionista_DNI101 { get; set; }
        public List<BloqueHorarioBE_DNI101> BloquesHorarios_DNI101 { get; set; } = new List<BloqueHorarioBE_DNI101>();
        public string? DV { get; set; }
    }
}
