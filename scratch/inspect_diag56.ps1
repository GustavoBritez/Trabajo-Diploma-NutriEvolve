try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $diag = $ea.GetDiagramByID(56)
    Write-Output "Diagram: $($diag.Name)"
    Write-Output "--- Objects ---"
    foreach ($do in $diag.DiagramObjects) {
        $elem = $ea.GetElementByID($do.ElementID)
        Write-Output "Obj ID: $($elem.ElementID) | Name: $($elem.Name) | Type: $($elem.Type) | ClassifierID: $($elem.ClassifierID)"
    }

    Write-Output "--- Connectors via SQL ---"
    $conns = $ea.SQLQuery("SELECT Connector_ID, Name, Connector_Type, SubType, Start_Object_ID, End_Object_ID, SeqNo FROM t_connector WHERE DiagramID = 56 ORDER BY SeqNo")
    Write-Output $conns

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
