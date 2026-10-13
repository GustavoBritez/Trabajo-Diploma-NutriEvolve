$ErrorActionPreference = "Stop"

Write-Host "=== CHECKING Data Source = . ==="
try {
    $cn1 = New-Object System.Data.SqlClient.SqlConnection("Data Source=.;Initial Catalog=ING;Integrated Security=True;TrustServerCertificate=True")
    $cn1.Open()
    $cmd1 = $cn1.CreateCommand()
    $cmd1.CommandText = "SELECT * FROM Usuarios"
    $ad1 = New-Object System.Data.SqlClient.SqlDataAdapter($cmd1)
    $dt1 = New-Object System.Data.DataTable
    $ad1.Fill($dt1) | Out-Null
    $dt1 | Format-Table -AutoSize
    $cn1.Close()
} catch {
    Write-Host "Error on . : " $_.Exception.Message
}

Write-Host "`n=== CHECKING Data Source = (LocalDB)\MSSQLLocalDB ==="
try {
    $cn2 = New-Object System.Data.SqlClient.SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=ING;Integrated Security=True")
    $cn2.Open()
    $cmd2 = $cn2.CreateCommand()
    $cmd2.CommandText = "SELECT * FROM Usuarios"
    $ad2 = New-Object System.Data.SqlClient.SqlDataAdapter($cmd2)
    $dt2 = New-Object System.Data.DataTable
    $ad2.Fill($dt2) | Out-Null
    $dt2 | Format-Table -AutoSize
    $cn2.Close()
} catch {
    Write-Host "Error on LocalDB: " $_.Exception.Message
}
