namespace UI.PN2
{
    partial class frmBitacoraCambiosConsultas
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

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dgvStyleHeader = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvStyleRows = new System.Windows.Forms.DataGridViewCellStyle();

            panelHeader = new System.Windows.Forms.Panel();
            lblTitulo = new System.Windows.Forms.Label();
            lblSubtitulo = new System.Windows.Forms.Label();
            btnAyudaContextual = new UI.Controles.BotonFuturista();

            gbGrilla = new System.Windows.Forms.GroupBox();
            dgvCambios = new System.Windows.Forms.DataGridView();
            colCodCons = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colHora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDni = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colEdad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colAct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBtnActivar = new System.Windows.Forms.DataGridViewButtonColumn();

            gbControles = new System.Windows.Forms.GroupBox();
            lblCodProd = new System.Windows.Forms.Label();
            txtCodConsulta = new System.Windows.Forms.TextBox();
            lblNombre = new System.Windows.Forms.Label();
            txtNombre = new System.Windows.Forms.TextBox();
            lblFechaIni = new System.Windows.Forms.Label();
            dtpFechaIni = new System.Windows.Forms.DateTimePicker();
            lblFechaFin = new System.Windows.Forms.Label();
            dtpFechaFin = new System.Windows.Forms.DateTimePicker();
            btnAplicar = new UI.Controles.BotonFuturista();
            btnSalir = new UI.Controles.BotonFuturista();
            btnLimpiar = new UI.Controles.BotonFuturista();
            btnActivar = new UI.Controles.BotonFuturista();

