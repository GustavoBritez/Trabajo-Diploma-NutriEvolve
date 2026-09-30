using BE;
using System;

namespace BLL
{
    public class AlertaVisualUIObserver_DNI101 : IObservadorAlertaClinica_DNI101
    {
        private readonly Action<AlertaClinicaBE_DNI101> _uiAction;

        public AlertaVisualUIObserver_DNI101(Action<AlertaClinicaBE_DNI101> uiAction)
        {
            _uiAction = uiAction ?? throw new ArgumentNullException(nameof(uiAction));
        }

        public void NotificarAlerta(AlertaClinicaBE_DNI101 alerta)
        {
            _uiAction.Invoke(alerta);
        }
    }
}
