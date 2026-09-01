try {
    $eaRepo = New-Object -ComObject EA.Repository
    $opened = $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    if (-not $opened) { exit 1 }

    $pkg = $eaRepo.GetPackageByID(4)
    $diag = $eaRepo.GetDiagramByID(27)

    # 1. Clean up temporary partition objects if needed
    for ($i = $pkg.Elements.Count - 1; $i -ge 0; $i--) {
        $el = $pkg.Elements.GetAt($i)
        if ($el.ElementID -eq 499 -or $el.ElementID -eq 500) {
            # Delete diagram object first
            for ($j = $diag.DiagramObjects.Count - 1; $j -ge 0; $j--) {
                $do = $diag.DiagramObjects.GetAt($j)
                if ($do.ElementID -eq $el.ElementID) {
                    $diag.DiagramObjects.DeleteAt($j, $false)
                }
            }
            $pkg.Elements.DeleteAt($i, $false)
        }
    }
    $diag.DiagramObjects.Refresh()

    # 2. Create standard UML Boundary elements for Swimlanes with Top Headers
    # Lane 1: Nutricionista (Operador)
    $b1 = $pkg.Elements.AddNew("Nutricionista (Operador)", "Boundary")
    $b1.StyleEx = "bclr=16777215;header=1;font=Arial;fontcolor=0;bold=1;align=center;"
    $b1.Update()

    # Lane 2: Sistema (NutriEvolve)
    $b2 = $pkg.Elements.AddNew("Sistema (NutriEvolve)", "Boundary")
    $b2.StyleEx = "bclr=16777215;header=1;font=Arial;fontcolor=0;bold=1;align=center;"
    $b2.Update()

    # Add to diagram
    $dObj1 = $diag.DiagramObjects.AddNew("", "")
    $dObj1.ElementID = $b1.ElementID
    $dObj1.left = 40
    $dObj1.right = 430
    $dObj1.top = -10
    $dObj1.bottom = -1280
    $dObj1.Sequence = 100 # send to back
    $dObj1.Update()

    $dObj2 = $diag.DiagramObjects.AddNew("", "")
    $dObj2.ElementID = $b2.ElementID
    $dObj2.left = 440
    $dObj2.right = 1100
    $dObj2.top = -10
    $dObj2.bottom = -1280
    $dObj2.Sequence = 100 # send to back
    $dObj2.Update()

    # Set native swimlanes in diagram properties as well
    $diag.Swimlanes = "swimlanes=locked=false;orientation=0;width=0;inbar=false;pwidth=0;cls=0;lane:Nutricionista (Operador):w=400:b=16777215:f=0:s=0:lane:Sistema (NutriEvolve):w=670:b=16777215:f=0:s=0:"
    $diag.DiagramObjects.Refresh()
    $diag.Update()
    $eaRepo.ReloadDiagram($diag.DiagramID)

    # 3. Export high-res diagram image
    $project = $eaRepo.GetProjectInterface()
    $outPng = "c:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagramas Actividades\CU01 Diagrama de Actividad.png"
    $res = $project.PutDiagramImageToFile($diag.DiagramGUID, $outPng, 1)
    Write-Output "Export result with visible top header swimlane boundaries: $res"

    $eaRepo.CloseFile()
    [System.Runtime.InteropServices.Marshal]::ReleaseComObject($eaRepo) | Out-Null
    [System.GC]::Collect()
    Write-Output "Done!"
} catch {
    Write-Error "Error: $($_.Exception.Message)"
}
