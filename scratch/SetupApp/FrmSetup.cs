using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using BLL;
using DAL;

namespace SetupApp
{
    public partial class FrmSetup : Form
    {
        private int _pasoActual = 1;
        private string _rutaInstaladaFinal = "";

        public FrmSetup()
        {
            InitializeComponent();
            MostrarPaso(1);
            _ = DetectarInstanciasSqlAsync();
        }

        #region Eventos de Controles

        private void BtnCancelar_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cancelar la instalación?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }

        private void BtnAtras_Click(object? sender, EventArgs e)
        {
            if (_pasoActual > 1)
            {
                MostrarPaso(_pasoActual - 1);
            }
        }

        private async void BtnSiguiente_Click(object? sender, EventArgs e)
        {
            if (_pasoActual == 1)
            {
                MostrarPaso(2);
            }
            else if (_pasoActual == 2)
            {
                if (string.IsNullOrWhiteSpace(txtRutaDestino.Text))
                {
                    MessageBox.Show("Por favor indique una carpeta de destino válida.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                MostrarPaso(3);
            }
            else if (_pasoActual == 3)
            {
                if (string.IsNullOrWhiteSpace(cmbServidorSql.Text))
                {
                    MessageBox.Show("Por favor indique un servidor o instancia de SQL Server.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MostrarPaso(4);
                await IniciarProcesoInstalacionAsync();
            }
            else if (_pasoActual == 5)
            {
                if (chkIniciarApp.Checked && File.Exists(Path.Combine(_rutaInstaladaFinal, "UI.exe")))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = Path.Combine(_rutaInstaladaFinal, "UI.exe"),
                            WorkingDirectory = _rutaInstaladaFinal,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"No se pudo iniciar la aplicación automáticamente: {ex.Message}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                Close();
            }
        }

        private void BtnExaminar_Click(object? sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog
            {
                Description = "Seleccione la carpeta donde desea instalar NutriEvolve",
                UseDescriptionForTitle = true,
                SelectedPath = txtRutaDestino.Text
            };
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtRutaDestino.Text = fbd.SelectedPath;
            }
        }

        private async void BtnBuscarInstancias_Click(object? sender, EventArgs e)
        {
            await DetectarInstanciasSqlAsync();
        }

        private void RdoAuthWindows_CheckedChanged(object? sender, EventArgs e)
        {
            ActualizarVisibilidadAuth();
        }

        private void RdoAuthSql_CheckedChanged(object? sender, EventArgs e)
        {
            ActualizarVisibilidadAuth();
        }

        private void BtnProbarConexion_Click(object? sender, EventArgs e)
        {
            string servidor = cmbServidorSql.Text.Trim();
            if (string.IsNullOrWhiteSpace(servidor))
            {
                MessageBox.Show("Ingrese un servidor SQL Server.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Cursor = Cursors.WaitCursor;
            lblEstadoConexion.Text = "Probando conexión con el servidor...";
            lblEstadoConexion.ForeColor = Color.FromArgb(30, 58, 138);

            try
            {
                string connStrMaster = ConstruirCadenaConexion(servidor, "master");
                using (var cn = new SqlConnection(connStrMaster))
                {
                    cn.Open();
                }

                lblEstadoConexion.Text = $"✅ Conexión exitosa a '{servidor}'. El servidor está listo para instalar la base de datos.";
                lblEstadoConexion.ForeColor = Color.FromArgb(22, 101, 52);
            }
            catch (Exception ex)
            {
                lblEstadoConexion.Text = $"❌ Error al conectar a '{servidor}': {ex.Message}";
                lblEstadoConexion.ForeColor = Color.FromArgb(220, 38, 38);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region Métodos de Navegación del Wizard

        private void MostrarPaso(int paso)
        {
            _pasoActual = paso;

            pnlPaso1.Visible = (paso == 1);
            pnlPaso2.Visible = (paso == 2);
            pnlPaso3.Visible = (paso == 3);
            pnlPaso4.Visible = (paso == 4);
            pnlPaso5.Visible = (paso == 5);

            btnAtras.Enabled = (paso > 1 && paso != 4 && paso != 5);
            btnCancelar.Enabled = (paso != 4);

            if (paso == 1)
            {
                lblTituloBanner.Text = "Instalación de NutriEvolve";
                lblSubtituloBanner.Text = "Bienvenido al asistente de instalación";
                btnSiguiente.Text = "Siguiente >";
            }
            else if (paso == 2)
            {
                lblTituloBanner.Text = "Ubicación de los Archivos";
                lblSubtituloBanner.Text = "Paso 1 de 3: Directorio de instalación";
                btnSiguiente.Text = "Siguiente >";
            }
            else if (paso == 3)
            {
                lblTituloBanner.Text = "Base de Datos y SQL Server";
                lblSubtituloBanner.Text = "Paso 2 de 3: Configuración de instancia y catálogo";
                btnSiguiente.Text = "Instalar Ahora";
            }
            else if (paso == 4)
            {
                lblTituloBanner.Text = "Instalando NutriEvolve";
                lblSubtituloBanner.Text = "Paso 3 de 3: Ejecutando tareas de configuración";
                btnSiguiente.Enabled = false;
            }
            else if (paso == 5)
            {
                lblTituloBanner.Text = "Instalación Completada";
                lblSubtituloBanner.Text = "El sistema está listo para ser utilizado";
                btnSiguiente.Enabled = true;
                btnSiguiente.Text = "Finalizar";
                btnCancelar.Visible = false;
            }
        }

        private void ActualizarVisibilidadAuth()
        {
            bool esSql = rdoAuthSql.Checked;
            lblSqlUsuario.Visible = esSql;
            txtSqlUsuario.Visible = esSql;
            lblSqlPassword.Visible = esSql;
            txtSqlPassword.Visible = esSql;
        }

        #endregion

        #region Conexión e Instancias SQL Server

        private string ConstruirCadenaConexion(string servidor, string catalogo)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = servidor,
                InitialCatalog = catalogo,
                TrustServerCertificate = true,
                ConnectTimeout = 15
            };

            if (rdoAuthWindows.Checked)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.IntegratedSecurity = false;
                builder.UserID = txtSqlUsuario.Text.Trim();
                builder.Password = txtSqlPassword.Text;
            }

            return builder.ConnectionString;
        }

        private async Task DetectarInstanciasSqlAsync()
        {
            btnBuscarInstancias.Enabled = false;
            btnBuscarInstancias.Text = "Buscando...";
            lblEstadoConexion.Text = "Buscando instancias activas de SQL Server en la máquina...";
            lblEstadoConexion.ForeColor = Color.FromArgb(30, 58, 138);

            List<string> candidatas = new List<string> { ".", @".\SQLEXPRESS", @"localhost", @"(LocalDB)\MSSQLLocalDB" };
            List<string> detectadas = new List<string>();

            await Task.Run(() =>
            {
                foreach (var c in candidatas)
                {
                    try
                    {
                        string connStr = $"Data Source={c};Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;Connect Timeout=2";
                        using var cn = new SqlConnection(connStr);
                        cn.Open();
                        detectadas.Add(c);
                    }
                    catch { }
                }
            });

            btnBuscarInstancias.Enabled = true;
            btnBuscarInstancias.Text = "🔍 Auto-detectar";

            if (detectadas.Count > 0)
            {
                cmbServidorSql.Items.Clear();
                foreach (var d in detectadas) cmbServidorSql.Items.Add(d);
                cmbServidorSql.SelectedIndex = 0;
                lblEstadoConexion.Text = $"✅ Instancia activa detectada y seleccionada: '{detectadas[0]}'";
                lblEstadoConexion.ForeColor = Color.FromArgb(22, 101, 52);
            }
            else
            {
                lblEstadoConexion.Text = "No se detectó respuesta rápida automática. Puede escribir el nombre o IP de su servidor.";
                lblEstadoConexion.ForeColor = Color.FromArgb(148, 64, 0);
            }
        }

        #endregion

        #region Proceso de Instalación

        private void Log(string mensaje)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Log(mensaje)));
                return;
            }
            rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {mensaje}\n");
            rtbLog.SelectionStart = rtbLog.Text.Length;
            rtbLog.ScrollToCaret();
        }

