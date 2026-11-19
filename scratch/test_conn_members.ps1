try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $conn = $ea.GetConnectorByID(4110) # btnRegistrarPaciente_Click()
    $conn | Get-Member | Select-Object Name, MemberType | Out-String | Write-Output

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
