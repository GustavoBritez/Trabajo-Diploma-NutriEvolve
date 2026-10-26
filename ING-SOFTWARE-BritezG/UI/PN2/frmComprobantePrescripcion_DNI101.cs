using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using BE;
using BE.PN2;
using BLL;
using Services;

namespace UI.PN2
{
    public partial class frmComprobantePrescripcion_DNI101 : Form, IIdiomaObserver
    {
        private readonly PacienteBE_DNI101 _paciente;
        private readonly ConsultaNutricionalBE_DNI101? _consulta;
        private readonly PlanAlimentarioBE_DNI101? _plan;
        private readonly string _diagnosticoClinico;
        private readonly PrintDocument _printDoc = new();

        public frmComprobantePrescripcion_DNI101(
            PacienteBE_DNI101 paciente,
            ConsultaNutricionalBE_DNI101? consulta = null,
            PlanAlimentarioBE_DNI101? plan = null,
            string? diagnosticoClinico = null)
        {
            _paciente = paciente ?? throw new ArgumentNullException(nameof(paciente));
            _consulta = consulta;
            _plan = plan;
            _diagnosticoClinico = !string.IsNullOrWhiteSpace(diagnosticoClinico)
                ? diagnosticoClinico
                : (_consulta?.Diagnostico?.ClasificacionOMS_DNI101 ?? "Evaluación nutricional pediátrica de rutina.");

            InitializeComponent();

            _printDoc.DocumentName = $"Comprobante_NutriEvolve_{_paciente.DNINiño_DNI101}";
            _printDoc.PrintPage += PrintDoc_PrintPage;
            printPreviewControl.Document = _printDoc;
            printPreviewControl.Zoom = 1.0;
            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();
        }

        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                var idiomaBLL = new IdiomaBLL();
                TraductorUI.TraducirFormulario(this, idiomaBLL);
                printPreviewControl?.InvalidatePreview();
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics!;
            int left = 50;
            int right = e.PageBounds.Width - 50;
            int y = 45;

            // Fuentes institucionales
            using Font fTitle = new Font("Segoe UI", 16, FontStyle.Bold);
            using Font fSub = new Font("Segoe UI", 10, FontStyle.Regular);
            using Font fSec = new Font("Segoe UI", 11, FontStyle.Bold);
            using Font fText = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            using Font fBold = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            using Font fFoot = new Font("Segoe UI", 8.5f, FontStyle.Italic);

            // Brochas y plumas
            using Brush bDark = new SolidBrush(Color.FromArgb(30, 60, 40));
            using Brush bGray = new SolidBrush(Color.FromArgb(90, 100, 95));
            using Brush bLight = new SolidBrush(Color.FromArgb(240, 246, 240));
            using Pen pBorder = new Pen(Color.FromArgb(76, 124, 89), 2);
            using Pen pLine = new Pen(Color.FromArgb(200, 215, 205), 1);

            // Marco institucional
            g.DrawRectangle(pBorder, 40, 35, e.PageBounds.Width - 80, e.PageBounds.Height - 70);

            // ENCABEZADO
            g.FillRectangle(bLight, 42, 37, e.PageBounds.Width - 84, 85);
            g.DrawString("NUTRIEVOLVE - CONSULTORIO NUTRICIONAL PEDIÁTRICO", fTitle, bDark, left, y);
            y += 28;
            g.DrawString("Especialidad en Nutrición Infantil y Crecimiento Antropométrico OMS | Matrícula MP-7842", fSub, bDark, left, y);
            y += 22;
            string nroComp = $"Folio Comprobante: NE-{DateTime.Now:yyyyMMdd}-{_paciente.IdPaciente_DNI101:D4}  |  Fecha de Emisión: {DateTime.Now:dd/MM/yyyy HH:mm}";
            g.DrawString(nroComp, fBold, bDark, left, y);
            y += 45;

            // SECCIÓN 1: DATOS DEL PACIENTE
            g.DrawLine(pBorder, left, y, right, y);
            y += 8;
            g.DrawString("1. DATOS DEL PACIENTE PEDIÁTRICO", fSec, bDark, left, y);
            y += 24;

            int meses = _paciente.ObtenerEdadMeses();
            string edadStr = meses < 24 ? $"{meses} meses" : $"{meses / 12} años ({meses} meses)";

            g.DrawString($"Nombre y Apellido: {_paciente.NombreCompleto}", fBold, bDark, left, y);
            g.DrawString($"DNI: {_paciente.DNINiño_DNI101}", fBold, bDark, left + 360, y);
            y += 18;

