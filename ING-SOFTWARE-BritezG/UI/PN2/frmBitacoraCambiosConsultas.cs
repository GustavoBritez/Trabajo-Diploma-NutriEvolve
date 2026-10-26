using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BE.PN1;
using BE.PN2;
using BLL;
using BLL.PN2;
using Services;

namespace UI.PN2
{
    public partial class frmBitacoraCambiosConsultas : Form, IIdiomaObserver
    {
        private readonly BitacoraCambiosConsultaBLL_DNI101 _bll = new();
        private List<ConsultaCambioBE_DNI101> _listaConsultas = new();
        private List<PlanAlimentarioCambioBE_DNI101> _listaPlanes = new();
        private List<PacienteCambioBE_DNI101> _listaPacientes = new();

        public frmBitacoraCambiosConsultas()
        {
            InitializeComponent();
            cmbEntidad.SelectedIndex = 0;
            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();
        }

        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                var idiomaBLL = new IdiomaBLL();
                TraductorUI.TraducirFormulario(this, idiomaBLL);
                ActualizarEtiquetasSegunEntidad();
                if (dgvCambios.DataSource != null)
                {
                    TraductorUI.TraducirDataGridView(dgvCambios, idiomaBLL, this.Name);
                }
            }
        }

        private void cmbEntidad_SelectedIndexChanged(object? sender, EventArgs e)
        {
            txtIdConsultaFiltro.Clear();
            ActualizarEtiquetasSegunEntidad();
            CargarBitacora();
        }

        private void ActualizarEtiquetasSegunEntidad()
        {
            var idiomaBLL = new IdiomaBLL();
            string lblConsulta = idiomaBLL.ExisteTraduccion("IdConsulta") ? idiomaBLL.Traducir("IdConsulta") : "ID Consulta";
            string lblPlan = idiomaBLL.ExisteTraduccion("IdPlan") ? idiomaBLL.Traducir("IdPlan") : "ID Plan";
            string lblPac = idiomaBLL.ExisteTraduccion("IdPaciente") ? idiomaBLL.Traducir("IdPaciente") : "ID Paciente";

            var lang = ServicesSessionManager.Instancia.ObtenerIdioma()?.Codigo ?? "es";

            switch (cmbEntidad.SelectedIndex)
            {
                case 1: // Planes Alimentarios
                    lblTitulo.Text = lang == "en" ? "Change Audit Log - Meal Plans (SQL Trigger)"
                        : lang == "po" ? "Histórico de Alterações - Planos Alimentares (Trigger SQL)"
                        : "Bitácora de Cambios - Planes Alimentarios (Trigger SQL)";
                    lblConsultaFiltro.Text = $"{lblPlan}:";
                    break;
                case 2: // Pacientes
                    lblTitulo.Text = lang == "en" ? "Change Audit Log - Patients (SQL Trigger)"
                        : lang == "po" ? "Histórico de Alterações - Pacientes (Trigger SQL)"
                        : "Bitácora de Cambios - Pacientes (Trigger SQL)";
                    lblConsultaFiltro.Text = $"{lblPac}:";
                    break;
                default: // Consultas Nutricionales
                    lblTitulo.Text = lang == "en" ? "Change Audit Log - Nutritional Consultations (SQL Trigger)"
                        : lang == "po" ? "Histórico de Alterações - Consultas Nutricionais (Trigger SQL)"
                        : "Bitácora de Cambios - Consultas Nutricionales (Trigger SQL)";
                    lblConsultaFiltro.Text = $"{lblConsulta}:";
                    break;
            }
        }

        public void CargarBitacora(int? idFiltro = null)
        {
            try
            {
                dgvCambios.DataSource = null;

                switch (cmbEntidad.SelectedIndex)
                {
                    case 1: // Planes Alimentarios
                        _listaPlanes = _bll.ListarCambiosPlanes(idFiltro);
                        dgvCambios.DataSource = _listaPlanes;
                        ConfigurarGrillaPlanes();
                        break;

                    case 2: // Pacientes
                        _listaPacientes = _bll.ListarCambiosPacientes(idFiltro);
                        dgvCambios.DataSource = _listaPacientes;
                        ConfigurarGrillaPacientes();
                        break;

                    default: // Consultas Nutricionales
                        _listaConsultas = _bll.ListarCambios(idFiltro);
                        dgvCambios.DataSource = _listaConsultas;
                        ConfigurarGrillaConsultas();
                        break;
                }

                btnRollback.Enabled = dgvCambios.CurrentRow != null;
                MostrarDetalleSeleccionado();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la bitácora de cambios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGrillaConsultas()
        {
            if (dgvCambios.Columns["IdCambio"] != null) dgvCambios.Columns["IdCambio"].HeaderText = "Nro Cambio";
            if (dgvCambios.Columns["IdConsulta"] != null) dgvCambios.Columns["IdConsulta"].HeaderText = "ID Consulta";
            if (dgvCambios.Columns["IdPaciente"] != null) dgvCambios.Columns["IdPaciente"].HeaderText = "ID Paciente";
            if (dgvCambios.Columns["FechaCambio"] != null) dgvCambios.Columns["FechaCambio"].HeaderText = "Fecha Modificación";
            if (dgvCambios.Columns["UsuarioDni"] != null) dgvCambios.Columns["UsuarioDni"].HeaderText = "DNI Usuario";
            if (dgvCambios.Columns["TipoOperacion"] != null) dgvCambios.Columns["TipoOperacion"].HeaderText = "Operación";
            if (dgvCambios.Columns["Observaciones"] != null) dgvCambios.Columns["Observaciones"].HeaderText = "Observaciones Previas";

            if (dgvCambios.Columns["DniNutricionista"] != null) dgvCambios.Columns["DniNutricionista"].Visible = false;
            if (dgvCambios.Columns["FechaControl"] != null) dgvCambios.Columns["FechaControl"].Visible = false;
            if (dgvCambios.Columns["EdadMeses"] != null) dgvCambios.Columns["EdadMeses"].Visible = false;
            if (dgvCambios.Columns["TipoLactancia"] != null) dgvCambios.Columns["TipoLactancia"].Visible = false;
            if (dgvCambios.Columns["AlimentacionComplementaria"] != null) dgvCambios.Columns["AlimentacionComplementaria"].Visible = false;
            if (dgvCambios.Columns["Alergias"] != null) dgvCambios.Columns["Alergias"].Visible = false;
            if (dgvCambios.Columns["AntecedentesFamiliares"] != null) dgvCambios.Columns["AntecedentesFamiliares"].Visible = false;

            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                TraductorUI.TraducirDataGridView(dgvCambios, new IdiomaBLL(), this.Name);
            }
        }

        private void ConfigurarGrillaPlanes()
        {
            if (dgvCambios.Columns["IdCambio"] != null) dgvCambios.Columns["IdCambio"].HeaderText = "Nro Cambio";
            if (dgvCambios.Columns["IdPlan"] != null) dgvCambios.Columns["IdPlan"].HeaderText = "ID Plan";
            if (dgvCambios.Columns["IdConsulta"] != null) dgvCambios.Columns["IdConsulta"].HeaderText = "ID Consulta";
            if (dgvCambios.Columns["RequerimientoCalorico"] != null) dgvCambios.Columns["RequerimientoCalorico"].HeaderText = "Kcal Previas";
            if (dgvCambios.Columns["PctCarbohidratos"] != null) dgvCambios.Columns["PctCarbohidratos"].HeaderText = "% Carbos";
            if (dgvCambios.Columns["PctProteinas"] != null) dgvCambios.Columns["PctProteinas"].HeaderText = "% Proteínas";
            if (dgvCambios.Columns["PctGrasas"] != null) dgvCambios.Columns["PctGrasas"].HeaderText = "% Grasas";
            if (dgvCambios.Columns["FechaCambio"] != null) dgvCambios.Columns["FechaCambio"].HeaderText = "Fecha Modificación";
            if (dgvCambios.Columns["UsuarioDni"] != null) dgvCambios.Columns["UsuarioDni"].HeaderText = "DNI Usuario";
            if (dgvCambios.Columns["TipoOperacion"] != null) dgvCambios.Columns["TipoOperacion"].HeaderText = "Operación";

            if (dgvCambios.Columns["PautasFamiliares"] != null) dgvCambios.Columns["PautasFamiliares"].Visible = false;
            if (dgvCambios.Columns["MetasSalud"] != null) dgvCambios.Columns["MetasSalud"].Visible = false;

            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                TraductorUI.TraducirDataGridView(dgvCambios, new IdiomaBLL(), this.Name);
            }
        }

        private void ConfigurarGrillaPacientes()
        {
            if (dgvCambios.Columns["IdCambio"] != null) dgvCambios.Columns["IdCambio"].HeaderText = "Nro Cambio";
            if (dgvCambios.Columns["IdPaciente"] != null) dgvCambios.Columns["IdPaciente"].HeaderText = "ID Paciente";
            if (dgvCambios.Columns["Dni"] != null) dgvCambios.Columns["Dni"].HeaderText = "DNI";
            if (dgvCambios.Columns["Apellido"] != null) dgvCambios.Columns["Apellido"].HeaderText = "Apellido";
            if (dgvCambios.Columns["Nombre"] != null) dgvCambios.Columns["Nombre"].HeaderText = "Nombre";
            if (dgvCambios.Columns["Telefono"] != null) dgvCambios.Columns["Telefono"].HeaderText = "Teléfono Previo";
            if (dgvCambios.Columns["Email"] != null) dgvCambios.Columns["Email"].HeaderText = "Email Previo";
            if (dgvCambios.Columns["Direccion"] != null) dgvCambios.Columns["Direccion"].HeaderText = "Dirección Previa";
            if (dgvCambios.Columns["Activo"] != null) dgvCambios.Columns["Activo"].HeaderText = "Estado Activo";
            if (dgvCambios.Columns["FechaCambio"] != null) dgvCambios.Columns["FechaCambio"].HeaderText = "Fecha Modificación";
            if (dgvCambios.Columns["TipoOperacion"] != null) dgvCambios.Columns["TipoOperacion"].HeaderText = "Operación";

            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                TraductorUI.TraducirDataGridView(dgvCambios, new IdiomaBLL(), this.Name);
            }
        }

        private void MostrarDetalleSeleccionado()
        {
            if (dgvCambios.CurrentRow == null)
            {
                txtDetalleHistorial.Text = "Seleccione un cambio registrado en la grilla para inspeccionar su estado anterior.";
                btnRollback.Enabled = false;
                return;
            }

            var sb = new StringBuilder();

            switch (cmbEntidad.SelectedIndex)
            {
                case 1: // Planes Alimentarios
                    if (dgvCambios.CurrentRow.DataBoundItem is PlanAlimentarioCambioBE_DNI101 plan)
                    {
                        sb.AppendLine($"[ID CAMBIO: {plan.IdCambio}] - Operación detectada por Trigger: {plan.TipoOperacion}");
                        sb.AppendLine($"Fecha/Hora: {plan.FechaCambio:dd/MM/yyyy HH:mm:ss} | Usuario Responsable: {plan.UsuarioDni}");
                        sb.AppendLine($"Plan Alimentario ID: {plan.IdPlan} | Consulta Nutricional Asociada: ID {plan.IdConsulta}");
                        sb.AppendLine(new string('-', 80));
                        sb.AppendLine($"Requerimiento Calórico Previo: {plan.RequerimientoCalorico:N0} Kcal/día");
                        sb.AppendLine($"Distribución Macronutrientes: Carbohidratos {plan.PctCarbohidratos}% | Proteínas {plan.PctProteinas}% | Grasas {plan.PctGrasas}%");
                        sb.AppendLine($"Metas de Salud Previas: {plan.MetasSalud ?? "(Sin metas registradas)"}");
                        sb.AppendLine($"Pautas Familiares Previas: {plan.PautasFamiliares ?? "(Sin pautas registradas)"}");
                    }
                    break;

                case 2: // Pacientes
                    if (dgvCambios.CurrentRow.DataBoundItem is PacienteCambioBE_DNI101 pac)
                    {
                        sb.AppendLine($"[ID CAMBIO: {pac.IdCambio}] - Operación detectada por Trigger: {pac.TipoOperacion}");
                        sb.AppendLine($"Fecha/Hora: {pac.FechaCambio:dd/MM/yyyy HH:mm:ss} | Usuario Responsable: {pac.UsuarioDni}");
                        sb.AppendLine($"Paciente ID: {pac.IdPaciente} | DNI: {pac.Dni} | Nombre: {pac.Apellido}, {pac.Nombre}");
                        sb.AppendLine(new string('-', 80));
                        sb.AppendLine($"Teléfono Previo: {pac.Telefono ?? "(Sin teléfono)"}");
                        sb.AppendLine($"Email Previo: {pac.Email ?? "(Sin email)"}");
                        sb.AppendLine($"Dirección Previa: {pac.Direccion ?? "(Sin dirección)"}");
                        sb.AppendLine($"Estado Lógico Previo: {(pac.Activo ? "Activo (1)" : "Baja Lógica (0)")}");
                    }
                    break;

                default: // Consultas Nutricionales
                    if (dgvCambios.CurrentRow.DataBoundItem is ConsultaCambioBE_DNI101 cambio)
                    {
                        sb.AppendLine($"[ID CAMBIO: {cambio.IdCambio}] - Operación detectada por Trigger: {cambio.TipoOperacion}");
                        sb.AppendLine($"Fecha/Hora: {cambio.FechaCambio:dd/MM/yyyy HH:mm:ss} | Usuario Responsable: {cambio.UsuarioDni}");
                        sb.AppendLine($"Consulta Asociada: ID {cambio.IdConsulta} | Paciente: ID {cambio.IdPaciente} | Fecha Control: {cambio.FechaControl:dd/MM/yyyy}");
                        sb.AppendLine(new string('-', 80));
                        sb.AppendLine($"Tipo de Lactancia: {cambio.TipoLactancia ?? "(No especificada)"}");
                        sb.AppendLine($"Alimentación Complementaria: {cambio.AlimentacionComplementaria ?? "(No especificada)"}");
                        sb.AppendLine($"Alergias Registradas: {cambio.Alergias ?? "(Ninguna)"}");
                        sb.AppendLine($"Antecedentes Familiares: {cambio.AntecedentesFamiliares ?? "(Ninguno)"}");
                        sb.AppendLine($"Observaciones Clínicas Previas: {cambio.Observaciones ?? "(Sin observaciones)"}");
                    }
                    break;
            }

            txtDetalleHistorial.Text = sb.ToString();
            btnRollback.Enabled = true;
        }

        private void dgvCambios_SelectionChanged(object? sender, EventArgs e)
        {
            MostrarDetalleSeleccionado();
        }

        private void btnFiltrar_Click(object? sender, EventArgs e)
        {
            if (int.TryParse(txtIdConsultaFiltro.Text.Trim(), out int id) && id > 0)
            {
                CargarBitacora(id);
            }
            else
            {
                MessageBox.Show("Por favor ingrese un ID numérico válido para filtrar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnVerTodos_Click(object? sender, EventArgs e)
        {
            txtIdConsultaFiltro.Clear();
            CargarBitacora();
        }

        private void btnRollback_Click(object? sender, EventArgs e)
        {
            if (dgvCambios.CurrentRow == null)
            {
                MessageBox.Show("Seleccione el registro histórico al cual desea restaurar la entidad.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int dniUsuario = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();

            try
            {
                switch (cmbEntidad.SelectedIndex)
                {
                    case 1: // Planes Alimentarios
                        if (dgvCambios.CurrentRow.DataBoundItem is PlanAlimentarioCambioBE_DNI101 plan)
                        {
                            string msg = $"¿Desea restaurar el Plan Alimentario ID {plan.IdPlan} a la versión del {plan.FechaCambio:dd/MM/yyyy HH:mm:ss}?\n\nEl Trigger registrará esta reversión automáticamente.";
                            if (MessageBox.Show(msg, "Confirmar Rollback de Plan Alimentario", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                if (_bll.RevertirCambioPlan(plan.IdCambio, dniUsuario))
                                {
                                    MessageBox.Show($"Rollback exitoso. El Plan Alimentario ID {plan.IdPlan} fue restaurado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    CargarBitacora();
                                }
                            }
                        }
                        break;

                    case 2: // Pacientes
                        if (dgvCambios.CurrentRow.DataBoundItem is PacienteCambioBE_DNI101 pac)
                        {
                            string msg = $"¿Desea restaurar el Paciente {pac.Apellido}, {pac.Nombre} (ID {pac.IdPaciente}) a la versión del {pac.FechaCambio:dd/MM/yyyy HH:mm:ss}?\n\nEl Trigger registrará esta reversión automáticamente.";
                            if (MessageBox.Show(msg, "Confirmar Rollback de Paciente", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                if (_bll.RevertirCambioPaciente(pac.IdCambio, dniUsuario))
                                {
                                    MessageBox.Show($"Rollback exitoso. El Paciente ID {pac.IdPaciente} fue restaurado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    CargarBitacora();
                                }
                            }
                        }
                        break;

                    default: // Consultas Nutricionales
                        if (dgvCambios.CurrentRow.DataBoundItem is ConsultaCambioBE_DNI101 cambio)
                        {
                            string msg = $"¿Desea restaurar la Consulta ID {cambio.IdConsulta} a la versión del {cambio.FechaCambio:dd/MM/yyyy HH:mm:ss}?\n\nEl Trigger registrará esta reversión automáticamente.";
                            if (MessageBox.Show(msg, "Confirmar Rollback de Consulta", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                if (_bll.RevertirCambio(cambio.IdCambio, dniUsuario))
                                {
                                    MessageBox.Show($"Rollback exitoso. La Consulta ID {cambio.IdConsulta} fue restaurada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    CargarBitacora();
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error durante la operación de Rollback: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAyudaContextual_Click(object? sender, EventArgs e)
        {
            try
            {
                using var frmAyuda = new frmManualAyuda(5);
                frmAyuda.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ayuda contextual: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
