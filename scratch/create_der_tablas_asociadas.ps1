$ErrorActionPreference = "Stop"

try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) {
        Write-Output "Could not open TD.EAP"
        exit 1
    }

    Write-Output "TD.EAP opened successfully."

    # Locate package 16
    $pkgBitacora = $ea.GetPackageByID(16)
    Write-Output "Package 16: $($pkgBitacora.Name)"

    function GetOrCreateElement([object]$pkg, [string]$name, [string]$type, [string]$stereo) {
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
        [void]$elem.Update()
        $pkg.Elements.Refresh()
        return $elem
    }

    function AddAttr([object]$elem, [string]$name, [string]$type, [string]$stereo) {
        foreach ($att in $elem.Attributes) {
            if ($att.Name -eq $name) { return $att }
        }
        $newAtt = $elem.Attributes.AddNew($name, $type)
        if ($stereo -ne "") { $newAtt.Stereotype = $stereo }
        [void]$newAtt.Update()
        $elem.Attributes.Refresh()
        return $newAtt
    }

    function AddAssociation([object]$srcElem, [object]$tgtElem, [string]$name, [string]$srcCard, [string]$tgtCard) {
        foreach ($c in $srcElem.Connectors) {
            if ($c.SupplierID -eq $tgtElem.ElementID -and $c.Name -eq $name) {
                return $c
            }
        }
        $conn = $srcElem.Connectors.AddNew($name, "Association")
        $conn.SupplierID = $tgtElem.ElementID
        if ($srcCard -ne "") { $conn.ClientEnd.Cardinality = $srcCard }
        if ($tgtCard -ne "") { $conn.SupplierEnd.Cardinality = $tgtCard }
        [void]$conn.Update()
        $srcElem.Connectors.Refresh()
        return $conn
    }

    # 1. Existing Tables
    $tblPac = $ea.GetElementByID(430)       # Pacientes_DNI101
    $tblCambios = GetOrCreateElement $pkgBitacora "Pacientes_Cambios_DNI101" "Class" "table"
    $tblTel = GetOrCreateElement $pkgBitacora "Paciente_Telefonos_DNI101" "Class" "table"
    $tblMail = GetOrCreateElement $pkgBitacora "Paciente_Emails_DNI101" "Class" "table"
    $tblConsultas = $ea.GetElementByID(797) # ConsultasNutricionales_DNI101
    $tblUser = $ea.GetElementByID(431)      # Usuarios
    $tblBit = $ea.GetElementByID(436)       # Bitacora
    $tblDV = $ea.GetElementByID(435)        # DV

    # 2. Create ConsultasNutricionales_Cambios_DNI101
    $tblConsCambios = GetOrCreateElement $pkgBitacora "ConsultasNutricionales_Cambios_DNI101" "Class" "table"
    AddAttr $tblConsCambios "IdCambio" "INT" "PK" | Out-Null
    AddAttr $tblConsCambios "IdConsulta_DNI101" "INT" "FK" | Out-Null
    AddAttr $tblConsCambios "IdPaciente_DNI101" "INT" "FK" | Out-Null
    AddAttr $tblConsCambios "DniNutricionista_DNI101" "INT" "FK" | Out-Null
    AddAttr $tblConsCambios "FechaControl_DNI101" "DATETIME2" "NOT NULL" | Out-Null
    AddAttr $tblConsCambios "Peso_DNI101" "DECIMAL(5,2)" "NOT NULL" | Out-Null
    AddAttr $tblConsCambios "Talla_DNI101" "DECIMAL(5,2)" "NOT NULL" | Out-Null
    AddAttr $tblConsCambios "IMC_DNI101" "DECIMAL(5,2)" "NOT NULL" | Out-Null
    AddAttr $tblConsCambios "Diagnostico_DNI101" "VARCHAR(255)" "NOT NULL" | Out-Null
    AddAttr $tblConsCambios "FechaCambio" "DATETIME2" "NOT NULL" | Out-Null
    AddAttr $tblConsCambios "UsuarioDni" "VARCHAR(20)" "FK" | Out-Null
    AddAttr $tblConsCambios "TipoOperacion" "VARCHAR(20)" "NOT NULL" | Out-Null
    AddAttr $tblConsCambios "Act" "BIT" "NOT NULL" | Out-Null

    # Ensure Pacientes_Cambios_DNI101 has all clear attributes
    AddAttr $tblCambios "IdCambio" "INT" "PK" | Out-Null
    AddAttr $tblCambios "IdPaciente" "INT" "FK" | Out-Null
    AddAttr $tblCambios "FechaHora" "DATETIME" "NOT NULL" | Out-Null
    AddAttr $tblCambios "DniNiño" "VARCHAR(50)" "NOT NULL" | Out-Null
    AddAttr $tblCambios "Nombre" "VARCHAR(100)" "NOT NULL" | Out-Null
    AddAttr $tblCambios "Apellido" "VARCHAR(100)" "NOT NULL" | Out-Null
    AddAttr $tblCambios "TelefonoPrevio" "VARCHAR(50)" "NULL" | Out-Null
    AddAttr $tblCambios "EmailPrevio" "VARCHAR(150)" "NULL" | Out-Null
    AddAttr $tblCambios "DireccionPrevia" "VARCHAR(200)" "NULL" | Out-Null
    AddAttr $tblCambios "Activo" "BIT" "NOT NULL" | Out-Null
    AddAttr $tblCambios "TipoOperacion" "VARCHAR(20)" "NOT NULL" | Out-Null
    AddAttr $tblCambios "DniUsuario" "VARCHAR(50)" "FK" | Out-Null
    AddAttr $tblCambios "Act" "BIT" "NOT NULL" | Out-Null

    # Relationships / Associations
    AddAssociation $tblPac $tblTel "FK_Paciente_Telefonos" "1" "0..*" | Out-Null
    AddAssociation $tblPac $tblMail "FK_Paciente_Emails" "1" "0..*" | Out-Null
    AddAssociation $tblPac $tblCambios "FK_Pacientes_Cambios" "1" "0..*" | Out-Null
    AddAssociation $tblPac $tblConsultas "FK_Consultas_Pacientes" "1" "0..*" | Out-Null
    AddAssociation $tblConsultas $tblConsCambios "FK_Consultas_Cambios" "1" "0..*" | Out-Null
    AddAssociation $tblUser $tblCambios "FK_Usuarios_PacientesCambios" "1" "0..*" | Out-Null
    AddAssociation $tblUser $tblConsCambios "FK_Usuarios_ConsultasCambios" "1" "0..*" | Out-Null
    AddAssociation $tblUser $tblBit "FK_Usuarios_Bitacora" "1" "0..*" | Out-Null

    Write-Output "Tables and Foreign Key associations verified."

    # 3. Create or Update Diagram "Tablas Asociadas en la Base de Datos"
    function GetOrCreateDiagramByName([object]$pkg, [string]$name, [string]$type) {
        foreach ($d in $pkg.Diagrams) {
            if ($d.Name -eq $name) { return $d }
        }
        $diag = $pkg.Diagrams.AddNew($name, $type)
        [void]$diag.Update()
        $pkg.Diagrams.Refresh()
        return $diag
    }

    function SetDiagramObject([object]$diag, [object]$elem, [int]$left, [int]$right, [int]$top, [int]$bottom, [int]$seq) {
        foreach ($do in $diag.DiagramObjects) {
            if ($do.ElementID -eq $elem.ElementID) {
                $do.left = $left
                $do.right = $right
                $do.top = $top
                $do.bottom = $bottom
                $do.Sequence = $seq
                [void]$do.Update()
                return $do
            }
        }
        $do = $diag.DiagramObjects.AddNew("l=$left;r=$right;t=$top;b=$bottom;", "")
        $do.ElementID = $elem.ElementID
        $do.left = $left
        $do.right = $right
        $do.top = $top
        $do.bottom = $bottom
        $do.Sequence = $seq
        [void]$do.Update()
        $diag.DiagramObjects.Refresh()
        return $do
    }

    $diagName = "Tablas Asociadas en la Base de Datos"
    $diag = GetOrCreateDiagramByName $pkgBitacora $diagName "Logical"
    Write-Output "Working on Diagram: $($diag.Name) (ID: $($diag.DiagramID))"

    # Set Layout Coordinates:
    # Col 1: Contactos
    SetDiagramObject $diag $tblTel 40 320 -60 -240 1 | Out-Null
    SetDiagramObject $diag $tblMail 40 320 -300 -480 2 | Out-Null

    # Col 2: Pacientes, Cambios, Consultas, Consultas_Cambios
    SetDiagramObject $diag $tblPac 400 700 -60 -360 3 | Out-Null
    SetDiagramObject $diag $tblCambios 400 700 -420 -760 4 | Out-Null
    SetDiagramObject $diag $tblConsultas 400 700 -820 -1100 5 | Out-Null
    SetDiagramObject $diag $tblConsCambios 400 700 -1160 -1480 6 | Out-Null

    # Col 3: Usuarios, Bitacora, DV
    SetDiagramObject $diag $tblUser 780 1060 -60 -300 7 | Out-Null
    SetDiagramObject $diag $tblBit 780 1060 -360 -560 8 | Out-Null
    SetDiagramObject $diag $tblDV 780 1060 -620 -760 9 | Out-Null

    [void]$ea.SaveDiagram($diag.DiagramID)
    Write-Output "Diagram saved successfully."

    # Export to PNG image
    $outDir = "C:\Users\Navegador\Desktop\GIT\Diagramas\Exported"
    if (-not (Test-Path $outDir)) { New-Item -ItemType Directory -Path $outDir | Out-Null }
    $cleanName = ($diag.Name -replace "[:/\\]", "_")
    $imgFile = Join-Path $outDir "$cleanName.png"
    $project = $ea.GetProjectInterface()
    $res = $project.PutDiagramImageToFile($diag.DiagramGUID, $imgFile, 1)
    Write-Output "Exported Diagram image to $imgFile : Result = $res"

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message) at line $($_.InvocationInfo.ScriptLineNumber)"
}
