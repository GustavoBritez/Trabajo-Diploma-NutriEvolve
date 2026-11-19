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
                string tempDir = Path.Combine(Path.GetTempPath(), "UniversalInstallerTemp", $"proj_{Guid.NewGuid():N}");
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

            // Helper para comprobar si una ruta pertenece a herramientas auxiliares/generador/scratch
            bool EsRutaIgnorada(string fullPath)
            {
                string fileName = Path.GetFileName(fullPath).ToLowerInvariant();
                if (fileName == "generador.csproj" ||
                    fileName == "universalsetup.csproj" ||
                    fileName == "setupapp.csproj" ||
                    fileName == "capturasapp.csproj" ||
                    fileName == "ocr_app.csproj" ||
                    fileName == "ea_manager.csproj" ||
                    fileName == "testpass.csproj")
                {
                    return true;
                }

                string rel = Path.GetRelativePath(workDir, fullPath);
                var parts = rel.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
                return parts.Any(part =>
                    part.Equals("GeneradorInstalador", StringComparison.OrdinalIgnoreCase) ||
                    part.Equals("Generador", StringComparison.OrdinalIgnoreCase) ||
                    part.Equals("UniversalSetup", StringComparison.OrdinalIgnoreCase) ||
                    part.Equals("Template", StringComparison.OrdinalIgnoreCase) ||
                    part.Equals("scratch", StringComparison.OrdinalIgnoreCase) ||
                    part.Equals("Instalador", StringComparison.OrdinalIgnoreCase) ||
                    part.Equals("bin", StringComparison.OrdinalIgnoreCase) ||
                    part.Equals("obj", StringComparison.OrdinalIgnoreCase) ||
                    part.Equals(".vs", StringComparison.OrdinalIgnoreCase));
            }

            // 1. Buscar proyectos .csproj
            var allCsprojFiles = Directory.GetFiles(workDir, "*.csproj", SearchOption.AllDirectories)
                .Where(p => !EsRutaIgnorada(p))
                .ToList();

            // Si hay proyectos fuera de la carpeta 'Patrones', priorizarlos y descartar ejercicios auxiliares de Patrones
            var nonPatronesProjects = allCsprojFiles.Where(p =>
            {
                string rel = Path.GetRelativePath(workDir, p);
                var parts = rel.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
                return !parts.Any(part => part.Equals("Patrones", StringComparison.OrdinalIgnoreCase));
            }).ToList();

            var csprojFiles = nonPatronesProjects.Count > 0 ? nonPatronesProjects : allCsprojFiles;

            manifest.DetectedProjects = csprojFiles;
            logger?.Invoke($"[ANALIZADOR] Se encontraron {csprojFiles.Count} proyectos .csproj.");

            // 2. Detectar proyecto principal ejecutable
            string? startupProject = null;

            // Prioridad 1: Por nombre convencional
            startupProject = csprojFiles.FirstOrDefault(p =>
            {
                string name = Path.GetFileNameWithoutExtension(p).ToLowerInvariant();
                return name == "ui" || name == "gui" || name == "presentacion" || name == "vistas" || name == "frontend" || name == "main";
            });

            // Prioridad 2: Si no se encuentra por nombre, buscar por <OutputType>WinExe
            if (startupProject == null)
            {
                startupProject = csprojFiles.FirstOrDefault(p =>
                {
                    try
                    {
                        string content = File.ReadAllText(p);
                        return content.Contains("<OutputType>WinExe</OutputType>", StringComparison.OrdinalIgnoreCase);
                    }
                    catch { return false; }
                });
            }

            // Prioridad 3: Buscar por <OutputType>Exe
            if (startupProject == null)
            {
                startupProject = csprojFiles.FirstOrDefault(p =>
                {
                    try
                    {
                        string content = File.ReadAllText(p);
                        return content.Contains("<OutputType>Exe</OutputType>", StringComparison.OrdinalIgnoreCase);
                    }
                    catch { return false; }
                });
            }

            // Prioridad 4: Si aún no se encuentra, usar el primer proyecto
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
                logger?.Invoke($"[ANALIZADOR] Proyecto ejecutable detectado: {projName} ({manifest.ExeName})");
            }

            // 3. Buscar solución (.sln) vinculada al proyecto principal
            var allSlnFiles = Directory.GetFiles(workDir, "*.sln", SearchOption.AllDirectories)
                .Where(s => !EsRutaIgnorada(s))
                .ToList();

            var nonPatronesSln = allSlnFiles.Where(s =>
            {
                string rel = Path.GetRelativePath(workDir, s);
                var parts = rel.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
                return !parts.Any(part => part.Equals("Patrones", StringComparison.OrdinalIgnoreCase));
            }).ToList();

            var validSlnFiles = nonPatronesSln.Count > 0 ? nonPatronesSln : allSlnFiles;

            string? chosenSln = null;
            if (startupProject != null)
            {
                string startupFileName = Path.GetFileName(startupProject);
                chosenSln = validSlnFiles.FirstOrDefault(s =>
                {
                    try
                    {
                        return File.ReadAllText(s).Contains(startupFileName, StringComparison.OrdinalIgnoreCase);
                    }
                    catch { return false; }
                });
            }

            if (chosenSln == null && validSlnFiles.Count > 0)
            {
                chosenSln = validSlnFiles[0];
            }

            if (chosenSln != null)
            {
                manifest.SolutionPath = chosenSln;
                logger?.Invoke($"[ANALIZADOR] Solución detectada: {Path.GetFileName(manifest.SolutionPath)}");
            }

            // 4. Deducir nombre amigable para la aplicación
            string appNameCandidate = "";
            if (!string.IsNullOrWhiteSpace(manifest.SolutionPath))
            {
                string slnName = Path.GetFileNameWithoutExtension(manifest.SolutionPath);
                if (!slnName.Equals("UI", StringComparison.OrdinalIgnoreCase) &&
                    !slnName.Equals("GUI", StringComparison.OrdinalIgnoreCase) &&
                    !slnName.Equals("Consola", StringComparison.OrdinalIgnoreCase) &&
                    !slnName.Equals("Console", StringComparison.OrdinalIgnoreCase))
                {
                    appNameCandidate = slnName;
                }
            }

            if (string.IsNullOrWhiteSpace(appNameCandidate))
            {
                string rawInputName = Path.GetFileNameWithoutExtension(inputPath);
                rawInputName = Regex.Replace(rawInputName, @"-(main|master|dev|release)$", "", RegexOptions.IgnoreCase);
                if (!string.IsNullOrWhiteSpace(rawInputName) &&
                    !rawInputName.Contains("Generador", StringComparison.OrdinalIgnoreCase))
                {
                    appNameCandidate = rawInputName;
                }
            }

            if ((string.IsNullOrWhiteSpace(appNameCandidate) || appNameCandidate.Equals("UI", StringComparison.OrdinalIgnoreCase)) && startupProject != null)
            {
                var dir = Directory.GetParent(startupProject);
                while (dir != null && !dir.FullName.Equals(workDir, StringComparison.OrdinalIgnoreCase))
                {
                    string dirName = dir.Name;
                    if (!dirName.Equals("UI", StringComparison.OrdinalIgnoreCase) &&
                        !dirName.Equals("bin", StringComparison.OrdinalIgnoreCase) &&
                        !dirName.Equals("obj", StringComparison.OrdinalIgnoreCase) &&
                        !dirName.StartsWith("proj_", StringComparison.OrdinalIgnoreCase))
                    {
                        appNameCandidate = dirName;
                        break;
                    }
                    dir = dir.Parent;
                }
            }

            if (string.IsNullOrWhiteSpace(appNameCandidate))
            {
                appNameCandidate = startupProject != null ? Path.GetFileNameWithoutExtension(startupProject) : "MiProyecto";
            }

            manifest.AppName = appNameCandidate;
            logger?.Invoke($"[ANALIZADOR] Nombre de aplicación detectado: {manifest.AppName}");

            // 5. Detectar capas de la arquitectura
            foreach (var proj in csprojFiles)
            {
                string name = Path.GetFileNameWithoutExtension(proj).ToUpperInvariant();
                if (name.Contains("DAL") || name.Contains("DATOS")) manifest.HasDal = true;
                if (name.Contains("BE") || name.Contains("ENTIDADES")) manifest.HasBe = true;
                if (name.Contains("BLL") || name.Contains("NEGOCIO")) manifest.HasBll = true;
                if (name.Contains("SERVICES") || name.Contains("SERVICIOS")) manifest.HasServices = true;
            }

            // 6. Detectar Dígitos Verificadores
            var csFiles = Directory.GetFiles(workDir, "*.cs", SearchOption.AllDirectories)
                .Where(f => !EsRutaIgnorada(f))
                .ToList();

            manifest.HasDigitoVerificador = csFiles.Any(f =>
            {
                string fn = Path.GetFileName(f).ToLowerInvariant();
                return fn.Contains("digitoverificador") || fn.Contains("dv_") || fn.Contains("verificador");
            });

            if (manifest.HasDigitoVerificador)
            {
                logger?.Invoke("[ANALIZADOR] Módulo de Dígitos Verificadores (DV) detectado.");
            }

            // 7. Buscar archivos SQL (.sql)
            var sqlFiles = Directory.GetFiles(workDir, "*.sql", SearchOption.AllDirectories)
                .Where(s => !EsRutaIgnorada(s))
                .ToList();

            manifest.DetectedSqlFiles = sqlFiles;
            if (sqlFiles.Count > 0)
            {
                // Preferir "script.sql" en la carpeta del proyecto o el más grande
                var preferredSql = sqlFiles.FirstOrDefault(s =>
                {
                    string fn = Path.GetFileName(s).ToLowerInvariant();
                    string rel = Path.GetRelativePath(workDir, s).ToLowerInvariant();
                    return fn == "script.sql" && (rel.Contains("ing-software") || rel.Contains("database") || rel.Contains("datos") || rel.Contains("ui"));
                })
                ?? sqlFiles.FirstOrDefault(s => Path.GetFileName(s).Equals("script.sql", StringComparison.OrdinalIgnoreCase))
                ?? sqlFiles.OrderByDescending(s => new FileInfo(s).Length).First();

                manifest.SelectedSqlScriptPath = preferredSql;
                logger?.Invoke($"[ANALIZADOR] Script SQL detectado: {Path.GetFileName(preferredSql)} ({Path.GetRelativePath(workDir, preferredSql)})");

                // Intentar deducir nombre de base de datos desde el script SQL
                manifest.DatabaseName = DetectDatabaseNameFromSql(preferredSql);
                logger?.Invoke($"[ANALIZADOR] Base de Datos deducida del script: '{manifest.DatabaseName}'");
            }
            else
            {
                manifest.DatabaseName = string.IsNullOrWhiteSpace(manifest.AppName) ? "MiBaseDatos" : $"{manifest.AppName}DB";
            }

            // 8. Ruta de salida por defecto
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string safeAppName = string.IsNullOrWhiteSpace(manifest.AppName) ? "Proyecto" : manifest.AppName;
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                safeAppName = safeAppName.Replace(c, '_');
            }
            manifest.OutputPath = Path.Combine(desktop, "Instaladores", $"{safeAppName}_Instalador");

            ConnectionInjector.Inject(manifest);

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
