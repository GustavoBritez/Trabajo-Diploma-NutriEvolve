using BE;
using BLL;
using Services;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class FrmReprogramarTurno_DNI101 : Form
    {
        private readonly TurnoBE_DNI101 _turno;
        private readonly TurnoBLL_DNI101 _turnoBLL = new();
        private readonly AgendaMedicaBLL_DNI101 _agendaBLL = new();
        private readonly UsuarioBLL _usuarioBLL = new();

        private class ItemProfesional
        {
            public int Dni { get; set; }
            public string NombreCompleto { get; set; } = string.Empty;

            public override string ToString() => NombreCompleto;
        }

        private class ItemBloque
        {
            public int? IdBloque { get; set; }
            public TimeSpan Hora { get; set; }
            public string HorarioTexto { get; set; } = string.Empty;

            public override string ToString() => HorarioTexto;
        }

        public FrmReprogramarTurno_DNI101(TurnoBE_DNI101 turno)
        {
            InitializeComponent();
            _turno = turno ?? throw new ArgumentNullException(nameof(turno));
        }

        private void FrmReprogramarTurno_DNI101_Load(object sender, EventArgs e)
        {
            // Mostrar los datos actuales del turno
            string nomPaciente = _turno.Paciente_DNI101 != null ? _turno.Paciente_DNI101.NombreCompleto : "Paciente #" + _turno.IdPaciente_DNI101;
            string dniPaciente = _turno.Paciente_DNI101?.DNINiño_DNI101 ?? "-";

            lblInfoTurno.Text = 
                $"• Código: {_turno.CodigoTurno_DNI101} | Estado Actual: {_turno.EstadoTurno_DNI101}\n" +
                $"• Paciente: {nomPaciente} (DNI: {dniPaciente})\n" +
                $"• Turno Asignado: {_turno.FechaTurno_DNI101:dd/MM/yyyy} a las {_turno.HoraTurno_DNI101:hh\\:mm}";

            CargarProfesionales();

            dtpNuevaFecha.MinDate = DateTime.Today;
            dtpNuevaFecha.Value = _turno.FechaTurno_DNI101 >= DateTime.Today ? _turno.FechaTurno_DNI101.AddDays(1) : DateTime.Today.AddDays(1);

            CargarBloquesDisponibles();
        }

        private void CargarProfesionales()
        {
            try
            {
                cmbProfesional.Items.Clear();
                var usuarios = _usuarioBLL.ListarUsuarios().Where(u => u._Estado).ToList();

                int indiceSeleccionado = 0;
                int i = 0;

                foreach (var u in usuarios)
                {
                    var item = new ItemProfesional
                    {
                        Dni = u._Dni,
                        NombreCompleto = $"Lic. {u._Apellido}, {u._Nombre} (DNI: {u._Dni})"
                    };
                    cmbProfesional.Items.Add(item);

                    if (u._Dni == _turno.DniNutricionista_DNI101)
                    {
                        indiceSeleccionado = i;
                    }
                    i++;
                }

                if (cmbProfesional.Items.Count > 0)
                {
                    cmbProfesional.SelectedIndex = indiceSeleccionado;
                }
            }
            catch
            {
                var usuarioActivo = ServicesSessionManager.Instancia.ObtenerUsuarioActivo();
                if (usuarioActivo != null)
                {
                    cmbProfesional.Items.Add(new ItemProfesional
                    {
                        Dni = usuarioActivo._Dni,
                        NombreCompleto = $"Lic. {usuarioActivo._Apellido}, {usuarioActivo._Nombre} (DNI: {usuarioActivo._Dni})"
                    });
                    cmbProfesional.SelectedIndex = 0;
                }
            }
        }

        private void cmbProfesional_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarBloquesDisponibles();
        }

        private void dtpNuevaFecha_ValueChanged(object sender, EventArgs e)
        {
            CargarBloquesDisponibles();
        }

        /// <summary>
        /// Paso 5: Recuperar los bloques horarios libres y cargar los horarios disponibles para la nueva fecha
        /// </summary>
        private void CargarBloquesDisponibles()
        {
            try
            {
                cmbNuevoHorario.Items.Clear();

                int dniNutricionista = 0;
                if (cmbProfesional.SelectedItem is ItemProfesional prof)
                {
                    dniNutricionista = prof.Dni;
                }

                DateTime fecha = dtpNuevaFecha.Value.Date;
                var bloques = _agendaBLL.ListarBloquesDisponibles(fecha, dniNutricionista);

                // Flujo alternativo 6.1: Sin bloques disponibles
                if (bloques == null || bloques.Count == 0)
                {
                    lblDisponibilidad.Text = "⚠ Sin bloques disponibles para el profesional en la fecha seleccionada (Flujo 6.1).";
                    lblDisponibilidad.ForeColor = Color.FromArgb(190, 60, 60);
                    cmbNuevoHorario.Enabled = false;
                    return;
                }

                cmbNuevoHorario.Enabled = true;
                foreach (var b in bloques)
                {
                    cmbNuevoHorario.Items.Add(new ItemBloque
                    {
                        IdBloque = b.IdBloque_DNI101 > 0 ? b.IdBloque_DNI101 : null,
                        Hora = b.HoraInicio_DNI101,
                        HorarioTexto = b.HoraInicio_DNI101.ToString(@"hh\:mm")
                    });
                }

                if (cmbNuevoHorario.Items.Count > 0)
                {
                    cmbNuevoHorario.SelectedIndex = 0;
                    lblDisponibilidad.Text = $"✔ {cmbNuevoHorario.Items.Count} horarios disponibles encontrados para reprogramación.";
                    lblDisponibilidad.ForeColor = Color.FromArgb(40, 120, 60);
                }
            }
            catch (Exception ex)
            {
                lblDisponibilidad.Text = $"Error al consultar disponibilidad: {ex.Message}";
                lblDisponibilidad.ForeColor = Color.FromArgb(190, 60, 60);
            }
        }

        /// <summary>
        /// Pasos 6, 7, 8, 9 y 10: Reprogramar Turno
        /// </summary>
        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            // Flujo 6.1: Validación de bloque horario seleccionado
            if (cmbNuevoHorario.SelectedItem is not ItemBloque bloqueSeleccionado)
            {
                MessageBox.Show("No hay un bloque horario disponible seleccionado para la reprogramación (Flujo 6.1).", "Horario no seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbNuevoHorario.Focus();
                return;
            }

            int dniNutri = 0;
            if (cmbProfesional.SelectedItem is ItemProfesional prof)
            {
                dniNutri = prof.Dni;
            }

            DateTime nuevaFecha = dtpNuevaFecha.Value.Date;
            TimeSpan nuevaHora = bloqueSeleccionado.Hora;
            int? nuevoIdBloque = bloqueSeleccionado.IdBloque;

            try
            {
                Cursor = Cursors.WaitCursor;

                // Paso 7, 8 y 9 ejecutados en TurnoBLL_DNI101:
                // - Valida que el estado permita modificación (Flujo 10.1: excepción si Asistió/Cancelado)
                // - Delega al estado concreto del patrón State la transición a Confirmado
                // - Libera el bloque anterior ('Disponible') y ocupa el nuevo bloque ('Ocupado')
                // - Recalcula Dígitos Verificadores (DV)
                // - Registra en bitácora de auditoría
                _turnoBLL.ReprogramarTurno(_turno.IdTurno_DNI101, nuevaFecha, nuevaHora, nuevoIdBloque, dniNutri);

                Cursor = Cursors.Default;

                // Paso 10: Mostrar mensaje de confirmación de operación exitosa
                MessageBox.Show(
                    $"¡El turno '{_turno.CodigoTurno_DNI101}' ha sido reprogramado exitosamente!\n\n" +
                    $"• Nueva Fecha: {nuevaFecha:dd/MM/yyyy}\n" +
                    $"• Nuevo Horario: {nuevaHora:hh\\:mm}\n" +
                    $"• Profesional: {cmbProfesional.SelectedItem}\n" +
                    $"• Nuevo Estado: Confirmado\n" +
                    $"• Bloque Horario Anterior: Liberado (Disponible)",
                    "CUN03 - Reprogramación Exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al reprogramar el turno:\n{ex.Message}", "Error al Reprogramar (CUN03)", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
