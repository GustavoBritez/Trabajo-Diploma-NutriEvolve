using System;
using System.Diagnostics;
using System.IO;

namespace GeneradorInstalador.Core
{
    public static class ProjectPublisher
    {
        public static bool Publish(ProjectManifest manifest, string outputAppDir, Action<string>? logger = null)
        {
            if (string.IsNullOrWhiteSpace(manifest.StartupProjectPath) || !File.Exists(manifest.StartupProjectPath))
            {
                throw new FileNotFoundException("No se especificó un proyecto de inicio válido para compilar.");
            }

            if (!Directory.Exists(outputAppDir))
            {
                Directory.CreateDirectory(outputAppDir);
            }

            logger?.Invoke($"[COMPILADOR] Compilando y publicando {Path.GetFileName(manifest.StartupProjectPath)} con dotnet publish (Release)...");

            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"publish \"{manifest.StartupProjectPath}\" -c Release -o \"{outputAppDir}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = psi };
            process.OutputDataReceived += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                {
                    logger?.Invoke($"[DOTNET] {e.Data}");
                }
            };
            process.ErrorDataReceived += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                {
                    logger?.Invoke($"[DOTNET ERROR] {e.Data}");
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                logger?.Invoke($"[COMPILADOR ERROR] dotnet publish finalizó con código de error {process.ExitCode}.");
                return false;
            }

            // Validar que se generó el ejecutable o librerías
            string targetExe = Path.Combine(outputAppDir, manifest.ExeName);
            if (!File.Exists(targetExe))
            {
                // Buscar cualquier .exe en el directorio
                var exes = Directory.GetFiles(outputAppDir, "*.exe");
                if (exes.Length > 0)
                {
                    manifest.ExeName = Path.GetFileName(exes[0]);
                    manifest.TargetProcessName = Path.GetFileNameWithoutExtension(exes[0]);
                    logger?.Invoke($"[COMPILADOR] Ejecutable detectado en salida: {manifest.ExeName}");
                }
                else
                {
                    logger?.Invoke("[COMPILADOR ADVERTENCIA] No se encontró archivo .exe en la salida de dotnet publish.");
                }
            }
            else
            {
                logger?.Invoke($"[COMPILADOR] ¡Publicación exitosa! Ejecutable listo: {manifest.ExeName}");
            }

            return true;
        }
    }
}
