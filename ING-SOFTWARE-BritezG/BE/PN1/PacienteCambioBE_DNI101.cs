using System;

namespace BE.PN1
{
    public class PacienteCambioBE_DNI101
    {
        public int IdCambio { get; set; }
        public int IdPaciente { get; set; }
        public string Dni { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCambio { get; set; }
        public int UsuarioDni { get; set; }
        public string TipoOperacion { get; set; } = string.Empty;
    }
}
