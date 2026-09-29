import os

def create_drawio(filename, title, swimlanes, steps):
    # This is a simplified drawio generator just for illustration.
    # In practice, generating a complex drawio with correct geometry is hard.
    # I'll create a basic xml structure for draw.io that opens correctly.
    xml = """<mxfile host="Electron" modified="2023-10-27T00:00:00.000Z" agent="Mozilla/5.0" version="22.0.4" type="device">
  <diagram id="diag1" name="Page-1">
    <mxGraphModel dx="1000" dy="1000" grid="1" gridSize="10" guides="1" tooltips="1" connect="1" arrows="1" fold="1" page="1" pageScale="1" pageWidth="827" pageHeight="1169" math="0" shadow="0">
      <root>
        <mxCell id="0" />
        <mxCell id="1" parent="0" />
"""
    y_offset = 40
    xml += f"""        <mxCell id="pool1" value="{title}" style="swimlane;childLayout=stackLayout;resizeParent=1;resizeParentMax=0;startSize=20;horizontal=0;horizontalStack=1;" vertex="1" parent="1">
          <mxGeometry x="40" y="40" width="600" height="{40 + len(steps)*60}" as="geometry" />
        </mxCell>
"""
    
    xml += f"""        <mxCell id="swimlane_paciente" value="Paciente" style="swimlane;startSize=20;horizontal=1;" vertex="1" parent="pool1">
          <mxGeometry x="20" y="0" width="290" height="{40 + len(steps)*60}" as="geometry" />
        </mxCell>
"""
    xml += f"""        <mxCell id="swimlane_nutricionista" value="Nutricionista" style="swimlane;startSize=20;horizontal=1;" vertex="1" parent="pool1">
          <mxGeometry x="310" y="0" width="290" height="{40 + len(steps)*60}" as="geometry" />
        </mxCell>
"""
    
    for i, step in enumerate(steps):
        # assign alternatively to demonstrate
        parent = "swimlane_paciente" if i % 2 == 0 else "swimlane_nutricionista"
        xml += f"""        <mxCell id="step_{i}" value="{step}" style="rounded=1;whiteSpace=wrap;html=1;" vertex="1" parent="{parent}">
          <mxGeometry x="40" y="{40 + i*60}" width="200" height="40" as="geometry" />
        </mxCell>
"""
    
    xml += """      </root>
    </mxGraphModel>
  </diagram>
</mxfile>"""
    
    with open(filename, "w", encoding="utf-8") as f:
        f.write(xml)

pn1_steps = [
    "Paciente envía datos para turno",
    "Nutricionista carga/registra paciente",
    "Reprogramar turno si hay superposición",
    "Seleccionar turno en agenda",
    "Actualizar agenda a Ocupado",
    "Modificar turno si paciente solicita cambios",
    "Actualizar agenda a Disponible si cancela",
    "Registrar estado final (Asistió/Ausente/Cancelado)"
]
create_drawio("C:/Users/Danie/Desktop/GIT/TD/Diagramas/Diagrama_Proceso_PN1.drawio", "Proceso PN1: Turnero Nutricional", ["Paciente", "Nutricionista"], pn1_steps)

pn2_steps = [
    "Seleccionar paciente en sistema local",
    "Registrar datos antropométricos",
    "Calcular IMC y Z-Scores",
    "Generar patrón de crecimiento (percentiles)",
    "Generar diagnóstico nutricional",
    "Emitir Alerta Clínica si hay caída",
    "Registrar antecedentes y recordatorio",
    "Prescribir plan alimentario",
    "Guardar historia clínica"
]
create_drawio("C:/Users/Danie/Desktop/GIT/TD/Diagramas/Diagrama_Proceso_PN2.drawio", "Proceso PN2: Seguimiento Nutricional", ["Paciente", "Nutricionista"], pn2_steps)

print("Draw.io files created.")
