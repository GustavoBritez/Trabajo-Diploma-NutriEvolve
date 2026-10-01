using System;

namespace BE
{
    public interface IEstadoTurno_DNI101
    {
        string NombreEstado { get; }
        void Atender(TurnoBE_DNI101 turno);
        void Cancelar(TurnoBE_DNI101 turno, string motivo);
        void Reprogramar(TurnoBE_DNI101 turno, DateTime nuevaFecha, TimeSpan nuevaHora, int? nuevoIdBloque);
    }
}
