using System;
using System.Drawing;
using System.Windows.Forms;
using BE;
using BE.PN2;
using BLL;
using BLL.PN2;
using BLL.PN2.Strategy;
using Services;

namespace UI.PN2
{
    public partial class frmRegistrarConsulta_DNI101 : Form, IIdiomaObserver
    {
        private readonly PacienteBE_DNI101 _paciente;
        private readonly ConsultaNutricionalBLL_DNI101 _consultaBLL;
        private readonly ConsultaNutricionalBE_DNI101? _consultaExistente;

        public frmRegistrarConsulta_DNI101(PacienteBE_DNI101 paciente, ConsultaNutricionalBE_DNI101? consultaAEditar = null)
        {
            _paciente = paciente ?? throw new ArgumentNullException(nameof(paciente));
            _consultaBLL = new ConsultaNutricionalBLL_DNI101();
            _consultaExistente = consultaAEditar;

            InitializeComponent();
            _dtpFecha.MaxDate = DateTime.Today;

            if (_consultaExistente != null)
            {
                Text = $"CUN08: Modificar Consulta Nutricional #{_consultaExistente.IdConsulta_DNI101} - {_paciente.NombreCompleto}";
                lblTit.Text = $" MODIFICAR CONSULTA NUTRICIONAL #{_consultaExistente.IdConsulta_DNI101}";
                btnGuardar.Text = "Guardar Modificación";
                _dtpFecha.Value = _consultaExistente.FechaControl_DNI101 <= DateTime.Today ? _consultaExistente.FechaControl_DNI101 : DateTime.Today;
                ConfigurarTextosPaciente();
                CargarDatosConsultaExistente();
            }
            else
            {
                _dtpFecha.Value = DateTime.Today;
                ConfigurarTextosPaciente();
            }

            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();
        }

