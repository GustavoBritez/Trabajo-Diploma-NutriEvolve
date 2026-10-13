$conn = New-Object System.Data.SqlClient.SqlConnection("Server=.;Database=master;Integrated Security=True;TrustServerCertificate=True")
try {
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT name FROM sys.databases"
    $r = $cmd.ExecuteReader()
    while ($r.Read()) {
        Write-Host $r["name"]
    }
}
catch {
    Write-Host "Error: " $_.Exception.Message
}
finally {
    $conn.Close()
}
