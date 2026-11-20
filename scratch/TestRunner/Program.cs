using System;
using PdfSharpCore.Pdf.IO;

namespace TestRunner
{
    class Program
    {
        static void Main()
        {
            string pdfPath = @"C:\Users\Navegador\Desktop\GIT\ING-SOFTWARE-BritezG\Manual_de_Ayuda_NutriEvolve.pdf";
            var doc = PdfReader.Open(pdfPath, PdfDocumentOpenMode.ReadOnly);
            Console.WriteLine($"El documento PDF tiene {doc.PageCount} páginas.");
            for (int i = 0; i < doc.PageCount; i++)
            {
                var p = doc.Pages[i];
                Console.WriteLine($"  Página {i + 1}: Ancho={p.Width}, Alto={p.Height}");
            }
        }
    }
}
