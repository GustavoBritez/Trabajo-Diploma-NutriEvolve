using System;

namespace BE
{
    public class PacienteEmailBE_DNI101
    {
        public int IdEmail { get; set; }
        public int IdPaciente { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool EsPrincipal { get; set; }
        public bool Activo { get; set; } = true;
    }

    public class PacienteTelefonoBE_DNI101
    {
        public int IdTelefono { get; set; }
        public int IdPaciente { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string Tipo { get; set; } = "Celular"; // Celular, Fijo, Laboral
        public bool EsPrincipal { get; set; }
        public bool Activo { get; set; } = true;
    }
}
