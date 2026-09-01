try {
    $eaRepo = New-Object -ComObject EA.Repository
    $eaRepo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    $project = $eaRepo.GetProjectInterface()
    $methods = $project | Get-Member -MemberType Method | Where-Object { $_.Name -like "*Diagram*" }
    foreach ($m in $methods) {
        Write-Output "$($m.Name) -> $($m.Definition)"
    }
    $eaRepo.CloseFile()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
