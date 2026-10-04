using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class AgendaMedicaBLL_DNI101
    {
        private readonly AgendaMedicaDAL_DNI101 _agendaDAL = new();
        private readonly TurnoDAL_DNI101 _turnoDAL = new();

        public List<BloqueHorarioBE_DNI101> ListarBloquesDisponibles(DateTime fecha, int dniNutricionista)
        {
            if (fecha.Date < DateTime.Today)
            {
                return new List<BloqueHorarioBE_DNI101>();
            }

            DataTable dtBloques = _agendaDAL.ListarBloquesDisponibles(fecha, dniNutricionista);
            var horariosOcupados = _turnoDAL.ObtenerHorariosOcupadosPorProfesional(dniNutricionista, fecha);

            var bloquesResultado = new List<BloqueHorarioBE_DNI101>();

            if (dtBloques != null && dtBloques.Rows.Count > 0)
            {
                foreach (DataRow row in dtBloques.Rows)
                {
                    var b = MapearBloque(row);
                    if (fecha.Date.Add(b.HoraInicio_DNI101) <= DateTime.Now)
                    {
                        continue;
                    }

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
                    if (fecha.Date.Add(inicio) > DateTime.Now)
                    {
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
                    }
                    inicio = bloqueFin;
                }
            }

            return bloquesResultado;
        }

        public AgendaMedicaBE_DNI101 ObtenerAgendaMedica(DateTime fecha, int dniNutricionista)
        {
            var bloques = ListarBloquesDisponibles(fecha, dniNutricionista);
            return new AgendaMedicaBE_DNI101
            {
                Fecha_DNI101 = fecha.Date,
                DniNutricionista_DNI101 = dniNutricionista,
                EstadoAgenda_DNI101 = "Abierta",
                BloquesHorarios_DNI101 = bloques
            };
        }

        public bool ActualizarEstadoBloque(int idBloque, string nuevoEstado)
        {
            bool ok = _agendaDAL.ActualizarEstadoBloque(idBloque, nuevoEstado);
            if (ok)
            {
                try
                {
                    int dniActual = Services.ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                    new BitacoraBLL().RegistrarBitacora(3, $"Bloque horario ID {idBloque} actualizado a estado '{nuevoEstado}'", dniActual, "AgendaMedica");
                }
                catch { }
            }
            return ok;
        }

        private BloqueHorarioBE_DNI101 MapearBloque(DataRow row)
        {
            return new BloqueHorarioBE_DNI101
            {
                IdBloque_DNI101 = Convert.ToInt32(row["IdBloque_DNI101"]),
                IdAgenda_DNI101 = Convert.ToInt32(row["IdAgenda_DNI101"]),
                HoraInicio_DNI101 = (TimeSpan)row["HoraInicio_DNI101"],
                HoraFin_DNI101 = (TimeSpan)row["HoraFin_DNI101"],
                EstadoBloque_DNI101 = row["EstadoBloque_DNI101"].ToString() ?? "Disponible",
                DV = row["DV"] != DBNull.Value ? row["DV"].ToString() : null
            };
        }
    }
}