            g.DrawString($"Fecha de Nacimiento: {_paciente.FechaNacimiento_DNI101:dd/MM/yyyy} ({edadStr})", fText, bDark, left, y);
            g.DrawString($"Sexo: {_paciente.Sexo_DNI101 ?? "Masculino"}", fText, bDark, left + 360, y);
            y += 18;

            g.DrawString($"Obra Social / Cobertura: {_paciente.ObraSocial_DNI101 ?? "Particular"}", fText, bDark, left, y);
            g.DrawString($"Contacto Tutor: {_paciente.Telefono_DNI101 ?? "No registrado"}", fText, bDark, left + 360, y);
            y += 30;

            // SECCIÓN 2: EVALUACIÓN ANTROPOMÉTRICA
            g.DrawLine(pBorder, left, y, right, y);
            y += 8;
            g.DrawString("2. EVALUACIÓN CLÍNICA ANTROPOMÉTRICA", fSec, bDark, left, y);
            y += 24;

            decimal peso = _consulta?.Medicion?.PesoKg_DNI101 ?? 0;
            decimal talla = _consulta?.Medicion?.TallaCm_DNI101 ?? 0;
            decimal imc = _consulta?.Medicion?.IMC_DNI101 ?? 0;

            g.DrawString($"Peso Actual: {peso:N2} kg", fBold, bDark, left, y);
            g.DrawString($"Talla Actual: {talla:N1} cm", fBold, bDark, left + 220, y);
            g.DrawString($"IMC: {imc:N2} kg/m²", fBold, bDark, left + 440, y);
            y += 20;

            g.DrawString($"Diagnóstico Clínico / Percentil OMS: {_diagnosticoClinico}", fText, bDark, left, y);
            y += 30;

            // SECCIÓN 3: PRESCRIPCIÓN DEL PLAN ALIMENTARIO
            g.DrawLine(pBorder, left, y, right, y);
            y += 8;
            g.DrawString("3. PRESCRIPCIÓN DIETOTERÁPICA Y DISTRIBUCIÓN DE MACRONUTRIENTES", fSec, bDark, left, y);
            y += 24;

            decimal kcal = _plan?.RequerimientoCalorico_DNI101 ?? 1500m;
            decimal cPct = _plan?.PctCarbohidratos_DNI101 ?? 55m;
            decimal pPct = _plan?.PctProteinas_DNI101 ?? 15m;
            decimal gPct = _plan?.PctGrasas_DNI101 ?? 30m;

            decimal cGram = _plan?.GramosCarbohidratos ?? Math.Round((kcal * (cPct / 100m)) / 4m, 1);
            decimal pGram = _plan?.GramosProteinas ?? Math.Round((kcal * (pPct / 100m)) / 4m, 1);
            decimal gGram = _plan?.GramosGrasas ?? Math.Round((kcal * (gPct / 100m)) / 9m, 1);

            g.DrawString($"Requerimiento Calórico Diario Prescrito: {kcal:N0} Kcal/día", fBold, bDark, left, y);
            y += 24;

            // Cuadro de macronutrientes
            int boxX = left;
            int boxW = right - left;
            g.FillRectangle(bLight, boxX, y, boxW, 50);
            g.DrawRectangle(pLine, boxX, y, boxW, 50);

            g.DrawString($"Carbohidratos: {cPct:N1}% ({cGram:N1} g/día)", fBold, bDark, boxX + 15, y + 15);
            g.DrawString($"Proteínas: {pPct:N1}% ({pGram:N1} g/día)", fBold, bDark, boxX + 260, y + 15);
            g.DrawString($"Grasas: {gPct:N1}% ({gGram:N1} g/día)", fBold, bDark, boxX + 500, y + 15);
            y += 65;

            // Metas de Salud
            string metas = string.IsNullOrWhiteSpace(_plan?.MetasSalud_DNI101)
                ? "Mantener crecimiento ponderoestatural armónico conforme al percentil esperado para la edad."
                : _plan.MetasSalud_DNI101;
            g.DrawString("Metas de Salud Clínicas:", fBold, bDark, left, y);
            y += 18;
            g.DrawString(metas, fText, bDark, new RectangleF(left, y, right - left, 40));
            y += 45;

            // Pautas Familiares
            string pautas = string.IsNullOrWhiteSpace(_plan?.PautasFamiliares_DNI101)
                ? "Incorporar variedad de verduras y frutas frescas. Evitar ultraprocesados, jugos azucarados y gaseosas. Hidratación adecuada con agua potable. Mantener rutinas de descanso y actividad física recreativa."
                : _plan.PautasFamiliares_DNI101;
            g.DrawString("Pautas Nutricionales Familiares y Recomendaciones Dietoterápicas:", fBold, bDark, left, y);
            y += 18;
            g.DrawString(pautas, fText, bDark, new RectangleF(left, y, right - left, 60));
            y += 75;

