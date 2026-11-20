using System;
using System.IO;
using UI.PN2;

namespace TestRunner
{
    class Program
    {
        static void Main()
        {
            string pdfRoot = @"C:\Users\Danie\Desktop\GIT\Manual_de_Ayuda_NutriEvolve.pdf";
            string pdfProj = @"C:\Users\Danie\Desktop\GIT\ING-SOFTWARE-BritezG\Manual_de_Ayuda_NutriEvolve.pdf";

            Console.WriteLine("Generando PDF limpio y simplificado...");
            ManualAyudaPdfGenerator.GenerarPdf(pdfRoot);
            Console.WriteLine($"Generado en raíz: {pdfRoot} (Tamaño: {new FileInfo(pdfRoot).Length} bytes)");

            File.Copy(pdfRoot, pdfProj, true);
            Console.WriteLine($"Copiado a proyecto: {pdfProj} (Tamaño: {new FileInfo(pdfProj).Length} bytes)");
        }
    }
}
