using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BE;
using BE.PN2;
using BLL;
using BLL.PN2;
using ScottPlot;
using Services;
using SPColor = ScottPlot.Color;
using SPAlignment = ScottPlot.Alignment;

namespace UI.PN2
{
  public partial class frmGraficarDiagnostico_DNI101 : Form, IIdiomaObserver
  {
    private readonly PacienteBE_DNI101 _paciente;
    private readonly ConsultaNutricionalBLL_DNI101 _consultaBLL;
    private List<ConsultaNutricionalBE_DNI101> _historialConsultas = new List<ConsultaNutricionalBE_DNI101>();
    private bool _isLoaded = false;

    public frmGraficarDiagnostico_DNI101(PacienteBE_DNI101 paciente)
    {
      _paciente = paciente ?? throw new ArgumentNullException(nameof(paciente));
      _consultaBLL = new ConsultaNutricionalBLL_DNI101();

      InitializeComponent();
      _formsPlot.MouseDown += FormsPlot_MouseDown;
      _dgvHistorial.CellDoubleClick += DgvHistorial_CellDoubleClick;
      ConfigurarTextosPaciente();
      CargarDatos();
      TraductorUI.SuscribirFormulario(this, this);
      pnlTop.Resize += (s, e) => AjustarPosicionComboIndicador();
      ActualizarIdioma();
      AjustarPosicionComboIndicador();
      this.Activated += (s, e) => { if (_isLoaded) CargarDatos(); };
      _isLoaded = true;
    }

    public void ActualizarIdioma()
    {
      if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
      {
        var idiomaBLL = new IdiomaBLL();
        TraductorUI.TraducirFormulario(this, idiomaBLL);
        ConfigurarTextosPaciente();
        if (_dgvHistorial.Columns.Count > 0)
        {
          TraductorUI.TraducirDataGridView(_dgvHistorial, idiomaBLL, this.Name);
        }
        AjustarPosicionComboIndicador();
        ActualizarGrafico();
      }
    }

    private void AjustarPosicionComboIndicador()
    {
      if (lblSel != null && _cmbIndicador != null && pnlTop != null)
      {
        _cmbIndicador.Location = new Point(pnlTop.Width - _cmbIndicador.Width - 20, 22);
        lblSel.Location = new Point(_cmbIndicador.Left - lblSel.Width - 8, 25);
      }
    }

    private void ConfigurarTextosPaciente()
    {
      Text = $"CUN07: Graficar Diagnóstico Nutricional Pediátrico - {_paciente.NombreCompleto}";
      lblTitulo.Text = $" CURVAS DE CRECIMIENTO INFANTIL (OMS) — {_paciente.NombreCompleto}";

      int meses = _paciente.ObtenerEdadMeses();
      string edadStr = meses < 24 ? $"{meses} meses" : $"{meses / 12} años ({meses} meses)";
      lblSub.Text = $"DNI: {_paciente.DNINiño_DNI101} | Sexo: {_paciente.Sexo_DNI101 ?? "N/D"} | Nacimiento: {_paciente.FechaNacimiento_DNI101:dd/MM/yyyy} ({edadStr}) | Cobertura: {_paciente.ObraSocial_DNI101 ?? "Particular"}";

      if (_cmbIndicador.Items.Count > 0)
        _cmbIndicador.SelectedIndex = 0;
    }

    private void _cmbIndicador_SelectedIndexChanged(object? sender, EventArgs e)
    {
      ActualizarGrafico();
    }

    private void _pnlAlertaCritica_Paint(object? sender, PaintEventArgs e)
    {
      using (var pen = new Pen(System.Drawing.Color.FromArgb(229, 57, 53), 1.5f))
      {
        e.Graphics.DrawRectangle(pen, 0, 0, _pnlAlertaCritica.Width - 1, _pnlAlertaCritica.Height - 1);
      }
    }

    private void BtnNuevaConsulta_Click(object? sender, EventArgs e)
    {
      using (var frm = new frmRegistrarConsulta_DNI101(_paciente))
      {
        if (frm.ShowDialog(this) == DialogResult.OK)
        {
          CargarDatos();
        }
      }
    }

    private void BtnModificarConsulta_Click(object? sender, EventArgs e)
    {
      if (_dgvHistorial.CurrentRow == null || _dgvHistorial.CurrentRow.Tag is not ConsultaNutricionalBE_DNI101 consulta)
      {
        MessageBox.Show("Seleccione un control médico de la grilla para modificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      AbrirModificarConsulta(consulta);
    }

    private void DgvHistorial_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex >= 0 && _dgvHistorial.Rows[e.RowIndex].Tag is ConsultaNutricionalBE_DNI101 consulta)
      {
        AbrirModificarConsulta(consulta);
      }
    }

