try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $diags = $ea.SQLQuery("SELECT Diagram_ID, Package_ID, Name, Diagram_Type FROM t_diagram WHERE Package_ID = 16 ORDER BY Diagram_ID")
    Write-Output "--- DIAGRAMS IN PACKAGE 16 ---"
    Write-Output $diags

    foreach ($dName in @("T06b: Bitacora de Cambios - Diagrama de Clases", "T06b: Bitacora de Cambios - DER (Modelo Relacional)", "T06b: Bitacora de Cambios - Diagrama de Secuencia", "T06b: Bitacora de Cambios - GUI (Diseño de Pantalla)")) {
        $xml = $ea.SQLQuery("SELECT Diagram_ID FROM t_diagram WHERE Name = '$dName'")
        [xml]$dDoc = $xml
        $dId = [int]$dDoc.SelectSingleNode("//Row/Diagram_ID").InnerText
        $diag = $ea.GetDiagramByID($dId)
        Write-Output "Diagram ID $dId : $($diag.Name) | Objects: $($diag.DiagramObjects.Count) | Links: $($diag.DiagramLinks.Count)"
    }

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
