namespace UI
{
    partial class fmrDigitoVerificador
    {
        private System.ComponentModel.IContainer components = null;

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
            panelHeader = new Panel();
            lblTitulo = new Label();
            panelCard = new UI.Controles.PanelOvalado();
            panelAlerta = new UI.Controles.PanelOvalado();
            lblIcono = new Label();
            lblMensajeError = new Label();
            lblDescripcion = new Label();
            btnRecalcularDV = new UI.Controles.BotonFuturista();
            btnSubirBackup = new UI.Controles.BotonFuturista();
            btnSalir = new UI.Controles.BotonFuturista();
            panelHeader.SuspendLayout();
            panelCard.SuspendLayout();
            panelAlerta.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(46, 75, 95);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(650, 60);
            panelHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(20, 16);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(385, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "🛡️ Control de Integridad de Base de Datos";
            // 
            // panelCard
            // 
            panelCard.BackColor = Color.White;
            panelCard.ColorBorde = Color.FromArgb(200, 215, 225);
            panelCard.Controls.Add(panelAlerta);
            panelCard.Controls.Add(lblDescripcion);
            panelCard.Controls.Add(btnRecalcularDV);
            panelCard.Controls.Add(btnSubirBackup);
            panelCard.Controls.Add(btnSalir);
            panelCard.DibujarBorde = true;
            panelCard.Location = new Point(25, 80);
            panelCard.Name = "panelCard";
            panelCard.RadioBorde = 24;
            panelCard.Size = new Size(600, 310);
            panelCard.TabIndex = 1;
            // 
            // panelAlerta
            // 
            panelAlerta.BackColor = Color.FromArgb(254, 242, 242);
            panelAlerta.ColorBorde = Color.FromArgb(248, 180, 180);
            panelAlerta.Controls.Add(lblIcono);
            panelAlerta.Controls.Add(lblMensajeError);
            panelAlerta.DibujarBorde = true;
            panelAlerta.Location = new Point(25, 25);
            panelAlerta.Name = "panelAlerta";
            panelAlerta.RadioBorde = 16;
            panelAlerta.Size = new Size(550, 95);
            panelAlerta.TabIndex = 0;
            // 
            // lblIcono
            // 
            lblIcono.Font = new Font("Segoe UI", 26F);
            lblIcono.Location = new Point(15, 18);
            lblIcono.Name = "lblIcono";
            lblIcono.Size = new Size(55, 55);
            lblIcono.TabIndex = 0;
            lblIcono.Text = "⚠️";
            lblIcono.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblMensajeError
            // 
            lblMensajeError.AutoEllipsis = true;
            lblMensajeError.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblMensajeError.ForeColor = Color.FromArgb(185, 28, 28);
            lblMensajeError.Location = new Point(78, 12);
            lblMensajeError.Name = "lblMensajeError";
            lblMensajeError.Padding = new Padding(0, 10, 10, 10);
            lblMensajeError.Size = new Size(455, 70);
            lblMensajeError.TabIndex = 1;
            lblMensajeError.Text = "Hubo Cambios en la Tabla [Nombre de tabla]";
            lblMensajeError.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDescripcion
            // 
            lblDescripcion.Font = new Font("Segoe UI", 9.5F);
            lblDescripcion.ForeColor = Color.FromArgb(70, 90, 105);
            lblDescripcion.Location = new Point(28, 132);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(544, 45);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Se ha detectado una inconsistencia de integridad. Puede forzar el recálculo de los Dígitos Verificadores, restaurar una copia de seguridad o cerrar la sesión.";
            // 
            // btnRecalcularDV
            // 
            btnRecalcularDV.ColorGlow = Color.LightGreen;
            btnRecalcularDV.ColorPrincipal = Color.FromArgb(46, 125, 50);
            btnRecalcularDV.ColorSecundario = Color.FromArgb(27, 94, 32);
            btnRecalcularDV.Cursor = Cursors.Hand;
            btnRecalcularDV.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            btnRecalcularDV.ForeColor = Color.White;
            btnRecalcularDV.Location = new Point(25, 230);
            btnRecalcularDV.Name = "btnRecalcularDV";
            btnRecalcularDV.RadioBorde = 20;
            btnRecalcularDV.Size = new Size(170, 48);
            btnRecalcularDV.TabIndex = 2;
            btnRecalcularDV.Text = "Recalcular DV";
            btnRecalcularDV.UseVisualStyleBackColor = true;
            btnRecalcularDV.Click += btnRecalcularDV_Click;
            // 
            // btnSubirBackup
            // 
            btnSubirBackup.ColorGlow = Color.LightSkyBlue;
            btnSubirBackup.ColorPrincipal = Color.FromArgb(30, 100, 150);
            btnSubirBackup.ColorSecundario = Color.FromArgb(15, 60, 100);
            btnSubirBackup.Cursor = Cursors.Hand;
            btnSubirBackup.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            btnSubirBackup.ForeColor = Color.White;
            btnSubirBackup.Location = new Point(215, 230);
            btnSubirBackup.Name = "btnSubirBackup";
            btnSubirBackup.RadioBorde = 20;
            btnSubirBackup.Size = new Size(170, 48);
            btnSubirBackup.TabIndex = 3;
            btnSubirBackup.Text = "SubirBackup";
            btnSubirBackup.UseVisualStyleBackColor = true;
            btnSubirBackup.Click += btnSubirBackup_Click;
            // 
            // btnSalir
            // 
            btnSalir.ColorGlow = Color.MistyRose;
            btnSalir.ColorPrincipal = Color.FromArgb(195, 60, 60);
            btnSalir.ColorSecundario = Color.FromArgb(145, 30, 30);
            btnSalir.Cursor = Cursors.Hand;
            btnSalir.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            btnSalir.ForeColor = Color.White;
            btnSalir.Location = new Point(405, 230);
            btnSalir.Name = "btnSalir";
            btnSalir.RadioBorde = 20;
            btnSalir.Size = new Size(170, 48);
            btnSalir.TabIndex = 4;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            // 
            // fmrDigitoVerificador
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 248, 250);
            ClientSize = new Size(650, 420);
            Controls.Add(panelCard);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "fmrDigitoVerificador";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Control de Dígito Verificador";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelCard.ResumeLayout(false);
            panelAlerta.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitulo;
        private UI.Controles.PanelOvalado panelCard;
        private UI.Controles.PanelOvalado panelAlerta;
        private Label lblIcono;
        private Label lblMensajeError;
        private Label lblDescripcion;
        private UI.Controles.BotonFuturista btnRecalcularDV;
        private UI.Controles.BotonFuturista btnSubirBackup;
        private UI.Controles.BotonFuturista btnSalir;
    }
}
