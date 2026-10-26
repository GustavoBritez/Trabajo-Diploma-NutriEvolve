using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using BE.PN2;
using BLL;
using BLL.PN2;
using Services;

namespace UI.PN2
{
    public partial class frmReporteInteligente : Form, IIdiomaObserver
    {
        private readonly ReporteInteligenteBLL _bll = new();
        private List<ReporteInteligenteDTO> _reportes = new();

        public frmReporteInteligente()
        {
            InitializeComponent();
            CargarReportes();
            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();
        }

        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                var idiomaBLL = new IdiomaBLL();
                TraductorUI.TraducirFormulario(this, idiomaBLL);
                if (dgvReporte.DataSource != null)
                {
                    TraductorUI.TraducirDataGridView(dgvReporte, idiomaBLL, this.Name);
                }
            }
        }

        public void CargarReportes()
        {
            try
            {
                _reportes = _bll.GenerarReporteGlobal();
                dgvReporte.DataSource = null;
                dgvReporte.DataSource = _reportes;

                if (dgvReporte.Columns["IdPaciente"] != null) dgvReporte.Columns["IdPaciente"].Visible = false;
                if (dgvReporte.Columns["TotalConsultas"] != null) dgvReporte.Columns["TotalConsultas"].HeaderText = "Controles";
                if (dgvReporte.Columns["NombreCompleto"] != null) dgvReporte.Columns["NombreCompleto"].HeaderText = "Paciente";
                if (dgvReporte.Columns["Dni"] != null) dgvReporte.Columns["Dni"].HeaderText = "DNI";
                if (dgvReporte.Columns["EdadMesesActual"] != null) dgvReporte.Columns["EdadMesesActual"].HeaderText = "Edad (m)";
                if (dgvReporte.Columns["UltimoPesoKg"] != null) dgvReporte.Columns["UltimoPesoKg"].HeaderText = "Peso Actual (kg)";
                if (dgvReporte.Columns["UltimaTallaCm"] != null) dgvReporte.Columns["UltimaTallaCm"].HeaderText = "Talla Actual (cm)";
                if (dgvReporte.Columns["UltimoZScoreIMC"] != null) dgvReporte.Columns["UltimoZScoreIMC"].HeaderText = "Z-Score IMC";
                if (dgvReporte.Columns["VelocidadPesoKgMes"] != null) dgvReporte.Columns["VelocidadPesoKgMes"].HeaderText = "Vel. Peso (kg/m)";
                if (dgvReporte.Columns["DeltaZScoreIMC"] != null) dgvReporte.Columns["DeltaZScoreIMC"].HeaderText = "Delta Z-IMC";
                if (dgvReporte.Columns["NivelRiesgo"] != null) dgvReporte.Columns["NivelRiesgo"].HeaderText = "Nivel Riesgo";
                if (dgvReporte.Columns["ClasificacionRiesgoFuturo"] != null) dgvReporte.Columns["ClasificacionRiesgoFuturo"].HeaderText = "Predicción Clínica";

                // Ocultar columnas detalladas de la grilla principal
                if (dgvReporte.Columns["Sexo"] != null) dgvReporte.Columns["Sexo"].Visible = false;
                if (dgvReporte.Columns["UltimoIMC"] != null) dgvReporte.Columns["UltimoIMC"].Visible = false;
                if (dgvReporte.Columns["UltimoPercentilIMC"] != null) dgvReporte.Columns["UltimoPercentilIMC"].Visible = false;
                if (dgvReporte.Columns["DiagnosticoOMSActual"] != null) dgvReporte.Columns["DiagnosticoOMSActual"].Visible = false;
                if (dgvReporte.Columns["VelocidadTallaCmMes"] != null) dgvReporte.Columns["VelocidadTallaCmMes"].Visible = false;
                if (dgvReporte.Columns["PesoProyectado90Dias"] != null) dgvReporte.Columns["PesoProyectado90Dias"].Visible = false;
                if (dgvReporte.Columns["TallaProyectada90Dias"] != null) dgvReporte.Columns["TallaProyectada90Dias"].Visible = false;
                if (dgvReporte.Columns["RecomendacionFrecuenciaControl"] != null) dgvReporte.Columns["RecomendacionFrecuenciaControl"].Visible = false;
                if (dgvReporte.Columns["RecomendacionDietoterapica"] != null) dgvReporte.Columns["RecomendacionDietoterapica"].Visible = false;
                if (dgvReporte.Columns["SuplementacionSugerida"] != null) dgvReporte.Columns["SuplementacionSugerida"].Visible = false;
                if (dgvReporte.Columns["PautaAlimentariaEspecial"] != null) dgvReporte.Columns["PautaAlimentariaEspecial"].Visible = false;

                if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
                {
                    TraductorUI.TraducirDataGridView(dgvReporte, new IdiomaBLL(), this.Name);
                }

                MostrarDetalleSeleccionado();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el reporte predictivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarDetalleSeleccionado()
        {
            if (dgvReporte.CurrentRow == null || dgvReporte.CurrentRow.DataBoundItem is not ReporteInteligenteDTO dto)
            {
                txtProyeccion.Text = "Seleccione un paciente de la grilla para visualizar su informe predictivo.";
                txtRecomendaciones.Text = "Seleccione un paciente para consultar las recomendaciones del motor de decisión.";
                return;
            }

            var sbProy = new StringBuilder();
            sbProy.AppendLine($"PACIENTE: {dto.NombreCompleto} (DNI: {dto.Dni})");
            sbProy.AppendLine($"Edad Actual: {dto.EdadMesesActual} meses | Controles Registrados: {dto.TotalConsultas}");
            sbProy.AppendLine($"Diagnóstico OMS Actual: {dto.DiagnosticoOMSActual}");
            sbProy.AppendLine(new string('-', 55));
            sbProy.AppendLine($"MÉTRICAS LONGITUDINALES CALCULADAS:");
            sbProy.AppendLine($" * Velocidad Ponderal: {dto.VelocidadPesoKgMes:N3} kg/mes");
            sbProy.AppendLine($" * Velocidad Talla: {dto.VelocidadTallaCmMes:N3} cm/mes");
            sbProy.AppendLine($" * Deriva Z-Score IMC (Delta): {dto.DeltaZScoreIMC:N2} DS");
            sbProy.AppendLine(new string('-', 55));
            sbProy.AppendLine($"PROYECCIÓN A 90 DÍAS (3 MESES):");
            sbProy.AppendLine($" * Peso Proyectado: {dto.PesoProyectado90Dias:N2} kg");
            sbProy.AppendLine($" * Talla Proyectada: {dto.TallaProyectada90Dias:N2} cm");
            sbProy.AppendLine($" * Nivel de Riesgo Predictivo: {dto.NivelRiesgo.ToUpper()}");
            sbProy.AppendLine($" * Clasificación: {dto.ClasificacionRiesgoFuturo}");

            txtProyeccion.Text = sbProy.ToString();

            var sbRec = new StringBuilder();
            sbRec.AppendLine($"MOTOR DE DECISIÓN NUTRICIONAL:");
            sbRec.AppendLine(new string('-', 55));
            sbRec.AppendLine($"1. FRECUENCIA DE SEGUIMIENTO SUGERIDA:");
            sbRec.AppendLine($"   {dto.RecomendacionFrecuenciaControl}");
            sbRec.AppendLine();
            sbRec.AppendLine($"2. ESTRATEGIA DIETOTERÁPICA:");
            sbRec.AppendLine($"   {dto.RecomendacionDietoterapica}");
            sbRec.AppendLine();
            sbRec.AppendLine($"3. SUPLEMENTACIÓN / FORTIFICACIÓN INDICADA:");
            sbRec.AppendLine($"   {dto.SuplementacionSugerida}");
            sbRec.AppendLine();
            sbRec.AppendLine($"4. PAUTAS VINCULANTES:");
            sbRec.AppendLine($"   {dto.PautaAlimentariaEspecial}");

            txtRecomendaciones.Text = sbRec.ToString();
        }

        private void dgvReporte_SelectionChanged(object? sender, EventArgs e)
        {
            MostrarDetalleSeleccionado();
        }

        private void dgvReporte_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvReporte.Columns[e.ColumnIndex].Name == "NivelRiesgo" && e.Value != null)
            {
                string riesgo = e.Value.ToString() ?? "";
                if (riesgo.Equals("Crítico", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 205, 210); // Rojo suave
                    e.CellStyle.ForeColor = Color.DarkRed;
                    e.CellStyle.Font = new Font(dgvReporte.Font, FontStyle.Bold);
                }
                else if (riesgo.Equals("Alto", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 236, 179); // Naranja suave
                    e.CellStyle.ForeColor = Color.FromArgb(180, 80, 0);
                    e.CellStyle.Font = new Font(dgvReporte.Font, FontStyle.Bold);
                }
                else if (riesgo.Equals("Moderado", StringComparison.OrdinalIgnoreCase))
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 249, 196); // Amarillo suave
                    e.CellStyle.ForeColor = Color.FromArgb(130, 110, 0);
                }
                else
                {
                    e.CellStyle.BackColor = Color.FromArgb(220, 245, 220); // Verde suave
                    e.CellStyle.ForeColor = Color.FromArgb(30, 90, 40);
                }
            }
        }

        private void btnActualizar_Click(object? sender, EventArgs e)
        {
            CargarReportes();
            MessageBox.Show("Métricas predictivas y proyecciones recalculadas satisfactoriamente.", "Reporte Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportar_Click(object? sender, EventArgs e)
        {
            if (_reportes.Count == 0)
            {
                MessageBox.Show("No hay datos predictivos para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "Archivo de Texto (*.txt)|*.txt",
                FileName = $"Informe_Predictivo_NutriEvolve_{DateTime.Now:yyyyMMdd_HHmm}.txt",
                Title = "Guardar Informe Predictivo"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("================================================================================");
                    sb.AppendLine("               NUTRIEVOLVE - INFORME CLÍNICO PREDICTIVO GLOBAL");
                    sb.AppendLine($"               Fecha de Emisión: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                    sb.AppendLine("================================================================================");
                    sb.AppendLine();

                    foreach (var rep in _reportes)
                    {
                        sb.AppendLine($"PACIENTE: {rep.NombreCompleto} | DNI: {rep.Dni} | Edad: {rep.EdadMesesActual} meses");
                        sb.AppendLine($"Diagnóstico Actual: {rep.DiagnosticoOMSActual} | Z-Score IMC: {rep.UltimoZScoreIMC:N2} DS");
                        sb.AppendLine($"Velocidad Ponderal: {rep.VelocidadPesoKgMes:N3} kg/mes | Deriva Z-Score: {rep.DeltaZScoreIMC:N2} DS");
                        sb.AppendLine($"Proyección a 90 días: Peso={rep.PesoProyectado90Dias:N2} kg, Talla={rep.TallaProyectada90Dias:N2} cm");
                        sb.AppendLine($"Nivel de Riesgo: {rep.NivelRiesgo.ToUpper()} - {rep.ClasificacionRiesgoFuturo}");
                        sb.AppendLine($"Conducta: {rep.RecomendacionFrecuenciaControl}");
                        sb.AppendLine($"Plan: {rep.RecomendacionDietoterapica}");
                        sb.AppendLine($"Suplemento: {rep.SuplementacionSugerida}");
                        sb.AppendLine(new string('-', 80));
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show($"Informe guardado exitosamente en:\n{sfd.FileName}", "Exportación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al exportar el informe: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCerrar_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void btnAyudaContextual_Click(object? sender, EventArgs e)
        {
            try
            {
                using var frmAyuda = new frmManualAyuda(6);
                frmAyuda.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ayuda contextual: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
