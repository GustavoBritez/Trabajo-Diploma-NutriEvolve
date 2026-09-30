using System;
using System.Numerics;
using System.Text;

class Program
{
    private static BigInteger ObtenerValorHexadecimal(object valor)
    {
        if (valor == null || valor == DBNull.Value) return BigInteger.Zero;
        byte[] bytes = Encoding.UTF8.GetBytes(valor.ToString() ?? "");
        BigInteger total = BigInteger.Zero;
        foreach (byte b in bytes) total += b;
        return total;
    }

    static void Main()
    {
        string passHash = "$2a$11$ZGr/gvxtcscqsxUd7hPvte0yVa5/iZlxQ8F13/EjHqor2a9XG0Qvy";
        
        // Columnas en Usuarios: DNI, NombreDeUsuario, Nombre, Apellido, Contraseña, Bloqueado, Estado, ID_Perfil, Idioma
        // En script.sql: (11111111, N'admin', N'Gabriel', N'Avalos', N'passHash', 0, 1, 1, N'Español')
        
        object[] cols = { 11111111, "admin", "Gabriel", "Avalos", passHash, 0, 1, 1, "Español" };
        
        BigInteger total = BigInteger.Zero;
        foreach (var c in cols)
        {
            total += ObtenerValorHexadecimal(c);
        }

        string dv = total.ToString("X");
        Console.WriteLine($"Calculated DV for admin with 123: {dv}");
    }
}
