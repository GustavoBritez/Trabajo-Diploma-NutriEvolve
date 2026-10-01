using System;

namespace BE
{
    public class TurnoSolicitadoState_DNI101 : IEstadoTurno_DNI101
    {
        public string NombreEstado => "Solicitado";

        public void Atender(TurnoBE_DNI101 turno)
        {
            turno.EstadoTurno_DNI101 = "Asistió";
            turno.EstadoActual_DNI101 = new TurnoAsistioState_DNI101();
        }

        public void Cancelar(TurnoBE_DNI101 turno, string motivo)
        {
            turno.EstadoTurno_DNI101 = "Cancelado";
            turno.MotivoConsulta_DNI101 = string.IsNullOrEmpty(turno.MotivoConsulta_DNI101) 
                ? $"Cancelado: {motivo}" 
                : $"{turno.MotivoConsulta_DNI101} | Cancelado: {motivo}";
            turno.EstadoActual_DNI101 = new TurnoCanceladoState_DNI101();
        }

        public void Reprogramar(TurnoBE_DNI101 turno, DateTime nuevaFecha, TimeSpan nuevaHora, int? nuevoIdBloque)
        {
            turno.FechaTurno_DNI101 = nuevaFecha;
            turno.HoraTurno_DNI101 = nuevaHora;
            turno.IdBloque_DNI101 = nuevoIdBloque;
            turno.EstadoTurno_DNI101 = "Confirmado";
            turno.EstadoActual_DNI101 = new TurnoConfirmadoState_DNI101();
        }
    }

    public class TurnoConfirmadoState_DNI101 : IEstadoTurno_DNI101
    {
        public string NombreEstado => "Confirmado";

        public void Atender(TurnoBE_DNI101 turno)
        {
            turno.EstadoTurno_DNI101 = "Asistió";
            turno.EstadoActual_DNI101 = new TurnoAsistioState_DNI101();
        }

        public void Cancelar(TurnoBE_DNI101 turno, string motivo)
        {
            turno.EstadoTurno_DNI101 = "Cancelado";
            turno.MotivoConsulta_DNI101 = $"{turno.MotivoConsulta_DNI101} | Cancelado: {motivo}";
            turno.EstadoActual_DNI101 = new TurnoCanceladoState_DNI101();
        }

        public void Reprogramar(TurnoBE_DNI101 turno, DateTime nuevaFecha, TimeSpan nuevaHora, int? nuevoIdBloque)
        {
            turno.FechaTurno_DNI101 = nuevaFecha;
            turno.HoraTurno_DNI101 = nuevaHora;
            turno.IdBloque_DNI101 = nuevoIdBloque;
        }
    }

    public class TurnoAsistioState_DNI101 : IEstadoTurno_DNI101
    {
        public string NombreEstado => "Asistió";

        public void Atender(TurnoBE_DNI101 turno)
        {
            throw new InvalidOperationException("El turno ya fue atendido.");
        }

        public void Cancelar(TurnoBE_DNI101 turno, string motivo)
        {
            throw new InvalidOperationException("No se puede cancelar un turno ya atendido.");
        }

        public void Reprogramar(TurnoBE_DNI101 turno, DateTime nuevaFecha, TimeSpan nuevaHora, int? nuevoIdBloque)
        {
            throw new InvalidOperationException("No se puede reprogramar un turno ya atendido.");
        }
    }

    public class TurnoCanceladoState_DNI101 : IEstadoTurno_DNI101
    {
        public string NombreEstado => "Cancelado";

        public void Atender(TurnoBE_DNI101 turno)
        {
            throw new InvalidOperationException("No se puede atender un turno cancelado.");
        }

        public void Cancelar(TurnoBE_DNI101 turno, string motivo)
        {
            throw new InvalidOperationException("El turno ya se encuentra cancelado.");
        }

        public void Reprogramar(TurnoBE_DNI101 turno, DateTime nuevaFecha, TimeSpan nuevaHora, int? nuevoIdBloque)
        {
            throw new InvalidOperationException("No se puede reprogramar directamente un turno cancelado.");
        }
    }
}
