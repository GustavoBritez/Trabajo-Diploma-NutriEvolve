#nullable enable
#pragma warning disable CS8618
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GeneradorInstalador
{
    partial class FrmGenerador
    {
        private System.ComponentModel.IContainer? components = null;

        // Banner Superior
        private Panel pnlBanner;
        private Label lblBannerTitle;
        private Label lblBannerSubtitle;
        private Label lblBadgeVersion;

        // Contenedor Principal con Scroll
        private Panel pnlMain;

        // ===== Sección 1: Entrada =====
        private GroupBox grpEntrada;
        private TextBox txtRutaEntrada;
        private Button btnExaminarZip;
        private Button btnExaminarCarpeta;
        private Label lblDropHint;

        // ===== Sección 2: Diagnóstico =====
        private GroupBox grpDiagnostico;
        private FlowLayoutPanel flowBadges;
        private Label lblBadgeDal;
        private Label lblBadgeBe;
        private Label lblBadgeBll;
        private Label lblBadgeGui;
        private Label lblBadgeSql;
        private Label lblBadgeDv;
        private Label lblResumenDiagnostico;

        // ===== Sección 3: Configuración =====
        private GroupBox grpConfiguracion;
        private Label lblNombreApp;
        private TextBox txtNombreApp;
        private Label lblDescripcion;
        private TextBox txtDescripcion;
        private Label lblNombreBD;
        private TextBox txtNombreBD;

        private Label lblProyectoPrincipal;
        private ComboBox cboProyectoPrincipal;
        private Label lblArchivoSql;
        private ComboBox cboArchivoSql;
        private Button btnExaminarSqlManual;

        private Label lblRutaSalida;
        private TextBox txtRutaSalida;
        private Button btnExaminarSalida;

        private CheckBox chkCompilarRelease;
        private CheckBox chkGenerarZip;
        private CheckBox chkDigitosVerificadores;

        // ===== Sección 4: Ejecución y Logs =====
        private Button btnGenerar;
        private ProgressBar progressBar;
        private TextBox txtLog;

        // Barra Inferior de Acciones
        private Panel pnlBottom;
        private Button btnAbrirSalida;
        private Button btnProbarInstalador;
        private Button btnLimpiar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            Text = "Generador Universal de Instaladores y Reinstaladores .NET";
            Size = new Size(930, 770);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            BackColor = Color.FromArgb(248, 250, 252);
            AllowDrop = true;

            // ==================== Banner Superior ====================
            pnlBanner = new Panel
            {
                Dock = DockStyle.Top,
                Height = 85,
                BackColor = Color.FromArgb(15, 23, 42)
            };

            lblBannerTitle = new Label
            {
                Text = "Generador Universal de Reinstaladores .NET",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(24, 15),
                AutoSize = true
            };

            lblBannerSubtitle = new Label
            {
                Text = "Automatiza la creación de instaladores inteligentes para proyectos en capas (DAL, BE, BLL, GUI + SQL Server)",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(148, 163, 184),
                Location = new Point(26, 47),
                AutoSize = true
            };

            lblBadgeVersion = new Label
            {
                Text = "UNIVERSAL .NET 9",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(37, 99, 235),
                Location = new Point(740, 26),
                Size = new Size(150, 28),
                TextAlign = ContentAlignment.MiddleCenter
            };

            pnlBanner.Controls.Add(lblBannerTitle);
            pnlBanner.Controls.Add(lblBannerSubtitle);
            pnlBanner.Controls.Add(lblBadgeVersion);

            // ==================== Barra Inferior ====================
            pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.FromArgb(241, 245, 249)
            };

            btnLimpiar = new Button
            {
                Text = "Reiniciar",
                Size = new Size(110, 36),
                Location = new Point(25, 12),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(71, 85, 105),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLimpiar.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnLimpiar.Click += BtnLimpiar_Click;

            btnProbarInstalador = new Button
            {
                Text = "Probar Instalador Ahora",
                Size = new Size(200, 36),
                Location = new Point(480, 12),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnProbarInstalador.FlatAppearance.BorderSize = 0;
            btnProbarInstalador.Click += BtnProbarInstalador_Click;

            btnAbrirSalida = new Button
            {
                Text = "Abrir Carpeta Generada",
                Size = new Size(200, 36),
                Location = new Point(690, 12),
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnAbrirSalida.FlatAppearance.BorderSize = 0;
            btnAbrirSalida.Click += BtnAbrirSalida_Click;

            pnlBottom.Controls.Add(btnLimpiar);
            pnlBottom.Controls.Add(btnProbarInstalador);
            pnlBottom.Controls.Add(btnAbrirSalida);

            // ==================== Contenedor Principal ====================
            pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20)
            };

            // ===== 1. Entrada =====
            grpEntrada = new GroupBox
            {
                Text = " 1. Entrada de la Solución o Proyecto ",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(20, 15),
                Size = new Size(875, 95)
            };

            txtRutaEntrada = new TextBox
            {
                Location = new Point(18, 30),
                Size = new Size(540, 29),
                Font = new Font("Segoe UI", 9.5f),
                BackColor = Color.White
            };

            btnExaminarZip = new Button
            {
                Text = "Cargar .ZIP...",
                Size = new Size(140, 30),
                Location = new Point(570, 29),
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExaminarZip.FlatAppearance.BorderSize = 0;
            btnExaminarZip.Click += BtnExaminarZip_Click;

            btnExaminarCarpeta = new Button
            {
                Text = "Seleccionar Carpeta...",
                Size = new Size(150, 30),
                Location = new Point(715, 29),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(51, 65, 85),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExaminarCarpeta.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnExaminarCarpeta.Click += BtnExaminarCarpeta_Click;

            lblDropHint = new Label
            {
                Text = "Nota: Puede arrastrar y soltar un archivo .zip o carpeta de proyecto directamente dentro de esta ventana.",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(18, 66),
                AutoSize = true
            };

            grpEntrada.Controls.Add(txtRutaEntrada);
            grpEntrada.Controls.Add(btnExaminarZip);
            grpEntrada.Controls.Add(btnExaminarCarpeta);
            grpEntrada.Controls.Add(lblDropHint);

            // ===== 2. Diagnóstico =====
            grpDiagnostico = new GroupBox
            {
                Text = " 2. Diagnóstico de Arquitectura Detectada ",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(20, 120),
                Size = new Size(875, 78)
            };

            flowBadges = new FlowLayoutPanel
            {
                Location = new Point(18, 26),
                Size = new Size(840, 32),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            lblBadgeDal = CrearBadge("DAL", false);
            lblBadgeBe = CrearBadge("BE", false);
            lblBadgeBll = CrearBadge("BLL", false);
            lblBadgeGui = CrearBadge("GUI / UI", false);
            lblBadgeSql = CrearBadge("SQL SCRIPT", false);
            lblBadgeDv = CrearBadge("DÍGITOS VERIFICADORES", false);

            flowBadges.Controls.Add(lblBadgeDal);
            flowBadges.Controls.Add(lblBadgeBe);
            flowBadges.Controls.Add(lblBadgeBll);
            flowBadges.Controls.Add(lblBadgeGui);
            flowBadges.Controls.Add(lblBadgeSql);
            flowBadges.Controls.Add(lblBadgeDv);

            lblResumenDiagnostico = new Label
            {
                Text = "Esperando selección de archivo o carpeta...",
                Font = new Font("Segoe UI", 8.8f, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 116, 139),
                Location = new Point(18, 56),
                AutoSize = true
            };

            grpDiagnostico.Controls.Add(flowBadges);
            grpDiagnostico.Controls.Add(lblResumenDiagnostico);

            // ===== 3. Configuración =====
            grpConfiguracion = new GroupBox
            {
                Text = " 3. Configuración y Personalización del Instalador ",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42),
                Location = new Point(20, 210),
                Size = new Size(875, 200)
            };

            // Columna Izquierda
            lblNombreApp = new Label { Text = "Nombre de la Aplicación:", Location = new Point(18, 28), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Regular) };
            txtNombreApp = new TextBox { Location = new Point(18, 48), Size = new Size(260, 27), Font = new Font("Segoe UI", 9.5f) };

            lblDescripcion = new Label { Text = "Descripción / Subtítulo:", Location = new Point(18, 80), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Regular) };
            txtDescripcion = new TextBox { Text = "Sistema de Gestión .NET", Location = new Point(18, 100), Size = new Size(260, 27), Font = new Font("Segoe UI", 9.5f) };

            lblNombreBD = new Label { Text = "Nombre de Base de Datos SQL:", Location = new Point(18, 132), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Regular) };
            txtNombreBD = new TextBox { Location = new Point(18, 152), Size = new Size(260, 27), Font = new Font("Segoe UI", 9.5f) };

            // Columna Central
            lblProyectoPrincipal = new Label { Text = "Proyecto Principal (UI / GUI):", Location = new Point(300, 28), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Regular) };
            cboProyectoPrincipal = new ComboBox { Location = new Point(300, 48), Size = new Size(280, 27), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f) };

            lblArchivoSql = new Label { Text = "Script SQL Maestro (.sql):", Location = new Point(300, 80), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Regular) };
            cboArchivoSql = new ComboBox { Location = new Point(300, 100), Size = new Size(280, 27), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5f) };

            btnExaminarSqlManual = new Button
            {
                Text = "Examinar SQL...",
                Location = new Point(300, 132),
                Size = new Size(130, 26),
                Font = new Font("Segoe UI", 8.5f),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExaminarSqlManual.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnExaminarSqlManual.Click += BtnExaminarSqlManual_Click;

            // Columna Derecha / Opciones
            lblRutaSalida = new Label { Text = "Carpeta de Salida:", Location = new Point(600, 28), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Regular) };
            txtRutaSalida = new TextBox { Location = new Point(600, 48), Size = new Size(200, 27), Font = new Font("Segoe UI", 9.5f) };

            btnExaminarSalida = new Button
            {
                Text = "...",
                Location = new Point(805, 47),
                Size = new Size(50, 28),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnExaminarSalida.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnExaminarSalida.Click += BtnExaminarSalida_Click;

            chkCompilarRelease = new CheckBox
            {
                Text = "Compilar con dotnet publish (Release)",
                Checked = true,
                Location = new Point(600, 85),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.8f)
            };

            chkGenerarZip = new CheckBox
            {
                Text = "Generar archivo .ZIP empaquetado",
                Checked = true,
                Location = new Point(600, 112),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.8f)
            };

            chkDigitosVerificadores = new CheckBox
            {
                Text = "Recalcular Dígitos Verificadores",
                Checked = false,
                Location = new Point(600, 139),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.8f)
            };

            grpConfiguracion.Controls.Add(lblNombreApp);
            grpConfiguracion.Controls.Add(txtNombreApp);
            grpConfiguracion.Controls.Add(lblDescripcion);
            grpConfiguracion.Controls.Add(txtDescripcion);
            grpConfiguracion.Controls.Add(lblNombreBD);
            grpConfiguracion.Controls.Add(txtNombreBD);

            grpConfiguracion.Controls.Add(lblProyectoPrincipal);
            grpConfiguracion.Controls.Add(cboProyectoPrincipal);
            grpConfiguracion.Controls.Add(lblArchivoSql);
            grpConfiguracion.Controls.Add(cboArchivoSql);
            grpConfiguracion.Controls.Add(btnExaminarSqlManual);

            grpConfiguracion.Controls.Add(lblRutaSalida);
            grpConfiguracion.Controls.Add(txtRutaSalida);
            grpConfiguracion.Controls.Add(btnExaminarSalida);
            grpConfiguracion.Controls.Add(chkCompilarRelease);
            grpConfiguracion.Controls.Add(chkGenerarZip);
            grpConfiguracion.Controls.Add(chkDigitosVerificadores);

            // ===== 4. Botón de Generación y Logs =====
            btnGenerar = new Button
            {
                Text = "COMPILAR Y GENERAR REINSTALADOR AUTOMÁTICO",
                Location = new Point(20, 420),
                Size = new Size(875, 45),
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnGenerar.FlatAppearance.BorderSize = 0;
            btnGenerar.Click += BtnGenerar_Click;

            progressBar = new ProgressBar
            {
                Location = new Point(20, 475),
                Size = new Size(875, 16),
                Style = ProgressBarStyle.Continuous
            };

            txtLog = new TextBox
            {
                Location = new Point(20, 500),
                Size = new Size(875, 175),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.FromArgb(15, 23, 42),
                ForeColor = Color.FromArgb(241, 245, 249),
                Font = new Font("Consolas", 8.8f)
            };

            pnlMain.Controls.Add(grpEntrada);
            pnlMain.Controls.Add(grpDiagnostico);
            pnlMain.Controls.Add(grpConfiguracion);
            pnlMain.Controls.Add(btnGenerar);
            pnlMain.Controls.Add(progressBar);
            pnlMain.Controls.Add(txtLog);

            // ==================== Controles Generales ====================
            Controls.Add(pnlMain);
            Controls.Add(pnlBottom);
            Controls.Add(pnlBanner);

            DragEnter += FrmGenerador_DragEnter;
            DragDrop += FrmGenerador_DragDrop;
        }

        private Label CrearBadge(string texto, bool activo)
        {
            var lbl = new Label
            {
                Text = texto,
                AutoSize = true,
                Padding = new Padding(6, 4, 6, 4),
                Margin = new Padding(3, 0, 3, 0),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = activo ? Color.White : Color.FromArgb(148, 163, 184),
                BackColor = activo ? Color.FromArgb(16, 185, 129) : Color.FromArgb(226, 232, 240)
            };
            return lbl;
        }
    }
}
