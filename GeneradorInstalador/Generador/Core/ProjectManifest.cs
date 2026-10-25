using System;
using System.Collections.Generic;

namespace GeneradorInstalador.Core
{
    public class ProjectManifest
    {
        public string SourceInputPath { get; set; } = "";
        public bool IsZip { get; set; } = false;
        public string ExtractedWorkingDir { get; set; } = "";

        public string SolutionPath { get; set; } = "";
        public List<string> DetectedProjects { get; set; } = new();
        public string StartupProjectPath { get; set; } = "";

        public string AppName { get; set; } = "";
        public string AppDescription { get; set; } = "Sistema de Gestión .NET";
        public string ExeName { get; set; } = "UI.exe";
        public string TargetProcessName { get; set; } = "UI";

        public List<string> DetectedSqlFiles { get; set; } = new();
        public string SelectedSqlScriptPath { get; set; } = "";
        public string DatabaseName { get; set; } = "MiBaseDatos";

        public bool HasDal { get; set; } = false;
        public bool HasBe { get; set; } = false;
        public bool HasBll { get; set; } = false;
        public bool HasServices { get; set; } = false;
        public bool HasDigitoVerificador { get; set; } = false;

        public bool RecompileWithDotnet { get; set; } = true;
        public bool CreateZipPackage { get; set; } = true;
        public string OutputPath { get; set; } = "";
        public string PrimaryColorHex { get; set; } = "#1E3A8A";
    }
}
