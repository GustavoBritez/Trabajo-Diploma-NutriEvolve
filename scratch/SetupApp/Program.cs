using System;
using System.Windows.Forms;

namespace SetupApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmSetup());
        }
    }
}
