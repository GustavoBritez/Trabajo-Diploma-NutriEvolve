import win32com.client

def check_frag_xref():
    ea = win32com.client.Dispatch("EA.Repository")
    ea.OpenFile(r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP")
    try:
        res = ea.SQLQuery("SELECT XrefID, Name, Type, Description, Client FROM t_xref WHERE Name = 'Partitions'")
        print("XREF Partitions:")
        print(res)
    finally:
        ea.CloseFile()
        try:
            ea.Exit()
        except:
            pass

if __name__ == "__main__":
    check_frag_xref()
