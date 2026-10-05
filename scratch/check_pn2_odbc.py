import pyodbc

conn_str = r'DRIVER={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP;'
conn = pyodbc.connect(conn_str)
cursor = conn.cursor()

print("=== PACKAGES ===")
for row in cursor.execute("SELECT Package_ID, Name, Parent_ID FROM t_package"):
    print(row)

print("\n=== USE CASES / OBJECTS WITH CUN OR SEGUIMIENTO ===")
for row in cursor.execute("SELECT Object_ID, Name, Object_Type, Package_ID FROM t_object WHERE Name LIKE '%CUN%' OR Name LIKE '%Seguimiento%' OR Name LIKE '%Consulta%' OR Name LIKE '%Prescribir%' OR Name LIKE '%Graficar%'"):
    print(row)

print("\n=== DIAGRAMS ===")
for row in cursor.execute("SELECT Diagram_ID, Name, Diagram_Type, Package_ID FROM t_diagram"):
    print(row)

conn.close()
