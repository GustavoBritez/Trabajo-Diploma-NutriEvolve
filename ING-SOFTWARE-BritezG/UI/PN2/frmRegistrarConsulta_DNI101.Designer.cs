using System.Drawing;
using System.Windows.Forms;
using UI.Controles;

namespace UI.PN2
{
    partial class frmRegistrarConsulta_DNI101
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
            lblTit = new Label();
            lblSub = new Label();
            pnlBody = new Panel();
            lblFecha = new Label();
            _dtpFecha = new DateTimePicker();
            cardMed = new PanelOvalado();
            lblMedTit = new Label();
            lblPeso = new Label();
            _txtPeso = new TextBox();
            lblTalla = new Label();
            _txtTalla = new TextBox();
            lblPerim = new Label();
            _txtPerimetro = new TextBox();
            pnlIMC = new PanelOvalado();
            lblTitIMC = new Label();
            _lblIMCValor = new Label();
            _lblCategoriaPreview = new Label();
            cardClinico = new PanelOvalado();
            lblClinTit = new Label();
            lblLact = new Label();
            _cmbLactancia = new ComboBox();
            lblAlim = new Label();
            _txtAlimentacionComp = new TextBox();
            lblAlerg = new Label();
            _txtAlergias = new TextBox();
            lblAnt = new Label();
            _txtAntFam = new TextBox();
            lblObs = new Label();
            _txtObservaciones = new TextBox();
            btnGuardar = new BotonFuturista();
            btnCancelar = new Button();

            pnlTop.SuspendLayout();
            pnlBody.SuspendLayout();
            cardMed.SuspendLayout();
            pnlIMC.SuspendLayout();
            cardClinico.SuspendLayout();
            SuspendLayout();

            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(76, 124, 89);
            pnlTop.Controls.Add(lblTit);
            pnlTop.Controls.Add(lblSub);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 72;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(20, 12, 20, 10);
            pnlTop.TabIndex = 0;

            // 
            // lblTit
            // 
            lblTit.AutoSize = true;
            lblTit.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTit.ForeColor = Color.White;
            lblTit.Location = new Point(20, 12);
            lblTit.Name = "lblTit";
            lblTit.Size = new Size(420, 21);
            lblTit.TabIndex = 0;
            lblTit.Text = "📝 NUEVA CONSULTA NUTRICIONAL (CUN08)";

            // 
            // lblSub
            // 
            lblSub.AutoSize = true;
            lblSub.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblSub.ForeColor = Color.FromArgb(220, 240, 225);
            lblSub.Location = new Point(22, 40);
            lblSub.Name = "lblSub";
            lblSub.Size = new Size(380, 17);
            lblSub.TabIndex = 1;
            lblSub.Text = "Paciente: -  |  DNI: -  |  Sexo: -  |  Edad: -";

            // 
            // pnlBody
            // 
            pnlBody.AutoScroll = true;
            pnlBody.BackColor = Color.FromArgb(235, 242, 237);
            pnlBody.Controls.Add(lblFecha);
            pnlBody.Controls.Add(_dtpFecha);
            pnlBody.Controls.Add(cardMed);
            pnlBody.Controls.Add(cardClinico);
            pnlBody.Controls.Add(btnGuardar);
            pnlBody.Controls.Add(btnCancelar);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Location = new Point(0, 72);
            pnlBody.Name = "pnlBody";
            pnlBody.Padding = new Padding(20, 15, 20, 15);
            pnlBody.TabIndex = 1;

            // 
            // lblFecha
            // 
            lblFecha.AutoSize = true;
            lblFecha.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblFecha.ForeColor = Color.FromArgb(40, 70, 45);
            lblFecha.Location = new Point(22, 12);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(160, 17);
            lblFecha.TabIndex = 0;
            lblFecha.Text = "Fecha del Control Clínico:";

            // 
            // _dtpFecha
            // 
            _dtpFecha.Font = new Font("Segoe UI", 9.5F);
            _dtpFecha.Format = DateTimePickerFormat.Short;
            _dtpFecha.Location = new Point(22, 34);
            _dtpFecha.Name = "_dtpFecha";
            _dtpFecha.Size = new Size(220, 24);
            _dtpFecha.TabIndex = 1;

            // 
            // cardMed
            // 
            cardMed.BackColor = Color.White;
            cardMed.ColorBorde = Color.FromArgb(190, 215, 195);
            cardMed.Controls.Add(lblMedTit);
            cardMed.Controls.Add(lblPeso);
            cardMed.Controls.Add(_txtPeso);
            cardMed.Controls.Add(lblTalla);
            cardMed.Controls.Add(_txtTalla);
            cardMed.Controls.Add(lblPerim);
            cardMed.Controls.Add(_txtPerimetro);
            cardMed.Controls.Add(pnlIMC);
            cardMed.DibujarBorde = true;
            cardMed.Location = new Point(20, 70);
            cardMed.Name = "cardMed";
            cardMed.RadioBorde = 20;
            cardMed.Size = new Size(700, 155);
            cardMed.TabIndex = 2;

