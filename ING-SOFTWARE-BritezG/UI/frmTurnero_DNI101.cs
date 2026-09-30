using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class frmTurnero_DNI101 : Form
    {
        private readonly TurnoBLL_DNI101 _turnoBLL = new();
        private readonly PacienteBLL_DNI101 _pacienteBLL = new();

        public frmTurnero_DNI101()
        {
            InitializeComponent();
        }

        private void frmTurnero_DNI101_Load(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            cmbFiltroEstado.SelectedIndex = 0; // "Todos"
            dtpFiltroFecha.Value = DateTime.Today;
            this.Resize += (s, ev) => AjustarDisenoResponsivo();
            CargarTurnos();
            AjustarDisenoResponsivo();
        }

        private void AjustarDisenoResponsivo()
        {
            try
            {
                this.SuspendLayout();

                int containerWidth = this.ClientSize.Width;
                int containerHeight = this.ClientSize.Height;

                // 1. Panel de Botones de Acción
                if (panelBotonesAccion != null)
                {
                    panelBotonesAccion.Location = new Point(15, 70);
                    panelBotonesAccion.Size = new Size(Math.Max(320, containerWidth - 30), 70);

                    // Ajustar posición de botones dentro del panel según ancho disponible
                    var botones = new Control[] { btn_Registrar_Turno, btn_Registrar_Paciente, btn_Reprogramar_Turno, btn_Modificar_Turno, btn_Cancelar_Turno };
                    int btnWidth = Math.Min(165, Math.Max(110, (panelBotonesAccion.ClientSize.Width - 60) / 5));
                    int gap = Math.Max(5, (panelBotonesAccion.ClientSize.Width - 30 - (btnWidth * 5)) / 4);
                    int startX = 15;

                    for (int i = 0; i < botones.Length; i++)
                    {
                        if (botones[i] != null)
                        {
                            botones[i].Location = new Point(startX + i * (btnWidth + gap), 14);
                            botones[i].Size = new Size(btnWidth, 42);
                        }
                    }
                }

                // 2. Panel de Filtros
                if (panelFiltros != null)
                {
                    panelFiltros.Location = new Point(15, panelBotonesAccion != null ? panelBotonesAccion.Bottom + 10 : 150);
                    panelFiltros.Size = new Size(Math.Max(320, containerWidth - 30), 60);

                    if (btnActualizar != null)
                    {
                        btnActualizar.Location = new Point(Math.Max(300, panelFiltros.ClientSize.Width - btnActualizar.Width - 20), 12);
                    }
                }

                // 3. Panel de la Grilla (DGV)
                if (panelGrilla != null)
                {
                    int topPos = panelFiltros != null ? panelFiltros.Bottom + 10 : 220;
                    panelGrilla.Location = new Point(15, topPos);
                    panelGrilla.Size = new Size(Math.Max(320, containerWidth - 30), Math.Max(200, containerHeight - topPos - 25));

                    if (dgvTurnos != null)
                    {
                        dgvTurnos.Dock = DockStyle.Fill;
                        dgvTurnos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch
            {
                // Ignorar excepciones temporales durante renderizado
            }
            finally
            {
                this.ResumeLayout(true);
            }
        }

        #region Métodos de Negocio PN1 (Diagramas de Secuencia)

        /// <summary>
        /// 1_RegistrarTurno(DNINiño,Fecha,Horario,Motivo) - Método principal del CU01
        /// </summary>
        public void RegistrarTurno(string dniNiño, DateTime fecha, string horario, string motivo)
        {
            try
            {
                var turno = _turnoBLL.RegistrarTurno(dniNiño, fecha, horario, motivo);
                CargarTurnos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar turno: {ex.Message}", "Error PN1", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 2_RegistrarPaciente(Nombre,Apellido,DNINiño,Telefono,Email,ObraSocial)
        /// </summary>
        public void RegistrarPaciente(string nombre, string apellido, string dniNiño, string telefono, string email, string obraSocial)
        {
            try
            {
                _pacienteBLL.RegistrarPaciente(nombre, apellido, dniNiño, telefono, email, obraSocial);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar paciente: {ex.Message}", "Error PN1", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 3_ReprogramarTurno(Horario)
        /// </summary>
        public void ReprogramarTurno(string horario)
        {
            try
            {
                _turnoBLL.ReprogramarTurno(horario);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al reprogramar turno: {ex.Message}", "Error PN1", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 4_ModificarTurno(CodigoTurno,Fecha,Hora,Motivo)
        /// </summary>
        public void ModificarTurno(string codigoTurno, DateTime fecha, string hora, string motivo)
        {
            try
            {
                _turnoBLL.ModificarTurno(codigoTurno, fecha, hora, motivo);
                CargarTurnos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar turno: {ex.Message}", "Error PN1", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 4_ModificarTurno(CodigoTurno,Motivo,Estado) - CUN04
        /// </summary>
        public bool ModificarTurno(string codigoTurno, string motivo, string estado)
        {
            try
            {
                bool ok = _turnoBLL.ModificarTurno(codigoTurno, motivo, estado);
                if (ok)
                {
                    CargarTurnos();
                }
                return ok;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar turno: {ex.Message}", "Error al Modificar (CUN04)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        #endregion

        #region Eventos de Botones de Proceso de Negocio

        private void btn_Registrar_Turno_Click(object sender, EventArgs e)
        {
            // Abre la pantalla modal que no puede cerrarse hasta Cancelar o Registrar
            using (var frmRegistrar = new FrmRegistrarTurno_DNI101())
            {
                var resultado = frmRegistrar.ShowDialog(this);
                if (resultado == DialogResult.OK)
                {
                    CargarTurnos();
                }
            }
        }

        private void btn_Registrar_Paciente_Click(object sender, EventArgs e)
        {
            using (var frmPac = new RegistrarPaciente_DNI101())
            {
                frmPac.ShowDialog(this);
                CargarTurnos();
            }
        }

        private void btn_Reprogramar_Turno_Click(object sender, EventArgs e)
        {
            // Paso 2 y Flujo 3.1: Turno no seleccionado
            var turnoSeleccionado = ObtenerTurnoSeleccionado();
            if (turnoSeleccionado == null)
            {
                MessageBox.Show("Por favor, seleccione un turno de la grilla para reprogramar (Flujo 3.1).", "Turno No Seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Paso 3: El módulo recupera Turno con su Estado desde la base de datos
            var turnoCompleto = _turnoBLL.ObtenerPorId(turnoSeleccionado.IdTurno_DNI101);
            if (turnoCompleto == null)
            {
                MessageBox.Show("No se pudo recuperar la información del turno desde la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Flujo 10.1: Estado no permite modificación (Asistió/Cancelado)
            if (string.Equals(turnoCompleto.EstadoTurno_DNI101, "Asistió", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(turnoCompleto.EstadoTurno_DNI101, "Asistio", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(turnoCompleto.EstadoTurno_DNI101, "Cancelado", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show($"El turno se encuentra en estado '{turnoCompleto.EstadoTurno_DNI101}' y no permite reprogramación (Flujo 10.1).\nSolo se permite modificar turnos en estado 'Solicitado' o 'Confirmado'.", "Operación no permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Pasos 4 al 10 ejecutados mediante el formulario modal de reprogramación CUN03
            using (var frmReprogramar = new FrmReprogramarTurno_DNI101(turnoCompleto))
            {
                var resultado = frmReprogramar.ShowDialog(this);
                if (resultado == DialogResult.OK)
                {
                    // Paso 10: Refresca la grilla de turnos para reflejar los cambios
                    CargarTurnos();
                }
            }
        }

        private void btn_Modificar_Turno_Click(object sender, EventArgs e)
        {
            var turnoSeleccionado = ObtenerTurnoSeleccionado();

            // Pasos 1 al 12 de CUN04 ejecutados mediante el formulario modal frmModificarTurno_DNI101
            using (var frmModificar = new frmModificarTurno_DNI101(turnoSeleccionado))
            {
                var resultado = frmModificar.ShowDialog(this);
                if (resultado == DialogResult.OK)
                {
                    // Paso 12: Refresca la grilla de turnos para reflejar los cambios
                    CargarTurnos();
                }
            }
        }

        private void btn_Cancelar_Turno_Click(object sender, EventArgs e)
        {
            // Paso 2: Selección del turno en dgvTurnos
            var turnoSeleccionado = ObtenerTurnoSeleccionado();
            if (turnoSeleccionado == null)
            {
                // Flujo alternativo 2.1.1 / 2.1.2: Turno no seleccionado
                MessageBox.Show("Por favor, seleccione un turno de la grilla para cancelar.", "Selección Requerida (CUN05)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Flujo alternativo 6.1.1: Estado no permite cancelación
            if (string.Equals(turnoSeleccionado.EstadoTurno_DNI101, "Cancelado", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("El turno seleccionado ya se encuentra cancelado.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.Equals(turnoSeleccionado.EstadoTurno_DNI101, "Asistió", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(turnoSeleccionado.EstadoTurno_DNI101, "Asistio", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("No se puede cancelar un turno que ya fue atendido.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nomPaciente = turnoSeleccionado.Paciente_DNI101 != null ? turnoSeleccionado.Paciente_DNI101.NombreCompleto : "Paciente #" + turnoSeleccionado.IdPaciente_DNI101;
            string dniPaciente = turnoSeleccionado.Paciente_DNI101?.DNINiño_DNI101 ?? "-";

            // Paso 4: Diálogo de confirmación
            DialogResult confirm = MessageBox.Show(
                $"¿Está seguro de que desea cancelar el siguiente turno?\n\n" +
                $"• Código: {turnoSeleccionado.CodigoTurno_DNI101}\n" +
                $"• Paciente: {nomPaciente} (DNI: {dniPaciente})\n" +
                $"• Turno: {turnoSeleccionado.FechaTurno_DNI101:dd/MM/yyyy} a las {turnoSeleccionado.HoraTurno_DNI101:hh\\:mm}\n\n" +
                $"Esta acción liberará el horario en la agenda médica y recalculará los Dígitos Verificadores del sistema.",
                "Confirmación de Cancelación - CUN05",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            // Flujo alternativo 5.1.1: Cancelación no confirmada
            if (confirm != DialogResult.Yes) return;

            try
            {
                Cursor = Cursors.WaitCursor;

                // Paso 6: Ejecución del método 5_CancelarTurno(CodigoTurno)
                _turnoBLL.CancelarTurno(turnoSeleccionado.CodigoTurno_DNI101);

                // Paso 12: Refrescar la grilla para reflejar el estado 'Cancelado'
                CargarTurnos();

                // Confirmación
                MessageBox.Show(
                    $"El turno '{turnoSeleccionado.CodigoTurno_DNI101}' ha sido cancelado con éxito.\n" +
                    $"El bloque horario asignado fue liberado y se registró el evento en la Bitácora de auditoría.",
                    "CUN05 - Turno Cancelado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Flujo alternativo 9.1.1
                MessageBox.Show($"Error al cancelar el turno: {ex.Message}", "Error al Cancelar (CUN05)", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region Manejo de Grilla y Filtros

        private void chkFiltrarFecha_CheckedChanged(object sender, EventArgs e)
        {
            dtpFiltroFecha.Enabled = chkFiltrarFecha.Checked;
            CargarTurnos();
        }

        private void dtpFiltroFecha_ValueChanged(object sender, EventArgs e)
        {
            if (chkFiltrarFecha.Checked)
            {
                CargarTurnos();
            }
        }

        private void cmbFiltroEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarTurnos();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarTurnos();
        }

        public void CargarTurnos()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                DateTime? fechaFiltro = chkFiltrarFecha.Checked ? dtpFiltroFecha.Value.Date : null;
                string? estadoFiltro = cmbFiltroEstado.SelectedItem?.ToString();

                var listaTurnos = _turnoBLL.ListarTurnos(fechaFiltro, estadoFiltro);

                var datosParaGrilla = listaTurnos.Select(t => new
                {
                    t.IdTurno_DNI101,
                    t.CodigoTurno_DNI101,
                    FechaFormateada = t.FechaTurno_DNI101.ToString("dd/MM/yyyy"),
                    HoraFormateada = t.HoraTurno_DNI101.ToString(@"hh\:mm"),
                    DniNiño = t.Paciente_DNI101?.DNINiño_DNI101 ?? string.Empty,
                    NombrePaciente = t.Paciente_DNI101 != null ? t.Paciente_DNI101.NombreCompleto : "Paciente #" + t.IdPaciente_DNI101,
                    ObraSocial = t.Paciente_DNI101?.ObraSocial_DNI101 ?? "Particular",
                    t.MotivoConsulta_DNI101,
                    t.EstadoTurno_DNI101
                }).ToList();

                dgvTurnos.DataSource = null;
                dgvTurnos.AutoGenerateColumns = false;
                dgvTurnos.DataSource = datosParaGrilla;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los turnos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private TurnoBE_DNI101? ObtenerTurnoSeleccionado()
        {
            if (dgvTurnos.SelectedRows.Count == 0) return null;

            var row = dgvTurnos.SelectedRows[0];
            if (row.Cells["colId"].Value != null)
            {
                int idTurno = Convert.ToInt32(row.Cells["colId"].Value);
                return _turnoBLL.ObtenerPorId(idTurno);
            }

            return null;
        }

        #endregion
    }
}
