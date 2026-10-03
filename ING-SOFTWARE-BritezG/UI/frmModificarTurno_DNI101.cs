using BE;
using BLL;
using Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    public partial class frmModificarTurno_DNI101 : Form, IIdiomaObserver
    {
        private TurnoBE_DNI101? _turnoActual;
        private readonly TurnoBLL_DNI101 _turnoBLL = new();
        private readonly IdiomaBLL _idiomaBLL = new();

        public frmModificarTurno_DNI101(TurnoBE_DNI101? turno = null)
        {
            InitializeComponent();
            _turnoActual = turno;
            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();
        }

        public frmModificarTurno_DNI101(string codigoTurno)
        {
            InitializeComponent();
            txtCodigoTurno.Text = codigoTurno ?? string.Empty;
            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();
        }

        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                TraductorUI.TraducirFormulario(this, _idiomaBLL);
                TraducirComboEstados();
            }
        }

        private void TraducirComboEstados()
        {
            int indexPrevio = cmbEstado.SelectedIndex;
            cmbEstado.Items.Clear();
            cmbEstado.Items.Add(_idiomaBLL.Traducir("estado_solicitado"));
            cmbEstado.Items.Add(_idiomaBLL.Traducir("estado_confirmado"));
            cmbEstado.Items.Add(_idiomaBLL.Traducir("estado_asistio"));
            cmbEstado.Items.Add(_idiomaBLL.Traducir("estado_cancelado"));
            if (indexPrevio >= 0 && indexPrevio < cmbEstado.Items.Count)
            {
                cmbEstado.SelectedIndex = indexPrevio;
            }
        }

        private void frmModificarTurno_DNI101_Load(object sender, EventArgs e)
        {
            TraducirComboEstados();

            if (_turnoActual != null)
            {
                txtCodigoTurno.Text = _turnoActual.CodigoTurno_DNI101;
                MostrarDatosTurno(_turnoActual);
            }
            else if (!string.IsNullOrWhiteSpace(txtCodigoTurno.Text))
            {
                BuscarTurnoPorCodigo(txtCodigoTurno.Text.Trim());
            }
            else
            {
                DeshabilitarEdicion();
            }
        }

        private void btnBuscarCodigo_Click(object sender, EventArgs e)
        {
            string codigo = txtCodigoTurno.Text.Trim();
            if (string.IsNullOrWhiteSpace(codigo))
            {
                _idiomaBLL.MostrarMensaje("msg_ingrese_codigo_turno", "titulo_busqueda_requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCodigoTurno.Focus();
                return;
            }

            BuscarTurnoPorCodigo(codigo);
        }

        private void txtCodigoTurno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnBuscarCodigo_Click(sender, e);
            }
        }

        private void BuscarTurnoPorCodigo(string codigo)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var turno = _turnoBLL.ObtenerPorCodigo(codigo);
                if (turno == null)
                {
                    // Flujo alternativo 3.1: Turno no encontrado
                    _idiomaBLL.MostrarMensaje("msg_turno_no_encontrado_cod", "titulo_turno_no_encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning, codigo);
                    _turnoActual = null;
                    DeshabilitarEdicion();
                    return;
                }

                _turnoActual = turno;
                MostrarDatosTurno(_turnoActual);
            }
            catch (Exception ex)
            {
                _idiomaBLL.MostrarMensaje("msg_error_consultar_turno", "titulo_error_consulta", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void MostrarDatosTurno(TurnoBE_DNI101 turno)
        {
            string nomPaciente = turno.Paciente_DNI101 != null ? turno.Paciente_DNI101.NombreCompleto : "Paciente #" + turno.IdPaciente_DNI101;
            string dniPaciente = turno.Paciente_DNI101?.DNINiño_DNI101 ?? "-";
            string obraSocial = turno.Paciente_DNI101?.ObraSocial_DNI101 ?? "Particular";

            lblInfoTurno.Text =
                $"• Código: {turno.CodigoTurno_DNI101} | Estado Actual: {turno.EstadoTurno_DNI101}\n" +
                $"• Paciente: {nomPaciente} (DNI: {dniPaciente}) | Obra Social: {obraSocial}\n" +
                $"• Turno Asignado: {turno.FechaTurno_DNI101:dd/MM/yyyy} a las {turno.HoraTurno_DNI101:hh\\:mm} | Nutricionista DNI: {turno.DniNutricionista_DNI101}";

            // Seleccionar estado en ComboBox por índice canónico
            if (string.Equals(turno.EstadoTurno_DNI101, "Solicitado", StringComparison.OrdinalIgnoreCase))
                cmbEstado.SelectedIndex = 0;
            else if (string.Equals(turno.EstadoTurno_DNI101, "Confirmado", StringComparison.OrdinalIgnoreCase))
                cmbEstado.SelectedIndex = 1;
            else if (string.Equals(turno.EstadoTurno_DNI101, "Asistió", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(turno.EstadoTurno_DNI101, "Asistio", StringComparison.OrdinalIgnoreCase))
                cmbEstado.SelectedIndex = 2;
            else if (string.Equals(turno.EstadoTurno_DNI101, "Cancelado", StringComparison.OrdinalIgnoreCase))
                cmbEstado.SelectedIndex = 3;

            txtMotivo.Text = turno.MotivoConsulta_DNI101;

            // Habilitar controles de edición
            cmbEstado.Enabled = true;
            txtMotivo.Enabled = true;
            btnGuardar.Enabled = true;

            // Alerta si el estado actual es terminal
            if (string.Equals(turno.EstadoTurno_DNI101, "Asistió", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(turno.EstadoTurno_DNI101, "Asistio", StringComparison.OrdinalIgnoreCase))
            {
                lblInfoTurno.Text += "\n⚠️ Turno finalizado ('Asistió'): Solo se permite actualizar observaciones.";
            }
            else if (string.Equals(turno.EstadoTurno_DNI101, "Cancelado", StringComparison.OrdinalIgnoreCase))
            {
                lblInfoTurno.Text += "\n⚠️ Turno cerrado ('Cancelado'): No es posible reactivar el turno.";
            }
        }

        private void DeshabilitarEdicion()
        {
            lblInfoTurno.Text = "Ingrese un Código de Turno y presione 'Buscar' o seleccione un turno desde el Turnero Principal.";
            cmbEstado.SelectedIndex = -1;
            cmbEstado.Enabled = false;
            txtMotivo.Text = string.Empty;
            txtMotivo.Enabled = false;
            btnGuardar.Enabled = false;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (_turnoActual == null)
            {
                _idiomaBLL.MostrarMensaje("msg_sel_turno_antes_modificar", "titulo_turno_requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string codigoTurno = _turnoActual.CodigoTurno_DNI101;
            string nuevoMotivo = txtMotivo.Text.Trim();

            // Mapear estado canónico para persistencia en base de datos
            string nuevoEstado = cmbEstado.SelectedIndex switch
            {
                0 => "Solicitado",
                1 => "Confirmado",
                2 => "Asistió",
                3 => "Cancelado",
                _ => cmbEstado.SelectedItem?.ToString() ?? "Solicitado"
            };

            // Flujo alternativo 5.1: Motivo de consulta vacío
            if (string.IsNullOrWhiteSpace(nuevoMotivo))
            {
                _idiomaBLL.MostrarMensaje("msg_motivo_vacio", "titulo_validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMotivo.Focus();
                return;
            }

            if (cmbEstado.SelectedIndex < 0)
            {
                _idiomaBLL.MostrarMensaje("msg_sel_estado_valido", "titulo_validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbEstado.Focus();
                return;
            }

            DialogResult confirm = _idiomaBLL.MostrarMensaje(
                "msg_confirmar_modificar_turno",
                "titulo_confirmar_modificacion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                codigoTurno,
                _turnoActual.EstadoTurno_DNI101,
                nuevoEstado,
                nuevoMotivo);

            if (confirm != DialogResult.Yes) return;

            try
            {
                Cursor = Cursors.WaitCursor;

                // Paso 7: Ejecución de ModificarTurno(CodigoTurno, Motivo, Estado)
                bool exito = _turnoBLL.ModificarTurno(codigoTurno, nuevoMotivo, nuevoEstado);

                if (exito)
                {
                    // Paso 12: Mensaje de confirmación
                    _idiomaBLL.MostrarMensaje(
                        "msg_turno_modificado_ok_det",
                        "titulo_turno_modificado_ok",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        codigoTurno,
                        nuevoEstado);

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                // Flujo alternativo 7.1 o 9.1
                _idiomaBLL.MostrarMensaje("msg_error_modificar_turno", "titulo_error_modificar", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }

    /// <summary>
    /// Alias de compatibilidad fmrModificarTurno_DNI101
    /// </summary>
    public class fmrModificarTurno_DNI101 : frmModificarTurno_DNI101
    {
        public fmrModificarTurno_DNI101(TurnoBE_DNI101? turno = null) : base(turno) { }
        public fmrModificarTurno_DNI101(string codigoTurno) : base(codigoTurno) { }
    }
}
