using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;

namespace GeneradorInstalador.Core
{
    public static class ProjectAnalyzer
    {
        public static ProjectManifest Analyze(string inputPath, Action<string>? logger = null)
        {
            var manifest = new ProjectManifest
            {
                SourceInputPath = inputPath
            };

            string workDir = inputPath;
            if (File.Exists(inputPath) && Path.GetExtension(inputPath).Equals(".zip", StringComparison.OrdinalIgnoreCase))
            {
                manifest.IsZip = true;
                string tempDir = Path.Combine(Path.GetTempPath(), "GeneradorInstalador", $"proj_{Guid.NewGuid():N}");
                Directory.CreateDirectory(tempDir);
                logger?.Invoke($"[ANALIZADOR] Descomprimiendo archivo zip en carpeta temporal: {tempDir}...");
                ZipFile.ExtractToDirectory(inputPath, tempDir, true);
                workDir = tempDir;
                manifest.ExtractedWorkingDir = tempDir;
            }
            else if (!Directory.Exists(inputPath))
            {
                throw new DirectoryNotFoundException($"No se encontró la ruta de entrada: {inputPath}");
            }

            logger?.Invoke("[ANALIZADOR] Analizando estructura del proyecto .NET...");

            // 1. Buscar solución (.sln)
            var slnFiles = Directory.GetFiles(workDir, "*.sln", SearchOption.AllDirectories);
            if (slnFiles.Length > 0)
            {
                manifest.SolutionPath = slnFiles[0];
                manifest.AppName = Path.GetFileNameWithoutExtension(slnFiles[0]);
                logger?.Invoke($"[ANALIZADOR] Solución detectada: {Path.GetFileName(manifest.SolutionPath)}");
            }

            // 2. Buscar proyectos .csproj
            var csprojFiles = Directory.GetFiles(workDir, "*.csproj", SearchOption.AllDirectories)
                .Where(p => !p.Contains("SetupApp") && !p.Contains("UniversalSetup") && !p.Contains("Generador"))
                .ToList();

            manifest.DetectedProjects = csprojFiles;
            logger?.Invoke($"[ANALIZADOR] Se encontraron {csprojFiles.Count} proyectos .csproj.");

            // 3. Detectar proyecto principal ejecutable
            string? startupProject = null;

            // Prioridad por nombre convencional
            startupProject = csprojFiles.FirstOrDefault(p =>
            {
                string name = Path.GetFileNameWithoutExtension(p).ToLowerInvariant();
                return name == "ui" || name == "gui" || name == "presentacion" || name == "vistas" || name == "frontend";
            });

            // Si no se encuentra por nombre, buscar por <OutputType>WinExe o <OutputType>Exe
            if (startupProject == null)
            {
                foreach (var proj in csprojFiles)
                {
                    try
                    {
                        string content = File.ReadAllText(proj);
                        if (content.Contains("<OutputType>WinExe</OutputType>", StringComparison.OrdinalIgnoreCase) ||
                            content.Contains("<OutputType>Exe</OutputType>", StringComparison.OrdinalIgnoreCase))
                        {
                            startupProject = proj;
                            break;
                        }
                    }
                    catch { }
                }
            }

            // Si aún no se encuentra, usar el primer proyecto
            if (startupProject == null && csprojFiles.Count > 0)
            {
                startupProject = csprojFiles[0];
            }

            if (startupProject != null)
            {
                manifest.StartupProjectPath = startupProject;
                string projName = Path.GetFileNameWithoutExtension(startupProject);
                manifest.ExeName = $"{projName}.exe";
                manifest.TargetProcessName = projName;
                if (string.IsNullOrWhiteSpace(manifest.AppName))
                {
                    manifest.AppName = projName;
                }
                logger?.Invoke($"[ANALIZADOR] Proyecto ejecutable detectado: {projName} ({manifest.ExeName})");
            }

            // 4. Detectar capas de la arquitectura
            foreach (var proj in csprojFiles)
            {
                string name = Path.GetFileNameWithoutExtension(proj).ToUpperInvariant();
                if (name.Contains("DAL") || name.Contains("DATOS")) manifest.HasDal = true;
                if (name.Contains("BE") || name.Contains("ENTIDADES")) manifest.HasBe = true;
                if (name.Contains("BLL") || name.Contains("NEGOCIO")) manifest.HasBll = true;
                if (name.Contains("SERVICES") || name.Contains("SERVICIOS")) manifest.HasServices = true;
            }

            // 5. Detectar Dígitos Verificadores
            var csFiles = Directory.GetFiles(workDir, "*.cs", SearchOption.AllDirectories);
            manifest.HasDigitoVerificador = csFiles.Any(f =>
            {
                string fn = Path.GetFileName(f).ToLowerInvariant();
                return fn.Contains("digitoverificador") || fn.Contains("dv_") || fn.Contains("verificador");
            });

            if (manifest.HasDigitoVerificador)
            {
                logger?.Invoke("[ANALIZADOR] Módulo de Dígitos Verificadores (DV) detectado.");
            }

            // 6. Buscar archivos SQL (.sql)
            var sqlFiles = Directory.GetFiles(workDir, "*.sql", SearchOption.AllDirectories)
                .Where(s => !s.Contains("bin") && !s.Contains("obj"))
                .ToList();

            manifest.DetectedSqlFiles = sqlFiles;
            if (sqlFiles.Count > 0)
            {
                // Preferir "script.sql", "database.sql" o el más grande
                var preferredSql = sqlFiles.FirstOrDefault(s => Path.GetFileName(s).Equals("script.sql", StringComparison.OrdinalIgnoreCase))
                                ?? sqlFiles.OrderByDescending(s => new FileInfo(s).Length).First();

                manifest.SelectedSqlScriptPath = preferredSql;
                logger?.Invoke($"[ANALIZADOR] Script SQL detectado: {Path.GetFileName(preferredSql)}");

                // Intentar deducir nombre de base de datos desde el script SQL
                manifest.DatabaseName = DetectDatabaseNameFromSql(preferredSql);
                logger?.Invoke($"[ANALIZADOR] Base de Datos deducida del script: '{manifest.DatabaseName}'");
            }
            else
            {
                manifest.DatabaseName = string.IsNullOrWhiteSpace(manifest.AppName) ? "MiBaseDatos" : $"{manifest.AppName}DB";
            }

            // 7. Ruta de salida por defecto
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string safeAppName = string.IsNullOrWhiteSpace(manifest.AppName) ? "Proyecto" : manifest.AppName;
            manifest.OutputPath = Path.Combine(desktop, "Instaladores", $"{safeAppName}_Instalador");

            return manifest;
        }

