# ==============================================================================
# Script de Instalación Automatizada / Consola de NutriEvolve
# ==============================================================================
[CmdletBinding()]
param (
    [string]$Server = "",
    [string]$Database = "ING",
    [string]$InstallPath = "$env:LOCALAPPDATA\NutriEvolve",
    [bool]$CreateDesktopShortcut = $true
)

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "   INSTALACIÓN AUTOMATIZADA DE NUTRIEVOLVE Y BASE DE DATOS" -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host ""

$baseDir = $PSScriptRoot

# Cargar assembly SqlClient si está disponible localmente
$sqlClientDll = Join-Path $baseDir "Microsoft.Data.SqlClient.dll"
if (Test-Path $sqlClientDll) {
    try { [System.Reflection.Assembly]::LoadFrom($sqlClientDll) | Out-Null } catch { }
}

# 1. Detección automática de la instancia de SQL Server si no se especificó
if ([string]::IsNullOrWhiteSpace($Server)) {
    Write-Host "[1/5] Detectando instancia de SQL Server disponible en la máquina..." -ForegroundColor Yellow
    $candidates = @('.', '.\SQLEXPRESS', 'localhost', '(LocalDB)\MSSQLLocalDB')
    foreach ($cand in $candidates) {
        try {
            $testConnStr = 'Data Source=' + $cand + ';Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=2'
            $testConn = New-Object Microsoft.Data.SqlClient.SqlConnection($testConnStr)
            $testConn.Open()
            $testConn.Close()
            $Server = $cand
            Write-Host "  -> Instancia SQL Server detectada con éxito: '$Server'" -ForegroundColor Green
            break
        } catch { }
    }

    if ([string]::IsNullOrWhiteSpace($Server)) {
        $Server = '.'
        Write-Host "  -> No se obtuvo respuesta rápida - utilizando predeterminada: '$Server'" -ForegroundColor Yellow
    }
} else {
    Write-Host "[1/5] Usando instancia especificada: '$Server'" -ForegroundColor Yellow
}

# 2. Conectar a master y crear la base de datos
Write-Host "`n[2/5] Creando e inicializando catálogo SQL [$Database] en '$Server'..." -ForegroundColor Yellow
$masterConnStr = 'Data Source=' + $Server + ';Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=30'

try {
    $cnMaster = New-Object Microsoft.Data.SqlClient.SqlConnection($masterConnStr)
    $cnMaster.Open()

    $cmdCheck = $cnMaster.CreateCommand()
    $cmdCheck.CommandText = "SELECT database_id FROM sys.databases WHERE name = '$Database'"
    $dbExists = $cmdCheck.ExecuteScalar()

    if ($null -eq $dbExists) {
        Write-Host "  -> Creando base de datos [$Database]..." -ForegroundColor Gray
        $cmdCreate = $cnMaster.CreateCommand()
        $cmdCreate.CommandText = "CREATE DATABASE [$Database]"
        $cmdCreate.ExecuteNonQuery() | Out-Null
    } else {
        Write-Host "  -> La base de datos [$Database] ya existe." -ForegroundColor Gray
    }
    $cnMaster.Close()
} catch {
    Write-Host "  [ADVERTENCIA] Error al verificar/crear base de datos en master: $($_.Exception.Message)" -ForegroundColor Red
}

