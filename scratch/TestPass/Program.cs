using System;
using System.IO;
using BLL;
using Services;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("=== PRUEBA DE INTEGRIDAD, BACKUP Y RESTORE ===");
        Console.WriteLine("========================================");

        var backupBLL = new BackupBLL();
        var dvBLL = new DigitoVerificadorBLL();

        // 1. Recalibrar y verificar estado inicial
        Console.WriteLine("\n[Paso 1] Recalibrando y verificando estado inicial...");
        dvBLL.RecalcularYPersistir();
        var alteradasInicio = dvBLL.ObtenerTablasAlteradas();
        Console.WriteLine($"Tablas alteradas iniciales: {alteradasInicio.Count}");
        if (alteradasInicio.Count > 0)
        {
            foreach (var t in alteradasInicio) Console.WriteLine($" - Alterada: {t}");
            return;
        }

        // Definir ruta de backup (en C:\Users\Public para permisos de SQL Server)
        string backupDir = @"C:\Users\Public";
        string backupPath = Path.Combine(backupDir, $"TestBackup_{DateTime.Now:yyyyMMdd_HHmmss}.bak");
        Console.WriteLine($"\n[Paso 2] Ejecutando Backup en: {backupPath}");

        backupBLL.RealizarBackup(backupPath);

        if (File.Exists(backupPath))
        {
            var fi = new FileInfo(backupPath);
            Console.WriteLine($"-> Backup completado con éxito! Tamaño del archivo: {fi.Length / 1024 / 1024.0:F2} MB ({fi.Length} bytes)");
        }
        else
        {
            Console.WriteLine("-> ERROR: El archivo de backup no fue encontrado!");
            return;
        }

        // 3. Verificar integridad tras Backup
        Console.WriteLine("\n[Paso 3] Verificando integridad tras Backup...");
        var alteradasTrasBackup = dvBLL.ObtenerTablasAlteradas();
        Console.WriteLine($"Tablas alteradas tras backup: {alteradasTrasBackup.Count}");
        if (alteradasTrasBackup.Count > 0)
        {
            foreach (var t in alteradasTrasBackup) Console.WriteLine($" - Alterada: {t}");
            return;
        }
        Console.WriteLine("-> Integridad post-backup: OK (0 tablas alteradas)");

        // 4. Ejecutar Restore
        Console.WriteLine($"\n[Paso 4] Ejecutando Restore desde: {backupPath}");
        backupBLL.RealizarRestore(backupPath);
        Console.WriteLine("-> Restore ejecutado con éxito.");

        // 5. Verificar integridad tras Restore
        Console.WriteLine("\n[Paso 5] Verificando integridad tras Restore...");
        var alteradasTrasRestore = dvBLL.ObtenerTablasAlteradas();
        Console.WriteLine($"Tablas alteradas tras restore: {alteradasTrasRestore.Count}");
        if (alteradasTrasRestore.Count > 0)
        {
            foreach (var t in alteradasTrasRestore) Console.WriteLine($" - Alterada: {t}");
            return;
        }
        Console.WriteLine("-> Integridad post-restore: OK (0 tablas alteradas)");

        // Limpieza de prueba
        try
        {
            if (File.Exists(backupPath))
            {
                File.Delete(backupPath);
                Console.WriteLine("\n[Limpieza] Archivo de prueba temporal eliminado correctamente.");
            }
        }
        catch { }

        Console.WriteLine("\n========================================");
        Console.WriteLine("=== TODAS LAS PRUEBAS DE BACKUP Y RESTORE PASARON CON ÉXITO ===");
        Console.WriteLine("========================================");
    }
}
