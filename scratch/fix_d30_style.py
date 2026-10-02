import win32com.client
import xml.etree.ElementTree as ET

def test():
    ea = win32com.client.Dispatch('EA.Repository')
    ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')
    proj = ea.GetProjectInterface()

    # Get StyleEx and PDATA from Diagram 25 via SQL
    xml = ea.SQLQuery("SELECT StyleEx, PDATA FROM t_diagram WHERE Diagram_ID = 25")
    root = ET.fromstring(xml)
    row = root.find('.//Row')
    s_ex = row.find('StyleEx').text or ''
    pdata = row.find('PDATA').text or ''

    print('s_ex:', s_ex)
    print('pdata:', pdata)

    s_ex_sql = s_ex.replace("'", "''")
    pdata_sql = pdata.replace("'", "''")
    ea.Execute(f"UPDATE t_diagram SET StyleEx = '{s_ex_sql}', PDATA = '{pdata_sql}' WHERE Diagram_ID = 30")

    d30 = ea.GetDiagramByID(30)
    d30.Update()
    ea.SaveDiagram(30)

    proj.PutDiagramImageToFile(d30.DiagramGUID, r'c:\Users\Danie\Desktop\GIT\TD\scratch\test_d30_copy25.png', 1)
    ea.CloseFile()
    ea.Exit()
    print('Exported test_d30_copy25.png')

if __name__ == '__main__':
    test()
