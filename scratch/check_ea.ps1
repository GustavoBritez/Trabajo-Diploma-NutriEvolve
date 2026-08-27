# Check EA.App COM object
try {
    $ea = New-Object -ComObject "EA.App"
    Write-Host "EA.App COM Object created successfully!"
    $repo = $ea.Repository
    Write-Host "EA Repository LibraryVersion: $($repo.LibraryVersion)"
} catch {
    Write-Host "Error EA.App COM: $_"
}

# Check ADODB Connection with Jet 4.0
try {
    $conn = New-Object -ComObject "ADODB.Connection"
    $dbPath = "c:\Users\Navegador\Desktop\td\Diagramas\TD.EAP"
    $conn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=$dbPath;")
    Write-Host "Jet 4.0 Connection successful!"
    $rs = $conn.Execute("SELECT Diagram_ID, Name, Diagram_Type, Package_ID FROM t_diagram")
    while (-not $rs.EOF) {
        $dId = $rs.Fields.Item("Diagram_ID").Value
        $dName = $rs.Fields.Item("Name").Value
        $dType = $rs.Fields.Item("Diagram_Type").Value
        $pId = $rs.Fields.Item("Package_ID").Value
        Write-Host "Diagram ID: $dId | Name: $dName | Type: $dType | Package_ID: $pId"
        $rs.MoveNext()
    }
    $conn.Close()
} catch {
    Write-Host "Error Jet 4.0: $_"
}
