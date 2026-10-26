namespace UI.PN2
{
    partial class frmComprobantePrescripcion_DNI101
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
            panelHeader = new System.Windows.Forms.Panel();
            lblTitulo = new System.Windows.Forms.Label();
            lblSubtitulo = new System.Windows.Forms.Label();
            btnAyudaContextual = new UI.Controles.BotonFuturista();

            panelBotones = new System.Windows.Forms.Panel();
            btnImprimir = new UI.Controles.BotonFuturista();
            btnVistaPrevia = new UI.Controles.BotonFuturista();
            btnExportar = new UI.Controles.BotonFuturista();
            btnCerrar = new UI.Controles.BotonFuturista();

            printPreviewControl = new System.Windows.Forms.PrintPreviewControl();

            panelHeader.SuspendLayout();
            panelBotones.SuspendLayout();
            SuspendLayout();

            // panelHeader
            panelHeader.BackColor = System.Drawing.Color.FromArgb(76, 124, 89);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(lblSubtitulo);
            panelHeader.Controls.Add(btnAyudaContextual);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(860, 65);
            panelHeader.TabIndex = 0;

            // btnAyudaContextual
            btnAyudaContextual.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnAyudaContextual.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnAyudaContextual.Location = new System.Drawing.Point(745, 15);
            btnAyudaContextual.Name = "btnAyudaContextual";
            btnAyudaContextual.Size = new System.Drawing.Size(95, 35);
            btnAyudaContextual.TabIndex = 2;
            btnAyudaContextual.Text = "? Ayuda";
            btnAyudaContextual.Click += btnAyudaContextual_Click;

            // lblTitulo
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblTitulo.ForeColor = System.Drawing.Color.White;
            lblTitulo.Location = new System.Drawing.Point(18, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new System.Drawing.Size(515, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Comprobante Formal - Ficha de Prescripción Pediátrica";

            // lblSubtitulo
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(235, 245, 235);
            lblSubtitulo.Location = new System.Drawing.Point(20, 38);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new System.Drawing.Size(535, 17);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Documento clínico oficial imprimible: Recibo de consulta y prescripción de plan nutricional";

            // panelBotones
            panelBotones.BackColor = System.Drawing.Color.FromArgb(235, 242, 236);
            panelBotones.Controls.Add(btnImprimir);
            panelBotones.Controls.Add(btnVistaPrevia);
            panelBotones.Controls.Add(btnExportar);
            panelBotones.Controls.Add(btnCerrar);
            panelBotones.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelBotones.Location = new System.Drawing.Point(0, 680);
            panelBotones.Name = "panelBotones";
            panelBotones.Size = new System.Drawing.Size(860, 60);
            panelBotones.TabIndex = 1;

            // btnImprimir
            btnImprimir.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnImprimir.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btnImprimir.Location = new System.Drawing.Point(270, 12);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new System.Drawing.Size(160, 38);
            btnImprimir.TabIndex = 0;
            btnImprimir.Text = "Imprimir Documento";
            btnImprimir.Click += btnImprimir_Click;

            // btnVistaPrevia
            btnVistaPrevia.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnVistaPrevia.EsBotonSecundario = true;
            btnVistaPrevia.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnVistaPrevia.Location = new System.Drawing.Point(440, 12);
            btnVistaPrevia.Name = "btnVistaPrevia";
            btnVistaPrevia.Size = new System.Drawing.Size(150, 38);
            btnVistaPrevia.TabIndex = 1;
            btnVistaPrevia.Text = "Vista Preliminar";
            btnVistaPrevia.Click += btnVistaPrevia_Click;

            // btnExportar
            btnExportar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnExportar.EsBotonSecundario = true;
            btnExportar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnExportar.Location = new System.Drawing.Point(600, 12);
            btnExportar.Name = "btnExportar";
            btnExportar.Size = new System.Drawing.Size(130, 38);
            btnExportar.TabIndex = 2;
            btnExportar.Text = "Exportar Copia";
            btnExportar.Click += btnExportar_Click;

            // btnCerrar
            btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCerrar.EsBotonSecundario = true;
            btnCerrar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            btnCerrar.Location = new System.Drawing.Point(740, 12);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new System.Drawing.Size(105, 38);
            btnCerrar.TabIndex = 3;
            btnCerrar.Text = "Cerrar";
            btnCerrar.Click += btnCerrar_Click;

            // printPreviewControl
            printPreviewControl.AutoZoom = false;
            printPreviewControl.BackColor = System.Drawing.Color.FromArgb(160, 175, 165);
            printPreviewControl.Dock = System.Windows.Forms.DockStyle.Fill;
            printPreviewControl.Location = new System.Drawing.Point(0, 65);
            printPreviewControl.Name = "printPreviewControl";
            printPreviewControl.Size = new System.Drawing.Size(860, 615);
            printPreviewControl.TabIndex = 2;
            printPreviewControl.Zoom = 1.0D;

            // Form
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 248, 245);
            ClientSize = new System.Drawing.Size(860, 740);
            Controls.Add(printPreviewControl);
            Controls.Add(panelBotones);
            Controls.Add(panelHeader);
            MinimumSize = new System.Drawing.Size(750, 600);
            Name = "frmComprobantePrescripcion_DNI101";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Comprobante Formal - Prescripción Nutricional Pediátrica";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelBotones.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private UI.Controles.BotonFuturista btnAyudaContextual;
        private System.Windows.Forms.Panel panelBotones;
        private UI.Controles.BotonFuturista btnImprimir;
        private UI.Controles.BotonFuturista btnVistaPrevia;
        private UI.Controles.BotonFuturista btnExportar;
        private UI.Controles.BotonFuturista btnCerrar;
        private System.Windows.Forms.PrintPreviewControl printPreviewControl;
    }
}
