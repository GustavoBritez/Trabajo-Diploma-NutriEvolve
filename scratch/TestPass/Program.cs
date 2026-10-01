using System;
using System.Collections.Generic;
using System.Linq;
using BLL;
using DAL;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            var dvBll = new DigitoVerificadorBLL();
            Console.WriteLine("--- Verificando Integridad de Base de Datos ---");
            bool ok = dvBll.VerificarBaseDatos();
            Console.WriteLine($"VerificarBaseDatos: {ok}");

            var dvDal = new DigitoVerificadorDAL();
            var persistidos = dvDal.ObtenerResumenPersistido().ToDictionary(p => p.Tabla, StringComparer.OrdinalIgnoreCase);

            // Obtenemos tablas
            var tablas = dvDal.ObtenerTablasPersistentes();
            Console.WriteLine($"Tablas persistentes encontradas: {tablas.Count}");

            foreach (var (schema, table) in tablas)
            {
                var dt = dvDal.ObtenerDatosTabla(schema, table);
                if (persistidos.TryGetValue(table, out var pers))
                {
                    // Comparamos
                    // Como los metodos de calculo son privados en BLL, veamos si en persistidos coincide
                }
                else
                {
                    Console.WriteLine($"[TABLA FALTANTE EN dbo.DV]: {table}");
                }
            }

            var corruptos = dvBll.ObtenerUsuariosCorruptos();
            Console.WriteLine($"Usuarios corruptos: {string.Join(", ", corruptos)}");

            if (args.Length > 0 && args[0] == "--fix")
            {
                Console.WriteLine("Ejecutando ActualizarDVIndividualesUsuarios y RecalcularYPersistir...");
                dvBll.ActualizarDVIndividualesUsuarios();
                dvBll.RecalcularYPersistir();
                Console.WriteLine("Recalculado con exito!");
                Console.WriteLine($"Nueva verificacion: {dvBll.VerificarBaseDatos()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex}");
        }
    }
}
