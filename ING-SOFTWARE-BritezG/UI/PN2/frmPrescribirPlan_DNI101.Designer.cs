using System.Drawing;
using System.Windows.Forms;
using UI.Controles;

namespace UI.PN2
{
  partial class frmPrescribirPlan_DNI101
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
      cardNutri = new PanelOvalado();
      lblCard1Tit = new Label();
      lblKcal = new Label();
      _txtKcal = new TextBox();
      lblKcalUnidad = new Label();
      lblDistTit = new Label();
      lblCarb = new Label();
      _txtCarbos = new TextBox();
      _lblGramosCarbos = new Label();
      lblProt = new Label();
      _txtProte = new TextBox();
      _lblGramosProte = new Label();
      lblGrasa = new Label();
      _txtGrasas = new TextBox();
      _lblGramosGrasas = new Label();
      pnlTotal = new PanelOvalado();
      lblSumaTit = new Label();
      _lblTotalPct = new Label();
      _lblTotalStatus = new Label();
      lblReq = new Label();
      cardPautas = new PanelOvalado();
      lblCard2Tit = new Label();
      lblMetas = new Label();
      _txtMetas = new TextBox();
      lblPautas = new Label();
      _txtPautas = new TextBox();
      btnGuardar = new BotonFuturista();
      btnCancelar = new Button();
      btnComprobante = new BotonFuturista();
      btnAyudaContextual = new BotonFuturista();

      pnlTop.SuspendLayout();
      pnlBody.SuspendLayout();
      cardNutri.SuspendLayout();
      pnlTotal.SuspendLayout();
      cardPautas.SuspendLayout();
      SuspendLayout();

      // 
      // pnlTop
      // 
      pnlTop.BackColor = Color.FromArgb(76, 124, 89);
      pnlTop.Controls.Add(lblTit);
      pnlTop.Controls.Add(lblSub);
      pnlTop.Controls.Add(btnAyudaContextual);
      pnlTop.Dock = DockStyle.Top;
      pnlTop.Height = 72;
      pnlTop.Location = new Point(0, 0);
      pnlTop.Name = "pnlTop";
      pnlTop.Padding = new Padding(20, 12, 20, 10);
      pnlTop.TabIndex = 0;

      // 
      // btnAyudaContextual
      // 
      btnAyudaContextual.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      btnAyudaContextual.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
      btnAyudaContextual.Location = new Point(625, 18);
      btnAyudaContextual.Name = "btnAyudaContextual";
      btnAyudaContextual.Size = new Size(95, 36);
      btnAyudaContextual.TabIndex = 2;
      btnAyudaContextual.Text = "? Ayuda";
      btnAyudaContextual.Click += BtnAyudaContextual_Click;

      // 
      // lblTit
      // 
      lblTit.AutoSize = true;
      lblTit.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
      lblTit.ForeColor = Color.White;
      lblTit.Location = new Point(20, 12);
      lblTit.Name = "lblTit";
      lblTit.Size = new Size(500, 21);
      lblTit.TabIndex = 0;
      lblTit.Text = " PRESCRIBIR PLAN ALIMENTARIO PERSONALIZADO (CUN09)";

      // 
      // lblSub
      // 
      lblSub.AutoSize = true;
      lblSub.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
      lblSub.ForeColor = Color.FromArgb(220, 240, 225);
      lblSub.Location = new Point(22, 40);
      lblSub.Name = "lblSub";
      lblSub.Size = new Size(400, 17);
      lblSub.TabIndex = 1;
      lblSub.Text = "Paciente: - | DNI: - | Sexo: - | Edad: -";

      // 
      // pnlBody
      // 
      pnlBody.AutoScroll = true;
      pnlBody.BackColor = Color.FromArgb(235, 242, 237);
      pnlBody.Controls.Add(cardNutri);
      pnlBody.Controls.Add(cardPautas);
      pnlBody.Controls.Add(btnComprobante);
      pnlBody.Controls.Add(btnGuardar);
      pnlBody.Controls.Add(btnCancelar);
      pnlBody.Dock = DockStyle.Fill;
      pnlBody.Location = new Point(0, 72);
      pnlBody.Name = "pnlBody";
      pnlBody.Padding = new Padding(20, 15, 20, 15);
      pnlBody.TabIndex = 1;

