namespace UI
{
    partial class RegistrarPaciente_DNI101
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
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblApellido = new Label();
            txtApellido = new TextBox();
            lblDni = new Label();
            txtDniNiño = new TextBox();
            lblTelefono = new Label();
            txtTelefono = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblObraSocial = new Label();
            cmbObraSocial = new ComboBox();
            btnRegistrarPaciente = new UI.Controles.BotonFuturista();
            btnCancelarPaciente = new UI.Controles.BotonFuturista();
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
            lblTitulo.Size = new Size(330, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "👤 Registro de Paciente Pediátrico";
            // 
            // panelCard
            // 
            panelCard.BackColor = Color.White;
            panelCard.ColorBorde = Color.FromArgb(180, 210, 185);
            panelCard.Controls.Add(lblSubtitulo);
            panelCard.Controls.Add(lblNombre);
            panelCard.Controls.Add(txtNombre);
            panelCard.Controls.Add(lblApellido);
            panelCard.Controls.Add(txtApellido);
            panelCard.Controls.Add(lblDni);
            panelCard.Controls.Add(txtDniNiño);
            panelCard.Controls.Add(lblTelefono);
            panelCard.Controls.Add(txtTelefono);
            panelCard.Controls.Add(lblEmail);
            panelCard.Controls.Add(txtEmail);
            panelCard.Controls.Add(lblObraSocial);
            panelCard.Controls.Add(cmbObraSocial);
            panelCard.Controls.Add(btnRegistrarPaciente);
            panelCard.Controls.Add(btnCancelarPaciente);
            panelCard.DibujarBorde = true;
            panelCard.Location = new Point(30, 80);
            panelCard.Name = "panelCard";
            panelCard.RadioBorde = 24;
            panelCard.Size = new Size(520, 480);
            panelCard.TabIndex = 1;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9.5F);
            lblSubtitulo.ForeColor = Color.FromArgb(70, 95, 75);
            lblSubtitulo.Location = new Point(30, 18);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(450, 17);
            lblSubtitulo.TabIndex = 0;
            lblSubtitulo.Text = "Complete los datos del paciente para vincularlo al turno nutricional.";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblNombre.ForeColor = Color.FromArgb(40, 70, 45);
            lblNombre.Location = new Point(30, 52);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(68, 19);
            lblNombre.TabIndex = 1;
            lblNombre.Text = "Nombre *";
            // 
            // txtNombre
            // 
            txtNombre.Font = new Font("Segoe UI", 10.5F);
            txtNombre.Location = new Point(30, 74);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(460, 26);
            txtNombre.TabIndex = 2;
            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblApellido.ForeColor = Color.FromArgb(40, 70, 45);
            lblApellido.Location = new Point(30, 110);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(68, 19);
            lblApellido.TabIndex = 3;
            lblApellido.Text = "Apellido *";
            // 
            // txtApellido
            // 
            txtApellido.Font = new Font("Segoe UI", 10.5F);
            txtApellido.Location = new Point(30, 132);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(460, 26);
            txtApellido.TabIndex = 4;
            // 
            // lblDni
            // 
            lblDni.AutoSize = true;
            lblDni.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblDni.ForeColor = Color.FromArgb(40, 70, 45);
            lblDni.Location = new Point(30, 168);
            lblDni.Name = "lblDni";
            lblDni.Size = new Size(95, 19);
            lblDni.TabIndex = 5;
            lblDni.Text = "DNI Niño/a *";
            // 
            // txtDniNiño
            // 
            txtDniNiño.Font = new Font("Segoe UI", 10.5F);
            txtDniNiño.Location = new Point(30, 190);
            txtDniNiño.Name = "txtDniNiño";
            txtDniNiño.Size = new Size(460, 26);
            txtDniNiño.TabIndex = 6;
            // 
            // lblTelefono
            // 
            lblTelefono.AutoSize = true;
            lblTelefono.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblTelefono.ForeColor = Color.FromArgb(40, 70, 45);
            lblTelefono.Location = new Point(30, 226);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(62, 19);
            lblTelefono.TabIndex = 7;
            lblTelefono.Text = "Teléfono";
            // 
            // txtTelefono
            // 
            txtTelefono.Font = new Font("Segoe UI", 10.5F);
            txtTelefono.Location = new Point(30, 248);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(460, 26);
            txtTelefono.TabIndex = 8;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(40, 70, 45);
            lblEmail.Location = new Point(30, 284);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(43, 19);
            lblEmail.TabIndex = 9;
            lblEmail.Text = "Email";
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Segoe UI", 10.5F);
            txtEmail.Location = new Point(30, 306);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(460, 26);
            txtEmail.TabIndex = 10;
            // 
            // lblObraSocial
            // 
            lblObraSocial.AutoSize = true;
            lblObraSocial.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblObraSocial.ForeColor = Color.FromArgb(40, 70, 45);
            lblObraSocial.Location = new Point(30, 342);
            lblObraSocial.Name = "lblObraSocial";
            lblObraSocial.Size = new Size(95, 19);
            lblObraSocial.TabIndex = 11;
            lblObraSocial.Text = "Obra Social *";
            // 
            // cmbObraSocial
            // 
            cmbObraSocial.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbObraSocial.Font = new Font("Segoe UI", 10.5F);
            cmbObraSocial.FormattingEnabled = true;
            cmbObraSocial.Items.AddRange(new object[] { "Medicus", "OSPEP", "SAO" });
            cmbObraSocial.Location = new Point(30, 364);
            cmbObraSocial.Name = "cmbObraSocial";
            cmbObraSocial.Size = new Size(460, 27);
            cmbObraSocial.TabIndex = 12;
            // 
            // btnRegistrarPaciente
            // 
            btnRegistrarPaciente.ColorGlow = Color.DarkSeaGreen;
            btnRegistrarPaciente.ColorPrincipal = Color.FromArgb(76, 124, 89);
            btnRegistrarPaciente.ColorSecundario = Color.FromArgb(50, 85, 60);
            btnRegistrarPaciente.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRegistrarPaciente.ForeColor = Color.White;
            btnRegistrarPaciente.Location = new Point(270, 415);
            btnRegistrarPaciente.Name = "btnRegistrarPaciente";
            btnRegistrarPaciente.RadioBorde = 30;
            btnRegistrarPaciente.Size = new Size(220, 42);
            btnRegistrarPaciente.TabIndex = 13;
            btnRegistrarPaciente.Text = "✔ Registrar Paciente";
            btnRegistrarPaciente.UseVisualStyleBackColor = false;
            btnRegistrarPaciente.Click += btnRegistrarPaciente_Click;
            // 
            // btnCancelarPaciente
            // 
            btnCancelarPaciente.ColorGlow = Color.FromArgb(180, 100, 100);
            btnCancelarPaciente.ColorPrincipal = Color.FromArgb(170, 80, 80);
            btnCancelarPaciente.ColorSecundario = Color.FromArgb(130, 50, 50);
            btnCancelarPaciente.EsBotonSecundario = true;
            btnCancelarPaciente.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelarPaciente.ForeColor = Color.White;
            btnCancelarPaciente.Location = new Point(30, 415);
            btnCancelarPaciente.Name = "btnCancelarPaciente";
            btnCancelarPaciente.RadioBorde = 30;
            btnCancelarPaciente.Size = new Size(160, 42);
            btnCancelarPaciente.TabIndex = 14;
            btnCancelarPaciente.Text = "❌ Cancelar";
            btnCancelarPaciente.UseVisualStyleBackColor = false;
            btnCancelarPaciente.Click += btnCancelarPaciente_Click;
            // 
            // RegistrarPaciente_DNI101
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(235, 245, 238);
            ClientSize = new Size(580, 580);
            Controls.Add(panelCard);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RegistrarPaciente_DNI101";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Registrar Paciente - DNI101";
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
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblApellido;
        private TextBox txtApellido;
        private Label lblDni;
        private TextBox txtDniNiño;
        private Label lblTelefono;
        private TextBox txtTelefono;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblObraSocial;
        private ComboBox cmbObraSocial;
        private UI.Controles.BotonFuturista btnRegistrarPaciente;
        private UI.Controles.BotonFuturista btnCancelarPaciente;
    }
}
