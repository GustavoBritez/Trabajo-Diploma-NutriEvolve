using System;
using System.IO;
using System.Text.Json;

namespace UniversalSetup
{
    public class SetupManifest
    {
        public string AppName { get; set; } = "MiAplicacion";
        public string AppDescription { get; set; } = "Sistema de Gestión .NET";
        public string ExeName { get; set; } = "UI.exe";
        public string TargetProcessName { get; set; } = "UI";
        public string DatabaseName { get; set; } = "MiBaseDatos";
        public string SqlScriptFile { get; set; } = "Datos/script.sql";
        public string DefaultInstallSubfolder { get; set; } = "MiAplicacion";
        public bool HasDigitoVerificador { get; set; } = false;
        public string PrimaryColorHex { get; set; } = "#1E3A8A";
        public string AccentColorHex { get; set; } = "#2563EB";
        public string Version { get; set; } = "1.0.0";
        public string Author { get; set; } = "Universidad / Alumno";

        public static SetupManifest Load()
        {
            try
            {
                string manifestPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "manifest.json");
                if (File.Exists(manifestPath))
                {
                    string json = File.ReadAllText(manifestPath);
                    var manifest = JsonSerializer.Deserialize<SetupManifest>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (manifest != null) return manifest;
                }
            }
            catch
            {
                // Fallback to default
            }

            return new SetupManifest();
        }

        public void Save(string path)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(path, json);
        }
    }
}