      // 
      // cardNutri
      // 
      cardNutri.BackColor = Color.White;
      cardNutri.ColorBorde = Color.FromArgb(190, 215, 195);
      cardNutri.Controls.Add(lblCard1Tit);
      cardNutri.Controls.Add(lblKcal);
      cardNutri.Controls.Add(_txtKcal);
      cardNutri.Controls.Add(lblKcalUnidad);
      cardNutri.Controls.Add(lblDistTit);
      cardNutri.Controls.Add(lblCarb);
      cardNutri.Controls.Add(_txtCarbos);
      cardNutri.Controls.Add(_lblGramosCarbos);
      cardNutri.Controls.Add(lblProt);
      cardNutri.Controls.Add(_txtProte);
      cardNutri.Controls.Add(_lblGramosProte);
      cardNutri.Controls.Add(lblGrasa);
      cardNutri.Controls.Add(_txtGrasas);
      cardNutri.Controls.Add(_lblGramosGrasas);
      cardNutri.Controls.Add(pnlTotal);
      cardNutri.DibujarBorde = true;
      cardNutri.Location = new Point(20, 15);
      cardNutri.Name = "cardNutri";
      cardNutri.RadioBorde = 20;
      cardNutri.Size = new Size(700, 235);
      cardNutri.TabIndex = 0;

      // 
      // lblCard1Tit
      // 
      lblCard1Tit.AutoSize = true;
      lblCard1Tit.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
      lblCard1Tit.ForeColor = Color.FromArgb(30, 75, 40);
      lblCard1Tit.Location = new Point(18, 14);
      lblCard1Tit.Name = "lblCard1Tit";
      lblCard1Tit.Size = new Size(410, 19);
      lblCard1Tit.TabIndex = 0;
      lblCard1Tit.Text = " Requerimiento Calórico y Distribución de Macronutrientes";

      // 
      // lblKcal
      // 
      lblKcal.AutoSize = true;
      lblKcal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
      lblKcal.ForeColor = Color.FromArgb(50, 70, 55);
      lblKcal.Location = new Point(20, 44);
      lblKcal.Name = "lblKcal";
      lblKcal.Size = new Size(230, 15);
      lblKcal.TabIndex = 1;
      lblKcal.Text = "Requerimiento Calórico Total (kcal/día):";

      // 
      // _txtKcal
      // 
      _txtKcal.BorderStyle = BorderStyle.FixedSingle;
      _txtKcal.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
      _txtKcal.ForeColor = Color.FromArgb(30, 60, 40);
      _txtKcal.Location = new Point(20, 68);
      _txtKcal.Name = "_txtKcal";
      _txtKcal.Size = new Size(160, 28);
      _txtKcal.TabIndex = 2;
      _txtKcal.Text = "1400";
      _txtKcal.TextChanged += _txtKcal_TextChanged;

      // 
      // lblKcalUnidad
      // 
      lblKcalUnidad.AutoSize = true;
      lblKcalUnidad.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
      lblKcalUnidad.ForeColor = Color.FromArgb(100, 120, 105);
      lblKcalUnidad.Location = new Point(190, 74);
      lblKcalUnidad.Name = "lblKcalUnidad";
      lblKcalUnidad.Size = new Size(140, 15);
      lblKcalUnidad.TabIndex = 3;
      lblKcalUnidad.Text = "kcal / día recomendadas";

      // 
      // lblDistTit
      // 
      lblDistTit.AutoSize = true;
      lblDistTit.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
      lblDistTit.ForeColor = Color.FromArgb(70, 95, 75);
      lblDistTit.Location = new Point(20, 110);
      lblDistTit.Name = "lblDistTit";
      lblDistTit.Size = new Size(210, 15);
      lblDistTit.TabIndex = 4;
      lblDistTit.Text = "Distribución Porcentual y Gramaje:";

      // 
      // lblCarb
      // 
      lblCarb.AutoSize = true;
      lblCarb.Font = new Font("Segoe UI", 9F);
      lblCarb.Location = new Point(20, 134);
      lblCarb.Name = "lblCarb";
      lblCarb.Size = new Size(110, 15);
      lblCarb.TabIndex = 5;
      lblCarb.Text = "Carbohidratos (%):";

      // 
      // _txtCarbos
      // 
      _txtCarbos.BorderStyle = BorderStyle.FixedSingle;
      _txtCarbos.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
      _txtCarbos.Location = new Point(20, 156);
      _txtCarbos.Name = "_txtCarbos";
      _txtCarbos.Size = new Size(125, 25);
      _txtCarbos.TabIndex = 6;
      _txtCarbos.Text = "55";
      _txtCarbos.TextChanged += _txtNutri_TextChanged;

