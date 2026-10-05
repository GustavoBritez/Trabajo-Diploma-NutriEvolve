import win32com.client

def check_68_conns():
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    try:
        eids = "417, 418, 419, 420, 421, 422, 423, 424, 425, 426"
        res = ea.SQLQuery(f"SELECT Connector_ID, Name, Connector_Type, SubType, Stereotype, Start_Object_ID, End_Object_ID FROM t_connector WHERE Start_Object_ID IN ({eids}) AND End_Object_ID IN ({eids})")
        print("Diagram 68 Connectors:")
        print(res)
    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    check_68_conns()
