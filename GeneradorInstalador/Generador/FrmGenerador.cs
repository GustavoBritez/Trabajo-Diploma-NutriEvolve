#pragma warning disable CS8618
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneradorInstalador.Core;

namespace GeneradorInstalador
{
    public partial class FrmGenerador : Form
    {
        private ProjectManifest? _manifestActual;
        private string? _ultimoDirSalidaGenerado;

        public FrmGenerador()
        {
            InitializeComponent();
            this.Load += (_, _) => AjustarLayoutResponsivo();
            this.Resize += (_, _) => AjustarLayoutResponsivo();
            this.grpConfiguracion.Resize += (_, _) => AjustarLayoutResponsivo();
            this.pnlBottom.Resize += (_, _) => AjustarLayoutResponsivo();
        }

        private void AjustarLayoutResponsivo()
        {
            if (grpConfiguracion.ClientSize.Width > 100)
            {
                grpConfiguracion.SuspendLayout();
                int pad = 15;
                int gap = 20;
                int availableWidth = Math.Max(300, grpConfiguracion.ClientSize.Width - (pad * 2) - (gap * 2));

                int col1W = availableWidth / 3;
                int col2W = availableWidth / 3;
                int col3W = availableWidth - col1W - col2W;

                int col1X = pad;
                int col2X = col1X + col1W + gap;
                int col3X = col2X + col2W + gap;

                // Columna 1
                lblNombreApp.Left = col1X;
                txtNombreApp.Left = col1X;
                txtNombreApp.Width = col1W;

                lblDescripcion.Left = col1X;
                txtDescripcion.Left = col1X;
                txtDescripcion.Width = col1W;

                lblNombreBD.Left = col1X;
                txtNombreBD.Left = col1X;
                txtNombreBD.Width = col1W;

                // Columna 2
                lblProyectoPrincipal.Left = col2X;
                cboProyectoPrincipal.Left = col2X;
                cboProyectoPrincipal.Width = col2W;

                lblArchivoSql.Left = col2X;
                cboArchivoSql.Left = col2X;
                cboArchivoSql.Width = col2W;

                btnExaminarSqlManual.Left = col2X;

                // Columna 3
                lblRutaSalida.Left = col3X;
                txtRutaSalida.Left = col3X;
                int btnBrowseW = btnExaminarSalida.Width;
                txtRutaSalida.Width = Math.Max(80, col3W - btnBrowseW - 8);
                btnExaminarSalida.Left = txtRutaSalida.Right + 8;

                chkCompilarRelease.Left = col3X;
                chkGenerarZip.Left = col3X;
                chkDigitosVerificadores.Left = col3X;

                grpConfiguracion.ResumeLayout();
            }

            if (pnlBottom.ClientSize.Width > 100)
            {
                int leftEdge = btnLimpiar.Right + 16;
                int rightEdge = btnProbarInstalador.Left - 16;
                int space = rightEdge - leftEdge;
                if (space > lblCreditos.Width)
                {
                    lblCreditos.Left = leftEdge + (space - lblCreditos.Width) / 2;
                }
                else
                {
                    lblCreditos.Left = leftEdge;
                }
                lblCreditos.Top = (pnlBottom.ClientSize.Height - lblCreditos.Height) / 2;
            }
        }