            // SECCIÓN 4: PRÓXIMO CONTROL Y VALIDEZ
            g.DrawLine(pBorder, left, y, right, y);
            y += 8;
            g.DrawString("4. INDICACIONES GENERALES Y PRÓXIMO CONTROL", fSec, bDark, left, y);
            y += 22;
            g.DrawString("• Próximo control sugerido: a los 30 días de emitida la presente ficha para monitoreo de peso y talla.", fText, bDark, left, y);
            y += 18;
            g.DrawString("• Documento emitido electrónicamente por el sistema NutriEvolve. Válido como comprobante formal de atención.", fFoot, bGray, left, y);
            y += 50;

            // SECCIÓN 5: FIRMA Y SELLO
            int sigX = right - 280;
            g.DrawLine(pBorder, sigX, y, right - 20, y);
            y += 6;
            g.DrawString("Lic. en Nutrición Pediátrica", fBold, bDark, sigX + 15, y);
            y += 16;
            g.DrawString("Matrícula Profesional MP-7842", fSub, bDark, sigX + 10, y);
            y += 16;
            g.DrawString("NutriEvolve - Consultorio Pediátrico", fFoot, bGray, sigX + 5, y);

            e.HasMorePages = false;
        }

        private void btnImprimir_Click(object? sender, EventArgs e)
        {
            try
            {
                using var pDlg = new PrintDialog();
                pDlg.Document = _printDoc;
                if (pDlg.ShowDialog(this) == DialogResult.OK)
                {
                    _printDoc.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar a impresión: {ex.Message}", "Error de Impresión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVistaPrevia_Click(object? sender, EventArgs e)
        {
            try
            {
                using var prevDlg = new PrintPreviewDialog();
                prevDlg.Document = _printDoc;
                prevDlg.WindowState = FormWindowState.Maximized;
                prevDlg.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar vista previa: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportar_Click(object? sender, EventArgs e)
        {
            try
            {
                using var sfd = new SaveFileDialog();
                sfd.Filter = "Imagen PNG (*.png)|*.png|Documento de Texto (*.txt)|*.txt";
                sfd.FileName = $"Comprobante_NutriEvolve_{_paciente.DNINiño_DNI101}_{DateTime.Now:yyyyMMdd}";

                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    if (sfd.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    {
                        string txt = 
                            "========================================================================\n" +
                            "NUTRIEVOLVE - CONSULTORIO NUTRICIONAL PEDIÁTRICO\n" +
                            "COMPROBANTE FORMAL DE ATENCIÓN Y FICHA DE PRESCRIPCIÓN\n" +
                            "========================================================================\n" +
                            $"Fecha de Emisión: {DateTime.Now:dd/MM/yyyy HH:mm}\n" +
                            $"Paciente: {_paciente.NombreCompleto} | DNI: {_paciente.DNINiño_DNI101}\n" +
                            $"Edad: {_paciente.ObtenerEdadMeses()} meses | Sexo: {_paciente.Sexo_DNI101 ?? "Masculino"}\n" +
                            $"Diagnóstico Clínico: {_diagnosticoClinico}\n\n" +
                            $"PLAN NUTRICIONAL:\n" +
                            $"Calorías: {_plan?.RequerimientoCalorico_DNI101 ?? 1500:N0} Kcal/día\n" +
                            $"Carbohidratos: {_plan?.PctCarbohidratos_DNI101 ?? 55}%\n" +
                            $"Proteínas: {_plan?.PctProteinas_DNI101 ?? 15}%\n" +
                            $"Grasas: {_plan?.PctGrasas_DNI101 ?? 30}%\n" +
                            $"Metas de Salud: {_plan?.MetasSalud_DNI101}\n" +
                            $"Pautas: {_plan?.PautasFamiliares_DNI101}\n" +
                            "========================================================================\n";
                        File.WriteAllText(sfd.FileName, txt);
                    }
                    else
                    {
                        // Renderizar página a Bitmap de alta resolución
                        using var bmp = new Bitmap(850, 1100);
                        using var g = Graphics.FromImage(bmp);
                        g.Clear(Color.White);

                        var pageArgs = new PrintPageEventArgs(g, new Rectangle(0, 0, 850, 1100), new Rectangle(0, 0, 850, 1100), new PageSettings());
                        PrintDoc_PrintPage(this, pageArgs);
                        bmp.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    }

                    MessageBox.Show("¡Comprobante guardado con éxito!", "Archivo Generado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar comprobante: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAyudaContextual_Click(object? sender, EventArgs e)
        {
            try
            {
                using var frmAyuda = new frmManualAyuda(4);
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
