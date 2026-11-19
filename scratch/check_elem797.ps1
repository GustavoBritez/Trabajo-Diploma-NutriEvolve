try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $elem = $ea.GetElementByID(797) # ConsultasNutricionales_DNI101
    Write-Output "Table: $($elem.Name)"
    foreach ($att in $elem.Attributes) {
        Write-Output "  - Att: $($att.Name) ($($att.Type)) [Stereo: $($att.Stereotype)]"
    }

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
