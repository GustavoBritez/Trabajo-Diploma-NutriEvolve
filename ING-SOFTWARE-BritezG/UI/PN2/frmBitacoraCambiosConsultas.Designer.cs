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

            gbFiltros = new System.Windows.Forms.GroupBox();
            lblConsultaFiltro = new System.Windows.Forms.Label();
            txtIdConsultaFiltro = new System.Windows.Forms.TextBox();
            btnFiltrar = new UI.Controles.BotonFuturista();
            btnVerTodos = new UI.Controles.BotonFuturista();

            gbGrilla = new System.Windows.Forms.GroupBox();
            dgvCambios = new System.Windows.Forms.DataGridView();

            gbDetalle = new System.Windows.Forms.GroupBox();
            lblDetalleInfo = new System.Windows.Forms.Label();
            txtDetalleHistorial = new System.Windows.Forms.TextBox();
            btnRollback = new UI.Controles.BotonFuturista();
            btnCerrar = new UI.Controles.BotonFuturista();

            panelHeader.SuspendLayout();
            gbFiltros.SuspendLayout();
            gbGrilla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCambios).BeginInit();
            gbDetalle.SuspendLayout();
            SuspendLayout();

            lblEntidad = new System.Windows.Forms.Label();
            cmbEntidad = new System.Windows.Forms.ComboBox();
            btnAyudaContextual = new UI.Controles.BotonFuturista();

            // panelHeader
            panelHeader.BackColor = System.Drawing.Color.FromArgb(76, 124, 89);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(lblSubtitulo);
            panelHeader.Controls.Add(btnAyudaContextual);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(980, 65);
            panelHeader.TabIndex = 0;

            // btnAyudaContextual
            btnAyudaContextual.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAyudaContextual.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnAyudaContextual.Location = new System.Drawing.Point(860, 15);
            btnAyudaContextual.Name = "btnAyudaContextual";
            btnAyudaContextual.Size = new System.Drawing.Size(100, 35);
            btnAyudaContextual.TabIndex = 2;
            btnAyudaContextual.Text = "? Ayuda";
            btnAyudaContextual.Click += btnAyudaContextual_Click;

            // lblTitulo
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblTitulo.ForeColor = System.Drawing.Color.White;
            lblTitulo.Location = new System.Drawing.Point(18, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new System.Drawing.Size(490, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Bitácora de Cambios de Consultas Nutricionales (Trigger SQL)";

            // lblSubtitulo
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(235, 245, 235);
            lblSubtitulo.Location = new System.Drawing.Point(20, 38);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new System.Drawing.Size(580, 17);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Auditoría de estados históricos ante UPDATE/DELETE y restauración de versiones previas (Rollback)";

            // gbFiltros
            gbFiltros.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            gbFiltros.Controls.Add(lblEntidad);
            gbFiltros.Controls.Add(cmbEntidad);
            gbFiltros.Controls.Add(lblConsultaFiltro);
            gbFiltros.Controls.Add(txtIdConsultaFiltro);
            gbFiltros.Controls.Add(btnFiltrar);
            gbFiltros.Controls.Add(btnVerTodos);
            gbFiltros.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            gbFiltros.ForeColor = System.Drawing.Color.FromArgb(40, 70, 50);
            gbFiltros.Location = new System.Drawing.Point(18, 75);
            gbFiltros.Name = "gbFiltros";
            gbFiltros.Size = new System.Drawing.Size(944, 65);
            gbFiltros.TabIndex = 1;
            gbFiltros.TabStop = false;
            gbFiltros.Text = "Filtro de Auditoría y Selección de Entidad";

            // lblEntidad
            lblEntidad.AutoSize = true;
            lblEntidad.Location = new System.Drawing.Point(15, 28);
            lblEntidad.Name = "lblEntidad";
            lblEntidad.Size = new System.Drawing.Size(58, 17);
            lblEntidad.TabIndex = 4;
            lblEntidad.Text = "Entidad:";

            // cmbEntidad
            cmbEntidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbEntidad.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            cmbEntidad.FormattingEnabled = true;
            cmbEntidad.Items.AddRange(new object[] { "Consultas Nutricionales", "Planes Alimentarios", "Pacientes (Clientes)" });
            cmbEntidad.Location = new System.Drawing.Point(78, 25);
            cmbEntidad.Name = "cmbEntidad";
            cmbEntidad.Size = new System.Drawing.Size(195, 24);
            cmbEntidad.TabIndex = 5;
            cmbEntidad.SelectedIndexChanged += cmbEntidad_SelectedIndexChanged;

            // lblConsultaFiltro
            lblConsultaFiltro.AutoSize = true;
            lblConsultaFiltro.Location = new System.Drawing.Point(295, 28);
            lblConsultaFiltro.Name = "lblConsultaFiltro";
            lblConsultaFiltro.Size = new System.Drawing.Size(65, 17);
            lblConsultaFiltro.TabIndex = 0;
            lblConsultaFiltro.Text = "ID Filtro:";

            // txtIdConsultaFiltro
            txtIdConsultaFiltro.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txtIdConsultaFiltro.Location = new System.Drawing.Point(365, 25);
            txtIdConsultaFiltro.Name = "txtIdConsultaFiltro";
            txtIdConsultaFiltro.PlaceholderText = "Ej: 1, 2, 3...";
            txtIdConsultaFiltro.Size = new System.Drawing.Size(110, 24);
            txtIdConsultaFiltro.TabIndex = 1;

            // btnFiltrar
            btnFiltrar.Location = new System.Drawing.Point(490, 22);
            btnFiltrar.Name = "btnFiltrar";
            btnFiltrar.Size = new System.Drawing.Size(100, 30);
            btnFiltrar.TabIndex = 2;
            btnFiltrar.Text = "Filtrar";
            btnFiltrar.Click += btnFiltrar_Click;

            // btnVerTodos
            btnVerTodos.EsBotonSecundario = true;
            btnVerTodos.Location = new System.Drawing.Point(600, 22);
            btnVerTodos.Name = "btnVerTodos";
            btnVerTodos.Size = new System.Drawing.Size(100, 30);
            btnVerTodos.TabIndex = 3;
            btnVerTodos.Text = "Ver Todos";
            btnVerTodos.Click += btnVerTodos_Click;

            // gbGrilla
            gbGrilla.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            gbGrilla.Controls.Add(dgvCambios);
            gbGrilla.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            gbGrilla.ForeColor = System.Drawing.Color.FromArgb(40, 70, 50);
            gbGrilla.Location = new System.Drawing.Point(18, 148);
            gbGrilla.Name = "gbGrilla";
            gbGrilla.Size = new System.Drawing.Size(944, 250);
            gbGrilla.TabIndex = 2;
            gbGrilla.TabStop = false;
            gbGrilla.Text = "Registro Histórico de Cambios (Pseudotabla 'deleted' en Trigger AFTER UPDATE/DELETE)";

            // dgvCambios
            dgvCambios.AllowUserToAddRows = false;
            dgvCambios.AllowUserToDeleteRows = false;
            dgvCambios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvCambios.BackgroundColor = System.Drawing.Color.White;
            dgvStyleHeader.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvStyleHeader.BackColor = System.Drawing.Color.FromArgb(76, 124, 89);
            dgvStyleHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            dgvStyleHeader.ForeColor = System.Drawing.Color.White;
            dgvCambios.ColumnHeadersDefaultCellStyle = dgvStyleHeader;
            dgvCambios.ColumnHeadersHeight = 30;
            dgvCambios.EnableHeadersVisualStyles = false;
            dgvCambios.Location = new System.Drawing.Point(15, 25);
            dgvCambios.MultiSelect = false;
            dgvCambios.Name = "dgvCambios";
            dgvCambios.ReadOnly = true;
            dgvCambios.RowHeadersVisible = false;
            dgvStyleRows.Font = new System.Drawing.Font("Segoe UI", 9F);
            dgvCambios.RowsDefaultCellStyle = dgvStyleRows;
            dgvCambios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvCambios.Size = new System.Drawing.Size(914, 210);
            dgvCambios.TabIndex = 0;
            dgvCambios.SelectionChanged += dgvCambios_SelectionChanged;

            // gbDetalle
            gbDetalle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            gbDetalle.Controls.Add(lblDetalleInfo);
            gbDetalle.Controls.Add(txtDetalleHistorial);
            gbDetalle.Controls.Add(btnRollback);
            gbDetalle.Controls.Add(btnCerrar);
            gbDetalle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            gbDetalle.ForeColor = System.Drawing.Color.FromArgb(40, 70, 50);
            gbDetalle.Location = new System.Drawing.Point(18, 405);
            gbDetalle.Name = "gbDetalle";
            gbDetalle.Size = new System.Drawing.Size(944, 180);
            gbDetalle.TabIndex = 3;
            gbDetalle.TabStop = false;
            gbDetalle.Text = "Detalle del Estado Histórico Seleccionado y Opciones de Reversión";

            // lblDetalleInfo
            lblDetalleInfo.AutoSize = true;
            lblDetalleInfo.Location = new System.Drawing.Point(15, 24);
            lblDetalleInfo.Name = "lblDetalleInfo";
            lblDetalleInfo.Size = new System.Drawing.Size(430, 17);
            lblDetalleInfo.TabIndex = 0;
            lblDetalleInfo.Text = "Valores de la consulta nutricional previos a la modificación seleccionada:";

            // txtDetalleHistorial
            txtDetalleHistorial.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtDetalleHistorial.BackColor = System.Drawing.Color.White;
            txtDetalleHistorial.Font = new System.Drawing.Font("Consolas", 9F);
            txtDetalleHistorial.Location = new System.Drawing.Point(18, 48);
            txtDetalleHistorial.Multiline = true;
            txtDetalleHistorial.Name = "txtDetalleHistorial";
            txtDetalleHistorial.ReadOnly = true;
            txtDetalleHistorial.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtDetalleHistorial.Size = new System.Drawing.Size(650, 118);
            txtDetalleHistorial.TabIndex = 1;

            // btnRollback
            btnRollback.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnRollback.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnRollback.Location = new System.Drawing.Point(685, 75);
            btnRollback.Name = "btnRollback";
            btnRollback.Size = new System.Drawing.Size(245, 42);
            btnRollback.TabIndex = 2;
            btnRollback.Text = "Revertir a esta Versión (Rollback)";
            btnRollback.Click += btnRollback_Click;

            // btnCerrar
            btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCerrar.EsBotonSecundario = true;
            btnCerrar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnCerrar.Location = new System.Drawing.Point(685, 125);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new System.Drawing.Size(245, 36);
            btnCerrar.TabIndex = 3;
            btnCerrar.Text = "Cerrar";
            btnCerrar.Click += btnCerrar_Click;

            // Form
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 248, 245);
            ClientSize = new System.Drawing.Size(980, 600);
            Controls.Add(gbDetalle);
            Controls.Add(gbGrilla);
            Controls.Add(gbFiltros);
            Controls.Add(panelHeader);
            Name = "frmBitacoraCambiosConsultas";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Bitácora de Cambios - Consultas Nutricionales";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            gbFiltros.ResumeLayout(false);
            gbFiltros.PerformLayout();
            gbGrilla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCambios).EndInit();
            gbDetalle.ResumeLayout(false);
            gbDetalle.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.GroupBox gbFiltros;
        private System.Windows.Forms.Label lblConsultaFiltro;
        private System.Windows.Forms.TextBox txtIdConsultaFiltro;
        private UI.Controles.BotonFuturista btnFiltrar;
        private UI.Controles.BotonFuturista btnVerTodos;
        private System.Windows.Forms.GroupBox gbGrilla;
        private System.Windows.Forms.DataGridView dgvCambios;
        private System.Windows.Forms.GroupBox gbDetalle;
        private System.Windows.Forms.Label lblDetalleInfo;
        private System.Windows.Forms.TextBox txtDetalleHistorial;
        private UI.Controles.BotonFuturista btnRollback;
        private UI.Controles.BotonFuturista btnCerrar;
        private UI.Controles.BotonFuturista btnAyudaContextual;
        private System.Windows.Forms.Label lblEntidad;
        private System.Windows.Forms.ComboBox cmbEntidad;
    }
}
