namespace UI.PN2
{
    partial class frmReporteInteligente
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

            gbListado = new System.Windows.Forms.GroupBox();
            dgvReporte = new System.Windows.Forms.DataGridView();

            gbDetallePredictivo = new System.Windows.Forms.GroupBox();
            lblProyeccionTitulo = new System.Windows.Forms.Label();
            txtProyeccion = new System.Windows.Forms.TextBox();
            lblRecomendacionTitulo = new System.Windows.Forms.Label();
            txtRecomendaciones = new System.Windows.Forms.TextBox();

            btnActualizar = new UI.Controles.BotonFuturista();
            btnExportar = new UI.Controles.BotonFuturista();
            btnCerrar = new UI.Controles.BotonFuturista();

            panelHeader.SuspendLayout();
            gbListado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvReporte).BeginInit();
            gbDetallePredictivo.SuspendLayout();
            SuspendLayout();

            btnAyudaContextual = new UI.Controles.BotonFuturista();

            // panelHeader
            panelHeader.BackColor = System.Drawing.Color.FromArgb(76, 124, 89);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(lblSubtitulo);
            panelHeader.Controls.Add(btnAyudaContextual);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(1020, 65);
            panelHeader.TabIndex = 0;

            // btnAyudaContextual
            btnAyudaContextual.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAyudaContextual.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnAyudaContextual.Location = new System.Drawing.Point(900, 15);
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
            lblTitulo.Size = new System.Drawing.Size(530, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Reporte Inteligente Predictivo y Soporte a Decisiones Clínicas";

            // lblSubtitulo
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(235, 245, 235);
            lblSubtitulo.Location = new System.Drawing.Point(20, 38);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new System.Drawing.Size(640, 17);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Cálculo de velocidad ponderal, pendientes de crecimiento percentilar OMS y proyección clínica a 90 días";

            // gbListado
            gbListado.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            gbListado.Controls.Add(dgvReporte);
            gbListado.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            gbListado.ForeColor = System.Drawing.Color.FromArgb(40, 70, 50);
            gbListado.Location = new System.Drawing.Point(18, 75);
            gbListado.Name = "gbListado";
            gbListado.Size = new System.Drawing.Size(984, 275);
            gbListado.TabIndex = 1;
            gbListado.TabStop = false;
            gbListado.Text = "Tablero Multivariable de Pacientes con Semáforo Predictivo de Riesgo Nutricional";

            // dgvReporte
            dgvReporte.AllowUserToAddRows = false;
            dgvReporte.AllowUserToDeleteRows = false;
            dgvReporte.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvReporte.BackgroundColor = System.Drawing.Color.White;
            dgvStyleHeader.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvStyleHeader.BackColor = System.Drawing.Color.FromArgb(76, 124, 89);
            dgvStyleHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            dgvStyleHeader.ForeColor = System.Drawing.Color.White;
            dgvReporte.ColumnHeadersDefaultCellStyle = dgvStyleHeader;
            dgvReporte.ColumnHeadersHeight = 32;
            dgvReporte.EnableHeadersVisualStyles = false;
            dgvReporte.Location = new System.Drawing.Point(15, 25);
            dgvReporte.MultiSelect = false;
            dgvReporte.Name = "dgvReporte";
            dgvReporte.ReadOnly = true;
            dgvReporte.RowHeadersVisible = false;
            dgvStyleRows.Font = new System.Drawing.Font("Segoe UI", 9F);
            dgvReporte.RowsDefaultCellStyle = dgvStyleRows;
            dgvReporte.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvReporte.Size = new System.Drawing.Size(954, 235);
            dgvReporte.TabIndex = 0;
            dgvReporte.CellFormatting += dgvReporte_CellFormatting;
            dgvReporte.SelectionChanged += dgvReporte_SelectionChanged;

            // gbDetallePredictivo
            gbDetallePredictivo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            gbDetallePredictivo.Controls.Add(lblProyeccionTitulo);
            gbDetallePredictivo.Controls.Add(txtProyeccion);
            gbDetallePredictivo.Controls.Add(lblRecomendacionTitulo);
            gbDetallePredictivo.Controls.Add(txtRecomendaciones);
            gbDetallePredictivo.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            gbDetallePredictivo.ForeColor = System.Drawing.Color.FromArgb(40, 70, 50);
            gbDetallePredictivo.Location = new System.Drawing.Point(18, 360);
            gbDetallePredictivo.Name = "gbDetallePredictivo";
            gbDetallePredictivo.Size = new System.Drawing.Size(984, 215);
            gbDetallePredictivo.TabIndex = 2;
            gbDetallePredictivo.TabStop = false;
            gbDetallePredictivo.Text = "Análisis Predictivo Individual y Recomendaciones Accionables del Motor de Decisión";

            // lblProyeccionTitulo
            lblProyeccionTitulo.AutoSize = true;
            lblProyeccionTitulo.Location = new System.Drawing.Point(15, 24);
            lblProyeccionTitulo.Name = "lblProyeccionTitulo";
            lblProyeccionTitulo.Size = new System.Drawing.Size(260, 17);
            lblProyeccionTitulo.TabIndex = 0;
            lblProyeccionTitulo.Text = "Proyección Antropométrica a 90 Días:";

            // txtProyeccion
            txtProyeccion.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtProyeccion.BackColor = System.Drawing.Color.White;
            txtProyeccion.Font = new System.Drawing.Font("Consolas", 9F);
            txtProyeccion.Location = new System.Drawing.Point(18, 45);
            txtProyeccion.Multiline = true;
            txtProyeccion.Name = "txtProyeccion";
            txtProyeccion.ReadOnly = true;
            txtProyeccion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtProyeccion.Size = new System.Drawing.Size(430, 155);
            txtProyeccion.TabIndex = 1;

            // lblRecomendacionTitulo
            lblRecomendacionTitulo.AutoSize = true;
            lblRecomendacionTitulo.Location = new System.Drawing.Point(465, 24);
            lblRecomendacionTitulo.Name = "lblRecomendacionTitulo";
            lblRecomendacionTitulo.Size = new System.Drawing.Size(295, 17);
            lblRecomendacionTitulo.TabIndex = 2;
            lblRecomendacionTitulo.Text = "Pautas Dietoterápicas y Conductuales Sugeridas:";

            // txtRecomendaciones
            txtRecomendaciones.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtRecomendaciones.BackColor = System.Drawing.Color.White;
            txtRecomendaciones.Font = new System.Drawing.Font("Consolas", 9F);
            txtRecomendaciones.Location = new System.Drawing.Point(468, 45);
            txtRecomendaciones.Multiline = true;
            txtRecomendaciones.Name = "txtRecomendaciones";
            txtRecomendaciones.ReadOnly = true;
            txtRecomendaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtRecomendaciones.Size = new System.Drawing.Size(501, 155);
            txtRecomendaciones.TabIndex = 3;

            // Botones inferiores
            btnActualizar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnActualizar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnActualizar.Location = new System.Drawing.Point(18, 590);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new System.Drawing.Size(160, 38);
            btnActualizar.TabIndex = 3;
            btnActualizar.Text = "Actualizar Reporte";
            btnActualizar.Click += btnActualizar_Click;

            btnExportar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnExportar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnExportar.Location = new System.Drawing.Point(190, 590);
            btnExportar.Name = "btnExportar";
            btnExportar.Size = new System.Drawing.Size(180, 38);
            btnExportar.TabIndex = 4;
            btnExportar.Text = "Exportar Informe";
            btnExportar.Click += btnExportar_Click;

            btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCerrar.EsBotonSecundario = true;
            btnCerrar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnCerrar.Location = new System.Drawing.Point(882, 590);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new System.Drawing.Size(120, 38);
            btnCerrar.TabIndex = 5;
            btnCerrar.Text = "Cerrar";
            btnCerrar.Click += btnCerrar_Click;

            // Form
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 248, 245);
            ClientSize = new System.Drawing.Size(1020, 640);
            Controls.Add(btnCerrar);
            Controls.Add(btnExportar);
            Controls.Add(btnActualizar);
            Controls.Add(gbDetallePredictivo);
            Controls.Add(gbListado);
            Controls.Add(panelHeader);
            Name = "frmReporteInteligente";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Reporte Inteligente Predictivo - NutriEvolve";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            gbListado.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvReporte).EndInit();
            gbDetallePredictivo.ResumeLayout(false);
            gbDetallePredictivo.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.GroupBox gbListado;
        private System.Windows.Forms.DataGridView dgvReporte;
        private System.Windows.Forms.GroupBox gbDetallePredictivo;
        private System.Windows.Forms.Label lblProyeccionTitulo;
        private System.Windows.Forms.TextBox txtProyeccion;
        private System.Windows.Forms.Label lblRecomendacionTitulo;
        private System.Windows.Forms.TextBox txtRecomendaciones;
        private UI.Controles.BotonFuturista btnActualizar;
        private UI.Controles.BotonFuturista btnExportar;
        private UI.Controles.BotonFuturista btnCerrar;
        private UI.Controles.BotonFuturista btnAyudaContextual;
    }
}
