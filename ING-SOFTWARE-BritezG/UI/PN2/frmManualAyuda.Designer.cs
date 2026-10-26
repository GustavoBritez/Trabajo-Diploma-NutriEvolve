namespace UI.PN2
{
    partial class frmManualAyuda
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

            panelAvisoSeguridad = new System.Windows.Forms.Panel();
            lblAvisoTitulo = new System.Windows.Forms.Label();
            lblAvisoTexto = new System.Windows.Forms.Label();

            panelIndice = new System.Windows.Forms.Panel();
            lblIndiceTitulo = new System.Windows.Forms.Label();
            btnTema1 = new UI.Controles.BotonFuturista();
            btnTema2 = new UI.Controles.BotonFuturista();
            btnTema3 = new UI.Controles.BotonFuturista();
            btnTema4 = new UI.Controles.BotonFuturista();
            btnTema5 = new UI.Controles.BotonFuturista();
            btnTema6 = new UI.Controles.BotonFuturista();
            btnTema7 = new UI.Controles.BotonFuturista();

            panelContenido = new System.Windows.Forms.Panel();
            lblCapituloTitulo = new System.Windows.Forms.Label();
            rtbContenido = new System.Windows.Forms.RichTextBox();

            panelHeader.SuspendLayout();
            panelAvisoSeguridad.SuspendLayout();
            panelIndice.SuspendLayout();
            panelContenido.SuspendLayout();
            SuspendLayout();

            // panelHeader
            panelHeader.BackColor = System.Drawing.Color.FromArgb(76, 124, 89);
            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(lblSubtitulo);
            panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new System.Drawing.Size(960, 60);
            panelHeader.TabIndex = 0;

            // lblTitulo
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            lblTitulo.ForeColor = System.Drawing.Color.White;
            lblTitulo.Location = new System.Drawing.Point(18, 9);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new System.Drawing.Size(430, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Sistema de Ayuda Contextual y Manual de Usuario";

            // lblSubtitulo
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(235, 245, 235);
            lblSubtitulo.Location = new System.Drawing.Point(20, 34);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new System.Drawing.Size(560, 17);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Índice interactivo para navegación rápida (secciones extensas) y directivas operativas del sistema";

            // panelAvisoSeguridad (Callout de seguridad destacado)
            panelAvisoSeguridad.BackColor = System.Drawing.Color.FromArgb(254, 243, 199);
            panelAvisoSeguridad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panelAvisoSeguridad.Controls.Add(lblAvisoTitulo);
            panelAvisoSeguridad.Controls.Add(lblAvisoTexto);
            panelAvisoSeguridad.Dock = System.Windows.Forms.DockStyle.Top;
            panelAvisoSeguridad.Location = new System.Drawing.Point(0, 60);
            panelAvisoSeguridad.Name = "panelAvisoSeguridad";
            panelAvisoSeguridad.Size = new System.Drawing.Size(960, 52);
            panelAvisoSeguridad.TabIndex = 1;

            // lblAvisoTitulo
            lblAvisoTitulo.AutoSize = true;
            lblAvisoTitulo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblAvisoTitulo.ForeColor = System.Drawing.Color.FromArgb(146, 64, 14);
            lblAvisoTitulo.Location = new System.Drawing.Point(18, 6);
            lblAvisoTitulo.Name = "lblAvisoTitulo";
            lblAvisoTitulo.Size = new System.Drawing.Size(295, 15);
            lblAvisoTitulo.TabIndex = 0;
            lblAvisoTitulo.Text = "DIRECTIVA CRÍTICA DE SEGURIDAD Y PREVENCIÓN:";

            // lblAvisoTexto
            lblAvisoTexto.AutoSize = true;
            lblAvisoTexto.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblAvisoTexto.ForeColor = System.Drawing.Color.FromArgb(120, 53, 15);
            lblAvisoTexto.Location = new System.Drawing.Point(18, 25);
            lblAvisoTexto.Name = "lblAvisoTexto";
            lblAvisoTexto.Size = new System.Drawing.Size(730, 15);
            lblAvisoTexto.TabIndex = 1;
            lblAvisoTexto.Text = "Realice un backup completo de la base de datos antes de reinstalar. El instalador inteligente detectará y protegerá sus datos existentes.";

            // panelIndice (Índice de navegación interactivo)
            panelIndice.BackColor = System.Drawing.Color.FromArgb(240, 245, 240);
            panelIndice.Controls.Add(lblIndiceTitulo);
            panelIndice.Controls.Add(btnTema1);
            panelIndice.Controls.Add(btnTema2);
            panelIndice.Controls.Add(btnTema3);
            panelIndice.Controls.Add(btnTema4);
            panelIndice.Controls.Add(btnTema5);
            panelIndice.Controls.Add(btnTema6);
            panelIndice.Controls.Add(btnTema7);
            panelIndice.Dock = System.Windows.Forms.DockStyle.Left;
            panelIndice.Location = new System.Drawing.Point(0, 112);
            panelIndice.Name = "panelIndice";
            panelIndice.Size = new System.Drawing.Size(260, 488);
            panelIndice.TabIndex = 2;

            // lblIndiceTitulo
            lblIndiceTitulo.AutoSize = true;
            lblIndiceTitulo.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            lblIndiceTitulo.ForeColor = System.Drawing.Color.FromArgb(40, 70, 50);
            lblIndiceTitulo.Location = new System.Drawing.Point(12, 10);
            lblIndiceTitulo.Name = "lblIndiceTitulo";
            lblIndiceTitulo.Size = new System.Drawing.Size(145, 19);
            lblIndiceTitulo.TabIndex = 0;
            lblIndiceTitulo.Text = "ÍNDICE DE MATERIAS";

            // Botones de temas del índice
            btnTema1.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnTema1.Location = new System.Drawing.Point(10, 40);
            btnTema1.Name = "btnTema1";
            btnTema1.Size = new System.Drawing.Size(240, 36);
            btnTema1.TabIndex = 1;
            btnTema1.Text = "1. Arquitectura General";
            btnTema1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnTema1.Click += (s, e) => MostrarCapitulo(1);

            btnTema2.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnTema2.Location = new System.Drawing.Point(10, 82);
            btnTema2.Name = "btnTema2";
            btnTema2.Size = new System.Drawing.Size(240, 36);
            btnTema2.TabIndex = 2;
            btnTema2.Text = "2. Maestro de Clientes (ABM-C)";
            btnTema2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnTema2.Click += (s, e) => MostrarCapitulo(2);

            btnTema3.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnTema3.Location = new System.Drawing.Point(10, 124);
            btnTema3.Name = "btnTema3";
            btnTema3.Size = new System.Drawing.Size(240, 36);
            btnTema3.TabIndex = 3;
            btnTema3.Text = "3. Tablas 1:N y Baja Lógica";
            btnTema3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnTema3.Click += (s, e) => MostrarCapitulo(3);

            btnTema4.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnTema4.Location = new System.Drawing.Point(10, 166);
            btnTema4.Name = "btnTema4";
            btnTema4.Size = new System.Drawing.Size(240, 36);
            btnTema4.TabIndex = 4;
            btnTema4.Text = "4. Consultas y Curvas OMS";
            btnTema4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnTema4.Click += (s, e) => MostrarCapitulo(4);

            btnTema5.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnTema5.Location = new System.Drawing.Point(10, 208);
            btnTema5.Name = "btnTema5";
            btnTema5.Size = new System.Drawing.Size(240, 36);
            btnTema5.TabIndex = 5;
            btnTema5.Text = "5. Bitácora Trigger y Rollback";
            btnTema5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnTema5.Click += (s, e) => MostrarCapitulo(5);

            btnTema6.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnTema6.Location = new System.Drawing.Point(10, 250);
            btnTema6.Name = "btnTema6";
            btnTema6.Size = new System.Drawing.Size(240, 36);
            btnTema6.TabIndex = 6;
            btnTema6.Text = "6. Reporte Predictivo";
            btnTema6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnTema6.Click += (s, e) => MostrarCapitulo(6);

            btnTema7.Font = new System.Drawing.Font("Segoe UI", 9F);
            btnTema7.Location = new System.Drawing.Point(10, 292);
            btnTema7.Name = "btnTema7";
            btnTema7.Size = new System.Drawing.Size(240, 36);
            btnTema7.TabIndex = 7;
            btnTema7.Text = "7. Reinstalador y Seguridad";
            btnTema7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnTema7.Click += (s, e) => MostrarCapitulo(7);

            // panelContenido
            panelContenido.Controls.Add(lblCapituloTitulo);
            panelContenido.Controls.Add(rtbContenido);
            panelContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            panelContenido.Location = new System.Drawing.Point(260, 112);
            panelContenido.Name = "panelContenido";
            panelContenido.Padding = new System.Windows.Forms.Padding(15);
            panelContenido.Size = new System.Drawing.Size(700, 488);
            panelContenido.TabIndex = 3;

            // lblCapituloTitulo
            lblCapituloTitulo.AutoSize = true;
            lblCapituloTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            lblCapituloTitulo.ForeColor = System.Drawing.Color.FromArgb(30, 60, 40);
            lblCapituloTitulo.Location = new System.Drawing.Point(15, 12);
            lblCapituloTitulo.Name = "lblCapituloTitulo";
            lblCapituloTitulo.Size = new System.Drawing.Size(160, 21);
            lblCapituloTitulo.TabIndex = 0;
            lblCapituloTitulo.Text = "Título del Capítulo";

            // rtbContenido
            rtbContenido.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            rtbContenido.BackColor = System.Drawing.Color.White;
            rtbContenido.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            rtbContenido.Font = new System.Drawing.Font("Segoe UI", 10F);
            rtbContenido.Location = new System.Drawing.Point(15, 42);
            rtbContenido.Name = "rtbContenido";
            rtbContenido.ReadOnly = true;
            rtbContenido.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            rtbContenido.Size = new System.Drawing.Size(670, 431);
            rtbContenido.TabIndex = 1;
            rtbContenido.Text = "";

            // Form
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(960, 600);
            Controls.Add(panelContenido);
            Controls.Add(panelIndice);
            Controls.Add(panelAvisoSeguridad);
            Controls.Add(panelHeader);
            Name = "frmManualAyuda";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Manual de Ayuda de NutriEvolve";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelAvisoSeguridad.ResumeLayout(false);
            panelAvisoSeguridad.PerformLayout();
            panelIndice.ResumeLayout(false);
            panelIndice.PerformLayout();
            panelContenido.ResumeLayout(false);
            panelContenido.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Panel panelAvisoSeguridad;
        private System.Windows.Forms.Label lblAvisoTitulo;
        private System.Windows.Forms.Label lblAvisoTexto;
        private System.Windows.Forms.Panel panelIndice;
        private System.Windows.Forms.Label lblIndiceTitulo;
        private UI.Controles.BotonFuturista btnTema1;
        private UI.Controles.BotonFuturista btnTema2;
        private UI.Controles.BotonFuturista btnTema3;
        private UI.Controles.BotonFuturista btnTema4;
        private UI.Controles.BotonFuturista btnTema5;
        private UI.Controles.BotonFuturista btnTema6;
        private UI.Controles.BotonFuturista btnTema7;
        private System.Windows.Forms.Panel panelContenido;
        private System.Windows.Forms.Label lblCapituloTitulo;
        private System.Windows.Forms.RichTextBox rtbContenido;
    }
}
