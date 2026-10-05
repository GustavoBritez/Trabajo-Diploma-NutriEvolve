import win32com.client

def check_table_conns():
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    try:
        t_ids = "430, 431, 435, 436"
        res = ea.SQLQuery(f"SELECT Connector_ID, Name, Connector_Type, SubType, Start_Object_ID, End_Object_ID, SourceCard, DestCard, SourceRole, DestRole FROM t_connector WHERE Start_Object_ID IN ({t_ids}) OR End_Object_ID IN ({t_ids})")
        print("Table Connectors:")
        print(res)
    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    check_table_conns()
