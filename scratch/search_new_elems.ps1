try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) { exit 1 }

    $names = @('PacienteCambioBE_DNI101', 'BitacoraCambiosConsultaBLL_DNI101', 'BitacoraCambiosConsultaDAL_DNI101', 'Pacientes_Cambios_DNI101', 'Paciente_Telefonos_DNI101', 'Paciente_Emails_DNI101', 'frmBitacoraCambiosConsultas', 'ServicioCriptografiaAES')
    foreach ($n in $names) {
        $xml = $ea.SQLQuery("SELECT Object_ID, Name, Object_Type, Stereotype FROM t_object WHERE Name = '$n'")
        Write-Output "Search $n : $xml"
    }

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
