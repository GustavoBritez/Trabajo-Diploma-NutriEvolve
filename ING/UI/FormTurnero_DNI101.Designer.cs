using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    partial class FormTurnero_DNI101
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitulo = new Label();
            flowLayoutPanelMain = new FlowLayoutPanel();
            gbTutor = new GroupBox();
            btnBuscarTutor = new Button();
            cmbParentesco = new ComboBox();
            lblParentesco = new Label();
            txtEmail = new TextBox();
            lblEmail = new Label();
            txtTelefono = new TextBox();
            lblTelefono = new Label();
            txtApellidoTutor = new TextBox();
            lblApellidoTutor = new Label();
            txtNombreTutor = new TextBox();
            lblNombreTutor = new Label();
            txtDniTutor = new TextBox();
            lblDniTutor = new Label();
            gbPaciente = new GroupBox();
            btnBuscarPaciente = new Button();
            txtObraSocial = new TextBox();
            lblObraSocial = new Label();
            cmbSexo = new ComboBox();
            lblSexo = new Label();
            dtpFechaNac = new DateTimePicker();
            lblFechaNac = new Label();
            txtApellidoNiño = new TextBox();
            lblApellidoNiño = new Label();
            txtNombreNiño = new TextBox();
            lblNombreNiño = new Label();
            txtDniNiño = new TextBox();
            lblDniNiño = new Label();
            gbAgendaTurno = new GroupBox();
            txtMotivoConsulta = new TextBox();
            lblMotivoConsulta = new Label();
            cmbBloquesHorarios = new ComboBox();
            lblBloqueHorario = new Label();
            dtpFechaTurno = new DateTimePicker();
            lblFechaTurno = new Label();
            gbAcciones = new GroupBox();
            btnLimpiar = new Button();
            btnMarcarAsistencia = new Button();
            btnCancelar = new Button();
            btnReprogramar = new Button();
            btnConfirmar = new Button();
            btnAgendar = new Button();
            gbTurnos = new GroupBox();
            dgvTurnos = new DataGridView();
            panelFooter = new Panel();
            btnSalir = new Button();
            flowLayoutPanelMain.SuspendLayout();
            gbTutor.SuspendLayout();
            gbPaciente.SuspendLayout();
            gbAgendaTurno.SuspendLayout();
            gbAcciones.SuspendLayout();
            gbTurnos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTurnos).BeginInit();
            panelFooter.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(46, 94, 67);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(1244, 45);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "📅 Gestión de Turnos Nutricionales Pediátricos (RF1)";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanelMain
            // 
            flowLayoutPanelMain.AutoScroll = true;
            flowLayoutPanelMain.Controls.Add(gbTutor);
            flowLayoutPanelMain.Controls.Add(gbPaciente);
            flowLayoutPanelMain.Controls.Add(gbAgendaTurno);
            flowLayoutPanelMain.Controls.Add(gbAcciones);
            flowLayoutPanelMain.Controls.Add(gbTurnos);
            flowLayoutPanelMain.Dock = DockStyle.Fill;
            flowLayoutPanelMain.Location = new Point(0, 45);
            flowLayoutPanelMain.Name = "flowLayoutPanelMain";
            flowLayoutPanelMain.Padding = new Padding(10);
            flowLayoutPanelMain.Size = new Size(1244, 610);
            flowLayoutPanelMain.TabIndex = 1;
            // 
            // gbTutor
            // 
            gbTutor.BackColor = Color.FromArgb(225, 240, 228);
            gbTutor.Controls.Add(btnBuscarTutor);
            gbTutor.Controls.Add(cmbParentesco);
            gbTutor.Controls.Add(lblParentesco);
            gbTutor.Controls.Add(txtEmail);
            gbTutor.Controls.Add(lblEmail);
            gbTutor.Controls.Add(txtTelefono);
            gbTutor.Controls.Add(lblTelefono);
            gbTutor.Controls.Add(txtApellidoTutor);
            gbTutor.Controls.Add(lblApellidoTutor);
            gbTutor.Controls.Add(txtNombreTutor);
            gbTutor.Controls.Add(lblNombreTutor);
            gbTutor.Controls.Add(txtDniTutor);
            gbTutor.Controls.Add(lblDniTutor);
            gbTutor.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            gbTutor.ForeColor = Color.FromArgb(46, 94, 67);
            gbTutor.Location = new Point(13, 13);
            gbTutor.Margin = new Padding(3, 3, 10, 10);
            gbTutor.Name = "gbTutor";
            gbTutor.Size = new Size(590, 160);
            gbTutor.TabIndex = 1;
            gbTutor.TabStop = false;
            gbTutor.Text = "👨‍👩‍👧 Datos del Tutor";
            // 
            // btnBuscarTutor
            // 
            btnBuscarTutor.BackColor = Color.FromArgb(92, 145, 104);
            btnBuscarTutor.FlatAppearance.BorderSize = 0;
            btnBuscarTutor.FlatStyle = FlatStyle.Flat;
            btnBuscarTutor.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBuscarTutor.ForeColor = Color.White;
            btnBuscarTutor.Location = new Point(225, 25);
            btnBuscarTutor.Name = "btnBuscarTutor";
            btnBuscarTutor.Size = new Size(65, 25);
            btnBuscarTutor.TabIndex = 12;
            btnBuscarTutor.Text = "🔍 Buscar";
            btnBuscarTutor.UseVisualStyleBackColor = false;
            btnBuscarTutor.Click += btnBuscarTutor_Click;
            // 
            // cmbParentesco
            // 
            cmbParentesco.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbParentesco.Font = new Font("Segoe UI", 9F);
            cmbParentesco.FormattingEnabled = true;
            cmbParentesco.Items.AddRange(new object[] { "Madre", "Padre", "Tutor Legal", "Abuelo/a", "Otro" });
            cmbParentesco.Location = new Point(380, 115);
            cmbParentesco.Name = "cmbParentesco";
            cmbParentesco.Size = new Size(195, 23);
            cmbParentesco.TabIndex = 11;
            // 
            // lblParentesco
            // 
            lblParentesco.AutoSize = true;
            lblParentesco.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblParentesco.ForeColor = Color.Black;
            lblParentesco.Location = new Point(300, 118);
            lblParentesco.Name = "lblParentesco";
            lblParentesco.Size = new Size(68, 15);
            lblParentesco.TabIndex = 10;
            lblParentesco.Text = "Parentesco:";
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Segoe UI", 9F);
            txtEmail.Location = new Point(85, 115);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(205, 23);
            txtEmail.TabIndex = 9;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblEmail.ForeColor = Color.Black;
            lblEmail.Location = new Point(15, 118);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(39, 15);
            lblEmail.TabIndex = 8;
            lblEmail.Text = "Email:";
            // 
            // txtTelefono
            // 
            txtTelefono.Font = new Font("Segoe UI", 9F);
            txtTelefono.Location = new Point(380, 70);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(195, 23);
            txtTelefono.TabIndex = 7;
            // 
            // lblTelefono
            // 
            lblTelefono.AutoSize = true;
            lblTelefono.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblTelefono.ForeColor = Color.Black;
            lblTelefono.Location = new Point(300, 73);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(55, 15);
            lblTelefono.TabIndex = 6;
            lblTelefono.Text = "Teléfono:";
            // 
            // txtApellidoTutor
            // 
            txtApellidoTutor.Font = new Font("Segoe UI", 9F);
            txtApellidoTutor.Location = new Point(85, 70);
            txtApellidoTutor.Name = "txtApellidoTutor";
            txtApellidoTutor.Size = new Size(205, 23);
            txtApellidoTutor.TabIndex = 5;
            // 
            // lblApellidoTutor
            // 
            lblApellidoTutor.AutoSize = true;
            lblApellidoTutor.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblApellidoTutor.ForeColor = Color.Black;
            lblApellidoTutor.Location = new Point(15, 73);
            lblApellidoTutor.Name = "lblApellidoTutor";
            lblApellidoTutor.Size = new Size(54, 15);
            lblApellidoTutor.TabIndex = 4;
            lblApellidoTutor.Text = "Apellido:";
            // 
            // txtNombreTutor
            // 
            txtNombreTutor.Font = new Font("Segoe UI", 9F);
            txtNombreTutor.Location = new Point(380, 25);
            txtNombreTutor.Name = "txtNombreTutor";
            txtNombreTutor.Size = new Size(195, 23);
            txtNombreTutor.TabIndex = 3;
            // 
            // lblNombreTutor
            // 
            lblNombreTutor.AutoSize = true;
            lblNombreTutor.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblNombreTutor.ForeColor = Color.Black;
            lblNombreTutor.Location = new Point(300, 28);
            lblNombreTutor.Name = "lblNombreTutor";
            lblNombreTutor.Size = new Size(54, 15);
            lblNombreTutor.TabIndex = 2;
            lblNombreTutor.Text = "Nombre:";
            // 
            // txtDniTutor
            // 
            txtDniTutor.Font = new Font("Segoe UI", 9F);
            txtDniTutor.Location = new Point(85, 25);
            txtDniTutor.Name = "txtDniTutor";
            txtDniTutor.Size = new Size(130, 23);
            txtDniTutor.TabIndex = 1;
            // 
            // lblDniTutor
            // 
            lblDniTutor.AutoSize = true;
            lblDniTutor.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblDniTutor.ForeColor = Color.Black;
            lblDniTutor.Location = new Point(15, 28);
            lblDniTutor.Name = "lblDniTutor";
            lblDniTutor.Size = new Size(61, 15);
            lblDniTutor.TabIndex = 0;
            lblDniTutor.Text = "DNI Tutor:";
            // 
            // gbPaciente
            // 
            gbPaciente.BackColor = Color.FromArgb(225, 240, 228);
            gbPaciente.Controls.Add(btnBuscarPaciente);
            gbPaciente.Controls.Add(txtObraSocial);
            gbPaciente.Controls.Add(lblObraSocial);
            gbPaciente.Controls.Add(cmbSexo);
            gbPaciente.Controls.Add(lblSexo);
            gbPaciente.Controls.Add(dtpFechaNac);
            gbPaciente.Controls.Add(lblFechaNac);
            gbPaciente.Controls.Add(txtApellidoNiño);
            gbPaciente.Controls.Add(lblApellidoNiño);
            gbPaciente.Controls.Add(txtNombreNiño);
            gbPaciente.Controls.Add(lblNombreNiño);
            gbPaciente.Controls.Add(txtDniNiño);
            gbPaciente.Controls.Add(lblDniNiño);
            gbPaciente.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            gbPaciente.ForeColor = Color.FromArgb(46, 94, 67);
            gbPaciente.Location = new Point(616, 13);
            gbPaciente.Margin = new Padding(3, 3, 10, 10);
            gbPaciente.Name = "gbPaciente";
            gbPaciente.Size = new Size(590, 160);
            gbPaciente.TabIndex = 2;
            gbPaciente.TabStop = false;
            gbPaciente.Text = "👶 Datos del Paciente Pediátrico";
            // 
            // btnBuscarPaciente
            // 
            btnBuscarPaciente.BackColor = Color.FromArgb(92, 145, 104);
            btnBuscarPaciente.FlatAppearance.BorderSize = 0;
            btnBuscarPaciente.FlatStyle = FlatStyle.Flat;
            btnBuscarPaciente.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnBuscarPaciente.ForeColor = Color.White;
            btnBuscarPaciente.Location = new Point(225, 25);
            btnBuscarPaciente.Name = "btnBuscarPaciente";
            btnBuscarPaciente.Size = new Size(65, 25);
            btnBuscarPaciente.TabIndex = 12;
            btnBuscarPaciente.Text = "🔍 Buscar";
            btnBuscarPaciente.UseVisualStyleBackColor = false;
            btnBuscarPaciente.Click += btnBuscarPaciente_Click;
            // 
            // txtObraSocial
            // 
            txtObraSocial.Font = new Font("Segoe UI", 9F);
            txtObraSocial.Location = new Point(380, 115);
            txtObraSocial.Name = "txtObraSocial";
            txtObraSocial.Size = new Size(195, 23);
            txtObraSocial.TabIndex = 11;
            // 
            // lblObraSocial
            // 
            lblObraSocial.AutoSize = true;
            lblObraSocial.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblObraSocial.ForeColor = Color.Black;
            lblObraSocial.Location = new Point(300, 118);
            lblObraSocial.Name = "lblObraSocial";
            lblObraSocial.Size = new Size(70, 15);
            lblObraSocial.TabIndex = 10;
            lblObraSocial.Text = "Obra Social:";
            // 
            // cmbSexo
            // 
            cmbSexo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSexo.Font = new Font("Segoe UI", 9F);
            cmbSexo.FormattingEnabled = true;
            cmbSexo.Items.AddRange(new object[] { "Masculino", "Femenino", "Otro" });
            cmbSexo.Location = new Point(85, 115);
            cmbSexo.Name = "cmbSexo";
            cmbSexo.Size = new Size(205, 23);
            cmbSexo.TabIndex = 9;
            // 
            // lblSexo
            // 
            lblSexo.AutoSize = true;
            lblSexo.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblSexo.ForeColor = Color.Black;
            lblSexo.Location = new Point(15, 118);
            lblSexo.Name = "lblSexo";
            lblSexo.Size = new Size(35, 15);
            lblSexo.TabIndex = 8;
            lblSexo.Text = "Sexo:";
            // 
            // dtpFechaNac
            // 
            dtpFechaNac.Font = new Font("Segoe UI", 9F);
            dtpFechaNac.Format = DateTimePickerFormat.Short;
            dtpFechaNac.Location = new Point(380, 70);
            dtpFechaNac.Name = "dtpFechaNac";
            dtpFechaNac.Size = new Size(195, 23);
            dtpFechaNac.TabIndex = 7;
            // 
            // lblFechaNac
            // 
            lblFechaNac.AutoSize = true;
            lblFechaNac.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblFechaNac.ForeColor = Color.Black;
            lblFechaNac.Location = new Point(300, 73);
            lblFechaNac.Name = "lblFechaNac";
            lblFechaNac.Size = new Size(65, 15);
            lblFechaNac.TabIndex = 6;
            lblFechaNac.Text = "Fecha Nac:";
            // 
            // txtApellidoNiño
            // 
            txtApellidoNiño.Font = new Font("Segoe UI", 9F);
            txtApellidoNiño.Location = new Point(85, 70);
            txtApellidoNiño.Name = "txtApellidoNiño";
            txtApellidoNiño.Size = new Size(205, 23);
            txtApellidoNiño.TabIndex = 5;
            // 
            // lblApellidoNiño
            // 
            lblApellidoNiño.AutoSize = true;
            lblApellidoNiño.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblApellidoNiño.ForeColor = Color.Black;
            lblApellidoNiño.Location = new Point(15, 73);
            lblApellidoNiño.Name = "lblApellidoNiño";
            lblApellidoNiño.Size = new Size(54, 15);
            lblApellidoNiño.TabIndex = 4;
            lblApellidoNiño.Text = "Apellido:";
            // 
            // txtNombreNiño
            // 
            txtNombreNiño.Font = new Font("Segoe UI", 9F);
            txtNombreNiño.Location = new Point(380, 25);
            txtNombreNiño.Name = "txtNombreNiño";
            txtNombreNiño.Size = new Size(195, 23);
            txtNombreNiño.TabIndex = 3;
            // 
            // lblNombreNiño
            // 
            lblNombreNiño.AutoSize = true;
            lblNombreNiño.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblNombreNiño.ForeColor = Color.Black;
            lblNombreNiño.Location = new Point(300, 28);
            lblNombreNiño.Name = "lblNombreNiño";
            lblNombreNiño.Size = new Size(54, 15);
            lblNombreNiño.TabIndex = 2;
            lblNombreNiño.Text = "Nombre:";
            // 
            // txtDniNiño
            // 
            txtDniNiño.Font = new Font("Segoe UI", 9F);
            txtDniNiño.Location = new Point(85, 25);
            txtDniNiño.Name = "txtDniNiño";
            txtDniNiño.Size = new Size(130, 23);
            txtDniNiño.TabIndex = 1;
            // 
            // lblDniNiño
            // 
            lblDniNiño.AutoSize = true;
            lblDniNiño.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblDniNiño.ForeColor = Color.Black;
            lblDniNiño.Location = new Point(15, 28);
            lblDniNiño.Name = "lblDniNiño";
            lblDniNiño.Size = new Size(60, 15);
            lblDniNiño.TabIndex = 0;
            lblDniNiño.Text = "DNI Niño:";
            // 
            // gbAgendaTurno
            // 
            gbAgendaTurno.BackColor = Color.FromArgb(225, 240, 228);
            gbAgendaTurno.Controls.Add(txtMotivoConsulta);
            gbAgendaTurno.Controls.Add(lblMotivoConsulta);
            gbAgendaTurno.Controls.Add(cmbBloquesHorarios);
            gbAgendaTurno.Controls.Add(lblBloqueHorario);
            gbAgendaTurno.Controls.Add(dtpFechaTurno);
            gbAgendaTurno.Controls.Add(lblFechaTurno);
            gbAgendaTurno.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            gbAgendaTurno.ForeColor = Color.FromArgb(46, 94, 67);
            gbAgendaTurno.Location = new Point(13, 186);
            gbAgendaTurno.Margin = new Padding(3, 3, 10, 10);
            gbAgendaTurno.Name = "gbAgendaTurno";
            gbAgendaTurno.Size = new Size(590, 110);
            gbAgendaTurno.TabIndex = 3;
            gbAgendaTurno.TabStop = false;
            gbAgendaTurno.Text = "🕒 Programación del Turno";
            // 
            // txtMotivoConsulta
            // 
            txtMotivoConsulta.Font = new Font("Segoe UI", 9F);
            txtMotivoConsulta.Location = new Point(115, 68);
            txtMotivoConsulta.Name = "txtMotivoConsulta";
            txtMotivoConsulta.Size = new Size(460, 23);
            txtMotivoConsulta.TabIndex = 5;
            // 
            // lblMotivoConsulta
            // 
            lblMotivoConsulta.AutoSize = true;
            lblMotivoConsulta.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblMotivoConsulta.ForeColor = Color.Black;
            lblMotivoConsulta.Location = new Point(15, 71);
            lblMotivoConsulta.Name = "lblMotivoConsulta";
            lblMotivoConsulta.Size = new Size(96, 15);
            lblMotivoConsulta.TabIndex = 4;
            lblMotivoConsulta.Text = "Motivo Consulta:";
            // 
            // cmbBloquesHorarios
            // 
            cmbBloquesHorarios.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBloquesHorarios.Font = new Font("Segoe UI", 9F);
            cmbBloquesHorarios.FormattingEnabled = true;
            cmbBloquesHorarios.Location = new Point(380, 28);
            cmbBloquesHorarios.Name = "cmbBloquesHorarios";
            cmbBloquesHorarios.Size = new Size(195, 23);
            cmbBloquesHorarios.TabIndex = 3;
            // 
            // lblBloqueHorario
            // 
            lblBloqueHorario.AutoSize = true;
            lblBloqueHorario.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblBloqueHorario.ForeColor = Color.Black;
            lblBloqueHorario.Location = new Point(300, 31);
            lblBloqueHorario.Name = "lblBloqueHorario";
            lblBloqueHorario.Size = new Size(47, 15);
            lblBloqueHorario.TabIndex = 2;
            lblBloqueHorario.Text = "Horario:";
            // 
            // dtpFechaTurno
            // 
            dtpFechaTurno.Font = new Font("Segoe UI", 9F);
            dtpFechaTurno.Format = DateTimePickerFormat.Short;
            dtpFechaTurno.Location = new Point(115, 28);
            dtpFechaTurno.Name = "dtpFechaTurno";
            dtpFechaTurno.Size = new Size(175, 23);
            dtpFechaTurno.TabIndex = 1;
            // 
            // lblFechaTurno
            // 
            lblFechaTurno.AutoSize = true;
            lblFechaTurno.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblFechaTurno.ForeColor = Color.Black;
            lblFechaTurno.Location = new Point(15, 31);
            lblFechaTurno.Name = "lblFechaTurno";
            lblFechaTurno.Size = new Size(74, 15);
            lblFechaTurno.TabIndex = 0;
            lblFechaTurno.Text = "Fecha Turno:";
            // 
            // gbAcciones
            // 
            gbAcciones.BackColor = Color.FromArgb(225, 240, 228);
            gbAcciones.Controls.Add(btnLimpiar);
            gbAcciones.Controls.Add(btnMarcarAsistencia);
            gbAcciones.Controls.Add(btnCancelar);
            gbAcciones.Controls.Add(btnReprogramar);
            gbAcciones.Controls.Add(btnConfirmar);
            gbAcciones.Controls.Add(btnAgendar);
            gbAcciones.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            gbAcciones.ForeColor = Color.FromArgb(46, 94, 67);
            gbAcciones.Location = new Point(616, 186);
            gbAcciones.Margin = new Padding(3, 3, 10, 10);
            gbAcciones.Name = "gbAcciones";
            gbAcciones.Size = new Size(590, 110);
            gbAcciones.TabIndex = 4;
            gbAcciones.TabStop = false;
            gbAcciones.Text = "⚡ Panel de Acciones";
            // 
            // btnLimpiar
            // 
            btnLimpiar.BackColor = Color.FromArgb(225, 225, 225);
            btnLimpiar.FlatAppearance.BorderSize = 0;
            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLimpiar.ForeColor = Color.Black;
            btnLimpiar.Location = new Point(400, 65);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(175, 32);
            btnLimpiar.TabIndex = 5;
            btnLimpiar.Text = "🧹 Limpiar Campos";
            btnLimpiar.UseVisualStyleBackColor = false;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnMarcarAsistencia
            // 
            btnMarcarAsistencia.BackColor = Color.FromArgb(76, 124, 89);
            btnMarcarAsistencia.FlatAppearance.BorderSize = 0;
            btnMarcarAsistencia.FlatStyle = FlatStyle.Flat;
            btnMarcarAsistencia.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnMarcarAsistencia.ForeColor = Color.White;
            btnMarcarAsistencia.Location = new Point(205, 65);
            btnMarcarAsistencia.Name = "btnMarcarAsistencia";
            btnMarcarAsistencia.Size = new Size(180, 32);
            btnMarcarAsistencia.TabIndex = 4;
            btnMarcarAsistencia.Text = "✔️ Marcar Asistencia";
            btnMarcarAsistencia.UseVisualStyleBackColor = false;
            btnMarcarAsistencia.Click += btnMarcarAsistencia_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(200, 100, 100);
            btnCancelar.FlatAppearance.BorderSize = 0;
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(15, 65);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(175, 32);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "❌ Cancelar Turno";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnReprogramar
            // 
            btnReprogramar.BackColor = Color.FromArgb(76, 124, 89);
            btnReprogramar.FlatAppearance.BorderSize = 0;
            btnReprogramar.FlatStyle = FlatStyle.Flat;
            btnReprogramar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnReprogramar.ForeColor = Color.White;
            btnReprogramar.Location = new Point(400, 25);
            btnReprogramar.Name = "btnReprogramar";
            btnReprogramar.Size = new Size(175, 32);
            btnReprogramar.TabIndex = 2;
            btnReprogramar.Text = "🔄 Reprogramar";
            btnReprogramar.UseVisualStyleBackColor = false;
            btnReprogramar.Click += btnReprogramar_Click;
            // 
            // btnConfirmar
            // 
            btnConfirmar.BackColor = Color.FromArgb(76, 124, 89);
            btnConfirmar.FlatAppearance.BorderSize = 0;
            btnConfirmar.FlatStyle = FlatStyle.Flat;
            btnConfirmar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnConfirmar.ForeColor = Color.White;
            btnConfirmar.Location = new Point(205, 25);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(180, 32);
            btnConfirmar.TabIndex = 1;
            btnConfirmar.Text = "✅ Confirmar Turno";
            btnConfirmar.UseVisualStyleBackColor = false;
            btnConfirmar.Click += btnConfirmar_Click;
            // 
            // btnAgendar
            // 
            btnAgendar.BackColor = Color.FromArgb(76, 124, 89);
            btnAgendar.FlatAppearance.BorderSize = 0;
            btnAgendar.FlatStyle = FlatStyle.Flat;
            btnAgendar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnAgendar.ForeColor = Color.White;
            btnAgendar.Location = new Point(15, 25);
            btnAgendar.Name = "btnAgendar";
            btnAgendar.Size = new Size(175, 32);
            btnAgendar.TabIndex = 0;
            btnAgendar.Text = "➕ Agendar Turno";
            btnAgendar.UseVisualStyleBackColor = false;
            btnAgendar.Click += btnAgendar_Click;
            // 
            // gbTurnos
            // 
            gbTurnos.BackColor = Color.FromArgb(225, 240, 228);
            gbTurnos.Controls.Add(dgvTurnos);
            gbTurnos.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            gbTurnos.ForeColor = Color.FromArgb(46, 94, 67);
            gbTurnos.Location = new Point(13, 309);
            gbTurnos.Margin = new Padding(3, 3, 10, 10);
            gbTurnos.Name = "gbTurnos";
            gbTurnos.Size = new Size(1193, 280);
            gbTurnos.TabIndex = 5;
            gbTurnos.TabStop = false;
            gbTurnos.Text = "📋 Listado General de Turnos Registrados";
            // 
            // dgvTurnos
            // 
            dgvTurnos.AllowUserToAddRows = false;
            dgvTurnos.AllowUserToDeleteRows = false;
            dgvTurnos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTurnos.BackgroundColor = Color.White;
            dgvTurnos.BorderStyle = BorderStyle.Fixed3D;
            dgvTurnos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTurnos.Dock = DockStyle.Fill;
            dgvTurnos.GridColor = Color.FromArgb(200, 220, 205);
            dgvTurnos.Location = new Point(3, 20);
            dgvTurnos.MultiSelect = false;
            dgvTurnos.Name = "dgvTurnos";
            dgvTurnos.ReadOnly = true;
            dgvTurnos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTurnos.Size = new Size(1187, 257);
            dgvTurnos.TabIndex = 0;
            // 
            // panelFooter
            // 
            panelFooter.Controls.Add(btnSalir);
            panelFooter.Dock = DockStyle.Bottom;
            panelFooter.Location = new Point(0, 655);
            panelFooter.Name = "panelFooter";
            panelFooter.Size = new Size(1244, 50);
            panelFooter.TabIndex = 2;
            // 
            // btnSalir
            // 
            btnSalir.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSalir.BackColor = Color.FromArgb(255, 120, 120);
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSalir.ForeColor = Color.White;
            btnSalir.Location = new Point(1112, 8);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(120, 35);
            btnSalir.TabIndex = 6;
            btnSalir.Text = "🚪 Salir";
            btnSalir.UseVisualStyleBackColor = false;
            btnSalir.Click += btnSalir_Click;
            // 
            // FormTurnero_DNI101
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(218, 237, 223);
            ClientSize = new Size(1244, 705);
            Controls.Add(flowLayoutPanelMain);
            Controls.Add(panelFooter);
            Controls.Add(lblTitulo);
            MinimumSize = new Size(650, 500);
            Name = "FormTurnero_DNI101";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestión de Turnos - RF1";
            flowLayoutPanelMain.ResumeLayout(false);
            gbTutor.ResumeLayout(false);
            gbTutor.PerformLayout();
            gbPaciente.ResumeLayout(false);
            gbPaciente.PerformLayout();
            gbAgendaTurno.ResumeLayout(false);
            gbAgendaTurno.PerformLayout();
            gbAcciones.ResumeLayout(false);
            gbTurnos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTurnos).EndInit();
            panelFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label lblTitulo;
        private FlowLayoutPanel flowLayoutPanelMain;
        private GroupBox gbTutor;
        private Label lblDniTutor;
        private TextBox txtDniTutor;
        private Label lblNombreTutor;
        private TextBox txtNombreTutor;
        private Label lblApellidoTutor;
        private TextBox txtApellidoTutor;
        private Label lblTelefono;
        private TextBox txtTelefono;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblParentesco;
        private ComboBox cmbParentesco;
        private Button btnBuscarTutor;

        private GroupBox gbPaciente;
        private Label lblDniNiño;
        private TextBox txtDniNiño;
        private Label lblNombreNiño;
        private TextBox txtNombreNiño;
        private Label lblApellidoNiño;
        private TextBox txtApellidoNiño;
        private Label lblFechaNac;
        private DateTimePicker dtpFechaNac;
        private Label lblSexo;
        private ComboBox cmbSexo;
        private Label lblObraSocial;
        private TextBox txtObraSocial;
        private Button btnBuscarPaciente;

        private GroupBox gbAgendaTurno;
        private Label lblFechaTurno;
        private DateTimePicker dtpFechaTurno;
        private Label lblBloqueHorario;
        private ComboBox cmbBloquesHorarios;
        private Label lblMotivoConsulta;
        private TextBox txtMotivoConsulta;

        private GroupBox gbAcciones;
        private Button btnAgendar;
        private Button btnConfirmar;
        private Button btnReprogramar;
        private Button btnCancelar;
        private Button btnMarcarAsistencia;
        private Button btnLimpiar;

        private GroupBox gbTurnos;
        private DataGridView dgvTurnos;
        private Panel panelFooter;
        private Button btnSalir;
    }
}
