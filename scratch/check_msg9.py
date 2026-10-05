import win32com.client

def check_msg9():
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    try:
        # Check connector 28 from diag 12
        res12 = ea.SQLQuery("SELECT Connector_ID, Name, SubType, LineStyle, RouteStyle, LineColor, PDATA1, PDATA2, PDATA3, PDATA4 FROM t_connector WHERE DiagramID = 12 AND SeqNo = 28")
        print("Diag 12 Seq 28 (TurnoBE creation):")
        print(res12)

        # Check connector 9 from diag 56
        res56 = ea.SQLQuery("SELECT Connector_ID, Name, SubType, LineStyle, RouteStyle, LineColor, PDATA1, PDATA2, PDATA3, PDATA4 FROM t_connector WHERE DiagramID = 56 AND SeqNo = 9")
        print("Diag 56 Seq 9 (PacienteBE creation):")
        print(res56)
    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    check_msg9()
