using BE;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class AgendaMedicaBLL_DNI101
    {
        private readonly AgendaMedicaDAL_DNI101 _agendaDAL = new();

        public List<BloqueHorarioBE_DNI101> ListarBloquesDisponibles(DateTime fecha, int dniNutricionista)
        {
            var bloques = _agendaDAL.ListarBloquesDisponibles(fecha, dniNutricionista);
            if (bloques.Count == 0)
            {
                // Generar bloques por defecto entre las 08:00 y las 18:00 en intervalos de 30 min para conveniencia
                TimeSpan inicio = new TimeSpan(8, 0, 0);
                TimeSpan fin = new TimeSpan(18, 0, 0);
                TimeSpan intervalo = TimeSpan.FromMinutes(30);

                int idMock = 1;
                while (inicio < fin)
                {
                    TimeSpan bloqueFin = inicio.Add(intervalo);
                    bloques.Add(new BloqueHorarioBE_DNI101
                    {
                        IdBloque_DNI101 = idMock++,
                        HoraInicio_DNI101 = inicio,
                        HoraFin_DNI101 = bloqueFin,
                        EstadoBloque_DNI101 = "Disponible"
                    });
                    inicio = bloqueFin;
                }
            }
            return bloques;
        }

        public bool ActualizarEstadoBloque(int idBloque, string nuevoEstado)
        {
            return _agendaDAL.ActualizarEstadoBloque(idBloque, nuevoEstado);
        }
    }
}
