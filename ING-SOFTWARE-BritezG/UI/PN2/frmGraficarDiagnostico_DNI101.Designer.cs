using System.Drawing;
using System.Windows.Forms;
using ScottPlot.WinForms;

namespace UI.PN2
{
    partial class frmGraficarDiagnostico_DNI101
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
            pnlTop = new Panel();
            lblTitulo = new Label();
            lblSub = new Label();
            lblSel = new Label();
            _cmbIndicador = new ComboBox();
            split = new SplitContainer();
            _formsPlot = new FormsPlot();
            pnlClinico = new Panel();
            lblTitPanel = new Label();
            _lblDiagnosticoBadge = new Label();
            _lblDetallesClinicos = new Label();
            _pnlAlertaCritica = new Panel();
            _lblTextoAlerta = new Label();
            lblTitGrilla = new Label();
            _dgvHistorial = new DataGridView();
            pnlBotones = new Panel();
            btnNuevaConsulta = new Button();
            btnPlan = new Button();

            pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)split).BeginInit();
            split.Panel1.SuspendLayout();
            split.Panel2.SuspendLayout();
            split.SuspendLayout();
            pnlClinico.SuspendLayout();
            _pnlAlertaCritica.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_dgvHistorial).BeginInit();
            pnlBotones.SuspendLayout();
            SuspendLayout();

            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(76, 124, 89);
            pnlTop.Controls.Add(lblTitulo);
            pnlTop.Controls.Add(lblSub);
            pnlTop.Controls.Add(lblSel);
            pnlTop.Controls.Add(_cmbIndicador);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 85;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(15, 10, 15, 10);
            pnlTop.TabIndex = 0;

            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(15, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(500, 21);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "📈 CURVAS DE CRECIMIENTO INFANTIL (OMS)";

            // 
            // lblSub
            // 
            lblSub.AutoSize = true;
            lblSub.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblSub.ForeColor = Color.FromArgb(220, 240, 225);
            lblSub.Location = new Point(17, 42);
            lblSub.Name = "lblSub";
            lblSub.Size = new Size(400, 15);
            lblSub.TabIndex = 1;
            lblSub.Text = "DNI: -  |  Sexo: -  |  Nacimiento: -  |  Cobertura: -";

            // 
            // lblSel
            // 
            lblSel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSel.AutoSize = true;
            lblSel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblSel.ForeColor = Color.White;
            lblSel.Location = new Point(pnlTop.Width - 360, 25);
            lblSel.Name = "lblSel";
            lblSel.Size = new Size(69, 17);
            lblSel.TabIndex = 2;
            lblSel.Text = "Indicador:";

            // 
            // _cmbIndicador
            // 
            _cmbIndicador.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _cmbIndicador.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbIndicador.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            _cmbIndicador.FormattingEnabled = true;
            _cmbIndicador.Items.AddRange(new object[] {
                "📊  IMC para la Edad (kg/m²)",
                "⚖️  Peso para la Edad (kg)",
                "📏  Talla para la Edad (cm)"
            });
            _cmbIndicador.Location = new Point(pnlTop.Width - 280, 22);
            _cmbIndicador.Name = "_cmbIndicador";
            _cmbIndicador.Size = new Size(260, 24);
            _cmbIndicador.TabIndex = 3;
            _cmbIndicador.SelectedIndexChanged += _cmbIndicador_SelectedIndexChanged;

            // 
            // split
            // 
            split.BackColor = Color.FromArgb(225, 235, 228);
            split.Dock = DockStyle.Fill;
            split.Location = new Point(0, 85);
            split.Name = "split";
            split.Orientation = Orientation.Vertical;
            split.SplitterDistance = 720;
            split.SplitterWidth = 6;
            split.TabIndex = 1;

            // 
            // split.Panel1
            // 
            split.Panel1.Controls.Add(_formsPlot);

            // 
            // split.Panel2
            // 
            split.Panel2.Controls.Add(pnlClinico);

            // 
            // _formsPlot
            // 
            _formsPlot.BackColor = Color.White;
            _formsPlot.Dock = DockStyle.Fill;
            _formsPlot.Location = new Point(0, 0);
            _formsPlot.Name = "_formsPlot";
            _formsPlot.Size = new Size(720, 615);
            _formsPlot.TabIndex = 0;

            // 
            // pnlClinico
            // 
            pnlClinico.BackColor = Color.White;
            pnlClinico.Controls.Add(_dgvHistorial);
            pnlClinico.Controls.Add(lblTitGrilla);
            pnlClinico.Controls.Add(_pnlAlertaCritica);
            pnlClinico.Controls.Add(_lblDetallesClinicos);
            pnlClinico.Controls.Add(_lblDiagnosticoBadge);
            pnlClinico.Controls.Add(lblTitPanel);
            pnlClinico.Controls.Add(pnlBotones);
            pnlClinico.Dock = DockStyle.Fill;
            pnlClinico.Location = new Point(0, 0);
            pnlClinico.Name = "pnlClinico";
            pnlClinico.Padding = new Padding(15);
            pnlClinico.TabIndex = 0;

            // 
            // lblTitPanel
            // 
            lblTitPanel.Dock = DockStyle.Top;
            lblTitPanel.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblTitPanel.ForeColor = Color.FromArgb(60, 80, 65);
            lblTitPanel.Height = 25;
            lblTitPanel.Location = new Point(15, 15);
            lblTitPanel.Name = "lblTitPanel";
            lblTitPanel.Size = new Size(410, 25);
            lblTitPanel.TabIndex = 0;
            lblTitPanel.Text = "DIAGNÓSTICO CLÍNICO EVOLUTIVO";

            // 
            // _lblDiagnosticoBadge
            // 
            _lblDiagnosticoBadge.BackColor = Color.FromArgb(46, 125, 50);
            _lblDiagnosticoBadge.Dock = DockStyle.Top;
            _lblDiagnosticoBadge.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _lblDiagnosticoBadge.ForeColor = Color.White;
            _lblDiagnosticoBadge.Height = 35;
            _lblDiagnosticoBadge.Location = new Point(15, 40);
            _lblDiagnosticoBadge.Name = "_lblDiagnosticoBadge";
            _lblDiagnosticoBadge.Size = new Size(410, 35);
            _lblDiagnosticoBadge.TabIndex = 1;
            _lblDiagnosticoBadge.Text = "Cargando estado...";
            _lblDiagnosticoBadge.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // _lblDetallesClinicos
            // 
            _lblDetallesClinicos.Dock = DockStyle.Top;
            _lblDetallesClinicos.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            _lblDetallesClinicos.ForeColor = Color.FromArgb(50, 65, 55);
            _lblDetallesClinicos.Height = 45;
            _lblDetallesClinicos.Location = new Point(15, 75);
            _lblDetallesClinicos.Name = "_lblDetallesClinicos";
            _lblDetallesClinicos.Padding = new Padding(0, 5, 0, 5);
            _lblDetallesClinicos.Size = new Size(410, 45);
            _lblDetallesClinicos.TabIndex = 2;
            _lblDetallesClinicos.Text = "Evaluando parámetros antropométricos...";

            // 
            // _pnlAlertaCritica
            // 
            _pnlAlertaCritica.BackColor = Color.FromArgb(255, 235, 238);
            _pnlAlertaCritica.Controls.Add(_lblTextoAlerta);
            _pnlAlertaCritica.Dock = DockStyle.Top;
            _pnlAlertaCritica.Height = 55;
            _pnlAlertaCritica.Location = new Point(15, 120);
            _pnlAlertaCritica.Name = "_pnlAlertaCritica";
            _pnlAlertaCritica.Padding = new Padding(8);
            _pnlAlertaCritica.Size = new Size(410, 55);
            _pnlAlertaCritica.TabIndex = 3;
            _pnlAlertaCritica.Visible = false;
            _pnlAlertaCritica.Paint += _pnlAlertaCritica_Paint;

            // 
            // _lblTextoAlerta
            // 
            _lblTextoAlerta.Dock = DockStyle.Fill;
            _lblTextoAlerta.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            _lblTextoAlerta.ForeColor = Color.FromArgb(198, 40, 40);
            _lblTextoAlerta.Location = new Point(8, 8);
            _lblTextoAlerta.Name = "_lblTextoAlerta";
            _lblTextoAlerta.Size = new Size(394, 39);
            _lblTextoAlerta.TabIndex = 0;
            _lblTextoAlerta.Text = "";

            // 
            // lblTitGrilla
            // 
            lblTitGrilla.Dock = DockStyle.Top;
            lblTitGrilla.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblTitGrilla.ForeColor = Color.FromArgb(70, 85, 75);
            lblTitGrilla.Height = 28;
            lblTitGrilla.Location = new Point(15, 175);
            lblTitGrilla.Name = "lblTitGrilla";
            lblTitGrilla.Padding = new Padding(0, 8, 0, 0);
            lblTitGrilla.Size = new Size(410, 28);
            lblTitGrilla.TabIndex = 4;
            lblTitGrilla.Text = "Historial de Mediciones Antropométricas:";

            // 
            // _dgvHistorial
            // 
            _dgvHistorial.AllowUserToAddRows = false;
            _dgvHistorial.AllowUserToDeleteRows = false;
            _dgvHistorial.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _dgvHistorial.BackgroundColor = Color.FromArgb(250, 252, 250);
            _dgvHistorial.BorderStyle = BorderStyle.None;
            _dgvHistorial.Dock = DockStyle.Fill;
            _dgvHistorial.Location = new Point(15, 203);
            _dgvHistorial.Name = "_dgvHistorial";
            _dgvHistorial.ReadOnly = true;
            _dgvHistorial.RowHeadersVisible = false;
            _dgvHistorial.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _dgvHistorial.Size = new Size(410, 347);
            _dgvHistorial.TabIndex = 5;

            // 
            // pnlBotones
            // 
            pnlBotones.Controls.Add(btnNuevaConsulta);
            pnlBotones.Controls.Add(btnPlan);
            pnlBotones.Dock = DockStyle.Bottom;
            pnlBotones.Height = 50;
            pnlBotones.Location = new Point(15, 550);
            pnlBotones.Name = "pnlBotones";
            pnlBotones.Padding = new Padding(0, 8, 0, 0);
            pnlBotones.Size = new Size(410, 50);
            pnlBotones.TabIndex = 6;

            // 
            // btnNuevaConsulta
            // 
            btnNuevaConsulta.BackColor = Color.FromArgb(46, 125, 50);
            btnNuevaConsulta.Cursor = Cursors.Hand;
            btnNuevaConsulta.FlatAppearance.BorderSize = 0;
            btnNuevaConsulta.FlatStyle = FlatStyle.Flat;
            btnNuevaConsulta.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNuevaConsulta.ForeColor = Color.White;
            btnNuevaConsulta.Location = new Point(0, 8);
            btnNuevaConsulta.Name = "btnNuevaConsulta";
            btnNuevaConsulta.Size = new Size(130, 35);
            btnNuevaConsulta.TabIndex = 0;
            btnNuevaConsulta.Text = "➕ Nueva Consulta";
            btnNuevaConsulta.UseVisualStyleBackColor = false;
            btnNuevaConsulta.Click += BtnNuevaConsulta_Click;

            // 
            // btnPlan
            // 
            btnPlan.BackColor = Color.FromArgb(76, 124, 89);
            btnPlan.Cursor = Cursors.Hand;
            btnPlan.FlatAppearance.BorderSize = 0;
            btnPlan.FlatStyle = FlatStyle.Flat;
            btnPlan.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPlan.ForeColor = Color.White;
            btnPlan.Location = new Point(140, 8);
            btnPlan.Name = "btnPlan";
            btnPlan.Size = new Size(130, 35);
            btnPlan.TabIndex = 1;
            btnPlan.Text = "📋 Prescribir Plan";
            btnPlan.UseVisualStyleBackColor = false;
            btnPlan.Click += BtnPlan_Click;

            // 
            // frmGraficarDiagnostico_DNI101
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 248, 245);
            ClientSize = new Size(1164, 700);
            Controls.Add(split);
            Controls.Add(pnlTop);
            MinimumSize = new Size(1000, 650);
            Name = "frmGraficarDiagnostico_DNI101";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CUN07: Graficar Diagnóstico Nutricional Pediátrico";

            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            split.Panel1.ResumeLayout(false);
            split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)split).EndInit();
            split.ResumeLayout(false);
            pnlClinico.ResumeLayout(false);
            _pnlAlertaCritica.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_dgvHistorial).EndInit();
            pnlBotones.ResumeLayout(false);
            ResumeLayout(false);

            split.BringToFront();
        }

        #endregion

        private Panel pnlTop;
        private Label lblTitulo;
        private Label lblSub;
        private Label lblSel;
        private ComboBox _cmbIndicador;
        private SplitContainer split;
        private FormsPlot _formsPlot;
        private Panel pnlClinico;
        private Label lblTitPanel;
        private Label _lblDiagnosticoBadge;
        private Label _lblDetallesClinicos;
        private Panel _pnlAlertaCritica;
        private Label _lblTextoAlerta;
        private Label lblTitGrilla;
        private DataGridView _dgvHistorial;
        private Panel pnlBotones;
        private Button btnNuevaConsulta;
        private Button btnPlan;
    }
}
