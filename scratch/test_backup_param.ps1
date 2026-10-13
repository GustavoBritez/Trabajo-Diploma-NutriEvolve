$conn = New-Object System.Data.SqlClient.SqlConnection("Server=.;Database=ING;Integrated Security=True;TrustServerCertificate=True")
try {
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "BACKUP DATABASE [ING] TO DISK = @ruta WITH INIT"
    $param = New-Object System.Data.SqlClient.SqlParameter("@ruta", "C:\Users\Danie\Desktop\GIT\TD\scratch\test_backup.bak")
    $cmd.Parameters.Add($param)
    $cmd.ExecuteNonQuery()
    Write-Host "BACKUP SUCCESS!"
}
catch {
    Write-Host "BACKUP FAILED: " $_.Exception.ToString()
}
finally {
    if ($conn) { $conn.Close() }
}
