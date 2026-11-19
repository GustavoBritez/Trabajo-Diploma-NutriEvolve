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
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dgvStyleHeader = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvStyleRows = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvStyleHeaderPac = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvStyleRowsPac = new System.Windows.Forms.DataGridViewCellStyle();

            toolTipGeneral = new System.Windows.Forms.ToolTip(components);

            panelHeader = new System.Windows.Forms.Panel();
            lblTitulo = new System.Windows.Forms.Label();
            lblSubtitulo = new System.Windows.Forms.Label();
            lblModulo = new System.Windows.Forms.Label();
            cmbModulo = new System.Windows.Forms.ComboBox();
            btnAyudaContextual = new UI.Controles.BotonFuturista();

            gbGrilla = new System.Windows.Forms.GroupBox();
            
            // Grilla Maestro Clientes
            dgvCambiosPacientes = new System.Windows.Forms.DataGridView();
            colPacIdCambio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPacFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPacHora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPacDni = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPacCliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPacEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPacCelular = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPacDireccion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPacOperacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPacUsuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPacAct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPacBtnActivar = new System.Windows.Forms.DataGridViewButtonColumn();

            // Grilla Consultas Nutricionales
            dgvCambios = new System.Windows.Forms.DataGridView();
            colCodCons = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colHora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDni = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colEdad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colPeso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colTalla = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colClasif = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colAct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBtnActivar = new System.Windows.Forms.DataGridViewButtonColumn();

            gbControles = new System.Windows.Forms.GroupBox();
            lblCodProd = new System.Windows.Forms.Label();
            txtCodConsulta = new System.Windows.Forms.TextBox();
            lblNombre = new System.Windows.Forms.Label();
            txtNombre = new System.Windows.Forms.TextBox();
            lblOperacion = new System.Windows.Forms.Label();
            cmbOperacion = new System.Windows.Forms.ComboBox();
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
            ((System.ComponentModel.ISupportInitialize)dgvCambiosPacientes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCambios).BeginInit();
            gbControles.SuspendLayout();
            SuspendLayout();

            // 
            // panelHeader
            // 
            panelHeader.BackColor = System.Drawing.Color.FromArgb(76, 124, 89);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(lblSubtitulo);
            panelHeader.Controls.Add(lblModulo);
            panelHeader.Controls.Add(cmbModulo);
            panelHeader.Controls.Add(btnAyudaContextual);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(980, 60);
            panelHeader.TabIndex = 0;

            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold);
            lblTitulo.ForeColor = System.Drawing.Color.White;
            lblTitulo.Location = new System.Drawing.Point(18, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new System.Drawing.Size(460, 23);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "BITÁCORA DE CAMBIOS - MAESTRO DE CLIENTES (ABM)";

            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(235, 245, 235);
            lblSubtitulo.Location = new System.Drawing.Point(20, 35);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new System.Drawing.Size(490, 15);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Persistencia histórica de cambios, bajas lógicas y activación de versiones de clientes";

            // 
            // lblModulo
            // 
            lblModulo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblModulo.AutoSize = true;
            lblModulo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblModulo.ForeColor = System.Drawing.Color.White;
            lblModulo.Location = new System.Drawing.Point(585, 21);
            lblModulo.Name = "lblModulo";
            lblModulo.Size = new System.Drawing.Size(53, 15);
            lblModulo.TabIndex = 2;
            lblModulo.Text = "Módulo:";

            // 
            // cmbModulo
            // 
            cmbModulo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            cmbModulo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbModulo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular);
            cmbModulo.FormattingEnabled = true;
            cmbModulo.Items.AddRange(new object[] {
                "Maestro de Clientes (ABM)",
                "Consultas Nutricionales (CUN08)"
            });
            cmbModulo.Location = new System.Drawing.Point(645, 17);
            cmbModulo.Name = "cmbModulo";
            cmbModulo.Size = new System.Drawing.Size(205, 23);
            cmbModulo.TabIndex = 3;
            toolTipGeneral.SetToolTip(cmbModulo, "Selecciona el módulo para auditar sus cambios históricos.");
            cmbModulo.SelectedIndexChanged += cmbModulo_SelectedIndexChanged;

            // 
            // btnAyudaContextual
            // 
            btnAyudaContextual.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAyudaContextual.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnAyudaContextual.Location = new System.Drawing.Point(860, 12);
            btnAyudaContextual.Name = "btnAyudaContextual";
            btnAyudaContextual.Size = new System.Drawing.Size(100, 36);
            btnAyudaContextual.TabIndex = 4;
            btnAyudaContextual.Text = "? Ayuda";
            toolTipGeneral.SetToolTip(btnAyudaContextual, "Ayuda: Consulta el manual del sistema sobre Bitácora de Cambios y Rollback (Capítulo 5).");
            btnAyudaContextual.Click += btnAyudaContextual_Click;

            // 
            // gbGrilla
            // 
            gbGrilla.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            gbGrilla.Controls.Add(dgvCambiosPacientes);
            gbGrilla.Controls.Add(dgvCambios);
            gbGrilla.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            gbGrilla.ForeColor = System.Drawing.Color.FromArgb(40, 70, 50);
            gbGrilla.Location = new System.Drawing.Point(18, 68);
            gbGrilla.Name = "gbGrilla";
            gbGrilla.Size = new System.Drawing.Size(944, 380);
            gbGrilla.TabIndex = 1;
            gbGrilla.TabStop = false;
            gbGrilla.Text = "Registro Histórico de Cambios - Maestro de Clientes";

            // 
            // dgvCambiosPacientes
            // 
            dgvCambiosPacientes.AllowUserToAddRows = false;
            dgvCambiosPacientes.AllowUserToDeleteRows = false;
            dgvCambiosPacientes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvCambiosPacientes.BackgroundColor = System.Drawing.Color.White;
            dgvCambiosPacientes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dgvStyleHeaderPac.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvStyleHeaderPac.BackColor = System.Drawing.Color.FromArgb(76, 124, 89);
            dgvStyleHeaderPac.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            dgvStyleHeaderPac.ForeColor = System.Drawing.Color.White;
            dgvCambiosPacientes.ColumnHeadersDefaultCellStyle = dgvStyleHeaderPac;
            dgvCambiosPacientes.ColumnHeadersHeight = 30;
            dgvCambiosPacientes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                colPacIdCambio,
                colPacFecha,
                colPacHora,
                colPacDni,
                colPacCliente,
                colPacEmail,
                colPacCelular,
                colPacDireccion,
                colPacOperacion,
                colPacUsuario,
                colPacAct,
                colPacBtnActivar
            });
            dgvCambiosPacientes.EnableHeadersVisualStyles = false;
            dgvCambiosPacientes.Location = new System.Drawing.Point(15, 25);
            dgvCambiosPacientes.MultiSelect = false;
            dgvCambiosPacientes.Name = "dgvCambiosPacientes";
            dgvCambiosPacientes.ReadOnly = true;
            dgvCambiosPacientes.RowHeadersVisible = false;
            dgvStyleRowsPac.Font = new System.Drawing.Font("Segoe UI", 9F);
            dgvCambiosPacientes.RowsDefaultCellStyle = dgvStyleRowsPac;
            dgvCambiosPacientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvCambiosPacientes.Size = new System.Drawing.Size(914, 340);
            dgvCambiosPacientes.TabIndex = 0;
            dgvCambiosPacientes.CellContentClick += dgvCambiosPacientes_CellContentClick;
            dgvCambiosPacientes.CellFormatting += dgvCambiosPacientes_CellFormatting;

            // 
            // colPacIdCambio
            // 
            colPacIdCambio.DataPropertyName = "IdCambio";
            colPacIdCambio.HeaderText = "ID";
            colPacIdCambio.Name = "colPacIdCambio";
            colPacIdCambio.ReadOnly = true;
            colPacIdCambio.Width = 55;

            // 
            // colPacFecha
            // 
            colPacFecha.DataPropertyName = "FechaStr";
            colPacFecha.HeaderText = "Fecha";
            colPacFecha.Name = "colPacFecha";
            colPacFecha.ReadOnly = true;
            colPacFecha.Width = 80;

            // 
            // colPacHora
            // 
            colPacHora.DataPropertyName = "HoraStr";
            colPacHora.HeaderText = "Hora";
            colPacHora.Name = "colPacHora";
            colPacHora.ReadOnly = true;
            colPacHora.Width = 65;

            // 
            // colPacDni
            // 
            colPacDni.DataPropertyName = "Dni";
            colPacDni.HeaderText = "DNI";
            colPacDni.Name = "colPacDni";
            colPacDni.ReadOnly = true;
            colPacDni.Width = 80;

            // 
            // colPacCliente
            // 
            colPacCliente.DataPropertyName = "Cliente";
            colPacCliente.HeaderText = "Cliente / Paciente";
            colPacCliente.Name = "colPacCliente";
            colPacCliente.ReadOnly = true;
            colPacCliente.Width = 135;

            // 
            // colPacEmail
            // 
            colPacEmail.DataPropertyName = "Email";
            colPacEmail.HeaderText = "Email";
            colPacEmail.Name = "colPacEmail";
            colPacEmail.ReadOnly = true;
            colPacEmail.Width = 130;

            // 
            // colPacCelular
            // 
            colPacCelular.DataPropertyName = "Telefono";
            colPacCelular.HeaderText = "Celular";
            colPacCelular.Name = "colPacCelular";
            colPacCelular.ReadOnly = true;
            colPacCelular.Width = 90;

            // 
            // colPacDireccion
            // 
            colPacDireccion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colPacDireccion.DataPropertyName = "Direccion";
            colPacDireccion.HeaderText = "Dirección";
            colPacDireccion.Name = "colPacDireccion";
            colPacDireccion.ReadOnly = true;

            // 
            // colPacOperacion
            // 
            colPacOperacion.DataPropertyName = "OperacionStr";
            colPacOperacion.HeaderText = "Operación";
            colPacOperacion.Name = "colPacOperacion";
            colPacOperacion.ReadOnly = true;
            colPacOperacion.Width = 100;

            // 
            // colPacUsuario
            // 
            colPacUsuario.DataPropertyName = "UsuarioDni";
            colPacUsuario.HeaderText = "Usuario";
            colPacUsuario.Name = "colPacUsuario";
            colPacUsuario.ReadOnly = true;
            colPacUsuario.Width = 70;

            // 
            // colPacAct
            // 
            colPacAct.DataPropertyName = "Act";
            colPacAct.HeaderText = "Act.";
            colPacAct.Name = "colPacAct";
            colPacAct.ReadOnly = true;
            colPacAct.Width = 45;
            colPacAct.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;

            // 
            // colPacBtnActivar
            // 
            colPacBtnActivar.HeaderText = "";
            colPacBtnActivar.Name = "colPacBtnActivar";
            colPacBtnActivar.ReadOnly = true;
            colPacBtnActivar.Text = "Activar";
            colPacBtnActivar.UseColumnTextForButtonValue = true;
            colPacBtnActivar.Width = 70;

            // 
            // dgvCambios (Consultas Nutricionales)
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
                colPeso,
                colTalla,
                colClasif,
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
            dgvCambios.TabIndex = 1;
            dgvCambios.Visible = false;
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
            // colPeso
            // 
            colPeso.DataPropertyName = "PesoStr";
            colPeso.HeaderText = "Peso (kg)";
            colPeso.Name = "colPeso";
            colPeso.ReadOnly = true;
            colPeso.Width = 75;
            colPeso.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;

            // 
            // colTalla
            // 
            colTalla.DataPropertyName = "TallaStr";
            colTalla.HeaderText = "Talla (cm)";
            colTalla.Name = "colTalla";
            colTalla.ReadOnly = true;
            colTalla.Width = 75;
            colTalla.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;

            // 
            // colClasif
            // 
            colClasif.DataPropertyName = "ClasificacionOMS";
            colClasif.HeaderText = "Diagnóstico OMS";
            colClasif.Name = "colClasif";
            colClasif.ReadOnly = true;
            colClasif.Width = 125;

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
            gbControles.Controls.Add(lblOperacion);
            gbControles.Controls.Add(cmbOperacion);
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
            lblCodProd.Location = new System.Drawing.Point(15, 30);
            lblCodProd.Name = "lblCodProd";
            lblCodProd.Size = new System.Drawing.Size(80, 17);
            lblCodProd.TabIndex = 0;
            lblCodProd.Text = "DNI Cliente:";

            // 
            // txtCodConsulta
            // 
            txtCodConsulta.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txtCodConsulta.Location = new System.Drawing.Point(100, 27);
            txtCodConsulta.Name = "txtCodConsulta";
            txtCodConsulta.PlaceholderText = "DNI Cliente";
            txtCodConsulta.Size = new System.Drawing.Size(110, 24);
            txtCodConsulta.TabIndex = 1;

            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new System.Drawing.Point(220, 30);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new System.Drawing.Size(126, 17);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre / Apellido:";

            // 
            // txtNombre
            // 
            txtNombre.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txtNombre.Location = new System.Drawing.Point(350, 27);
            txtNombre.Name = "txtNombre";
            txtNombre.PlaceholderText = "Buscar por cliente";
            txtNombre.Size = new System.Drawing.Size(160, 24);
            txtNombre.TabIndex = 3;

            // 
            // lblOperacion
            // 
            lblOperacion.AutoSize = true;
            lblOperacion.Location = new System.Drawing.Point(220, 75);
            lblOperacion.Name = "lblOperacion";
            lblOperacion.Size = new System.Drawing.Size(74, 17);
            lblOperacion.TabIndex = 6;
            lblOperacion.Text = "Operación:";

            // 
            // cmbOperacion
            // 
            cmbOperacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbOperacion.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            cmbOperacion.FormattingEnabled = true;
            cmbOperacion.Items.AddRange(new object[] {
                "Todas",
                "ALTA",
                "MODIFICACIÓN",
                "BLOQUEAR / DESBLOQUEAR"
            });
            cmbOperacion.Location = new System.Drawing.Point(300, 72);
            cmbOperacion.Name = "cmbOperacion";
            cmbOperacion.Size = new System.Drawing.Size(210, 25);
            cmbOperacion.TabIndex = 7;

            // 
            // lblFechaIni
            // 
            lblFechaIni.AutoSize = true;
            lblFechaIni.Location = new System.Drawing.Point(15, 75);
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
            dtpFechaIni.Size = new System.Drawing.Size(110, 24);
            dtpFechaIni.TabIndex = 5;

            // 
            // lblFechaFin
            // 
            lblFechaFin.AutoSize = true;
            lblFechaFin.Location = new System.Drawing.Point(520, 75);
            lblFechaFin.Name = "lblFechaFin";
            lblFechaFin.Size = new System.Drawing.Size(66, 17);
            lblFechaFin.TabIndex = 8;
            lblFechaFin.Text = "Fecha Fin:";
            lblFechaFin.Visible = false;

            // 
            // dtpFechaFin
            // 
            dtpFechaFin.Checked = false;
            dtpFechaFin.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpFechaFin.Location = new System.Drawing.Point(590, 72);
            dtpFechaFin.Name = "dtpFechaFin";
            dtpFechaFin.ShowCheckBox = true;
            dtpFechaFin.Size = new System.Drawing.Size(110, 24);
            dtpFechaFin.TabIndex = 9;
            dtpFechaFin.Visible = false;

            // 
            // btnAplicar
            // 
            btnAplicar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAplicar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnAplicar.Location = new System.Drawing.Point(560, 22);
            btnAplicar.Name = "btnAplicar";
            btnAplicar.Size = new System.Drawing.Size(170, 40);
            btnAplicar.TabIndex = 10;
            btnAplicar.Text = "APLICAR";
            toolTipGeneral.SetToolTip(btnAplicar, "Aplicar: Filtra los registros históricos según los parámetros especificados.");
            btnAplicar.Click += btnAplicar_Click;

            // 
            // btnSalir
            // 
            btnSalir.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnSalir.EsBotonSecundario = true;
            btnSalir.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnSalir.Location = new System.Drawing.Point(750, 22);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new System.Drawing.Size(170, 40);
            btnSalir.TabIndex = 11;
            btnSalir.Text = "SALIR";
            toolTipGeneral.SetToolTip(btnSalir, "Salir: Cierra la vista de Bitácora de Cambios.");
            btnSalir.Click += btnSalir_Click;

            // 
            // btnLimpiar
            // 
            btnLimpiar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnLimpiar.EsBotonSecundario = true;
            btnLimpiar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnLimpiar.Location = new System.Drawing.Point(560, 70);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new System.Drawing.Size(170, 40);
            btnLimpiar.TabIndex = 12;
            btnLimpiar.Text = "LIMPIAR";
            toolTipGeneral.SetToolTip(btnLimpiar, "Limpiar: Restablece todos los filtros y recarga la bitácora completa.");
            btnLimpiar.Click += btnLimpiar_Click;

            // 
            // btnActivar
            // 
            btnActivar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnActivar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnActivar.Location = new System.Drawing.Point(750, 70);
            btnActivar.Name = "btnActivar";
            btnActivar.Size = new System.Drawing.Size(170, 40);
            btnActivar.TabIndex = 13;
            btnActivar.Text = "ACTIVAR";
            toolTipGeneral.SetToolTip(btnActivar, "Activar: Restaura la versión histórica seleccionada hacia la base de datos activa (Rollback).");
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
            Text = "Bitácora de Cambios";
            Load += frmBitacoraCambiosConsultas_Load;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            gbGrilla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCambiosPacientes).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvCambios).EndInit();
            gbControles.ResumeLayout(false);
            gbControles.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.ToolTip toolTipGeneral;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblModulo;
        private System.Windows.Forms.ComboBox cmbModulo;
        private UI.Controles.BotonFuturista btnAyudaContextual;

        private System.Windows.Forms.GroupBox gbGrilla;
        
        // Grilla Pacientes
        private System.Windows.Forms.DataGridView dgvCambiosPacientes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPacIdCambio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPacFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPacHora;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPacDni;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPacCliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPacEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPacCelular;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPacDireccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPacOperacion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPacUsuario;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPacAct;
        private System.Windows.Forms.DataGridViewButtonColumn colPacBtnActivar;

        // Grilla Consultas
        private System.Windows.Forms.DataGridView dgvCambios;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCodCons;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHora;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDni;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEdad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeso;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTalla;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClasif;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAct;
        private System.Windows.Forms.DataGridViewButtonColumn colBtnActivar;

        private System.Windows.Forms.GroupBox gbControles;
        private System.Windows.Forms.Label lblCodProd;
        private System.Windows.Forms.TextBox txtCodConsulta;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblOperacion;
        private System.Windows.Forms.ComboBox cmbOperacion;
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
