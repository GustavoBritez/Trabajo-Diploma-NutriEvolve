using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using BE;

namespace UI.PN2
{
    public enum AccionSeleccionadaPN2
    {
        Ninguna,
        GraficarDiagnostico,
        RegistrarConsulta,
        PrescribirPlan
    }

    public class CustomMessageBoxPN2 : Form
    {
        public AccionSeleccionadaPN2 AccionElegida { get; private set; } = AccionSeleccionadaPN2.Ninguna;

        public CustomMessageBoxPN2(PacienteBE_DNI101 paciente)
        {
            ConfigurarFormulario();
            ConstruirInterfaz(paciente);
        }

        private void ConfigurarFormulario()
        {
            Text = "Acciones Clínicas - Seguimiento Nutricional";
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(540, 480);
            BackColor = Color.FromArgb(245, 248, 245);
            ShowInTaskbar = false;
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // Borde redondeado suave para el formulario
            using (Pen p = new Pen(Color.FromArgb(76, 124, 89), 2))
            {
                e.Graphics.DrawRectangle(p, 1, 1, Width - 2, Height - 2);
            }
        }

        private void ConstruirInterfaz(PacienteBE_DNI101 paciente)
        {
            // Panel de Encabezado Superior (Verde del sistema)
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.FromArgb(76, 124, 89)
            };

            Label lblTitulo = new Label
            {
                Text = "🥗 SELECCIONAR ACCIÓN CLÍNICA",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                AutoSize = true
            };

            Button btnCerrarCruz = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(35, 35),
                Location = new Point(Width - 45, 15),
                Cursor = Cursors.Hand
            };
            btnCerrarCruz.FlatAppearance.BorderSize = 0;
            btnCerrarCruz.Click += (s, e) => { AccionElegida = AccionSeleccionadaPN2.Ninguna; Close(); };

            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Controls.Add(btnCerrarCruz);
            Controls.Add(pnlHeader);

            // Tarjeta de información del paciente
            Panel pnlPaciente = new Panel
            {
                Location = new Point(20, 80),
                Size = new Size(500, 75),
                BackColor = Color.White
            };
            pnlPaciente.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(Color.FromArgb(200, 220, 205), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, pnlPaciente.Width - 1, pnlPaciente.Height - 1);
                }
            };

            Label lblPacNombre = new Label
            {
                Text = $"👶 {paciente.NombreCompleto}",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 45, 35),
                Location = new Point(12, 10),
                AutoSize = true
            };

            int edadMeses = paciente.ObtenerEdadMeses();
            string strEdad = edadMeses < 24 ? $"{edadMeses} meses" : $"{edadMeses / 12} años ({edadMeses}m)";

            Label lblPacDetalles = new Label
            {
                Text = $"DNI: {paciente.DNINiño_DNI101}  |  Sexo: {paciente.Sexo_DNI101 ?? "N/D"}  |  Edad: {strEdad}  |  Cobertura: {paciente.ObraSocial_DNI101 ?? "Particular"}",
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.FromArgb(90, 105, 95),
                Location = new Point(14, 40),
                AutoSize = true
            };

            pnlPaciente.Controls.Add(lblPacNombre);
            pnlPaciente.Controls.Add(lblPacDetalles);
            Controls.Add(pnlPaciente);

            // Subtítulo
            Label lblSubtitulo = new Label
            {
                Text = "Seleccione la operación a realizar para este paciente:",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(70, 85, 75),
                Location = new Point(22, 165),
                AutoSize = true
            };
            Controls.Add(lblSubtitulo);

            // Botón 1: Graficar Diagnóstico (CUN07)
            Button btnGraficar = CrearBotonAccion(
                "📈  Graficar Diagnóstico",
                "Visualizar las curvas evolutivas de la OMS (Peso, Talla, IMC)",
                Color.FromArgb(46, 125, 50),
                new Point(20, 195)
            );
            btnGraficar.Click += (s, e) =>
            {
                AccionElegida = AccionSeleccionadaPN2.GraficarDiagnostico;
                DialogResult = DialogResult.OK;
                Close();
            };
            Controls.Add(btnGraficar);

            // Botón 2: Registrar Consulta (CUN08)
            Button btnRegistrar = CrearBotonAccion(
                "📝  Registrar Consulta",
                "Cargar mediciones antropométricas, diagnóstico clínico y alertas",
                Color.FromArgb(21, 101, 192),
                new Point(20, 275)
            );
            btnRegistrar.Click += (s, e) =>
            {
                AccionElegida = AccionSeleccionadaPN2.RegistrarConsulta;
                DialogResult = DialogResult.OK;
                Close();
            };
            Controls.Add(btnRegistrar);

            // Botón 3: Prescribir Plan (CUN09)
            Button btnPrescribir = CrearBotonAccion(
                "📋  Prescribir Plan Alimentario",
                "Definir metas calóricas, macronutrientes y pautas dietarias",
                Color.FromArgb(123, 31, 162),
                new Point(20, 355)
            );
            btnPrescribir.Click += (s, e) =>
            {
                AccionElegida = AccionSeleccionadaPN2.PrescribirPlan;
                DialogResult = DialogResult.OK;
                Close();
            };
            Controls.Add(btnPrescribir);

            // Botón Cancelar al pie
            Button btnCancelar = new Button
            {
                Text = "Cancelar",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(90, 100, 95),
                BackColor = Color.FromArgb(235, 238, 235),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 32),
                Location = new Point((Width - 120) / 2, 435),
                Cursor = Cursors.Hand
            };
            btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(200, 210, 200);
            btnCancelar.Click += (s, e) => { AccionElegida = AccionSeleccionadaPN2.Ninguna; Close(); };
            Controls.Add(btnCancelar);
        }

        private Button CrearBotonAccion(string titulo, string subtitulo, Color colorAcento, Point posicion)
        {
            Button btn = new Button
            {
                Location = posicion,
                Size = new Size(500, 68),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.TopLeft,
                Padding = new Padding(15, 10, 10, 10)
            };
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

            return btn;
        }
    }
}
