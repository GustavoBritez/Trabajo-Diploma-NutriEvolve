namespace BLL.PN2.Strategy
{
    public class ResultadoEvaluacionOMS
    {
        public string Clasificacion { get; set; } = string.Empty;
        public string Detalles { get; set; } = string.Empty;
        public decimal PercentilAproximado { get; set; }
        public bool RequiereAlerta { get; set; }
        public string? SeveridadAlerta { get; set; }
        public string? MensajeAlerta { get; set; }
    }
}
