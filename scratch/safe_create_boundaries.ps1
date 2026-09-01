try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) { exit 1 }

    $pkg = $eaRepo.GetPackageByID(4)
    $diag = $eaRepo.GetDiagramByID(27)

    # Find or Create Boundary elements
    $b1 = $null
    $b2 = $null
    foreach ($el in $pkg.Elements) {
        if ($el.Type -eq "Boundary") {
            if ($el.Name -like "*Nutricionista*") { $b1 = $el }
            if ($el.Name -like "*Sistema*") { $b2 = $el }
        }
    }

    if ($null -eq $b1) {
        $b1 = $pkg.Elements.AddNew("Nutricionista (Operador)", "Boundary")
        $b1.StyleEx = "header=1;font=Arial;bold=1;align=center;"
        $b1.Update()
        Write-Output "Created Boundary B1 ID: $($b1.ElementID)"
    }
    if ($null -eq $b2) {
        $b2 = $pkg.Elements.AddNew("Sistema (NutriEvolve)", "Boundary")
        $b2.StyleEx = "header=1;font=Arial;bold=1;align=center;"
        $b2.Update()
        Write-Output "Created Boundary B2 ID: $($b2.ElementID)"
    }

    function Set-DiagramBoundary($d, $elemID, $l, $r, $t, $b) {
        $found = $null
        foreach ($do in $d.DiagramObjects) {
            if ($do.ElementID -eq $elemID) { $found = $do; break }
        }
        if ($null -eq $found) {
            $found = $d.DiagramObjects.AddNew("", "")
            $found.ElementID = $elemID
        }
        $found.left = $l
        $found.right = $r
        $found.top = $t
        $found.bottom = $b
        $found.Sequence = 1000 # Send to background
        $found.Update()
        return $found
    }

    Set-DiagramBoundary $diag $b1.ElementID 40 430 -10 -1280 | Out-Null
    Set-DiagramBoundary $diag $b2.ElementID 440 1100 -10 -1280 | Out-Null

    $diag.DiagramObjects.Refresh()
    $diag.Update()
    $eaRepo.ReloadDiagram($diag.DiagramID)

    # Export high-res diagram image
    $project = $eaRepo.GetProjectInterface()
    $outPng = "c:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Actividades\CU01 Diagrama de Actividad.png"
    $res = $project.PutDiagramImageToFile($diag.DiagramGUID, $outPng, 1)
    Write-Output "Exported diagram with boundary swimlanes: $res"

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
    Write-Output "Success!"
} catch {
    Write-Error "Error: $($_.Exception.Message)"
}
