namespace UI
{
    partial class FrmReprogramarTurno_DNI101
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
            panelInfoActual = new UI.Controles.PanelOvalado();
            lblInfoTurno = new Label();
            lblProfesional = new Label();
            cmbProfesional = new ComboBox();
            lblNuevaFecha = new Label();
            dtpNuevaFecha = new DateTimePicker();
            lblNuevoHorario = new Label();
            cmbNuevoHorario = new ComboBox();
            lblDisponibilidad = new Label();
            btnConfirmar = new UI.Controles.BotonFuturista();
            btnCancelar = new UI.Controles.BotonFuturista();
            panelHeader.SuspendLayout();
            panelCard.SuspendLayout();
            panelInfoActual.SuspendLayout();
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
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(20, 16);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(420, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "🔄 Reprogramar Turno Nutricional - CUN03 (PN1)";
            // 
            // panelCard
            // 
            panelCard.BackColor = Color.White;
            panelCard.ColorBorde = Color.FromArgb(180, 205, 220);
            panelCard.Controls.Add(lblSubtitulo);
            panelCard.Controls.Add(panelInfoActual);
            panelCard.Controls.Add(lblProfesional);
            panelCard.Controls.Add(cmbProfesional);
            panelCard.Controls.Add(lblNuevaFecha);
            panelCard.Controls.Add(dtpNuevaFecha);
            panelCard.Controls.Add(lblNuevoHorario);
            panelCard.Controls.Add(cmbNuevoHorario);
            panelCard.Controls.Add(lblDisponibilidad);
            panelCard.Controls.Add(btnConfirmar);
            panelCard.Controls.Add(btnCancelar);
            panelCard.DibujarBorde = true;
            panelCard.Location = new Point(25, 75);
            panelCard.Name = "panelCard";
            panelCard.RadioBorde = 24;
            panelCard.Size = new Size(530, 480);
            panelCard.TabIndex = 1;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = Color.FromArgb(60, 85, 100);
            lblSubtitulo.Location = new Point(25, 15);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(465, 15);
            lblSubtitulo.TabIndex = 0;
            lblSubtitulo.Text = "Seleccione la nueva fecha y profesional deseado para reubicar el bloque horario del turno.";
            // 
            // panelInfoActual
            // 
            panelInfoActual.BackColor = Color.FromArgb(240, 246, 250);
            panelInfoActual.ColorBorde = Color.FromArgb(190, 215, 230);
            panelInfoActual.Controls.Add(lblInfoTurno);
            panelInfoActual.DibujarBorde = true;
            panelInfoActual.Location = new Point(25, 40);
            panelInfoActual.Name = "panelInfoActual";
            panelInfoActual.RadioBorde = 16;
            panelInfoActual.Size = new Size(480, 80);
            panelInfoActual.TabIndex = 1;
            // 
            // lblInfoTurno
            // 
            lblInfoTurno.Dock = DockStyle.Fill;
            lblInfoTurno.Font = new Font("Segoe UI", 9.5F);
            lblInfoTurno.ForeColor = Color.FromArgb(30, 60, 80);
            lblInfoTurno.Location = new Point(0, 0);
            lblInfoTurno.Name = "lblInfoTurno";
            lblInfoTurno.Padding = new Padding(12, 10, 12, 10);
            lblInfoTurno.Size = new Size(480, 80);
            lblInfoTurno.TabIndex = 0;
            lblInfoTurno.Text = "Cargando información del turno seleccionado...";
            // 
            // lblProfesional
            // 
            lblProfesional.AutoSize = true;
            lblProfesional.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblProfesional.ForeColor = Color.FromArgb(35, 65, 85);
            lblProfesional.Location = new Point(25, 135);
            lblProfesional.Name = "lblProfesional";
            lblProfesional.Size = new Size(134, 17);
            lblProfesional.TabIndex = 2;
            lblProfesional.Text = "Profesional Deseado *";
            // 
            // cmbProfesional
            // 
            cmbProfesional.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProfesional.Font = new Font("Segoe UI", 10F);
            cmbProfesional.FormattingEnabled = true;
            cmbProfesional.Location = new Point(25, 155);
            cmbProfesional.Name = "cmbProfesional";
            cmbProfesional.Size = new Size(480, 25);
            cmbProfesional.TabIndex = 3;
            cmbProfesional.SelectedIndexChanged += cmbProfesional_SelectedIndexChanged;
            // 
            // lblNuevaFecha
            // 
            lblNuevaFecha.AutoSize = true;
            lblNuevaFecha.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblNuevaFecha.ForeColor = Color.FromArgb(35, 65, 85);
            lblNuevaFecha.Location = new Point(25, 195);
            lblNuevaFecha.Name = "lblNuevaFecha";
            lblNuevaFecha.Size = new Size(95, 17);
            lblNuevaFecha.TabIndex = 4;
            lblNuevaFecha.Text = "Nueva Fecha *";
            // 
            // dtpNuevaFecha
            // 
            dtpNuevaFecha.CustomFormat = "dd/MM/yyyy";
            dtpNuevaFecha.Font = new Font("Segoe UI", 10F);
            dtpNuevaFecha.Format = DateTimePickerFormat.Custom;
            dtpNuevaFecha.Location = new Point(25, 215);
            dtpNuevaFecha.Name = "dtpNuevaFecha";
            dtpNuevaFecha.Size = new Size(230, 25);
            dtpNuevaFecha.TabIndex = 5;
            dtpNuevaFecha.ValueChanged += dtpNuevaFecha_ValueChanged;
            // 
            // lblNuevoHorario
            // 
            lblNuevoHorario.AutoSize = true;
            lblNuevoHorario.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblNuevoHorario.ForeColor = Color.FromArgb(35, 65, 85);
            lblNuevoHorario.Location = new Point(275, 195);
            lblNuevoHorario.Name = "lblNuevoHorario";
            lblNuevoHorario.Size = new Size(181, 17);
            lblNuevoHorario.TabIndex = 6;
            lblNuevoHorario.Text = "Nuevo Horario Disponible *";
            // 
            // cmbNuevoHorario
            // 
            cmbNuevoHorario.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNuevoHorario.Font = new Font("Segoe UI", 10F);
            cmbNuevoHorario.FormattingEnabled = true;
            cmbNuevoHorario.Location = new Point(275, 215);
            cmbNuevoHorario.Name = "cmbNuevoHorario";
            cmbNuevoHorario.Size = new Size(230, 25);
            cmbNuevoHorario.TabIndex = 7;
            // 
            // lblDisponibilidad
            // 
            lblDisponibilidad.AutoSize = true;
            lblDisponibilidad.Font = new Font("Segoe UI", 8.5F, FontStyle.Italic);
            lblDisponibilidad.ForeColor = Color.FromArgb(50, 110, 80);
            lblDisponibilidad.Location = new Point(25, 250);
            lblDisponibilidad.Name = "lblDisponibilidad";
            lblDisponibilidad.Size = new Size(260, 15);
            lblDisponibilidad.TabIndex = 8;
            lblDisponibilidad.Text = "CUN-07: Bloques horarios disponibles para la fecha.";
            // 
            // btnConfirmar
            // 
            btnConfirmar.ColorGlow = Color.LightSkyBlue;
            btnConfirmar.ColorPrincipal = Color.FromArgb(70, 115, 140);
            btnConfirmar.ColorSecundario = Color.FromArgb(45, 80, 100);
            btnConfirmar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnConfirmar.ForeColor = Color.White;
            btnConfirmar.Location = new Point(265, 415);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.RadioBorde = 30;
            btnConfirmar.Size = new Size(240, 42);
            btnConfirmar.TabIndex = 9;
            btnConfirmar.Text = "✔ Confirmar Reprogramación";
            btnConfirmar.UseVisualStyleBackColor = false;
            btnConfirmar.Click += btnConfirmar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.ColorGlow = Color.FromArgb(180, 100, 100);
            btnCancelar.ColorPrincipal = Color.FromArgb(170, 80, 80);
            btnCancelar.ColorSecundario = Color.FromArgb(130, 50, 50);
            btnCancelar.EsBotonSecundario = true;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(25, 415);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.RadioBorde = 30;
            btnCancelar.Size = new Size(160, 42);
            btnCancelar.TabIndex = 10;
            btnCancelar.Text = "❌ Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // FrmReprogramarTurno_DNI101
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(235, 242, 246);
            ClientSize = new Size(580, 580);
            Controls.Add(panelCard);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmReprogramarTurno_DNI101";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CUN03 - Reprogramar Turno";
            Load += FrmReprogramarTurno_DNI101_Load;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelCard.ResumeLayout(false);
            panelCard.PerformLayout();
            panelInfoActual.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitulo;
        private UI.Controles.PanelOvalado panelCard;
        private Label lblSubtitulo;
        private UI.Controles.PanelOvalado panelInfoActual;
        private Label lblInfoTurno;
        private Label lblProfesional;
        private ComboBox cmbProfesional;
        private Label lblNuevaFecha;
        private DateTimePicker dtpNuevaFecha;
        private Label lblNuevoHorario;
        private ComboBox cmbNuevoHorario;
        private Label lblDisponibilidad;
        private UI.Controles.BotonFuturista btnConfirmar;
        private UI.Controles.BotonFuturista btnCancelar;
    }
}
