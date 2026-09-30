using BE;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class EvaluadorCrecimientoSubject_DNI101
    {
        private readonly List<IObservadorAlertaClinica_DNI101> _observadores = new();

        public void Suscribir(IObservadorAlertaClinica_DNI101 observador)
        {
            if (!_observadores.Contains(observador))
            {
                _observadores.Add(observador);
            }
        }

        public void Desuscribir(IObservadorAlertaClinica_DNI101 observador)
        {
            if (_observadores.Contains(observador))
            {
                _observadores.Remove(observador);
            }
        }

        public void Notificar(AlertaClinicaBE_DNI101 alerta)
        {
            foreach (var obs in _observadores)
            {
                obs.NotificarAlerta(alerta);
            }
        }

        public AlertaClinicaBE_DNI101? EvaluarAlertaCrecimiento(ZScoreResultadoBE_DNI101 zscoreActual, ZScoreResultadoBE_DNI101? zscorePrevio)
        {
            AlertaClinicaBE_DNI101? alerta = null;

            // Regla 1: Desviación severa Z-Score
            if (zscoreActual.ZIMCEdad_DNI101 < -3.0 || zscoreActual.ZIMCEdad_DNI101 > 3.0)
            {
                alerta = new AlertaClinicaBE_DNI101
                {
                    TipoAlerta_DNI101 = "Desviación Z-Score Crítica",
                    Severidad_DNI101 = "Crítica",
                    MensajeAlerta_DNI101 = $"El Z-Score IMC ({zscoreActual.ZIMCEdad_DNI101:F2}) presenta una desviación crítica respecto al patrón OMS.",
                    FechaGeneracion_DNI101 = DateTime.Now
                };
            }
            else if (zscoreActual.ZTallaEdad_DNI101 < -2.0)
            {
                alerta = new AlertaClinicaBE_DNI101
                {
                    TipoAlerta_DNI101 = "Retardo de Crecimiento Estatural",
                    Severidad_DNI101 = "Alta",
                    MensajeAlerta_DNI101 = $"El Z-Score Talla/Edad ({zscoreActual.ZTallaEdad_DNI101:F2}) indica talla baja severa.",
                    FechaGeneracion_DNI101 = DateTime.Now
                };
            }
            // Regla 2: Caída brusca de percentiles comparado con consulta previa
            else if (zscorePrevio != null)
            {
                double caidaPercentilIMC = zscorePrevio.PercentilIMC_DNI101 - zscoreActual.PercentilIMC_DNI101;
                double caidaPercentilTalla = zscorePrevio.PercentilTalla_DNI101 - zscoreActual.PercentilTalla_DNI101;

                if (caidaPercentilIMC >= 25.0 || caidaPercentilTalla >= 25.0)
                {
                    alerta = new AlertaClinicaBE_DNI101
                    {
                        TipoAlerta_DNI101 = "Caída Abrupta de Percentil",
                        Severidad_DNI101 = "Alta",
                        MensajeAlerta_DNI101 = $"Se detectó una caída brusca de percentiles (>{Math.Max(caidaPercentilIMC, caidaPercentilTalla):F1}%) respecto a la consulta anterior.",
                        FechaGeneracion_DNI101 = DateTime.Now
                    };
                }
            }

            if (alerta != null)
            {
                Notificar(alerta);
            }

            return alerta;
        }
    }
}