      // 
      // _lblGramosCarbos
      // 
      _lblGramosCarbos.AutoSize = true;
      _lblGramosCarbos.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
      _lblGramosCarbos.ForeColor = Color.FromArgb(46, 125, 50);
      _lblGramosCarbos.Location = new Point(20, 188);
      _lblGramosCarbos.Name = "_lblGramosCarbos";
      _lblGramosCarbos.Size = new Size(115, 15);
      _lblGramosCarbos.TabIndex = 7;
      _lblGramosCarbos.Text = "= 192.5 g (4 kcal/g)";

      // 
      // lblProt
      // 
      lblProt.AutoSize = true;
      lblProt.Font = new Font("Segoe UI", 9F);
      lblProt.Location = new Point(175, 134);
      lblProt.Name = "lblProt";
      lblProt.Size = new Size(84, 15);
      lblProt.TabIndex = 8;
      lblProt.Text = "Proteínas (%):";

      // 
      // _txtProte
      // 
      _txtProte.BorderStyle = BorderStyle.FixedSingle;
      _txtProte.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
      _txtProte.Location = new Point(175, 156);
      _txtProte.Name = "_txtProte";
      _txtProte.Size = new Size(125, 25);
      _txtProte.TabIndex = 9;
      _txtProte.Text = "15";
      _txtProte.TextChanged += _txtNutri_TextChanged;

      // 
      // _lblGramosProte
      // 
      _lblGramosProte.AutoSize = true;
      _lblGramosProte.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
      _lblGramosProte.ForeColor = Color.FromArgb(25, 118, 210);
      _lblGramosProte.Location = new Point(175, 188);
      _lblGramosProte.Name = "_lblGramosProte";
      _lblGramosProte.Size = new Size(110, 15);
      _lblGramosProte.TabIndex = 10;
      _lblGramosProte.Text = "= 52.5 g (4 kcal/g)";

      // 
      // lblGrasa
      // 
      lblGrasa.AutoSize = true;
      lblGrasa.Font = new Font("Segoe UI", 9F);
      lblGrasa.Location = new Point(330, 134);
      lblGrasa.Name = "lblGrasa";
      lblGrasa.Size = new Size(119, 15);
      lblGrasa.TabIndex = 11;
      lblGrasa.Text = "Lípidos / Grasas (%):";

      // 
      // _txtGrasas
      // 
      _txtGrasas.BorderStyle = BorderStyle.FixedSingle;
      _txtGrasas.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
      _txtGrasas.Location = new Point(330, 156);
      _txtGrasas.Name = "_txtGrasas";
      _txtGrasas.Size = new Size(125, 25);
      _txtGrasas.TabIndex = 12;
      _txtGrasas.Text = "30";
      _txtGrasas.TextChanged += _txtNutri_TextChanged;

      // 
      // _lblGramosGrasas
      // 
      _lblGramosGrasas.AutoSize = true;
      _lblGramosGrasas.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
      _lblGramosGrasas.ForeColor = Color.FromArgb(230, 124, 15);
      _lblGramosGrasas.Location = new Point(330, 188);
      _lblGramosGrasas.Name = "_lblGramosGrasas";
      _lblGramosGrasas.Size = new Size(110, 15);
      _lblGramosGrasas.TabIndex = 13;
      _lblGramosGrasas.Text = "= 46.7 g (9 kcal/g)";

      // 
      // pnlTotal
      // 
      pnlTotal.BackColor = Color.FromArgb(245, 250, 246);
      pnlTotal.ColorBorde = Color.FromArgb(195, 220, 200);
      pnlTotal.Controls.Add(lblSumaTit);
      pnlTotal.Controls.Add(_lblTotalPct);
      pnlTotal.Controls.Add(_lblTotalStatus);
      pnlTotal.Controls.Add(lblReq);
      pnlTotal.DibujarBorde = true;
      pnlTotal.Location = new Point(480, 40);
      pnlTotal.Name = "pnlTotal";
      pnlTotal.RadioBorde = 18;
      pnlTotal.Size = new Size(200, 175);
      pnlTotal.TabIndex = 14;

      // 
      // lblSumaTit
      // 
      lblSumaTit.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
      lblSumaTit.ForeColor = Color.FromArgb(70, 100, 80);
      lblSumaTit.Location = new Point(10, 14);
      lblSumaTit.Name = "lblSumaTit";
      lblSumaTit.Size = new Size(180, 18);
      lblSumaTit.TabIndex = 0;
      lblSumaTit.Text = "BALANCE TOTAL";
      lblSumaTit.TextAlign = ContentAlignment.MiddleCenter;

