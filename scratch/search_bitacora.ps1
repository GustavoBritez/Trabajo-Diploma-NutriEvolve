try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $elems = $ea.SQLQuery("SELECT Object_ID, Name, Object_Type, Stereotype, Package_ID FROM t_object WHERE Name LIKE '*Cambio*' OR Name LIKE '*Bitacora*'")
    Write-Output "--- ELEMENTS ---"
    Write-Output $elems

    $diags = $ea.SQLQuery("SELECT Diagram_ID, Name, Diagram_Type, Package_ID FROM t_diagram WHERE Name LIKE '*Cambio*' OR Name LIKE '*Bitacora*'")
    Write-Output "--- DIAGRAMS ---"
    Write-Output $diags

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
