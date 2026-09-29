namespace UI
{
    partial class frmTurnero_DNI101
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            panelHeader = new Panel();
            lblTitulo = new Label();
            panelBotonesAccion = new UI.Controles.PanelOvalado();
            btn_Registrar_Turno = new UI.Controles.BotonFuturista();
            btn_Registrar_Paciente = new UI.Controles.BotonFuturista();
            btn_Reprogramar_Turno = new UI.Controles.BotonFuturista();
            btn_Modificar_Turno = new UI.Controles.BotonFuturista();
            btn_Cancelar_Turno = new UI.Controles.BotonFuturista();
            panelFiltros = new UI.Controles.PanelOvalado();
            chkFiltrarFecha = new CheckBox();
            dtpFiltroFecha = new DateTimePicker();
            lblEstado = new Label();
            cmbFiltroEstado = new ComboBox();
            btnActualizar = new UI.Controles.BotonFuturista();
            panelGrilla = new UI.Controles.PanelOvalado();
            dgvTurnos = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colCodigo = new DataGridViewTextBoxColumn();
            colFecha = new DataGridViewTextBoxColumn();
            colHora = new DataGridViewTextBoxColumn();
            colDniNiño = new DataGridViewTextBoxColumn();
            colPaciente = new DataGridViewTextBoxColumn();
            colObraSocial = new DataGridViewTextBoxColumn();
            colMotivo = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            panelHeader.SuspendLayout();
            panelBotonesAccion.SuspendLayout();
            panelFiltros.SuspendLayout();
            panelGrilla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTurnos).BeginInit();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(76, 124, 89);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(950, 60);
            panelHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(25, 16);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(365, 28);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "📅 PN1: Turnero Nutricional Pediátrico";
            // 
            // panelBotonesAccion
            // 
            panelBotonesAccion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelBotonesAccion.BackColor = Color.White;
            panelBotonesAccion.ColorBorde = Color.FromArgb(190, 215, 195);
            panelBotonesAccion.Controls.Add(btn_Registrar_Turno);
            panelBotonesAccion.Controls.Add(btn_Registrar_Paciente);
            panelBotonesAccion.Controls.Add(btn_Reprogramar_Turno);
            panelBotonesAccion.Controls.Add(btn_Modificar_Turno);
            panelBotonesAccion.Controls.Add(btn_Cancelar_Turno);
            panelBotonesAccion.DibujarBorde = true;
            panelBotonesAccion.Location = new Point(20, 75);
            panelBotonesAccion.Name = "panelBotonesAccion";
            panelBotonesAccion.RadioBorde = 24;
            panelBotonesAccion.Size = new Size(910, 70);
            panelBotonesAccion.TabIndex = 1;
            // 
            // btn_Registrar_Turno
            // 
            btn_Registrar_Turno.ColorGlow = Color.DarkSeaGreen;
            btn_Registrar_Turno.ColorPrincipal = Color.FromArgb(76, 124, 89);
            btn_Registrar_Turno.ColorSecundario = Color.FromArgb(50, 85, 60);
            btn_Registrar_Turno.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btn_Registrar_Turno.ForeColor = Color.White;
            btn_Registrar_Turno.Location = new Point(15, 14);
            btn_Registrar_Turno.Name = "btn_Registrar_Turno";
            btn_Registrar_Turno.RadioBorde = 28;
            btn_Registrar_Turno.Size = new Size(165, 42);
            btn_Registrar_Turno.TabIndex = 0;
            btn_Registrar_Turno.Text = "➕ Registrar Turno";
            btn_Registrar_Turno.UseVisualStyleBackColor = false;
            btn_Registrar_Turno.Click += btn_Registrar_Turno_Click;
            // 
            // btn_Registrar_Paciente
            // 
            btn_Registrar_Paciente.ColorGlow = Color.DarkSeaGreen;
            btn_Registrar_Paciente.ColorPrincipal = Color.FromArgb(92, 145, 104);
            btn_Registrar_Paciente.ColorSecundario = Color.FromArgb(60, 105, 75);
            btn_Registrar_Paciente.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btn_Registrar_Paciente.ForeColor = Color.White;
            btn_Registrar_Paciente.Location = new Point(190, 14);
            btn_Registrar_Paciente.Name = "btn_Registrar_Paciente";
            btn_Registrar_Paciente.RadioBorde = 28;
            btn_Registrar_Paciente.Size = new Size(165, 42);
            btn_Registrar_Paciente.TabIndex = 1;
            btn_Registrar_Paciente.Text = "👤 Registrar Paciente";
            btn_Registrar_Paciente.UseVisualStyleBackColor = false;
            btn_Registrar_Paciente.Click += btn_Registrar_Paciente_Click;
            // 
            // btn_Reprogramar_Turno
            // 
            btn_Reprogramar_Turno.ColorGlow = Color.LightSkyBlue;
            btn_Reprogramar_Turno.ColorPrincipal = Color.FromArgb(70, 115, 140);
            btn_Reprogramar_Turno.ColorSecundario = Color.FromArgb(45, 80, 100);
            btn_Reprogramar_Turno.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btn_Reprogramar_Turno.ForeColor = Color.White;
            btn_Reprogramar_Turno.Location = new Point(365, 14);
            btn_Reprogramar_Turno.Name = "btn_Reprogramar_Turno";
            btn_Reprogramar_Turno.RadioBorde = 28;
            btn_Reprogramar_Turno.Size = new Size(170, 42);
            btn_Reprogramar_Turno.TabIndex = 2;
            btn_Reprogramar_Turno.Text = "🔄 Reprogramar Turno";
            btn_Reprogramar_Turno.UseVisualStyleBackColor = false;
            btn_Reprogramar_Turno.Click += btn_Reprogramar_Turno_Click;
            // 
            // btn_Modificar_Turno
            // 
            btn_Modificar_Turno.ColorGlow = Color.Khaki;
            btn_Modificar_Turno.ColorPrincipal = Color.FromArgb(145, 130, 65);
            btn_Modificar_Turno.ColorSecundario = Color.FromArgb(105, 95, 45);
            btn_Modificar_Turno.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btn_Modificar_Turno.ForeColor = Color.White;
            btn_Modificar_Turno.Location = new Point(545, 14);
            btn_Modificar_Turno.Name = "btn_Modificar_Turno";
            btn_Modificar_Turno.RadioBorde = 28;
            btn_Modificar_Turno.Size = new Size(165, 42);
            btn_Modificar_Turno.TabIndex = 3;
            btn_Modificar_Turno.Text = "✏️ Modificar Turno";
            btn_Modificar_Turno.UseVisualStyleBackColor = false;
            btn_Modificar_Turno.Click += btn_Modificar_Turno_Click;
            // 
            // btn_Cancelar_Turno
            // 
            btn_Cancelar_Turno.ColorGlow = Color.FromArgb(200, 120, 120);
            btn_Cancelar_Turno.ColorPrincipal = Color.FromArgb(160, 75, 75);
            btn_Cancelar_Turno.ColorSecundario = Color.FromArgb(120, 45, 45);
            btn_Cancelar_Turno.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btn_Cancelar_Turno.ForeColor = Color.White;
            btn_Cancelar_Turno.Location = new Point(720, 14);
            btn_Cancelar_Turno.Name = "btn_Cancelar_Turno";
            btn_Cancelar_Turno.RadioBorde = 28;
            btn_Cancelar_Turno.Size = new Size(165, 42);
            btn_Cancelar_Turno.TabIndex = 4;
            btn_Cancelar_Turno.Text = "❌ Cancelar Turno";
            btn_Cancelar_Turno.UseVisualStyleBackColor = false;
            btn_Cancelar_Turno.Click += btn_Cancelar_Turno_Click;
            // 
            // panelFiltros
            // 
            panelFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelFiltros.BackColor = Color.White;
            panelFiltros.ColorBorde = Color.FromArgb(190, 215, 195);
            panelFiltros.Controls.Add(chkFiltrarFecha);
            panelFiltros.Controls.Add(dtpFiltroFecha);
            panelFiltros.Controls.Add(lblEstado);
            panelFiltros.Controls.Add(cmbFiltroEstado);
            panelFiltros.Controls.Add(btnActualizar);
            panelFiltros.DibujarBorde = true;
            panelFiltros.Location = new Point(20, 155);
            panelFiltros.Name = "panelFiltros";
            panelFiltros.RadioBorde = 24;
            panelFiltros.Size = new Size(910, 55);
            panelFiltros.TabIndex = 2;
            // 
            // chkFiltrarFecha
            // 
            chkFiltrarFecha.AutoSize = true;
            chkFiltrarFecha.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            chkFiltrarFecha.ForeColor = Color.FromArgb(40, 70, 45);
            chkFiltrarFecha.Location = new Point(20, 16);
            chkFiltrarFecha.Name = "chkFiltrarFecha";
            chkFiltrarFecha.Size = new Size(130, 21);
            chkFiltrarFecha.TabIndex = 0;
            chkFiltrarFecha.Text = "Filtrar por Fecha:";
            chkFiltrarFecha.UseVisualStyleBackColor = true;
            chkFiltrarFecha.CheckedChanged += chkFiltrarFecha_CheckedChanged;
            // 
            // dtpFiltroFecha
            // 
            dtpFiltroFecha.CustomFormat = "dd/MM/yyyy";
            dtpFiltroFecha.Enabled = false;
            dtpFiltroFecha.Font = new Font("Segoe UI", 9.5F);
            dtpFiltroFecha.Format = DateTimePickerFormat.Custom;
            dtpFiltroFecha.Location = new Point(155, 14);
            dtpFiltroFecha.Name = "dtpFiltroFecha";
            dtpFiltroFecha.Size = new Size(140, 24);
            dtpFiltroFecha.TabIndex = 1;
            dtpFiltroFecha.ValueChanged += dtpFiltroFecha_ValueChanged;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblEstado.ForeColor = Color.FromArgb(40, 70, 45);
            lblEstado.Location = new Point(320, 17);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(52, 17);
            lblEstado.TabIndex = 2;
            lblEstado.Text = "Estado:";
            // 
            // cmbFiltroEstado
            // 
            cmbFiltroEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltroEstado.Font = new Font("Segoe UI", 9.5F);
            cmbFiltroEstado.FormattingEnabled = true;
            cmbFiltroEstado.Items.AddRange(new object[] { "Todos", "Solicitado", "Confirmado", "Asistió", "Cancelado" });
            cmbFiltroEstado.Location = new Point(378, 14);
            cmbFiltroEstado.Name = "cmbFiltroEstado";
            cmbFiltroEstado.Size = new Size(150, 24);
            cmbFiltroEstado.TabIndex = 3;
            cmbFiltroEstado.SelectedIndexChanged += cmbFiltroEstado_SelectedIndexChanged;
            // 
            // btnActualizar
            // 
            btnActualizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnActualizar.ColorGlow = Color.DarkSeaGreen;
            btnActualizar.ColorPrincipal = Color.FromArgb(76, 124, 89);
            btnActualizar.ColorSecundario = Color.FromArgb(50, 85, 60);
            btnActualizar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnActualizar.ForeColor = Color.White;
            btnActualizar.Location = new Point(745, 10);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.RadioBorde = 24;
            btnActualizar.Size = new Size(145, 35);
            btnActualizar.TabIndex = 4;
            btnActualizar.Text = "🔄 Actualizar";
            btnActualizar.UseVisualStyleBackColor = false;
            btnActualizar.Click += btnActualizar_Click;
            // 
            // panelGrilla
            // 
            panelGrilla.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelGrilla.BackColor = Color.White;
            panelGrilla.ColorBorde = Color.FromArgb(190, 215, 195);
            panelGrilla.Controls.Add(dgvTurnos);
            panelGrilla.DibujarBorde = true;
            panelGrilla.Location = new Point(20, 220);
            panelGrilla.Name = "panelGrilla";
            panelGrilla.RadioBorde = 24;
            panelGrilla.Size = new Size(910, 395);
            panelGrilla.TabIndex = 3;
            // 
            // dgvTurnos
            // 
            dgvTurnos.AllowUserToAddRows = false;
            dgvTurnos.AllowUserToDeleteRows = false;
            dgvTurnos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTurnos.BackgroundColor = Color.White;
            dgvTurnos.BorderStyle = BorderStyle.None;
            dgvTurnos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(76, 124, 89);
            dataGridViewCellStyle1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(76, 124, 89);
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvTurnos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvTurnos.ColumnHeadersHeight = 35;
            dgvTurnos.Columns.AddRange(new DataGridViewColumn[] { colId, colCodigo, colFecha, colHora, colDniNiño, colPaciente, colObraSocial, colMotivo, colEstado });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.5F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(40, 50, 45);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(200, 230, 210);
            dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvTurnos.DefaultCellStyle = dataGridViewCellStyle2;
            dgvTurnos.Dock = DockStyle.Fill;
            dgvTurnos.EnableHeadersVisualStyles = false;
            dgvTurnos.Location = new Point(0, 0);
            dgvTurnos.MultiSelect = false;
            dgvTurnos.Name = "dgvTurnos";
            dgvTurnos.ReadOnly = true;
            dgvTurnos.RowHeadersVisible = false;
            dgvTurnos.RowTemplate.Height = 30;
            dgvTurnos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTurnos.Size = new Size(910, 395);
            dgvTurnos.TabIndex = 0;
            // 
            // colId
            // 
            colId.DataPropertyName = "IdTurno_DNI101";
            colId.FillWeight = 40F;
            colId.HeaderText = "ID";
            colId.Name = "colId";
            colId.ReadOnly = true;
            // 
            // colCodigo
            // 
            colCodigo.DataPropertyName = "CodigoTurno_DNI101";
            colCodigo.FillWeight = 85F;
            colCodigo.HeaderText = "Código";
            colCodigo.Name = "colCodigo";
            colCodigo.ReadOnly = true;
            // 
            // colFecha
            // 
            colFecha.DataPropertyName = "FechaFormateada";
            colFecha.FillWeight = 65F;
            colFecha.HeaderText = "Fecha";
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            // 
            // colHora
            // 
            colHora.DataPropertyName = "HoraFormateada";
            colHora.FillWeight = 50F;
            colHora.HeaderText = "Hora";
            colHora.Name = "colHora";
            colHora.ReadOnly = true;
            // 
            // colDniNiño
            // 
            colDniNiño.DataPropertyName = "DniNiño";
            colDniNiño.FillWeight = 65F;
            colDniNiño.HeaderText = "DNI Niño";
            colDniNiño.Name = "colDniNiño";
            colDniNiño.ReadOnly = true;
            // 
            // colPaciente
            // 
            colPaciente.DataPropertyName = "NombrePaciente";
            colPaciente.FillWeight = 110F;
            colPaciente.HeaderText = "Paciente";
            colPaciente.Name = "colPaciente";
            colPaciente.ReadOnly = true;
            // 
            // colObraSocial
            // 
            colObraSocial.DataPropertyName = "ObraSocial";
            colObraSocial.FillWeight = 65F;
            colObraSocial.HeaderText = "Obra Social";
            colObraSocial.Name = "colObraSocial";
            colObraSocial.ReadOnly = true;
            // 
            // colMotivo
            // 
            colMotivo.DataPropertyName = "MotivoConsulta_DNI101";
            colMotivo.FillWeight = 120F;
            colMotivo.HeaderText = "Motivo Consulta";
            colMotivo.Name = "colMotivo";
            colMotivo.ReadOnly = true;
            // 
            // colEstado
            // 
            colEstado.DataPropertyName = "EstadoTurno_DNI101";
            colEstado.FillWeight = 65F;
            colEstado.HeaderText = "Estado";
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            // 
            // frmTurnero_DNI101
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(225, 240, 228);
            ClientSize = new Size(950, 630);
            Controls.Add(panelGrilla);
            Controls.Add(panelFiltros);
            Controls.Add(panelBotonesAccion);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmTurnero_DNI101";
            Text = "Turnero Nutricional - DNI101";
            Load += frmTurnero_DNI101_Load;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelBotonesAccion.ResumeLayout(false);
            panelFiltros.ResumeLayout(false);
            panelFiltros.PerformLayout();
            panelGrilla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTurnos).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTitulo;
        private UI.Controles.PanelOvalado panelBotonesAccion;
        private UI.Controles.BotonFuturista btn_Registrar_Turno;
        private UI.Controles.BotonFuturista btn_Registrar_Paciente;
        private UI.Controles.BotonFuturista btn_Reprogramar_Turno;
        private UI.Controles.BotonFuturista btn_Modificar_Turno;
        private UI.Controles.BotonFuturista btn_Cancelar_Turno;
        private UI.Controles.PanelOvalado panelFiltros;
        private CheckBox chkFiltrarFecha;
        private DateTimePicker dtpFiltroFecha;
        private Label lblEstado;
        private ComboBox cmbFiltroEstado;
        private UI.Controles.BotonFuturista btnActualizar;
        private UI.Controles.PanelOvalado panelGrilla;
        private DataGridView dgvTurnos;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colCodigo;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colHora;
        private DataGridViewTextBoxColumn colDniNiño;
        private DataGridViewTextBoxColumn colPaciente;
        private DataGridViewTextBoxColumn colObraSocial;
        private DataGridViewTextBoxColumn colMotivo;
        private DataGridViewTextBoxColumn colEstado;
    }
}
