try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) { exit 1 }

    # List all diagrams in TD.EAP
    $rs = $eaRepo.SQLQuery("SELECT Diagram_ID, Name, Diagram_Type, StyleEx FROM t_diagram")
    Write-Output $rs

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
} catch {
    Write-Error "Error: $($_.Exception.Message)"
}