            // 
            // lblMedTit
            // 
            lblMedTit.AutoSize = true;
            lblMedTit.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            lblMedTit.ForeColor = Color.FromArgb(30, 75, 40);
            lblMedTit.Location = new Point(18, 14);
            lblMedTit.Name = "lblMedTit";
            lblMedTit.Size = new Size(250, 19);
            lblMedTit.TabIndex = 0;
            lblMedTit.Text = "📏 Mediciones Antropométricas (OMS)";

            // 
            // lblPeso
            // 
            lblPeso.AutoSize = true;
            lblPeso.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPeso.ForeColor = Color.FromArgb(50, 70, 55);
            lblPeso.Location = new Point(20, 48);
            lblPeso.Name = "lblPeso";
            lblPeso.Size = new Size(65, 15);
            lblPeso.TabIndex = 1;
            lblPeso.Text = "Peso (kg):";

            // 
            // _txtPeso
            // 
            _txtPeso.BorderStyle = BorderStyle.FixedSingle;
            _txtPeso.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _txtPeso.Location = new Point(20, 70);
            _txtPeso.Name = "_txtPeso";
            _txtPeso.Size = new Size(115, 25);
            _txtPeso.TabIndex = 2;
            _txtPeso.TextChanged += _txtAntropo_TextChanged;

            // 
            // lblTalla
            // 
            lblTalla.AutoSize = true;
            lblTalla.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTalla.ForeColor = Color.FromArgb(50, 70, 55);
            lblTalla.Location = new Point(160, 48);
            lblTalla.Name = "lblTalla";
            lblTalla.Size = new Size(100, 15);
            lblTalla.TabIndex = 3;
            lblTalla.Text = "Talla/Long. (cm):";

            // 
            // _txtTalla
            // 
            _txtTalla.BorderStyle = BorderStyle.FixedSingle;
            _txtTalla.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _txtTalla.Location = new Point(160, 70);
            _txtTalla.Name = "_txtTalla";
            _txtTalla.Size = new Size(115, 25);
            _txtTalla.TabIndex = 4;
            _txtTalla.TextChanged += _txtAntropo_TextChanged;

            // 
            // lblPerim
            // 
            lblPerim.AutoSize = true;
            lblPerim.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPerim.ForeColor = Color.FromArgb(50, 70, 55);
            lblPerim.Location = new Point(300, 48);
            lblPerim.Name = "lblPerim";
            lblPerim.Size = new Size(125, 15);
            lblPerim.TabIndex = 5;
            lblPerim.Text = "Perím. Cefálico (cm):";

            // 
            // _txtPerimetro
            // 
            _txtPerimetro.BorderStyle = BorderStyle.FixedSingle;
            _txtPerimetro.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _txtPerimetro.Location = new Point(300, 70);
            _txtPerimetro.Name = "_txtPerimetro";
            _txtPerimetro.Size = new Size(115, 25);
            _txtPerimetro.TabIndex = 6;
            _txtPerimetro.Text = "0";

            // 
            // pnlIMC
            // 
            pnlIMC.BackColor = Color.FromArgb(245, 250, 246);
            pnlIMC.ColorBorde = Color.FromArgb(195, 220, 200);
            pnlIMC.Controls.Add(lblTitIMC);
            pnlIMC.Controls.Add(_lblIMCValor);
            pnlIMC.Controls.Add(_lblCategoriaPreview);
            pnlIMC.DibujarBorde = true;
            pnlIMC.Location = new Point(460, 20);
            pnlIMC.Name = "pnlIMC";
            pnlIMC.RadioBorde = 18;
            pnlIMC.Size = new Size(220, 115);
            pnlIMC.TabIndex = 7;

            // 
            // lblTitIMC
            // 
            lblTitIMC.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblTitIMC.ForeColor = Color.FromArgb(70, 100, 80);
            lblTitIMC.Location = new Point(10, 8);
            lblTitIMC.Name = "lblTitIMC";
            lblTitIMC.Size = new Size(200, 18);
            lblTitIMC.TabIndex = 0;
            lblTitIMC.Text = "IMC CALCULADO";
            lblTitIMC.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // _lblIMCValor
            // 
            _lblIMCValor.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            _lblIMCValor.ForeColor = Color.FromArgb(46, 125, 50);
            _lblIMCValor.Location = new Point(10, 28);
            _lblIMCValor.Name = "_lblIMCValor";
            _lblIMCValor.Size = new Size(200, 36);
            _lblIMCValor.TabIndex = 1;
            _lblIMCValor.Text = "0.00";
            _lblIMCValor.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // _lblCategoriaPreview
            // 
            _lblCategoriaPreview.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            _lblCategoriaPreview.ForeColor = Color.FromArgb(80, 105, 90);
            _lblCategoriaPreview.Location = new Point(10, 68);
            _lblCategoriaPreview.Name = "_lblCategoriaPreview";
            _lblCategoriaPreview.Size = new Size(200, 38);
            _lblCategoriaPreview.TabIndex = 2;
            _lblCategoriaPreview.Text = "Ingrese peso y talla";
            _lblCategoriaPreview.TextAlign = ContentAlignment.TopCenter;

