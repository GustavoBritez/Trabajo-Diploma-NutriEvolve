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
            lblResumenDiagnostico.ForeColor = Color.FromArgb(16, 185, 129);

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
            lbl.BackColor = activo ? Color.FromArgb(16, 185, 129) : Color.FromArgb(226, 232, 240);
            lbl.ForeColor = activo ? Color.White : Color.FromArgb(148, 163, 184);
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
            lblResumenDiagnostico.ForeColor = Color.FromArgb(100, 116, 139);
        }
    }
}
