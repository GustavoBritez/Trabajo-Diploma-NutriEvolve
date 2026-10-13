$connStr = "Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=ING;Integrated Security=True;Connect Timeout=30"
$conn = New-Object System.Data.SqlClient.SqlConnection($connStr)
try {
    $conn.Open()
    Write-Host "Connected successfully to ING database."
    
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT DNI, NombreDeUsuario, Nombre, Apellido, [Contrasea] as [Pass], Bloqueado, Estado, ID_Perfil FROM Usuarios"
    # Actually select all columns
    $cmd.CommandText = "SELECT * FROM Usuarios"
    $adapter = New-Object System.Data.SqlClient.SqlDataAdapter($cmd)
    $dt = New-Object System.Data.DataTable
    $adapter.Fill($dt) | Out-Null
    
    $dt | Format-Table DNI, NombreDeUsuario, Nombre, Apellido, Bloqueado, Estado, ID_Perfil, Contraseña -AutoSize
    
    Write-Host "`nProfiles / Roles:"
    $cmd.CommandText = "SELECT * FROM Perfil"
    $adapter.SelectCommand = $cmd
    $dtPerfil = New-Object System.Data.DataTable
    $adapter.Fill($dtPerfil) | Out-Null
    $dtPerfil | Format-Table -AutoSize

    Write-Host "`nNutricionista related tables/profiles:"
    $cmd.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'"
    $dtTables = New-Object System.Data.DataTable
    $adapter.Fill($dtTables) | Out-Null
    $dtTables | Format-Table -AutoSize
}
catch {
    Write-Error $_.Exception.Message
}
finally {
    $conn.Close()
}
