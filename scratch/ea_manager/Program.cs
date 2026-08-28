using System;
using System.Data.OleDb;

class Program
{
    static void Main()
    {
        string dbPath = @"c:\Users\Navegador\Desktop\td\Diagramas\TD.EAP";
        string connStr = $@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={dbPath};";

        using var conn = new OleDbConnection(connStr);
        conn.Open();

        using (var cmd = new OleDbCommand("SELECT Diagram_ID, Name, Diagram_Type, Package_ID, ParentID FROM t_diagram WHERE Diagram_ID = 28", conn))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                int diagId = Convert.ToInt32(reader["Diagram_ID"]);
                string name = reader["Name"].ToString()!;
                string type = reader["Diagram_Type"].ToString()!;

                using var cmdObj = new OleDbCommand($"SELECT COUNT(*) FROM t_diagramobjects WHERE Diagram_ID = {diagId}", conn);
                int objCount = Convert.ToInt32(cmdObj.ExecuteScalar());

                using var cmdLnk = new OleDbCommand($"SELECT COUNT(*) FROM t_diagramlinks WHERE DiagramID = {diagId}", conn);
                int lnkCount = Convert.ToInt32(cmdLnk.ExecuteScalar());

                Console.WriteLine($"Diagram ID {diagId} [{type}]: '{name}' -> {objCount} Objects, {lnkCount} Links.");
            }
        }
    }
}
