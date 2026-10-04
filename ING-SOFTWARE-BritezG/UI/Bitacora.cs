using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using BE;
using System.IO;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Services;

namespace UI
{
    public partial class Bitacora : Form, IIdiomaObserver
    {
        private readonly BitacoraBLL _bitacoraBLL = new BitacoraBLL();
        private readonly UsuarioBLL _usuarioBLL = new UsuarioBLL();
        private List<BitacoraBE>? _bitacoraCompleta;
        private readonly IdiomaBLL idiomaBLL = new IdiomaBLL();
        private bool _ignorarEventosFiltro = false;

        public Bitacora()
        {
            InitializeComponent();
            TraductorUI.SuscribirFormulario(this, this);
            TraductorUI.ConfigurarComboIdiomas(cmbIdioma, idiomaBLL);
            ActualizarIdioma();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible)
            {
                RecargarBitacora();
            }
        }

        public void RecargarBitacora()
        {
            try
            {
                _ignorarEventosFiltro = true;

                _bitacoraCompleta = _bitacoraBLL.VerBitacora();

                ActualizarItemsModulos();

                DateTime hoy = DateTime.Today;
                dtpHasta.Value = hoy;
                if (_bitacoraCompleta != null && _bitacoraCompleta.Count > 0)
                {
                    DateTime minFecha = _bitacoraCompleta.Min(b => b._Fecha).Date;
                    dtpDesde.Value = minFecha;
                }
                else
                {
                    dtpDesde.Value = hoy.AddDays(-30);
                }

                if (cmbModulo.Items.Count > 0) cmbModulo.SelectedIndex = 0;
                if (cmbCriticidad.Items.Count > 0) cmbCriticidad.SelectedIndex = 0;
                if (cmbEvento.Items.Count > 0) cmbEvento.SelectedIndex = 0;

                CargarBitacora(_bitacoraCompleta ?? new List<BitacoraBE>());
                ApuntarComboBox();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al recargar bitácora: {ex.Message}");
            }
            finally
            {
                _ignorarEventosFiltro = false;
            }
        }

        private void ActualizarItemsModulos()
        {
            if (_bitacoraCompleta == null) return;
            var modulos = _bitacoraCompleta
                .Select(b => b._Modulo)
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .Distinct()
                .OrderBy(m => m);

            foreach (var m in modulos)
            {
                if (!cmbModulo.Items.Contains(m))
                {
                    cmbModulo.Items.Add(m);
                }
            }
        }

        private void btnSalir_Click(object? sender, EventArgs e)
        {
            FormManager.Navegar(this, FormManager.ObtenerMenuPrincipal());
        }

        private void Bitacora_Load(object? sender, EventArgs e)
        {
            GestionBitacora_Load(sender, e);
            RecargarBitacora();
        }

        private void GestionBitacora_Load(object? sender, EventArgs e)
        {
            InicializarComboBoxCriticidad();
            InicializarComboBoxC();
            InicializarComboBoxEvento();

            cmbModulo.SelectedIndexChanged += CmbCriticidad_SelectedIndexChanged;

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            dgvBitacora.AllowUserToResizeColumns = false;
            dgvBitacora.AllowUserToResizeRows = false;

            dgvBitacora.CellClick += DgvBitacora_CellClick;
        }

