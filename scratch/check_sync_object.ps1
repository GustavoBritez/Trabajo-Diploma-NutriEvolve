try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) { exit 1 }

    $xml = $eaRepo.SQLQuery("SELECT Object_ID, Object_Type, Name, NType, Subtype, PDATA1, PDATA2, PDATA3, StyleEx FROM t_object WHERE Object_ID IN (495, 496)")
    Write-Output $xml

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
} catch {
    Write-Error "Error: $($_.Exception.Message)"
}
