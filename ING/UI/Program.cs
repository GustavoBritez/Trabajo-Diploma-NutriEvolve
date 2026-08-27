namespace UI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            // Configurar DataDirectory para que LocalDB encuentre la BD relativa al ejecutable
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            AppDomain.CurrentDomain.SetData("DataDirectory", baseDir);

            // Inicializa la base de datos a partir del backup si no existe
            DAL.DatabaseInitializer.InitializeDatabase();

            ApplicationConfiguration.Initialize();
            Application.Run(new FormTurnero_DNI101());
        }
    }
}