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
        public bool Act { get; set; } = true;

        public string Cliente => $"{Apellido}, {Nombre}".Trim(' ', ',');
        public string FechaStr => FechaCambio.ToString("dd/MM/yyyy");
        public string HoraStr => FechaCambio.ToString("HH:mm:ss");
        public string EstadoStr => Activo ? "Activo" : "Bloqueado";
        public string OperacionStr => NormalizarOperacion(TipoOperacion);

        public static string NormalizarOperacion(string op)
        {
            if (string.IsNullOrWhiteSpace(op)) return "MODIFICACIÓN";
            string u = op.Trim().ToUpperInvariant();
            if (u.Contains("ALTA") || u.Contains("INSERT")) return "ALTA";
            if (u.Contains("BLOQ") || u.Contains("DELETE") || u.Contains("BAJA") || u.Contains("ELIMIN")) return "BLOQUEAR";
            if (u.Contains("DESBLOQ") || u.Contains("REACTIV")) return "DESBLOQUEAR";
            return "MODIFICACIÓN";
        }
    }
}