        private void InicializarComboBoxCriticidad()
        {
            cmbModulo.Items.Clear();
            cmbModulo.Items.Add("Todos");
            cmbModulo.Items.Add("Login");
            cmbModulo.Items.Add("GestionUsuario");
            cmbModulo.Items.Add("Permisos");
            cmbModulo.Items.Add("Perfiles");
            cmbModulo.Items.Add("Respaldo");
            cmbModulo.Items.Add("Seguridad");
            cmbModulo.Items.Add("TurneroNutricional");
            cmbModulo.Items.Add("AgendaMedica");
            cmbModulo.SelectedIndex = 0;
        }
        private void InicializarComboBoxC()
        {
            cmbCriticidad.Items.Clear();

            cmbCriticidad.Items.Add("Todas");
            cmbCriticidad.Items.Add("1");
            cmbCriticidad.Items.Add("2");
            cmbCriticidad.Items.Add("3");
            cmbCriticidad.Items.Add("4");
            cmbCriticidad.Items.Add("5");

            cmbCriticidad.SelectedIndex = 0;
        }
        private void InicializarComboBoxEvento()
        {
            cmbEvento.Items.Clear();

            cmbEvento.Items.Add("Todos");
            cmbEvento.Items.Add("Inicio de Sesion");
            cmbEvento.Items.Add("Cierre de Sesion");
            cmbEvento.Items.Add("Error");
            cmbEvento.Items.Add("Creacion de Usuario");
            cmbEvento.Items.Add("Modificar Usuario");
            cmbEvento.Items.Add("Desbloqueo de Usuario");
            cmbEvento.Items.Add("Bloqueo de Cuenta");
            cmbEvento.Items.Add("Cambio de Estado");
            cmbEvento.Items.Add("Cambio de Clave");
            cmbEvento.Items.Add("Cambio de Idioma");
            cmbEvento.Items.Add("Eliminacion de Perfil");
            cmbEvento.Items.Add("Creacion de Perfil");
            cmbEvento.Items.Add("Eliminacion de Patente");
            cmbEvento.Items.Add("Creacion de Patente");
            cmbEvento.Items.Add("Eliminacion de Familia");
            cmbEvento.Items.Add("Creacion de Familia");
            cmbEvento.Items.Add("BackUp");
            cmbEvento.Items.Add("Restore");

            cmbEvento.SelectedIndex = 0;
        }
        private void DtpFecha_ValueChanged(object? sender, EventArgs e)
        {
            if (_ignorarEventosFiltro) return;

            DateTime hoy = DateTime.Today;

            if (dtpHasta.Value.Date > hoy)
            {
                idiomaBLL.MostrarMensaje("msg_fecha_futura", "titulo_fecha_futura", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _ignorarEventosFiltro = true;
                dtpHasta.Value = hoy;
                _ignorarEventosFiltro = false;
                return;
            }

            if (dtpDesde.Value.Date > dtpHasta.Value.Date)
            {
                idiomaBLL.MostrarMensaje("msg_fecha_invalida", "titulo_fecha_invalida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _ignorarEventosFiltro = true;
                dtpDesde.Value = dtpHasta.Value.Date;
                _ignorarEventosFiltro = false;
                return;
            }

            AplicarFiltrosCombinados();
        }

        private void CmbCriticidad_SelectedIndexChanged(object? sender, EventArgs e)
        {
            AplicarFiltrosCombinados();
        }

        private void BtnAplicarFiltro_Click(object? sender, EventArgs e)
        {
            AplicarFiltrosCombinados();
        }

        private void AplicarFiltrosCombinados()
        {
            if (_ignorarEventosFiltro || _bitacoraCompleta == null) return;

            try
            {
                DateTime fechaDesde = dtpDesde.Value.Date;
                DateTime fechaHasta = dtpHasta.Value.Date;
                string modulo = cmbModulo.SelectedItem?.ToString() ?? "Todos";
                string criticidad = cmbCriticidad.SelectedItem?.ToString() ?? "Todas";
                string evento = cmbEvento.SelectedItem?.ToString() ?? "Todos";

                IEnumerable<BitacoraBE> filtrada = _bitacoraCompleta;

                // Rango de fechas
                filtrada = filtrada.Where(b => b._Fecha.Date >= fechaDesde && b._Fecha.Date <= fechaHasta);

                // Módulo
                if (!string.IsNullOrEmpty(modulo) && modulo != "Todos" && modulo != "Todas")
                {
                    filtrada = filtrada.Where(b => string.Equals(b._Modulo, modulo, StringComparison.OrdinalIgnoreCase));
                }

                // Criticidad
                if (!string.IsNullOrEmpty(criticidad) && criticidad != "Todas" && criticidad != "Todos")
                {
                    filtrada = filtrada.Where(b => b._Criticidad.ToString() == criticidad);
                }

                // Evento / Descripción
                if (!string.IsNullOrEmpty(evento) && evento != "Todos" && evento != "Todas")
                {
                    if (string.Equals(evento, "Error", StringComparison.OrdinalIgnoreCase))
                    {
                        filtrada = filtrada.Where(b => b._Descripcion != null && b._Descripcion.StartsWith("error", StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        filtrada = filtrada.Where(b => b._Descripcion != null && b._Descripcion.Contains(evento, StringComparison.OrdinalIgnoreCase));
                    }
                }

                CargarBitacora(filtrada.ToList());
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_filtrar", "titulo_error_filtrar", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        private void CargarBitacora(List<BitacoraBE> bitacora)
        {
            dgvBitacora.DataSource = null;
            dgvBitacora.DataSource = bitacora;
            dgvBitacora.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            ConfigurarColumnasGrid();
        }

        private void ConfigurarColumnasGrid()
        {
            if (dgvBitacora.Columns.Contains("_Login"))
            {
                dgvBitacora.Columns["_Login"].HeaderText = idiomaBLL.Traducir("_Login");
            }

            if (dgvBitacora.Columns.Contains("_Fecha"))
            {
                dgvBitacora.Columns["_Fecha"].HeaderText = idiomaBLL.Traducir("_Fecha");
            }

            if (dgvBitacora.Columns.Contains("_Hora"))
            {
                dgvBitacora.Columns["_Hora"].HeaderText = idiomaBLL.Traducir("_Hora");
            }

            if (dgvBitacora.Columns.Contains("_Modulo"))
            {
                dgvBitacora.Columns["_Modulo"].HeaderText = idiomaBLL.Traducir("_Modulo");
            }

            if (dgvBitacora.Columns.Contains("_Evento"))
            {
                dgvBitacora.Columns["_Evento"].HeaderText = idiomaBLL.Traducir("_Evento");
            }

            if (dgvBitacora.Columns.Contains("_Criticidad"))
            {
                dgvBitacora.Columns["_Criticidad"].HeaderText = idiomaBLL.Traducir("_Criticidad");
            }

        }

        private void btnLimpiarFiltros_Click(object? sender, EventArgs e)
        {
            LimpiarFiltros();
            idiomaBLL.MostrarMensaje("msg_filtros_ok", "titulo_filtros_ok", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LimpiarFiltros()
        {
            RecargarBitacora();
        }



        private void btnExportar_Click_1(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Archivos PDF (*.pdf)|*.pdf|Archivos CSV (*.csv)|*.csv";
                saveFileDialog.DefaultExt = "pdf";
                saveFileDialog.FileName = $"Bitacora_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (saveFileDialog.FileName.EndsWith(".pdf"))
                    {
                        ExportarAPDF(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_exportar", "titulo_error_exportar", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }

        private void ExportarAPDF(string rutaArchivo)
        {
            try
            {
                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                XFont fontTitulo = new XFont("Segoe UI", 18, XFontStyle.Bold);
                XFont fontEncabezado = new XFont("Segoe UI", 10, XFontStyle.Bold);
                XFont fontDatos = new XFont("Segoe UI", 9);
                XFont fontPie = new XFont("Segoe UI", 8, XFontStyle.Italic);

                XColor colorEncabezado = XColor.FromArgb(46, 94, 67); // Verde oscuro
                XColor colorTextoEncabezado = XColor.FromArgb(255, 255, 255); // Blanco
                XColor colorTexto = XColor.FromArgb(0, 0, 0); // Negro

                double margenIzq = 20;
                double margenDer = 20;
                double margenSup = 20;
                double margenInf = 20;

                double anchoUtil = page.Width - margenIzq - margenDer;
                double yPos = margenSup;

                gfx.DrawString("📋 Auditoría y Bitácora del Sistema", fontTitulo, XBrushes.DarkGreen,
                    new XRect(margenIzq, yPos, anchoUtil, 30), XStringFormats.TopCenter);
                yPos += 40;

                string filtroInfo = $"Período: {dtpDesde.Value:dd/MM/yyyy} al {dtpHasta.Value:dd/MM/yyyy} | " +
                                    $"Criticidad: {cmbModulo.SelectedItem} | " +
                                    $"Fecha de Exportación: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                gfx.DrawString(filtroInfo, fontDatos, XBrushes.Black,
                    new XRect(margenIzq, yPos, anchoUtil, 15), XStringFormats.TopLeft);
                yPos += 25;

                double[] anchos = { 45, 105, 55, 50, 80, 200 };
                double xPosColumna = margenIzq;
                string[] encabezados = { "ID Bitácora", "Fecha y Hora", "DNI", "Criticidad", "Módulo", "Descripción" };

                for (int i = 0; i < encabezados.Length; i++)
                {
                    gfx.DrawRectangle(new XSolidBrush(colorEncabezado), xPosColumna, yPos, anchos[i], 15);
                    gfx.DrawString(encabezados[i], fontEncabezado, new XSolidBrush(colorTextoEncabezado),
                        new XRect(xPosColumna, yPos, anchos[i], 15), XStringFormats.CenterLeft);
                    xPosColumna += anchos[i];
                }
                yPos += 20;

                if (dgvBitacora.DataSource is List<BitacoraBE> bitacoraData)
                {
                    foreach (BitacoraBE bitacora in bitacoraData)
                    {
                        string descripcionCompleta = bitacora._Descripcion ?? "";
                        List<string> lineasDescripcion = new List<string>();
                        int maxCaracteresPorLinea = 38;

                        if (descripcionCompleta.Length <= maxCaracteresPorLinea)
                        {
                            lineasDescripcion.Add(descripcionCompleta);
                        }
                        else
                        {
                            string temp = descripcionCompleta;
                            while (temp.Length > 0)
                            {
                                if (temp.Length <= maxCaracteresPorLinea)
                                {
                                    lineasDescripcion.Add(temp);
                                    break;
                                }
                                else
                                {
                                    int indiceCorte = temp.LastIndexOf(' ', maxCaracteresPorLinea);
                                    if (indiceCorte <= 0) indiceCorte = maxCaracteresPorLinea; // Si no hay espacios, corta directo

                                    lineasDescripcion.Add(temp.Substring(0, indiceCorte).Trim());
                                    temp = temp.Substring(indiceCorte).Trim();
                                }
                            }
                        }


                        double altoLinea = 15;
                        double altoCelda = lineasDescripcion.Count * altoLinea;
                        if (altoCelda < 15) altoCelda = 15;


                        if (yPos + altoCelda > page.Height - margenInf)
                        {
                            page = document.AddPage();
                            gfx = XGraphics.FromPdfPage(page);
                            yPos = margenSup;

                            xPosColumna = margenIzq;
                            for (int i = 0; i < encabezados.Length; i++)
                            {
                                gfx.DrawRectangle(new XSolidBrush(colorEncabezado), xPosColumna, yPos, anchos[i], 15);
                                gfx.DrawString(encabezados[i], fontEncabezado, new XSolidBrush(colorTextoEncabezado),
                                    new XRect(xPosColumna, yPos, anchos[i], 15), XStringFormats.CenterLeft);
                                xPosColumna += anchos[i];
                            }
                            yPos += 20;
                        }
                        string[] datos = {
                    bitacora._Id_Bitacora.ToString(),
                    bitacora._Fecha.ToString("dd/MM/yyyy HH:mm"),
                    bitacora._Dni.ToString(),
                    bitacora._Criticidad.ToString(),
                    bitacora._Modulo,
                    ""
                };

                        xPosColumna = margenIzq;
                        for (int i = 0; i < datos.Length; i++)
                        {

                            gfx.DrawRectangle(XPens.LightGray, xPosColumna, yPos, anchos[i], altoCelda);

                            if (i < 5) // Columnas normales del 0 al 4
                            {
                                gfx.DrawString(datos[i], fontDatos, new XSolidBrush(colorTexto),
                                    new XRect(xPosColumna + 2, yPos, anchos[i] - 2, altoCelda), XStringFormats.CenterLeft);
                            }
                            else
                            {
                                double yPosInterno = yPos;
                                foreach (string linea in lineasDescripcion)
                                {
                                    gfx.DrawString(linea, fontDatos, new XSolidBrush(colorTexto),
                                        new XRect(xPosColumna + 2, yPosInterno, anchos[i] - 2, altoLinea), XStringFormats.CenterLeft);
                                    yPosInterno += altoLinea;
                                }
                            }

                            xPosColumna += anchos[i];
                        }
                        yPos += altoCelda; // El cursor de la página baja el alto total que usó esta fila
                    }
                }

                yPos = page.Height - margenInf - 10;
                gfx.DrawString($"Exportado el: {DateTime.Now:dd/MM/yyyy HH:mm:ss} | Total de registros: {(dgvBitacora.DataSource is List<BitacoraBE> list ? list.Count : 0)}",
                    fontPie, XBrushes.Gray, new XRect(margenIzq, yPos, anchoUtil, 10), XStringFormats.BottomLeft);

                // Guardar documento
                document.Save(rutaArchivo);

                idiomaBLL.MostrarMensaje("msg_exportar_ok", "titulo_exportar_ok", MessageBoxButtons.OK, MessageBoxIcon.Information, rutaArchivo);
            }
            catch (Exception ex)
            {
                idiomaBLL.MostrarMensaje("msg_error_exportar_pdf", "titulo_error_exportar_pdf", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.Message);
            }
        }


        private void DgvBitacora_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex < 0) return;

            //try
            //{
            //    BitacoraBE bitacora = (BitacoraBE)dgvBitacora.Rows[e.RowIndex].DataBoundItem;

            //    if (bitacora != null)
            //    {
            //        MostrarDetallesUsuarioBitacora(bitacora);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(
            //        $"Error al obtener detalles: {ex.Message}",
            //        "Error",
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Error
            //    );
            //}
        }

        private void btnAplicarFiltro_Click_1(object sender, EventArgs e)
        {

        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltrosCombinados();
        }

        private void lblHasta_Click(object sender, EventArgs e)
        {

        }

        private void panelLateral_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvBitacora_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBitacora.CurrentRow != null)
            {
                string dni = dgvBitacora.CurrentRow.Cells["_Dni"].Value.ToString();

                List<UsuarioBE> usuarios = _usuarioBLL.ListarUsuarios();
                UsuarioBE? usuario = usuarios.FirstOrDefault(u => u._Dni == Convert.ToInt32(dni));


                if (usuario != null)
                {
                    textBox1.Text = usuario._Nombre;
                    textBox2.Text = usuario._Apellido;
                }
            }
        }

        private void cmbEvento_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltrosCombinados();
        }

        #region Idioma
        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                TraductorUI.TraducirFormulario(this, idiomaBLL);
                ConfigurarColumnasGrid();
                TraductorUI.SincronizarComboIdioma(cmbIdioma);
            }
        }
        #endregion

        private void dtpDesde_ValueChanged(object sender, EventArgs e)
        {
            DtpFecha_ValueChanged(sender, e);
        }

        private void dtpHasta_ValueChanged(object sender, EventArgs e)
        {
            DtpFecha_ValueChanged(sender, e);
        }

        private void cmbIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            // El cambio de idioma es gestionado automáticamente por TraductorUI
        }

        private void ApuntarComboBox()
        {
            TraductorUI.SincronizarComboIdioma(cmbIdioma);
        }
    }
}
