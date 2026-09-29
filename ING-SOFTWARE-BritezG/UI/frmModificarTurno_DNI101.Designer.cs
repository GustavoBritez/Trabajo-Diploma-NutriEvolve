namespace UI
{
    partial class frmModificarTurno_DNI101
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
            lblSubtitulo = new Label();
            lblCodigoTurno = new Label();
            txtCodigoTurno = new TextBox();
            btnBuscarCodigo = new UI.Controles.BotonFuturista();
            panelInfoTurno = new UI.Controles.PanelOvalado();
            lblInfoTurno = new Label();
            lblEstado = new Label();
            cmbEstado = new ComboBox();
            lblMotivo = new Label();
            txtMotivo = new TextBox();
            lblAvisoReprogramar = new Label();
            btnGuardar = new UI.Controles.BotonFuturista();
            btnCancelar = new UI.Controles.BotonFuturista();
            panelHeader.SuspendLayout();
            panelCard.SuspendLayout();
            panelInfoTurno.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(70, 115, 140);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(580, 60);
            panelHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(20, 16);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(410, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "📝 Modificar Turno Nutricional - CUN04 (PN1)";
            // 
            // panelCard
            // 
            panelCard.BackColor = Color.White;
            panelCard.ColorBorde = Color.FromArgb(180, 205, 220);
            panelCard.Controls.Add(lblSubtitulo);
            panelCard.Controls.Add(lblCodigoTurno);
            panelCard.Controls.Add(txtCodigoTurno);
            panelCard.Controls.Add(btnBuscarCodigo);
            panelCard.Controls.Add(panelInfoTurno);
            panelCard.Controls.Add(lblEstado);
            panelCard.Controls.Add(cmbEstado);
            panelCard.Controls.Add(lblMotivo);
            panelCard.Controls.Add(txtMotivo);
            panelCard.Controls.Add(lblAvisoReprogramar);
            panelCard.Controls.Add(btnGuardar);
            panelCard.Controls.Add(btnCancelar);
            panelCard.DibujarBorde = true;
            panelCard.Location = new Point(25, 75);
            panelCard.Name = "panelCard";
            panelCard.RadioBorde = 24;
            panelCard.Size = new Size(530, 520);
            panelCard.TabIndex = 1;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = Color.FromArgb(60, 85, 100);
            lblSubtitulo.Location = new Point(25, 15);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(462, 15);
            lblSubtitulo.TabIndex = 0;
            lblSubtitulo.Text = "Busque un turno por su Código de Turno y actualice su Estado y Motivo de Consulta.";
            // 
            // lblCodigoTurno
            // 
            lblCodigoTurno.AutoSize = true;
            lblCodigoTurno.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblCodigoTurno.ForeColor = Color.FromArgb(35, 65, 85);
            lblCodigoTurno.Location = new Point(25, 45);
            lblCodigoTurno.Name = "lblCodigoTurno";
            lblCodigoTurno.Size = new Size(123, 17);
            lblCodigoTurno.TabIndex = 1;
            lblCodigoTurno.Text = "Código de Turno *";
            // 
            // txtCodigoTurno
            // 
            txtCodigoTurno.Font = new Font("Segoe UI", 10F);
            txtCodigoTurno.Location = new Point(25, 68);
            txtCodigoTurno.Name = "txtCodigoTurno";
            txtCodigoTurno.Size = new Size(335, 25);
            txtCodigoTurno.TabIndex = 2;
            txtCodigoTurno.KeyDown += txtCodigoTurno_KeyDown;
            // 
            // btnBuscarCodigo
            // 
            btnBuscarCodigo.ColorGlow = Color.LightSkyBlue;
            btnBuscarCodigo.ColorPrincipal = Color.FromArgb(70, 115, 140);
            btnBuscarCodigo.ColorSecundario = Color.FromArgb(45, 80, 100);
            btnBuscarCodigo.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            btnBuscarCodigo.ForeColor = Color.White;
            btnBuscarCodigo.Location = new Point(370, 65);
            btnBuscarCodigo.Name = "btnBuscarCodigo";
            btnBuscarCodigo.RadioBorde = 20;
            btnBuscarCodigo.Size = new Size(135, 30);
            btnBuscarCodigo.TabIndex = 3;
            btnBuscarCodigo.Text = "🔍 Buscar";
            btnBuscarCodigo.UseVisualStyleBackColor = false;
            btnBuscarCodigo.Click += btnBuscarCodigo_Click;
            // 
            // panelInfoTurno
            // 
            panelInfoTurno.BackColor = Color.FromArgb(240, 246, 250);
            panelInfoTurno.ColorBorde = Color.FromArgb(190, 215, 230);
            panelInfoTurno.Controls.Add(lblInfoTurno);
            panelInfoTurno.DibujarBorde = true;
            panelInfoTurno.Location = new Point(25, 108);
            panelInfoTurno.Name = "panelInfoTurno";
            panelInfoTurno.RadioBorde = 16;
            panelInfoTurno.Size = new Size(480, 95);
            panelInfoTurno.TabIndex = 4;
            // 
            // lblInfoTurno
            // 
            lblInfoTurno.Dock = DockStyle.Fill;
            lblInfoTurno.Font = new Font("Segoe UI", 9.5F);
            lblInfoTurno.ForeColor = Color.FromArgb(30, 60, 80);
            lblInfoTurno.Location = new Point(0, 0);
            lblInfoTurno.Name = "lblInfoTurno";
            lblInfoTurno.Padding = new Padding(12, 10, 12, 10);
            lblInfoTurno.Size = new Size(480, 95);
            lblInfoTurno.TabIndex = 0;
            lblInfoTurno.Text = "Ingrese un Código de Turno y presione 'Buscar' o seleccione un turno desde el Turnero Principal.";
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblEstado.ForeColor = Color.FromArgb(35, 65, 85);
            lblEstado.Location = new Point(25, 215);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(153, 17);
            lblEstado.TabIndex = 5;
            lblEstado.Text = "Nuevo Estado del Turno *";
            // 
            // cmbEstado
            // 
            cmbEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEstado.Font = new Font("Segoe UI", 10F);
            cmbEstado.FormattingEnabled = true;
            cmbEstado.Items.AddRange(new object[] { "Solicitado", "Confirmado", "Asistió", "Cancelado" });
            cmbEstado.Location = new Point(25, 237);
            cmbEstado.Name = "cmbEstado";
            cmbEstado.Size = new Size(480, 25);
            cmbEstado.TabIndex = 6;
            // 
            // lblMotivo
            // 
            lblMotivo.AutoSize = true;
            lblMotivo.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblMotivo.ForeColor = Color.FromArgb(35, 65, 85);
            lblMotivo.Location = new Point(25, 275);
            lblMotivo.Name = "lblMotivo";
            lblMotivo.Size = new Size(224, 17);
            lblMotivo.TabIndex = 7;
            lblMotivo.Text = "Motivo de Consulta / Observaciones *";
            // 
            // txtMotivo
            // 
            txtMotivo.Font = new Font("Segoe UI", 9.5F);
            txtMotivo.Location = new Point(25, 297);
            txtMotivo.Multiline = true;
            txtMotivo.Name = "txtMotivo";
            txtMotivo.ScrollBars = ScrollBars.Vertical;
            txtMotivo.Size = new Size(480, 95);
            txtMotivo.TabIndex = 8;
            // 
            // lblAvisoReprogramar
            // 
            lblAvisoReprogramar.AutoSize = true;
            lblAvisoReprogramar.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblAvisoReprogramar.ForeColor = Color.FromArgb(70, 100, 120);
            lblAvisoReprogramar.Location = new Point(25, 400);
            lblAvisoReprogramar.Name = "lblAvisoReprogramar";
            lblAvisoReprogramar.Size = new Size(393, 15);
            lblAvisoReprogramar.TabIndex = 9;
            lblAvisoReprogramar.Text = "ℹ️ Nota: Para modificar la fecha o el horario del turno utilice CUN03 (Reprogramar).";
            // 
            // btnGuardar
            // 
            btnGuardar.ColorGlow = Color.LightSkyBlue;
            btnGuardar.ColorPrincipal = Color.FromArgb(70, 115, 140);
            btnGuardar.ColorSecundario = Color.FromArgb(45, 80, 100);
            btnGuardar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(275, 455);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.RadioBorde = 30;
            btnGuardar.Size = new Size(230, 42);
            btnGuardar.TabIndex = 10;
            btnGuardar.Text = "💾 Modificar Turno";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.ColorGlow = Color.FromArgb(180, 100, 100);
            btnCancelar.ColorPrincipal = Color.FromArgb(170, 80, 80);
            btnCancelar.ColorSecundario = Color.FromArgb(130, 50, 50);
            btnCancelar.EsBotonSecundario = true;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(25, 455);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.RadioBorde = 30;
            btnCancelar.Size = new Size(160, 42);
            btnCancelar.TabIndex = 11;
            btnCancelar.Text = "❌ Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // frmModificarTurno_DNI101
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(235, 242, 246);
            ClientSize = new Size(580, 615);
            Controls.Add(panelCard);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmModificarTurno_DNI101";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CUN04 - Modificar Turno";
            Load += frmModificarTurno_DNI101_Load;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelCard.ResumeLayout(false);
            panelCard.PerformLayout();
            panelInfoTurno.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitulo;
        private UI.Controles.PanelOvalado panelCard;
        private Label lblSubtitulo;
        private Label lblCodigoTurno;
        private TextBox txtCodigoTurno;
        private UI.Controles.BotonFuturista btnBuscarCodigo;
        private UI.Controles.PanelOvalado panelInfoTurno;
        private Label lblInfoTurno;
        private Label lblEstado;
        private ComboBox cmbEstado;
        private Label lblMotivo;
        private TextBox txtMotivo;
        private Label lblAvisoReprogramar;
        private UI.Controles.BotonFuturista btnGuardar;
        private UI.Controles.BotonFuturista btnCancelar;
    }
}
