using BE;
using DAL;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class TurnoBLL_DNI101
    {
        private readonly TurnoDAL_DNI101 _turnoDAL = new();
        private readonly PacienteDAL_DNI101 _pacienteDAL = new();
        private readonly EventoBLL _bitacoraBLL = new();

        public TurnoBE_DNI101 RegistrarTurno(string dniNiño, DateTime fecha, string horario, string motivo, int? idBloque = null, int? dniNutricionistaParam = null)
        {
            if (string.IsNullOrWhiteSpace(dniNiño)) throw new ArgumentException("El DNI del niño es requerido.", nameof(dniNiño));
            if (string.IsNullOrWhiteSpace(horario)) throw new ArgumentException("El horario del turno es requerido.", nameof(horario));
            if (string.IsNullOrWhiteSpace(motivo)) throw new ArgumentException("El motivo de consulta es requerido.", nameof(motivo));

            dniNiño = dniNiño.Trim();
            var paciente = _pacienteDAL.ObtenerPacientePorDNI(dniNiño);
            if (paciente == null)
            {
                throw new InvalidOperationException($"El paciente con DNI {dniNiño} no se encuentra registrado en el sistema.");
            }

            TimeSpan horaSpan;
            if (!TimeSpan.TryParse(horario.Trim(), out horaSpan))
            {
                if (DateTime.TryParse(horario.Trim(), out DateTime dtParsed))
                {
                    horaSpan = dtParsed.TimeOfDay;
                }
                else
                {
                    horaSpan = new TimeSpan(9, 0, 0);
                }
            }

            int dniNutricionista = dniNutricionistaParam ?? 0;
            if (dniNutricionista == 0)
            {
                try
                {
                    var usuarioActivo = ServicesSessionManager.Instancia.ObtenerUsuarioActivo();
                    if (usuarioActivo != null)
                    {
                        dniNutricionista = usuarioActivo._Dni;
                    }
                }
                catch { }
            }

            // Validar que no exista ya un turno activo para el profesional en la misma fecha y hora
            if (_turnoDAL.ExisteTurnoParaProfesional(dniNutricionista, fecha, horaSpan))
            {
                throw new InvalidOperationException($"El profesional seleccionado (DNI: {dniNutricionista}) ya cuenta con un turno asignado para la fecha {fecha:dd/MM/yyyy} a las {horaSpan:hh\\:mm}. No se permiten superposiciones de turnos.");
            }

            // 10. Generar código único y crear Turno en estado 'Solicitado'
            var turno = new TurnoBE_DNI101
            {
                CodigoTurno_DNI101 = $"TRN-{DateTime.Now:yyyyMMddHHmmss}-{new Random().Next(100, 999)}",
                FechaTurno_DNI101 = fecha.Date,
                HoraTurno_DNI101 = horaSpan,
                MotivoConsulta_DNI101 = motivo.Trim(),
                EstadoTurno_DNI101 = "Solicitado",
                IdPaciente_DNI101 = paciente.IdPaciente_DNI101,
                DniNutricionista_DNI101 = dniNutricionista,
                IdBloque_DNI101 = idBloque,
                Paciente_DNI101 = paciente
            };

            // 13. Calcular Dígito Verificador (DV) individual del Turno
            string cadenaDV = $"{turno.CodigoTurno_DNI101};{turno.FechaTurno_DNI101:yyyy-MM-dd};{turno.HoraTurno_DNI101};{turno.IdPaciente_DNI101};{turno.DniNutricionista_DNI101};{turno.EstadoTurno_DNI101}";
            turno.DV = ServicioBcrypt.CalcularDV(cadenaDV);

            // 11. Persistir el nuevo turno en la base de datos
            int idGenerado = _turnoDAL.Guardar(turno);
            turno.IdTurno_DNI101 = idGenerado;

            // 12. Actualizar el estado del bloque horario asignado a 'Ocupado'
            if (idBloque.HasValue)
            {
                try
                {
                    new AgendaMedicaDAL_DNI101().ActualizarEstadoBloque(idBloque.Value, "Ocupado");
                }
                catch { }
            }

            // 13. Registrar el evento de agendamiento en la bitácora de auditoría
            try
            {
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                _bitacoraBLL.RegistrarEvento(1, $"Turno registrado con éxito ({turno.CodigoTurno_DNI101}) para paciente DNI {dniNiño} el {fecha:dd/MM/yyyy} a las {horaSpan:hh\\:mm}", dniActual, "TurneroNutricional");
            }
            catch { }

            // 14. Recalcular y persistir Dígitos Verificadores globales (tras completar todos los INSERTs)
            try
            {
                new DigitoVerificadorBLL().RecalcularYPersistir();
            }
            catch { }

            return turno;
        }

        public bool AgendarTurno(TurnoBE_DNI101 turno)
        {
            if (turno == null) throw new ArgumentNullException(nameof(turno));
            int id = _turnoDAL.Guardar(turno);
            return id > 0;
        }

        /// <summary>
        /// Método unificado CUN03 - Reprogramar Turno.
        /// Recupera el turno por código, valida su estado, verifica superposiciones
        /// y persiste los nuevos datos de fecha, hora y profesional.
        /// </summary>
        public bool ReprogramarTurno(string codigoTurno, DateTime nuevaFecha, TimeSpan nuevaHora, int? nuevoIdBloque = null, int? nuevoDniNutricionista = null)
        {
            if (string.IsNullOrWhiteSpace(codigoTurno))
                throw new ArgumentException("El código de turno es requerido.", nameof(codigoTurno));

            codigoTurno = codigoTurno.Trim();

            // Paso 3: Recupera el turno con su estado desde la base de datos
            var turno = _turnoDAL.ObtenerPorCodigo(codigoTurno);
            if (turno == null)
                throw new InvalidOperationException($"No se encontró ningún turno registrado con el código '{codigoTurno}'.");

            // Flujo 10.1: Estado no permite reprogramación
            if (string.Equals(turno.EstadoTurno_DNI101, "Asistió", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(turno.EstadoTurno_DNI101, "Asistio", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(turno.EstadoTurno_DNI101, "Cancelado", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"El turno se encuentra en estado '{turno.EstadoTurno_DNI101}' y no permite reprogramación (Flujo 10.1).");
            }

            int dniNutriAValidar = (nuevoDniNutricionista.HasValue && nuevoDniNutricionista.Value > 0)
                ? nuevoDniNutricionista.Value
                : turno.DniNutricionista_DNI101;

            if (_turnoDAL.ExisteTurnoParaProfesional(dniNutriAValidar, nuevaFecha, nuevaHora, idTurnoExcluir: turno.IdTurno_DNI101))
                throw new InvalidOperationException($"El profesional (DNI: {dniNutriAValidar}) ya cuenta con un turno para el {nuevaFecha:dd/MM/yyyy} a las {nuevaHora:hh\\:mm}. No se permiten superposiciones.");

            int? idBloqueAnterior = turno.IdBloque_DNI101 != nuevoIdBloque ? turno.IdBloque_DNI101 : null;

            // Paso 7: Delegación al Patrón State (transición a 'Confirmado')
            turno.Reprogramar(nuevaFecha, nuevaHora, nuevoIdBloque);

            if (nuevoDniNutricionista.HasValue && nuevoDniNutricionista.Value > 0)
                turno.DniNutricionista_DNI101 = nuevoDniNutricionista.Value;

            // Recalcular DV individual
            string cadenaDV = $"{turno.CodigoTurno_DNI101};{turno.FechaTurno_DNI101:yyyy-MM-dd};{turno.HoraTurno_DNI101};{turno.IdPaciente_DNI101};{turno.DniNutricionista_DNI101};{turno.EstadoTurno_DNI101}";
            turno.DV = ServicioBcrypt.CalcularDV(cadenaDV);

            // Paso 8: Persistir actualización
            bool ok = _turnoDAL.ReprogramarTurno(turno.IdTurno_DNI101, nuevaFecha, nuevaHora, nuevoIdBloque, turno.DniNutricionista_DNI101, turno.DV);
            if (!ok)
                throw new InvalidOperationException("No se pudo completar la reprogramación. Se detectó un conflicto de concurrencia en la base de datos.");

            // Liberar bloque anterior y ocupar el nuevo
            if (idBloqueAnterior.HasValue)
            {
                try { new AgendaMedicaDAL_DNI101().ActualizarEstadoBloque(idBloqueAnterior.Value, "Disponible"); } catch { }
            }
            if (nuevoIdBloque.HasValue)
            {
                try { new AgendaMedicaDAL_DNI101().ActualizarEstadoBloque(nuevoIdBloque.Value, "Ocupado"); } catch { }
            }

            // Registrar en bitácora
            try
            {
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                _bitacoraBLL.RegistrarEvento(2, $"Turno {codigoTurno} reprogramado para {nuevaFecha:dd/MM/yyyy} a las {nuevaHora:hh\\:mm} (Estado: {turno.EstadoTurno_DNI101})", dniActual, "TurneroNutricional");
            }
            catch { }

            // Recalcular DV globales
            try { new DigitoVerificadorBLL().RecalcularYPersistir(); } catch { }

            return true;
        }

        /// <summary>
        /// Método unificado de ModificarTurno (CUN04).
        /// Permite modificar motivo, estado y opcionalmente fecha y hora del turno en un solo flujo seguro.
        /// </summary>
        public bool ModificarTurno(string codigoTurno, string motivo, string? estado = null, DateTime? fecha = null, TimeSpan? hora = null)
        {
            if (string.IsNullOrWhiteSpace(codigoTurno))
                throw new ArgumentException("El código de turno es requerido.", nameof(codigoTurno));

            if (string.IsNullOrWhiteSpace(motivo))
                throw new ArgumentException("El motivo de consulta no puede estar vacío.", nameof(motivo));

            codigoTurno = codigoTurno.Trim();
            motivo = motivo.Trim();

            // Paso 3 y 4: Recupera Turno con su Estado desde la base de datos
            var turno = _turnoDAL.ObtenerPorCodigo(codigoTurno);
            if (turno == null)
                throw new InvalidOperationException($"No se encontró ningún turno registrado con el código '{codigoTurno}'.");

            string estadoAnterior = turno.EstadoTurno_DNI101;

            // Flujo alternativo 7.1: Validar si el estado anterior permite modificación
            if (string.Equals(estadoAnterior, "Asistió", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(estadoAnterior, "Asistio", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(estado) &&
                    !string.Equals(estado, "Asistió", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(estado, "Asistio", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("El turno ya figura como 'Asistió' y no permite cambiar a otro estado.");
                }
            }
            else if (string.Equals(estadoAnterior, "Cancelado", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(estado) &&
                    !string.Equals(estado, "Cancelado", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("El turno se encuentra en estado 'Cancelado' y no puede reactivarse.");
                }
            }

            // Validar superposición de turnos si se modifica la fecha u hora
            DateTime fechaFinal = fecha?.Date ?? turno.FechaTurno_DNI101.Date;
            TimeSpan horaFinal = hora ?? turno.HoraTurno_DNI101;

            if (fecha.HasValue || hora.HasValue)
            {
                if (_turnoDAL.ExisteTurnoParaProfesional(turno.DniNutricionista_DNI101, fechaFinal, horaFinal, idTurnoExcluir: turno.IdTurno_DNI101))
                    throw new InvalidOperationException($"El profesional ya cuenta con un turno para el {fechaFinal:dd/MM/yyyy} a las {horaFinal:hh\\:mm}. No se permiten superposiciones.");

                turno.FechaTurno_DNI101 = fechaFinal;
                turno.HoraTurno_DNI101 = horaFinal;
            }

            // Aplicar cambios de estado mediante Patrón State (CUN04)
            string estadoFinal = !string.IsNullOrWhiteSpace(estado) ? estado.Trim() : estadoAnterior;
            bool liberarBloque = string.Equals(estadoFinal, "Cancelado", StringComparison.OrdinalIgnoreCase) &&
                                 !string.Equals(estadoAnterior, "Cancelado", StringComparison.OrdinalIgnoreCase) &&
                                 turno.IdBloque_DNI101.HasValue;

            if (string.Equals(estadoFinal, "Asistió", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(estadoFinal, "Asistio", StringComparison.OrdinalIgnoreCase))
            {
                turno.Atender();
            }
            else if (string.Equals(estadoFinal, "Cancelado", StringComparison.OrdinalIgnoreCase))
            {
                turno.Cancelar(motivo);
            }
            else if (!string.IsNullOrWhiteSpace(estado))
            {
                turno.ConfigurarEstadoPorNombre(estadoFinal);
            }

            turno.MotivoConsulta_DNI101 = motivo;

            // Recalcular DV individual
            string cadenaDV = $"{turno.CodigoTurno_DNI101};{turno.FechaTurno_DNI101:yyyy-MM-dd};{turno.HoraTurno_DNI101};{turno.IdPaciente_DNI101};{turno.DniNutricionista_DNI101};{turno.EstadoTurno_DNI101}";
            turno.DV = ServicioBcrypt.CalcularDV(cadenaDV);

            // Persistir cambios de forma unificada
            bool ok = _turnoDAL.ModificarTurno(turno.IdTurno_DNI101, turno.FechaTurno_DNI101, turno.HoraTurno_DNI101, motivo, turno.EstadoTurno_DNI101, turno.DV);
            if (!ok)
                throw new InvalidOperationException("No se pudo completar la modificación del turno en la base de datos.");

            if (liberarBloque)
            {
                try { new AgendaMedicaDAL_DNI101().ActualizarEstadoBloque(turno.IdBloque_DNI101!.Value, "Disponible"); } catch { }
            }

            // Registrar en bitácora
            try
            {
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                _bitacoraBLL.RegistrarEvento(2, $"Turno {codigoTurno} modificado (CUN04). Estado: '{turno.EstadoTurno_DNI101}', Motivo: '{motivo}'", dniActual, "TurneroNutricional");
            }
            catch { }

            // Recalcular DV globales
            try { new DigitoVerificadorBLL().RecalcularYPersistir(); } catch { }

            return true;
        }

        /// <summary>
        /// Método unificado CUN05 - Cancelar Turno.
        /// Valida el estado, aplica la transición 'Cancelado' y libera el bloque horario.
        /// </summary>
        public bool CancelarTurno(string codigoTurno, string motivo = "Cancelado por el profesional/paciente")
        {
            if (string.IsNullOrWhiteSpace(codigoTurno))
                throw new ArgumentException("El código de turno es requerido.", nameof(codigoTurno));

            codigoTurno = codigoTurno.Trim();

            // Paso 6: Recupera el turno por su código
            var turno = _turnoDAL.ObtenerPorCodigo(codigoTurno);
            if (turno == null)
                throw new InvalidOperationException($"No se encontró ningún turno registrado con el código '{codigoTurno}'.");

            // Flujo 6.1.1: Estado no permite cancelación
            if (string.Equals(turno.EstadoTurno_DNI101, "Cancelado", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("El turno ya se encuentra cancelado (Flujo 6.1.1).");

            if (string.Equals(turno.EstadoTurno_DNI101, "Asistió", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(turno.EstadoTurno_DNI101, "Asistio", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("No se puede cancelar un turno que ya fue atendido (Flujo 6.1.1).");
            }

            // Paso 6 y 7: Delegación al Patrón State
            turno.Cancelar(motivo);

            // Recalcular DV individual
            string cadenaDV = $"{turno.CodigoTurno_DNI101};{turno.FechaTurno_DNI101:yyyy-MM-dd};{turno.HoraTurno_DNI101};{turno.IdPaciente_DNI101};{turno.DniNutricionista_DNI101};{turno.EstadoTurno_DNI101}";
            turno.DV = ServicioBcrypt.CalcularDV(cadenaDV);

            // Persistir cancelación
            bool ok = _turnoDAL.CancelarTurno(turno.IdTurno_DNI101, motivo, turno.DV);
            if (!ok)
                throw new InvalidOperationException("No se pudo completar la cancelación del turno en la base de datos.");

            // Paso 8: Liberar bloque horario
            if (turno.IdBloque_DNI101.HasValue)
            {
                try { new AgendaMedicaDAL_DNI101().ActualizarEstadoBloque(turno.IdBloque_DNI101.Value, "Disponible"); } catch { }
            }

            // Registrar en bitácora
            try
            {
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                _bitacoraBLL.RegistrarEvento(2, $"Turno {codigoTurno} cancelado (CUN05). Motivo: {motivo}", dniActual, "TurneroNutricional");
            }
            catch { }

            // Recalcular DV globales
            try { new DigitoVerificadorBLL().RecalcularYPersistir(); } catch { }

            return true;
        }



        public List<TurnoBE_DNI101> ListarTurnos(DateTime? fecha = null, string? estado = null)
        {
            List<TurnoBE_DNI101> lista;
            if (fecha.HasValue)
            {
                lista = _turnoDAL.ListarTurnosPorFecha(fecha.Value);
            }
            else
            {
                lista = _turnoDAL.ListarTodos();
            }

            if (!string.IsNullOrWhiteSpace(estado) && estado != "Todos")
            {
                lista = lista.Where(t => string.Equals(t.EstadoTurno_DNI101, estado, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return lista;
        }

        public TurnoBE_DNI101? ObtenerPorId(int idTurno)
        {
            return _turnoDAL.ObtenerPorId(idTurno);
        }

        public TurnoBE_DNI101? ObtenerPorCodigo(string codigoTurno)
        {
            return _turnoDAL.ObtenerPorCodigo(codigoTurno);
        }

        public bool MarcarAsistencia(int idTurno)
        {
            return _turnoDAL.ActualizarEstado(idTurno, "Asistio");
        }
    }
}
