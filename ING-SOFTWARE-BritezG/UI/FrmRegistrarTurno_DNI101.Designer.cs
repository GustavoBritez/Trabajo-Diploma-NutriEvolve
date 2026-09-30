namespace UI
{
    partial class FrmRegistrarTurno_DNI101
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
            lblProfesional = new Label();
            cmbProfesional = new ComboBox();
            lblFecha = new Label();
            dtpFecha = new DateTimePicker();
            lblHorario = new Label();
            cmbHorario = new ComboBox();
            lblDisponibilidad = new Label();
            lblDni = new Label();
            txtDniNiño = new TextBox();
            lblPacienteInfo = new Label();
            lblMotivo = new Label();
            txtMotivo = new TextBox();
            btnRegistrarTurno = new UI.Controles.BotonFuturista();
            btnCancelar = new UI.Controles.BotonFuturista();
            panelHeader.SuspendLayout();
            panelCard.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(76, 124, 89);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(600, 60);
            panelHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(20, 16);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(421, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "📅 Registrar Turno Nutricional - CUN01 (PN1)";
            // 
            // panelCard
            // 
            panelCard.BackColor = Color.White;
            panelCard.ColorBorde = Color.FromArgb(180, 210, 185);
            panelCard.Controls.Add(lblSubtitulo);
            panelCard.Controls.Add(lblProfesional);
            panelCard.Controls.Add(cmbProfesional);
            panelCard.Controls.Add(lblFecha);
            panelCard.Controls.Add(dtpFecha);
            panelCard.Controls.Add(lblHorario);
            panelCard.Controls.Add(cmbHorario);
            panelCard.Controls.Add(lblDisponibilidad);
            panelCard.Controls.Add(lblDni);
            panelCard.Controls.Add(txtDniNiño);
            panelCard.Controls.Add(lblPacienteInfo);
            panelCard.Controls.Add(lblMotivo);
            panelCard.Controls.Add(txtMotivo);
            panelCard.Controls.Add(btnRegistrarTurno);
            panelCard.Controls.Add(btnCancelar);
            panelCard.DibujarBorde = true;
            panelCard.Location = new Point(25, 75);
            panelCard.Name = "panelCard";
            panelCard.RadioBorde = 24;
            panelCard.Size = new Size(550, 565);
            panelCard.TabIndex = 1;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = Color.FromArgb(70, 95, 75);
            lblSubtitulo.Location = new Point(25, 12);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(309, 15);
            lblSubtitulo.TabIndex = 0;
            lblSubtitulo.Text = "Consulta la disponibilidad y registra el turno del paciente.";
            // 
            // lblProfesional
            // 
            lblProfesional.AutoSize = true;
            lblProfesional.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblProfesional.ForeColor = Color.FromArgb(40, 70, 45);
            lblProfesional.Location = new Point(25, 38);
            lblProfesional.Name = "lblProfesional";
            lblProfesional.Size = new Size(166, 17);
            lblProfesional.TabIndex = 1;
            lblProfesional.Text = "Profesional Nutricionista *";
            // 
            // cmbProfesional
            // 
            cmbProfesional.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProfesional.Font = new Font("Segoe UI", 10F);
            cmbProfesional.FormattingEnabled = true;
            cmbProfesional.Location = new Point(25, 58);
            cmbProfesional.Name = "cmbProfesional";
            cmbProfesional.Size = new Size(500, 25);
            cmbProfesional.TabIndex = 2;
            cmbProfesional.SelectedIndexChanged += cmbProfesional_SelectedIndexChanged;
            // 
            // lblFecha
            // 
            lblFecha.AutoSize = true;
            lblFecha.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblFecha.ForeColor = Color.FromArgb(40, 70, 45);
            lblFecha.Location = new Point(25, 93);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(129, 17);
            lblFecha.TabIndex = 3;
            lblFecha.Text = "Fecha de Consulta *";
            // 
            // dtpFecha
            // 
            dtpFecha.CustomFormat = "dd/MM/yyyy";
            dtpFecha.Font = new Font("Segoe UI", 10F);
            dtpFecha.Format = DateTimePickerFormat.Custom;
            dtpFecha.Location = new Point(25, 113);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(240, 25);
            dtpFecha.TabIndex = 4;
            dtpFecha.ValueChanged += dtpFecha_ValueChanged;
            // 
            // lblHorario
            // 
            lblHorario.AutoSize = true;
            lblHorario.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblHorario.ForeColor = Color.FromArgb(40, 70, 45);
            lblHorario.Location = new Point(285, 93);
            lblHorario.Name = "lblHorario";
            lblHorario.Size = new Size(131, 17);
            lblHorario.TabIndex = 5;
            lblHorario.Text = "Horario Disponible *";
            // 
            // cmbHorario
            // 
            cmbHorario.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbHorario.Font = new Font("Segoe UI", 10F);
            cmbHorario.FormattingEnabled = true;
            cmbHorario.Location = new Point(285, 113);
            cmbHorario.Name = "cmbHorario";
            cmbHorario.Size = new Size(240, 25);
            cmbHorario.TabIndex = 6;
            // 
            // lblDisponibilidad
            // 
            lblDisponibilidad.AutoSize = true;
            lblDisponibilidad.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblDisponibilidad.ForeColor = Color.FromArgb(60, 110, 70);
            lblDisponibilidad.Location = new Point(25, 142);
            lblDisponibilidad.Name = "lblDisponibilidad";
            lblDisponibilidad.Size = new Size(267, 15);
            lblDisponibilidad.TabIndex = 7;
            lblDisponibilidad.Text = "CUN-07: Bloques horarios cargados exitosamente.";
            // 
            // lblDni
            // 
            lblDni.AutoSize = true;
            lblDni.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblDni.ForeColor = Color.FromArgb(40, 70, 45);
            lblDni.Location = new Point(25, 168);
            lblDni.Name = "lblDni";
            lblDni.Size = new Size(86, 17);
            lblDni.TabIndex = 8;
            lblDni.Text = "DNI Niño/a *";
            // 
            // txtDniNiño
            // 
            txtDniNiño.Font = new Font("Segoe UI", 10F);
            txtDniNiño.Location = new Point(25, 188);
            txtDniNiño.Name = "txtDniNiño";
            txtDniNiño.Size = new Size(500, 25);
            txtDniNiño.TabIndex = 9;
            txtDniNiño.Leave += txtDniNiño_Leave;
            // 
            // lblPacienteInfo
            // 
            lblPacienteInfo.AutoSize = true;
            lblPacienteInfo.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblPacienteInfo.ForeColor = Color.FromArgb(100, 125, 105);
            lblPacienteInfo.Location = new Point(25, 218);
            lblPacienteInfo.Name = "lblPacienteInfo";
            lblPacienteInfo.Size = new Size(358, 15);
            lblPacienteInfo.TabIndex = 10;
            lblPacienteInfo.Text = "Punto de Extensión CUN-02: Se abrirá registro si no está en padrón.";
            // 
            // lblMotivo
            // 
            lblMotivo.AutoSize = true;
            lblMotivo.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblMotivo.ForeColor = Color.FromArgb(40, 70, 45);
            lblMotivo.Location = new Point(25, 245);
            lblMotivo.Name = "lblMotivo";
            lblMotivo.Size = new Size(137, 17);
            lblMotivo.TabIndex = 11;
            lblMotivo.Text = "Motivo de Consulta *";
            // 
            // txtMotivo
            // 
            txtMotivo.Font = new Font("Segoe UI", 9.5F);
            txtMotivo.Location = new Point(25, 265);
            txtMotivo.Multiline = true;
            txtMotivo.Name = "txtMotivo";
            txtMotivo.ScrollBars = ScrollBars.Vertical;
            txtMotivo.Size = new Size(500, 215);
            txtMotivo.TabIndex = 12;
            // 
            // btnRegistrarTurno
            // 
            btnRegistrarTurno.BackColor = Color.Transparent;
            btnRegistrarTurno.ColorGlow = Color.DarkSeaGreen;
            btnRegistrarTurno.ColorPrincipal = Color.FromArgb(76, 124, 89);
            btnRegistrarTurno.ColorSecundario = Color.FromArgb(50, 85, 60);
            btnRegistrarTurno.EsBotonSecundario = false;
            btnRegistrarTurno.EsMenuLateral = false;
            btnRegistrarTurno.FlatStyle = FlatStyle.Flat;
            btnRegistrarTurno.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRegistrarTurno.ForeColor = Color.White;
            btnRegistrarTurno.Location = new Point(305, 500);
            btnRegistrarTurno.Name = "btnRegistrarTurno";
            btnRegistrarTurno.PaddingIzquierdo = 18;
            btnRegistrarTurno.RadioBorde = 30;
            btnRegistrarTurno.Size = new Size(220, 42);
            btnRegistrarTurno.TabIndex = 13;
            btnRegistrarTurno.Text = "✔ Registrar Turno";
            btnRegistrarTurno.UseVisualStyleBackColor = false;
            btnRegistrarTurno.Click += btnRegistrarTurno_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.Transparent;
            btnCancelar.ColorGlow = Color.FromArgb(180, 100, 100);
            btnCancelar.ColorPrincipal = Color.FromArgb(170, 80, 80);
            btnCancelar.ColorSecundario = Color.FromArgb(130, 50, 50);
            btnCancelar.EsBotonSecundario = true;
            btnCancelar.EsMenuLateral = false;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(25, 500);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.PaddingIzquierdo = 18;
            btnCancelar.RadioBorde = 30;
            btnCancelar.Size = new Size(160, 42);
            btnCancelar.TabIndex = 14;
            btnCancelar.Text = "❌ Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // FrmRegistrarTurno_DNI101
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(235, 245, 238);
            ClientSize = new Size(600, 655);
            ControlBox = false;
            Controls.Add(panelCard);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmRegistrarTurno_DNI101";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CUN01 - Registrar Turno";
            FormClosing += FrmRegistrarTurno_DNI101_FormClosing;
            Load += FrmRegistrarTurno_DNI101_Load;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelCard.ResumeLayout(false);
            panelCard.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitulo;
        private UI.Controles.PanelOvalado panelCard;
        private Label lblSubtitulo;
        private Label lblProfesional;
        private ComboBox cmbProfesional;
        private Label lblFecha;
        private DateTimePicker dtpFecha;
        private Label lblHorario;
        private ComboBox cmbHorario;
        private Label lblDisponibilidad;
        private Label lblDni;
        private TextBox txtDniNiño;
        private Label lblPacienteInfo;
        private Label lblMotivo;
        private TextBox txtMotivo;
        private UI.Controles.BotonFuturista btnRegistrarTurno;
        private UI.Controles.BotonFuturista btnCancelar;
    }
}
