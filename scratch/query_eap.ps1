try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) {
        Write-Output "Could not open TD.EAP"
        exit 1
    }

    Write-Output "=== PACKAGES ==="
    $xml = $ea.SQLQuery("SELECT Package_ID, Name, Parent_ID FROM t_package ORDER BY Package_ID")
    Write-Output $xml

    Write-Output "=== DIAGRAMS (BITACORA / CAMBIOS / CUN08 / CUN02) ==="
    $diagXml = $ea.SQLQuery("SELECT Diagram_ID, Name, Diagram_Type, Package_ID FROM t_diagram WHERE Name LIKE '%Bitacora%' OR Name LIKE '%Cambio%' OR Name LIKE '%CUN08%' OR Name LIKE '%CUN02%' ORDER BY Diagram_ID")
    Write-Output $diagXml

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
