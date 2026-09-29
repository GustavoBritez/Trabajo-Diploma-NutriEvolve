import os

def generate_pn2_drawio():
    xml = """<mxfile host="Electron" modified="2023-10-27T00:00:00.000Z" agent="Mozilla/5.0" version="22.0.4" type="device">
  <diagram id="pn2" name="Proceso PN2">
    <mxGraphModel dx="1200" dy="1200" grid="1" gridSize="10" guides="1" tooltips="1" connect="1" arrows="1" fold="1" page="1" pageScale="1" pageWidth="1169" pageHeight="827" math="0" shadow="0">
      <root>
        <mxCell id="0" />
        <mxCell id="1" parent="0" />
        
        <!-- Pool -->
        <mxCell id="pool" value="PN2: Seguimiento Nutricional" style="swimlane;childLayout=stackLayout;horizontal=0;horizontalStack=1;startSize=30;" vertex="1" parent="1">
          <mxGeometry x="40" y="40" width="1000" height="900" as="geometry" />
        </mxCell>
        
        <!-- Calles -->
        <mxCell id="lane_paciente" value="PACIENTE" style="swimlane;horizontal=1;startSize=30;" vertex="1" parent="pool">
          <mxGeometry x="30" y="0" width="300" height="900" as="geometry" />
        </mxCell>
        
        <mxCell id="lane_nutricionista" value="NUTRICIONISTA" style="swimlane;horizontal=1;startSize=30;" vertex="1" parent="pool">
          <mxGeometry x="330" y="0" width="350" height="900" as="geometry" />
        </mxCell>
        
        <mxCell id="lane_db" value="ALMACENAMIENTO DE DATOS NUTRIEVOLVE" style="swimlane;horizontal=1;startSize=30;" vertex="1" parent="pool">
          <mxGeometry x="680" y="0" width="320" height="900" as="geometry" />
        </mxCell>

        <!-- Nodos PACIENTE -->
        <mxCell id="p_antecedentes" value="Antecedentes y hábitos" style="shape=note;whiteSpace=wrap;html=1;backgroundOutline=1;darkOpacity=0.05;size=15;" vertex="1" parent="lane_paciente">
          <mxGeometry x="70" y="440" width="150" height="40" as="geometry" />
        </mxCell>
        
        <mxCell id="p_plan_recibido" value="Plan alimentario&#xa;recibido" style="shape=note;whiteSpace=wrap;html=1;backgroundOutline=1;darkOpacity=0.05;size=15;" vertex="1" parent="lane_paciente">
          <mxGeometry x="70" y="700" width="150" height="50" as="geometry" />
        </mxCell>

        <mxCell id="end_paciente" value="" style="ellipse;html=1;shape=endState;fillColor=#000000;strokeColor=#000000;" vertex="1" parent="lane_paciente">
          <mxGeometry x="130" y="790" width="30" height="30" as="geometry" />
        </mxCell>

        <!-- Nodos NUTRICIONISTA -->
        <!-- We assume a start node for Nutricionista if it's the beginning of PN2 -->
        <mxCell id="start_nutri" value="" style="ellipse;fillColor=#000000;strokeColor=#000000;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="160" y="50" width="30" height="30" as="geometry" />
        </mxCell>

        <mxCell id="n_seleccionar" value="Seleccionar paciente local&#xa;y desplegar Historia Clínica" style="rounded=1;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="80" y="110" width="190" height="50" as="geometry" />
        </mxCell>
        
        <mxCell id="n_registrar_antropo" value="Registrar mediciones antropométricas" style="rounded=1;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="80" y="190" width="190" height="40" as="geometry" />
        </mxCell>
        
        <mxCell id="n_calcular_imc" value="Calcular IMC y Z-Scores OMS" style="rounded=1;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="80" y="260" width="190" height="40" as="geometry" />
        </mxCell>
        
        <mxCell id="n_generar_diag" value="Generar diagnóstico nutricional&#xa;según patrones de la OMS" style="rounded=1;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="80" y="330" width="190" height="50" as="geometry" />
        </mxCell>
        
        <mxCell id="d_caida" value="¿Caída de&#xa;percentil?" style="rhombus;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="110" y="410" width="130" height="60" as="geometry" />
        </mxCell>
        
        <mxCell id="n_alerta" value="Emitir Alerta Clínica" style="rounded=1;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="230" y="490" width="110" height="40" as="geometry" />
        </mxCell>
        
        <mxCell id="n_anamnesis" value="Registrar anamnesis, antecedentes&#xa;clínicos y recordatorio 24h" style="rounded=1;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="70" y="550" width="200" height="50" as="geometry" />
        </mxCell>
        
        <mxCell id="n_prescribir" value="Prescribir plan alimentario" style="rounded=1;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="80" y="630" width="190" height="40" as="geometry" />
        </mxCell>
        
        <mxCell id="n_guardar" value="Guardar Historia Clínica local" style="rounded=1;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="80" y="700" width="190" height="40" as="geometry" />
        </mxCell>

        <!-- Nodos DB -->
        <mxCell id="db_historia" value="HISTORIA CLÍNICA" style="shape=folder;fontStyle=1;tabWidth=110;tabHeight=20;tabPosition=left;html=1;boundedLbl=1;" vertex="1" parent="lane_db">
          <mxGeometry x="70" y="110" width="160" height="50" as="geometry" />
        </mxCell>
        
        <mxCell id="db_patrones" value="PATRONES OMS" style="shape=cylinder3;whiteSpace=wrap;html=1;boundedLbl=1;backgroundOutline=1;size=15;fontStyle=1;" vertex="1" parent="lane_db">
          <mxGeometry x="90" y="250" width="130" height="60" as="geometry" />
        </mxCell>
        
        <mxCell id="db_planes" value="PLANES ALIMENTARIOS" style="shape=folder;fontStyle=1;tabWidth=130;tabHeight=20;tabPosition=left;html=1;boundedLbl=1;" vertex="1" parent="lane_db">
          <mxGeometry x="70" y="625" width="160" height="50" as="geometry" />
        </mxCell>
        
        <!-- CONNECTIONS -->
        <mxCell id="c0" edge="1" parent="1" source="start_nutri" target="n_seleccionar">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="c1" edge="1" parent="1" source="n_seleccionar" target="n_registrar_antropo">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="c2" edge="1" parent="1" source="n_registrar_antropo" target="n_calcular_imc">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="c3" edge="1" parent="1" source="n_calcular_imc" target="n_generar_diag">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="c4" edge="1" parent="1" source="n_generar_diag" target="d_caida">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="c5" value="Sí" edge="1" parent="1" source="d_caida" target="n_alerta">
          <mxGeometry relative="1" as="geometry">
            <Array as="points">
              <mxPoint x="645" y="480" />
            </Array>
          </mxGeometry>
        </mxCell>
        
        <mxCell id="c6" value="No" edge="1" parent="1" source="d_caida" target="n_anamnesis">
          <mxGeometry relative="1" as="geometry">
             <Array as="points">
              <mxPoint x="545" y="520" />
            </Array>
          </mxGeometry>
        </mxCell>
        
        <mxCell id="c7" edge="1" parent="1" source="n_alerta" target="n_anamnesis">
          <mxGeometry relative="1" as="geometry">
            <Array as="points">
              <mxPoint x="645" y="575" />
            </Array>
          </mxGeometry>
        </mxCell>
        
        <mxCell id="c8" edge="1" parent="1" source="p_antecedentes" target="n_anamnesis">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="c9" edge="1" parent="1" source="n_anamnesis" target="n_prescribir">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="c10" edge="1" parent="1" source="n_prescribir" target="n_guardar">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="c11" edge="1" parent="1" source="n_guardar" target="p_plan_recibido">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="c12" edge="1" parent="1" source="p_plan_recibido" target="end_paciente">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <!-- DB Connections (dashed) -->
        <mxCell id="db_c1" edge="1" parent="1" source="n_seleccionar" target="db_historia" style="dashed=1;">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="db_c2" edge="1" parent="1" source="n_calcular_imc" target="db_patrones" style="dashed=1;strokeColor=#0000FF;">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="db_c3" edge="1" parent="1" source="n_prescribir" target="db_planes" style="dashed=1;">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="db_c4" edge="1" parent="1" source="n_guardar" target="db_historia" style="dashed=1;">
          <mxGeometry relative="1" as="geometry">
            <Array as="points">
              <mxPoint x="830" y="760" />
            </Array>
          </mxGeometry>
        </mxCell>

      </root>
    </mxGraphModel>
  </diagram>
</mxfile>"""

    filepath = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagrama_Proceso_PN2_Revisado.drawio"
    with open(filepath, "w", encoding="utf-8") as f:
        f.write(xml)
    print("Created:", filepath)

if __name__ == '__main__':
    generate_pn2_drawio()