    private void FormsPlot_MouseDown(object? sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left || _historialConsultas.Count == 0) return;

      try
      {
        var pixel = new ScottPlot.Pixel(e.X, e.Y);
        var coords = _formsPlot.Plot.GetCoordinates(pixel);

        string tipoIndicador = "IMC";
        if (_cmbIndicador.SelectedIndex == 1) tipoIndicador = "PESO";
        else if (_cmbIndicador.SelectedIndex == 2) tipoIndicador = "TALLA";

        var xRange = _formsPlot.Plot.Axes.Bottom.Range;
        var yRange = _formsPlot.Plot.Axes.Left.Range;
        if (xRange.Span <= 0 || yRange.Span <= 0) return;

        ConsultaNutricionalBE_DNI101? puntoCercano = null;
        double menorDistanciaNorm = double.MaxValue;

        foreach (var c in _historialConsultas)
        {
          if (c.Medicion == null) continue;

          double px = c.EdadMeses_DNI101;
          double py = tipoIndicador == "PESO" ? (double)c.Medicion.PesoKg_DNI101
                    : tipoIndicador == "TALLA" ? (double)c.Medicion.TallaCm_DNI101
                    : (double)c.Medicion.IMC_DNI101;

          double dxNorm = (coords.X - px) / xRange.Span;
          double dyNorm = (coords.Y - py) / yRange.Span;
          double distNorm = dxNorm * dxNorm + dyNorm * dyNorm;

          // Tolerancia para detectar el clic sobre el punto azul
          if (distNorm < 0.003 && distNorm < menorDistanciaNorm)
          {
            menorDistanciaNorm = distNorm;
            puntoCercano = c;
          }
        }

        if (puntoCercano != null)
        {
          var dr = MessageBox.Show(
            $"Has seleccionado el control médico:\n\n" +
            $"• Consulta #{puntoCercano.IdConsulta_DNI101}\n" +
            $"• Fecha del control: {puntoCercano.FechaControl_DNI101:dd/MM/yyyy}\n" +
            $"• Edad: {puntoCercano.EdadMeses_DNI101} meses\n" +
            $"• Peso: {puntoCercano.Medicion?.PesoKg_DNI101:N2} kg | Talla: {puntoCercano.Medicion?.TallaCm_DNI101:N2} cm\n\n" +
            $"¿Deseas modificar los datos clínicos de esta consulta?",
            "Modificar Control Clínico",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

          if (dr == DialogResult.Yes)
          {
            AbrirModificarConsulta(puntoCercano);
          }
        }
      }
      catch { }
    }

    private void AbrirModificarConsulta(ConsultaNutricionalBE_DNI101 consulta)
    {
      using (var frm = new frmRegistrarConsulta_DNI101(_paciente, consulta))
      {
        if (frm.ShowDialog(this) == DialogResult.OK)
        {
          CargarDatos();
        }
      }
    }

    private void BtnPlan_Click(object? sender, EventArgs e)
    {
      using (var frm = new frmPrescribirPlan_DNI101(_paciente))
      {
        frm.ShowDialog(this);
      }
    }

    private void BtnAyuda_Click(object? sender, EventArgs e)
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

    private void CargarDatos()
    {
      try
      {
        Cursor = Cursors.WaitCursor;
        _historialConsultas = _consultaBLL.ObtenerHistorialPorPaciente(_paciente.IdPaciente_DNI101);

        // Poblar grilla
        _dgvHistorial.Columns.Clear();
        _dgvHistorial.Columns.Add("Fecha", "Fecha");
        _dgvHistorial.Columns.Add("Edad", "Edad");
        _dgvHistorial.Columns.Add("Peso", "Peso (kg)");
        _dgvHistorial.Columns.Add("Talla", "Talla (cm)");
        _dgvHistorial.Columns.Add("IMC", "IMC");
        _dgvHistorial.Columns.Add("Estado", "Diagnóstico");

        if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
        {
          TraductorUI.TraducirDataGridView(_dgvHistorial, new IdiomaBLL(), this.Name);
        }

        foreach (var c in _historialConsultas)
        {
          int rowIndex = _dgvHistorial.Rows.Add(
            c.FechaControl_DNI101.ToString("dd/MM/yyyy"),
            $"{c.EdadMeses_DNI101} m",
            c.Medicion?.PesoKg_DNI101.ToString("N2") ?? "-",
            c.Medicion?.TallaCm_DNI101.ToString("N2") ?? "-",
            c.Medicion?.IMC_DNI101.ToString("N2") ?? "-",
            c.Diagnostico?.ClasificacionOMS_DNI101 ?? "Eutrófico"
          );
          _dgvHistorial.Rows[rowIndex].Tag = c;
        }

        ActualizarPanelDiagnostico();
        ActualizarGrafico();
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error al cargar el historial del paciente: {ex.Message}", "Error Clínico", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        Cursor = Cursors.Default;
      }
    }

