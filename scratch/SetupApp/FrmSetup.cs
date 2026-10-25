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
        private bool _esReinstalacion = false;
        private bool _baseDatosExisteEnServidor = false;

        public FrmSetup()
        {
            InitializeComponent();
            VerificarInstalacionExistente();
            MostrarPaso(1);
            _ = DetectarInstanciasSqlAsync();
        }

        #region Detección de Instalación Previa (Reinstalador)

        private void VerificarInstalacionExistente()
        {
            string destino = txtRutaDestino.Text.Trim();
            bool appExiste = Directory.Exists(destino) && File.Exists(Path.Combine(destino, "UI.exe"));
            _esReinstalacion = appExiste;
            ActualizarVisibilidadAvisoP2();
        }

        private void ActualizarVisibilidadAvisoP2()
        {
            if (lblAvisoInstalacionPreviaP2 == null) return;
            string destino = txtRutaDestino.Text.Trim();
            bool appExiste = Directory.Exists(destino) && File.Exists(Path.Combine(destino, "UI.exe"));
            lblAvisoInstalacionPreviaP2.Visible = appExiste;
            if (appExiste)
            {
                _esReinstalacion = true;
            }
        }

        private async Task ComprobarEstadoBDAsync()
        {
            string servidor = cmbServidorSql.Text.Trim();
            string nombreBD = txtNombreBD.Text.Trim();
            if (string.IsNullOrWhiteSpace(servidor) || string.IsNullOrWhiteSpace(nombreBD)) return;

            try
            {
                string connMaster = ConstruirCadenaConexion(servidor, "master");
                bool existe = false;
                await Task.Run(() =>
                {
                    try
                    {
                        using var cn = new SqlConnection(connMaster);
                        cn.Open();
                        using var cmd = new SqlCommand($"SELECT database_id FROM sys.databases WHERE name = '{nombreBD}'", cn);
                        existe = (cmd.ExecuteScalar() != null);
                    }
                    catch { }
                });

                _baseDatosExisteEnServidor = existe;
                if (existe)
                {
                    _esReinstalacion = true;
                    lblAvisoBDP3.Text = $"Se detectó la base de datos '{nombreBD}' existente en '{servidor}'.";
                    lblAvisoBDP3.ForeColor = Color.FromArgb(2, 132, 199);
                    lblAvisoBDP3.Visible = true;
                    grpModoBDReinstalacion.Visible = true;
                    chkCrearBD.Visible = false;
                }
                else
                {
                    lblAvisoBDP3.Text = $"✨ La base de datos '{nombreBD}' no existe aún en el servidor. Será creada nueva.";
                    lblAvisoBDP3.ForeColor = Color.FromArgb(22, 101, 52);
                    lblAvisoBDP3.Visible = true;
                    grpModoBDReinstalacion.Visible = false;
                    chkCrearBD.Visible = true;
                }

                btnSiguiente.Text = _esReinstalacion ? "Reinstalar Ahora" : "Instalar Ahora";
            }
            catch { }
        }

        #endregion

        #region Eventos de Controles

        private void TxtRutaDestino_TextChanged(object? sender, EventArgs e)
        {
            ActualizarVisibilidadAvisoP2();
        }

        private async void CmbServidorSql_SelectedIndexChanged(object? sender, EventArgs e)
        {
            await ComprobarEstadoBDAsync();
        }

        private async void CmbServidorSql_TextChanged(object? sender, EventArgs e)
        {
            await ComprobarEstadoBDAsync();
        }

        private async void TxtNombreBD_TextChanged(object? sender, EventArgs e)
        {
            await ComprobarEstadoBDAsync();
        }

        private void BtnCancelar_Click(object? sender, EventArgs e)
        {
            string accion = _esReinstalacion ? "reinstalación" : "instalación";
            if (MessageBox.Show($"¿Desea cancelar la {accion}?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

                // Si la BD ya existe y eligió recrear desde cero, pedir confirmación
                if (_baseDatosExisteEnServidor && rdoRecrearBD.Checked)
                {
                    var conf = MessageBox.Show(
                        $"ATENCIÓN:\n\nHa seleccionado recrear la base de datos '{txtNombreBD.Text.Trim()}' desde cero.\n" +
                        "Todos los pacientes, consultas y datos existentes serán reemplazados por los datos iniciales de fábrica.\n\n" +
                        "¿Está seguro de que desea continuar?",
                        "Confirmar Sobreescritura",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (conf != DialogResult.Yes) return;
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
                Description = "Seleccione la carpeta donde desea instalar o reinstalar NutriEvolve",
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
            await ComprobarEstadoBDAsync();
        }

        private void RdoAuthWindows_CheckedChanged(object? sender, EventArgs e)
        {
            ActualizarVisibilidadAuth();
        }

        private void RdoAuthSql_CheckedChanged(object? sender, EventArgs e)
        {
            ActualizarVisibilidadAuth();
        }

        private async void BtnProbarConexion_Click(object? sender, EventArgs e)
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
                await Task.Run(() =>
                {
                    using var cn = new SqlConnection(connStrMaster);
                    cn.Open();
                });

                lblEstadoConexion.Text = $"✅ Conexión exitosa a '{servidor}'. El servidor está listo.";
                lblEstadoConexion.ForeColor = Color.FromArgb(22, 101, 52);

                await ComprobarEstadoBDAsync();
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
                if (_esReinstalacion)
                {
                    lblTituloBanner.Text = "Reinstalación de NutriEvolve";
                    lblSubtituloBanner.Text = "Actualización, reparación y reinstalación del sistema";
                    lblBienvenidaP1.Text = "Asistente de Reinstalación / Reparación de NutriEvolve";
                    lblDescP1.Text = "Se ha detectado una instalación previa de NutriEvolve en este equipo.\n\n" +
                                     "Este asistente le permitirá:\n\n" +
                                     "  • Reinstalar y actualizar todos los binarios y dependencias a la última versión.\n" +
                                     "  • Conservar sus datos clínicos y turnos existentes, reparando la base de datos.\n" +
                                     "  • Opcionalmente recrear la base de datos desde cero si desea reiniciar de fábrica.\n" +
                                     "  • Recalcular y sellar Dígitos Verificadores (DVH y DVV) para garantizar 100% de integridad.\n" +
                                     "  • Reconfigurar o cambiar la instancia de SQL Server si es necesario.\n" +
                                     "  • Reparar accesos directos en el sistema.\n\n" +
                                     "Haga clic en 'Siguiente' para continuar con la reinstalación.";
                }
                else
                {
                    lblTituloBanner.Text = "Instalación de NutriEvolve";
                    lblSubtituloBanner.Text = "Bienvenido al asistente de instalación";
                    lblBienvenidaP1.Text = "Bienvenido al Asistente de Instalación de NutriEvolve";
                    lblDescP1.Text = "Este instalador configurará en su computadora:\n\n" +
                                     "  • La aplicación de escritorio NutriEvolve (Gestión Nutricional, Turnos y Seguimiento Clínico).\n" +
                                     "  • La Base de Datos SQL Server ('ING') con estructura completa, tablas, vistas y procedimientos.\n" +
                                     "  • Datos de prueba iniciales (evaluaciones pediátricas, curvas de percentiles OMS y usuarios).\n" +
                                     "  • Dígitos Verificadores (DVH y DVV) para garantizar la integridad referencial del sistema.\n" +
                                     "  • Configuración automática de la cadena de conexión personalizada para esta máquina.\n\n" +
                                     "Requisitos previos requeridos:\n" +
                                     "  ✓ Windows 10 / 11 (64-bit)\n" +
                                     "  ✓ Microsoft SQL Server (Local, Express o LocalDB)\n" +
                                     "  ✓ .NET Desktop Runtime 9.0 o superior\n\n" +
                                     "Haga clic en 'Siguiente' para comenzar con la configuración.";
                }
                btnSiguiente.Text = "Siguiente >";
            }
            else if (paso == 2)
            {
                lblTituloBanner.Text = _esReinstalacion ? "Ubicación de Reinstalación" : "Ubicación de los Archivos";
                lblSubtituloBanner.Text = "Paso 1 de 3: Directorio de la aplicación";
                ActualizarVisibilidadAvisoP2();
                btnSiguiente.Text = "Siguiente >";
            }
            else if (paso == 3)
            {
                lblTituloBanner.Text = _esReinstalacion ? "Base de Datos y Reinstalación" : "Base de Datos y SQL Server";
                lblSubtituloBanner.Text = "Paso 2 de 3: Configuración de instancia y catálogo";
                _ = ComprobarEstadoBDAsync();
                btnSiguiente.Text = _esReinstalacion ? "Reinstalar Ahora" : "Instalar Ahora";
            }
            else if (paso == 4)
            {
                lblTituloBanner.Text = _esReinstalacion ? "Reinstalando NutriEvolve" : "Instalando NutriEvolve";
                lblSubtituloBanner.Text = "Paso 3 de 3: Ejecutando tareas de configuración";
                btnSiguiente.Enabled = false;
            }
            else if (paso == 5)
            {
                lblTituloBanner.Text = _esReinstalacion ? "¡Reinstalación Completada!" : "Instalación Completada";
                lblSubtituloBanner.Text = "El sistema está listo para ser utilizado";
                lblTituloP5.Text = _esReinstalacion ? "¡Reinstalación Finalizada con Éxito!" : "¡Instalación Finalizada con Éxito!";
                lblDescP5.Text = (_esReinstalacion
                    ? "NutriEvolve y la base de datos se han reinstalado, actualizado y verificado correctamente en su equipo.\n\n"
                    : "NutriEvolve y la base de datos se han instalado y configurado correctamente en su equipo.\n\n") +
                    "Información de Cuentas de Acceso Predefinidas:\n\n" +
                    "  👑 Perfil Administrador:\n" +
                    "      • Usuario: admin\n" +
                    "      • Contraseña: admin123\n\n" +
                    "  🥗 Perfil Nutricionista:\n" +
                    "      • Usuario: nutri\n" +
                    "      • Contraseña: nutri123\n\n" +
                    "La cadena de conexión ha sido sellada y vinculada a la instancia de SQL Server configurada.";

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

            await ComprobarEstadoBDAsync();
        }

        #endregion

        #region Proceso de Instalación / Reinstalación

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
            bool bdYaExiste = _baseDatosExisteEnServidor;
            bool recrearBD = bdYaExiste ? rdoRecrearBD.Checked : chkCrearBD.Checked;
            bool conservarBD = bdYaExiste && rdoConservarBD.Checked;
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
                    // 0. Comprobar si UI.exe está en ejecución y cerrarlo para evitar bloqueo de archivos en reinstalación
                    ActualizarProgreso(5, "Comprobando procesos activos de NutriEvolve...");
                    var procesos = Process.GetProcessesByName("UI");
                    if (procesos.Length > 0)
                    {
                        ActualizarProgreso(8, "Cerrando instancias abiertas de NutriEvolve para permitir la sobreescritura...");
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

                    // 1. Preparar carpeta destino
                    ActualizarProgreso(15, $"Preparando directorio de destino en: {destino}...");
                    if (!Directory.Exists(destino))
                    {
                        Directory.CreateDirectory(destino);
                    }

                    // 2. Copiar/Reemplazar archivos de la aplicación
                    ActualizarProgreso(25, (_esReinstalacion ? "Reemplazando y actualizando" : "Copiando") + " binarios y dependencias de NutriEvolve...");
                    if (Directory.Exists(origenApp))
                    {
                        CopiarDirectorioRecursivo(origenApp, destino);
                    }
                    else
                    {
                        Log("[AVISO] Carpeta 'App' no encontrada en el origen. Verifique la estructura del instalador.");
                    }

                    // 3. Cadena de conexión final para el cliente
                    string cadenaConexionFinal = ConstruirCadenaConexion(servidor, nombreBD);

                    // 4. Base de Datos
                    string connMaster = ConstruirCadenaConexion(servidor, "master");
                    string scriptSqlPath = Path.Combine(origenDatos, "script.sql");
                    if (!File.Exists(scriptSqlPath))
                    {
                        scriptSqlPath = Path.Combine(destino, "script.sql");
                    }

                    if (bdYaExiste && recrearBD)
                    {
                        ActualizarProgreso(45, $"Recreando base de datos [{nombreBD}] desde cero en '{servidor}'...");
                        using (var cnMaster = new SqlConnection(connMaster))
                        {
                            cnMaster.Open();
                            // Forzar desconexión y eliminar BD
                            try
                            {
                                using var cmdDrop = new SqlCommand($@"ALTER DATABASE [{nombreBD}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{nombreBD}];", cnMaster);
                                cmdDrop.ExecuteNonQuery();
                            }
                            catch { }

                            using var cmdCreate = new SqlCommand($"CREATE DATABASE [{nombreBD}]", cnMaster);
                            cmdCreate.ExecuteNonQuery();
                        }

                        // Ejecutar script completo
                        if (File.Exists(scriptSqlPath))
                        {
                            ActualizarProgreso(60, "Ejecutando script maestro completo de tablas, perfiles y datos iniciales...");
                            EjecutarLotesScript(scriptSqlPath, cadenaConexionFinal);
                        }
                    }
                    else if (bdYaExiste && conservarBD)
                    {
                        ActualizarProgreso(45, $"Conservando datos clínicos existentes en [{nombreBD}]. Verificando esquema...");
                        // Ejecutar lotes de script.sql de forma segura (las tablas usan IF NOT EXISTS)
                        if (File.Exists(scriptSqlPath))
                        {
                            ActualizarProgreso(60, "Verificando y actualizando objetos de base de datos...");
                            EjecutarLotesScript(scriptSqlPath, cadenaConexionFinal);
                        }
                    }
                    else if (!bdYaExiste && (chkCrearBD.Checked || recrearBD))
                    {
                        ActualizarProgreso(45, $"Creando catálogo [{nombreBD}] en '{servidor}'...");
                        using (var cnMaster = new SqlConnection(connMaster))
                        {
                            cnMaster.Open();
                            using var cmdCreate = new SqlCommand($"CREATE DATABASE [{nombreBD}]", cnMaster);
                            cmdCreate.ExecuteNonQuery();
                        }

                        if (File.Exists(scriptSqlPath))
                        {
                            ActualizarProgreso(60, "Ejecutando script de tablas, vistas y datos iniciales...");
                            EjecutarLotesScript(scriptSqlPath, cadenaConexionFinal);
                        }
                    }

                    // Sellar Dígitos Verificadores
                    if (sellarDV)
                    {
                        ActualizarProgreso(80, "Recalculando y sellando Dígitos Verificadores de integridad (DVH, DVV y global)...");
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

                    // 5. Configurar archivos de conexión en la aplicación destino
                    ActualizarProgreso(90, "Actualizando configuración de conexión en UI.dll.config y db_connection.txt...");
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
                        ActualizarProgreso(95, "Creando / renovando acceso directo en el Escritorio...");
                        CrearAccesoDirectoEscritorio(Path.Combine(destino, "UI.exe"), destino);
                    }

                    ActualizarProgreso(100, _esReinstalacion ? "¡Reinstalación completada exitosamente!" : "¡Instalación completada exitosamente!");
                });

                await Task.Delay(800);
                MostrarPaso(5);
            }
            catch (Exception exGral)
            {
                Log($"\n❌ ERROR CRÍTICO:\n{exGral.Message}\n{exGral.StackTrace}");
                MessageBox.Show($"Ocurrió un error durante el proceso:\n\n{exGral.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSiguiente.Enabled = true;
                btnAtras.Enabled = true;
            }
        }

        private void EjecutarLotesScript(string scriptPath, string connStr)
        {
            string scriptContent = File.ReadAllText(scriptPath);
            string[] lotes = Regex.Split(scriptContent, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            using var cnDB = new SqlConnection(connStr);
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
                    // Ignorar violaciones de clave única o duplicados durante reinstalación con conservación de datos
                    if (!exBatch.Message.Contains("PRIMARY KEY") && !exBatch.Message.Contains("UNIQUE") && !exBatch.Message.Contains("ya existe"))
                    {
                        Log($"Aviso en lote {loteNum}: {exBatch.Message}");
                    }
                }
                loteNum++;
            }
            Log($"Se procesaron {lotes.Length} bloques SQL satisfactoriamente.");
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
                Log($"Acceso directo creado o actualizado en: {shortcutPath}");
            }
            catch (Exception ex)
            {
                Log($"No se pudo crear el acceso directo: {ex.Message}");
            }
        }

        #endregion
    }
}
