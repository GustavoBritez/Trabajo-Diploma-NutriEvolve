try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) { exit 1 }

    $pkg = $eaRepo.GetPackageByID(4)
    $diag = $eaRepo.GetDiagramByID(27)

    # Check existing ActivityPartition elements
    $p1 = $null
    $p2 = $null
    foreach ($elem in $pkg.Elements) {
        if ($elem.Type -eq "ActivityPartition" -or $elem.Type -eq "Partition") {
            if ($elem.Name -like "*Nutricionista*" -or $elem.Name -like "*Operador*") { $p1 = $elem }
            if ($elem.Name -like "*Sistema*") { $p2 = $elem }
        }
    }

    if ($null -eq $p1) {
        $p1 = $pkg.Elements.AddNew("Nutricionista (Operador)", "ActivityPartition")
        $p1.Update()
        Write-Output "Created ActivityPartition: $($p1.Name) (ID: $($p1.ElementID))"
    }
    if ($null -eq $p2) {
        $p2 = $pkg.Elements.AddNew("Sistema (NutriEvolve)", "ActivityPartition")
        $p2.Update()
        Write-Output "Created ActivityPartition: $($p2.Name) (ID: $($p2.ElementID))"
    }

    # Ensure both partitions are in the diagram
    function Ensure-PartitionObject($d, $elemID, $l, $r, $t, $b) {
        $found = $null
        foreach ($dObj in $d.DiagramObjects) {
            if ($dObj.ElementID -eq $elemID) { $found = $dObj; break }
        }
        if ($null -eq $found) {
            $found = $d.DiagramObjects.AddNew("", "")
            $found.ElementID = $elemID
        }
        $found.left = $l
        $found.right = $r
        $found.top = $t
        $found.bottom = $b
        $found.Sequence = 1 # Send to back
        $found.Update()
        return $found
    }

    # Partition 1: Left column
    Ensure-PartitionObject $diag $p1.ElementID 50 430 -10 -1300 | Out-Null
    # Partition 2: Right column
    Ensure-PartitionObject $diag $p2.ElementID 440 1100 -10 -1300 | Out-Null

    $diag.DiagramObjects.Refresh()
    $diag.Update()
    $eaRepo.ReloadDiagram($diag.DiagramID)

    # Export diagram image
    $project = $eaRepo.GetProjectInterface()
    $outPng = "c:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Actividades\CU01 Diagrama de Actividad.png"
    $res = $project.PutDiagramImageToFile($diag.DiagramGUID, $outPng, 1)
    Write-Output "Exported diagram with ActivityPartitions: $res"

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
} catch {
    Write-Error "Error: $($_.Exception.Message)"
}
