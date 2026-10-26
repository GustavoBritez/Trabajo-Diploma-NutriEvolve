using System;

namespace BE
{
    public class PacienteBE_DNI101
    {
        public int IdPaciente_DNI101 { get; set; }
        public string DNINiño_DNI101 { get; set; } = string.Empty;
        public string Nombre_DNI101 { get; set; } = string.Empty;
        public string Apellido_DNI101 { get; set; } = string.Empty;
        public string? Telefono_DNI101 { get; set; }
        public string? Email_DNI101 { get; set; }
        public DateTime FechaNacimiento_DNI101 { get; set; } = DateTime.Today;
        public string? Sexo_DNI101 { get; set; }
        public string? ObraSocial_DNI101 { get; set; }
        public string? Direccion_DNI101 { get; set; }
        public bool Activo_DNI101 { get; set; } = true;
        public string? DV { get; set; }

        public System.Collections.Generic.List<PacienteEmailBE_DNI101> Emails { get; set; } = new();
        public System.Collections.Generic.List<PacienteTelefonoBE_DNI101> Telefonos { get; set; } = new();

        public string NombreCompleto => $"{Apellido_DNI101}, {Nombre_DNI101}".Trim();

        public int ObtenerEdadMeses()
        {
            var hoy = DateTime.Today;
            int meses = (hoy.Year - FechaNacimiento_DNI101.Year) * 12 + (hoy.Month - FechaNacimiento_DNI101.Month);
            if (hoy.Day < FechaNacimiento_DNI101.Day)
            {
                meses--;
            }
            return Math.Max(0, meses);
        }
    }
}