            // 
            // cardClinico
            // 
            cardClinico.BackColor = Color.White;
            cardClinico.ColorBorde = Color.FromArgb(190, 215, 195);
            cardClinico.Controls.Add(lblClinTit);
            cardClinico.Controls.Add(lblLact);
            cardClinico.Controls.Add(_cmbLactancia);
            cardClinico.Controls.Add(lblAlim);
            cardClinico.Controls.Add(_txtAlimentacionComp);
            cardClinico.Controls.Add(lblAlerg);
            cardClinico.Controls.Add(_txtAlergias);
            cardClinico.Controls.Add(lblAnt);
            cardClinico.Controls.Add(_txtAntFam);
            cardClinico.Controls.Add(lblObs);
            cardClinico.Controls.Add(_txtObservaciones);
            cardClinico.DibujarBorde = true;
            cardClinico.Location = new Point(20, 240);
            cardClinico.Name = "cardClinico";
            cardClinico.RadioBorde = 20;
            cardClinico.Size = new Size(700, 275);
            cardClinico.TabIndex = 3;

            // 
            // lblClinTit
            // 
            lblClinTit.AutoSize = true;
            lblClinTit.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            lblClinTit.ForeColor = Color.FromArgb(30, 75, 40);
            lblClinTit.Location = new Point(18, 14);
            lblClinTit.Name = "lblClinTit";
            lblClinTit.Size = new Size(290, 19);
            lblClinTit.TabIndex = 0;
            lblClinTit.Text = "🩺 Evaluación Clínica y Hábitos Alimentarios";

            // 
            // lblLact
            // 
            lblLact.AutoSize = true;
            lblLact.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblLact.ForeColor = Color.FromArgb(50, 70, 55);
            lblLact.Location = new Point(20, 44);
            lblLact.Name = "lblLact";
            lblLact.Size = new Size(64, 15);
            lblLact.TabIndex = 1;
            lblLact.Text = "Lactancia:";

            // 
            // _cmbLactancia
            // 
            _cmbLactancia.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbLactancia.Font = new Font("Segoe UI", 9.5F);
            _cmbLactancia.Items.AddRange(new object[] { "Materna Exclusiva", "Predominante / Parcial", "Fórmula Infantil", "Destetado / Sin lactancia" });
            _cmbLactancia.Location = new Point(20, 66);
            _cmbLactancia.Name = "_cmbLactancia";
            _cmbLactancia.Size = new Size(210, 25);
            _cmbLactancia.TabIndex = 2;

            // 
            // lblAlim
            // 
            lblAlim.AutoSize = true;
            lblAlim.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAlim.ForeColor = Color.FromArgb(50, 70, 55);
            lblAlim.Location = new Point(255, 44);
            lblAlim.Name = "lblAlim";
            lblAlim.Size = new Size(183, 15);
            lblAlim.TabIndex = 3;
            lblAlim.Text = "Alimentación Complementaria:";

            // 
            // _txtAlimentacionComp
            // 
            _txtAlimentacionComp.BorderStyle = BorderStyle.FixedSingle;
            _txtAlimentacionComp.Font = new Font("Segoe UI", 9.5F);
            _txtAlimentacionComp.Location = new Point(255, 66);
            _txtAlimentacionComp.Name = "_txtAlimentacionComp";
            _txtAlimentacionComp.Size = new Size(425, 24);
            _txtAlimentacionComp.TabIndex = 4;

            // 
            // lblAlerg
            // 
            lblAlerg.AutoSize = true;
            lblAlerg.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAlerg.ForeColor = Color.FromArgb(50, 70, 55);
            lblAlerg.Location = new Point(20, 104);
            lblAlerg.Name = "lblAlerg";
            lblAlerg.Size = new Size(141, 15);
            lblAlerg.TabIndex = 5;
            lblAlerg.Text = "Alergias / Intolerancias:";

