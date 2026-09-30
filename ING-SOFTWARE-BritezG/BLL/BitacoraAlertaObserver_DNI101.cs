using BE;
using Services;
using System;

namespace BLL
{
    public class BitacoraAlertaObserver_DNI101 : IObservadorAlertaClinica_DNI101
    {
        private readonly EventoBLL _bitacoraBLL = new();

        public void NotificarAlerta(AlertaClinicaBE_DNI101 alerta)
        {
            try
            {
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                _bitacoraBLL.RegistrarEvento(
                    alerta.Severidad_DNI101 == "Crítica" ? 1 : 2,
                    $"[ALERTA CLÍNICA] {alerta.TipoAlerta_DNI101} - Severidad: {alerta.Severidad_DNI101}. Detalle: {alerta.MensajeAlerta_DNI101}",
                    dniActual,
                    "SeguimientoNutricional"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar alerta en bitácora: {ex.Message}");
            }
        }
    }
}
