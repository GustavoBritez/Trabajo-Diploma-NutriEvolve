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

            // 13. Recalcular y persistir Dígitos Verificadores globales
            try
            {
                new DigitoVerificadorBLL().RecalcularYPersistir();
            }
            catch { }

            // 14. Registrar el evento de agendamiento en la bitácora de auditoría
            try
            {
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                _bitacoraBLL.RegistrarEvento(1, $"Turno registrado con éxito ({turno.CodigoTurno_DNI101}) para paciente DNI {dniNiño} el {fecha:dd/MM/yyyy} a las {horaSpan:hh\\:mm}", dniActual, "TurneroNutricional");
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

        // Métodos de diagrama de secuencia PN1 - CUN03 Reprogramar Turno
        public void ReprogramarTurno(string horario)
        {
            try
            {
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                _bitacoraBLL.RegistrarEvento(2, $"Solicitud de reprogramación de turno al horario {horario}", dniActual, "TurneroNutricional");
            }
            catch { }
        }

        public bool ReprogramarTurno(int idTurno, DateTime nuevaFecha, TimeSpan nuevaHora, int? nuevoIdBloque = null, int? nuevoDniNutricionista = null)
        {
            // Paso 3: El módulo recupera Turno con su Estado desde la base de datos
            var turno = _turnoDAL.ObtenerPorId(idTurno);
            if (turno == null)
            {
                throw new InvalidOperationException("El turno no fue encontrado en la base de datos.");
            }

            // Flujo 10.1: Estado no permite modificación (Asistió/Cancelado)
            if (string.Equals(turno.EstadoTurno_DNI101, "Asistió", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(turno.EstadoTurno_DNI101, "Asistio", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(turno.EstadoTurno_DNI101, "Cancelado", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"El turno se encuentra en estado '{turno.EstadoTurno_DNI101}' y no permite reprogramación (Flujo 10.1).");
            }

            int? idBloqueAnterior = turno.IdBloque_DNI101 != nuevoIdBloque ? turno.IdBloque_DNI101 : null;

            // Paso 7: El módulo delega la lógica de cambio de estado y validación a la clase concreta del estado actual (Patrón State)
            turno.Reprogramar(nuevaFecha, nuevaHora, nuevoIdBloque);

            if (nuevoDniNutricionista.HasValue && nuevoDniNutricionista.Value > 0)
            {
                turno.DniNutricionista_DNI101 = nuevoDniNutricionista.Value;
            }

            // Paso 9: Recalcula Dígitos Verificadores
            string cadenaDV = $"{turno.CodigoTurno_DNI101};{turno.FechaTurno_DNI101:yyyy-MM-dd};{turno.HoraTurno_DNI101};{turno.IdPaciente_DNI101};{turno.DniNutricionista_DNI101};{turno.EstadoTurno_DNI101}";
            turno.DV = ServicioBcrypt.CalcularDV(cadenaDV);

            // Paso 8: Persiste la actualización
            bool ok = _turnoDAL.ReprogramarTurno(idTurno, nuevaFecha, nuevaHora, nuevoIdBloque, turno.DniNutricionista_DNI101, turno.DV);
            if (!ok)
            {
                // Flujo alternativo 9.1: Conflicto de concurrencia
                throw new InvalidOperationException("No se pudo completar la reprogramación. Se detectó un conflicto de concurrencia en la base de datos.");
            }

            // Paso 8 (continuación): Actualiza el estado del bloque horario anterior a 'Disponible'
            if (idBloqueAnterior.HasValue)
            {
                try
                {
                    new AgendaMedicaDAL_DNI101().ActualizarEstadoBloque(idBloqueAnterior.Value, "Disponible");
                }
                catch { }
            }

            // Actualiza el nuevo bloque a 'Ocupado'
            if (nuevoIdBloque.HasValue)
            {
                try
                {
                    new AgendaMedicaDAL_DNI101().ActualizarEstadoBloque(nuevoIdBloque.Value, "Ocupado");
                }
                catch { }
            }

            // Recalcula y persiste DV globales
            try
            {
                new DigitoVerificadorBLL().RecalcularYPersistir();
            }
            catch { }

            // Paso 9: Registra el evento de reprogramación en la bitácora de auditoría
            try
            {
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                _bitacoraBLL.RegistrarEvento(2, $"Turno {turno.CodigoTurno_DNI101} reprogramado para {nuevaFecha:dd/MM/yyyy} a las {nuevaHora:hh\\:mm} (Estado: {turno.EstadoTurno_DNI101})", dniActual, "TurneroNutricional");
            }
            catch { }

            return true;
        }

        public void ModificarTurno(string codigoTurno, DateTime fecha, string hora, string motivo)
        {
            var turno = _turnoDAL.ObtenerPorCodigo(codigoTurno);
            if (turno != null)
            {
                TimeSpan.TryParse(hora, out TimeSpan horaSpan);
                ModificarTurno(turno.IdTurno_DNI101, fecha, horaSpan, motivo);
            }
        }

        public bool ModificarTurno(int idTurno, DateTime fecha, TimeSpan hora, string motivo)
        {
            bool ok = _turnoDAL.ModificarTurno(idTurno, fecha, hora, motivo);
            if (ok)
            {
                try
                {
                    int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                    _bitacoraBLL.RegistrarEvento(2, $"Turno ID {idTurno} modificado ({motivo})", dniActual, "TurneroNutricional");
                }
                catch { }
            }
            return ok;
        }

        // Método de diagrama de secuencia PN1 - CUN04 Modificar Turno (Estado y Motivo)
        public bool ModificarTurno(string codigoTurno, string motivo, string estado)
        {
            if (string.IsNullOrWhiteSpace(codigoTurno))
                throw new ArgumentException("El código de turno es requerido.", nameof(codigoTurno));

            if (string.IsNullOrWhiteSpace(motivo))
                throw new ArgumentException("El motivo de consulta no puede estar vacío.", nameof(motivo));

            if (string.IsNullOrWhiteSpace(estado))
                throw new ArgumentException("El estado del turno es requerido.", nameof(estado));

            codigoTurno = codigoTurno.Trim();
            motivo = motivo.Trim();
            estado = estado.Trim();

            // Paso 3 y 4: Recupera Turno con su Estado desde la base de datos
            var turno = _turnoDAL.ObtenerPorCodigo(codigoTurno);
            if (turno == null)
            {
                throw new InvalidOperationException($"No se encontró ningún turno registrado con el código '{codigoTurno}'.");
            }

            string estadoAnterior = turno.EstadoTurno_DNI101;

            // Flujo alternativo 7.1: Estado no permite modificación
            if (string.Equals(estadoAnterior, "Asistió", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(estadoAnterior, "Asistio", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.Equals(estado, "Asistió", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(estado, "Asistio", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("El turno ya figura como 'Asistió' y no permite cambiar a otro estado.");
                }
            }
            else if (string.Equals(estadoAnterior, "Cancelado", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.Equals(estado, "Cancelado", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("El turno se encuentra en estado 'Cancelado' y no puede reactivarse.");
                }
            }

            bool liberarBloque = string.Equals(estado, "Cancelado", StringComparison.OrdinalIgnoreCase) &&
                                 !string.Equals(estadoAnterior, "Cancelado", StringComparison.OrdinalIgnoreCase) &&
                                 turno.IdBloque_DNI101.HasValue;

            // Paso 7: Delegación al patrón State y configuración de la entidad
            if (string.Equals(estado, "Asistió", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(estado, "Asistio", StringComparison.OrdinalIgnoreCase))
            {
                turno.Atender();
            }
            else if (string.Equals(estado, "Cancelado", StringComparison.OrdinalIgnoreCase))
            {
                turno.Cancelar(motivo);
            }
            else
            {
                turno.ConfigurarEstadoPorNombre(estado);
            }

            turno.MotivoConsulta_DNI101 = motivo;

            // Paso 9: Recalcular Dígito Verificador (DV) individual del Turno
            string cadenaDV = $"{turno.CodigoTurno_DNI101};{turno.FechaTurno_DNI101:yyyy-MM-dd};{turno.HoraTurno_DNI101};{turno.IdPaciente_DNI101};{turno.DniNutricionista_DNI101};{turno.EstadoTurno_DNI101}";
            turno.DV = ServicioBcrypt.CalcularDV(cadenaDV);

            // Persistir cambios en la base de datos
            bool ok = _turnoDAL.ModificarTurnoEstadoYMotivo(turno.IdTurno_DNI101, motivo, turno.EstadoTurno_DNI101, turno.DV);
            if (!ok)
            {
                throw new InvalidOperationException("No se pudo completar la modificación del turno en la base de datos.");
            }

            // Paso 8: Si el nuevo estado es Cancelado, liberar bloque horario
            if (liberarBloque)
            {
                try
                {
                    new AgendaMedicaDAL_DNI101().ActualizarEstadoBloque(turno.IdBloque_DNI101!.Value, "Disponible");
                }
                catch { }
            }

            // Recalcular y persistir Dígitos Verificadores globales
            try
            {
                new DigitoVerificadorBLL().RecalcularYPersistir();
            }
            catch { }

            // Paso 10: Registrar evento en Bitácora de auditoría
            try
            {
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                _bitacoraBLL.RegistrarEvento(2, $"Turno {codigoTurno} modificado con éxito (CUN04). Estado: '{turno.EstadoTurno_DNI101}', Motivo: '{motivo}'", dniActual, "TurneroNutricional");
            }
            catch { }

            return true;
        }

        // Método de diagrama de secuencia PN1 - CUN05 Cancelar Turno
        public bool CancelarTurno(string codigoTurno, string motivo = "Cancelado por el profesional/paciente")
        {
            if (string.IsNullOrWhiteSpace(codigoTurno))
                throw new ArgumentException("El código de turno es requerido.", nameof(codigoTurno));

            codigoTurno = codigoTurno.Trim();

            // Paso 6: El módulo recupera el turno desde la base de datos por su Código
            var turno = _turnoDAL.ObtenerPorCodigo(codigoTurno);
            if (turno == null)
            {
                throw new InvalidOperationException($"No se encontró ningún turno registrado con el código '{codigoTurno}'.");
            }

            // Flujo alternativo 6.1.1: Estado no permite cancelación
            if (string.Equals(turno.EstadoTurno_DNI101, "Cancelado", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("El turno ya se encuentra cancelado (Flujo 6.1.1).");
            }

            if (string.Equals(turno.EstadoTurno_DNI101, "Asistió", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(turno.EstadoTurno_DNI101, "Asistio", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("No se puede cancelar un turno que ya fue atendido (Flujo 6.1.1).");
            }

            // Paso 6 y 7: Delegación a la entidad y al patrón State
            turno.Cancelar(motivo);

            // Paso 9: Recalcular Dígito Verificador (DV) individual del Turno
            string cadenaDV = $"{turno.CodigoTurno_DNI101};{turno.FechaTurno_DNI101:yyyy-MM-dd};{turno.HoraTurno_DNI101};{turno.IdPaciente_DNI101};{turno.DniNutricionista_DNI101};{turno.EstadoTurno_DNI101}";
            turno.DV = ServicioBcrypt.CalcularDV(cadenaDV);

            // Persistir cambios en la base de datos
            bool ok = _turnoDAL.CancelarTurno(turno.IdTurno_DNI101, motivo, turno.DV);
            if (!ok)
            {
                throw new InvalidOperationException("No se pudo completar la cancelación del turno en la base de datos.");
            }

            // Paso 8: Liberar el bloque horario asignado en la agenda médica
            if (turno.IdBloque_DNI101.HasValue)
            {
                try
                {
                    new AgendaMedicaDAL_DNI101().ActualizarEstadoBloque(turno.IdBloque_DNI101.Value, "Disponible");
                }
                catch { }
            }

            // Paso 10: Recalcular y persistir Dígitos Verificadores globales
            try
            {
                new DigitoVerificadorBLL().RecalcularYPersistir();
            }
            catch { }

            // Paso 11: Registrar evento en la bitácora de auditoría
            try
            {
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                _bitacoraBLL.RegistrarEvento(2, $"Turno {turno.CodigoTurno_DNI101} cancelado con éxito (CUN05). Motivo: {motivo}", dniActual, "TurneroNutricional");
            }
            catch { }

            return true;
        }

        // Sobrecarga de conveniencia por si se requiere invocar mediante Id
        public bool CancelarTurno(int idTurno, string motivo = "Cancelado por el profesional/paciente")
        {
            var turno = _turnoDAL.ObtenerPorId(idTurno);
            if (turno == null)
            {
                throw new InvalidOperationException($"No se encontró ningún turno registrado con el ID {idTurno}.");
            }
            return CancelarTurno(turno.CodigoTurno_DNI101, motivo);
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
