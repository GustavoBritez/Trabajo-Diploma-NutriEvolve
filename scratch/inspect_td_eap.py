import os
import sys
import json
import glob

# 1. Analyze TD.EAP tables and sequence messages
import win32com.client
import xml.etree.ElementTree as ET

def query_xml(ea, sql):
    try:
        xml_str = ea.SQLQuery(sql)
        root = ET.fromstring(xml_str)
        rows = []
        for row in root.findall('.//Row'):
            r = {}
            for elem in row:
                r[elem.tag] = elem.text
            rows.append(r)
        return rows
    except Exception as e:
        return [{"error": str(e)}]

ea = win32com.client.Dispatch('EA.Repository')
ea.OpenFile(r'C:\Users\Danie\Desktop\GIT\TD\Diagramas\TD.EAP')

# Let's inspect connector types
connector_types = query_xml(ea, "SELECT DISTINCT Connector_Type FROM t_connector")
print("Connector Types:", [c.get('Connector_Type') for c in connector_types])

# Sequence connectors / messages
seq_conns = query_xml(ea, "SELECT Connector_ID, Name, Connector_Type, SubType, Start_Object_ID, End_Object_ID, SeqNo, DiagramID FROM t_connector WHERE Connector_Type = 'Sequence'")
print(f"Sequence connectors found: {len(seq_conns)}")
for sc in seq_conns[:10]:
    print(sc)

# Diagrams
diagrams = query_xml(ea, "SELECT Diagram_ID, Name, Diagram_Type, Package_ID FROM t_diagram ORDER BY Diagram_Type, Name")
print("\n--- DIAGRAMS IN TD.EAP ---")
for d in diagrams:
    print(f"[{d.get('Diagram_Type')}] (ID: {d.get('Diagram_ID')}, Pkg: {d.get('Package_ID')}) {d.get('Name')}")

# Packages
packages = query_xml(ea, "SELECT Package_ID, Name, Parent_ID FROM t_package ORDER BY Package_ID")
print("\n--- PACKAGES IN TD.EAP ---")
for p in packages:
    print(f"(ID: {p.get('Package_ID')}, Parent: {p.get('Parent_ID')}) {p.get('Name')}")

# Use Cases
usecases = query_xml(ea, "SELECT Object_ID, Name, Note FROM t_object WHERE Object_Type = 'UseCase'")
print("\n--- USE CASES ---")
for u in usecases:
    print(f"- {u.get('Name')}")

# Classes
classes = query_xml(ea, "SELECT Object_ID, Name, Stereotype FROM t_object WHERE Object_Type = 'Class'")
print("\n--- CLASSES ---")
for c in classes:
    print(f"- {c.get('Name')} (Stereotype: {c.get('Stereotype')})")

ea.CloseFile()
ea.Exit()
