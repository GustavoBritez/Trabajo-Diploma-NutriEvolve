using BE;
using BLL;
namespace Service
{
    public class ServiceBusqueda
    {
        public void Buscar_ElonMusk()
        {
            PeriodistaBE periodista = new PeriodistaBE() { Nombre = "Juan" };
            ReporteroBE reportero = new ReporteroBE() { Nombre = "Pedro" };
       
            PeriodistaBLL periodistaBLL = new PeriodistaBLL(periodista);
            ReporteroBLL reporteroBLL = new ReporteroBLL(reportero);
            // eppe
            Console.WriteLine("Comenzamos la busqueda de elon musk...");

            periodistaBLL.Agregar(reporteroBLL);

            periodistaBLL.Notificar( " ELON MUSK ");

            Console.WriteLine(" Fin ");
        }
    }
}
