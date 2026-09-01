try {
    $ea = New-Object -ComObject EA.App -ErrorAction Stop
    Write-Output "EA.App COM Object is available!"
    $repo = $ea.Repository
    $opened = $repo.OpenFile("c:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    Write-Output "File opened: $opened"
    Write-Output "Models count: $($repo.Models.Count)"
    foreach ($m in $repo.Models) {
        Write-Output "Model: $($m.Name)"
    }
    $repo.CloseFile()
    $ea.Quit()
} catch {
    Write-Output "COM EA.App error: $($_.Exception.Message)"
}
