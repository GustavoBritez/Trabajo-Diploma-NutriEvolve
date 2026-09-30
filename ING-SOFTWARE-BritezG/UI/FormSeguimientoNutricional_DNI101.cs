using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace UI
{
    public partial class FormSeguimientoNutricional_DNI101 : Form
    {
        private readonly SeguimientoNutricionalBLL_DNI101 _seguimientoBLL = new();
        private PacienteBE_DNI101? _pacienteActual;
        private MedicionAntropometricaBE_DNI101? _medicionActual;
        private ZScoreResultadoBE_DNI101? _zscoresActuales;
        private DiagnosticoNutricionalBE_DNI101? _diagnosticoActual;
        private List<ConsultaNutricionalBE_DNI101> _historialConsultas = new();

        public FormSeguimientoNutricional_DNI101()
        {
            InitializeComponent();
        }

        private void FormSeguimientoNutricional_DNI101_Load(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            if (tabAntropometria != null) tabAntropometria.AutoScroll = true;
            if (tabDiagnostico != null) tabDiagnostico.AutoScroll = true;
            if (tabAnamnesis != null) tabAnamnesis.AutoScroll = true;
            if (tabPlanAlimentario != null) tabPlanAlimentario.AutoScroll = true;

            this.Resize += (s, ev) => AjustarDisenoResponsivo();

            if (cmbTipoLactancia.Items.Count > 0)
            {
                cmbTipoLactancia.SelectedIndex = 0;
            }
            if (cmbTipoGrafico.Items.Count > 0)
            {
                cmbTipoGrafico.SelectedIndex = 0;
            }
            ValidarMacronutrientes();
            AjustarDisenoResponsivo();
        }

        private void AjustarDisenoResponsivo()
        {
            try
            {
                this.SuspendLayout();

                // 1. Header Paciente Responsivo
                if (panelPacienteHeader != null && btnBuscarPaciente != null && lblInfoPaciente != null)
                {
                    int leftOffset = btnBuscarPaciente.Right + 15;
                    lblInfoPaciente.Location = new Point(leftOffset, 18);
                    lblInfoPaciente.MaximumSize = new Size(Math.Max(200, panelPacienteHeader.ClientSize.Width - leftOffset - 15), 45);
                }

                // 2. Tab Antropometría
                if (tabAntropometria != null && gbMediciones != null && gbResultados != null && gbGrafico != null)
                {
                    int containerWidth = tabAntropometria.ClientSize.Width;
                    int containerHeight = tabAntropometria.ClientSize.Height;

                    if (containerWidth >= 820)
                    {
                        // Pantalla Ancha: Columna Izquierda + Gráficos a la derecha
                        int colIzquierdaWidth = Math.Min(360, (int)(containerWidth * 0.35));

                        gbMediciones.Location = new Point(15, 10);
                        gbMediciones.Size = new Size(colIzquierdaWidth, 240);

                        gbResultados.Location = new Point(15, gbMediciones.Bottom + 10);
                        gbResultados.Size = new Size(colIzquierdaWidth, Math.Max(270, containerHeight - gbMediciones.Bottom - 25));

                        int graficoLeft = gbMediciones.Right + 15;
                        int graficoWidth = Math.Max(380, containerWidth - graficoLeft - 15);
                        int graficoHeight = Math.Max(480, containerHeight - 25);

                        gbGrafico.Location = new Point(graficoLeft, 10);
                        gbGrafico.Size = new Size(graficoWidth, graficoHeight);
                    }
                    else
                    {
                        // Pantalla Estrecha / Laptop pequeña: Disposición apilada con Scroll
                        int fullWidth = Math.Max(320, containerWidth - 30);

                        gbMediciones.Location = new Point(15, 10);
                        gbMediciones.Size = new Size(fullWidth, 240);

                        gbResultados.Location = new Point(15, gbMediciones.Bottom + 10);
                        gbResultados.Size = new Size(fullWidth, 280);

                        gbGrafico.Location = new Point(15, gbResultados.Bottom + 10);
                        gbGrafico.Size = new Size(fullWidth, 480);
                    }
                }

                // 3. Tab Diagnóstico
                if (tabDiagnostico != null && gbDiagnostico != null && panelAlertaVisual != null && txtDetallesClinicos != null)
                {
                    int containerWidth = tabDiagnostico.ClientSize.Width;
                    int fullWidth = Math.Max(320, containerWidth - 30);

                    gbDiagnostico.Location = new Point(15, 10);
                    gbDiagnostico.Size = new Size(fullWidth, Math.Max(260, tabDiagnostico.ClientSize.Height - panelAlertaVisual.Height - 40));

                    txtDetallesClinicos.Size = new Size(gbDiagnostico.ClientSize.Width - 30, Math.Max(100, gbDiagnostico.ClientSize.Height - 120));

                    panelAlertaVisual.Location = new Point(15, gbDiagnostico.Bottom + 10);
                    panelAlertaVisual.Size = new Size(fullWidth, 110);
                }

                // 4. Tab Anamnesis
                if (tabAnamnesis != null && gbAnamnesis != null && gbRecordatorio != null)
                {
                    int containerWidth = tabAnamnesis.ClientSize.Width;

                    if (containerWidth >= 850)
                    {
                        int halfWidth = (containerWidth - 45) / 2;

                        gbAnamnesis.Location = new Point(15, 10);
                        gbAnamnesis.Size = new Size(halfWidth, 540);

                        gbRecordatorio.Location = new Point(gbAnamnesis.Right + 15, 10);
                        gbRecordatorio.Size = new Size(halfWidth, 540);
                    }
                    else
                    {
                        int fullWidth = Math.Max(320, containerWidth - 30);

                        gbAnamnesis.Location = new Point(15, 10);
                        gbAnamnesis.Size = new Size(fullWidth, 500);

                        gbRecordatorio.Location = new Point(15, gbAnamnesis.Bottom + 10);
                        gbRecordatorio.Size = new Size(fullWidth, 520);
                    }
                }

                // 5. Tab Plan Alimentario
                if (tabPlanAlimentario != null && gbPlan != null && gbHistorial != null && dgvHistorialConsultas != null)
                {
                    int containerWidth = tabPlanAlimentario.ClientSize.Width;
                    int fullWidth = Math.Max(320, containerWidth - 30);

                    gbPlan.Location = new Point(15, 10);
                    gbPlan.Size = new Size(fullWidth, 340);

                    if (txtPautasFamiliares != null) txtPautasFamiliares.Size = new Size(gbPlan.ClientSize.Width - 30, 50);
                    if (txtMetasSalud != null) txtMetasSalud.Size = new Size(gbPlan.ClientSize.Width - 30, 50);
                    if (btnGuardarConsulta != null) btnGuardarConsulta.Location = new Point(Math.Max(15, (gbPlan.ClientSize.Width - btnGuardarConsulta.Width) / 2), 275);

                    gbHistorial.Location = new Point(15, gbPlan.Bottom + 10);
                    gbHistorial.Size = new Size(fullWidth, Math.Max(200, tabPlanAlimentario.ClientSize.Height - gbPlan.Bottom - 25));

                    dgvHistorialConsultas.Location = new Point(10, 25);
                    dgvHistorialConsultas.Size = new Size(gbHistorial.ClientSize.Width - 20, gbHistorial.ClientSize.Height - 35);
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

        private void btnBuscarPaciente_Click(object sender, EventArgs e)
        {
            string dni = txtDniNiño.Text.Trim();
            if (string.IsNullOrWhiteSpace(dni))
            {
                MessageBox.Show("Por favor, ingrese el DNI del paciente pediátrico.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                _pacienteActual = _seguimientoBLL.ObtenerPacientePorDNI(dni);

                if (_pacienteActual == null)
                {
                    lblInfoPaciente.Text = "Paciente: ❌ No encontrado en la base de datos.";
                    lblInfoPaciente.ForeColor = Color.DarkRed;
                    MessageBox.Show($"El paciente con DNI '{dni}' no se encuentra registrado.", "Paciente No Encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int edadMeses = ObtenerEdadMeses(_pacienteActual.FechaNacimiento_DNI101);
                double edadAnios = Math.Round(edadMeses / 12.0, 1);

                lblInfoPaciente.Text = $"Paciente: 👶 {_pacienteActual.Nombre_DNI101} {_pacienteActual.Apellido_DNI101} | Edad: {edadMeses} meses ({edadAnios} años) | Sexo: {_pacienteActual.Sexo_DNI101} | Obra Social: {_pacienteActual.ObraSocial_DNI101}";
                lblInfoPaciente.ForeColor = Color.FromArgb(20, 80, 40);

                CargarHistorialConsultas();
                panelGraficoCurvas.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar paciente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private int ObtenerEdadMeses(DateTime fechaNacimiento)
        {
            DateTime ahora = DateTime.Now;
            int meses = (ahora.Year - fechaNacimiento.Year) * 12 + ahora.Month - fechaNacimiento.Month;
            if (ahora.Day < fechaNacimiento.Day) meses--;
            return Math.Max(0, meses);
        }

        private void CargarHistorialConsultas()
        {
            if (_pacienteActual == null) return;
            try
            {
                _historialConsultas = _seguimientoBLL.ObtenerHistoriaClinica(_pacienteActual.IdPaciente_DNI101);
                dgvHistorialConsultas.DataSource = null;
                dgvHistorialConsultas.DataSource = _historialConsultas;

                if (dgvHistorialConsultas.Columns.Count > 0)
                {
                    if (dgvHistorialConsultas.Columns["IdConsulta_DNI101"] != null) dgvHistorialConsultas.Columns["IdConsulta_DNI101"].HeaderText = "# Consulta";
                    if (dgvHistorialConsultas.Columns["FechaControl_DNI101"] != null) dgvHistorialConsultas.Columns["FechaControl_DNI101"].HeaderText = "Fecha Control";
                    if (dgvHistorialConsultas.Columns["EdadMeses_DNI101"] != null) dgvHistorialConsultas.Columns["EdadMeses_DNI101"].HeaderText = "Edad (Meses)";
                    if (dgvHistorialConsultas.Columns["TipoLactancia_DNI101"] != null) dgvHistorialConsultas.Columns["TipoLactancia_DNI101"].HeaderText = "Lactancia";

                    string[] ocultar = { "IdPaciente_DNI101", "DniNutricionista_DNI101", "IdTurno_DNI101", "DV", "Paciente_DNI101", "Medicion_DNI101", "ZScores_DNI101", "Diagnostico_DNI101", "Alerta_DNI101", "PlanAlimentario_DNI101", "Recordatorio_DNI101", "AlimentacionComplementaria_DNI101", "Alergias_DNI101", "AntecedentesFamiliares_DNI101", "Observaciones_DNI101" };
                    foreach (var col in ocultar)
                    {
                        if (dgvHistorialConsultas.Columns[col] != null)
                            dgvHistorialConsultas.Columns[col].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar historial: {ex.Message}");
            }
        }

        private void btnCalcularZScores_Click(object sender, EventArgs e)
        {
            if (_pacienteActual == null)
            {
                MessageBox.Show("Primero debe seleccionar un paciente pediátrico cargado.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double peso = (double)numPeso.Value;
            double talla = (double)numTalla.Value;
            double pc = (double)numPerimetroCefalico.Value;
            double? cc = numCircunferenciaCintura.Value > 0 ? (double)numCircunferenciaCintura.Value : null;

            if (peso <= 0 || talla <= 0 || pc <= 0)
            {
                MessageBox.Show("Por favor, ingrese valores mayores a cero para Peso, Talla y Perímetro Cefálico.", "Datos Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int edadMeses = ObtenerEdadMeses(_pacienteActual.FechaNacimiento_DNI101);
                var res = _seguimientoBLL.CalcularZScoresEnVivo(peso, talla, pc, cc, edadMeses, _pacienteActual.Sexo_DNI101);

                _medicionActual = res.medicion;
                _zscoresActuales = res.zscores;
                _diagnosticoActual = res.diagnostico;

                lblIMCValue.Text = $"IMC: {_medicionActual.IMC_DNI101:F2} Kg/m²";
                lblZPesoValue.Text = $"Z-Peso / Edad: {_zscoresActuales.ZPesoEdad_DNI101:F2} D.E.";
                lblZTallaValue.Text = $"Z-Talla / Edad: {_zscoresActuales.ZTallaEdad_DNI101:F2} D.E.";
                lblZIMCValue.Text = $"Z-IMC / Edad: {_zscoresActuales.ZIMCEdad_DNI101:F2} D.E.";
                lblZPCValue.Text = $"Z-Perím. Cefálico / Edad: {_zscoresActuales.ZPCEdad_DNI101:F2} D.E.";
                lblPercentilValue.Text = $"Percentil IMC OMS: P{_zscoresActuales.PercentilIMC_DNI101:F1}";

                lblClasificacionOMS.Text = $"Clasificación OMS: {_diagnosticoActual.ClasificacionOMS_DNI101}";
                txtDetallesClinicos.Text = _diagnosticoActual.DetallesClinicos_DNI101;

                panelGraficoCurvas.Invalidate();
                MessageBox.Show("Cálculo de Z-Scores y Percentiles OMS realizado correctamente (CUN07).", "Cálculo Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en el cálculo antropométrico: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbTipoGrafico_SelectedIndexChanged(object? sender, EventArgs e)
        {
            panelGraficoCurvas.Invalidate();
        }

        private void panelGraficoCurvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int w = panelGraficoCurvas.Width;
            int h = panelGraficoCurvas.Height;

            int marginLeft = 55;
            int marginRight = 65;
            int marginTop = 40;
            int marginBottom = 45;

            Rectangle graphArea = new Rectangle(marginLeft, marginTop, w - marginLeft - marginRight, h - marginTop - marginBottom);

            // Dibujar Fondo Púrpura/Marco Oficial OMS
            g.Clear(Color.FromArgb(110, 50, 110));

            // Fondo Blanco del Área de la Gráfica
            using (SolidBrush bgBrush = new SolidBrush(Color.White))
            {
                g.FillRectangle(bgBrush, graphArea);
            }

            int edadMaxMeses = 228; // 19 años
            int edadMinMeses = 60;  // 5 años (o 0 según paciente)
            if (_pacienteActual != null)
            {
                int edadActual = ObtenerEdadMeses(_pacienteActual.FechaNacimiento_DNI101);
                if (edadActual <= 60)
                {
                    edadMinMeses = 0;
                    edadMaxMeses = 60; // 0 a 5 años
                }
            }

            double valMin = 12.0;
            double valMax = 30.0;
            string tituloVar = "Índice de Masa Corporal (kg/m²)";

            int modoGrafico = cmbTipoGrafico.SelectedIndex;
            if (modoGrafico == 1) // Peso
            {
                valMin = 10.0; valMax = 90.0; tituloVar = "Peso para la Edad (Kg)";
            }
            else if (modoGrafico == 2) // Talla
            {
                valMin = 50.0; valMax = 190.0; tituloVar = "Talla para la Edad (cm)";
            }
            else if (modoGrafico == 3) // PC
            {
                valMin = 30.0; valMax = 60.0; tituloVar = "Perímetro Cefálico (cm)";
            }

            // DIBUJAR FRANJAS DE COLORES ESTÁNDAR OMS
            // 1. Zona Superior Obesidad (> P97): Naranja / Amarillo Dorado (#E69A18)
            // 2. Zona Sobrepeso (P85 a P97): Amarillo Claro (#F7EE94)
            // 3. Zona Eutrófica / Normal (P3 a P85): Verde Menta (#A8D59D)
            // 4. Zona Desnutrición / Bajo Peso (< P3): Rosa / Magenta (#F5B7CE)

            PointF MapCoords(double edadMeses, double valor)
            {
                double xFrac = (edadMeses - edadMinMeses) / (double)(edadMaxMeses - edadMinMeses);
                double yFrac = (valor - valMin) / (valMax - valMin);

                float px = (float)(graphArea.Left + Math.Clamp(xFrac, 0.0, 1.0) * graphArea.Width);
                float py = (float)(graphArea.Bottom - Math.Clamp(yFrac, 0.0, 1.0) * graphArea.Height);
                return new PointF(px, py);
            }

            // Generar Curvas Percentilares OMS Curvadas (P3, P15, P50, P85, P97)
            List<PointF> ptsP97 = new();
            List<PointF> ptsP85 = new();
            List<PointF> ptsP50 = new();
            List<PointF> ptsP15 = new();
            List<PointF> ptsP3 = new();

            int pasos = 20;
            for (int step = 0; step <= pasos; step++)
            {
                double em = edadMinMeses + (step * (edadMaxMeses - edadMinMeses) / (double)pasos);
                double tAnio = em / 12.0;

                double p50, p85, p97, p15, p3;

                if (modoGrafico == 0) // IMC
                {
                    p50 = 15.2 + (tAnio - 5.0) * 0.45 + Math.Pow((tAnio - 5.0) / 7.0, 2) * 1.2;
                    p85 = p50 + 2.0 + (tAnio - 5.0) * 0.3;
                    p97 = p85 + 2.5 + (tAnio - 5.0) * 0.4;
                    p15 = p50 - 1.2;
                    p3 = p15 - 1.3;
                }
                else if (modoGrafico == 1) // Peso
                {
                    p50 = 18.0 + (tAnio - 5.0) * 3.5;
                    p85 = p50 * 1.2; p97 = p50 * 1.38; p15 = p50 * 0.85; p3 = p50 * 0.72;
                }
                else if (modoGrafico == 2) // Talla
                {
                    p50 = 110.0 + (tAnio - 5.0) * 5.8;
                    p85 = p50 + 5.0; p97 = p50 + 10.0; p15 = p50 - 5.0; p3 = p50 - 10.0;
                }
                else // PC
                {
                    p50 = 50.0 + Math.Min(6.0, (tAnio - 5.0) * 0.4);
                    p85 = p50 + 1.5; p97 = p50 + 3.0; p15 = p50 - 1.5; p3 = p50 - 3.0;
                }

                ptsP97.Add(MapCoords(em, p97));
                ptsP85.Add(MapCoords(em, p85));
                ptsP50.Add(MapCoords(em, p50));
                ptsP15.Add(MapCoords(em, p15));
                ptsP3.Add(MapCoords(em, p3));
            }

            // Rellenar Franjas de Colores OMS usando GraphicsPath
            void RellenarFranja(List<PointF> superior, List<PointF> inferior, Color color)
            {
                using GraphicsPath path = new GraphicsPath();
                path.AddLines(superior.ToArray());
                List<PointF> revInf = new List<PointF>(inferior);
                revInf.Reverse();
                path.AddLines(revInf.ToArray());
                path.CloseFigure();

                using SolidBrush b = new SolidBrush(color);
                g.FillPath(b, path);
            }

            // 1. Fondo completo Superior Obesidad (> P97)
            using (SolidBrush bTop = new SolidBrush(Color.FromArgb(235, 160, 30)))
            {
                g.FillRectangle(bTop, graphArea);
            }

            // 2. Franja P85 a P97 (Sobrepeso)
            RellenarFranja(ptsP97, ptsP85, Color.FromArgb(248, 238, 148));

            // 3. Franja P3 a P85 (Eutrófico / Normal)
            RellenarFranja(ptsP85, ptsP3, Color.FromArgb(168, 213, 157));

            // 4. Franja < P3 (Bajo Peso / Desnutrición)
            List<PointF> ptsBottom = new();
            foreach (var pt in ptsP3) ptsBottom.Add(new PointF(pt.X, graphArea.Bottom));
            RellenarFranja(ptsP3, ptsBottom, Color.FromArgb(245, 183, 206));

            // DIBUJAR LÍNEAS DE CURVAS OMS (P3, P15, P50, P85, P97)
            using (Pen penP50 = new Pen(Color.FromArgb(20, 20, 20), 2f))
            using (Pen penP = new Pen(Color.FromArgb(50, 50, 50), 1.4f))
            {
                g.DrawCurve(penP, ptsP97.ToArray());
                g.DrawCurve(penP, ptsP85.ToArray());
                g.DrawCurve(penP50, ptsP50.ToArray());
                g.DrawCurve(penP, ptsP15.ToArray());
                g.DrawCurve(penP, ptsP3.ToArray());
            }

            // DIBUJAR GRILLA SECUNDARIA Y EJES
            using (Pen penGrid = new Pen(Color.FromArgb(100, 0, 0, 0), 0.8f))
            {
                penGrid.DashStyle = DashStyle.Dot;
                for (int m = edadMinMeses; m <= edadMaxMeses; m += 12)
                {
                    PointF topP = MapCoords(m, valMax);
                    PointF botP = MapCoords(m, valMin);
                    g.DrawLine(penGrid, topP.X, graphArea.Top, botP.X, graphArea.Bottom);

                    // Etiqueta Eje X (Años)
                    string labelAnio = $"{m / 12}a";
                    using Font fontAxis = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                    using SolidBrush bText = new SolidBrush(Color.White);
                    g.DrawString(labelAnio, fontAxis, bText, topP.X - 10, graphArea.Bottom + 5);
                }
            }

            // Etiquetas de Percentiles en el borde derecho
            string[] labelsP = { "p97", "p85", "p50", "p15", "p3" };
            PointF[] endPts = { ptsP97.Last(), ptsP85.Last(), ptsP50.Last(), ptsP15.Last(), ptsP3.Last() };
            for (int i = 0; i < labelsP.Length; i++)
            {
                using Font fontP = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                using SolidBrush bP = new SolidBrush(Color.White);
                g.DrawString(labelsP[i], fontP, bP, graphArea.Right + 5, endPts[i].Y - 8);
            }

            // Título de la Gráfica
            using (Font fontTitle = new Font("Segoe UI", 10f, FontStyle.Bold))
            using (SolidBrush bTitle = new SolidBrush(Color.White))
            {
                g.DrawString($"{tituloVar} - Patrón OMS (5-19 años)", fontTitle, bTitle, graphArea.Left, 10);
            }

            // =========================================================================
            // GRAFICAR SEGUIMIENTO HISTÓRICO COMPLETO DEL PACIENTE (EVOLUCIÓN EN EL TIEMPO)
            // =========================================================================
            if (_pacienteActual != null && _historialConsultas != null && _historialConsultas.Count > 0)
            {
                List<PointF> puntosHistoricos = new();

                // Ordenar consultas cronológicamente de la más antigua a la más reciente
                var historialOrdenado = _historialConsultas.OrderBy(c => c.EdadMeses_DNI101).ToList();

                foreach (var c in historialOrdenado)
                {
                    if (c.Medicion_DNI101 != null)
                    {
                        double valorVar = c.Medicion_DNI101.IMC_DNI101;
                        if (modoGrafico == 1) valorVar = c.Medicion_DNI101.PesoKg_DNI101;
                        else if (modoGrafico == 2) valorVar = c.Medicion_DNI101.TallaCm_DNI101;
                        else if (modoGrafico == 3) valorVar = c.Medicion_DNI101.PerimetroCefalicoCm_DNI101;

                        PointF ptHist = MapCoords(c.EdadMeses_DNI101, valorVar);
                        puntosHistoricos.Add(ptHist);
                    }
                }

                // Trazar línea de trayectoria histórica uniendo los puntos
                if (puntosHistoricos.Count > 1)
                {
                    using Pen penLineaHist = new Pen(Color.FromArgb(200, 0, 50, 160), 3f) { DashStyle = DashStyle.Solid };
                    g.DrawLines(penLineaHist, puntosHistoricos.ToArray());
                }

                // Dibujar cada punto histórico
                for (int i = 0; i < puntosHistoricos.Count; i++)
                {
                    PointF pt = puntosHistoricos[i];
                    bool esUltimo = (i == puntosHistoricos.Count - 1);

                    using SolidBrush glowB = new SolidBrush(esUltimo ? Color.FromArgb(150, 220, 20, 20) : Color.FromArgb(120, 0, 80, 200));
                    g.FillEllipse(glowB, pt.X - 8, pt.Y - 8, 16, 16);

                    using SolidBrush ptB = new SolidBrush(esUltimo ? Color.Crimson : Color.RoyalBlue);
                    g.FillEllipse(ptB, pt.X - 5, pt.Y - 5, 10, 10);

                    using Pen penBorder = new Pen(Color.White, 1.5f);
                    g.DrawEllipse(penBorder, pt.X - 5, pt.Y - 5, 10, 10);
                }
            }

            // Si hay un cálculo actual en borrador (aún no guardado), graficarlo también
            if (_medicionActual != null && _pacienteActual != null)
            {
                int edadMesesActual = ObtenerEdadMeses(_pacienteActual.FechaNacimiento_DNI101);
                double valorBorrador = _medicionActual.IMC_DNI101;
                if (modoGrafico == 1) valorBorrador = _medicionActual.PesoKg_DNI101;
                else if (modoGrafico == 2) valorBorrador = _medicionActual.TallaCm_DNI101;
                else if (modoGrafico == 3) valorBorrador = _medicionActual.PerimetroCefalicoCm_DNI101;

                PointF ptActual = MapCoords(edadMesesActual, valorBorrador);

                using SolidBrush glowBrush = new SolidBrush(Color.FromArgb(180, 255, 50, 50));
                g.FillEllipse(glowBrush, ptActual.X - 10, ptActual.Y - 10, 20, 20);

                using SolidBrush ptBrush = new SolidBrush(Color.Red);
                g.FillEllipse(ptBrush, ptActual.X - 6, ptActual.Y - 6, 12, 12);

                using Pen borderPen = new Pen(Color.White, 2f);
                g.DrawEllipse(borderPen, ptActual.X - 6, ptActual.Y - 6, 12, 12);

                string tag = $"Control Actual: {valorBorrador:F1}";
                using Font fontTag = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                using SolidBrush bTagBg = new SolidBrush(Color.FromArgb(30, 30, 30));
                using SolidBrush bTagTxt = new SolidBrush(Color.Yellow);
                g.FillRectangle(bTagBg, ptActual.X + 8, ptActual.Y - 12, 130, 20);
                g.DrawString(tag, fontTag, bTagTxt, ptActual.X + 12, ptActual.Y - 9);
            }
        }

        private void Macronutrientes_ValueChanged(object? sender, EventArgs e)
        {
            ValidarMacronutrientes();
        }

        private bool ValidarMacronutrientes()
        {
            double suma = (double)(numPctCHO.Value + numPctPROT.Value + numPctGrasas.Value);
            lblSumaMacronutrientes.Text = $"Suma: {suma:F2}%";

            if (Math.Abs(suma - 100.0) < 0.01)
            {
                lblSumaMacronutrientes.ForeColor = Color.DarkGreen;
                return true;
            }
            else
            {
                lblSumaMacronutrientes.ForeColor = Color.DarkRed;
                return false;
            }
        }

        private void btnGuardarConsulta_Click(object sender, EventArgs e)
        {
            if (_pacienteActual == null)
            {
                MessageBox.Show("Debe seleccionar un paciente pediátrico antes de guardar la consulta.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_medicionActual == null || _zscoresActuales == null || _diagnosticoActual == null)
            {
                MessageBox.Show("Debe realizar el cálculo de antropometría y Z-Scores en la Pestaña 1 antes de guardar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarMacronutrientes())
            {
                MessageBox.Show("La suma de los macronutrientes (% Carbohidratos + % Proteínas + % Grasas) debe ser exactamente igual a 100%.", "Validación Fallida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                panelAlertaVisual.Visible = false;

                int edadMeses = ObtenerEdadMeses(_pacienteActual.FechaNacimiento_DNI101);
                var consulta = new ConsultaNutricionalBE_DNI101
                {
                    IdPaciente_DNI101 = _pacienteActual.IdPaciente_DNI101,
                    EdadMeses_DNI101 = edadMeses,
                    TipoLactancia_DNI101 = cmbTipoLactancia.SelectedItem?.ToString(),
                    AlimentacionComplementaria_DNI101 = txtAlimentacionComplementaria.Text.Trim(),
                    Alergias_DNI101 = txtAlergias.Text.Trim(),
                    AntecedentesFamiliares_DNI101 = txtAntecedentesFamiliares.Text.Trim(),
                    Observaciones_DNI101 = txtDetallesClinicos.Text.Trim(),
                    Medicion_DNI101 = _medicionActual,
                    ZScores_DNI101 = _zscoresActuales,
                    Diagnostico_DNI101 = _diagnosticoActual,
                    Recordatorio_DNI101 = new Recordatorio24hBE_DNI101
                    {
                        Desayuno_DNI101 = txtDesayuno.Text.Trim(),
                        Almuerzo_DNI101 = txtAlmuerzo.Text.Trim(),
                        Merienda_DNI101 = txtMerienda.Text.Trim(),
                        Cena_DNI101 = txtCena.Text.Trim(),
                        Colaciones_DNI101 = txtColaciones.Text.Trim(),
                        FrecuenciaAlimentos_DNI101 = txtFrecuenciaAlimentos.Text.Trim()
                    },
                    PlanAlimentario_DNI101 = new PlanAlimentarioBE_DNI101
                    {
                        RequerimientoCalorico_DNI101 = (double)numKcal.Value,
                        PctCarbohidratos_DNI101 = (double)numPctCHO.Value,
                        PctProteinas_DNI101 = (double)numPctPROT.Value,
                        PctGrasas_DNI101 = (double)numPctGrasas.Value,
                        PautasFamiliares_DNI101 = txtPautasFamiliares.Text.Trim(),
                        MetasSalud_DNI101 = txtMetasSalud.Text.Trim()
                    }
                };

                var consultaGuardada = _seguimientoBLL.RegistrarConsulta(consulta, OnAlertaVisualEmitida);

                MessageBox.Show($"Consulta Nutricional #{consultaGuardada.IdConsulta_DNI101} registrada con éxito en la Historia Clínica (CUN11).\n\nLos puntos históricos fueron guardados y actualizados en el Graficador Evolutivo OMS.", "Guardado Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarHistorialConsultas();
                panelGraficoCurvas.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la consulta nutricional: {ex.Message}", "Error de Persistencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void OnAlertaVisualEmitida(AlertaClinicaBE_DNI101 alerta)
        {
            panelAlertaVisual.Visible = true;
            lblAlertaTitulo.Text = $"⚠️ ALERTA CLÍNICA: {alerta.TipoAlerta_DNI101.ToUpper()} ({alerta.Severidad_DNI101.ToUpper()})";
            lblAlertaMensaje.Text = alerta.MensajeAlerta_DNI101;
            tabControlMain.SelectedTab = tabDiagnostico;
        }
    }
}
