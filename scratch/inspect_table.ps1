try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $elem = $ea.GetElementByID(430) # Pacientes_DNI101
    Write-Output "Table: $($elem.Name) | Type: $($elem.Type) | Stereotype: $($elem.Stereotype)"
    Write-Output "Attributes ($($elem.Attributes.Count)):"
    foreach ($att in $elem.Attributes) {
        Write-Output "  - Att: $($att.Name) | Type: $($att.Type) | IsPK: $($att.IsOrdered) | Stereo: $($att.Stereotype)"
    }

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
