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
        public List<Idioma> ObtenerIdiomas()
        {
            string ruta = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Idiomas",
                "idiomas.json");

            string json = File.ReadAllText(ruta);

            return JsonSerializer.Deserialize<List<Idioma>>(json);
        }


        public string Traducir(string clave)
        {
            Idioma idioma = ServicesSessionManager.Instancia.ObtenerIdioma();

            return Traducir(clave, idioma);
        }

        public string Traducir(string clave, Idioma idioma)
        {
            if (idioma == null)
                return clave;

            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Idiomas", idioma.ArchivoJson);

            string json = File.ReadAllText(ruta);

            Dictionary<string, string> traducciones =
                JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            if (traducciones.ContainsKey(clave))
                return traducciones[clave];

            return clave;
        }
    }
}
