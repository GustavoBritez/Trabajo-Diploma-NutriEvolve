namespace UI
{
    partial class FormSeguimientoNutricional_DNI101
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
            panelHeader = new Panel();
            lblTituloModulo = new Label();
            panelPacienteHeader = new Panel();
            lblDniPrompt = new Label();
            txtDniNiño = new TextBox();
            btnBuscarPaciente = new UI.Controles.BotonFuturista();
            lblInfoPaciente = new Label();
            tabControlMain = new TabControl();
            tabAntropometria = new TabPage();
            gbMediciones = new GroupBox();
            lblPeso = new Label();
            numPeso = new NumericUpDown();
            lblTalla = new Label();
            numTalla = new NumericUpDown();
            lblPC = new Label();
            numPerimetroCefalico = new NumericUpDown();
            lblCC = new Label();
            numCircunferenciaCintura = new NumericUpDown();
            btnCalcularZScores = new UI.Controles.BotonFuturista();
            gbResultados = new GroupBox();
            lblIMCValue = new Label();
            lblZPesoValue = new Label();
            lblZTallaValue = new Label();
            lblZIMCValue = new Label();
            lblZPCValue = new Label();
            lblPercentilValue = new Label();
            gbGrafico = new GroupBox();
            lblTipoGrafico = new Label();
            cmbTipoGrafico = new ComboBox();
            panelGraficoCurvas = new Panel();
            tabDiagnostico = new TabPage();
            gbDiagnostico = new GroupBox();
            lblClasificacionOMS = new Label();
            lblDetallesPrompt = new Label();
            txtDetallesClinicos = new TextBox();
            panelAlertaVisual = new Panel();
            lblAlertaTitulo = new Label();
            lblAlertaMensaje = new Label();
            tabAnamnesis = new TabPage();
            gbAnamnesis = new GroupBox();
            lblLactancia = new Label();
            cmbTipoLactancia = new ComboBox();
            lblAliComp = new Label();
            txtAlimentacionComplementaria = new TextBox();
            lblAlergias = new Label();
            txtAlergias = new TextBox();
            lblAntecedentes = new Label();
            txtAntecedentesFamiliares = new TextBox();
            gbRecordatorio = new GroupBox();
            lblDesayuno = new Label();
            txtDesayuno = new TextBox();
            lblAlmuerzo = new Label();
            txtAlmuerzo = new TextBox();
            lblMerienda = new Label();
            txtMerienda = new TextBox();
            lblCena = new Label();
            txtCena = new TextBox();
            lblColaciones = new Label();
            txtColaciones = new TextBox();
            lblFrecuencia = new Label();
            txtFrecuenciaAlimentos = new TextBox();
            tabPlanAlimentario = new TabPage();
            gbPlan = new GroupBox();
            lblKcal = new Label();
            numKcal = new NumericUpDown();
            lblCHO = new Label();
            numPctCHO = new NumericUpDown();
            lblPROT = new Label();
            numPctPROT = new NumericUpDown();
            lblGRASAS = new Label();
            numPctGrasas = new NumericUpDown();
            lblSumaMacronutrientes = new Label();
            lblPautas = new Label();
            txtPautasFamiliares = new TextBox();
            lblMetas = new Label();
            txtMetasSalud = new TextBox();
            btnGuardarConsulta = new UI.Controles.BotonFuturista();
            gbHistorial = new GroupBox();
            dgvHistorialConsultas = new DataGridView();

            panelHeader.SuspendLayout();
            panelPacienteHeader.SuspendLayout();
            tabControlMain.SuspendLayout();
            tabAntropometria.SuspendLayout();
            gbMediciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPeso).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTalla).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPerimetroCefalico).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCircunferenciaCintura).BeginInit();
            gbResultados.SuspendLayout();
            gbGrafico.SuspendLayout();
            tabDiagnostico.SuspendLayout();
            gbDiagnostico.SuspendLayout();
            panelAlertaVisual.SuspendLayout();
            tabAnamnesis.SuspendLayout();
            gbAnamnesis.SuspendLayout();
            gbRecordatorio.SuspendLayout();
            tabPlanAlimentario.SuspendLayout();
            gbPlan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numKcal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPctCHO).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPctPROT).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPctGrasas).BeginInit();
            gbHistorial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistorialConsultas).BeginInit();
            SuspendLayout();

            // panelHeader
            panelHeader.BackColor = Color.FromArgb(46, 94, 67);
            panelHeader.Controls.Add(lblTituloModulo);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1100, 50);
            panelHeader.TabIndex = 0;

            // lblTituloModulo
            lblTituloModulo.AutoSize = true;
            lblTituloModulo.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            lblTituloModulo.ForeColor = Color.White;
            lblTituloModulo.Location = new Point(15, 12);
            lblTituloModulo.Name = "lblTituloModulo";
            lblTituloModulo.Size = new Size(490, 25);
            lblTituloModulo.Text = "🥗 SEGUIMIENTO NUTRICIONAL PEDIÁTRICO (PN2)";

            // panelPacienteHeader
            panelPacienteHeader.BackColor = Color.FromArgb(225, 240, 228);
            panelPacienteHeader.Controls.Add(lblDniPrompt);
            panelPacienteHeader.Controls.Add(txtDniNiño);
            panelPacienteHeader.Controls.Add(btnBuscarPaciente);
            panelPacienteHeader.Controls.Add(lblInfoPaciente);
            panelPacienteHeader.Dock = DockStyle.Top;
            panelPacienteHeader.Location = new Point(0, 50);
            panelPacienteHeader.Name = "panelPacienteHeader";
            panelPacienteHeader.Size = new Size(1100, 60);
            panelPacienteHeader.TabIndex = 1;

            // lblDniPrompt
            lblDniPrompt.AutoSize = true;
            lblDniPrompt.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDniPrompt.ForeColor = Color.FromArgb(30, 60, 40);
            lblDniPrompt.Location = new Point(15, 20);
            lblDniPrompt.Name = "lblDniPrompt";
            lblDniPrompt.Size = new Size(74, 19);
            lblDniPrompt.Text = "DNI Niño:";

            // txtDniNiño
            txtDniNiño.Font = new Font("Segoe UI", 10F);
            txtDniNiño.Location = new Point(95, 17);
            txtDniNiño.Name = "txtDniNiño";
            txtDniNiño.Size = new Size(140, 25);
            txtDniNiño.TabIndex = 0;

            // btnBuscarPaciente
            btnBuscarPaciente.ColorPrincipal = Color.FromArgb(46, 94, 67);
            btnBuscarPaciente.ColorSecundario = Color.FromArgb(32, 68, 48);
            btnBuscarPaciente.ColorGlow = Color.DarkSeaGreen;
            btnBuscarPaciente.RadioBorde = 24;
            btnBuscarPaciente.Location = new Point(245, 13);
            btnBuscarPaciente.Name = "btnBuscarPaciente";
            btnBuscarPaciente.Size = new Size(140, 34);
            btnBuscarPaciente.Text = "🔍 Buscar";
            btnBuscarPaciente.Click += btnBuscarPaciente_Click;

            // lblInfoPaciente
            lblInfoPaciente.AutoSize = true;
            lblInfoPaciente.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            lblInfoPaciente.ForeColor = Color.FromArgb(20, 50, 30);
            lblInfoPaciente.Location = new Point(400, 20);
            lblInfoPaciente.Name = "lblInfoPaciente";
            lblInfoPaciente.Size = new Size(310, 19);
            lblInfoPaciente.Text = "Paciente: [Seleccione un paciente mediante DNI]";

            // tabControlMain
            tabControlMain.Controls.Add(tabAntropometria);
            tabControlMain.Controls.Add(tabDiagnostico);
            tabControlMain.Controls.Add(tabAnamnesis);
            tabControlMain.Controls.Add(tabPlanAlimentario);
            tabControlMain.Dock = DockStyle.Fill;
            tabControlMain.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            tabControlMain.Location = new Point(0, 110);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new Size(1100, 590);
            tabControlMain.TabIndex = 2;

            // --- TAB 1: ANTROPOMETRÍA ---
            tabAntropometria.BackColor = Color.FromArgb(245, 249, 246);
            tabAntropometria.Controls.Add(gbMediciones);
            tabAntropometria.Controls.Add(gbResultados);
            tabAntropometria.Controls.Add(gbGrafico);
            tabAntropometria.Location = new Point(4, 26);
            tabAntropometria.Name = "tabAntropometria";
            tabAntropometria.Padding = new Padding(10);
            tabAntropometria.Size = new Size(1092, 560);
            tabAntropometria.Text = "📏 Antropometría y Curvas OMS";

            // gbMediciones
            gbMediciones.Controls.Add(lblPeso);
            gbMediciones.Controls.Add(numPeso);
            gbMediciones.Controls.Add(lblTalla);
            gbMediciones.Controls.Add(numTalla);
            gbMediciones.Controls.Add(lblPC);
            gbMediciones.Controls.Add(numPerimetroCefalico);
            gbMediciones.Controls.Add(lblCC);
            gbMediciones.Controls.Add(numCircunferenciaCintura);
            gbMediciones.Controls.Add(btnCalcularZScores);
            gbMediciones.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbMediciones.ForeColor = Color.FromArgb(30, 60, 40);
            gbMediciones.Location = new Point(15, 10);
            gbMediciones.Name = "gbMediciones";
            gbMediciones.Size = new Size(350, 240);
            gbMediciones.Text = "Mediciones del Control Actual";

            lblPeso.Location = new Point(15, 30);
            lblPeso.Text = "Peso (Kg):";
            lblPeso.Size = new Size(130, 25);

            numPeso.DecimalPlaces = 2;
            numPeso.Font = new Font("Segoe UI", 10F);
            numPeso.Location = new Point(150, 28);
            numPeso.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numPeso.Name = "numPeso";
            numPeso.Size = new Size(120, 25);

            lblTalla.Location = new Point(15, 65);
            lblTalla.Text = "Talla (cm):";
            lblTalla.Size = new Size(130, 25);

            numTalla.DecimalPlaces = 2;
            numTalla.Font = new Font("Segoe UI", 10F);
            numTalla.Location = new Point(150, 63);
            numTalla.Maximum = new decimal(new int[] { 250, 0, 0, 0 });
            numTalla.Name = "numTalla";
            numTalla.Size = new Size(120, 25);

            lblPC.Location = new Point(15, 100);
            lblPC.Text = "Perím. Cefálico (cm):";
            lblPC.Size = new Size(130, 25);

            numPerimetroCefalico.DecimalPlaces = 2;
            numPerimetroCefalico.Font = new Font("Segoe UI", 10F);
            numPerimetroCefalico.Location = new Point(150, 98);
            numPerimetroCefalico.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numPerimetroCefalico.Name = "numPerimetroCefalico";
            numPerimetroCefalico.Size = new Size(120, 25);

            lblCC.Location = new Point(15, 135);
            lblCC.Text = "Circ. Cintura (cm):";
            lblCC.Size = new Size(130, 25);

            numCircunferenciaCintura.DecimalPlaces = 2;
            numCircunferenciaCintura.Font = new Font("Segoe UI", 10F);
            numCircunferenciaCintura.Location = new Point(150, 133);
            numCircunferenciaCintura.Maximum = new decimal(new int[] { 150, 0, 0, 0 });
            numCircunferenciaCintura.Name = "numCircunferenciaCintura";
            numCircunferenciaCintura.Size = new Size(120, 25);

            btnCalcularZScores.ColorPrincipal = Color.FromArgb(46, 94, 67);
            btnCalcularZScores.ColorSecundario = Color.FromArgb(32, 68, 48);
            btnCalcularZScores.ColorGlow = Color.DarkSeaGreen;
            btnCalcularZScores.RadioBorde = 28;
            btnCalcularZScores.Location = new Point(15, 175);
            btnCalcularZScores.Name = "btnCalcularZScores";
            btnCalcularZScores.Size = new Size(255, 45);
            btnCalcularZScores.Text = "⚡ Calcular Z-Scores e IMC";
            btnCalcularZScores.Click += btnCalcularZScores_Click;

            // gbResultados
            gbResultados.Controls.Add(lblIMCValue);
            gbResultados.Controls.Add(lblZPesoValue);
            gbResultados.Controls.Add(lblZTallaValue);
            gbResultados.Controls.Add(lblZIMCValue);
            gbResultados.Controls.Add(lblZPCValue);
            gbResultados.Controls.Add(lblPercentilValue);
            gbResultados.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbResultados.ForeColor = Color.FromArgb(30, 60, 40);
            gbResultados.Location = new Point(15, 260);
            gbResultados.Name = "gbResultados";
            gbResultados.Size = new Size(350, 280);
            gbResultados.Text = "Resultados Z-Scores OMS";

            lblIMCValue.Location = new Point(15, 30);
            lblIMCValue.Text = "IMC: --";
            lblIMCValue.Size = new Size(300, 25);

            lblZPesoValue.Location = new Point(15, 65);
            lblZPesoValue.Text = "Z-Peso / Edad: --";
            lblZPesoValue.Size = new Size(300, 25);

            lblZTallaValue.Location = new Point(15, 100);
            lblZTallaValue.Text = "Z-Talla / Edad: --";
            lblZTallaValue.Size = new Size(300, 25);

            lblZIMCValue.Location = new Point(15, 135);
            lblZIMCValue.Text = "Z-IMC / Edad: --";
            lblZIMCValue.Size = new Size(300, 25);

            lblZPCValue.Location = new Point(15, 170);
            lblZPCValue.Text = "Z-Perím. Cefálico / Edad: --";
            lblZPCValue.Size = new Size(300, 25);

            lblPercentilValue.Location = new Point(15, 205);
            lblPercentilValue.Text = "Percentil IMC: --";
            lblPercentilValue.Size = new Size(300, 25);
            lblPercentilValue.ForeColor = Color.FromArgb(20, 80, 40);

            // gbGrafico
            gbGrafico.Controls.Add(lblTipoGrafico);
            gbGrafico.Controls.Add(cmbTipoGrafico);
            gbGrafico.Controls.Add(panelGraficoCurvas);
            gbGrafico.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbGrafico.ForeColor = Color.FromArgb(30, 60, 40);
            gbGrafico.Location = new Point(380, 10);
            gbGrafico.Name = "gbGrafico";
            gbGrafico.Size = new Size(690, 530);
            gbGrafico.Text = "Curvas Patrón de Crecimiento OMS Pediátricas";

            lblTipoGrafico.AutoSize = true;
            lblTipoGrafico.Location = new Point(15, 25);
            lblTipoGrafico.Text = "Variable:";
            lblTipoGrafico.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            cmbTipoGrafico.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoGrafico.Font = new Font("Segoe UI", 9F);
            cmbTipoGrafico.FormattingEnabled = true;
            cmbTipoGrafico.Items.AddRange(new object[] { "IMC vs Edad (Kg/m²)", "Peso vs Edad (Kg)", "Talla vs Edad (cm)", "Perímetro Cefálico vs Edad (cm)" });
            cmbTipoGrafico.Location = new Point(80, 22);
            cmbTipoGrafico.Name = "cmbTipoGrafico";
            cmbTipoGrafico.Size = new Size(240, 23);
            cmbTipoGrafico.SelectedIndex = 0;
            cmbTipoGrafico.SelectedIndexChanged += cmbTipoGrafico_SelectedIndexChanged;

            panelGraficoCurvas.BackColor = Color.White;
            panelGraficoCurvas.Location = new Point(10, 52);
            panelGraficoCurvas.Name = "panelGraficoCurvas";
            panelGraficoCurvas.Size = new Size(670, 468);
            panelGraficoCurvas.Paint += panelGraficoCurvas_Paint;

            // --- TAB 2: DIAGNÓSTICO Y ALERTAS ---
            tabDiagnostico.BackColor = Color.FromArgb(245, 249, 246);
            tabDiagnostico.Controls.Add(gbDiagnostico);
            tabDiagnostico.Controls.Add(panelAlertaVisual);
            tabDiagnostico.Location = new Point(4, 26);
            tabDiagnostico.Name = "tabDiagnostico";
            tabDiagnostico.Padding = new Padding(10);
            tabDiagnostico.Text = "🩺 Diagnóstico y Alertas Clínicas";

            panelAlertaVisual.BackColor = Color.FromArgb(240, 210, 210);
            panelAlertaVisual.BorderStyle = BorderStyle.FixedSingle;
            panelAlertaVisual.Controls.Add(lblAlertaTitulo);
            panelAlertaVisual.Controls.Add(lblAlertaMensaje);
            panelAlertaVisual.Location = new Point(15, 15);
            panelAlertaVisual.Name = "panelAlertaVisual";
            panelAlertaVisual.Size = new Size(1050, 90);
            panelAlertaVisual.Visible = false;

            lblAlertaTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblAlertaTitulo.ForeColor = Color.DarkRed;
            lblAlertaTitulo.Location = new Point(15, 10);
            lblAlertaTitulo.Text = "⚠️ ALERTA CLÍNICA VISUAL DE CRECIMIENTO DETECTADA";
            lblAlertaTitulo.Size = new Size(1000, 30);

            lblAlertaMensaje.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lblAlertaMensaje.ForeColor = Color.Maroon;
            lblAlertaMensaje.Location = new Point(15, 45);
            lblAlertaMensaje.Text = "Mensaje de Alerta...";
            lblAlertaMensaje.Size = new Size(1000, 35);

            gbDiagnostico.Controls.Add(lblClasificacionOMS);
            gbDiagnostico.Controls.Add(lblDetallesPrompt);
            gbDiagnostico.Controls.Add(txtDetallesClinicos);
            gbDiagnostico.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbDiagnostico.ForeColor = Color.FromArgb(30, 60, 40);
            gbDiagnostico.Location = new Point(15, 120);
            gbDiagnostico.Name = "gbDiagnostico";
            gbDiagnostico.Size = new Size(1050, 420);
            gbDiagnostico.Text = "Evaluación Diagnóstica OMS";

            lblClasificacionOMS.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblClasificacionOMS.ForeColor = Color.FromArgb(20, 80, 40);
            lblClasificacionOMS.Location = new Point(20, 35);
            lblClasificacionOMS.Text = "Clasificación OMS: [Sin Calcular]";
            lblClasificacionOMS.Size = new Size(1000, 35);

            lblDetallesPrompt.Location = new Point(20, 80);
            lblDetallesPrompt.Text = "Detalles Clínicos u Observaciones del Profesional:";
            lblDetallesPrompt.Size = new Size(400, 25);

            txtDetallesClinicos.Font = new Font("Segoe UI", 10F);
            txtDetallesClinicos.Location = new Point(20, 110);
            txtDetallesClinicos.Multiline = true;
            txtDetallesClinicos.Name = "txtDetallesClinicos";
            txtDetallesClinicos.Size = new Size(1000, 280);

            // --- TAB 3: ANAMNESIS Y RECORDATORIO 24H ---
            tabAnamnesis.BackColor = Color.FromArgb(245, 249, 246);
            tabAnamnesis.Controls.Add(gbAnamnesis);
            tabAnamnesis.Controls.Add(gbRecordatorio);
            tabAnamnesis.Location = new Point(4, 26);
            tabAnamnesis.Name = "tabAnamnesis";
            tabAnamnesis.Padding = new Padding(10);
            tabAnamnesis.Text = "📝 Anamnesis y Recordatorio 24h";

            gbAnamnesis.Controls.Add(lblLactancia);
            gbAnamnesis.Controls.Add(cmbTipoLactancia);
            gbAnamnesis.Controls.Add(lblAliComp);
            gbAnamnesis.Controls.Add(txtAlimentacionComplementaria);
            gbAnamnesis.Controls.Add(lblAlergias);
            gbAnamnesis.Controls.Add(txtAlergias);
            gbAnamnesis.Controls.Add(lblAntecedentes);
            gbAnamnesis.Controls.Add(txtAntecedentesFamiliares);
            gbAnamnesis.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbAnamnesis.ForeColor = Color.FromArgb(30, 60, 40);
            gbAnamnesis.Location = new Point(15, 10);
            gbAnamnesis.Name = "gbAnamnesis";
            gbAnamnesis.Size = new Size(510, 530);
            gbAnamnesis.Text = "Antecedentes y Lactancia";

            lblLactancia.Location = new Point(15, 35);
            lblLactancia.Text = "Tipo de Lactancia:";
            lblLactancia.Size = new Size(180, 25);

            cmbTipoLactancia.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoLactancia.Font = new Font("Segoe UI", 10F);
            cmbTipoLactancia.FormattingEnabled = true;
            cmbTipoLactancia.Items.AddRange(new object[] { "Materna Exclusiva", "Fórmula", "Mixta", "Destete" });
            cmbTipoLactancia.Location = new Point(200, 32);
            cmbTipoLactancia.Name = "cmbTipoLactancia";
            cmbTipoLactancia.Size = new Size(280, 25);

            lblAliComp.Location = new Point(15, 80);
            lblAliComp.Text = "Alimentación Comp.:";
            lblAliComp.Size = new Size(180, 25);

            txtAlimentacionComplementaria.Font = new Font("Segoe UI", 10F);
            txtAlimentacionComplementaria.Location = new Point(15, 110);
            txtAlimentacionComplementaria.Multiline = true;
            txtAlimentacionComplementaria.Size = new Size(465, 90);

            lblAlergias.Location = new Point(15, 215);
            lblAlergias.Text = "Alergias / Intolerancias:";
            lblAlergias.Size = new Size(180, 25);

            txtAlergias.Font = new Font("Segoe UI", 10F);
            txtAlergias.Location = new Point(15, 245);
            txtAlergias.Multiline = true;
            txtAlergias.Size = new Size(465, 90);

            lblAntecedentes.Location = new Point(15, 350);
            lblAntecedentes.Text = "Antecedentes Familiares:";
            lblAntecedentes.Size = new Size(180, 25);

            txtAntecedentesFamiliares.Font = new Font("Segoe UI", 10F);
            txtAntecedentesFamiliares.Location = new Point(15, 380);
            txtAntecedentesFamiliares.Multiline = true;
            txtAntecedentesFamiliares.Size = new Size(465, 120);

            gbRecordatorio.Controls.Add(lblDesayuno);
            gbRecordatorio.Controls.Add(txtDesayuno);
            gbRecordatorio.Controls.Add(lblAlmuerzo);
            gbRecordatorio.Controls.Add(txtAlmuerzo);
            gbRecordatorio.Controls.Add(lblMerienda);
            gbRecordatorio.Controls.Add(txtMerienda);
            gbRecordatorio.Controls.Add(lblCena);
            gbRecordatorio.Controls.Add(txtCena);
            gbRecordatorio.Controls.Add(lblColaciones);
            gbRecordatorio.Controls.Add(txtColaciones);
            gbRecordatorio.Controls.Add(lblFrecuencia);
            gbRecordatorio.Controls.Add(txtFrecuenciaAlimentos);
            gbRecordatorio.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbRecordatorio.ForeColor = Color.FromArgb(30, 60, 40);
            gbRecordatorio.Location = new Point(540, 10);
            gbRecordatorio.Name = "gbRecordatorio";
            gbRecordatorio.Size = new Size(530, 530);
            gbRecordatorio.Text = "Recordatorio 24 horas y Ingesta";

            lblDesayuno.Location = new Point(15, 30);
            lblDesayuno.Text = "Desayuno:";
            lblDesayuno.Size = new Size(100, 25);

            txtDesayuno.Font = new Font("Segoe UI", 10F);
            txtDesayuno.Location = new Point(120, 28);
            txtDesayuno.Size = new Size(380, 25);

            lblAlmuerzo.Location = new Point(15, 70);
            lblAlmuerzo.Text = "Almuerzo:";
            lblAlmuerzo.Size = new Size(100, 25);

            txtAlmuerzo.Font = new Font("Segoe UI", 10F);
            txtAlmuerzo.Location = new Point(120, 68);
            txtAlmuerzo.Size = new Size(380, 25);

            lblMerienda.Location = new Point(15, 110);
            lblMerienda.Text = "Merienda:";
            lblMerienda.Size = new Size(100, 25);

            txtMerienda.Font = new Font("Segoe UI", 10F);
            txtMerienda.Location = new Point(120, 108);
            txtMerienda.Size = new Size(380, 25);

            lblCena.Location = new Point(15, 150);
            lblCena.Text = "Cena:";
            lblCena.Size = new Size(100, 25);

            txtCena.Font = new Font("Segoe UI", 10F);
            txtCena.Location = new Point(120, 148);
            txtCena.Size = new Size(380, 25);

            lblColaciones.Location = new Point(15, 190);
            lblColaciones.Text = "Colaciones:";
            lblColaciones.Size = new Size(100, 25);

            txtColaciones.Font = new Font("Segoe UI", 10F);
            txtColaciones.Location = new Point(120, 188);
            txtColaciones.Size = new Size(380, 25);

            lblFrecuencia.Location = new Point(15, 230);
            lblFrecuencia.Text = "Frecuencia Alimentos:";
            lblFrecuencia.Size = new Size(200, 25);

            txtFrecuenciaAlimentos.Font = new Font("Segoe UI", 10F);
            txtFrecuenciaAlimentos.Location = new Point(15, 260);
            txtFrecuenciaAlimentos.Multiline = true;
            txtFrecuenciaAlimentos.Size = new Size(485, 240);

            // --- TAB 4: PLAN ALIMENTARIO ---
            tabPlanAlimentario.BackColor = Color.FromArgb(245, 249, 246);
            tabPlanAlimentario.Controls.Add(gbPlan);
            tabPlanAlimentario.Controls.Add(gbHistorial);
            tabPlanAlimentario.Location = new Point(4, 26);
            tabPlanAlimentario.Name = "tabPlanAlimentario";
            tabPlanAlimentario.Padding = new Padding(10);
            tabPlanAlimentario.Text = "🍎 Plan Alimentario y Guardar";

            gbPlan.Controls.Add(lblKcal);
            gbPlan.Controls.Add(numKcal);
            gbPlan.Controls.Add(lblCHO);
            gbPlan.Controls.Add(numPctCHO);
            gbPlan.Controls.Add(lblPROT);
            gbPlan.Controls.Add(numPctPROT);
            gbPlan.Controls.Add(lblGRASAS);
            gbPlan.Controls.Add(numPctGrasas);
            gbPlan.Controls.Add(lblSumaMacronutrientes);
            gbPlan.Controls.Add(lblPautas);
            gbPlan.Controls.Add(txtPautasFamiliares);
            gbPlan.Controls.Add(lblMetas);
            gbPlan.Controls.Add(txtMetasSalud);
            gbPlan.Controls.Add(btnGuardarConsulta);
            gbPlan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbPlan.ForeColor = Color.FromArgb(30, 60, 40);
            gbPlan.Location = new Point(15, 10);
            gbPlan.Name = "gbPlan";
            gbPlan.Size = new Size(520, 530);
            gbPlan.Text = "Prescripción del Plan Alimentario";

            lblKcal.Location = new Point(15, 30);
            lblKcal.Text = "Req. Calórico (Kcal/día):";
            lblKcal.Size = new Size(180, 25);

            numKcal.DecimalPlaces = 2;
            numKcal.Font = new Font("Segoe UI", 10F);
            numKcal.Location = new Point(200, 28);
            numKcal.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numKcal.Value = new decimal(new int[] { 1500, 0, 0, 0 });
            numKcal.Size = new Size(120, 25);

            lblCHO.Location = new Point(15, 65);
            lblCHO.Text = "% Carbohidratos:";
            lblCHO.Size = new Size(180, 25);

            numPctCHO.DecimalPlaces = 2;
            numPctCHO.Font = new Font("Segoe UI", 10F);
            numPctCHO.Location = new Point(200, 63);
            numPctCHO.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numPctCHO.Value = new decimal(new int[] { 55, 0, 0, 0 });
            numPctCHO.Size = new Size(120, 25);
            numPctCHO.ValueChanged += Macronutrientes_ValueChanged;

            lblPROT.Location = new Point(15, 100);
            lblPROT.Text = "% Proteínas:";
            lblPROT.Size = new Size(180, 25);

            numPctPROT.DecimalPlaces = 2;
            numPctPROT.Font = new Font("Segoe UI", 10F);
            numPctPROT.Location = new Point(200, 98);
            numPctPROT.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numPctPROT.Value = new decimal(new int[] { 15, 0, 0, 0 });
            numPctPROT.Size = new Size(120, 25);
            numPctPROT.ValueChanged += Macronutrientes_ValueChanged;

            lblGRASAS.Location = new Point(15, 135);
            lblGRASAS.Text = "% Grasas:";
            lblGRASAS.Size = new Size(180, 25);

            numPctGrasas.DecimalPlaces = 2;
            numPctGrasas.Font = new Font("Segoe UI", 10F);
            numPctGrasas.Location = new Point(200, 133);
            numPctGrasas.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numPctGrasas.Value = new decimal(new int[] { 30, 0, 0, 0 });
            numPctGrasas.Size = new Size(120, 25);
            numPctGrasas.ValueChanged += Macronutrientes_ValueChanged;

            lblSumaMacronutrientes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSumaMacronutrientes.ForeColor = Color.DarkGreen;
            lblSumaMacronutrientes.Location = new Point(335, 98);
            lblSumaMacronutrientes.Text = "Suma: 100.00%";
            lblSumaMacronutrientes.Size = new Size(170, 25);

            lblPautas.Location = new Point(15, 175);
            lblPautas.Text = "Pautas Alimentarias Familiares:";
            lblPautas.Size = new Size(300, 25);

            txtPautasFamiliares.Font = new Font("Segoe UI", 10F);
            txtPautasFamiliares.Location = new Point(15, 200);
            txtPautasFamiliares.Multiline = true;
            txtPautasFamiliares.Size = new Size(485, 110);

            lblMetas.Location = new Point(15, 320);
            lblMetas.Text = "Metas de Salud y Tratamiento:";
            lblMetas.Size = new Size(300, 25);

            txtMetasSalud.Font = new Font("Segoe UI", 10F);
            txtMetasSalud.Location = new Point(15, 345);
            txtMetasSalud.Multiline = true;
            txtMetasSalud.Size = new Size(485, 100);

            btnGuardarConsulta.ColorPrincipal = Color.FromArgb(46, 94, 67);
            btnGuardarConsulta.ColorSecundario = Color.FromArgb(32, 68, 48);
            btnGuardarConsulta.ColorGlow = Color.DarkSeaGreen;
            btnGuardarConsulta.RadioBorde = 30;
            btnGuardarConsulta.Location = new Point(15, 460);
            btnGuardarConsulta.Name = "btnGuardarConsulta";
            btnGuardarConsulta.Size = new Size(485, 50);
            btnGuardarConsulta.Text = "💾 GUARDAR CONSULTA NUTRICIONAL (CUN09)";
            btnGuardarConsulta.Click += btnGuardarConsulta_Click;

            // gbHistorial
            gbHistorial.Controls.Add(dgvHistorialConsultas);
            gbHistorial.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gbHistorial.ForeColor = Color.FromArgb(30, 60, 40);
            gbHistorial.Location = new Point(550, 10);
            gbHistorial.Name = "gbHistorial";
            gbHistorial.Size = new Size(525, 530);
            gbHistorial.Text = "Historia Clínica de Consultas Anteriores";

            dgvHistorialConsultas.AllowUserToAddRows = false;
            dgvHistorialConsultas.AllowUserToDeleteRows = false;
            dgvHistorialConsultas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistorialConsultas.BackgroundColor = Color.White;
            dgvHistorialConsultas.Dock = DockStyle.Fill;
            dgvHistorialConsultas.Location = new Point(3, 21);
            dgvHistorialConsultas.Name = "dgvHistorialConsultas";
            dgvHistorialConsultas.ReadOnly = true;
            dgvHistorialConsultas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistorialConsultas.Size = new Size(519, 506);

            // Form
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 700);
            Controls.Add(tabControlMain);
            Controls.Add(panelPacienteHeader);
            Controls.Add(panelHeader);
            Name = "FormSeguimientoNutricional_DNI101";
            Text = "Gestión de Seguimiento Nutricional Pediátrico";
            Load += FormSeguimientoNutricional_DNI101_Load;

            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelPacienteHeader.ResumeLayout(false);
            panelPacienteHeader.PerformLayout();
            tabControlMain.ResumeLayout(false);
            tabAntropometria.ResumeLayout(false);
            gbMediciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numPeso).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTalla).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPerimetroCefalico).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCircunferenciaCintura).EndInit();
            gbResultados.ResumeLayout(false);
            gbGrafico.ResumeLayout(false);
            tabDiagnostico.ResumeLayout(false);
            gbDiagnostico.ResumeLayout(false);
            gbDiagnostico.PerformLayout();
            panelAlertaVisual.ResumeLayout(false);
            tabAnamnesis.ResumeLayout(false);
            gbAnamnesis.ResumeLayout(false);
            gbAnamnesis.PerformLayout();
            gbRecordatorio.ResumeLayout(false);
            gbRecordatorio.PerformLayout();
            tabPlanAlimentario.ResumeLayout(false);
            gbPlan.ResumeLayout(false);
            gbPlan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numKcal).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPctCHO).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPctPROT).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPctGrasas).EndInit();
            gbHistorial.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHistorialConsultas).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelHeader;
        private Label lblTituloModulo;
        private Panel panelPacienteHeader;
        private Label lblDniPrompt;
        private TextBox txtDniNiño;
        private UI.Controles.BotonFuturista btnBuscarPaciente;
        private Label lblInfoPaciente;
        private TabControl tabControlMain;
        private TabPage tabAntropometria;
        private GroupBox gbMediciones;
        private Label lblPeso;
        private NumericUpDown numPeso;
        private Label lblTalla;
        private NumericUpDown numTalla;
        private Label lblPC;
        private NumericUpDown numPerimetroCefalico;
        private Label lblCC;
        private NumericUpDown numCircunferenciaCintura;
        private UI.Controles.BotonFuturista btnCalcularZScores;
        private GroupBox gbResultados;
        private Label lblIMCValue;
        private Label lblZPesoValue;
        private Label lblZTallaValue;
        private Label lblZIMCValue;
        private Label lblZPCValue;
        private Label lblPercentilValue;
        private GroupBox gbGrafico;
        private Label lblTipoGrafico;
        private ComboBox cmbTipoGrafico;
        private Panel panelGraficoCurvas;
        private TabPage tabDiagnostico;
        private GroupBox gbDiagnostico;
        private Label lblClasificacionOMS;
        private Label lblDetallesPrompt;
        private TextBox txtDetallesClinicos;
        private Panel panelAlertaVisual;
        private Label lblAlertaTitulo;
        private Label lblAlertaMensaje;
        private TabPage tabAnamnesis;
        private GroupBox gbAnamnesis;
        private Label lblLactancia;
        private ComboBox cmbTipoLactancia;
        private Label lblAliComp;
        private TextBox txtAlimentacionComplementaria;
        private Label lblAlergias;
        private TextBox txtAlergias;
        private Label lblAntecedentes;
        private TextBox txtAntecedentesFamiliares;
        private GroupBox gbRecordatorio;
        private Label lblDesayuno;
        private TextBox txtDesayuno;
        private Label lblAlmuerzo;
        private TextBox txtAlmuerzo;
        private Label lblMerienda;
        private TextBox txtMerienda;
        private Label lblCena;
        private TextBox txtCena;
        private Label lblColaciones;
        private TextBox txtColaciones;
        private Label lblFrecuencia;
        private TextBox txtFrecuenciaAlimentos;
        private TabPage tabPlanAlimentario;
        private GroupBox gbPlan;
        private Label lblKcal;
        private NumericUpDown numKcal;
        private Label lblCHO;
        private NumericUpDown numPctCHO;
        private Label lblPROT;
        private NumericUpDown numPctPROT;
        private Label lblGRASAS;
        private NumericUpDown numPctGrasas;
        private Label lblSumaMacronutrientes;
        private Label lblPautas;
        private TextBox txtPautasFamiliares;
        private Label lblMetas;
        private TextBox txtMetasSalud;
        private UI.Controles.BotonFuturista btnGuardarConsulta;
        private GroupBox gbHistorial;
        private DataGridView dgvHistorialConsultas;
    }
}