        private void FrmGenerador_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void FrmGenerador_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (e.Data.GetData(DataFormats.FileDrop) is string[] files && files.Length > 0)
                {
                    CargarEntrada(files[0]);
                }
            }
        }

        private void BtnExaminarZip_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Title = "Seleccionar archivo ZIP con el proyecto .NET completo",
                Filter = "Archivos Comprimidos (*.zip)|*.zip|Todos los archivos (*.*)|*.*"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                CargarEntrada(ofd.FileName);
            }
        }

        private void BtnExaminarCarpeta_Click(object? sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog
            {
                Description = "Seleccione la carpeta que contiene la solución o proyecto .NET:"
            };

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                CargarEntrada(fbd.SelectedPath);
            }
        }

        private async void CargarEntrada(string path)
        {
            txtRutaEntrada.Text = path;
            btnGenerar.Enabled = false;
            btnAbrirSalida.Enabled = false;
            btnProbarInstalador.Enabled = false;
            txtLog.Clear();
            progressBar.Value = 0;

            Log($"Analizando entrada: {path}...");

            await Task.Run(() =>
            {
                try
                {
                    var manifest = ProjectAnalyzer.Analyze(path, Log);
                    _manifestActual = manifest;

                    Invoke(new Action(() =>
                    {
                        ActualizarVistaConDiagnostico(manifest);
                        btnGenerar.Enabled = true;
                        Log("¡Diagnóstico completado! Revise los parámetros y presione 'COMPILAR Y GENERAR'.");
                    }));
                }
                catch (Exception ex)
                {
                    Log($"ERROR AL ANALIZAR: {ex.Message}");
                    Invoke(new Action(() =>
                    {
                        MessageBox.Show($"No se pudo analizar el proyecto seleccionado:\n\n{ex.Message}", "Error de Diagnóstico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            });
        }

        private void ActualizarVistaConDiagnostico(ProjectManifest m)
        {
            // Actualizar Badges
            ActualizarBadge(lblBadgeDal, "DAL", m.HasDal);
            ActualizarBadge(lblBadgeBe, "BE", m.HasBe);
            ActualizarBadge(lblBadgeBll, "BLL", m.HasBll);
            ActualizarBadge(lblBadgeGui, "GUI / UI", !string.IsNullOrEmpty(m.StartupProjectPath));
            ActualizarBadge(lblBadgeSql, "SQL SCRIPT", m.DetectedSqlFiles.Count > 0);
            ActualizarBadge(lblBadgeDv, "DÍGITOS VERIFICADORES", m.HasDigitoVerificador);

            lblResumenDiagnostico.Text = $"Se detectaron {m.DetectedProjects.Count} proyectos, {m.DetectedSqlFiles.Count} scripts SQL y arquitectura en capas.";
            lblResumenDiagnostico.ForeColor = Color.DarkGreen;

            // Campos de texto
            txtNombreApp.Text = m.AppName;
            txtDescripcion.Text = m.AppDescription;
            txtNombreBD.Text = m.DatabaseName;
            txtRutaSalida.Text = m.OutputPath;
            chkDigitosVerificadores.Checked = m.HasDigitoVerificador;

            // Proyectos detectados en combo
            cboProyectoPrincipal.Items.Clear();
            foreach (var proj in m.DetectedProjects)
            {
                cboProyectoPrincipal.Items.Add(Path.GetFileName(proj));
            }
            if (!string.IsNullOrEmpty(m.StartupProjectPath))
            {
                cboProyectoPrincipal.SelectedItem = Path.GetFileName(m.StartupProjectPath);
            }

            // Scripts SQL en combo
            cboArchivoSql.Items.Clear();
            foreach (var sql in m.DetectedSqlFiles)
            {
                cboArchivoSql.Items.Add(Path.GetFileName(sql));
            }
            if (!string.IsNullOrEmpty(m.SelectedSqlScriptPath))
            {
                cboArchivoSql.SelectedItem = Path.GetFileName(m.SelectedSqlScriptPath);
            }
        }

        private void ActualizarBadge(Label lbl, string nombre, bool activo)
        {
            lbl.Text = activo ? $"{nombre} [OK]" : $"{nombre} [-]";
            lbl.ForeColor = activo ? Color.DarkGreen : Color.Gray;
            lbl.BackColor = Color.Transparent;
        }

        private void BtnExaminarSqlManual_Click(object? sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Title = "Seleccionar Script SQL de Base de Datos",
                Filter = "Scripts SQL (*.sql)|*.sql|Todos los archivos (*.*)|*.*"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (_manifestActual != null)
                {
                    _manifestActual.SelectedSqlScriptPath = ofd.FileName;
                    if (!cboArchivoSql.Items.Contains(Path.GetFileName(ofd.FileName)))
                    {
                        cboArchivoSql.Items.Add(Path.GetFileName(ofd.FileName));
                    }
                    cboArchivoSql.SelectedItem = Path.GetFileName(ofd.FileName);
                    ActualizarBadge(lblBadgeSql, "SQL SCRIPT", true);
                    Log($"Script SQL manual seleccionado: {ofd.FileName}");
                }
            }
        }

        private void BtnExaminarSalida_Click(object? sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog
            {
                Description = "Seleccione la carpeta donde se generará el paquete del instalador:"
            };

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtRutaSalida.Text = fbd.SelectedPath;
            }
        }

        private async void BtnGenerar_Click(object? sender, EventArgs e)
        {
            if (_manifestActual == null)
            {
                MessageBox.Show("Primero debe cargar un archivo .ZIP o carpeta de proyecto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Sincronizar datos del formulario al manifest
            _manifestActual.AppName = txtNombreApp.Text.Trim();
            _manifestActual.AppDescription = txtDescripcion.Text.Trim();
            _manifestActual.DatabaseName = txtNombreBD.Text.Trim();
            _manifestActual.OutputPath = txtRutaSalida.Text.Trim();
            _manifestActual.RecompileWithDotnet = chkCompilarRelease.Checked;
            _manifestActual.CreateZipPackage = chkGenerarZip.Checked;
            _manifestActual.HasDigitoVerificador = chkDigitosVerificadores.Checked;

            // Sincronizar proyecto principal seleccionado
            if (cboProyectoPrincipal.SelectedItem != null)
            {
                string selectedName = cboProyectoPrincipal.SelectedItem.ToString()!;
                var matchedProj = _manifestActual.DetectedProjects.Find(p => Path.GetFileName(p).Equals(selectedName, StringComparison.OrdinalIgnoreCase));
                if (matchedProj != null)
                {
                    _manifestActual.StartupProjectPath = matchedProj;
                    string projName = Path.GetFileNameWithoutExtension(matchedProj);
                    _manifestActual.ExeName = $"{projName}.exe";
                    _manifestActual.TargetProcessName = projName;
                }
            }

            // Sincronizar script SQL seleccionado
            if (cboArchivoSql.SelectedItem != null)
            {
                string selectedSql = cboArchivoSql.SelectedItem.ToString()!;
                var matchedSql = _manifestActual.DetectedSqlFiles.Find(s => Path.GetFileName(s).Equals(selectedSql, StringComparison.OrdinalIgnoreCase));
                if (matchedSql != null)
                {
                    _manifestActual.SelectedSqlScriptPath = matchedSql;
                }
            }

            if (string.IsNullOrWhiteSpace(_manifestActual.StartupProjectPath) || !File.Exists(_manifestActual.StartupProjectPath))
            {
                MessageBox.Show("No se especificó un proyecto de inicio válido para compilar. Seleccione un proyecto en 'Proyecto Principal (UI / GUI)'.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_manifestActual.AppName))
            {
                MessageBox.Show("Debe ingresar un nombre para la aplicación.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_manifestActual.DatabaseName))
            {
                MessageBox.Show("Debe ingresar un nombre para la base de datos.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnGenerar.Enabled = false;
            btnExaminarZip.Enabled = false;
            btnExaminarCarpeta.Enabled = false;
            btnAbrirSalida.Enabled = false;
            btnProbarInstalador.Enabled = false;
            progressBar.Value = 10;

            Log("==================================================================");
            Log($"INICIANDO GENERACIÓN DE INSTALADOR / REINSTALADOR: {_manifestActual.AppName}");
            Log("==================================================================");

            await Task.Run(() =>
            {
                try
                {
                    ConnectionInjector.Inject(_manifestActual);
                    string tempAppOut = Path.Combine(Path.GetTempPath(), "GeneradorInstalador", $"publish_{Guid.NewGuid():N}");

                    // 1. Compilación
                    if (_manifestActual.RecompileWithDotnet)
                    {
                        Log("PASO 1/3: Compilando solución con dotnet publish...");
                        bool exitoPublish = ProjectPublisher.Publish(_manifestActual, tempAppOut, Log);
                        if (!exitoPublish)
                        {
                            throw new InvalidOperationException("La compilación de la solución falló. Revise los registros anteriores.");
                        }
                    }
                    else
                    {
                        Log("PASO 1/3: Omitiendo compilación automática, buscando binarios...");
                        // Si no compila, usar carpeta de binarios del proyecto
                        string projDir = Path.GetDirectoryName(_manifestActual.StartupProjectPath) ?? "";
                        string relBin = Path.Combine(projDir, "bin", "Release");
                        if (Directory.Exists(relBin)) tempAppOut = relBin;
                        else tempAppOut = projDir;
                    }
                    SetProgress(50);

                    // 2. Empaquetado
                    Log("PASO 2/3: Ensamblando archivos del instalador, script SQL y manifest...");
                    bool exitoPackage = InstallerPackager.Package(_manifestActual, tempAppOut, Log);
                    if (!exitoPackage)
                    {
                        throw new InvalidOperationException("Ocurrió un error al empaquetar los archivos del instalador.");
                    }
                    SetProgress(90);

                    // 3. Finalización
                    Log("PASO 3/3: Verificando integridad del paquete generado...");
                    _ultimoDirSalidaGenerado = _manifestActual.OutputPath;
                    SetProgress(100);

                    Log("==================================================================");
                    Log($"¡INSTALADOR / REINSTALADOR GENERADO CON ÉXITO!");
                    Log($"Ubicación: {_manifestActual.OutputPath}");
                    if (_manifestActual.CreateZipPackage)
                    {
                        Log($"Archivo ZIP: {_manifestActual.OutputPath}.zip");
                    }
                    Log("==================================================================");

                    Invoke(new Action(() =>
                    {
                        btnGenerar.Enabled = true;
                        btnExaminarZip.Enabled = true;
                        btnExaminarCarpeta.Enabled = true;
                        btnAbrirSalida.Enabled = true;
                        btnProbarInstalador.Enabled = true;

                        MessageBox.Show(
                            $"¡El instalador y reinstalador para '{_manifestActual.AppName}' se generó exitosamente!\n\nCarpeta: {_manifestActual.OutputPath}\n\nPuede probarlo ahora mismo o abrir la carpeta para entregarlo.",
                            "Generación Completada",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }));
                }
                catch (Exception ex)
                {
                    Log($"ERROR DURANTE LA GENERACIÓN: {ex.Message}");
                    Invoke(new Action(() =>
                    {
                        btnGenerar.Enabled = true;
                        btnExaminarZip.Enabled = true;
                        btnExaminarCarpeta.Enabled = true;
                        MessageBox.Show($"Ocurrió un error al generar el instalador:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            });
        }

        private void SetProgress(int val)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetProgress(val)));
                return;
            }
            progressBar.Value = Math.Clamp(val, 0, 100);
        }

        private void Log(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Log(msg)));
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\r\n");
        }

        private void BtnAbrirSalida_Click(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_ultimoDirSalidaGenerado) && Directory.Exists(_ultimoDirSalidaGenerado))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = _ultimoDirSalidaGenerado,
                    UseShellExecute = true
                });
            }
        }

        private void BtnProbarInstalador_Click(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_ultimoDirSalidaGenerado))
            {
                string setupExe = Path.Combine(_ultimoDirSalidaGenerado, "Setup.exe");
                if (File.Exists(setupExe))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = setupExe,
                        WorkingDirectory = _ultimoDirSalidaGenerado,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("No se encontró Setup.exe en la carpeta generada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void BtnLimpiar_Click(object? sender, EventArgs e)
        {
            _manifestActual = null;
            _ultimoDirSalidaGenerado = null;
            txtRutaEntrada.Clear();
            txtNombreApp.Clear();
            txtDescripcion.Text = "Sistema de Gestión .NET";
            txtNombreBD.Clear();
            txtRutaSalida.Clear();
            cboProyectoPrincipal.Items.Clear();
            cboArchivoSql.Items.Clear();
            txtLog.Clear();
            progressBar.Value = 0;
            btnGenerar.Enabled = false;
            btnAbrirSalida.Enabled = false;
            btnProbarInstalador.Enabled = false;

            ActualizarBadge(lblBadgeDal, "DAL", false);
            ActualizarBadge(lblBadgeBe, "BE", false);
            ActualizarBadge(lblBadgeBll, "BLL", false);
            ActualizarBadge(lblBadgeGui, "GUI / UI", false);
            ActualizarBadge(lblBadgeSql, "SQL SCRIPT", false);
            ActualizarBadge(lblBadgeDv, "DÍGITOS VERIFICADORES", false);
            lblResumenDiagnostico.Text = "Esperando selección de archivo o carpeta...";
            lblResumenDiagnostico.ForeColor = Color.Gray;
        }
    }
}