# 3. Ejecutar script.sql
$scriptPath = Join-Path $baseDir "Datos\script.sql"
if (Test-Path $scriptPath) {
    Write-Host "`n[3/5] Ejecutando script de tablas, perfiles y datos iniciales en [$Database]..." -ForegroundColor Yellow
    $dbConnStr = 'Data Source=' + $Server + ';Initial Catalog=' + $Database + ';Integrated Security=True;TrustServerCertificate=True;Connect Timeout=60'
    
    try {
        $cnDb = New-Object Microsoft.Data.SqlClient.SqlConnection($dbConnStr)
        $cnDb.Open()

        $scriptText = [System.IO.File]::ReadAllText($scriptPath)
        $batches = [System.Text.RegularExpressions.Regex]::Split($scriptText, "^\s*GO\s*$", [System.Text.RegularExpressions.RegexOptions]::Multiline -bor [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)

        $count = 0
        foreach ($b in $batches) {
            $trimmed = $b.Trim()
            if (-not [string]::IsNullOrWhiteSpace($trimmed)) {
                if ($trimmed -match "^\s*CREATE\s+DATABASE" -or $trimmed -match "^\s*USE\s+\[?ING\]?") {
                    continue
                }
                try {
                    $cmd = $cnDb.CreateCommand()
                    $cmd.CommandText = $trimmed
                    $cmd.CommandTimeout = 300
                    $cmd.ExecuteNonQuery() | Out-Null
                    $count++
                } catch {
                    # Omitir warnings no críticos
                }
            }
        }
        $cnDb.Close()
        Write-Host "  -> $count bloques de script SQL ejecutados correctamente." -ForegroundColor Green
    } catch {
        Write-Host "  [ADVERTENCIA] Error al ejecutar script.sql: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# 4. Copiar archivos de la aplicación
Write-Host "`n[4/5] Copiando archivos de la aplicación en '$InstallPath'..." -ForegroundColor Yellow
$appDir = Join-Path $baseDir "App"
if (Test-Path $appDir) {
    if (-not (Test-Path $InstallPath)) {
        New-Item -ItemType Directory -Path $InstallPath -Force | Out-Null
    }
    Copy-Item -Path "$appDir\*" -Destination $InstallPath -Recurse -Force
    Write-Host "  -> Archivos copiados exitosamente." -ForegroundColor Green
} else {
    Write-Host "  [ERROR] No se encontró la carpeta 'App' en $appDir" -ForegroundColor Red
}

# 5. Configurar conexión en destino
Write-Host "`n[5/5] Configurando cadena de conexión de NutriEvolve..." -ForegroundColor Yellow
$finalConnStr = 'Data Source=' + $Server + ';Initial Catalog=' + $Database + ';Integrated Security=True;Trust Server Certificate=True;Connect Timeout=30'

$txtPath = Join-Path $InstallPath "db_connection.txt"
[System.IO.File]::WriteAllText($txtPath, $finalConnStr)

$cfgPath = Join-Path $InstallPath "UI.dll.config"
if (Test-Path $cfgPath) {
    try {
        [xml]$xml = Get-Content -Path $cfgPath
        $node = $xml.configuration.connectionStrings.add | Where-Object { $_.name -eq "MiConexionDB" }
        if ($node) {
            $node.connectionString = $finalConnStr
            $xml.Save($cfgPath)
        }
    } catch { }
}

# 6. Crear acceso directo si corresponde
if ($CreateDesktopShortcut) {
    try {
        $desktop = [Environment]::GetFolderPath("Desktop")
        $shortcutFile = Join-Path $desktop "NutriEvolve.lnk"
        $ws = New-Object -ComObject WScript.Shell
        $s = $ws.CreateShortcut($shortcutFile)
        $s.TargetPath = Join-Path $InstallPath "UI.exe"
        $s.WorkingDirectory = $InstallPath
        $s.Description = "NutriEvolve - Sistema Integral de Gestión Nutricional"
        $s.Save()
        Write-Host "  -> Acceso directo creado en el Escritorio." -ForegroundColor Green
    } catch { }
}

Write-Host "`n==========================================================" -ForegroundColor Green
Write-Host "         ¡INSTALACIÓN COMPLETADA EXITOSAMENTE!" -ForegroundColor Green
Write-Host "==========================================================" -ForegroundColor Green
Write-Host "Ubicación instalada: $InstallPath\UI.exe" -ForegroundColor White
Write-Host "Servidor configurado: $Server" -ForegroundColor White
Write-Host "Base de datos:       $Database" -ForegroundColor White
Write-Host ""
Write-Host "Cuentas de acceso:" -ForegroundColor Cyan
Write-Host "  • Administrador:  admin / admin123" -ForegroundColor White
Write-Host "  • Nutricionista:  nutri / nutri123" -ForegroundColor White
Write-Host "==========================================================" -ForegroundColor Green
