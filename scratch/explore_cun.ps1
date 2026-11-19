try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    # Query all diagrams
    $xml = $ea.SQLQuery("SELECT Diagram_ID, Package_ID, Name, Diagram_Type FROM t_diagram WHERE Name LIKE '*08*' OR Name LIKE '*02*' OR Name LIKE '*Bitacora*' OR Name LIKE '*Cliente*' ORDER BY Diagram_ID")
    Write-Output "--- DIAGRAMS ---"
    Write-Output $xml

    # Check Use Cases
    $ucXml = $ea.SQLQuery("SELECT Object_ID, Name, Object_Type, Note FROM t_object WHERE Object_Type = 'UseCase' ORDER BY Object_ID")
    Write-Output "--- USE CASES ---"
    Write-Output $ucXml

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