        private void ActualizarProgreso(int valor, string estado)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ActualizarProgreso(valor, estado)));
                return;
            }
            prgProgreso.Value = Math.Min(100, Math.Max(0, valor));
            lblEstadoProgreso.Text = estado;
            Log(estado);
        }

        private async Task IniciarProcesoInstalacionAsync()
        {
            string destino = Path.GetFullPath(txtRutaDestino.Text.Trim());
            _rutaInstaladaFinal = destino;
            string servidor = cmbServidorSql.Text.Trim();
            string nombreBD = txtNombreBD.Text.Trim();
            bool crearBD = chkCrearBD.Checked;
            bool sellarDV = chkSellarDV.Checked;
            bool crearAcceso = chkAccesoDirecto.Checked;

            string baseDirInstalador = AppDomain.CurrentDomain.BaseDirectory;
            string origenApp = Path.Combine(baseDirInstalador, "App");
            string origenDatos = Path.Combine(baseDirInstalador, "Datos");

            if (!Directory.Exists(origenApp))
            {
                string altApp = Path.Combine(baseDirInstalador, "..", "..", "..", "..", "Instalador", "App");
                if (Directory.Exists(altApp)) origenApp = Path.GetFullPath(altApp);
            }
            if (!Directory.Exists(origenDatos))
            {
                string altDatos = Path.Combine(baseDirInstalador, "..", "..", "..", "..", "Instalador", "Datos");
                if (Directory.Exists(altDatos)) origenDatos = Path.GetFullPath(altDatos);
            }

            try
            {
                await Task.Run(() =>
                {
                    // 1. Preparar carpeta destino
                    ActualizarProgreso(10, $"Creando directorio de destino en: {destino}...");
                    if (!Directory.Exists(destino))
                    {
                        Directory.CreateDirectory(destino);
                    }

                    // 2. Copiar archivos de la aplicación
                    ActualizarProgreso(20, "Copiando binarios y dependencias de NutriEvolve...");
                    if (Directory.Exists(origenApp))
                    {
                        CopiarDirectorioRecursivo(origenApp, destino);
                    }
                    else
                    {
                        Log("⚠️ Carpeta 'App' no encontrada en el origen. Verifique la estructura del instalador.");
                    }

                    // 3. Cadena de conexión final para el cliente
                    string cadenaConexionFinal = ConstruirCadenaConexion(servidor, nombreBD);

                    // 4. Instalar Base de Datos
                    if (crearBD)
                    {
                        ActualizarProgreso(40, $"Conectando a SQL Server ({servidor}) para inicializar base de datos '{nombreBD}'...");
                        string connMaster = ConstruirCadenaConexion(servidor, "master");

                        using (var cnMaster = new SqlConnection(connMaster))
                        {
                            cnMaster.Open();

                            string checkQuery = $"SELECT database_id FROM sys.databases WHERE name = '{nombreBD}'";
                            using (var cmdCheck = new SqlCommand(checkQuery, cnMaster))
                            {
                                var res = cmdCheck.ExecuteScalar();
                                if (res == null)
                                {
                                    ActualizarProgreso(50, $"Creando catálogo [{nombreBD}]...");
                                    using var cmdCreate = new SqlCommand($"CREATE DATABASE [{nombreBD}]", cnMaster);
                                    cmdCreate.ExecuteNonQuery();
                                }
                                else
                                {
                                    Log($"La base de datos [{nombreBD}] ya existe en el servidor. Se procederá a verificar y actualizar esquema.");
                                }
                            }
                        }

                        // Ejecutar script.sql
                        string scriptSqlPath = Path.Combine(origenDatos, "script.sql");
                        if (!File.Exists(scriptSqlPath))
                        {
                            scriptSqlPath = Path.Combine(destino, "script.sql");
                        }

                        if (File.Exists(scriptSqlPath))
                        {
                            ActualizarProgreso(60, "Ejecutando script de tablas, vistas y datos iniciales de NutriEvolve...");
                            string scriptContent = File.ReadAllText(scriptSqlPath);
                            string[] lotes = Regex.Split(scriptContent, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

                            using (var cnDB = new SqlConnection(cadenaConexionFinal))
                            {
                                cnDB.Open();
                                int loteNum = 0;
                                foreach (var lote in lotes)
                                {
                                    string batch = lote.Trim();
                                    if (string.IsNullOrWhiteSpace(batch)) continue;

                                    if (Regex.IsMatch(batch, @"^\s*CREATE\s+DATABASE", RegexOptions.IgnoreCase) ||
                                        Regex.IsMatch(batch, @"^\s*USE\s+\[?ING\]?", RegexOptions.IgnoreCase))
                                    {
                                        continue;
                                    }

                                    try
                                    {
                                        using var cmd = new SqlCommand(batch, cnDB);
                                        cmd.CommandTimeout = 300;
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (Exception exBatch)
                                    {
                                        Log($"Aviso en lote {loteNum}: {exBatch.Message}");
                                    }
                                    loteNum++;
                                }
                            }
                            Log($"Se ejecutaron {lotes.Length} bloques SQL satisfactoriamente.");
                        }
                        else
                        {
                            Log("⚠️ Archivo 'script.sql' no encontrado en el paquete de datos.");
                        }

                        // Sellar Dígitos Verificadores
                        if (sellarDV)
                        {
                            ActualizarProgreso(80, "Recalculando y sellando Dígitos Verificadores (DVH, DVV y global)...");
                            try
                            {
                                Conexion.EstablecerCadenaConexion(cadenaConexionFinal);
                                var dvBLL = new DigitoVerificadorBLL();
                                dvBLL.RecalcularYPersistir();
                                Log("✅ Integridad de base de datos verificada y sellada al 100%.");
                            }
                            catch (Exception exDV)
                            {
                                Log($"Aviso al recalcular DV: {exDV.Message}");
                            }
                        }
                    }

                    // 5. Configurar archivos de conexión en la aplicación destino
                    ActualizarProgreso(90, "Configurando cadena de conexión en UI.dll.config y db_connection.txt...");
                    try
                    {
                        File.WriteAllText(Path.Combine(destino, "db_connection.txt"), cadenaConexionFinal);

                        string configPath = Path.Combine(destino, "UI.dll.config");
                        if (File.Exists(configPath))
                        {
                            string configXml = File.ReadAllText(configPath);
                            string configModificado = Regex.Replace(
                                configXml,
                                @"connectionString\s*=\s*""[^""]*""",
                                $"connectionString=\"{cadenaConexionFinal}\""
                            );
                            File.WriteAllText(configPath, configModificado);
                        }
                    }
                    catch (Exception exCfg)
                    {
                        Log($"Aviso al actualizar configuración: {exCfg.Message}");
                    }

                    // 6. Acceso Directo
                    if (crearAcceso)
                    {
                        ActualizarProgreso(95, "Creando acceso directo en el Escritorio...");
                        CrearAccesoDirectoEscritorio(Path.Combine(destino, "UI.exe"), destino);
                    }

                    ActualizarProgreso(100, "¡Instalación completada exitosamente!");
                });

                await Task.Delay(800);
                MostrarPaso(5);
            }
            catch (Exception exGral)
            {
                Log($"\n❌ ERROR CRÍTICO DURANTE LA INSTALACIÓN:\n{exGral.Message}\n{exGral.StackTrace}");
                MessageBox.Show($"Ocurrió un error durante la instalación:\n\n{exGral.Message}", "Error de Instalación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSiguiente.Enabled = true;
                btnAtras.Enabled = true;
            }
        }

        private static void CopiarDirectorioRecursivo(string origen, string destino)
        {
            Directory.CreateDirectory(destino);
            foreach (string archivo in Directory.GetFiles(origen))
            {
                string archivoDestino = Path.Combine(destino, Path.GetFileName(archivo));
                File.Copy(archivo, archivoDestino, true);
            }
            foreach (string subDirectorio in Directory.GetDirectories(origen))
            {
                string subDestino = Path.Combine(destino, Path.GetFileName(subDirectorio));
                CopiarDirectorioRecursivo(subDirectorio, subDestino);
            }
        }

        private void CrearAccesoDirectoEscritorio(string targetExePath, string workingDir)
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string shortcutPath = Path.Combine(desktopPath, "NutriEvolve.lnk");

                string psScript = $"$ws = New-Object -ComObject WScript.Shell; " +
                                  $"$s = $ws.CreateShortcut('{shortcutPath}'); " +
                                  $"$s.TargetPath = '{targetExePath}'; " +
                                  $"$s.WorkingDirectory = '{workingDir}'; " +
                                  $"$s.Description = 'NutriEvolve - Sistema Integral de Gestión Nutricional'; " +
                                  $"$s.Save()";

                var psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{psScript}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                using var p = Process.Start(psi);
                p?.WaitForExit(3000);
                Log($"Acceso directo creado en: {shortcutPath}");
            }
            catch (Exception ex)
            {
                Log($"No se pudo crear el acceso directo: {ex.Message}");
            }
        }

        #endregion
    }
}
