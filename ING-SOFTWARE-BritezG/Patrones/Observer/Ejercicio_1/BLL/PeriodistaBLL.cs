using BE;

namespace BLL
{
    // Interfaz Observer
    public interface IObservador
    {
        void Actualizar(string noticia);
    }

    // Subject (Observable) - Patrón Observer
    public class PeriodistaBLL
    {
        private PeriodistaBE periodista;
        private List<IObservador> observadores = new List<IObservador>();

        public PeriodistaBLL(PeriodistaBE periodista)
        {
            this.periodista = periodista;
        }

        // Agregar un observador
        public void Agregar(IObservador observador)
        {
            observadores.Add(observador);
            Console.WriteLine($"{observador} se suscribió a las noticias de {periodista.Nombre}");
        }

        // Remover un observador
        public void Remover(IObservador observador)
        {
            observadores.Remove(observador);
            Console.WriteLine($"{observador} se desuscribió de {periodista.Nombre}");
        }

        // Notificar a todos los observadores
        public void Notificar(string noticia)
        {
            periodista.Noticia = noticia;
            Console.WriteLine($"\n{periodista.Nombre} encontró una noticia: {noticia}\n");

            foreach (var observador in observadores)
            {
                observador.Actualizar(noticia);
            }
        }

        public string ObtenerNombre() => periodista.Nombre ?? "Periodista Desconocido";
    }
}
