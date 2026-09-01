try {
    $eaRepo = New-Object -ComObject EA.Repository

    $files = @(
        "c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP",
        "c:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\Monitoreo Nutricional.EAP",
        "c:\Users\Danie\Desktop\GIT\TD\ING-SOFTWARE-BritezG\dssDeCampo.EAP"
    )

    foreach ($f in $files) {
        if (Test-Path $f) {
            $eaRepo.OpenFile($f)
            Write-Output "=== Checking $f ==="
            $xml = $eaRepo.SQLQuery("SELECT Diagram_ID, Name, Diagram_Type, Swimlanes, StyleEx, ExtendedStyle FROM t_diagram WHERE Swimlanes IS NOT NULL AND Swimlanes <> ''")
            Write-Output $xml
            $eaRepo.CloseFile()
        }
    }

    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
} catch {
    Write-Error "Error: $($_.Exception.Message)"
}
