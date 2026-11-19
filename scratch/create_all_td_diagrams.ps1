$ErrorActionPreference = "Stop"

try {
    $ea = New-Object -ComObject EA.Repository
    $opened = $ea.OpenFile('C:\Users\Navegador\Desktop\GIT\Diagramas\TD.EAP')
    if (-not $opened) {
        Write-Output "Could not open TD.EAP"
        exit 1
    }

    Write-Output "TD.EAP opened."

    # Locate package 16 or find by name
    $pkgBitacora = $ea.GetPackageByID(16)
    if ($pkgBitacora -eq $null) {
        $pkgPrimary = $ea.GetPackageByID(4)
        $pkgBitacora = $pkgPrimary.Packages.AddNew("T06b - Bitacora de Cambios", "Package")
        [void]$pkgBitacora.Update()
        $pkgPrimary.Packages.Refresh()
    }
    Write-Output "Working in Package: $($pkgBitacora.Name) (ID: $($pkgBitacora.PackageID))"

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

    function AddMeth([object]$elem, [string]$name, [string]$retType) {
        foreach ($m in $elem.Methods) {
            if ($m.Name -eq $name) { return $m }
        }
        $newM = $elem.Methods.AddNew($name, $retType)
        $newM.ReturnType = $retType
        [void]$newM.Update()
        $elem.Methods.Refresh()
        return $newM
    }

    function AddConnector([object]$srcElem, [object]$tgtElem, [string]$type, [string]$name, [string]$stereo) {
        foreach ($c in $srcElem.Connectors) {
            if ($c.SupplierID -eq $tgtElem.ElementID -and $c.Type -eq $type -and $c.Name -eq $name) {
                return $c
            }
        }
        $conn = $srcElem.Connectors.AddNew($name, $type)
        $conn.SupplierID = $tgtElem.ElementID
        if ($stereo -ne "") { $conn.Stereotype = $stereo }
        [void]$conn.Update()
        $srcElem.Connectors.Refresh()
        return $conn
    }

    function AddSeqMessage([object]$diag, [object]$srcObj, [object]$tgtObj, [string]$name, [int]$seqNo, [string]$subType) {
        $conn = $srcObj.Connectors.AddNew($name, "Sequence")
        $conn.SupplierID = $tgtObj.ElementID
        $conn.DiagramID = $diag.DiagramID
        $conn.SequenceNo = $seqNo
        if ($subType -ne "") { $conn.SubType = $subType }
        [void]$conn.Update()
        $srcObj.Connectors.Refresh()
        return $conn
    }

    # 1. Classes & Elements
    Write-Output "--- Setting up Classes ---"
    $elemAES = GetOrCreateElement $pkgBitacora "ServicioCriptografiaAES" "Class" ""
    AddMeth $elemAES "Encriptar" "string" | Out-Null
    AddMeth $elemAES "Desencriptar" "string" | Out-Null

    $elemBE = GetOrCreateElement $pkgBitacora "PacienteCambioBE_DNI101" "Class" ""
    AddAttr $elemBE "IdCambio" "int" "" | Out-Null
    AddAttr $elemBE "IdPaciente" "int" "" | Out-Null
    AddAttr $elemBE "FechaHora" "DateTime" "" | Out-Null
    AddAttr $elemBE "DniNiño" "string" "" | Out-Null
    AddAttr $elemBE "Nombre" "string" "" | Out-Null
    AddAttr $elemBE "Apellido" "string" "" | Out-Null
    AddAttr $elemBE "EmailPrevio" "string?" "" | Out-Null
    AddAttr $elemBE "TelefonoPrevio" "string?" "" | Out-Null
    AddAttr $elemBE "DireccionPrevia" "string?" "" | Out-Null
    AddAttr $elemBE "TipoOperacion" "string" "" | Out-Null
    AddAttr $elemBE "DniUsuario" "string" "" | Out-Null
    AddAttr $elemBE "Act" "bool" "" | Out-Null

    $elemBLL = GetOrCreateElement $pkgBitacora "BitacoraCambiosConsultaBLL_DNI101" "Class" ""
    AddMeth $elemBLL "ListarCambiosPacientes" "List<PacienteCambioBE_DNI101>" | Out-Null
    AddMeth $elemBLL "RevertirCambioPaciente" "bool" | Out-Null
    AddMeth $elemBLL "ListarCambios" "List<ConsultaCambioBE_DNI101>" | Out-Null
    AddMeth $elemBLL "RevertirCambio" "bool" | Out-Null

    $elemDAL = GetOrCreateElement $pkgBitacora "BitacoraCambiosConsultaDAL_DNI101" "Class" ""
    AddMeth $elemDAL "ListarCambiosPacientes" "List<PacienteCambioBE_DNI101>" | Out-Null
    AddMeth $elemDAL "RevertirCambioPaciente" "bool" | Out-Null
    AddMeth $elemDAL "ListarCambios" "List<ConsultaCambioBE_DNI101>" | Out-Null
    AddMeth $elemDAL "RevertirCambio" "bool" | Out-Null

    # Existing classes
    $bllPac = $ea.GetElementByID(757) # PacienteBLL_DNI101
    AddMeth $bllPac "CifrarDniReversible" "string" | Out-Null
    AddMeth $bllPac "DescifrarDniReversible" "string" | Out-Null

    $dalPac = $ea.GetElementByID(760) # PacienteDAL_DNI101
    $bePac = $ea.GetElementByID(662)  # PacienteBE_DNI101
    $bllBit = $ea.GetElementByID(762) # BitacoraBLL
    $dalBit = $ea.GetElementByID(764) # BitacoraDAL
    $bllDV = $ea.GetElementByID(763)  # DigitoVerificadorBLL
    $dalDV = $ea.GetElementByID(765)  # DigitoVerificadorDAL

    # Dependencies for Class Diagram
    AddConnector $elemBLL $elemDAL "Dependency" "" "" | Out-Null
    AddConnector $elemBLL $elemBE "Dependency" "" "" | Out-Null
    AddConnector $elemBLL $bllBit "Dependency" "" "" | Out-Null
    AddConnector $elemBLL $bllDV "Dependency" "" "" | Out-Null
    AddConnector $bllPac $elemAES "Dependency" "usa criptografia reversible AES-256" "" | Out-Null
    AddConnector $bllPac $dalPac "Dependency" "" "" | Out-Null
    AddConnector $bllPac $bePac "Dependency" "" "" | Out-Null
    AddConnector $elemDAL $elemBE "Dependency" "" "" | Out-Null

    # 2. Tables for DER
    Write-Output "--- Setting up DER Tables ---"
    $tblCambios = GetOrCreateElement $pkgBitacora "Pacientes_Cambios_DNI101" "Class" "table"
    AddAttr $tblCambios "IdCambio" "INT" "PK" | Out-Null
    AddAttr $tblCambios "IdPaciente" "INT" "FK" | Out-Null
    AddAttr $tblCambios "FechaHora" "DATETIME" "NOT NULL" | Out-Null
    AddAttr $tblCambios "DniNiño" "VARCHAR(50)" "NOT NULL" | Out-Null
    AddAttr $tblCambios "Nombre" "VARCHAR(100)" "NOT NULL" | Out-Null
    AddAttr $tblCambios "Apellido" "VARCHAR(100)" "NOT NULL" | Out-Null
    AddAttr $tblCambios "EmailPrevio" "VARCHAR(150)" "NULL" | Out-Null
    AddAttr $tblCambios "TelefonoPrevio" "VARCHAR(50)" "NULL" | Out-Null
    AddAttr $tblCambios "DireccionPrevia" "VARCHAR(200)" "NULL" | Out-Null
    AddAttr $tblCambios "Activo" "BIT" "NOT NULL" | Out-Null
    AddAttr $tblCambios "TipoOperacion" "VARCHAR(20)" "NOT NULL" | Out-Null
    AddAttr $tblCambios "DniUsuario" "VARCHAR(50)" "NOT NULL" | Out-Null
    AddAttr $tblCambios "Act" "BIT" "NOT NULL" | Out-Null

    $tblTel = GetOrCreateElement $pkgBitacora "Paciente_Telefonos_DNI101" "Class" "table"
    AddAttr $tblTel "IdTelefono" "INT" "PK" | Out-Null
    AddAttr $tblTel "IdPaciente" "INT" "FK" | Out-Null
    AddAttr $tblTel "Telefono" "VARCHAR(50)" "NOT NULL" | Out-Null
    AddAttr $tblTel "Tipo" "VARCHAR(20)" "NULL" | Out-Null
    AddAttr $tblTel "EsPrincipal" "BIT" "NULL" | Out-Null

    $tblMail = GetOrCreateElement $pkgBitacora "Paciente_Emails_DNI101" "Class" "table"
    AddAttr $tblMail "IdEmail" "INT" "PK" | Out-Null
    AddAttr $tblMail "IdPaciente" "INT" "FK" | Out-Null
    AddAttr $tblMail "Email" "VARCHAR(150)" "NOT NULL" | Out-Null
    AddAttr $tblMail "Tipo" "VARCHAR(20)" "NULL" | Out-Null
    AddAttr $tblMail "EsPrincipal" "BIT" "NULL" | Out-Null

    $tblPac = $ea.GetElementByID(430)  # Pacientes_DNI101
    $tblUser = $ea.GetElementByID(431) # Usuarios
    $tblBit = $ea.GetElementByID(436)  # Bitacora
    $tblDV = $ea.GetElementByID(435)   # DV

    AddConnector $tblPac $tblCambios "Association" "FK_PacientesCambios_Pacientes" "" | Out-Null
    AddConnector $tblPac $tblTel "Association" "FK_PacienteTelefonos_Pacientes" "" | Out-Null
    AddConnector $tblPac $tblMail "Association" "FK_PacienteEmails_Pacientes" "" | Out-Null
    AddConnector $tblUser $tblCambios "Association" "FK_PacientesCambios_Usuarios" "" | Out-Null

    Write-Output "Classes and Tables created successfully."

    # 3. Create or Update Diagrams
    function GetOrCreateDiagram([object]$pkg, [string]$name, [string]$type) {
        foreach ($d in $pkg.Diagrams) {
            if ($d.Name -eq $name) { return $d }
        }
        $diag = $pkg.Diagrams.AddNew($name, $type)
        [void]$diag.Update()
        $pkg.Diagrams.Refresh()
        return $diag
    }

    function AddDiagramObject([object]$diag, [object]$elem, [int]$left, [int]$right, [int]$top, [int]$bottom, [int]$seq) {
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

    # DIAGRAM 1: Modelo de Clases
    Write-Output "--- Creating Diagram: Modelo de Clases ---"
    $diagClases = GetOrCreateDiagram $pkgBitacora "T06b: Bitacora de Cambios - Diagrama de Clases" "Logical"
    AddDiagramObject $diagClases $elemBE 30 310 -50 -320 1 | Out-Null
    AddDiagramObject $diagClases $bePac 340 580 -50 -260 2 | Out-Null
    AddDiagramObject $diagClases $elemAES 620 920 -50 -150 3 | Out-Null
    AddDiagramObject $diagClases $elemBLL 30 430 -380 -500 4 | Out-Null
    AddDiagramObject $diagClases $bllPac 460 880 -380 -510 5 | Out-Null
    AddDiagramObject $diagClases $bllBit 920 1150 -280 -360 6 | Out-Null
    AddDiagramObject $diagClases $bllDV 920 1150 -440 -530 7 | Out-Null
    AddDiagramObject $diagClases $elemDAL 30 430 -580 -700 8 | Out-Null
    AddDiagramObject $diagClases $dalPac 460 880 -580 -690 9 | Out-Null
    AddDiagramObject $diagClases $dalBit 920 1150 -580 -670 10 | Out-Null
    AddDiagramObject $diagClases $dalDV 920 1150 -700 -800 11 | Out-Null
    [void]$ea.SaveDiagram($diagClases.DiagramID)

    # DIAGRAM 2: DER
    Write-Output "--- Creating Diagram: DER (Modelo Relacional) ---"
    $diagDER = GetOrCreateDiagram $pkgBitacora "T06b: Bitacora de Cambios - DER (Modelo Relacional)" "Logical"
    AddDiagramObject $diagDER $tblPac 50 300 -50 -300 1 | Out-Null
    AddDiagramObject $diagDER $tblCambios 370 680 -50 -350 2 | Out-Null
    AddDiagramObject $diagDER $tblTel 50 280 -370 -520 3 | Out-Null
    AddDiagramObject $diagDER $tblMail 50 280 -560 -710 4 | Out-Null
    AddDiagramObject $diagDER $tblUser 370 600 -430 -690 5 | Out-Null
    AddDiagramObject $diagDER $tblBit 720 900 -250 -430 6 | Out-Null
    AddDiagramObject $diagDER $tblDV 720 880 -80 -180 7 | Out-Null
    [void]$ea.SaveDiagram($diagDER.DiagramID)

    # DIAGRAM 3: Diagrama de Secuencia
    Write-Output "--- Creating Diagram: Diagrama de Secuencia ---"
    $diagSeq = GetOrCreateDiagram $pkgBitacora "T06b: Bitacora de Cambios - Diagrama de Secuencia" "Sequence"
    
    # Create Lifeline Objects for sequence diagram
    $actAdmin = GetOrCreateElement $pkgBitacora "Administrador" "Actor" ""
    $objGUI = GetOrCreateElement $pkgBitacora "GUI : frmBitacoraCambios / frmMaestroClientes" "Sequence" "boundary"
    $objCrypto = GetOrCreateElement $pkgBitacora "ServicioCriptografiaAES" "Object" ""
    $objPacBLL = GetOrCreateElement $pkgBitacora "obj_PacienteBLL" "Object" ""
    $objPacBLL.Name = "PacienteBLL_DNI101"
    [void]$objPacBLL.Update()
    $objBitBLL = GetOrCreateElement $pkgBitacora "obj_BitacoraCambiosBLL" "Object" ""
    $objBitBLL.Name = "BitacoraCambiosConsultaBLL_DNI101"
    [void]$objBitBLL.Update()
    $objDAL = GetOrCreateElement $pkgBitacora "obj_DAL_SQL" "Object" ""
    $objDAL.Name = "DAL / SQL Server"
    [void]$objDAL.Update()
    $objTrigger = GetOrCreateElement $pkgBitacora "obj_Trigger" "Object" ""
    $objTrigger.Name = "trg_Pacientes_BitacoraCambios"
    [void]$objTrigger.Update()
    $objBitacora = GetOrCreateElement $pkgBitacora "obj_BitacoraBLL" "Object" ""
    $objBitacora.Name = "BitacoraBLL"
    [void]$objBitacora.Update()
    $objDV = GetOrCreateElement $pkgBitacora "obj_DigitoVerificadorBLL" "Object" ""
    $objDV.Name = "DigitoVerificadorBLL"
    [void]$objDV.Update()

    # Place on diagram
    AddDiagramObject $diagSeq $actAdmin 20 110 -40 -1600 1 | Out-Null
    AddDiagramObject $diagSeq $objGUI 150 260 -40 -1600 2 | Out-Null
    AddDiagramObject $diagSeq $objCrypto 300 440 -40 -1600 3 | Out-Null
    AddDiagramObject $diagSeq $objPacBLL 480 620 -40 -1600 4 | Out-Null
    AddDiagramObject $diagSeq $objBitBLL 660 840 -40 -1600 5 | Out-Null
    AddDiagramObject $diagSeq $objDAL 880 1000 -40 -1600 6 | Out-Null
    AddDiagramObject $diagSeq $objTrigger 1040 1200 -40 -1600 7 | Out-Null
    AddDiagramObject $diagSeq $objBitacora 1240 1350 -40 -1600 8 | Out-Null
    AddDiagramObject $diagSeq $objDV 1390 1520 -40 -1600 9 | Out-Null

    # Sequence Messages
    # Clean previous sequence connectors for this diagram
    $existingConns = $ea.SQLQuery("SELECT Connector_ID FROM t_connector WHERE DiagramID = $($diagSeq.DiagramID)")
    [xml]$connDoc = $existingConns
    foreach ($cRow in $connDoc.SelectNodes("//Row")) {
        $cId = [int]$cRow.SelectSingleNode("Connector_ID").InnerText
        $cObj = $ea.GetConnectorByID($cId)
        # Delete if needed
    }

    $seqMessages = @(
        @{ Src=$actAdmin; Tgt=$objGUI; Name="btnRevertirDni_Click()"; Sub="SynchCall" },
        @{ Src=$objGUI; Tgt=$objPacBLL; Name="CifrarDniReversible(dni) / Descifrar(dniHex)"; Sub="SynchCall" },
        @{ Src=$objPacBLL; Tgt=$objCrypto; Name="Encriptar(dni) / Desencriptar(dniHex)"; Sub="SynchCall" },
        @{ Src=$objCrypto; Tgt=$objPacBLL; Name="retorna valor cifrado AES-256 / plano"; Sub="Return" },
        @{ Src=$objPacBLL; Tgt=$objGUI; Name="actualiza campo txtDni con AES-256 / DNI"; Sub="Return" },
        @{ Src=$actAdmin; Tgt=$objGUI; Name="btnModificar_Click(paciente)"; Sub="SynchCall" },
        @{ Src=$objGUI; Tgt=$objPacBLL; Name="Modificar(paciente)"; Sub="SynchCall" },
        @{ Src=$objPacBLL; Tgt=$objDAL; Name="Modificar(paciente)"; Sub="SynchCall" },
        @{ Src=$objDAL; Tgt=$objTrigger; Name="UPDATE Pacientes_DNI101 SET Nombre, Telefono, Email..."; Sub="SynchCall" },
        @{ Src=$objTrigger; Tgt=$objTrigger; Name="Validar cambio en negocio (ignora recálculo de DV)"; Sub="SynchCall" },
        @{ Src=$objTrigger; Tgt=$objTrigger; Name="SET Activo=0 (anteriores) & INSERT Pacientes_Cambios (Activo=1)"; Sub="SynchCall" },
        @{ Src=$objPacBLL; Tgt=$objBitacora; Name="RegistrarBitacora(2, 'Modificación Paciente', 'MaestroClientes')"; Sub="SynchCall" },
        @{ Src=$objPacBLL; Tgt=$objDV; Name="RecalcularYPersistir()"; Sub="SynchCall" },
        @{ Src=$actAdmin; Tgt=$objGUI; Name="Abrir Bitacora de Cambios (Menú Principal)"; Sub="SynchCall" },
        @{ Src=$objGUI; Tgt=$objGUI; Name="OnVisibleChanged() -> RefrescarDatos() [Auto-Carga sin requerir 'Limpiar']"; Sub="SynchCall" },
        @{ Src=$objGUI; Tgt=$objBitBLL; Name="ListarCambiosPacientes(null, null, null, null, null)"; Sub="SynchCall" },
        @{ Src=$objBitBLL; Tgt=$objDAL; Name="ListarCambiosPacientes(...) -> SELECT ORDER BY FechaHora DESC"; Sub="SynchCall" },
        @{ Src=$objDAL; Tgt=$objGUI; Name="Retorna historial y enlaza dgvCambiosPacientes"; Sub="Return" },
        @{ Src=$actAdmin; Tgt=$objGUI; Name="btnActivar_Click(versionSeleccionada)"; Sub="SynchCall" },
        @{ Src=$objGUI; Tgt=$objBitBLL; Name="RevertirCambioPaciente(idCambio, dniAdmin)"; Sub="SynchCall" },
        @{ Src=$objBitBLL; Tgt=$objDAL; Name="RevertirCambioPaciente(idCambio)"; Sub="SynchCall" },
        @{ Src=$objDAL; Tgt=$objDAL; Name="Restaurar datos en Pacientes_DNI101 & Sincronizar Contactos"; Sub="SynchCall" },
        @{ Src=$objDAL; Tgt=$objDAL; Name="UPDATE Pacientes_Cambios: Act=1 (elegida), Act=0 (demas)"; Sub="SynchCall" },
        @{ Src=$objBitBLL; Tgt=$objBitacora; Name="RegistrarBitacora(2, 'Activación de versión histórica', 'BitacoraCambios')"; Sub="SynchCall" },
        @{ Src=$objBitBLL; Tgt=$objDV; Name="RecalcularYPersistir()"; Sub="SynchCall" },
        @{ Src=$objBitBLL; Tgt=$objGUI; Name="Refrescar vista: Versión activa queda en Act=True"; Sub="Return" }
    )

    $sNum = 1
    foreach ($m in $seqMessages) {
        AddSeqMessage $diagSeq $m.Src $m.Tgt $m.Name $sNum $m.Sub | Out-Null
        $sNum++
    }
    [void]$ea.SaveDiagram($diagSeq.DiagramID)

    # DIAGRAM 4: GUI Bitacora de Cambios
    Write-Output "--- Creating Diagram: GUI (Diseño de Pantalla) ---"
    $diagGUI = GetOrCreateDiagram $pkgBitacora "T06b: Bitacora de Cambios - GUI (Diseño de Pantalla)" "Custom"

    $formScreen = GetOrCreateElement $pkgBitacora "frmBitacoraCambiosConsultas" "Screen" ""
    $pnlHeader = GetOrCreateElement $pkgBitacora "pnlHeader [Titulo, Subtitulo, cmbModulo, btnAyuda]" "Boundary" ""
    $gbGrilla = GetOrCreateElement $pkgBitacora "gbGrilla [dgvCambiosPacientes: Nro, Fecha, Hora, DNI, Cliente, Email, Tel, Dir, Operación, DNI Usuario, Act, Activar]" "Boundary" ""
    $gbFiltros = GetOrCreateElement $pkgBitacora "gbControles [DNI Cliente, Nombre/Apellido, Fecha Ini (Unchecked), Operacion, Botones APLICAR, LIMPIAR, SALIR, ACTIVAR]" "Boundary" ""
    $noteAutoCarga = GetOrCreateElement $pkgBitacora "Nota: Carga Automatica Inmediata en OnVisibleChanged / RefrescarDatos sin necesidad de clic en Limpiar" "Note" ""

    AddDiagramObject $diagGUI $formScreen 30 850 -30 -620 1 | Out-Null
    AddDiagramObject $diagGUI $pnlHeader 50 830 -50 -120 2 | Out-Null
    AddDiagramObject $diagGUI $gbGrilla 50 830 -130 -440 3 | Out-Null
    AddDiagramObject $diagGUI $gbFiltros 50 830 -450 -590 4 | Out-Null
    AddDiagramObject $diagGUI $noteAutoCarga 880 1200 -130 -300 5 | Out-Null

    [void]$ea.SaveDiagram($diagGUI.DiagramID)

    Write-Output "ALL 4 DIAGRAMS CREATED AND SAVED IN TD.EAP SUCCESSFULLY!"

    $ea.CloseFile()
    $ea.Exit()
} catch {
    Write-Output "Error: $($_.Exception.Message) at line $($_.InvocationInfo.ScriptLineNumber)"
}
