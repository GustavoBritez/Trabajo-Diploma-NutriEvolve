try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) {
        Write-Output "Could not open TD.EAP"
        exit 1
    }

    Write-Output "TD.EAP opened successfully."

    # 1. Locate or create Package 'T06b - Bitacora de Cambios' under Package 4 (Primary Use Cases)
    $pkgPrimary = $ea.GetPackageByID(4)
    $pkgBitacora = $null
    foreach ($p in $pkgPrimary.Packages) {
        if ($p.Name -like "*Bitacora de Cambios*") {
            $pkgBitacora = $p
            break
        }
    }
    if ($pkgBitacora -eq $null) {
        $pkgBitacora = $pkgPrimary.Packages.AddNew("T06b - Bitacora de Cambios", "Package")
        $pkgBitacora.Update()
        $pkgPrimary.Packages.Refresh()
        Write-Output "Created package: $($pkgBitacora.Name) (ID: $($pkgBitacora.PackageID))"
    } else {
        Write-Output "Found package: $($pkgBitacora.Name) (ID: $($pkgBitacora.PackageID))"
    }

    # Helper function to find or create an element
    function GetOrCreateElement($pkg, $name, $type, $stereo) {
        $xml = $ea.SQLQuery("SELECT Object_ID FROM t_object WHERE Name = '$name' AND Object_Type = '$type'")
        [xml]$doc = $xml
        $row = $doc.SelectSingleNode("//Row")
        if ($row -ne $null) {
            $objId = [int]$row.SelectSingleNode("Object_ID").InnerText
            return $ea.GetElementByID($objId)
        }
        $elem = $pkg.Elements.AddNew($name, $type)
        if ($stereo -ne "") {
            $elem.Stereotype = $stereo
        }
        $elem.Update()
        $pkg.Elements.Refresh()
        return $elem
    }

    # Helper function to add attribute if not exists
    function AddAttributeIfNotExists($elem, $name, $type, $stereo) {
        foreach ($att in $elem.Attributes) {
            if ($att.Name -eq $name) { return $att }
        }
        $newAtt = $elem.Attributes.AddNew($name, $type)
        if ($stereo -ne "") { $newAtt.Stereotype = $stereo }
        $newAtt.Update()
        $elem.Attributes.Refresh()
        return $newAtt
    }

    # Helper function to add method if not exists
    function AddMethodIfNotExists($elem, $name, $retType) {
        foreach ($m in $elem.Methods) {
            if ($m.Name -eq $name) { return $m }
        }
        $newM = $elem.Methods.AddNew($name, $retType)
        $newM.ReturnType = $retType
        $newM.Update()
        $elem.Methods.Refresh()
        return $newM
    }

    # Update PacienteBLL_DNI101 with reversible crypto methods
    $bllPac = $ea.GetElementByID(757)
    if ($bllPac -ne $null) {
        AddMethodIfNotExists $bllPac "CifrarDniReversible" "string" | Out-Null
        AddMethodIfNotExists $bllPac "DescifrarDniReversible" "string" | Out-Null
    }

    # Create / Get ServicioCriptografiaAES
    $elemAES = GetOrCreateElement $pkgBitacora "ServicioCriptografiaAES" "Class" ""
    AddMethodIfNotExists $elemAES "Encriptar" "string" | Out-Null
    AddMethodIfNotExists $elemAES "Desencriptar" "string" | Out-Null

    # Create / Get PacienteCambioBE_DNI101
    $elemBE = GetOrCreateElement $pkgBitacora "PacienteCambioBE_DNI101" "Class" ""
    AddAttributeIfNotExists $elemBE "IdCambio" "int" "" | Out-Null
    AddAttributeIfNotExists $elemBE "IdPaciente" "int" "" | Out-Null
    AddAttributeIfNotExists $elemBE "FechaHora" "DateTime" "" | Out-Null
    AddAttributeIfNotExists $elemBE "DniNiño" "string" "" | Out-Null
    AddAttributeIfNotExists $elemBE "Nombre" "string" "" | Out-Null
    AddAttributeIfNotExists $elemBE "Apellido" "string" "" | Out-Null
    AddAttributeIfNotExists $elemBE "EmailPrevio" "string?" "" | Out-Null
    AddAttributeIfNotExists $elemBE "TelefonoPrevio" "string?" "" | Out-Null
    AddAttributeIfNotExists $elemBE "DireccionPrevia" "string?" "" | Out-Null
    AddAttributeIfNotExists $elemBE "TipoOperacion" "string" "" | Out-Null
    AddAttributeIfNotExists $elemBE "DniUsuario" "string" "" | Out-Null
    AddAttributeIfNotExists $elemBE "Act" "bool" "" | Out-Null

    # Create / Get BitacoraCambiosConsultaBLL_DNI101
    $elemBLL = GetOrCreateElement $pkgBitacora "BitacoraCambiosConsultaBLL_DNI101" "Class" ""
    AddMethodIfNotExists $elemBLL "ListarCambiosPacientes" "List<PacienteCambioBE_DNI101>" | Out-Null
    AddMethodIfNotExists $elemBLL "RevertirCambioPaciente" "bool" | Out-Null
    AddMethodIfNotExists $elemBLL "ListarCambios" "List<ConsultaCambioBE_DNI101>" | Out-Null
    AddMethodIfNotExists $elemBLL "RevertirCambio" "bool" | Out-Null

    # Create / Get BitacoraCambiosConsultaDAL_DNI101
    $elemDAL = GetOrCreateElement $pkgBitacora "BitacoraCambiosConsultaDAL_DNI101" "Class" ""
    AddMethodIfNotExists $elemDAL "ListarCambiosPacientes" "List<PacienteCambioBE_DNI101>" | Out-Null
    AddMethodIfNotExists $elemDAL "RevertirCambioPaciente" "bool" | Out-Null
    AddMethodIfNotExists $elemDAL "ListarCambios" "List<ConsultaCambioBE_DNI101>" | Out-Null
    AddMethodIfNotExists $elemDAL "RevertirCambio" "bool" | Out-Null

    # Create Tables for DER
    # 1) Pacientes_Cambios_DNI101
    $tblCambios = GetOrCreateElement $pkgBitacora "Pacientes_Cambios_DNI101" "Class" "table"
    AddAttributeIfNotExists $tblCambios "IdCambio" "INT" "PK" | Out-Null
    AddAttributeIfNotExists $tblCambios "IdPaciente" "INT" "FK" | Out-Null
    AddAttributeIfNotExists $tblCambios "FechaHora" "DATETIME" "NOT NULL" | Out-Null
    AddAttributeIfNotExists $tblCambios "DniNiño" "VARCHAR(50)" "NOT NULL" | Out-Null
    AddAttributeIfNotExists $tblCambios "Nombre" "VARCHAR(100)" "NOT NULL" | Out-Null
    AddAttributeIfNotExists $tblCambios "Apellido" "VARCHAR(100)" "NOT NULL" | Out-Null
    AddAttributeIfNotExists $tblCambios "EmailPrevio" "VARCHAR(150)" "NULL" | Out-Null
    AddAttributeIfNotExists $tblCambios "TelefonoPrevio" "VARCHAR(50)" "NULL" | Out-Null
    AddAttributeIfNotExists $tblCambios "DireccionPrevia" "VARCHAR(200)" "NULL" | Out-Null
    AddAttributeIfNotExists $tblCambios "Activo" "BIT" "NOT NULL" | Out-Null
    AddAttributeIfNotExists $tblCambios "TipoOperacion" "VARCHAR(20)" "NOT NULL" | Out-Null
    AddAttributeIfNotExists $tblCambios "DniUsuario" "VARCHAR(50)" "NOT NULL" | Out-Null
    AddAttributeIfNotExists $tblCambios "Act" "BIT" "NOT NULL" | Out-Null

    # 2) Paciente_Telefonos_DNI101
    $tblTel = GetOrCreateElement $pkgBitacora "Paciente_Telefonos_DNI101" "Class" "table"
    AddAttributeIfNotExists $tblTel "IdTelefono" "INT" "PK" | Out-Null
    AddAttributeIfNotExists $tblTel "IdPaciente" "INT" "FK" | Out-Null
    AddAttributeIfNotExists $tblTel "Telefono" "VARCHAR(50)" "NOT NULL" | Out-Null
    AddAttributeIfNotExists $tblTel "Tipo" "VARCHAR(20)" "NULL" | Out-Null
    AddAttributeIfNotExists $tblTel "EsPrincipal" "BIT" "NULL" | Out-Null

    # 3) Paciente_Emails_DNI101
    $tblMail = GetOrCreateElement $pkgBitacora "Paciente_Emails_DNI101" "Class" "table"
    AddAttributeIfNotExists $tblMail "IdEmail" "INT" "PK" | Out-Null
    AddAttributeIfNotExists $tblMail "IdPaciente" "INT" "FK" | Out-Null
    AddAttributeIfNotExists $tblMail "Email" "VARCHAR(150)" "NOT NULL" | Out-Null
    AddAttributeIfNotExists $tblMail "Tipo" "VARCHAR(20)" "NULL" | Out-Null
    AddAttributeIfNotExists $tblMail "EsPrincipal" "BIT" "NULL" | Out-Null

    Write-Output "Entities, Classes, and Tables created / verified successfully."

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
