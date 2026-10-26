using System;

namespace BE.PN2
{
    public class ReporteInteligenteDTO
    {
        public int IdPaciente { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public int EdadMesesActual { get; set; }
        public string Sexo { get; set; } = string.Empty;
        public int TotalConsultas { get; set; }

        // Últimas mediciones
        public decimal UltimoPesoKg { get; set; }
        public decimal UltimaTallaCm { get; set; }
        public decimal UltimoIMC { get; set; }
        public decimal UltimoZScoreIMC { get; set; }
        public decimal UltimoPercentilIMC { get; set; }
        public string DiagnosticoOMSActual { get; set; } = string.Empty;

        // Métricas de Tendencia y Velocidad
        public decimal VelocidadPesoKgMes { get; set; } // Delta Peso / Delta Meses
        public decimal VelocidadTallaCmMes { get; set; } // Delta Talla / Delta Meses
        public decimal DeltaZScoreIMC { get; set; } // Deriva de Z-Score

        // Proyección Predictiva (60-90 días)
        public decimal PesoProyectado90Dias { get; set; }
        public decimal TallaProyectada90Dias { get; set; }
        public string ClasificacionRiesgoFuturo { get; set; } = string.Empty; // Normal, Alerta, Crítico
        public string NivelRiesgo { get; set; } = "Bajo"; // Bajo, Moderado, Alto, Crítico

        // Recomendaciones Accionables del Motor de Decisión Clínica
        public string RecomendacionFrecuenciaControl { get; set; } = string.Empty;
        public string RecomendacionDietoterapica { get; set; } = string.Empty;
        public string SuplementacionSugerida { get; set; } = string.Empty;
        public string PautaAlimentariaEspecial { get; set; } = string.Empty;
    }
}
