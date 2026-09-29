using System;

namespace BE
{
    public class TurnoBE_DNI101
    {
        public int IdTurno_DNI101 { get; set; }
        public string CodigoTurno_DNI101 { get; set; } = string.Empty;
        public DateTime FechaTurno_DNI101 { get; set; } = DateTime.Today;
        public TimeSpan HoraTurno_DNI101 { get; set; }
        public string MotivoConsulta_DNI101 { get; set; } = string.Empty;
        public string EstadoTurno_DNI101 { get; set; } = "Solicitado"; // Solicitado, Confirmado, Asistió, Cancelado
        public int IdPaciente_DNI101 { get; set; }
        public int DniNutricionista_DNI101 { get; set; }
        public int? IdBloque_DNI101 { get; set; }
        public PacienteBE_DNI101? Paciente_DNI101 { get; set; }
        public string? DV { get; set; }

        private IEstadoTurno_DNI101 _estadoActual = new TurnoSolicitadoState_DNI101();

        public IEstadoTurno_DNI101 EstadoActual_DNI101
        {
            get => _estadoActual;
            set
            {
                _estadoActual = value;
                EstadoTurno_DNI101 = value.NombreEstado;
            }
        }

        public TurnoBE_DNI101()
        {
            ConfigurarEstadoPorNombre(EstadoTurno_DNI101);
        }

        public void CambiarEstado(IEstadoTurno_DNI101 nuevoEstado)
        {
            EstadoActual_DNI101 = nuevoEstado;
        }

        public void ConfigurarEstadoPorNombre(string nombreEstado)
        {
            switch (nombreEstado)
            {
                case "Confirmado":
                    _estadoActual = new TurnoConfirmadoState_DNI101();
                    break;
                case "Asistió":
                case "Asistio":
                    _estadoActual = new TurnoAsistioState_DNI101();
                    break;
                case "Cancelado":
                    _estadoActual = new TurnoCanceladoState_DNI101();
                    break;
                case "Solicitado":
                default:
                    _estadoActual = new TurnoSolicitadoState_DNI101();
                    break;
            }
            EstadoTurno_DNI101 = _estadoActual.NombreEstado;
        }

        public void Atender()
        {
            EstadoActual_DNI101.Atender(this);
        }

        public void Cancelar(string motivo)
        {
            EstadoActual_DNI101.Cancelar(this, motivo);
        }

        public void Reprogramar(DateTime nuevaFecha, TimeSpan nuevaHora, int? nuevoIdBloque = null)
        {
            EstadoActual_DNI101.Reprogramar(this, nuevaFecha, nuevaHora, nuevoIdBloque);
        }
    }
}
