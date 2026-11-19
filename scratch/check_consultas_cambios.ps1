try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $xml = $ea.SQLQuery("SELECT Object_ID, Name, Object_Type, Stereotype FROM t_object WHERE Name LIKE '*Consultas*Cambio*'")
    Write-Output $xml

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
