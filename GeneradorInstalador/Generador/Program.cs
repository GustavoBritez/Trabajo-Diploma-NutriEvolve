using System;
using System.Windows.Forms;
using GeneradorInstalador.Core;

namespace GeneradorInstalador
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length >= 2 && args[0] == "--inject")
            {
                var manifest = ProjectAnalyzer.Analyze(args[1]);
                ConnectionInjector.Inject(manifest);
                return;
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new FrmGenerador());
        }
    }
}
