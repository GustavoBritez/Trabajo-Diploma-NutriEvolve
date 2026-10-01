using BE;
using DAL;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class AgendaMedicaBLL_DNI101
    {
        private readonly AgendaMedicaDAL_DNI101 _agendaDAL = new();
        private readonly TurnoDAL_DNI101 _turnoDAL = new();

        public List<BloqueHorarioBE_DNI101> ListarBloquesDisponibles(DateTime fecha, int dniNutricionista)
        {
            var bloquesBD = _agendaDAL.ListarBloquesDisponibles(fecha, dniNutricionista);
            var horariosOcupados = _turnoDAL.ObtenerHorariosOcupadosPorProfesional(dniNutricionista, fecha);

            var bloquesResultado = new List<BloqueHorarioBE_DNI101>();

            if (bloquesBD != null && bloquesBD.Count > 0)
            {
                foreach (var b in bloquesBD)
                {
                    if (!horariosOcupados.Contains(b.HoraInicio_DNI101))
                    {
                        bloquesResultado.Add(b);
                    }
                }
            }
            else
            {
                // Generar bloques por defecto entre las 08:00 y las 18:00 en intervalos de 30 min excluyendo horarios ocupados
                TimeSpan inicio = new TimeSpan(8, 0, 0);
                TimeSpan fin = new TimeSpan(18, 0, 0);
                TimeSpan intervalo = TimeSpan.FromMinutes(30);

                int idMock = 1;
                while (inicio < fin)
                {
                    TimeSpan bloqueFin = inicio.Add(intervalo);
                    if (!horariosOcupados.Contains(inicio))
                    {
                        bloquesResultado.Add(new BloqueHorarioBE_DNI101
                        {
                            IdBloque_DNI101 = idMock++,
                            HoraInicio_DNI101 = inicio,
                            HoraFin_DNI101 = bloqueFin,
                            EstadoBloque_DNI101 = "Disponible"
                        });
                    }
                    inicio = bloqueFin;
                }
            }

            return bloquesResultado;
        }

        public bool ActualizarEstadoBloque(int idBloque, string nuevoEstado)
        {
            return _agendaDAL.ActualizarEstadoBloque(idBloque, nuevoEstado);
        }
    }
}
