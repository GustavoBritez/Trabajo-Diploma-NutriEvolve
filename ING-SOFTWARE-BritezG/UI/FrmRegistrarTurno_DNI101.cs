using BE;
using BLL;
using Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class FrmRegistrarTurno_DNI101 : Form, IIdiomaObserver
    {
        private readonly TurnoBLL_DNI101 _turnoBLL = new();
        private readonly PacienteBLL_DNI101 _pacienteBLL = new();
        private readonly AgendaMedicaBLL_DNI101 _agendaBLL = new();
        private readonly UsuarioBLL _usuarioBLL = new();
        private readonly IdiomaBLL _idiomaBLL = new();
        private bool _permiteCerrar = false;

        private class ItemProfesional
        {
            public int Dni { get; set; }
            public string NombreCompleto { get; set; } = string.Empty;

            public override string ToString() => NombreCompleto;
        }

        private class ItemBloque
        {
            public int? IdBloque { get; set; }
            public string HorarioTexto { get; set; } = string.Empty;

            public override string ToString() => HorarioTexto;
        }

        public TurnoBE_DNI101? TurnoRegistrado { get; private set; }

        public FrmRegistrarTurno_DNI101()
        {
            InitializeComponent();
            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();
        }

        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                TraductorUI.TraducirFormulario(this, _idiomaBLL);
            }
        }

        private void FrmRegistrarTurno_DNI101_Load(object sender, EventArgs e)
        {
            dtpFecha.MinDate = DateTime.Today;
            dtpFecha.Value = DateTime.Today;

            // Cargar lista de profesionales
            try
            {
                cmbProfesional.Items.Clear();
                var usuarios = _usuarioBLL.ListarUsuarios().Where(u => u._Estado).ToList();
                var usuarioActivo = ServicesSessionManager.Instancia.ObtenerUsuarioActivo();

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

                    if (usuarioActivo != null && u._Dni == usuarioActivo._Dni)
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
                // Fallback si no hay lista de usuarios cargada
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

            ConsultarDisponibilidad();
        }

        #region Escenario Principal - Pasos 2, 3 y 4 (CUN-07 Consultar Disponibilidad)

        private void cmbProfesional_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConsultarDisponibilidad();
        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            ConsultarDisponibilidad();
        }

        /// <summary>
        /// Consultar Disponibilidad de horarios para la fecha y profesional seleccionados
        /// </summary>
        private void ConsultarDisponibilidad()
        {
            try
            {
                cmbHorario.Items.Clear();

                int dniNutricionista = 0;
                if (cmbProfesional.SelectedItem is ItemProfesional prof)
                {
                    dniNutricionista = prof.Dni;
                }

                DateTime fecha = dtpFecha.Value.Date;
                var bloquesDisponibles = _agendaBLL.ListarBloquesDisponibles(fecha, dniNutricionista);

                // Flujo alternativo 4.1: No existen bloques disponibles
                if (bloquesDisponibles == null || bloquesDisponibles.Count == 0)
                {
                    lblDisponibilidad.Text = fecha == DateTime.Today
                        ? _idiomaBLL.Traducir("msg_sin_horarios_hoy")
                        : _idiomaBLL.Traducir("msg_sin_horarios_fecha");
                    lblDisponibilidad.ForeColor = Color.FromArgb(190, 60, 60);
                    cmbHorario.Enabled = false;
                    btnRegistrarTurno.Enabled = false;
                    return;
                }

                cmbHorario.Enabled = true;
                btnRegistrarTurno.Enabled = true;
                foreach (var b in bloquesDisponibles)
                {
                    cmbHorario.Items.Add(new ItemBloque
                    {
                        IdBloque = b.IdBloque_DNI101 > 0 ? b.IdBloque_DNI101 : null,
                        HorarioTexto = b.HoraInicio_DNI101.ToString(@"hh\:mm")
                    });
                }

                if (cmbHorario.Items.Count > 0)
                {
                    cmbHorario.SelectedIndex = 0;
                    lblDisponibilidad.Text = $"✔ {cmbHorario.Items.Count} horarios disponibles encontrados para la fecha.";
                    lblDisponibilidad.ForeColor = Color.FromArgb(40, 120, 60);
                }
            }
            catch (Exception ex)
            {
                lblDisponibilidad.Text = $"Error al consultar disponibilidad: {ex.Message}";
                lblDisponibilidad.ForeColor = Color.FromArgb(190, 60, 60);
            }
        }

        #endregion

        #region Pasos 5 y 6 (Búsqueda o Detección de Paciente Pediátrico)

        private void txtDniNiño_Leave(object sender, EventArgs e)
        {
            string dni = txtDniNiño.Text.Trim();
            if (string.IsNullOrWhiteSpace(dni))
            {
                lblPacienteInfo.Text = "Punto de Extensión CUN-02: Se abrirá registro si no está en padrón.";
                lblPacienteInfo.ForeColor = Color.FromArgb(100, 125, 105);
                return;
            }

            try
            {
                var paciente = _pacienteBLL.ObtenerPacientePorDNI(dni);
                if (paciente != null)
                {
                    lblPacienteInfo.Text = $"✔ Paciente identificado: {paciente.NombreCompleto} | OS: {paciente.ObraSocial_DNI101}";
                    lblPacienteInfo.ForeColor = Color.FromArgb(40, 120, 60);
                }
                else
                {
                    lblPacienteInfo.Text = "ℹ DNI no registrado. Se iniciará el CUN-02 (Registrar Paciente) al presionar 'Registrar Turno'.";
                    lblPacienteInfo.ForeColor = Color.FromArgb(180, 110, 40);
                }
            }
            catch
            {
                // Silencioso en validación al abandonar campo
            }
        }

        #endregion

        #region Pasos 8 al 15 (Registro del Turno - CUN01)

        private void btnRegistrarTurno_Click(object sender, EventArgs e)
        {
            string dniNiño = txtDniNiño.Text.Trim();
            DateTime fecha = dtpFecha.Value.Date;
            string? horario = cmbHorario.SelectedItem?.ToString();
            string motivo = txtMotivo.Text.Trim();

            int? idBloqueSeleccionado = null;
            if (cmbHorario.SelectedItem is ItemBloque bloqueItem)
            {
                idBloqueSeleccionado = bloqueItem.IdBloque;
                horario = bloqueItem.HorarioTexto;
            }

            int dniNutricionista = 0;
            if (cmbProfesional.SelectedItem is ItemProfesional prof)
            {
                dniNutricionista = prof.Dni;
            }

            // Flujo Alternativo 9.1: Campos obligatorios incompletos
            if (fecha < DateTime.Today)
            {
                _idiomaBLL.MostrarMensaje("msg_fecha_invalida_pasada", "titulo_validacion_turno", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpFecha.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(dniNiño))
            {
                txtDniNiño.BackColor = Color.FromArgb(255, 235, 235);
                _idiomaBLL.MostrarMensaje("msg_dni_obligatorio", "titulo_validacion_turno", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDniNiño.Focus();
                return;
            }
            txtDniNiño.BackColor = Color.White;

            if (cmbProfesional.SelectedItem == null)
            {
                _idiomaBLL.MostrarMensaje("msg_profesional_obligatorio", "titulo_validacion_turno", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbProfesional.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(horario))
            {
                _idiomaBLL.MostrarMensaje("msg_horario_obligatorio", "titulo_validacion_turno", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbHorario.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(motivo))
            {
                txtMotivo.BackColor = Color.FromArgb(255, 235, 235);
                _idiomaBLL.MostrarMensaje("msg_motivo_obligatorio", "titulo_validacion_turno", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMotivo.Focus();
                return;
            }
            txtMotivo.BackColor = Color.White;

            try
            {
                Cursor = Cursors.WaitCursor;

                // Paso 6: Buscar al niño en la base de datos
                var paciente = _pacienteBLL.ObtenerPacientePorDNI(dniNiño);

                // Flujo alternativo 6.1: Paciente no registrado (Punto de Extensión: CUN-02)
                if (paciente == null)
                {
                    Cursor = Cursors.Default;
                    // 6.1.1 El modulo informa que el DNI no se encuentra en el padrón
                    _idiomaBLL.MostrarMensaje(
                        "msg_dni_no_encontrado_padron",
                        "titulo_ext_cun02",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        dniNiño);

                    // 6.1.2 y 6.1.3 El Nutricionista ingresa los datos y el módulo valida y registra al nuevo Paciente
                    using (var frmRegistrarPaciente = new RegistrarPaciente_DNI101(dniNiño))
                    {
                        var resPac = frmRegistrarPaciente.ShowDialog(this);
                        if (resPac != DialogResult.OK)
                        {
                            _idiomaBLL.MostrarMensaje("msg_registro_paciente_interrumpido", "titulo_cun01_interrumpido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    Cursor = Cursors.WaitCursor;
                    paciente = _pacienteBLL.ObtenerPacientePorDNI(dniNiño);
                    if (paciente == null)
                    {
                        _idiomaBLL.MostrarMensaje("msg_error_recuperar_paciente", "titulo_error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Pasos 10, 11, 12, 13 y 14 ejecutados en TurnoBLL_DNI101:
                // - Genera código único
                // - Crea turno en estado 'Solicitado'
                // - Persiste el turno en BD
                // - Actualiza el estado del bloque horario a 'Ocupado'
                // - Calcula los Dígitos Verificadores (DV)
                // - Registra en bitácora de auditoría
                var nuevoTurno = _turnoBLL.RegistrarTurno(dniNiño, fecha, horario, motivo, idBloqueSeleccionado, dniNutricionista);
                TurnoRegistrado = nuevoTurno;

                Cursor = Cursors.Default;

                // Paso 15: Muestra mensaje de éxito e informa el código de turno generado
                _idiomaBLL.MostrarMensaje(
                    "msg_turno_registrado_ok",
                    "titulo_turno_registrado_ok",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    nuevoTurno.CodigoTurno_DNI101);

                _permiteCerrar = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                _idiomaBLL.MostrarMensaje("msg_error_registrar_turno", "titulo_error_cun01", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
                ConsultarDisponibilidad();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _permiteCerrar = true;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FrmRegistrarTurno_DNI101_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_permiteCerrar && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                _idiomaBLL.MostrarMensaje(
                    "msg_pantalla_no_cerrar_directo",
                    "titulo_accion_requerida",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        #endregion
    }
}