    private void ActualizarPanelDiagnostico()
    {
      if (_historialConsultas.Count == 0)
      {
        _lblDiagnosticoBadge.Text = "Sin Consultas Previas";
        _lblDiagnosticoBadge.BackColor = System.Drawing.Color.FromArgb(120, 144, 156);
        _lblDetallesClinicos.Text = "El paciente no tiene mediciones registradas. Haga clic en ' Nueva Consulta' para iniciar el seguimiento.";
        _pnlAlertaCritica.Visible = false;
        return;
      }

      var ultima = _historialConsultas.Last();
      string clasif = ultima.Diagnostico?.ClasificacionOMS_DNI101 ?? "Eutrófico (Normal)";
      _lblDiagnosticoBadge.Text = $"Estado: {clasif}";

      // Colores semafóricos según clasificación OMS / CDC
      if (clasif.Contains("Bajo peso") || clasif.Contains("Desnutrición") || clasif.Contains("Severa") || clasif.Contains("Baja"))
      {
        _lblDiagnosticoBadge.BackColor = System.Drawing.Color.FromArgb(198, 40, 40); // Rojo intenso
      }
      else if (clasif.Contains("Obesidad"))
      {
        _lblDiagnosticoBadge.BackColor = System.Drawing.Color.FromArgb(198, 40, 40); // Rojo intenso
      }
      else if (clasif.Contains("Sobrepeso") || clasif.Contains("Riesgo"))
      {
        _lblDiagnosticoBadge.BackColor = System.Drawing.Color.FromArgb(245, 124, 0); // Ámbar
      }
      else
      {
        _lblDiagnosticoBadge.BackColor = System.Drawing.Color.FromArgb(46, 125, 50); // Verde normal
      }

      decimal imc = ultima.Medicion?.IMC_DNI101 ?? 0;
      decimal peso = ultima.Medicion?.PesoKg_DNI101 ?? 0;
      decimal talla = ultima.Medicion?.TallaCm_DNI101 ?? 0;
      _lblDetallesClinicos.Text = $"Último Control ({ultima.FechaControl_DNI101:dd/MM/yyyy}): Peso {peso:N2} kg, Talla {talla:N2} cm, IMC {imc:N2}. {ultima.Diagnostico?.DetallesClinicos_DNI101}";

      // Alerta activa
      var alerta = ultima.Alertas.FirstOrDefault();
      if (alerta != null || (ultima.Diagnostico?.RequiereAlerta_DNI101 ?? false))
      {
        _pnlAlertaCritica.Visible = true;
        _lblTextoAlerta.Text = alerta?.MensajeAlerta_DNI101 ?? $" ALERTA: Condición nutricional atípica detectada ({clasif}).";
      }
      else
      {
        _pnlAlertaCritica.Visible = false;
      }
    }