            panelHeader.SuspendLayout();
            gbGrilla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCambios).BeginInit();
            gbControles.SuspendLayout();
            SuspendLayout();

            // 
            // panelHeader
            // 
            panelHeader.BackColor = System.Drawing.Color.FromArgb(76, 124, 89);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(lblSubtitulo);
            panelHeader.Controls.Add(btnAyudaContextual);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(980, 60);
            panelHeader.TabIndex = 0;

            // 
            // btnAyudaContextual
            // 
            btnAyudaContextual.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAyudaContextual.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnAyudaContextual.Location = new System.Drawing.Point(860, 12);
            btnAyudaContextual.Name = "btnAyudaContextual";
            btnAyudaContextual.Size = new System.Drawing.Size(100, 36);
            btnAyudaContextual.TabIndex = 2;
            btnAyudaContextual.Text = "? Ayuda";
            btnAyudaContextual.Click += btnAyudaContextual_Click;

            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblTitulo.ForeColor = System.Drawing.Color.White;
            lblTitulo.Location = new System.Drawing.Point(18, 10);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new System.Drawing.Size(560, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "BITÁCORA DE CAMBIOS - CONSULTAS NUTRICIONALES (CUN08)";

            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(235, 245, 235);
            lblSubtitulo.Location = new System.Drawing.Point(20, 35);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new System.Drawing.Size(585, 17);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Persistencia histórica de cambios en consultas de pacientes y activación de versiones";

            // 
            // gbGrilla
            // 
            gbGrilla.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            gbGrilla.Controls.Add(dgvCambios);
            gbGrilla.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            gbGrilla.ForeColor = System.Drawing.Color.FromArgb(40, 70, 50);
            gbGrilla.Location = new System.Drawing.Point(18, 68);
            gbGrilla.Name = "gbGrilla";
            gbGrilla.Size = new System.Drawing.Size(944, 380);
            gbGrilla.TabIndex = 1;
            gbGrilla.TabStop = false;
            gbGrilla.Text = "Registro Histórico de Cambios";

            // 
            // dgvCambios
            // 
            dgvCambios.AllowUserToAddRows = false;
            dgvCambios.AllowUserToDeleteRows = false;
            dgvCambios.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvCambios.BackgroundColor = System.Drawing.Color.White;
            dgvCambios.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dgvStyleHeader.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvStyleHeader.BackColor = System.Drawing.Color.FromArgb(76, 124, 89);
            dgvStyleHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            dgvStyleHeader.ForeColor = System.Drawing.Color.White;
            dgvCambios.ColumnHeadersDefaultCellStyle = dgvStyleHeader;
            dgvCambios.ColumnHeadersHeight = 30;
            dgvCambios.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                colCodCons,
                colFecha,
                colHora,
                colNombre,
                colDni,
                colEdad,
                colDesc,
                colAct,
                colBtnActivar
            });
            dgvCambios.EnableHeadersVisualStyles = false;
            dgvCambios.Location = new System.Drawing.Point(15, 25);
            dgvCambios.MultiSelect = false;
            dgvCambios.Name = "dgvCambios";
            dgvCambios.ReadOnly = true;
            dgvCambios.RowHeadersVisible = false;
            dgvStyleRows.Font = new System.Drawing.Font("Segoe UI", 9F);
            dgvCambios.RowsDefaultCellStyle = dgvStyleRows;
            dgvCambios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvCambios.Size = new System.Drawing.Size(914, 340);
            dgvCambios.TabIndex = 0;
            dgvCambios.CellContentClick += dgvCambios_CellContentClick;
            dgvCambios.CellFormatting += dgvCambios_CellFormatting;

            // 
            // colCodCons
            // 
            colCodCons.DataPropertyName = "IdConsulta";
            colCodCons.HeaderText = "Cod_Cons";
            colCodCons.Name = "colCodCons";
            colCodCons.ReadOnly = true;
            colCodCons.Width = 85;

            // 
            // colFecha
            // 
            colFecha.DataPropertyName = "FechaStr";
            colFecha.HeaderText = "Fecha";
            colFecha.Name = "colFecha";
            colFecha.ReadOnly = true;
            colFecha.Width = 85;

            // 
            // colHora
            // 
            colHora.DataPropertyName = "HoraStr";
            colHora.HeaderText = "Hora";
            colHora.Name = "colHora";
            colHora.ReadOnly = true;
            colHora.Width = 70;

            // 
            // colNombre
            // 
            colNombre.DataPropertyName = "NombrePaciente";
            colNombre.HeaderText = "Nombre Paciente";
            colNombre.Name = "colNombre";
            colNombre.ReadOnly = true;
            colNombre.Width = 170;

            // 
            // colDni
            // 
            colDni.DataPropertyName = "DniPaciente";
            colDni.HeaderText = "DNI";
            colDni.Name = "colDni";
            colDni.ReadOnly = true;
            colDni.Width = 85;

            // 
            // colEdad
            // 
            colEdad.DataPropertyName = "EdadMeses";
            colEdad.HeaderText = "Edad (m)";
            colEdad.Name = "colEdad";
            colEdad.ReadOnly = true;
            colEdad.Width = 75;

            // 
            // colDesc
            // 
            colDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colDesc.DataPropertyName = "Observaciones";
            colDesc.HeaderText = "Desc.";
            colDesc.Name = "colDesc";
            colDesc.ReadOnly = true;

            // 
            // colAct
            // 
            colAct.DataPropertyName = "ActivoInt";
            colAct.HeaderText = "Act.";
            colAct.Name = "colAct";
            colAct.ReadOnly = true;
            colAct.Width = 55;
            colAct.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // 
            // colBtnActivar
            // 
            colBtnActivar.HeaderText = "";
            colBtnActivar.Name = "colBtnActivar";
            colBtnActivar.ReadOnly = true;
            colBtnActivar.Text = "Activar";
            colBtnActivar.UseColumnTextForButtonValue = true;
            colBtnActivar.Width = 75;

            // 
            // gbControles
            // 
            gbControles.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            gbControles.Controls.Add(lblCodProd);
            gbControles.Controls.Add(txtCodConsulta);
            gbControles.Controls.Add(lblNombre);
            gbControles.Controls.Add(txtNombre);
            gbControles.Controls.Add(lblFechaIni);
            gbControles.Controls.Add(dtpFechaIni);
            gbControles.Controls.Add(lblFechaFin);
            gbControles.Controls.Add(dtpFechaFin);
            gbControles.Controls.Add(btnAplicar);
            gbControles.Controls.Add(btnSalir);
            gbControles.Controls.Add(btnLimpiar);
            gbControles.Controls.Add(btnActivar);
            gbControles.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            gbControles.ForeColor = System.Drawing.Color.FromArgb(40, 70, 50);
            gbControles.Location = new System.Drawing.Point(18, 456);
            gbControles.Name = "gbControles";
            gbControles.Size = new System.Drawing.Size(944, 125);
            gbControles.TabIndex = 2;
            gbControles.TabStop = false;
            gbControles.Text = "Filtros y Operaciones";

            // 
            // lblCodProd
            // 
            lblCodProd.AutoSize = true;
            lblCodProd.Location = new System.Drawing.Point(20, 30);
            lblCodProd.Name = "lblCodProd";
            lblCodProd.Size = new System.Drawing.Size(73, 17);
            lblCodProd.TabIndex = 0;
            lblCodProd.Text = "Cod_Cons.:";

            // 
            // txtCodConsulta
            // 
            txtCodConsulta.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txtCodConsulta.Location = new System.Drawing.Point(100, 27);
            txtCodConsulta.Name = "txtCodConsulta";
            txtCodConsulta.PlaceholderText = "ID Consulta";
            txtCodConsulta.Size = new System.Drawing.Size(110, 24);
            txtCodConsulta.TabIndex = 1;

            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new System.Drawing.Point(235, 30);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new System.Drawing.Size(61, 17);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre:";

            // 
            // txtNombre
            // 
            txtNombre.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txtNombre.Location = new System.Drawing.Point(305, 27);
            txtNombre.Name = "txtNombre";
            txtNombre.PlaceholderText = "Buscar por paciente";
            txtNombre.Size = new System.Drawing.Size(180, 24);
            txtNombre.TabIndex = 3;

            // 
            // lblFechaIni
            // 
            lblFechaIni.AutoSize = true;
            lblFechaIni.Location = new System.Drawing.Point(20, 75);
            lblFechaIni.Name = "lblFechaIni";
            lblFechaIni.Size = new System.Drawing.Size(68, 17);
            lblFechaIni.TabIndex = 4;
            lblFechaIni.Text = "Fecha Ini.:";

            // 
            // dtpFechaIni
            // 
            dtpFechaIni.Checked = false;
            dtpFechaIni.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dtpFechaIni.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpFechaIni.Location = new System.Drawing.Point(100, 72);
            dtpFechaIni.Name = "dtpFechaIni";
            dtpFechaIni.ShowCheckBox = true;
            dtpFechaIni.Size = new System.Drawing.Size(115, 24);
            dtpFechaIni.TabIndex = 5;

            // 
            // lblFechaFin
            // 
            lblFechaFin.AutoSize = true;
            lblFechaFin.Location = new System.Drawing.Point(235, 75);
            lblFechaFin.Name = "lblFechaFin";
            lblFechaFin.Size = new System.Drawing.Size(66, 17);
            lblFechaFin.TabIndex = 6;
            lblFechaFin.Text = "Fecha Fin:";

            // 
            // dtpFechaFin
            // 
            dtpFechaFin.Checked = false;
            dtpFechaFin.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpFechaFin.Location = new System.Drawing.Point(305, 72);
            dtpFechaFin.Name = "dtpFechaFin";
            dtpFechaFin.ShowCheckBox = true;
            dtpFechaFin.Size = new System.Drawing.Size(115, 24);
            dtpFechaFin.TabIndex = 7;

            // 
            // btnAplicar
            // 
            btnAplicar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAplicar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnAplicar.Location = new System.Drawing.Point(530, 22);
            btnAplicar.Name = "btnAplicar";
            btnAplicar.Size = new System.Drawing.Size(180, 40);
            btnAplicar.TabIndex = 8;
            btnAplicar.Text = "APLICAR";
            btnAplicar.Click += btnAplicar_Click;

            // 
            // btnSalir
            // 
            btnSalir.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSalir.EsBotonSecundario = true;
            btnSalir.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnSalir.Location = new System.Drawing.Point(730, 22);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new System.Drawing.Size(180, 40);
            btnSalir.TabIndex = 9;
            btnSalir.Text = "SALIR";
            btnSalir.Click += btnSalir_Click;

            // 
            // btnLimpiar
            // 
            btnLimpiar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnLimpiar.EsBotonSecundario = true;
            btnLimpiar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnLimpiar.Location = new System.Drawing.Point(530, 70);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new System.Drawing.Size(180, 40);
            btnLimpiar.TabIndex = 10;
            btnLimpiar.Text = "LIMPIAR";
            btnLimpiar.Click += btnLimpiar_Click;

            // 
            // btnActivar
            // 
            btnActivar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnActivar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnActivar.Location = new System.Drawing.Point(730, 70);
            btnActivar.Name = "btnActivar";
            btnActivar.Size = new System.Drawing.Size(180, 40);
            btnActivar.TabIndex = 11;
            btnActivar.Text = "ACTIVAR";
            btnActivar.Click += btnActivar_Click;

            // 
            // frmBitacoraCambiosConsultas
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 248, 245);
            ClientSize = new System.Drawing.Size(980, 600);
            Controls.Add(gbControles);
            Controls.Add(gbGrilla);
            Controls.Add(panelHeader);
            Name = "frmBitacoraCambiosConsultas";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Bitácora de Cambios - CUN08";
            Load += frmBitacoraCambiosConsultas_Load;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            gbGrilla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCambios).EndInit();
            gbControles.ResumeLayout(false);
            gbControles.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private UI.Controles.BotonFuturista btnAyudaContextual;

        private System.Windows.Forms.GroupBox gbGrilla;
        private System.Windows.Forms.DataGridView dgvCambios;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCodCons;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHora;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDni;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEdad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAct;
        private System.Windows.Forms.DataGridViewButtonColumn colBtnActivar;

        private System.Windows.Forms.GroupBox gbControles;
        private System.Windows.Forms.Label lblCodProd;
        private System.Windows.Forms.TextBox txtCodConsulta;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblFechaIni;
        private System.Windows.Forms.DateTimePicker dtpFechaIni;
        private System.Windows.Forms.Label lblFechaFin;
        private System.Windows.Forms.DateTimePicker dtpFechaFin;

        private UI.Controles.BotonFuturista btnAplicar;
        private UI.Controles.BotonFuturista btnSalir;
        private UI.Controles.BotonFuturista btnLimpiar;
        private UI.Controles.BotonFuturista btnActivar;
    }
}
