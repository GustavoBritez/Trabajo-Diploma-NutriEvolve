using System;
using System.Collections.Generic;

namespace BE
{
    public class TutorBE_DNI101
    {
        public int IdTutor_DNI101 { get; set; }
        public string DniTutor_DNI101 { get; set; } = string.Empty;
        public string Nombre_DNI101 { get; set; } = string.Empty;
        public string Apellido_DNI101 { get; set; } = string.Empty;
        public string? Telefono_DNI101 { get; set; }
        public string? Email_DNI101 { get; set; }
        public string? Parentesco_DNI101 { get; set; }
        public DateTime FechaRegistro_DNI101 { get; set; } = DateTime.Now;
        public List<PacienteBE_DNI101> Pacientes_DNI101 { get; set; } = new List<PacienteBE_DNI101>();
        public string? DV { get; set; }

        public string NombreCompleto => $"{Apellido_DNI101}, {Nombre_DNI101}".Trim();
    }
}
