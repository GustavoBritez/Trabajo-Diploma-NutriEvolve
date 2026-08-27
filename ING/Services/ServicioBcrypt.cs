using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services
{
    public class ServicioBcrypt
    {

        //DV

        private const int WorkFactor = 10;

        public string HashearContraseña(string contraseña)
        {
            return BCrypt.Net.BCrypt.HashPassword(contraseña);
        }
        public bool ValidarContraseña(string contraseñaPlana, string hashGuardado)
        {
            return BCrypt.Net.BCrypt.Verify(contraseñaPlana, hashGuardado);
        }


        public static string CalcularDV(string datos)
        {
            if (datos is null)
            {
                datos = string.Empty;
            }

            ulong total = 0;

            foreach (byte valor in System.Text.Encoding.UTF8.GetBytes(datos))
            {
                total += valor;
            }

            return total.ToString("X");
        }

        public static bool ValidarDV(string datos, string dvGuardado)
        {
            if (string.IsNullOrWhiteSpace(dvGuardado))
            {
                return false;
            }

            if (dvGuardado.StartsWith("$2", StringComparison.Ordinal))
            {
                return BCrypt.Net.BCrypt.Verify(datos, dvGuardado);
            }

            return string.Equals(CalcularDV(datos), dvGuardado, StringComparison.OrdinalIgnoreCase);

        }


    }
}