      // 
      // _lblTotalPct
      // 
      _lblTotalPct.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
      _lblTotalPct.ForeColor = Color.FromArgb(46, 125, 50);
      _lblTotalPct.Location = new Point(10, 42);
      _lblTotalPct.Name = "_lblTotalPct";
      _lblTotalPct.Size = new Size(180, 40);
      _lblTotalPct.TabIndex = 1;
      _lblTotalPct.Text = "100.0%";
      _lblTotalPct.TextAlign = ContentAlignment.MiddleCenter;

      // 
      // _lblTotalStatus
      // 
      _lblTotalStatus.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
      _lblTotalStatus.ForeColor = Color.FromArgb(46, 125, 50);
      _lblTotalStatus.Location = new Point(10, 92);
      _lblTotalStatus.Name = "_lblTotalStatus";
      _lblTotalStatus.Size = new Size(180, 20);
      _lblTotalStatus.TabIndex = 2;
      _lblTotalStatus.Text = " Distribución válida (100%)";
      _lblTotalStatus.TextAlign = ContentAlignment.MiddleCenter;

      // 
      // lblReq
      // 
      lblReq.Font = new Font("Segoe UI", 7.5F);
      lblReq.ForeColor = Color.FromArgb(100, 125, 110);
      lblReq.Location = new Point(10, 118);
      lblReq.Name = "lblReq";
      lblReq.Size = new Size(180, 42);
      lblReq.TabIndex = 3;
      lblReq.Text = "La suma de macronutrientes debe totalizar exactamente el 100%.";
      lblReq.TextAlign = ContentAlignment.TopCenter;

      // 
      // cardPautas
      // 
      cardPautas.BackColor = Color.White;
      cardPautas.ColorBorde = Color.FromArgb(190, 215, 195);
      cardPautas.Controls.Add(lblCard2Tit);
      cardPautas.Controls.Add(lblMetas);
      cardPautas.Controls.Add(_txtMetas);
      cardPautas.Controls.Add(lblPautas);
      cardPautas.Controls.Add(_txtPautas);
      cardPautas.DibujarBorde = true;
      cardPautas.Location = new Point(20, 265);
      cardPautas.Name = "cardPautas";
      cardPautas.RadioBorde = 20;
      cardPautas.Size = new Size(700, 275);
      cardPautas.TabIndex = 1;

      // 
      // lblCard2Tit
      // 
      lblCard2Tit.AutoSize = true;
      lblCard2Tit.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
      lblCard2Tit.ForeColor = Color.FromArgb(30, 75, 40);
      lblCard2Tit.Location = new Point(18, 14);
      lblCard2Tit.Name = "lblCard2Tit";
      lblCard2Tit.Size = new Size(390, 19);
      lblCard2Tit.TabIndex = 0;
      lblCard2Tit.Text = " Objetivos Terapéuticos y Recomendaciones Familiares";

      // 
      // lblMetas
      // 
      lblMetas.AutoSize = true;
      lblMetas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
      lblMetas.ForeColor = Color.FromArgb(50, 70, 55);
      lblMetas.Location = new Point(20, 44);
      lblMetas.Name = "lblMetas";
      lblMetas.Size = new Size(350, 15);
      lblMetas.TabIndex = 1;
      lblMetas.Text = "Metas del Tratamiento (Objetivos clínicos a mediano plazo):";

      // 
      // _txtMetas
      // 
      _txtMetas.BorderStyle = BorderStyle.FixedSingle;
      _txtMetas.Font = new Font("Segoe UI", 9.5F);
      _txtMetas.Location = new Point(20, 68);
      _txtMetas.Name = "_txtMetas";
      _txtMetas.Size = new Size(660, 24);
      _txtMetas.TabIndex = 2;
      _txtMetas.Text = "Mantener carril percentilar normopeso y fomentar hidratación.";

      // 
      // lblPautas
      // 
      lblPautas.AutoSize = true;
      lblPautas.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
      lblPautas.ForeColor = Color.FromArgb(50, 70, 55);
      lblPautas.Location = new Point(20, 108);
      lblPautas.Name = "lblPautas";
      lblPautas.Size = new Size(470, 15);
      lblPautas.TabIndex = 3;
      lblPautas.Text = "Pautas Nutricionales y Familiares (Alimentos sugeridos, conductas y frecuencias):";

