#pragma warning disable CS8618
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace UniversalSetup
{
    public partial class FrmUniversalSetup : Form
    {
        private readonly SetupManifest _manifest;
        private int _pasoActual = 1;
        private bool _esReinstalacion = false;
        private bool _baseDatosExisteEnServidor = false;

        public FrmUniversalSetup()
        {
            _manifest = SetupManifest.Load();
            InitializeComponent();
            ConfigurarSegunManifiesto();
            DetectarInstanciaInicial();
            VerificarEstadoReinstalacion();
        }

        private void ConfigurarSegunManifiesto()
        {
            Text = $"Instalador / Reinstalador de {_manifest.AppName}";
            lblBannerTitle.Text = $"{_manifest.AppName} - Instalador y Reinstalador";
            lblBannerSubtitle.Text = string.IsNullOrEmpty(_manifest.AppDescription)
                ? "Asistente inteligente de despliegue para Windows y SQL Server"
                : _manifest.AppDescription;

            lblTituloP1.Text = $"Bienvenido al Asistente de {_manifest.AppName}";
            lblInfoApp.Text = $"Aplicación Principal: {_manifest.AppName} ({_manifest.ExeName})";
            lblInfoBD.Text = $"Base de Datos: {_manifest.DatabaseName} (Script: {Path.GetFileName(_manifest.SqlScriptFile)})";
            lblInfoAutor.Text = $"Versión: {_manifest.Version} | Autor: {_manifest.Author}";

            // Color del banner si se especificó
            if (!string.IsNullOrWhiteSpace(_manifest.PrimaryColorHex))
            {
                try
                {
                    Color primary = ColorTranslator.FromHtml(_manifest.PrimaryColorHex);
                    pnlBanner.BackColor = primary;
                    btnSiguiente.BackColor = primary;
                }
                catch { }
            }

            // Ruta por defecto en AppData Local
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string subFolder = string.IsNullOrWhiteSpace(_manifest.DefaultInstallSubfolder) ? _manifest.AppName : _manifest.DefaultInstallSubfolder;
            txtRutaInstalacion.Text = Path.Combine(localAppData, subFolder);

            chkSellarDV.Visible = _manifest.HasDigitoVerificador;
            chkSellarDV.Checked = _manifest.HasDigitoVerificador;
        }

        private void DetectarInstanciaInicial()
        {
            Task.Run(() =>
            {
                string active = SqlExecutor.DetectActiveInstance();
                Invoke(new Action(() =>
                {
                    if (!cboServidor.Items.Contains(active))
                    {
                        cboServidor.Items.Insert(0, active);
                    }
                    cboServidor.SelectedItem = active;
                    VerificarEstadoReinstalacion();
                }));
            });
        }

        private void TxtRutaInstalacion_TextChanged(object? sender, EventArgs e)
        {
            VerificarEstadoReinstalacion();
        }

        private void VerificarEstadoReinstalacion()
        {
            string rutaDestino = txtRutaInstalacion.Text.Trim();
            string exeDestino = Path.Combine(rutaDestino, _manifest.ExeName);

            bool carpetaInstalada = File.Exists(exeDestino);

            string servidor = cboServidor.Text.Trim();
            bool authWin = rdoAuthWindows.Checked;
            string? user = authWin ? null : txtUsuarioSql.Text.Trim();
            string? pass = authWin ? null : txtClaveSql.Text;

            _baseDatosExisteEnServidor = SqlExecutor.DatabaseExists(servidor, _manifest.DatabaseName, authWin, user, pass);
            _esReinstalacion = carpetaInstalada || _baseDatosExisteEnServidor;

            if (_esReinstalacion)
            {
                lblBadgeModo.Text = "REINSTALACIÓN";
                lblBadgeModo.BackColor = Color.FromArgb(234, 88, 12);
                lblAvisoModoP1.Text = "Se detectó una instalación previa. El asistente actuará en MODO REINSTALADOR / REPARADOR.";
                lblAvisoModoP1.ForeColor = Color.FromArgb(194, 65, 12);

                lblAvisoInstalacionPreviaP2.Visible = carpetaInstalada;
                grpModoBDReinstalacion.Visible = _baseDatosExisteEnServidor;
            }
            else
            {
                lblBadgeModo.Text = "NUEVA INSTALACIÓN";
                lblBadgeModo.BackColor = Color.FromArgb(16, 185, 129);
                lblAvisoModoP1.Text = "Modo de Instalación inicial listo para comenzar.";
                lblAvisoModoP1.ForeColor = Color.FromArgb(16, 185, 129);

                lblAvisoInstalacionPreviaP2.Visible = false;
                grpModoBDReinstalacion.Visible = false;
            }

            btnSiguiente.Text = (_pasoActual == 3)
                ? (_esReinstalacion ? "Reinstalar Ahora" : "Instalar Ahora")
                : (_pasoActual == 5 ? "Finalizar" : "Siguiente >");
        }

        private void RdoAuth_CheckedChanged(object? sender, EventArgs e)
        {
            bool sqlAuth = rdoAuthSql.Checked;
            lblUsuarioSql.Enabled = sqlAuth;
            txtUsuarioSql.Enabled = sqlAuth;
            lblClaveSql.Enabled = sqlAuth;
            txtClaveSql.Enabled = sqlAuth;
            VerificarEstadoReinstalacion();
        }

        private void BtnExaminarRuta_Click(object? sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            fbd.Description = "Seleccione la carpeta de instalación de la aplicación:";
            if (Directory.Exists(txtRutaInstalacion.Text))
            {
                fbd.SelectedPath = txtRutaInstalacion.Text;
            }

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtRutaInstalacion.Text = fbd.SelectedPath;
            }
        }

        private void BtnProbarConexion_Click(object? sender, EventArgs e)
        {
            string servidor = cboServidor.Text.Trim();
            if (string.IsNullOrWhiteSpace(servidor))
            {
                MessageBox.Show("Por favor seleccione o ingrese el nombre de la instancia de SQL Server.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnProbarConexion.Enabled = false;
            btnProbarConexion.Text = "Probando...";

            Task.Run(() =>
            {
                bool winAuth = rdoAuthWindows.Checked;
                string? u = winAuth ? null : txtUsuarioSql.Text.Trim();
                string? p = winAuth ? null : txtClaveSql.Text;

                bool exito = SqlExecutor.TestServerConnection(servidor, winAuth, u, p);
                bool bdExiste = exito && SqlExecutor.DatabaseExists(servidor, _manifest.DatabaseName, winAuth, u, p);

                Invoke(new Action(() =>
                {
                    btnProbarConexion.Enabled = true;
                    btnProbarConexion.Text = "Probar Conexión";

                    if (exito)
                    {
                        string msgBd = bdExiste
                            ? $"\n\nCatálogo '{_manifest.DatabaseName}': YA EXISTE en este servidor."
                            : $"\n\nCatálogo '{_manifest.DatabaseName}': Aún no existe (se creará durante la instalación).";

                        MessageBox.Show($"¡Conexión exitosa a la instancia '{servidor}'!{msgBd}", "Conexión Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        VerificarEstadoReinstalacion();
                    }
                    else
                    {
                        MessageBox.Show($"No se pudo conectar a la instancia '{servidor}'. Verifique que SQL Server esté en ejecución y los permisos sean correctos.", "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }));
            });
        }

        private void BtnAnterior_Click(object? sender, EventArgs e)
        {
            if (_pasoActual > 1 && _pasoActual < 4)
            {
                _pasoActual--;
                MostrarPaso(_pasoActual);
            }
        }

        private void BtnSiguiente_Click(object? sender, EventArgs e)
        {
            if (_pasoActual == 1)
            {
                _pasoActual = 2;
                MostrarPaso(_pasoActual);
            }
            else if (_pasoActual == 2)
            {
                if (string.IsNullOrWhiteSpace(txtRutaInstalacion.Text))
                {
                    MessageBox.Show("Debe especificar una ruta de instalación válida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                _pasoActual = 3;
                MostrarPaso(_pasoActual);
            }
            else if (_pasoActual == 3)
            {
                // Iniciar instalación
                if (_esReinstalacion && _baseDatosExisteEnServidor && rdoRecrearBD.Checked)
                {
                    var confirm = MessageBox.Show(
                        $"Ha seleccionado RECREAR la base de datos '{_manifest.DatabaseName}' desde cero.\n\nEsto ELIMINARÁ y sobrescribirá todos los datos existentes.\n\n¿Está seguro de continuar?",
                        "Confirmar Sobrescritura de Base de Datos",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2);

                    if (confirm != DialogResult.Yes) return;
                }

                _pasoActual = 4;
                MostrarPaso(_pasoActual);
                EjecutarInstalacionAsync();
            }
            else if (_pasoActual == 5)
            {
                if (chkIniciarApp.Checked)
                {
                    string exePath = Path.Combine(txtRutaInstalacion.Text, _manifest.ExeName);
                    if (File.Exists(exePath))
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = exePath,
                            WorkingDirectory = txtRutaInstalacion.Text,
                            UseShellExecute = true
                        });
                    }
                }
                Close();
            }
        }

        private void BtnCancelar_Click(object? sender, EventArgs e)
        {
            if (_pasoActual == 4)
            {
                MessageBox.Show("La instalación está en curso, por favor espere a que finalice.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("¿Está seguro de que desea salir del asistente?", "Confirmar Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }

        private void MostrarPaso(int paso)
        {
            pnlPaso1_Bienvenida.Visible = (paso == 1);
            pnlPaso2_Ruta.Visible = (paso == 2);
            pnlPaso3_BaseDatos.Visible = (paso == 3);
            pnlPaso4_Progreso.Visible = (paso == 4);
            pnlPaso5_Finalizado.Visible = (paso == 5);

            btnAnterior.Enabled = (paso > 1 && paso < 4);

            if (paso == 3)
            {
                btnSiguiente.Text = _esReinstalacion ? "Reinstalar Ahora" : "Instalar Ahora";
                btnSiguiente.BackColor = _esReinstalacion ? Color.FromArgb(234, 88, 12) : Color.FromArgb(37, 99, 235);
            }
            else if (paso == 4)
            {
                btnSiguiente.Enabled = false;
                btnCancelar.Enabled = false;
                btnAnterior.Enabled = false;
            }
            else if (paso == 5)
            {
                btnSiguiente.Enabled = true;
                btnSiguiente.Text = "Finalizar";
                btnSiguiente.BackColor = Color.FromArgb(16, 185, 129);
                btnCancelar.Visible = false;
                btnAnterior.Visible = false;
            }
            else
            {
                btnSiguiente.Text = "Siguiente >";
                btnSiguiente.BackColor = Color.FromArgb(37, 99, 235);
            }
        }

        private void Log(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Log(msg)));
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\r\n");
            lblEstadoP4.Text = msg;
        }

        private void SetProgress(int value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetProgress(value)));
                return;
            }
            progressBar.Value = Math.Clamp(value, 0, 100);
        }

        private async void EjecutarInstalacionAsync()
        {
            string rutaDestino = txtRutaInstalacion.Text.Trim();
            string servidor = cboServidor.Text.Trim();
            bool authWin = rdoAuthWindows.Checked;
            string? user = authWin ? null : txtUsuarioSql.Text.Trim();
            string? pass = authWin ? null : txtClaveSql.Text;
            bool instalarBD = chkInstalarBD.Checked;
            bool recrearBD = _esReinstalacion && rdoRecrearBD.Checked;
            bool sellarDV = chkSellarDV.Checked;
            bool crearAcceso = chkCrearAccesoDirecto.Checked;

            await Task.Run(() =>
            {
                try
                {
                    Log($"Iniciando {(_esReinstalacion ? "REINSTALACIÓN" : "INSTALACIÓN")} de {_manifest.AppName}...");
                    SetProgress(5);

                    // 1. Cerrar procesos existentes
                    CerrarProcesosEnEjecucion();
                    SetProgress(15);

                    // 2. Preparar directorio
                    if (!Directory.Exists(rutaDestino))
                    {
                        Log($"Creando directorio de destino: {rutaDestino}");
                        Directory.CreateDirectory(rutaDestino);
                    }
                    SetProgress(20);

                    // 3. Copiar archivos de App\
                    string appSourceDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App");
                    if (Directory.Exists(appSourceDir))
                    {
                        Log("Copiando archivos de la aplicación...");
                        CopiarDirectorio(appSourceDir, rutaDestino, true);
                    }
                    else
                    {
                        Log("AVISO: No se encontró la carpeta 'App', se continuará con los archivos disponibles.");
                    }
                    SetProgress(50);

                    // 4. Actualizar configuración de conexión en archivos .config y appsettings.json
                    Log("Configurando parámetros de conexión en la aplicación...");
                    ActualizarConfiguracionConexion(rutaDestino, servidor, _manifest.DatabaseName, authWin, user, pass);
                    SetProgress(60);

                    // 5. Instalar o reparar Base de Datos
                    if (instalarBD)
                    {
                        string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _manifest.SqlScriptFile);
                        if (!File.Exists(scriptPath))
                        {
                            // Intentar en Datos/
                            string altScript = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Datos", Path.GetFileName(_manifest.SqlScriptFile));
                            if (File.Exists(altScript)) scriptPath = altScript;
                        }

                        if (File.Exists(scriptPath))
                        {
                            if (recrearBD)
                            {
                                Log($"Recreando base de datos '{_manifest.DatabaseName}' desde cero...");
                                SqlExecutor.DropDatabase(servidor, _manifest.DatabaseName, authWin, user, pass, Log);
                            }

                            Log($"Ejecutando script SQL maestro en {servidor}...");
                            SqlExecutor.ExecuteScript(servidor, _manifest.DatabaseName, scriptPath, authWin, user, pass, Log);
                        }
                        else
                        {
                            Log($"ADVERTENCIA: No se encontró el script SQL '{_manifest.SqlScriptFile}'. Se continuará.");
                        }
                    }
                    SetProgress(80);

                    // 6. Recalcular Dígitos Verificadores si aplica
                    if (sellarDV)
                    {
                        Log("Recalculando Dígitos Verificadores de integridad...");
                        SqlExecutor.TryRecalculateDigitosVerificadores(rutaDestino, Log);
                    }
                    SetProgress(90);

                    // 7. Acceso directo
                    if (crearAcceso)
                    {
                        Log("Generando acceso directo en el Escritorio...");
                        CrearAccesoDirectoEnEscritorio(rutaDestino, _manifest.ExeName, _manifest.AppName);
                    }
                    SetProgress(100);

                    Log($"¡{(_esReinstalacion ? "Reinstalación" : "Instalación")} completada con éxito!");

                    Invoke(new Action(() =>
                    {
                        _pasoActual = 5;
                        MostrarPaso(_pasoActual);
                    }));
                }
                catch (Exception ex)
                {
                    Log($"ERROR CRÍTICO: {ex.Message}");
                    Invoke(new Action(() =>
                    {
                        btnCancelar.Enabled = true;
                        MessageBox.Show($"Ocurrió un error durante la instalación:\n\n{ex.Message}", "Error de Instalación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
            });
        }

        private void CerrarProcesosEnEjecucion()
        {
            string procName = Path.GetFileNameWithoutExtension(_manifest.TargetProcessName);
            if (string.IsNullOrWhiteSpace(procName))
            {
                procName = Path.GetFileNameWithoutExtension(_manifest.ExeName);
            }

            try
            {
                var procesos = Process.GetProcessesByName(procName);
                if (procesos.Length > 0)
                {
                    Log($"Se detectó '{procName}' en ejecución. Cerrando proceso para evitar bloqueos de archivos...");
                    foreach (var p in procesos)
                    {
                        try
                        {
                            p.Kill();
                            p.WaitForExit(3000);
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Aviso al comprobar procesos: {ex.Message}");
            }
        }

        private void CopiarDirectorio(string sourceDir, string targetDir, bool overwrite)
        {
            var dir = new DirectoryInfo(sourceDir);
            foreach (var file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(targetDir, file.Name);
                file.CopyTo(targetFilePath, overwrite);
            }

            foreach (var subDir in dir.GetDirectories())
            {
                string targetSubDir = Path.Combine(targetDir, subDir.Name);
                if (!Directory.Exists(targetSubDir))
                {
                    Directory.CreateDirectory(targetSubDir);
                }
                CopiarDirectorio(subDir.FullName, targetSubDir, overwrite);
            }
        }

        private void ActualizarConfiguracionConexion(string appDir, string server, string database, bool useWindowsAuth, string? user, string? password)
        {
            string connString = SqlExecutor.BuildConnectionString(server, database, useWindowsAuth, user, password);

            // 1. Guardar archivo de respaldo plano db_connection.txt (usado por arquitecturas DAL dinámicas)
            try
            {
                string txtConn = Path.Combine(appDir, "db_connection.txt");
                File.WriteAllText(txtConn, connString);
            }
            catch { }

            // 2. Actualizar todos los archivos .config (ej. UI.dll.config, app.config)
            foreach (var configFile in Directory.GetFiles(appDir, "*.config", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    var xml = new XmlDocument();
                    xml.Load(configFile);

                    var addNodes = xml.SelectNodes("//connectionStrings/add");
                    if (addNodes != null && addNodes.Count > 0)
                    {
                        foreach (XmlNode node in addNodes)
                        {
                            if (node.Attributes?["connectionString"] != null)
                            {
                                node.Attributes["connectionString"]!.Value = connString;
                            }
                        }
                        xml.Save(configFile);
                        Log($"Cadena de conexión actualizada en: {Path.GetFileName(configFile)}");
                    }
                }
                catch { }
            }
        }

        private void CrearAccesoDirectoEnEscritorio(string appDir, string exeName, string appName)
        {
            try
            {
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string shortcutPath = Path.Combine(desktop, $"{appName}.lnk");
                string targetExe = Path.Combine(appDir, exeName);

                if (!File.Exists(targetExe)) return;

                // Crear mediante script temporal de PowerShell para máxima compatibilidad sin dependencias COM externas
                string psCommand = $@"$ws = New-Object -ComObject WScript.Shell; $s = $ws.CreateShortcut('{shortcutPath}'); $s.TargetPath = '{targetExe}'; $s.WorkingDirectory = '{appDir}'; $s.Save();";

                var psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{psCommand}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                using var p = Process.Start(psi);
                p?.WaitForExit(5000);
            }
            catch (Exception ex)
            {
                Log($"Aviso: No se pudo crear el acceso directo: {ex.Message}");
            }
        }
    }
}
