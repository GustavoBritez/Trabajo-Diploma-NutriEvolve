using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Services
{
    /// <summary>
    /// Servicio de Criptografía Reversible utilizando el estándar simétrico AES-256 (Advanced Encryption Standard).
    /// Permite cifrar y descifrar datos confidenciales sensibles (por ejemplo DNI de pacientes) 
    /// garantizando la recuperación exacta del valor original en texto plano.
    /// </summary>
    public static class ServicioCriptografiaAES
    {
        // Clave secreta simétrica de 256 bits (32 bytes UTF-8)
        private static readonly byte[] Clave256 = Encoding.UTF8.GetBytes("NutriEvolveAES256SecretKey2026!!");

        // Vector de Inicialización (IV) de 128 bits (16 bytes UTF-8) para modo CBC
        private static readonly byte[] Iv128 = Encoding.UTF8.GetBytes("NutriEvolveIV16!");

        /// <summary>
        /// Cifra un texto plano utilizando el algoritmo AES-256 en modo CBC con relleno PKCS7.
        /// Devuelve el resultado en formato Base64 para visualización segura en controles de interfaz.
        /// </summary>
        /// <param name="textoPlano">Cadena en texto plano (ej. número de DNI).</param>
        /// <returns>Cadena cifrada en Base64.</returns>
        public static string CifrarAES256(string textoPlano)
        {
            if (string.IsNullOrEmpty(textoPlano))
            {
                return string.Empty;
            }

            // Evitar re-cifrado si la cadena ya es un texto cifrado válido con este servicio (Idempotencia)
            if (EsTextoCifrado(textoPlano))
            {
                return textoPlano;
            }

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Clave256;
                aes.IV = Iv128;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter sw = new StreamWriter(cs, Encoding.UTF8))
                    {
                        sw.Write(textoPlano);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Descifra una cadena previamente cifrada en AES-256 codificada en Base64, 
        /// devolviendo el valor original en texto plano.
        /// </summary>
        /// <param name="textoCifradoBase64">Cadena cifrada en Base64.</param>
        /// <returns>Texto plano original o la cadena recibida si no era válida.</returns>
        public static string DescifrarAES256(string textoCifradoBase64)
        {
            if (string.IsNullOrEmpty(textoCifradoBase64))
            {
                return string.Empty;
            }

            try
            {
                byte[] buffer = Convert.FromBase64String(textoCifradoBase64);

                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = Clave256;
                    aes.IV = Iv128;

                    using (MemoryStream ms = new MemoryStream(buffer))
                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (StreamReader sr = new StreamReader(cs, Encoding.UTF8))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch
            {
                // Si la cadena no es un Base64 válido o no corresponde a un cifrado AES, se devuelve intacta
                return textoCifradoBase64;
            }
        }

        /// <summary>
        /// Determina si una cadena dada aparenta ser un texto cifrado con este servicio.
        /// </summary>
        public static bool EsTextoCifrado(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return false;

            try
            {
                string descifrado = DescifrarAES256(texto);
                return !string.Equals(descifrado, texto, StringComparison.Ordinal);
            }
            catch
            {
                return false;
            }
        }
    }
}
