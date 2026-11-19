try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $xml = $ea.SQLQuery("SELECT Diagram_ID, Package_ID, Name, Diagram_Type FROM t_diagram WHERE Package_ID = 16")
    Write-Output $xml

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
