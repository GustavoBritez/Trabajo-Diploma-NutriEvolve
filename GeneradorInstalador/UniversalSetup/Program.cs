using System;
using System.Windows.Forms;

namespace UniversalSetup
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmUniversalSetup());
        }
    }
}
