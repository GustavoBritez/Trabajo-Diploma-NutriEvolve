using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GeneradorInstalador.Core
{
    public static class ConnectionInjector
    {
        public static void Inject(ProjectManifest manifest)
        {
            try
            {
                string rootDir = DetermineRootDirectory(manifest);
                if (string.IsNullOrWhiteSpace(rootDir) || !Directory.Exists(rootDir))
                {
                    return;
                }

                string targetDb = string.IsNullOrWhiteSpace(manifest.DatabaseName) ? "ING" : manifest.DatabaseName.Trim();

                var csFiles = Directory.GetFiles(rootDir, "*.cs", SearchOption.AllDirectories)
                    .Where(f => !EsRutaIgnorada(rootDir, f))
                    .ToList();

                foreach (var file in csFiles)
                {
                    string fileName = Path.GetFileName(file);
                    if (fileName.Equals("Conexion.cs", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcesarArchivoConexion(file, targetDb);
                    }
                    else if (fileName.Equals("BackupDAL.cs", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcesarArchivoBackupDal(file);
                    }
                }
            }
            catch
            {
            }
        }

        private static string DetermineRootDirectory(ProjectManifest manifest)
        {
            if (!string.IsNullOrWhiteSpace(manifest.ExtractedWorkingDir) && Directory.Exists(manifest.ExtractedWorkingDir))
            {
                return manifest.ExtractedWorkingDir;
            }

            if (!string.IsNullOrWhiteSpace(manifest.SourceInputPath) && Directory.Exists(manifest.SourceInputPath))
            {
                return manifest.SourceInputPath;
            }

            if (!string.IsNullOrWhiteSpace(manifest.SolutionPath) && File.Exists(manifest.SolutionPath))
            {
                var slnDir = Path.GetDirectoryName(manifest.SolutionPath)!;
                if (Path.GetFileName(slnDir).Equals("UI", StringComparison.OrdinalIgnoreCase) ||
                    Path.GetFileName(slnDir).Equals("GUI", StringComparison.OrdinalIgnoreCase))
                {
                    return Directory.GetParent(slnDir)?.FullName ?? slnDir;
                }
                return slnDir;
            }

            if (!string.IsNullOrWhiteSpace(manifest.StartupProjectPath) && File.Exists(manifest.StartupProjectPath))
            {
                var parent = Directory.GetParent(Path.GetDirectoryName(manifest.StartupProjectPath)!);
                return parent?.FullName ?? Path.GetDirectoryName(manifest.StartupProjectPath)!;
            }

            return "";
        }

        private static bool EsRutaIgnorada(string workDir, string fullPath)
        {
            string rel = Path.GetRelativePath(workDir, fullPath);
            var parts = rel.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Any(part =>
                part.Equals("GeneradorInstalador", StringComparison.OrdinalIgnoreCase) ||
                part.Equals("Generador", StringComparison.OrdinalIgnoreCase) ||
                part.Equals("UniversalSetup", StringComparison.OrdinalIgnoreCase) ||
                part.Equals("Template", StringComparison.OrdinalIgnoreCase) ||
                part.Equals("scratch", StringComparison.OrdinalIgnoreCase) ||
                part.Equals("Instalador", StringComparison.OrdinalIgnoreCase) ||
                part.Equals("Patrones", StringComparison.OrdinalIgnoreCase) ||
                part.Equals("bin", StringComparison.OrdinalIgnoreCase) ||
                part.Equals("obj", StringComparison.OrdinalIgnoreCase) ||
                part.Equals(".vs", StringComparison.OrdinalIgnoreCase));
        }

        private static void ProcesarArchivoConexion(string filePath, string targetDb)
        {
            string content = ReadFilePreservingEncoding(filePath);

            // Si ya cuenta con ObtenerCadenaConexion, no volver a inyectar
            if (content.Contains("ObtenerCadenaConexion()", StringComparison.Ordinal))
            {
                return;
            }

            // Buscar literales de cadena de conexión hardcodeados (excluyendo líneas comentadas)
            var connLiteralRegex = new Regex(@"(?<!//[^\r\n]*)@?""[^""\r\n]*(?:Data\s+Source|Server)\s*=[^""\r\n]*""", RegexOptions.IgnoreCase);
            if (!connLiteralRegex.IsMatch(content))
            {
                return;
            }

            // Extraer catálogo original de Conexion.cs si existía (ej. ING)
            string fallbackDb = targetDb;
            var catMatch = Regex.Match(content, @"(?:Initial\s+Catalog|Database)\s*=\s*(?!master\b)([a-zA-Z0-9_]+)", RegexOptions.IgnoreCase);
            if (catMatch.Success && !string.IsNullOrWhiteSpace(catMatch.Groups[1].Value))
            {
                fallbackDb = catMatch.Groups[1].Value.Trim();
            }

            // Asegurar directivas using necesarias
            if (!Regex.IsMatch(content, @"^\s*using\s+System\.IO\s*;", RegexOptions.Multiline))
            {
                content = "using System.IO;\r\n" + content;
            }
            if (!Regex.IsMatch(content, @"^\s*using\s+System\.Collections\.Generic\s*;", RegexOptions.Multiline))
            {
                content = "using System.Collections.Generic;\r\n" + content;
            }

            // Reemplazar cada literal de cadena de conexión por ObtenerCadenaMaster() u ObtenerCadenaConexion()
            content = connLiteralRegex.Replace(content, match =>
            {
                string literal = match.Value;
                if (Regex.IsMatch(literal, @"(?:Initial\s+Catalog|Database)\s*=\s*master\b", RegexOptions.IgnoreCase))
                {
                    return "ObtenerCadenaMaster()";
                }
                return "ObtenerCadenaConexion()";
            });

            // Reemplazar 'const string' en asignaciones a ObtenerCadenaConexion() / ObtenerCadenaMaster()
            content = Regex.Replace(
                content,
                @"\b(private|public|internal|protected)\s+const\s+string\s+(\w+\s*=\s*(?:\r?\n\s*)?ObtenerCadena(?:Conexion|Master)\(\))",
                "$1 static string $2");

            content = Regex.Replace(
                content,
                @"\bconst\s+string\s+(\w+\s*=\s*(?:\r?\n\s*)?ObtenerCadena(?:Conexion|Master)\(\))",
                "string $1");

            // Insertar los métodos de resolución dinámica dentro de class Conexion
            var classMatch = Regex.Match(content, @"\bclass\s+Conexion\b[^{]*\{");
            if (!classMatch.Success)
            {
                return;
            }

            string metodosInyectados = GenerarBloqueMetodosConexion(fallbackDb);
            int insertPos = classMatch.Index + classMatch.Length;
            content = content.Insert(insertPos, "\r\n" + metodosInyectados);

            File.WriteAllText(filePath, content, new UTF8Encoding(false));
        }

        private static void ProcesarArchivoBackupDal(string filePath)
        {
            string content = ReadFilePreservingEncoding(filePath);
            string pattern = @"(?:private\s+)?const\s+string\s+NOMBRE_BD\s*=\s*""[^""]+"";";
            if (Regex.IsMatch(content, pattern))
            {
                string replacement = "private string NOMBRE_BD => new SqlConnectionStringBuilder(Conexion.ObtenerCadenaConexion()).InitialCatalog;";
                string updated = Regex.Replace(content, pattern, replacement);
                if (updated != content)
                {
                    File.WriteAllText(filePath, updated, new UTF8Encoding(false));
                }
            }
        }

        private static string GenerarBloqueMetodosConexion(string databaseName)
        {
            return $@"        private static string? _cadenaConexionEstatica;

        public static void EstablecerCadenaConexion(string nuevaCadena)
        {{
            if (!string.IsNullOrWhiteSpace(nuevaCadena))
            {{
                _cadenaConexionEstatica = NormalizarCadena(nuevaCadena);
            }}
        }}

        public static string ObtenerCadenaConexion()
        {{
            if (!string.IsNullOrWhiteSpace(_cadenaConexionEstatica))
            {{
                return _cadenaConexionEstatica;
            }}

            try
            {{
                string asmDir = Path.GetDirectoryName(typeof(Conexion).Assembly.Location) ?? AppDomain.CurrentDomain.BaseDirectory;
                string[] candidatosTxt = new[]
                {{
                    Path.Combine(asmDir, ""db_connection.txt""),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""db_connection.txt"")
                }};

                foreach (string pathTxt in candidatosTxt)
                {{
                    if (File.Exists(pathTxt))
                    {{
                        string contenido = File.ReadAllText(pathTxt).Trim();
                        if (!string.IsNullOrWhiteSpace(contenido))
                        {{
                            _cadenaConexionEstatica = NormalizarCadena(contenido);
                            return _cadenaConexionEstatica;
                        }}
                    }}
                }}
            }}
            catch {{ }}

            _cadenaConexionEstatica = DetectarInstanciaDisponible();
            return _cadenaConexionEstatica;
        }}

        public static string ObtenerCadenaMaster()
        {{
            var builder = new SqlConnectionStringBuilder(ObtenerCadenaConexion())
            {{
                InitialCatalog = ""master""
            }};
            return builder.ConnectionString;
        }}

        private static string NormalizarCadena(string cadena)
        {{
            try
            {{
                var builder = new SqlConnectionStringBuilder(cadena);
                builder.TrustServerCertificate = true;
                if (builder.ConnectTimeout < 5)
                {{
                    builder.ConnectTimeout = 30;
                }}
                return builder.ConnectionString;
            }}
            catch
            {{
                return cadena;
            }}
        }}

        private static string DetectarInstanciaDisponible()
        {{
            var instancias = new List<string>();
            try
            {{
                using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@""SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL"");
                if (key != null)
                {{
                    foreach (string nombre in key.GetValueNames())
                    {{
                        string inst = nombre == ""MSSQLSERVER"" ? ""."" : @"".\"" + nombre;
                        if (!instancias.Contains(inst)) instancias.Add(inst);
                    }}
                }}
            }}
            catch {{ }}

            foreach (var fallback in new[] {{ ""."", @"".\SQLEXPRESS"", ""localhost"", @""(LocalDB)\MSSQLLocalDB"" }})
            {{
                if (!instancias.Contains(fallback)) instancias.Add(fallback);
            }}

            foreach (var inst in instancias)
            {{
                try
                {{
                    string connStr = $""Data Source={{inst}};Initial Catalog=master;Integrated Security=True;Trust Server Certificate=True;Connect Timeout=2"";
                    using (var cn = new SqlConnection(connStr))
                    {{
                        cn.Open();
                        return NormalizarCadena($""Data Source={{inst}};Initial Catalog={databaseName};Integrated Security=True;Trust Server Certificate=True;Connect Timeout=30"");
                    }}
                }}
                catch {{ }}
            }}

            return ""Data Source=.;Initial Catalog={databaseName};Integrated Security=True;Trust Server Certificate=True;Connect Timeout=30"";
        }}
";
        }

        private static string ReadFilePreservingEncoding(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            string text;
            try
            {
                var utf8Strict = new UTF8Encoding(false, true);
                text = utf8Strict.GetString(bytes);
            }
            catch
            {
                text = Encoding.Latin1.GetString(bytes);
            }
            return text.TrimStart('\uFEFF');
        }
    }
}
