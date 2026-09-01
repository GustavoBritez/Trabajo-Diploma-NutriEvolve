try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) { exit 1 }

    $xml = $eaRepo.SQLQuery("SELECT Diagram_ID, Diagram_Type, Name, StyleEx, ExtendedStyle, Swimlanes FROM t_diagram WHERE Diagram_ID = 27")
    Write-Output "SQL Result: $xml"

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
