import win32com.client
import xml.etree.ElementTree as ET

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

sql_style = """
UPDATE t_diagram
SET ShowForeign = 0,
    ShowPackageContents = 0,
    StyleEx = 'ExcludeRTF=0;DocAll=0;HideQuals=0;AttPkg=1;ShowTests=0;ShowMaint=0;SuppressFOC=1;MatrixActive=0;SwimlanesActive=1;KanbanActive=0;MatrixLineWidth=1;MatrixLocked=0;TConnectorNotation=UML 2.1;TExplicitNavigability=0;AdvancedElementProps=1;AdvancedFeatureProps=1;AdvancedConnectorProps=1;ProfileData=;MDGDgm=;STBLDgm=;ShowNotes=0;VisibleAttributeDetail=0;ShowOpRetType=1;SuppressBrackets=0;SuppConnectorLabels=0;PrintPageHeadFoot=0;ShowAsList=0;SuppressedCompartments=;SaveTag=EB4905C2;',
    PDATA = 'HideRel=0;ShowTags=0;ShowReqs=0;ShowCons=0;OpParams=1;ShowSN=0;ScalePI=0;PPgs.cx=2;PPgs.cy=1;PSize=9;ShowIcons=1;SuppCN=0;HideProps=0;HideParents=0;UseAlias=0;HideAtts=0;HideOps=0;HideStereo=0;HideEStereo=0;FormName=;'
WHERE Diagram_ID = 93
"""
ea.Execute(sql_style)

# Check
xml = ea.SQLQuery("SELECT Diagram_ID, ShowPackageContents, ShowForeign, StyleEx, PDATA FROM t_diagram WHERE Diagram_ID = 93")
root = ET.fromstring(xml)
row = root.find('.//Row')
for col in row:
    print(col.tag, ":", col.text[:50] if col.text else None)

# Now export
proj = ea.GetProjectInterface()
diag = ea.GetDiagramByID(93)
proj.PutDiagramImageToFile(diag.DiagramGUID, r"C:\Users\Danie\Desktop\GIT\TD\scratch\test_d93_fixed.png", 1)
print("Exported test_d93_fixed.png")

ea.CloseFile()
