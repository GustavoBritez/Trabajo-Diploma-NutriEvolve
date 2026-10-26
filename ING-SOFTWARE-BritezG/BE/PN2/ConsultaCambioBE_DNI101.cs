using System;

namespace BE.PN2
{
    public class ConsultaCambioBE_DNI101
    {
        public int IdCambio { get; set; }
        public int IdConsulta { get; set; }
        public int IdPaciente { get; set; }
        public int DniNutricionista { get; set; }
        public DateTime FechaControl { get; set; }
        public int EdadMeses { get; set; }
        public string? TipoLactancia { get; set; }
        public string? AlimentacionComplementaria { get; set; }
        public string? Alergias { get; set; }
        public string? AntecedentesFamiliares { get; set; }
        public string? Observaciones { get; set; }
        public DateTime FechaCambio { get; set; }
        public int UsuarioDni { get; set; }
        public string TipoOperacion { get; set; } = string.Empty; // INSERT, UPDATE, DELETE, ROLLBACK
        public bool Act { get; set; } = true;
        public string NombrePaciente { get; set; } = string.Empty;
        public string DniPaciente { get; set; } = string.Empty;
        public decimal? PesoKg { get; set; }
        public decimal? TallaCm { get; set; }
        public decimal? PerimetroCefalico { get; set; }
        public decimal? IMC { get; set; }
        public string? ClasificacionOMS { get; set; }
        public string FechaStr => FechaCambio.ToString("dd/MM/yy");
        public string HoraStr => FechaCambio.ToString("HH:mm");
        public string PesoStr => PesoKg.HasValue ? PesoKg.Value.ToString("N2") : "-";
        public string TallaStr => TallaCm.HasValue ? TallaCm.Value.ToString("N2") : "-";
        public string IMCStr => IMC.HasValue ? IMC.Value.ToString("N2") : "-";
        public int ActivoInt => Act ? 1 : 0;
    }
}
