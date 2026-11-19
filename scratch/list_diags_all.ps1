try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $diags = $ea.SQLQuery("SELECT Diagram_ID, Package_ID, Name, Diagram_Type FROM t_diagram ORDER BY Package_ID, Diagram_Type, Name")
    Write-Output $diags

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