    private void ActualizarGrafico()
    {
      _formsPlot.Plot.Clear();

      string sexo = _paciente.Sexo_DNI101 ?? "Masculino";
      string tipoIndicador = "IMC";
      string unidad = "kg/m²";
      string tituloEjeY = "Índice de Masa Corporal (IMC)";

      if (_cmbIndicador.SelectedIndex == 1)
      {
        tipoIndicador = "PESO";
        unidad = "kg";
        tituloEjeY = "Peso (kg)";
      }
      else if (_cmbIndicador.SelectedIndex == 2)
      {
        tipoIndicador = "TALLA";
        unidad = "cm";
        tituloEjeY = "Talla / Longitud (cm)";
      }

      var curva = CurvasOMSData_DNI101.ObtenerCurva(tipoIndicador, sexo);

      // 1. Agregar Curvas de Percentiles de Referencia OMS / CDC
      if (tipoIndicador == "IMC")
      {
        var c95 = _formsPlot.Plot.Add.ScatterLine(curva.Meses, curva.P95);
        c95.Color = new SPColor(211, 47, 47, 220); // Rojo
        c95.LineWidth = 2.2f;
        c95.LegendText = "Percentil 95 (Corte de Obesidad ≥ 95)";

        var c85 = _formsPlot.Plot.Add.ScatterLine(curva.Meses, curva.P85);
        c85.Color = new SPColor(245, 124, 0, 200); // Naranja
        c85.LineWidth = 1.8f;
        c85.LegendText = "Percentil 85 (Corte Sobrepeso 85 a < 95)";

        var c50 = _formsPlot.Plot.Add.ScatterLine(curva.Meses, curva.P50);
        c50.Color = new SPColor(46, 125, 50, 240); // Verde Mediana
        c50.LineWidth = 2.5f;
        c50.LegendText = "Percentil 50 (Mediana / Peso saludable)";

        var c5 = _formsPlot.Plot.Add.ScatterLine(curva.Meses, curva.P5);
        c5.Color = new SPColor(229, 57, 53, 220); // Rojo
        c5.LineWidth = 2.2f;
        c5.LegendText = "Percentil 5 (Corte Bajo peso < 5)";
      }
      else
      {
        var c97 = _formsPlot.Plot.Add.ScatterLine(curva.Meses, curva.P97);
        c97.Color = new SPColor(229, 57, 53, 180); // Rojo
        c97.LineWidth = 2f;
        c97.LegendText = "Percentil 97 (Límite Superior)";

        var c85 = _formsPlot.Plot.Add.ScatterLine(curva.Meses, curva.P85);
        c85.Color = new SPColor(245, 124, 0, 180); // Naranja
        c85.LineWidth = 1.5f;
        c85.LegendText = "Percentil 85 (Alerta Superior)";

        var c50 = _formsPlot.Plot.Add.ScatterLine(curva.Meses, curva.P50);
        c50.Color = new SPColor(46, 125, 50, 240); // Verde Mediana OMS
        c50.LineWidth = 2.5f;
        c50.LegendText = "Percentil 50 (Mediana OMS)";

        var c15 = _formsPlot.Plot.Add.ScatterLine(curva.Meses, curva.P15);
        c15.Color = new SPColor(245, 124, 0, 180); // Naranja
        c15.LineWidth = 1.5f;
        c15.LegendText = "Percentil 15 (Alerta Inferior)";

        var c3 = _formsPlot.Plot.Add.ScatterLine(curva.Meses, curva.P3);
        c3.Color = new SPColor(229, 57, 53, 180); // Rojo
        c3.LineWidth = 2f;
        c3.LegendText = "Percentil 3 (Límite Inferior)";
      }

      // 2. Extraer y Graficar las Mediciones Reales del Paciente
      List<double> pacMeses = new List<double>();
      List<double> pacValores = new List<double>();

      foreach (var c in _historialConsultas)
      {
        if (c.Medicion != null)
        {
          double val = 0;
          if (tipoIndicador == "PESO") val = (double)c.Medicion.PesoKg_DNI101;
          else if (tipoIndicador == "TALLA") val = (double)c.Medicion.TallaCm_DNI101;
          else val = (double)c.Medicion.IMC_DNI101;

          if (val > 0)
          {
            pacMeses.Add(c.EdadMeses_DNI101);
            pacValores.Add(val);
          }
        }
      }

      if (pacMeses.Count > 0)
      {
        var pacSerie = _formsPlot.Plot.Add.Scatter(pacMeses.ToArray(), pacValores.ToArray());
        pacSerie.Color = new SPColor(25, 118, 210, 255); // Azul Rey profesional
        pacSerie.LineWidth = 3f;
        pacSerie.MarkerSize = 10f;
        pacSerie.LegendText = $"Evolución: {_paciente.Nombre_DNI101} ({pacMeses.Count} controles)";
      }

      // Configurar Ejes y Títulos
      _formsPlot.Plot.Axes.Title.Label.Text = $"{_cmbIndicador.Text} — Estándar OMS ({sexo})";
      _formsPlot.Plot.Axes.Title.Label.FontSize = 14;
      _formsPlot.Plot.Axes.Title.Label.Bold = true;

      _formsPlot.Plot.Axes.Bottom.Label.Text = "Edad del Paciente (en meses cumplidos)";
      _formsPlot.Plot.Axes.Bottom.Label.FontSize = 11;

      _formsPlot.Plot.Axes.Left.Label.Text = $"{tituloEjeY} ({unidad})";
      _formsPlot.Plot.Axes.Left.Label.FontSize = 11;

      _formsPlot.Plot.Axes.SetLimitsX(0, 60);

      _formsPlot.Plot.ShowLegend(SPAlignment.UpperLeft);
      _formsPlot.Refresh();
    }
  }
}