        private static string DetectDatabaseNameFromSql(string sqlPath)
        {
            try
            {
                string content = File.ReadAllText(sqlPath);

                // Buscar: CREATE DATABASE [Nombre] o CREATE DATABASE Nombre
                var matchCreate = Regex.Match(content, @"CREATE\s+DATABASE\s+\[?([a-zA-Z0-9_]+)\]?", RegexOptions.IgnoreCase);
                if (matchCreate.Success && matchCreate.Groups[1].Value.Length > 0)
                {
                    string candidate = matchCreate.Groups[1].Value.Trim();
                    if (!candidate.Equals("master", StringComparison.OrdinalIgnoreCase) &&
                        !candidate.Equals("tempdb", StringComparison.OrdinalIgnoreCase))
                    {
                        return candidate;
                    }
                }

                // Buscar: USE [Nombre] o USE Nombre
                var matchUse = Regex.Match(content, @"USE\s+\[?([a-zA-Z0-9_]+)\]?", RegexOptions.IgnoreCase);
                if (matchUse.Success && matchUse.Groups[1].Value.Length > 0)
                {
                    string candidate = matchUse.Groups[1].Value.Trim();
                    if (!candidate.Equals("master", StringComparison.OrdinalIgnoreCase) &&
                        !candidate.Equals("tempdb", StringComparison.OrdinalIgnoreCase))
                    {
                        return candidate;
                    }
                }
            }
            catch { }

            return "MiBaseDatos";
        }
    }
}
