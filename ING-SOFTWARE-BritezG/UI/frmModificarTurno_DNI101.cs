using BE;
using BLL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    public partial class frmModificarTurno_DNI101 : Form
    {
        private TurnoBE_DNI101? _turnoActual;
        private readonly TurnoBLL_DNI101 _turnoBLL = new();

        public frmModificarTurno_DNI101(TurnoBE_DNI101? turno = null)
        {
            InitializeComponent();
            _turnoActual = turno;
        }

        public frmModificarTurno_DNI101(string codigoTurno)
        {
            InitializeComponent();
            txtCodigoTurno.Text = codigoTurno ?? string.Empty;
        }

        private void frmModificarTurno_DNI101_Load(object sender, EventArgs e)
        {
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
                MessageBox.Show("Por favor, ingrese un código de turno para buscar.", "Búsqueda Requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show($"No se encontró ningún turno registrado con el código '{codigo}'.\nVerifique el código e intente nuevamente (Flujo 3.1).", "Turno no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _turnoActual = null;
                    DeshabilitarEdicion();
                    return;
                }

                _turnoActual = turno;
                MostrarDatosTurno(_turnoActual);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al consultar el turno: {ex.Message}", "Error de Consulta", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // Seleccionar estado en ComboBox
            int index = cmbEstado.FindStringExact(turno.EstadoTurno_DNI101);
            if (index >= 0)
            {
                cmbEstado.SelectedIndex = index;
            }
            else
            {
                cmbEstado.Text = turno.EstadoTurno_DNI101;
            }

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
                MessageBox.Show("Por favor busque y seleccione un turno antes de intentar modificarlo.", "Turno Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string codigoTurno = _turnoActual.CodigoTurno_DNI101;
            string nuevoMotivo = txtMotivo.Text.Trim();
            string? nuevoEstado = cmbEstado.SelectedItem?.ToString();

            // Flujo alternativo 5.1: Motivo de consulta vacío
            if (string.IsNullOrWhiteSpace(nuevoMotivo))
            {
                MessageBox.Show("El motivo de consulta no puede estar vacío (Flujo 5.1).", "Validación Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMotivo.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(nuevoEstado))
            {
                MessageBox.Show("Debe seleccionar un estado válido para el turno.", "Validación Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbEstado.Focus();
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"¿Está seguro de que desea guardar las modificaciones del turno '{codigoTurno}'?\n\n" +
                $"• Estado previo: {_turnoActual.EstadoTurno_DNI101} ➔ Nuevo Estado: {nuevoEstado}\n" +
                $"• Nuevo Motivo: {nuevoMotivo}",
                "Confirmar Modificación (CUN04)",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                Cursor = Cursors.WaitCursor;

                // Paso 7: Ejecución de ModificarTurno(CodigoTurno, Motivo, Estado)
                bool exito = _turnoBLL.ModificarTurno(codigoTurno, nuevoMotivo, nuevoEstado);

                if (exito)
                {
                    // Paso 12: Mensaje de confirmación
                    MessageBox.Show(
                        $"El turno '{codigoTurno}' fue modificado exitosamente.\n" +
                        $"Estado: '{nuevoEstado}'\n" +
                        $"Dígitos Verificadores recalculados y asentado en Bitácora.",
                        "CUN04 - Modificación Exitosa",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                // Flujo alternativo 7.1 o 9.1
                MessageBox.Show($"Ocurrió un error al modificar el turno:\n{ex.Message}", "Error al Modificar (CUN04)", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
