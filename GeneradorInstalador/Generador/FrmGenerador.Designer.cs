namespace GeneradorInstalador
{
    partial class FrmGenerador
    {
        private System.ComponentModel.IContainer components = null;

        // Banner Superior
        private System.Windows.Forms.Panel pnlBanner;
        private System.Windows.Forms.Label lblBannerTitle;
        private System.Windows.Forms.Label lblBannerSubtitle;
        private System.Windows.Forms.Label lblBadgeVersion;

        // Contenedor Principal con Scroll
        private System.Windows.Forms.Panel pnlMain;

        // ===== Sección 1: Entrada =====
        private System.Windows.Forms.GroupBox grpEntrada;
        private System.Windows.Forms.TextBox txtRutaEntrada;
        private System.Windows.Forms.Button btnExaminarZip;
        private System.Windows.Forms.Button btnExaminarCarpeta;
        private System.Windows.Forms.Label lblDropHint;

        // ===== Sección 2: Diagnóstico =====
        private System.Windows.Forms.GroupBox grpDiagnostico;
        private System.Windows.Forms.FlowLayoutPanel flowBadges;
        private System.Windows.Forms.Label lblBadgeDal;
        private System.Windows.Forms.Label lblBadgeBe;
        private System.Windows.Forms.Label lblBadgeBll;
        private System.Windows.Forms.Label lblBadgeGui;
        private System.Windows.Forms.Label lblBadgeSql;
        private System.Windows.Forms.Label lblBadgeDv;
        private System.Windows.Forms.Label lblResumenDiagnostico;

        // ===== Sección 3: Configuración =====
        private System.Windows.Forms.GroupBox grpConfiguracion;
        private System.Windows.Forms.Label lblNombreApp;
        private System.Windows.Forms.TextBox txtNombreApp;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblNombreBD;
        private System.Windows.Forms.TextBox txtNombreBD;

        private System.Windows.Forms.Label lblProyectoPrincipal;
        private System.Windows.Forms.ComboBox cboProyectoPrincipal;
        private System.Windows.Forms.Label lblArchivoSql;
        private System.Windows.Forms.ComboBox cboArchivoSql;
        private System.Windows.Forms.Button btnExaminarSqlManual;

        private System.Windows.Forms.Label lblRutaSalida;
        private System.Windows.Forms.TextBox txtRutaSalida;
        private System.Windows.Forms.Button btnExaminarSalida;

        private System.Windows.Forms.CheckBox chkCompilarRelease;
        private System.Windows.Forms.CheckBox chkGenerarZip;
        private System.Windows.Forms.CheckBox chkDigitosVerificadores;

        // ===== Sección 4: Ejecución y Logs =====
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox txtLog;

        // Barra Inferior de Acciones
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblCreditos;
        private System.Windows.Forms.Button btnAbrirSalida;
        private System.Windows.Forms.Button btnProbarInstalador;
        private System.Windows.Forms.Button btnLimpiar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlBanner = new System.Windows.Forms.Panel();
            this.lblBannerTitle = new System.Windows.Forms.Label();
            this.lblBannerSubtitle = new System.Windows.Forms.Label();
            this.lblBadgeVersion = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblCreditos = new System.Windows.Forms.Label();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnProbarInstalador = new System.Windows.Forms.Button();
            this.btnAbrirSalida = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.grpEntrada = new System.Windows.Forms.GroupBox();
            this.txtRutaEntrada = new System.Windows.Forms.TextBox();
            this.btnExaminarZip = new System.Windows.Forms.Button();
            this.btnExaminarCarpeta = new System.Windows.Forms.Button();
            this.lblDropHint = new System.Windows.Forms.Label();
            this.grpDiagnostico = new System.Windows.Forms.GroupBox();
            this.flowBadges = new System.Windows.Forms.FlowLayoutPanel();
            this.lblBadgeDal = new System.Windows.Forms.Label();
            this.lblBadgeBe = new System.Windows.Forms.Label();
            this.lblBadgeBll = new System.Windows.Forms.Label();
            this.lblBadgeGui = new System.Windows.Forms.Label();
            this.lblBadgeSql = new System.Windows.Forms.Label();
            this.lblBadgeDv = new System.Windows.Forms.Label();
            this.lblResumenDiagnostico = new System.Windows.Forms.Label();
            this.grpConfiguracion = new System.Windows.Forms.GroupBox();
            this.lblNombreApp = new System.Windows.Forms.Label();
            this.txtNombreApp = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblNombreBD = new System.Windows.Forms.Label();
            this.txtNombreBD = new System.Windows.Forms.TextBox();
            this.lblProyectoPrincipal = new System.Windows.Forms.Label();
            this.cboProyectoPrincipal = new System.Windows.Forms.ComboBox();
            this.lblArchivoSql = new System.Windows.Forms.Label();
            this.cboArchivoSql = new System.Windows.Forms.ComboBox();
            this.btnExaminarSqlManual = new System.Windows.Forms.Button();
            this.lblRutaSalida = new System.Windows.Forms.Label();
            this.txtRutaSalida = new System.Windows.Forms.TextBox();
            this.btnExaminarSalida = new System.Windows.Forms.Button();
            this.chkCompilarRelease = new System.Windows.Forms.CheckBox();
            this.chkGenerarZip = new System.Windows.Forms.CheckBox();
            this.chkDigitosVerificadores = new System.Windows.Forms.CheckBox();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.pnlBanner.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.grpEntrada.SuspendLayout();
            this.grpDiagnostico.SuspendLayout();
            this.flowBadges.SuspendLayout();
            this.grpConfiguracion.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBanner
            // 
            this.pnlBanner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBanner.Controls.Add(this.lblBadgeVersion);
            this.pnlBanner.Controls.Add(this.lblBannerSubtitle);
            this.pnlBanner.Controls.Add(this.lblBannerTitle);
            this.pnlBanner.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBanner.Location = new System.Drawing.Point(0, 0);
            this.pnlBanner.Name = "pnlBanner";
            this.pnlBanner.Size = new System.Drawing.Size(914, 70);
            this.pnlBanner.TabIndex = 0;
            // 
            // lblBannerTitle
            // 
            this.lblBannerTitle.AutoSize = true;
            this.lblBannerTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblBannerTitle.Location = new System.Drawing.Point(15, 12);
            this.lblBannerTitle.Name = "lblBannerTitle";
            this.lblBannerTitle.Size = new System.Drawing.Size(165, 21);
            this.lblBannerTitle.TabIndex = 0;
            this.lblBannerTitle.Text = "Generador R Alpha";
            // 
            // lblBannerSubtitle
            // 
            this.lblBannerSubtitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBannerSubtitle.AutoEllipsis = true;
            this.lblBannerSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBannerSubtitle.Location = new System.Drawing.Point(17, 38);
            this.lblBannerSubtitle.Name = "lblBannerSubtitle";
            this.lblBannerSubtitle.Size = new System.Drawing.Size(715, 18);
            this.lblBannerSubtitle.TabIndex = 1;
            this.lblBannerSubtitle.Text = "Automatiza la creación de instaladores para proyectos en capas (DAL, BE, BLL, GUI + SQL Server)";
            // 
            // lblBadgeVersion
            // 
            this.lblBadgeVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBadgeVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBadgeVersion.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblBadgeVersion.Location = new System.Drawing.Point(745, 18);
            this.lblBadgeVersion.Name = "lblBadgeVersion";
            this.lblBadgeVersion.Size = new System.Drawing.Size(150, 30);
            this.lblBadgeVersion.TabIndex = 2;
            this.lblBadgeVersion.Text = "[ UNIVERSAL .NET 9 ]";
            this.lblBadgeVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBottom.Controls.Add(this.lblCreditos);
            this.pnlBottom.Controls.Add(this.btnAbrirSalida);
            this.pnlBottom.Controls.Add(this.btnProbarInstalador);
            this.pnlBottom.Controls.Add(this.btnLimpiar);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 671);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(914, 60);
            this.pnlBottom.TabIndex = 1;
            // 
            // lblCreditos
            // 
            this.lblCreditos.AutoSize = true;
            this.lblCreditos.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblCreditos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(65)))), ((int)(((byte)(81)))));
            this.lblCreditos.Location = new System.Drawing.Point(210, 20);
            this.lblCreditos.Name = "lblCreditos";
            this.lblCreditos.Size = new System.Drawing.Size(202, 17);
            this.lblCreditos.TabIndex = 3;
            this.lblCreditos.Text = "Desarrollado por Codigo Mecha";
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(15, 12);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(110, 34);
            this.btnLimpiar.TabIndex = 0;
            this.btnLimpiar.Text = "Reiniciar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.BtnLimpiar_Click);
            // 
            // btnProbarInstalador
            // 
            this.btnProbarInstalador.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProbarInstalador.Enabled = false;
            this.btnProbarInstalador.Location = new System.Drawing.Point(525, 12);
            this.btnProbarInstalador.Name = "btnProbarInstalador";
            this.btnProbarInstalador.Size = new System.Drawing.Size(180, 34);
            this.btnProbarInstalador.TabIndex = 1;
            this.btnProbarInstalador.Text = "Probar Instalador Ahora";
            this.btnProbarInstalador.UseVisualStyleBackColor = true;
            this.btnProbarInstalador.Click += new System.EventHandler(this.BtnProbarInstalador_Click);
            // 
            // btnAbrirSalida
            // 
            this.btnAbrirSalida.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbrirSalida.Enabled = false;
            this.btnAbrirSalida.Location = new System.Drawing.Point(715, 12);
            this.btnAbrirSalida.Name = "btnAbrirSalida";
            this.btnAbrirSalida.Size = new System.Drawing.Size(180, 34);
            this.btnAbrirSalida.TabIndex = 2;
            this.btnAbrirSalida.Text = "Abrir Carpeta Generada";
            this.btnAbrirSalida.UseVisualStyleBackColor = true;
            this.btnAbrirSalida.Click += new System.EventHandler(this.BtnAbrirSalida_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.Controls.Add(this.txtLog);
            this.pnlMain.Controls.Add(this.progressBar);
            this.pnlMain.Controls.Add(this.btnGenerar);
            this.pnlMain.Controls.Add(this.grpConfiguracion);
            this.pnlMain.Controls.Add(this.grpDiagnostico);
            this.pnlMain.Controls.Add(this.grpEntrada);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 70);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(15);
            this.pnlMain.Size = new System.Drawing.Size(914, 601);
            this.pnlMain.TabIndex = 2;
            // 
            // grpEntrada
            // 
            this.grpEntrada.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpEntrada.Controls.Add(this.lblDropHint);
            this.grpEntrada.Controls.Add(this.btnExaminarCarpeta);
            this.grpEntrada.Controls.Add(this.btnExaminarZip);
            this.grpEntrada.Controls.Add(this.txtRutaEntrada);
            this.grpEntrada.Location = new System.Drawing.Point(15, 10);
            this.grpEntrada.Name = "grpEntrada";
            this.grpEntrada.Size = new System.Drawing.Size(880, 90);
            this.grpEntrada.TabIndex = 0;
            this.grpEntrada.TabStop = false;
            this.grpEntrada.Text = "1. Entrada de la Solución o Proyecto";
            // 
            // txtRutaEntrada
            // 
            this.txtRutaEntrada.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRutaEntrada.Location = new System.Drawing.Point(15, 25);
            this.txtRutaEntrada.Name = "txtRutaEntrada";
            this.txtRutaEntrada.Size = new System.Drawing.Size(550, 23);
            this.txtRutaEntrada.TabIndex = 0;
            // 
            // btnExaminarZip
            // 
            this.btnExaminarZip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExaminarZip.Location = new System.Drawing.Point(575, 23);
            this.btnExaminarZip.Name = "btnExaminarZip";
            this.btnExaminarZip.Size = new System.Drawing.Size(140, 27);
            this.btnExaminarZip.TabIndex = 1;
            this.btnExaminarZip.Text = "Cargar .ZIP...";
            this.btnExaminarZip.UseVisualStyleBackColor = true;
            this.btnExaminarZip.Click += new System.EventHandler(this.BtnExaminarZip_Click);
            // 
            // btnExaminarCarpeta
            // 
            this.btnExaminarCarpeta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExaminarCarpeta.Location = new System.Drawing.Point(725, 23);
            this.btnExaminarCarpeta.Name = "btnExaminarCarpeta";
            this.btnExaminarCarpeta.Size = new System.Drawing.Size(140, 27);
            this.btnExaminarCarpeta.TabIndex = 2;
            this.btnExaminarCarpeta.Text = "Seleccionar Carpeta...";
            this.btnExaminarCarpeta.UseVisualStyleBackColor = true;
            this.btnExaminarCarpeta.Click += new System.EventHandler(this.BtnExaminarCarpeta_Click);
            // 
            // lblDropHint
            // 
            this.lblDropHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDropHint.AutoEllipsis = true;
            this.lblDropHint.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblDropHint.Location = new System.Drawing.Point(15, 58);
            this.lblDropHint.Name = "lblDropHint";
            this.lblDropHint.Size = new System.Drawing.Size(850, 18);
            this.lblDropHint.TabIndex = 3;
            this.lblDropHint.Text = "Nota: Puede arrastrar y soltar un archivo .zip o carpeta de proyecto directamente dentro de esta ventana.";
            // 
            // grpDiagnostico
            // 
            this.grpDiagnostico.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDiagnostico.Controls.Add(this.lblResumenDiagnostico);
            this.grpDiagnostico.Controls.Add(this.flowBadges);
            this.grpDiagnostico.Location = new System.Drawing.Point(15, 105);
            this.grpDiagnostico.Name = "grpDiagnostico";
            this.grpDiagnostico.Size = new System.Drawing.Size(880, 80);
            this.grpDiagnostico.TabIndex = 1;
            this.grpDiagnostico.TabStop = false;
            this.grpDiagnostico.Text = "2. Diagnóstico de Arquitectura Detectada";
            // 
            // flowBadges
            // 
            this.flowBadges.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowBadges.Controls.Add(this.lblBadgeDal);
            this.flowBadges.Controls.Add(this.lblBadgeBe);
            this.flowBadges.Controls.Add(this.lblBadgeBll);
            this.flowBadges.Controls.Add(this.lblBadgeGui);
            this.flowBadges.Controls.Add(this.lblBadgeSql);
            this.flowBadges.Controls.Add(this.lblBadgeDv);
            this.flowBadges.Location = new System.Drawing.Point(15, 22);
            this.flowBadges.Name = "flowBadges";
            this.flowBadges.Size = new System.Drawing.Size(850, 28);
            this.flowBadges.TabIndex = 0;
            // 
            // lblBadgeDal
            // 
            this.lblBadgeDal.AutoSize = true;
            this.lblBadgeDal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBadgeDal.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblBadgeDal.Location = new System.Drawing.Point(3, 0);
            this.lblBadgeDal.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.lblBadgeDal.Padding = new System.Windows.Forms.Padding(4);
            this.lblBadgeDal.Name = "lblBadgeDal";
            this.lblBadgeDal.Size = new System.Drawing.Size(59, 25);
            this.lblBadgeDal.TabIndex = 0;
            this.lblBadgeDal.Text = "DAL [-]";
            // 
            // lblBadgeBe
            // 
            this.lblBadgeBe.AutoSize = true;
            this.lblBadgeBe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBadgeBe.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblBadgeBe.Location = new System.Drawing.Point(73, 0);
            this.lblBadgeBe.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.lblBadgeBe.Padding = new System.Windows.Forms.Padding(4);
            this.lblBadgeBe.Name = "lblBadgeBe";
            this.lblBadgeBe.Size = new System.Drawing.Size(50, 25);
            this.lblBadgeBe.TabIndex = 1;
            this.lblBadgeBe.Text = "BE [-]";
            // 
            // lblBadgeBll
            // 
            this.lblBadgeBll.AutoSize = true;
            this.lblBadgeBll.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBadgeBll.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblBadgeBll.Location = new System.Drawing.Point(134, 0);
            this.lblBadgeBll.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.lblBadgeBll.Padding = new System.Windows.Forms.Padding(4);
            this.lblBadgeBll.Name = "lblBadgeBll";
            this.lblBadgeBll.Size = new System.Drawing.Size(56, 25);
            this.lblBadgeBll.TabIndex = 2;
            this.lblBadgeBll.Text = "BLL [-]";
            // 
            // lblBadgeGui
            // 
            this.lblBadgeGui.AutoSize = true;
            this.lblBadgeGui.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBadgeGui.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblBadgeGui.Location = new System.Drawing.Point(201, 0);
            this.lblBadgeGui.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.lblBadgeGui.Padding = new System.Windows.Forms.Padding(4);
            this.lblBadgeGui.Name = "lblBadgeGui";
            this.lblBadgeGui.Size = new System.Drawing.Size(76, 25);
            this.lblBadgeGui.TabIndex = 3;
            this.lblBadgeGui.Text = "GUI / UI [-]";
            // 
            // lblBadgeSql
            // 
            this.lblBadgeSql.AutoSize = true;
            this.lblBadgeSql.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBadgeSql.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblBadgeSql.Location = new System.Drawing.Point(288, 0);
            this.lblBadgeSql.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.lblBadgeSql.Padding = new System.Windows.Forms.Padding(4);
            this.lblBadgeSql.Name = "lblBadgeSql";
            this.lblBadgeSql.Size = new System.Drawing.Size(95, 25);
            this.lblBadgeSql.TabIndex = 4;
            this.lblBadgeSql.Text = "SQL SCRIPT [-]";
            // 
            // lblBadgeDv
            // 
            this.lblBadgeDv.AutoSize = true;
            this.lblBadgeDv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBadgeDv.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.lblBadgeDv.Location = new System.Drawing.Point(394, 0);
            this.lblBadgeDv.Margin = new System.Windows.Forms.Padding(3, 0, 8, 0);
            this.lblBadgeDv.Padding = new System.Windows.Forms.Padding(4);
            this.lblBadgeDv.Name = "lblBadgeDv";
            this.lblBadgeDv.Size = new System.Drawing.Size(164, 25);
            this.lblBadgeDv.TabIndex = 5;
            this.lblBadgeDv.Text = "DÍGITOS VERIFICADORES [-]";
            // 
            // lblResumenDiagnostico
            // 
            this.lblResumenDiagnostico.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResumenDiagnostico.AutoEllipsis = true;
            this.lblResumenDiagnostico.Location = new System.Drawing.Point(15, 55);
            this.lblResumenDiagnostico.Name = "lblResumenDiagnostico";
            this.lblResumenDiagnostico.Size = new System.Drawing.Size(850, 18);
            this.lblResumenDiagnostico.TabIndex = 1;
            this.lblResumenDiagnostico.Text = "Esperando selección de archivo o carpeta...";
            // 
            // grpConfiguracion
            // 
            this.grpConfiguracion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpConfiguracion.Controls.Add(this.chkDigitosVerificadores);
            this.grpConfiguracion.Controls.Add(this.chkGenerarZip);
            this.grpConfiguracion.Controls.Add(this.chkCompilarRelease);
            this.grpConfiguracion.Controls.Add(this.btnExaminarSalida);
            this.grpConfiguracion.Controls.Add(this.txtRutaSalida);
            this.grpConfiguracion.Controls.Add(this.lblRutaSalida);
            this.grpConfiguracion.Controls.Add(this.btnExaminarSqlManual);
            this.grpConfiguracion.Controls.Add(this.cboArchivoSql);
            this.grpConfiguracion.Controls.Add(this.lblArchivoSql);
            this.grpConfiguracion.Controls.Add(this.cboProyectoPrincipal);
            this.grpConfiguracion.Controls.Add(this.lblProyectoPrincipal);
            this.grpConfiguracion.Controls.Add(this.txtNombreBD);
            this.grpConfiguracion.Controls.Add(this.lblNombreBD);
            this.grpConfiguracion.Controls.Add(this.txtDescripcion);
            this.grpConfiguracion.Controls.Add(this.lblDescripcion);
            this.grpConfiguracion.Controls.Add(this.txtNombreApp);
            this.grpConfiguracion.Controls.Add(this.lblNombreApp);
            this.grpConfiguracion.Location = new System.Drawing.Point(15, 190);
            this.grpConfiguracion.Name = "grpConfiguracion";
            this.grpConfiguracion.Size = new System.Drawing.Size(880, 205);
            this.grpConfiguracion.TabIndex = 2;
            this.grpConfiguracion.TabStop = false;
            this.grpConfiguracion.Text = "3. Configuración y Personalización del Instalador";
            // 
            // lblNombreApp
            // 
            this.lblNombreApp.AutoSize = true;
            this.lblNombreApp.Location = new System.Drawing.Point(15, 25);
            this.lblNombreApp.Name = "lblNombreApp";
            this.lblNombreApp.Size = new System.Drawing.Size(140, 15);
            this.lblNombreApp.TabIndex = 0;
            this.lblNombreApp.Text = "Nombre de la Aplicación:";
            // 
            // txtNombreApp
            // 
            this.txtNombreApp.Location = new System.Drawing.Point(15, 45);
            this.txtNombreApp.Name = "txtNombreApp";
            this.txtNombreApp.Size = new System.Drawing.Size(260, 23);
            this.txtNombreApp.TabIndex = 1;
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(15, 75);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(126, 15);
            this.lblDescripcion.TabIndex = 2;
            this.lblDescripcion.Text = "Descripción / Subtítulo:";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(15, 95);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(260, 23);
            this.txtDescripcion.TabIndex = 3;
            this.txtDescripcion.Text = "Sistema de Gestión .NET";
            // 
            // lblNombreBD
            // 
            this.lblNombreBD.AutoSize = true;
            this.lblNombreBD.Location = new System.Drawing.Point(15, 125);
            this.lblNombreBD.Name = "lblNombreBD";
            this.lblNombreBD.Size = new System.Drawing.Size(163, 15);
            this.lblNombreBD.TabIndex = 4;
            this.lblNombreBD.Text = "Nombre de Base de Datos SQL:";
            // 
            // txtNombreBD
            // 
            this.txtNombreBD.Location = new System.Drawing.Point(15, 145);
            this.txtNombreBD.Name = "txtNombreBD";
            this.txtNombreBD.Size = new System.Drawing.Size(260, 23);
            this.txtNombreBD.TabIndex = 5;
            // 
            // lblProyectoPrincipal
            // 
            this.lblProyectoPrincipal.AutoSize = true;
            this.lblProyectoPrincipal.Location = new System.Drawing.Point(295, 25);
            this.lblProyectoPrincipal.Name = "lblProyectoPrincipal";
            this.lblProyectoPrincipal.Size = new System.Drawing.Size(155, 15);
            this.lblProyectoPrincipal.TabIndex = 6;
            this.lblProyectoPrincipal.Text = "Proyecto Principal (UI / GUI):";
            // 
            // cboProyectoPrincipal
            // 
            this.cboProyectoPrincipal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProyectoPrincipal.FormattingEnabled = true;
            this.cboProyectoPrincipal.Location = new System.Drawing.Point(295, 45);
            this.cboProyectoPrincipal.Name = "cboProyectoPrincipal";
            this.cboProyectoPrincipal.Size = new System.Drawing.Size(275, 23);
            this.cboProyectoPrincipal.TabIndex = 7;
            // 
            // lblArchivoSql
            // 
            this.lblArchivoSql.AutoSize = true;
            this.lblArchivoSql.Location = new System.Drawing.Point(295, 75);
            this.lblArchivoSql.Name = "lblArchivoSql";
            this.lblArchivoSql.Size = new System.Drawing.Size(142, 15);
            this.lblArchivoSql.TabIndex = 8;
            this.lblArchivoSql.Text = "Script SQL Maestro (.sql):";
            // 
            // cboArchivoSql
            // 
            this.cboArchivoSql.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboArchivoSql.FormattingEnabled = true;
            this.cboArchivoSql.Location = new System.Drawing.Point(295, 95);
            this.cboArchivoSql.Name = "cboArchivoSql";
            this.cboArchivoSql.Size = new System.Drawing.Size(275, 23);
            this.cboArchivoSql.TabIndex = 9;
            // 
            // btnExaminarSqlManual
            // 
            this.btnExaminarSqlManual.Location = new System.Drawing.Point(295, 125);
            this.btnExaminarSqlManual.Name = "btnExaminarSqlManual";
            this.btnExaminarSqlManual.Size = new System.Drawing.Size(125, 26);
            this.btnExaminarSqlManual.TabIndex = 10;
            this.btnExaminarSqlManual.Text = "Examinar SQL...";
            this.btnExaminarSqlManual.UseVisualStyleBackColor = true;
            this.btnExaminarSqlManual.Click += new System.EventHandler(this.BtnExaminarSqlManual_Click);
            // 
            // lblRutaSalida
            // 
            this.lblRutaSalida.AutoSize = true;
            this.lblRutaSalida.Location = new System.Drawing.Point(590, 25);
            this.lblRutaSalida.Name = "lblRutaSalida";
            this.lblRutaSalida.Size = new System.Drawing.Size(103, 15);
            this.lblRutaSalida.TabIndex = 11;
            this.lblRutaSalida.Text = "Carpeta de Salida:";
            // 
            // txtRutaSalida
            // 
            this.txtRutaSalida.Location = new System.Drawing.Point(590, 45);
            this.txtRutaSalida.Name = "txtRutaSalida";
            this.txtRutaSalida.Size = new System.Drawing.Size(225, 23);
            this.txtRutaSalida.TabIndex = 12;
            // 
            // btnExaminarSalida
            // 
            this.btnExaminarSalida.Location = new System.Drawing.Point(825, 43);
            this.btnExaminarSalida.Name = "btnExaminarSalida";
            this.btnExaminarSalida.Size = new System.Drawing.Size(45, 26);
            this.btnExaminarSalida.TabIndex = 13;
            this.btnExaminarSalida.Text = "...";
            this.btnExaminarSalida.UseVisualStyleBackColor = true;
            this.btnExaminarSalida.Click += new System.EventHandler(this.BtnExaminarSalida_Click);
            // 
            // chkCompilarRelease
            // 
            this.chkCompilarRelease.AutoSize = true;
            this.chkCompilarRelease.Checked = true;
            this.chkCompilarRelease.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCompilarRelease.Location = new System.Drawing.Point(590, 85);
            this.chkCompilarRelease.Name = "chkCompilarRelease";
            this.chkCompilarRelease.Size = new System.Drawing.Size(206, 19);
            this.chkCompilarRelease.TabIndex = 14;
            this.chkCompilarRelease.Text = "Compilar con dotnet publish (Release)";
            this.chkCompilarRelease.UseVisualStyleBackColor = true;
            // 
            // chkGenerarZip
            // 
            this.chkGenerarZip.AutoSize = true;
            this.chkGenerarZip.Checked = true;
            this.chkGenerarZip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGenerarZip.Location = new System.Drawing.Point(590, 110);
            this.chkGenerarZip.Name = "chkGenerarZip";
            this.chkGenerarZip.Size = new System.Drawing.Size(177, 19);
            this.chkGenerarZip.TabIndex = 15;
            this.chkGenerarZip.Text = "Generar archivo .ZIP empaquetado";
            this.chkGenerarZip.UseVisualStyleBackColor = true;
            // 
            // chkDigitosVerificadores
            // 
            this.chkDigitosVerificadores.AutoSize = true;
            this.chkDigitosVerificadores.Checked = true;
            this.chkDigitosVerificadores.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDigitosVerificadores.Location = new System.Drawing.Point(590, 135);
            this.chkDigitosVerificadores.Name = "chkDigitosVerificadores";
            this.chkDigitosVerificadores.Size = new System.Drawing.Size(189, 19);
            this.chkDigitosVerificadores.TabIndex = 16;
            this.chkDigitosVerificadores.Text = "Recalcular Dígitos Verificadores";
            this.chkDigitosVerificadores.UseVisualStyleBackColor = true;
            // 
            // btnGenerar
            // 
            this.btnGenerar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGenerar.Location = new System.Drawing.Point(15, 405);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(880, 40);
            this.btnGenerar.TabIndex = 3;
            this.btnGenerar.Text = "COMPILAR Y GENERAR REINSTALADOR AUTOMÁTICO";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.BtnGenerar_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(15, 455);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(880, 15);
            this.progressBar.TabIndex = 4;
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Font = new System.Drawing.Font("Consolas", 8.8F);
            this.txtLog.Location = new System.Drawing.Point(15, 480);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(880, 110);
            this.txtLog.TabIndex = 5;
            // 
            // FrmGenerador
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 731);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlBanner);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimumSize = new System.Drawing.Size(780, 640);
            this.Name = "FrmGenerador";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generador R Alpha";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FrmGenerador_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FrmGenerador_DragEnter);
            this.pnlBanner.ResumeLayout(false);
            this.pnlBanner.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.grpEntrada.ResumeLayout(false);
            this.grpEntrada.PerformLayout();
            this.grpDiagnostico.ResumeLayout(false);
            this.grpDiagnostico.PerformLayout();
            this.flowBadges.ResumeLayout(false);
            this.flowBadges.PerformLayout();
            this.grpConfiguracion.ResumeLayout(false);
            this.grpConfiguracion.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
