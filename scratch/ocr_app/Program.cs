using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage;

class Program {
    static async Task Main(string[] args) {
        var engine = OcrEngine.TryCreateFromUserProfileLanguages();
        if (engine == null) {
            Console.WriteLine("Could not create OcrEngine");
            return;
        }
        var files = Directory.GetFiles(@"c:\Users\Navegador\Desktop\td\temp_extracted_imgs", "*.png");
        foreach (var file in files) {
            Console.WriteLine($"=== FILE: {Path.GetFileName(file)} ===");
            var sf = await StorageFile.GetFileFromPathAsync(Path.GetFullPath(file));
            using var stream = await sf.OpenAsync(FileAccessMode.Read);
            var decoder = await BitmapDecoder.CreateAsync(stream);
            var bitmap = await decoder.GetSoftwareBitmapAsync();
            var result = await engine.RecognizeAsync(bitmap);
            Console.WriteLine(result.Text);
        }
    }
}