        private void CargarDatosConsultaExistente()
        {
            if (_consultaExistente == null) return;

            if (_consultaExistente.Medicion != null)
            {
                _txtPeso.Text = _consultaExistente.Medicion.PesoKg_DNI101.ToString("F2");
                _txtTalla.Text = _consultaExistente.Medicion.TallaCm_DNI101.ToString("F2");
                if (_consultaExistente.Medicion.PerimetroCefalicoCm_DNI101 > 0)
                {
                    _txtPerimetro.Text = _consultaExistente.Medicion.PerimetroCefalicoCm_DNI101.ToString("F2");
                }
            }

            if (!string.IsNullOrWhiteSpace(_consultaExistente.TipoLactancia_DNI101))
            {
                int idx = _cmbLactancia.FindStringExact(_consultaExistente.TipoLactancia_DNI101);
                if (idx >= 0) _cmbLactancia.SelectedIndex = idx;
                else _cmbLactancia.Text = _consultaExistente.TipoLactancia_DNI101;
            }

            _txtAlimentacionComp.Text = _consultaExistente.AlimentacionComplementaria_DNI101 ?? "";
            _txtAlergias.Text = _consultaExistente.Alergias_DNI101 ?? "";
            _txtAntFam.Text = _consultaExistente.AntecedentesFamiliares_DNI101 ?? "";
            _txtObservaciones.Text = _consultaExistente.Observaciones_DNI101 ?? "";

            RecalcularIMC();
        }

        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                var idiomaBLL = new IdiomaBLL();
                TraductorUI.TraducirFormulario(this, idiomaBLL);
                ConfigurarTextosPaciente();
            }
        }

        private void ConfigurarTextosPaciente()
        {
            Text = $"CUN08: Registrar Consulta Nutricional - {_paciente.NombreCompleto}";

            int meses = _paciente.ObtenerEdadMeses();
            string edadStr = meses < 24 ? $"{meses} meses" : $"{meses / 12} años ({meses}m)";
            lblSub.Text = $"Paciente: {_paciente.NombreCompleto}  |  DNI: {_paciente.DNINiño_DNI101}  |  Sexo: {_paciente.Sexo_DNI101 ?? "N/D"}  |  Edad: {edadStr}";

            if (_cmbLactancia.Items.Count > 0)
                _cmbLactancia.SelectedIndex = 0;
        }

        private void _txtAntropo_TextChanged(object? sender, EventArgs e)
        {
            RecalcularIMC();
        }

        private void RecalcularIMC()
        {
            if (decimal.TryParse(_txtPeso.Text.Trim().Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal peso) &&
                decimal.TryParse(_txtTalla.Text.Trim().Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal talla) &&
                peso > 0 && talla > 0)
            {
                decimal imc = _consultaBLL.CalcularIMC(peso, talla);
                _lblIMCValor.Text = imc.ToString("N2");

                int edadMeses = _paciente.ObtenerEdadMeses();
                string sexo = _paciente.Sexo_DNI101 ?? "Masculino";
                ResultadoEvaluacionOMS eval = _consultaBLL.EvaluarEstadoNutricional(imc, edadMeses, sexo);

                _lblCategoriaPreview.Text = eval.Clasificacion;

                if (eval.RequiereAlerta)
                {
                    _lblCategoriaPreview.ForeColor = Color.FromArgb(198, 40, 40);
                    _lblIMCValor.ForeColor = Color.FromArgb(198, 40, 40);
                }
                else if (eval.Clasificacion.Contains("Sobrepeso"))
                {
                    _lblCategoriaPreview.ForeColor = Color.FromArgb(245, 124, 0);
                    _lblIMCValor.ForeColor = Color.FromArgb(245, 124, 0);
                }
                else
                {
                    _lblCategoriaPreview.ForeColor = Color.FromArgb(46, 125, 50);
                    _lblIMCValor.ForeColor = Color.FromArgb(46, 125, 50);
                }
            }
            else
            {
                _lblIMCValor.Text = "0.00";
                _lblIMCValor.ForeColor = Color.FromArgb(90, 105, 95);
                _lblCategoriaPreview.Text = "Ingrese peso y talla";
                _lblCategoriaPreview.ForeColor = Color.FromArgb(90, 105, 95);
            }
        }

        private void BtnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_dtpFecha.Value.Date > DateTime.Today)
                {
                    MessageBox.Show("No se pueden registrar consultas nutricionales con fechas futuras.", "Fecha Inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _dtpFecha.Focus();
                    return;
                }

                if (!decimal.TryParse(_txtPeso.Text.Trim().Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal peso) || peso <= 0)
                {
                    MessageBox.Show("Por favor, ingrese un peso válido en kilogramos.", "Dato Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _txtPeso.Focus();
                    return;
                }

                if (!decimal.TryParse(_txtTalla.Text.Trim().Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal talla) || talla <= 20)
                {
                    MessageBox.Show("Por favor, ingrese una talla/longitud válida en centímetros.", "Dato Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _txtTalla.Focus();
                    return;
                }

                decimal.TryParse(_txtPerimetro.Text.Trim().Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal perimetro);

                Cursor = Cursors.WaitCursor;

                int edadMeses = _consultaExistente != null ? _consultaExistente.EdadMeses_DNI101 : _paciente.ObtenerEdadMeses();
                string sexo = _paciente.Sexo_DNI101 ?? "Masculino";

                if (_consultaExistente != null)
                {
                    bool ok = _consultaBLL.ModificarConsulta(
                        idConsulta: _consultaExistente.IdConsulta_DNI101,
                        idPaciente: _paciente.IdPaciente_DNI101,
                        edadMeses: edadMeses,
                        sexo: sexo,
                        pesoKg: peso,
                        tallaCm: talla,
                        perimetroCefalico: perimetro,
                        fechaControl: _dtpFecha.Value,
                        tipoLactancia: _cmbLactancia.SelectedItem?.ToString(),
                        alimentacionComp: _txtAlimentacionComp.Text.Trim(),
                        alergias: _txtAlergias.Text.Trim(),
                        antFamiliares: _txtAntFam.Text.Trim(),
                        observaciones: _txtObservaciones.Text.Trim()
                    );

                    if (ok)
                    {
                        MessageBox.Show(
                            $"¡Consulta #{_consultaExistente.IdConsulta_DNI101} modificada con éxito!\n\n" +
                            $"Peso: {peso:N2} kg | Talla: {talla:N2} cm | IMC: {_lblIMCValor.Text}\n" +
                            $"Diagnóstico: {_lblCategoriaPreview.Text}\n\n" +
                            "Se ha registrado una nueva versión activa (Act = 1) en la Bitácora de Cambios (CUN08) " +
                            "y la versión previa quedó archivada como histórica (Act = 0).",
                            "Consulta Nutricional Actualizada",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar la consulta nutricional.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // ÚNICO HIT A BASE DE DATOS
                    int idConsulta = _consultaBLL.RegistrarConsulta(
                        idPaciente: _paciente.IdPaciente_DNI101,
                        edadMeses: edadMeses,
                        sexo: sexo,
                        pesoKg: peso,
                        tallaCm: talla,
                        perimetroCefalico: perimetro,
                        fechaControl: _dtpFecha.Value,
                        tipoLactancia: _cmbLactancia.SelectedItem?.ToString(),
                        alimentacionComp: _txtAlimentacionComp.Text.Trim(),
                        alergias: _txtAlergias.Text.Trim(),
                        antFamiliares: _txtAntFam.Text.Trim(),
                        observaciones: _txtObservaciones.Text.Trim()
                    );

                    MessageBox.Show(
                        $"¡Consulta #{idConsulta} registrada con éxito!\n\nPeso: {peso:N2} kg | Talla: {talla:N2} cm | IMC: {_lblIMCValor.Text}\nDiagnóstico: {_lblCategoriaPreview.Text}",
                        "Consulta Nutricional Registrada",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la consulta nutricional: {ex.Message}", "Error Clínico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void BtnCancelar_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void BtnAyudaContextual_Click(object? sender, EventArgs e)
        {
            try
            {
                using var frmAyuda = new frmManualAyuda(4);
                frmAyuda.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el manual de ayuda: {ex.Message}", "Ayuda", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
