using System;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BLL;
using Services;

namespace UI.PN2
{
    public enum AccionSeleccionadaPN2
    {
        Ninguna,
        GraficarDiagnostico,
        RegistrarConsulta,
        PrescribirPlan
    }

    public partial class CustomMessageBoxPN2 : Form, IIdiomaObserver
    {
        public AccionSeleccionadaPN2 AccionElegida { get; private set; } = AccionSeleccionadaPN2.Ninguna;

        public CustomMessageBoxPN2(PacienteBE_DNI101 paciente)
        {
            InitializeComponent();
            ConfigurarDatosPaciente(paciente);
            ConfigurarBotonesAccion();
            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();
        }

        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                var idiomaBLL = new IdiomaBLL();
                TraductorUI.TraducirFormulario(this, idiomaBLL);
                ConfigurarBotonesAccion();
            }
        }

        private void ConfigurarDatosPaciente(PacienteBE_DNI101 paciente)
        {
            lblPacNombre.Text = paciente.NombreCompleto;

            int edadMeses = paciente.ObtenerEdadMeses();
            string strEdad = edadMeses < 24 ? $"{edadMeses} meses" : $"{edadMeses / 12} años ({edadMeses}m)";
            lblPacDetalles.Text = $"DNI: {paciente.DNINiño_DNI101}  |  Sexo: {paciente.Sexo_DNI101 ?? "N/D"}  |  Edad: {strEdad}  |  Cobertura: {paciente.ObraSocial_DNI101 ?? "Particular"}";
        }

        private void ConfigurarBotonesAccion()
        {
            var idiomaBLL = new IdiomaBLL();

            string titGraficar = idiomaBLL.ExisteTraduccion("btnGraficar") ? idiomaBLL.Traducir("btnGraficar") : "Graficar Diagnóstico";
            string subGraficar = idiomaBLL.ExisteTraduccion("sub_btnGraficar") ? idiomaBLL.Traducir("sub_btnGraficar") : "Visualizar las curvas evolutivas de la OMS (Peso, Talla, IMC)";

            string titReg = idiomaBLL.ExisteTraduccion("btnRegistrar") ? idiomaBLL.Traducir("btnRegistrar") : "Registrar Consulta";
            string subReg = idiomaBLL.ExisteTraduccion("sub_btnRegistrar") ? idiomaBLL.Traducir("sub_btnRegistrar") : "Cargar mediciones antropométricas, diagnóstico clínico y alertas";

            string titPlan = idiomaBLL.ExisteTraduccion("btnPrescribir") ? idiomaBLL.Traducir("btnPrescribir") : "Prescribir Plan Alimentario";
            string subPlan = idiomaBLL.ExisteTraduccion("sub_btnPrescribir") ? idiomaBLL.Traducir("sub_btnPrescribir") : "Definir metas calóricas, macronutrientes y pautas dietarias";

            EstilizarBotonAccion(
                btnGraficar,
                titGraficar,
                subGraficar,
                Color.FromArgb(46, 125, 50)
            );

            EstilizarBotonAccion(
                btnRegistrar,
                titReg,
                subReg,
                Color.FromArgb(21, 101, 192)
            );

            EstilizarBotonAccion(
                btnPrescribir,
                titPlan,
                subPlan,
                Color.FromArgb(76, 124, 89)
            );
        }

        private void EstilizarBotonAccion(Button btn, string titulo, string subtitulo, Color colorAcento)
        {
            btn.BackColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Cursor = Cursors.Hand;
            btn.TextAlign = ContentAlignment.TopLeft;
            btn.Padding = new Padding(15, 10, 10, 10);
            btn.FlatAppearance.BorderColor = Color.FromArgb(215, 225, 218);
            btn.FlatAppearance.BorderSize = 1;

            Label lblT = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = colorAcento,
                Location = new Point(14, 10),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            lblT.Click += (s, e) => btn.PerformClick();

            Label lblS = new Label
            {
                Text = subtitulo,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(100, 115, 105),
                Location = new Point(16, 36),
                AutoSize = true,
                BackColor = Color.Transparent
            };
            lblS.Click += (s, e) => btn.PerformClick();

            Panel pnlBarraLateral = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(6, 68),
                BackColor = colorAcento
            };
            pnlBarraLateral.Click += (s, e) => btn.PerformClick();

            btn.Controls.Add(pnlBarraLateral);
            btn.Controls.Add(lblT);
            btn.Controls.Add(lblS);

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(240, 247, 242);
                btn.FlatAppearance.BorderColor = colorAcento;
            };
            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = Color.White;
                btn.FlatAppearance.BorderColor = Color.FromArgb(215, 225, 218);
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen p = new Pen(Color.FromArgb(76, 124, 89), 2))
            {
                e.Graphics.DrawRectangle(p, 1, 1, Width - 2, Height - 2);
            }
        }

        private void PnlPaciente_Paint(object? sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.FromArgb(200, 220, 205), 1))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, pnlPaciente.Width - 1, pnlPaciente.Height - 1);
            }
        }

        private void BtnGraficar_Click(object? sender, EventArgs e)
        {
            AccionElegida = AccionSeleccionadaPN2.GraficarDiagnostico;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnRegistrar_Click(object? sender, EventArgs e)
        {
            AccionElegida = AccionSeleccionadaPN2.RegistrarConsulta;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnPrescribir_Click(object? sender, EventArgs e)
        {
            AccionElegida = AccionSeleccionadaPN2.PrescribirPlan;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCerrar_Click(object? sender, EventArgs e)
        {
            AccionElegida = AccionSeleccionadaPN2.Ninguna;
            Close();
        }
    }
}
