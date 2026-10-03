using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL
{
    public class IdiomaDAL
    {
        private static readonly object _cacheLock = new object();
        private static readonly Dictionary<string, Dictionary<string, string>> _cacheTraducciones = new();
        private static List<Idioma>? _cacheIdiomas;

        public List<Idioma> ObtenerIdiomas()
        {
            if (_cacheIdiomas != null) return _cacheIdiomas;

            string ruta = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Idiomas",
                "idiomas.json");

            if (!File.Exists(ruta)) return new List<Idioma>();

            try
            {
                string json = File.ReadAllText(ruta);
                _cacheIdiomas = JsonSerializer.Deserialize<List<Idioma>>(json) ?? new List<Idioma>();
                return _cacheIdiomas;
            }
            catch
            {
                return new List<Idioma>();
            }
        }

        private Dictionary<string, string>? ObtenerDiccionario(Idioma idioma)
        {
            if (idioma == null || string.IsNullOrEmpty(idioma.ArchivoJson))
                return null;

            lock (_cacheLock)
            {
                if (_cacheTraducciones.TryGetValue(idioma.ArchivoJson, out var dict))
                    return dict;

                string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Idiomas", idioma.ArchivoJson);
                if (File.Exists(ruta))
                {
                    try
                    {
                        string json = File.ReadAllText(ruta);
                        dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                    }
                    catch
                    {
                        dict = new Dictionary<string, string>();
                    }
                }
                dict ??= new Dictionary<string, string>();
                _cacheTraducciones[idioma.ArchivoJson] = dict;
                return dict;
            }
        }

        public string Traducir(string clave)
        {
            Idioma idioma = ServicesSessionManager.Instancia.ObtenerIdioma();
            return Traducir(clave, idioma);
        }

        public string Traducir(string clave, Idioma idioma)
        {
            if (string.IsNullOrWhiteSpace(clave) || idioma == null)
                return clave;

            var dict = ObtenerDiccionario(idioma);
            if (dict != null && dict.TryGetValue(clave, out var valor))
            {
                return valor;
            }

            return clave;
        }

        public bool ExisteTraduccion(string clave)
        {
            Idioma idioma = ServicesSessionManager.Instancia.ObtenerIdioma();
            return ExisteTraduccion(clave, idioma);
        }

        public bool ExisteTraduccion(string clave, Idioma idioma)
        {
            if (string.IsNullOrWhiteSpace(clave) || idioma == null) return false;
            var dict = ObtenerDiccionario(idioma);
            return dict != null && dict.ContainsKey(clave);
        }
    }
}