      // 
      // _txtPautas
      // 
      _txtPautas.BorderStyle = BorderStyle.FixedSingle;
      _txtPautas.Font = new Font("Segoe UI", 9.5F);
      _txtPautas.Location = new Point(20, 132);
      _txtPautas.Multiline = true;
      _txtPautas.Name = "_txtPautas";
      _txtPautas.ScrollBars = ScrollBars.Vertical;
      _txtPautas.Size = new Size(660, 122);
      _txtPautas.TabIndex = 4;
      _txtPautas.Text = "• Priorizar alimentos naturales, frutas y verduras variadas.\r\n• Evitar ultraprocesados, bebidas azucaradas y harinas refinadas.\r\n• Fomentar 60 minutos diarios de juego activo o actividad física.";

      // 
      // btnComprobante
      // 
      btnComprobante.ColorGlow = Color.DarkSeaGreen;
      btnComprobante.ColorPrincipal = Color.FromArgb(40, 110, 65);
      btnComprobante.ColorSecundario = Color.FromArgb(28, 75, 45);
      btnComprobante.Cursor = Cursors.Hand;
      btnComprobante.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
      btnComprobante.ForeColor = Color.White;
      btnComprobante.Location = new Point(20, 555);
      btnComprobante.Name = "btnComprobante";
      btnComprobante.RadioBorde = 26;
      btnComprobante.Size = new Size(195, 46);
      btnComprobante.TabIndex = 4;
      btnComprobante.Text = "Ver / Imprimir Ficha";
      btnComprobante.Click += BtnComprobante_Click;

      // 
      // btnGuardar
      // 
      btnGuardar.ColorGlow = Color.DarkSeaGreen;
      btnGuardar.ColorPrincipal = Color.FromArgb(76, 124, 89);
      btnGuardar.ColorSecundario = Color.FromArgb(50, 85, 60);
      btnGuardar.Cursor = Cursors.Hand;
      btnGuardar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
      btnGuardar.ForeColor = Color.White;
      btnGuardar.Location = new Point(230, 555);
      btnGuardar.Name = "btnGuardar";
      btnGuardar.RadioBorde = 26;
      btnGuardar.Size = new Size(260, 46);
      btnGuardar.TabIndex = 2;
      btnGuardar.Text = " Prescribir y Guardar Plan";
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
      btnCancelar.Location = new Point(510, 555);
      btnCancelar.Name = "btnCancelar";
      btnCancelar.Size = new Size(120, 46);
      btnCancelar.TabIndex = 3;
      btnCancelar.Text = "Cancelar";
      btnCancelar.UseVisualStyleBackColor = false;
      btnCancelar.Click += BtnCancelar_Click;

      // 
      // frmPrescribirPlan_DNI101
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = Color.FromArgb(235, 242, 237);
      ClientSize = new Size(740, 710);
      Controls.Add(pnlBody);
      Controls.Add(pnlTop);
      FormBorderStyle = FormBorderStyle.FixedDialog;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "frmPrescribirPlan_DNI101";
      StartPosition = FormStartPosition.CenterParent;
      Text = "CUN09: Prescribir Plan Alimentario";

      pnlTop.ResumeLayout(false);
      pnlTop.PerformLayout();
      cardNutri.ResumeLayout(false);
      cardNutri.PerformLayout();
      pnlTotal.ResumeLayout(false);
      cardPautas.ResumeLayout(false);
      cardPautas.PerformLayout();
      pnlBody.ResumeLayout(false);
      ResumeLayout(false);

      pnlBody.BringToFront();
    }

    #endregion

    private Panel pnlTop;
    private Label lblTit;
    private Label lblSub;
    private Panel pnlBody;
    private PanelOvalado cardNutri;
    private Label lblCard1Tit;
    private Label lblKcal;
    private TextBox _txtKcal;
    private Label lblKcalUnidad;
    private Label lblDistTit;
    private Label lblCarb;
    private TextBox _txtCarbos;
    private Label _lblGramosCarbos;
    private Label lblProt;
    private TextBox _txtProte;
    private Label _lblGramosProte;
    private Label lblGrasa;
    private TextBox _txtGrasas;
    private Label _lblGramosGrasas;
    private PanelOvalado pnlTotal;
    private Label lblSumaTit;
    private Label _lblTotalPct;
    private Label _lblTotalStatus;
    private Label lblReq;
    private PanelOvalado cardPautas;
    private Label lblCard2Tit;
    private Label lblMetas;
    private TextBox _txtMetas;
    private Label lblPautas;
    private TextBox _txtPautas;
    private BotonFuturista btnGuardar;
    private Button btnCancelar;
    private BotonFuturista btnComprobante;
    private BotonFuturista btnAyudaContextual;
  }
}
