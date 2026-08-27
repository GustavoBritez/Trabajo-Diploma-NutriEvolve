using BE;

namespace BLL
{
    // Observador concreto
    public class ReporteroBLL : IObservador
    {
        private ReporteroBE reportero;

        public ReporteroBLL(ReporteroBE reportero)
        {
            this.reportero = reportero;
        }

        public void Actualizar(string noticia)
        {
            Console.WriteLine($"   📰 {reportero.Nombre} recibió la noticia: {noticia}");
        }

        public override string ToString()
        {
            return reportero.Nombre ?? "Reportero Desconocido";
        }
    }
}
