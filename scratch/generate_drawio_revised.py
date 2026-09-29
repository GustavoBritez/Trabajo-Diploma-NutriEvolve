import os
import xml.etree.ElementTree as ET

def generate_drawio():
    xml = """<mxfile host="Electron" modified="2023-10-27T00:00:00.000Z" agent="Mozilla/5.0" version="22.0.4" type="device">
  <diagram id="pn1" name="Proceso PN1">
    <mxGraphModel dx="1200" dy="1200" grid="1" gridSize="10" guides="1" tooltips="1" connect="1" arrows="1" fold="1" page="1" pageScale="1" pageWidth="1169" pageHeight="827" math="0" shadow="0">
      <root>
        <mxCell id="0" />
        <mxCell id="1" parent="0" />
        
        <!-- Pool -->
        <mxCell id="pool" value="PN1: Turnero Nutricional" style="swimlane;childLayout=stackLayout;horizontal=0;horizontalStack=1;startSize=30;" vertex="1" parent="1">
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
        <mxCell id="start" value="" style="ellipse;fillColor=#000000;strokeColor=#000000;" vertex="1" parent="lane_paciente">
          <mxGeometry x="130" y="50" width="30" height="30" as="geometry" />
        </mxCell>
        
        <mxCell id="p_datos_turno" value="Datos del turno" style="shape=note;whiteSpace=wrap;html=1;backgroundOutline=1;darkOpacity=0.05;size=15;" vertex="1" parent="lane_paciente">
          <mxGeometry x="70" y="120" width="150" height="40" as="geometry" />
        </mxCell>
        
        <mxCell id="p_datos_fil" value="Datos filiatorios" style="shape=note;whiteSpace=wrap;html=1;backgroundOutline=1;darkOpacity=0.05;size=15;" vertex="1" parent="lane_paciente">
          <mxGeometry x="70" y="240" width="150" height="40" as="geometry" />
        </mxCell>
        
        <mxCell id="p_horario" value="Horario seleccionado" style="shape=note;whiteSpace=wrap;html=1;backgroundOutline=1;darkOpacity=0.05;size=15;" vertex="1" parent="lane_paciente">
          <mxGeometry x="70" y="440" width="150" height="40" as="geometry" />
        </mxCell>
        
        <mxCell id="p_confirm" value="Confirmación del turno" style="shape=note;whiteSpace=wrap;html=1;backgroundOutline=1;darkOpacity=0.05;size=15;" vertex="1" parent="lane_paciente">
          <mxGeometry x="70" y="520" width="150" height="40" as="geometry" />
        </mxCell>
        
        <mxCell id="p_asistencia" value="Presentación a consulta" style="shape=note;whiteSpace=wrap;html=1;backgroundOutline=1;darkOpacity=0.05;size=15;" vertex="1" parent="lane_paciente">
          <mxGeometry x="70" y="700" width="150" height="40" as="geometry" />
        </mxCell>

        <!-- Nodos NUTRICIONISTA -->
        <mxCell id="n_consultar" value="Consultar disponibilidad&#xa;en agenda médica" style="rounded=1;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="90" y="110" width="170" height="50" as="geometry" />
        </mxCell>
        
        <mxCell id="d_existe_bd" value="¿Paciente&#xa;existe en BD?" style="rhombus;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="110" y="180" width="130" height="60" as="geometry" />
        </mxCell>
        
        <mxCell id="n_registrar" value="Registrar Paciente" style="rounded=1;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="90" y="270" width="170" height="40" as="geometry" />
        </mxCell>
        
        <mxCell id="d_superp" value="¿Existe&#xa;superposición?" style="rhombus;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="110" y="340" width="130" height="60" as="geometry" />
        </mxCell>
        
        <mxCell id="n_reprogramar" value="Reprogramar y ofrecer&#xa;horarios alternativos libres" style="rounded=1;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="90" y="430" width="170" height="50" as="geometry" />
        </mxCell>
        
        <mxCell id="n_seleccionar" value="Seleccionar turno disponible&#xa;en la agenda médica" style="rounded=1;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="90" y="520" width="170" height="50" as="geometry" />
        </mxCell>
        
        <mxCell id="n_actualizar" value="Actualizar agenda médica&#xa;(bloque horario como 'Ocupado')" style="rounded=1;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="80" y="600" width="190" height="50" as="geometry" />
        </mxCell>
        
        <mxCell id="d_cambios" value="¿Solicita cambios&#xa;en el turno?" style="rhombus;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="110" y="680" width="130" height="60" as="geometry" />
        </mxCell>
        
        <mxCell id="n_mod_cancel" value="Modificar turno por cancelación" style="rounded=1;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="20" y="770" width="130" height="50" as="geometry" />
        </mxCell>
        
        <mxCell id="n_mod_reprog" value="Modificar turno por reprogramación" style="rounded=1;whiteSpace=wrap;html=1;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="200" y="770" width="130" height="50" as="geometry" />
        </mxCell>
        
        <!-- End node for cancellation -->
        <mxCell id="end_cancel" value="" style="ellipse;html=1;shape=endState;fillColor=#000000;strokeColor=#000000;" vertex="1" parent="lane_nutricionista">
          <mxGeometry x="70" y="850" width="30" height="30" as="geometry" />
        </mxCell>

        <!-- Nodos DB -->
        <mxCell id="db_agenda" value="AGENDA Y TURNOS" style="shape=folder;fontStyle=1;tabWidth=110;tabHeight=20;tabPosition=left;html=1;boundedLbl=1;" vertex="1" parent="lane_db">
          <mxGeometry x="70" y="110" width="160" height="50" as="geometry" />
        </mxCell>
        
        <mxCell id="db_pacientes" value="PACIENTES" style="shape=folder;fontStyle=1;tabWidth=90;tabHeight=20;tabPosition=left;html=1;boundedLbl=1;" vertex="1" parent="lane_db">
          <mxGeometry x="70" y="260" width="160" height="50" as="geometry" />
        </mxCell>
        
        <mxCell id="db_bloques" value="BLOQUES HORARIOS" style="shape=folder;fontStyle=1;tabWidth=120;tabHeight=20;tabPosition=left;html=1;boundedLbl=1;" vertex="1" parent="lane_db">
          <mxGeometry x="70" y="600" width="160" height="50" as="geometry" />
        </mxCell>
        
        <!-- CONNECTIONS -->
        <!-- Start to Datos -->
        <mxCell id="c1" edge="1" parent="1" source="start" target="p_datos_turno">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <!-- Paciente to Nutri -->
        <mxCell id="c2" edge="1" parent="1" source="p_datos_turno" target="n_consultar">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="c3" edge="1" parent="1" source="n_consultar" target="d_existe_bd">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <!-- Decision Paciente Existe -->
        <mxCell id="c4" value="No" edge="1" parent="1" source="d_existe_bd" target="p_datos_fil">
          <mxGeometry relative="1" as="geometry">
            <Array as="points">
              <mxPoint x="435" y="260" />
            </Array>
          </mxGeometry>
        </mxCell>
        
        <mxCell id="c5" edge="1" parent="1" source="p_datos_fil" target="n_registrar">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <!-- Yes path -> superposicion -->
        <mxCell id="c6" value="Sí" edge="1" parent="1" source="d_existe_bd" target="d_superp">
          <mxGeometry relative="1" as="geometry">
            <Array as="points">
              <mxPoint x="550" y="250" />
              <mxPoint x="550" y="410" />
            </Array>
          </mxGeometry>
        </mxCell>
        
        <mxCell id="c7" edge="1" parent="1" source="n_registrar" target="d_superp">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <!-- Decision Superposicion -->
        <mxCell id="c8" value="Sí" edge="1" parent="1" source="d_superp" target="n_reprogramar">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="c9" value="No" edge="1" parent="1" source="d_superp" target="n_seleccionar">
          <mxGeometry relative="1" as="geometry">
            <Array as="points">
              <mxPoint x="560" y="410" />
              <mxPoint x="560" y="585" />
            </Array>
          </mxGeometry>
        </mxCell>
        
        <mxCell id="c10" edge="1" parent="1" source="n_reprogramar" target="p_horario">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="c11" edge="1" parent="1" source="p_horario" target="n_seleccionar">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="c12" edge="1" parent="1" source="n_seleccionar" target="n_actualizar">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="c13" edge="1" parent="1" source="n_actualizar" target="p_confirm">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="c14" edge="1" parent="1" source="n_actualizar" target="d_cambios">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <!-- Cambios en el turno? -->
        <mxCell id="c15" value="Cancelación" edge="1" parent="1" source="d_cambios" target="n_mod_cancel">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="c16" value="Reprogramación" edge="1" parent="1" source="d_cambios" target="n_mod_reprog">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="c17" edge="1" parent="1" source="n_mod_cancel" target="end_cancel">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <!-- DB Connections (dashed) -->
        <mxCell id="db_c1" edge="1" parent="1" source="n_consultar" target="db_agenda" style="dashed=1;">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="db_c2" edge="1" parent="1" source="n_registrar" target="db_pacientes" style="dashed=1;">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
        <mxCell id="db_c3" edge="1" parent="1" source="n_actualizar" target="db_bloques" style="dashed=1;">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>
        
      </root>
    </mxGraphModel>
  </diagram>
</mxfile>"""

    filepath = r"C:\Users\Danie\Desktop\GIT\TD\Diagramas\Diagrama_Proceso_PN1_Revisado.drawio"
    with open(filepath, "w", encoding="utf-8") as f:
        f.write(xml)
    print("Created:", filepath)

if __name__ == '__main__':
    generate_drawio()
