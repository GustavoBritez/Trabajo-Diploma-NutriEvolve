import win32com.client
import os
import xml.etree.ElementTree as ET

def get_or_create_package(ea, parent_pkg, name):
    for i in range(parent_pkg.Packages.Count):
        p = parent_pkg.Packages.GetAt(i)
        if p.Name == name or name.lower() in p.Name.lower():
            return p
    new_pkg = parent_pkg.Packages.AddNew(name, "Package")
    new_pkg.Update()
    parent_pkg.Packages.Refresh()
    print(f"Created Package: {name} (ID: {new_pkg.PackageID})")
    return new_pkg

def get_or_create_element(ea, pkg, name, elem_type, stereotype=""):
    sql = f"SELECT Object_ID FROM t_object WHERE Package_ID = {pkg.PackageID} AND Name = '{name.replace("'", "''")}' AND Object_Type = '{elem_type}'"
    xml_str = ea.SQLQuery(sql)
    root = ET.fromstring(xml_str)
    rows = root.findall(".//Row")
    if rows:
        obj_id = int(rows[0].find("Object_ID").text)
        el = ea.GetElementByID(obj_id)
        if stereotype and el.Stereotype != stereotype:
            el.Stereotype = stereotype
            el.Update()
        return el

    el = pkg.Elements.AddNew(name, elem_type)
    if stereotype:
        el.Stereotype = stereotype
    el.Update()
    pkg.Elements.Refresh()
    print(f"Created Element [{elem_type}] '{name}' (ID: {el.ElementID})")
    return el

def get_or_create_diagram(ea, pkg, name, diag_type):
    for i in range(pkg.Diagrams.Count):
        d = pkg.Diagrams.GetAt(i)
        if d.Name == name:
            return d
    # Check by SQL
    sql = f"SELECT Diagram_ID FROM t_diagram WHERE Package_ID = {pkg.PackageID} AND Name = '{name.replace("'", "''")}'"
    xml_str = ea.SQLQuery(sql)
    root = ET.fromstring(xml_str)
    rows = root.findall(".//Row")
    if rows:
        d_id = int(rows[0].find("Diagram_ID").text)
        return ea.GetDiagramByID(d_id)

    diag = pkg.Diagrams.AddNew(name, diag_type)
    diag.Update()
    pkg.Diagrams.Refresh()
    print(f"Created Diagram [{diag_type}] '{name}' (ID: {diag.DiagramID})")
    return diag

def add_attribute_if_not_exists(el, name, att_type, stereotype=""):
    for a in el.Attributes:
        if a.Name == name:
            return a
    a = el.Attributes.AddNew(name, att_type)
    if stereotype:
        a.Stereotype = stereotype
    a.Update()
    el.Attributes.Refresh()
    return a

def add_method_if_not_exists(el, name, return_type):
    for m in el.Methods:
        if m.Name == name:
            return m
    m = el.Methods.AddNew(name, return_type)
    m.ReturnType = return_type
    m.Update()
    el.Methods.Refresh()
    return m

def ensure_diagram_object(diag, el, left, top, right, bottom):
    # Check if object already on diagram
    for i in range(diag.DiagramObjects.Count):
        dobj = diag.DiagramObjects.GetAt(i)
        if dobj.ElementID == el.ElementID:
            dobj.left = left
            dobj.top = -top
            dobj.right = right
            dobj.bottom = -bottom
            dobj.Update()
            return dobj

    dobj = diag.DiagramObjects.AddNew(f"l={left};r={right};t={top};b={bottom};", "")
    dobj.ElementID = el.ElementID
    dobj.left = left
    dobj.top = -top
    dobj.right = right
    dobj.bottom = -bottom
    dobj.Update()
    diag.DiagramObjects.Refresh()
    return dobj

def get_or_create_connector(ea, src, tgt, conn_type, name="", stereotype="", subtype=""):
    # Check existing connector
    for c in src.Connectors:
        if c.SupplierID == tgt.ElementID and c.Type == conn_type and (not name or c.Name == name):
            if stereotype and c.Stereotype != stereotype:
                c.Stereotype = stereotype
                c.Update()
            return c
    c = src.Connectors.AddNew(name, conn_type)
    c.SupplierID = tgt.ElementID
    if stereotype:
        c.Stereotype = stereotype
    if subtype:
        c.SubType = subtype
    c.Update()
    src.Connectors.Refresh()
    return c

print("Helper definitions loaded successfully.")
