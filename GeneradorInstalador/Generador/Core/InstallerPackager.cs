using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;

namespace GeneradorInstalador.Core
{
    public static class InstallerPackager
    {
        public static bool Package(ProjectManifest manifest, string appSourceDir, Action<string>? logger = null)
        {
            string outDir = manifest.OutputPath;
            logger?.Invoke($"[EMPAQUETADOR] Creando estructura del instalador en: {outDir}...");

            if (!Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            // 1. Localizar y copiar archivos de la plantilla UniversalSetup
            string templateDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template");
            if (!Directory.Exists(templateDir))
            {
                // Buscar relativo en desarrollo
                string devTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Template");
                if (Directory.Exists(devTemplate))
                {
                    templateDir = Path.GetFullPath(devTemplate);
                }
            }

            if (!Directory.Exists(templateDir))
            {
                throw new DirectoryNotFoundException($"No se encontró la carpeta con la plantilla del instalador en: {templateDir}");
            }

            logger?.Invoke("[EMPAQUETADOR] Copiando motor de instalación inteligente (Setup.exe)...");
            CopiarDirectorio(templateDir, outDir, true);

            // 2. Crear carpeta App/ y copiar aplicación publicada
            string destAppDir = Path.Combine(outDir, "App");
            if (!Directory.Exists(destAppDir)) Directory.CreateDirectory(destAppDir);

            logger?.Invoke("[EMPAQUETADOR] Copiando binarios de la aplicación en App/...");
            CopiarDirectorio(appSourceDir, destAppDir, true);

            // 3. Crear carpeta Datos/ y copiar script SQL
            string destDatosDir = Path.Combine(outDir, "Datos");
            if (!Directory.Exists(destDatosDir)) Directory.CreateDirectory(destDatosDir);

            string scriptFileName = "script.sql";
            if (!string.IsNullOrWhiteSpace(manifest.SelectedSqlScriptPath) && File.Exists(manifest.SelectedSqlScriptPath))
            {
                logger?.Invoke($"[EMPAQUETADOR] Copiando script SQL maestro ({Path.GetFileName(manifest.SelectedSqlScriptPath)})...");
                File.Copy(manifest.SelectedSqlScriptPath, Path.Combine(destDatosDir, scriptFileName), true);
            }
            else
            {
                logger?.Invoke("[EMPAQUETADOR ADVERTENCIA] No se especificó script SQL maestro.");
            }

            // 4. Generar manifest.json
            logger?.Invoke("[EMPAQUETADOR] Generando manifest.json con metadatos del proyecto...");
            var setupManifest = new
            {
                appName = manifest.AppName,
                appDescription = manifest.AppDescription,
                exeName = manifest.ExeName,
                targetProcessName = manifest.TargetProcessName,
                databaseName = manifest.DatabaseName,
                sqlScriptFile = $"Datos/{scriptFileName}",
                defaultInstallSubfolder = manifest.AppName,
                hasDigitoVerificador = manifest.HasDigitoVerificador,
                primaryColorHex = manifest.PrimaryColorHex,
                version = "1.0.0",
                author = "Alumno / Cátedra de Ingeniería de Software"
            };

            string manifestJson = JsonSerializer.Serialize(setupManifest, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path.Combine(outDir, "manifest.json"), manifestJson);

            // 5. Generar Instalar.bat
            string batContent = $@"@echo off
chcp 65001 > nul
title Instalador / Reinstalador de {manifest.AppName}

echo ==========================================================
echo        INSTALADOR DE {manifest.AppName.ToUpper()}
echo ==========================================================
echo.
echo Iniciando asistente de instalacion y reinstalacion...
echo.

cd /d ""%~dp0""

if exist ""Setup.exe"" (
    start """" ""Setup.exe""
) else (
    echo [ERROR] No se encontro Setup.exe en este directorio.
    pause
)
";
            File.WriteAllText(Path.Combine(outDir, "Instalar.bat"), batContent);

            // 6. Generar Instalar_Consola.ps1
            string ps1Content = GenerarScriptPowerShell(manifest, scriptFileName);
            File.WriteAllText(Path.Combine(outDir, "Instalar_Consola.ps1"), ps1Content);

            // 7. Generar INSTRUCCIONES_INSTALACION.md
            string mdContent = GenerarManualInstalacion(manifest, scriptFileName);
            File.WriteAllText(Path.Combine(outDir, "INSTRUCCIONES_INSTALACION.md"), mdContent);

            // 8. Generar paquete .zip si está habilitado
            if (manifest.CreateZipPackage)
            {
                string zipPath = $"{outDir.TrimEnd('\\', '/')}.zip";
                if (File.Exists(zipPath)) File.Delete(zipPath);

                logger?.Invoke($"[EMPAQUETADOR] Comprimiendo paquete completo en archivo ZIP: {Path.GetFileName(zipPath)}...");
                ZipFile.CreateFromDirectory(outDir, zipPath, CompressionLevel.Optimal, false);
                logger?.Invoke($"[EMPAQUETADOR] ¡Archivo ZIP generado con éxito! ({new FileInfo(zipPath).Length / 1024 / 1024} MB)");
            }

            logger?.Invoke("[EMPAQUETADOR] ¡Paquete de instalación y reinstalación generado exitosamente!");
            return true;
        }

        private static void CopiarDirectorio(string sourceDir, string targetDir, bool overwrite)
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

        private static string GenerarScriptPowerShell(ProjectManifest manifest, string scriptFileName)
        {
            return $@"# ==============================================================================
# Script de Instalación y Reinstalación Silenciosa / Consola: {manifest.AppName}
# ==============================================================================

param(
    [string]$Server = """",
    [string]$Database = ""{manifest.DatabaseName}"",
    [string]$InstallPath = ""$env:LOCALAPPDATA\{manifest.AppName}"",
    [switch]$RecreateDatabase = $false
)

$ErrorActionPreference = ""Stop""
Write-Host ""========================================================="" -ForegroundColor Cyan
Write-Host ""   INSTALADOR / REINSTALADOR: {manifest.AppName.ToUpper()}"" -ForegroundColor Cyan
Write-Host ""========================================================="" -ForegroundColor Cyan

# 1. Detener procesos en ejecución
$procName = ""{manifest.TargetProcessName}""
$activeProcs = Get-Process -Name $procName -ErrorAction SilentlyContinue
if ($activeProcs) {{
    Write-Host ""Deteniendo proceso activo '$procName'..."" -ForegroundColor Yellow
    Stop-Process -Name $procName -Force
    Start-Sleep -Seconds 1
}}

# 2. Detección de Instancia SQL Server
if (-not $Server) {{
    $candidates = @("".\"", "".\SQLEXPRESS\"", ""localhost\"", ""(LocalDB)\MSSQLLocalDB\"")
    foreach ($cand in $candidates) {{
        try {{
            $testConn = 'Data Source=' + $cand + ';Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=3;'
            $sqlConn = New-Object Microsoft.Data.SqlClient.SqlConnection($testConn)
            $sqlConn.Open()
            $sqlConn.Close()
            $Server = $cand
            Write-Host ""Servidor SQL detectado automaticamente: $Server"" -ForegroundColor Green
            break
        }} catch {{}}
    }}
    if (-not $Server) {{ $Server = ""."" }}
}}

# 3. Copiar Archivos
Write-Host ""Copiando archivos en $InstallPath..."" -ForegroundColor Cyan
if (-not (Test-Path $InstallPath)) {{ New-Item -ItemType Directory -Path $InstallPath -Force | Out-Null }}
Copy-Item -Path ""$PSScriptRoot\App\*"" -Destination $InstallPath -Recurse -Force

# 4. Configurar Base de Datos
$scriptFile = ""$PSScriptRoot\Datos\{scriptFileName}""
if (Test-Path $scriptFile) {{
    Write-Host ""Configurando base de datos '$Database' en '$Server'..."" -ForegroundColor Cyan
    $masterConnStr = 'Data Source=' + $Server + ';Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True;'
    $masterConn = New-Object Microsoft.Data.SqlClient.SqlConnection($masterConnStr)
    $masterConn.Open()

    if ($RecreateDatabase) {{
        $dropCmd = $masterConn.CreateCommand()
        $dropCmd.CommandText = ""IF EXISTS (SELECT name FROM sys.databases WHERE name = N'$Database') BEGIN ALTER DATABASE [$Database] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [$Database]; END""
        $dropCmd.ExecuteNonQuery() | Out-Null
    }}

    $createCmd = $masterConn.CreateCommand()
    $createCmd.CommandText = ""IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'$Database') CREATE DATABASE [$Database]""
    $createCmd.ExecuteNonQuery() | Out-Null
    $masterConn.Close()

    # Ejecutar script
    $targetConnStr = 'Data Source=' + $Server + ';Initial Catalog=' + $Database + ';Integrated Security=True;TrustServerCertificate=True;'
    $targetConn = New-Object Microsoft.Data.SqlClient.SqlConnection($targetConnStr)
    $targetConn.Open()

    $rawContent = [System.IO.File]::ReadAllText($scriptFile)
    $batches = $rawContent -split '(?m)^\s*GO\s*$'
    foreach ($batch in $batches) {{
        $b = $batch.Trim()
        if ($b) {{
            try {{
                $cmd = $targetConn.CreateCommand()
                $cmd.CommandText = $b
                $cmd.CommandTimeout = 180
                $cmd.ExecuteNonQuery() | Out-Null
            }} catch {{}}
        }}
    }}
    $targetConn.Close()
    Write-Host ""Base de datos lista."" -ForegroundColor Green
}}

# 5. Guardar configuracion de conexion
$connStrFinal = 'Data Source=' + $Server + ';Initial Catalog=' + $Database + ';Integrated Security=True;TrustServerCertificate=True;'
[System.IO.File]::WriteAllText(""""$InstallPath\db_connection.txt"""", $connStrFinal)

Write-Host ""========================================================="" -ForegroundColor Green
Write-Host ""  ¡INSTALACION / REINSTALACION COMPLETADA CON EXITO!"" -ForegroundColor Green
Write-Host ""========================================================="" -ForegroundColor Green
";
        }

        private static string GenerarManualInstalacion(ProjectManifest manifest, string scriptFileName)
        {
            return $@"# Guía de Instalación y Reinstalación: {manifest.AppName}

Este paquete contiene todo lo necesario para instalar y poner en marcha **{manifest.AppName}** junto con su base de datos SQL Server (`{manifest.DatabaseName}`).

---

## 1. Contenido del Paquete

```
{manifest.AppName}_Instalador/
├── Setup.exe                 # Asistente Gráfico de Instalación y Reinstalación (Recomendado)
├── Instalar.bat              # Lanzador rápido con un solo clic
├── Instalar_Consola.ps1      # Script de instalación automatizada por PowerShell
├── INSTRUCCIONES_INSTALACION.md
├── manifest.json             # Metadatos del proyecto
├── Datos/
│   └── {scriptFileName}         # Script maestro de base de datos SQL Server
└── App/                      # Binarios de la aplicación compilada
    ├── {manifest.ExeName}
    └── ...
```

---

## 2. Requisitos Previos

1. **Sistema Operativo:** Windows 10 o Windows 11 (64-bit).
2. **Motor SQL Server:** SQL Server Standard / Developer (`.`), SQL Server Express (`.\SQLEXPRESS`) o LocalDB (`(LocalDB)\MSSQLLocalDB`).
3. **.NET Desktop Runtime 9.0** o superior.

---

## 3. Comportamiento Inteligente: Instalador y Reinstalador

El asistente `Setup.exe` detecta automáticamente si el sistema ya fue instalado:
- **Primera vez (Instalación Nueva):** Copia los archivos, crea la base de datos `{manifest.DatabaseName}` y ejecuta el script `{scriptFileName}`.
- **Si ya está instalado (Reinstalación / Reparador):**
  - Cierra de manera segura cualquier instancia abierta de `{manifest.ExeName}`.
  - Actualiza todos los ejecutables y librerías.
  - Permite elegir entre:
    * **Conservar datos existentes:** Actualiza tablas sin borrar registros y sella la integridad.
    * **Recrear base de datos:** Reinicializa los datos de fábrica desde cero.

---

## 4. Pasos para Instalar mediante el Asistente Gráfico (`Setup.exe`)

1. Haga doble clic en `Setup.exe` (o ejecute `Instalar.bat`).
2. Compruebe la pantalla de bienvenida y presione **Siguiente**.
3. Confirme la carpeta de destino y presione **Siguiente**.
4. Verifique la instancia detectada de SQL Server con el botón **Probar Conexión**.
5. Presione **Instalar Ahora** (o **Reinstalar Ahora**).
6. Al finalizar, presione **Finalizar** para abrir `{manifest.AppName}`.
";
        }
    }
}