            // 
            // _txtAlergias
            // 
            _txtAlergias.BorderStyle = BorderStyle.FixedSingle;
            _txtAlergias.Font = new Font("Segoe UI", 9.5F);
            _txtAlergias.Location = new Point(20, 126);
            _txtAlergias.Name = "_txtAlergias";
            _txtAlergias.Size = new Size(310, 24);
            _txtAlergias.TabIndex = 6;

            // 
            // lblAnt
            // 
            lblAnt.AutoSize = true;
            lblAnt.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAnt.ForeColor = Color.FromArgb(50, 70, 55);
            lblAnt.Location = new Point(355, 104);
            lblAnt.Name = "lblAnt";
            lblAnt.Size = new Size(153, 15);
            lblAnt.TabIndex = 7;
            lblAnt.Text = "Antecedentes Familiares:";

            // 
            // _txtAntFam
            // 
            _txtAntFam.BorderStyle = BorderStyle.FixedSingle;
            _txtAntFam.Font = new Font("Segoe UI", 9.5F);
            _txtAntFam.Location = new Point(355, 126);
            _txtAntFam.Name = "_txtAntFam";
            _txtAntFam.Size = new Size(325, 24);
            _txtAntFam.TabIndex = 8;

            // 
            // lblObs
            // 
            lblObs.AutoSize = true;
            lblObs.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblObs.ForeColor = Color.FromArgb(50, 70, 55);
            lblObs.Location = new Point(20, 162);
            lblObs.Name = "lblObs";
            lblObs.Size = new Size(224, 15);
            lblObs.TabIndex = 9;
            lblObs.Text = "Observaciones Médicas y Evolución:";

            // 
            // _txtObservaciones
            // 
            _txtObservaciones.BorderStyle = BorderStyle.FixedSingle;
            _txtObservaciones.Font = new Font("Segoe UI", 9.5F);
            _txtObservaciones.Location = new Point(20, 184);
            _txtObservaciones.Multiline = true;
            _txtObservaciones.Name = "_txtObservaciones";
            _txtObservaciones.ScrollBars = ScrollBars.Vertical;
            _txtObservaciones.Size = new Size(660, 75);
            _txtObservaciones.TabIndex = 10;

            // 
            // btnGuardar
            // 
            btnGuardar.ColorGlow = Color.DarkSeaGreen;
            btnGuardar.ColorPrincipal = Color.FromArgb(76, 124, 89);
            btnGuardar.ColorSecundario = Color.FromArgb(50, 85, 60);
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(210, 530);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.RadioBorde = 26;
            btnGuardar.Size = new Size(280, 46);
            btnGuardar.TabIndex = 4;
            btnGuardar.Text = "💾  Guardar Consulta Nutricional";
            btnGuardar.Click += BtnGuardar_Click;

            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(235, 240, 236);
            btnCancelar.Cursor = Cursors.Hand;
            btnCancelar.FlatAppearance.BorderColor = Color.FromArgb(190, 215, 195);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.FromArgb(80, 100, 85);
            btnCancelar.Location = new Point(510, 530);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 46);
            btnCancelar.TabIndex = 5;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += BtnCancelar_Click;

            // 
            // frmRegistrarConsulta_DNI101
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(235, 242, 237);
            ClientSize = new Size(740, 680);
            Controls.Add(pnlBody);
            Controls.Add(pnlTop);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmRegistrarConsulta_DNI101";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CUN08: Registrar Consulta Nutricional";

            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            cardMed.ResumeLayout(false);
            cardMed.PerformLayout();
            pnlIMC.ResumeLayout(false);
            cardClinico.ResumeLayout(false);
            cardClinico.PerformLayout();
            pnlBody.ResumeLayout(false);
            pnlBody.PerformLayout();
            ResumeLayout(false);

            pnlBody.BringToFront();
        }

        #endregion

        private Panel pnlTop;
        private Label lblTit;
        private Label lblSub;
        private Panel pnlBody;
        private Label lblFecha;
        private DateTimePicker _dtpFecha;
        private PanelOvalado cardMed;
        private Label lblMedTit;
        private Label lblPeso;
        private TextBox _txtPeso;
        private Label lblTalla;
        private TextBox _txtTalla;
        private Label lblPerim;
        private TextBox _txtPerimetro;
        private PanelOvalado pnlIMC;
        private Label lblTitIMC;
        private Label _lblIMCValor;
        private Label _lblCategoriaPreview;
        private PanelOvalado cardClinico;
        private Label lblClinTit;
        private Label lblLact;
        private ComboBox _cmbLactancia;
        private Label lblAlim;
        private TextBox _txtAlimentacionComp;
        private Label lblAlerg;
        private TextBox _txtAlergias;
        private Label lblAnt;
        private TextBox _txtAntFam;
        private Label lblObs;
        private TextBox _txtObservaciones;
        private BotonFuturista btnGuardar;
        private Button btnCancelar;
    }
}
