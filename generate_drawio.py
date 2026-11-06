import xml.etree.ElementTree as ET

def generate_drawio_xml():
    xml_content = '''<mxfile host="app.diagrams.net" modified="2026-11-06T00:30:00.000Z" agent="Antigravity" version="21.0.0" type="device">
  <diagram id="diagram_activity_turnos" name="Diagrama de Actividad - Gestión de Turnos">
    <mxGraphModel dx="1200" dy="1200" grid="1" gridSize="10" guides="1" tooltips="1" connect="1" arrows="1" fold="1" page="1" pageScale="1" pageWidth="900" pageHeight="1150" background="#000000" math="0" shadow="0">
      <root>
        <mxCell id="0" />
        <mxCell id="1" parent="0" />

        <!-- ================= CARRILES / SWIMLANES ================= -->
        <!-- Lane 1: PACIENTE -->
        <mxCell id="lane_paciente" value="PACIENTE" style="swimlane;whiteSpace=wrap;html=1;startSize=32;horizontal=1;collapsible=0;recursiveResize=0;expand=0;swimlaneFillColor=none;strokeColor=#555555;strokeWidth=1.5;fontColor=#ffffff;fontStyle=1;fontSize=12;fillColor=#000000;" vertex="1" parent="1">
          <mxGeometry x="40" y="40" width="220" height="980" as="geometry" />
        </mxCell>

        <!-- Lane 2: NUTRICIONISTA -->
        <mxCell id="lane_nutricionista" value="NUTRICIONISTA" style="swimlane;whiteSpace=wrap;html=1;startSize=32;horizontal=1;collapsible=0;recursiveResize=0;expand=0;swimlaneFillColor=none;strokeColor=#555555;strokeWidth=1.5;fontColor=#ffffff;fontStyle=1;fontSize=12;fillColor=#000000;" vertex="1" parent="1">
          <mxGeometry x="260" y="40" width="360" height="980" as="geometry" />
        </mxCell>

        <!-- Lane 3: DATA STORE -->
        <mxCell id="lane_datastore" value="DATA STORE" style="swimlane;whiteSpace=wrap;html=1;startSize=32;horizontal=1;collapsible=0;recursiveResize=0;expand=0;swimlaneFillColor=none;strokeColor=#555555;strokeWidth=1.5;fontColor=#ffffff;fontStyle=1;fontSize=12;fillColor=#000000;" vertex="1" parent="1">
          <mxGeometry x="620" y="40" width="240" height="980" as="geometry" />
        </mxCell>

        <!-- ================= ELEMENTOS EN PACIENTE ================= -->
        <!-- Start Node (UML Initial State) -->
        <mxCell id="node_start" value="" style="ellipse;html=1;shape=startState;fillColor=#ffffff;strokeColor=#ffffff;strokeWidth=2;" vertex="1" parent="1">
          <mxGeometry x="137" y="90" width="26" height="26" as="geometry" />
        </mxCell>

        <!-- Datos Turno -->
        <mxCell id="doc_datos_turno" value="Datos Turno" style="shape=note;whiteSpace=wrap;html=1;backgroundOutline=1;darkOpacity=0.05;size=14;fillColor=#000000;strokeColor=#ffffff;strokeWidth=1.5;fontColor=#ffffff;fontSize=11;align=center;verticalAlign=middle;" vertex="1" parent="1">
          <mxGeometry x="85" y="150" width="130" height="50" as="geometry" />
        </mxCell>

        <!-- Datos Filiales -->
        <mxCell id="doc_datos_filiales" value="Datos Filiales" style="shape=note;whiteSpace=wrap;html=1;backgroundOutline=1;darkOpacity=0.05;size=14;fillColor=#000000;strokeColor=#ffffff;strokeWidth=1.5;fontColor=#ffffff;fontSize=11;align=center;verticalAlign=middle;" vertex="1" parent="1">
          <mxGeometry x="85" y="300" width="130" height="50" as="geometry" />
        </mxCell>

        <!-- Horario Seleccionado -->
        <mxCell id="doc_horario_sel" value="Horario Seleccionado" style="shape=note;whiteSpace=wrap;html=1;backgroundOutline=1;darkOpacity=0.05;size=14;fillColor=#000000;strokeColor=#ffffff;strokeWidth=1.5;fontColor=#ffffff;fontSize=11;align=center;verticalAlign=middle;" vertex="1" parent="1">
          <mxGeometry x="85" y="515" width="130" height="50" as="geometry" />
        </mxCell>

        <!-- Confirmacion Turno -->
        <mxCell id="doc_confirmacion_turno" value="Confirmacion Turno" style="shape=note;whiteSpace=wrap;html=1;backgroundOutline=1;darkOpacity=0.05;size=14;fillColor=#000000;strokeColor=#ffffff;strokeWidth=1.5;fontColor=#ffffff;fontSize=11;align=center;verticalAlign=middle;" vertex="1" parent="1">
          <mxGeometry x="85" y="675" width="130" height="50" as="geometry" />
        </mxCell>

        <!-- ================= ELEMENTOS EN NUTRICIONISTA ================= -->
        <!-- Registrar Turno (Consultar disponibilidad) -->
        <mxCell id="act_registrar_turno" value="Registrar Turno&#xa;(Consultar disponibilidad)" style="rounded=1;whiteSpace=wrap;html=1;arcSize=25;fillColor=#000000;strokeColor=#ffffff;strokeWidth=1.5;fontColor=#ffffff;fontSize=11;align=center;verticalAlign=middle;" vertex="1" parent="1">
          <mxGeometry x="355" y="150" width="170" height="50" as="geometry" />
        </mxCell>

        <!-- Decision 1: ¿Paciente existe en BD? -->
        <mxCell id="dec_paciente_existe" value="¿Paciente&#xa;existe en BD?" style="rhombus;whiteSpace=wrap;html=1;fillColor=#000000;strokeColor=#ffffff;strokeWidth=1.5;fontColor=#ffffff;fontSize=10;align=center;verticalAlign=middle;" vertex="1" parent="1">
          <mxGeometry x="380" y="235" width="120" height="60" as="geometry" />
        </mxCell>

        <!-- Registrar Paciente -->
        <mxCell id="act_registrar_paciente" value="Registrar Paciente" style="rounded=1;whiteSpace=wrap;html=1;arcSize=25;fillColor=#000000;strokeColor=#ffffff;strokeWidth=1.5;fontColor=#ffffff;fontSize=11;align=center;verticalAlign=middle;" vertex="1" parent="1">
          <mxGeometry x="290" y="325" width="150" height="50" as="geometry" />
        </mxCell>

        <!-- Decision 2: ¿Existe superposición? -->
        <mxCell id="dec_existe_superposicion" value="¿Existe&#xa;superposición?" style="rhombus;whiteSpace=wrap;html=1;fillColor=#000000;strokeColor=#ffffff;strokeWidth=1.5;fontColor=#ffffff;fontSize=10;align=center;verticalAlign=middle;" vertex="1" parent="1">
          <mxGeometry x="380" y="415" width="120" height="55" as="geometry" />
        </mxCell>

        <!-- Reprogramar Turno (Ofrecer horarios libres) -->
        <mxCell id="act_reprogramar_libres" value="Reprogramar Turno&#xa;(Ofrecer horarios libres)" style="rounded=1;whiteSpace=wrap;html=1;arcSize=25;fillColor=#000000;strokeColor=#ffffff;strokeWidth=1.5;fontColor=#ffffff;fontSize=11;align=center;verticalAlign=middle;" vertex="1" parent="1">
          <mxGeometry x="355" y="515" width="170" height="50" as="geometry" />
        </mxCell>

        <!-- Seleccionar turno disponible en la agenda médica -->
        <mxCell id="act_seleccionar_turno" value="Seleccionar turno disponible&#xa;en la agenda médica" style="rounded=1;whiteSpace=wrap;html=1;arcSize=25;fillColor=#000000;strokeColor=#ffffff;strokeWidth=1.5;fontColor=#ffffff;fontSize=11;align=center;verticalAlign=middle;" vertex="1" parent="1">
          <mxGeometry x="355" y="595" width="170" height="50" as="geometry" />
        </mxCell>

        <!-- Modificar Turno / Actualizar agenda -->
        <mxCell id="act_modificar_turno" value="Modificar Turno /&#xa;Actualizar agenda" style="rounded=1;whiteSpace=wrap;html=1;arcSize=25;fillColor=#000000;strokeColor=#ffffff;strokeWidth=1.5;fontColor=#ffffff;fontSize=11;align=center;verticalAlign=middle;" vertex="1" parent="1">
          <mxGeometry x="355" y="675" width="170" height="50" as="geometry" />
        </mxCell>

        <!-- Decision 3: ¿Solicita cambios en el turno? -->
        <mxCell id="dec_solicita_cambios" value="¿Solicita cambios&#xa;en el turno?" style="rhombus;whiteSpace=wrap;html=1;fillColor=#000000;strokeColor=#ffffff;strokeWidth=1.5;fontColor=#ffffff;fontSize=10;align=center;verticalAlign=middle;" vertex="1" parent="1">
          <mxGeometry x="375" y="760" width="130" height="60" as="geometry" />
        </mxCell>

        <!-- Cancelar Turno (Modificar por cancelación) -->
        <mxCell id="act_cancelar_turno" value="Cancelar Turno&#xa;(Modificar por cancelación)" style="rounded=1;whiteSpace=wrap;html=1;arcSize=25;fillColor=#000000;strokeColor=#ffffff;strokeWidth=1.5;fontColor=#ffffff;fontSize=10;align=center;verticalAlign=middle;" vertex="1" parent="1">
          <mxGeometry x="275" y="865" width="145" height="50" as="geometry" />
        </mxCell>

        <!-- Reprogramar Turno (Modificar por reprogramación) -->
        <mxCell id="act_reprogramar_mod" value="Reprogramar Turno&#xa;(Modificar por&#xa;reprogramación)" style="rounded=1;whiteSpace=wrap;html=1;arcSize=25;fillColor=#000000;strokeColor=#ffffff;strokeWidth=1.5;fontColor=#ffffff;fontSize=10;align=center;verticalAlign=middle;" vertex="1" parent="1">
          <mxGeometry x="460" y="865" width="145" height="50" as="geometry" />
        </mxCell>

        <!-- End Node (UML Activity Final State) -->
        <mxCell id="node_end" value="" style="ellipse;html=1;shape=endState;fillColor=#ffffff;strokeColor=#ffffff;strokeWidth=2;" vertex="1" parent="1">
          <mxGeometry x="426" y="955" width="28" height="28" as="geometry" />
        </mxCell>

        <!-- ================= ELEMENTOS EN DATA STORE ================= -->
        <!-- AGENDA Y TURNOS -->
        <mxCell id="ds_agenda_turnos" value="AGENDA Y TURNOS" style="shape=folder;tabWidth=55;tabHeight=14;tabPosition=left;html=1;boundedLbl=1;backgroundOutline=0;fillColor=#000000;strokeColor=#ffffff;strokeWidth=1.5;fontColor=#ffffff;fontStyle=1;fontSize=10;align=center;verticalAlign=bottom;spacingBottom=6;" vertex="1" parent="1">
          <mxGeometry x="665" y="157" width="150" height="42" as="geometry" />
        </mxCell>

        <!-- PACIENTES -->
        <mxCell id="ds_pacientes" value="PACIENTES" style="shape=folder;tabWidth=55;tabHeight=14;tabPosition=left;html=1;boundedLbl=1;backgroundOutline=0;fillColor=#000000;strokeColor=#ffffff;strokeWidth=1.5;fontColor=#ffffff;fontStyle=1;fontSize=10;align=center;verticalAlign=bottom;spacingBottom=6;" vertex="1" parent="1">
          <mxGeometry x="680" y="337" width="150" height="42" as="geometry" />
        </mxCell>

        <!-- BLOQUES HORARIOS -->
        <mxCell id="ds_bloques_horarios" value="BLOQUES HORARIOS" style="shape=folder;tabWidth=55;tabHeight=14;tabPosition=left;html=1;boundedLbl=1;backgroundOutline=0;fillColor=#000000;strokeColor=#ffffff;strokeWidth=1.5;fontColor=#ffffff;fontStyle=1;fontSize=10;align=center;verticalAlign=bottom;spacingBottom=6;" vertex="1" parent="1">
          <mxGeometry x="665" y="682" width="150" height="42" as="geometry" />
        </mxCell>

        <!-- ================= CONEXIONES / FLUJOS ================= -->
        <!-- 1. Start Node -> Datos Turno -->
        <mxCell id="edge_start_datos" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;entryX=0;entryY=0.5;entryDx=0;entryDy=0;" edge="1" source="node_start" target="doc_datos_turno" parent="1">
          <mxGeometry relative="1" as="geometry">
            <Array as="points">
              <mxPoint x="150" y="125" />
              <mxPoint x="60" y="125" />
              <mxPoint x="60" y="175" />
            </Array>
          </mxGeometry>
        </mxCell>

        <!-- 2. Datos Turno -> Registrar Turno -->
        <mxCell id="edge_datos_registrar" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;exitX=1;exitY=0.5;exitDx=0;exitDy=0;entryX=0;entryY=0.5;entryDx=0;entryDy=0;" edge="1" source="doc_datos_turno" target="act_registrar_turno" parent="1">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>

        <!-- 3. Registrar Turno -> AGENDA Y TURNOS (Dashed) -->
        <mxCell id="edge_reg_agenda" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;dashed=1;dashPattern=2 3;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;exitX=1;exitY=0.5;exitDx=0;exitDy=0;entryX=0;entryY=0.5;entryDx=0;entryDy=0;" edge="1" source="act_registrar_turno" target="ds_agenda_turnos" parent="1">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>

        <!-- 4. Registrar Turno -> ¿Paciente existe en BD? -->
        <mxCell id="edge_reg_dec1" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;exitX=0.5;exitY=1;exitDx=0;exitDy=0;entryX=0.5;entryY=0;entryDx=0;entryDy=0;" edge="1" source="act_registrar_turno" target="dec_paciente_existe" parent="1">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>

        <!-- 5. ¿Paciente existe en BD? -> No -> Datos Filiales -->
        <mxCell id="edge_dec1_no" value="No" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;fontColor=#ffffff;fontSize=10;labelBackgroundColor=#000000;exitX=0;exitY=0.5;exitDx=0;exitDy=0;entryX=0.5;entryY=0;entryDx=0;entryDy=0;" edge="1" source="dec_paciente_existe" target="doc_datos_filiales" parent="1">
          <mxGeometry relative="1" as="geometry">
            <mxPoint as="offset" />
          </mxGeometry>
        </mxCell>

        <!-- 6. Datos Filiales -> Registrar Paciente -->
        <mxCell id="edge_filiales_regpac" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;exitX=1;exitY=0.5;exitDx=0;exitDy=0;entryX=0;entryY=0.5;entryDx=0;entryDy=0;" edge="1" source="doc_datos_filiales" target="act_registrar_paciente" parent="1">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>

        <!-- 7. ¿Paciente existe en BD? -> Sí -> Merge point before Decision 2 -->
        <mxCell id="edge_dec1_si" value="Sí" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=none;fontColor=#ffffff;fontSize=10;labelBackgroundColor=#000000;exitX=1;exitY=0.5;exitDx=0;exitDy=0;" edge="1" source="dec_paciente_existe" parent="1">
          <mxGeometry relative="1" as="geometry">
            <mxPoint x="440" y="390" as="targetPoint" />
            <Array as="points">
              <mxPoint x="540" y="265" />
              <mxPoint x="540" y="390" />
            </Array>
            <mxPoint as="offset" />
          </mxGeometry>
        </mxCell>

        <!-- 8. Registrar Paciente -> Merge point before Decision 2 -->
        <mxCell id="edge_regpac_dec2" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;exitX=0.5;exitY=1;exitDx=0;exitDy=0;entryX=0.5;entryY=0;entryDx=0;entryDy=0;" edge="1" source="act_registrar_paciente" target="dec_existe_superposicion" parent="1">
          <mxGeometry relative="1" as="geometry">
            <Array as="points">
              <mxPoint x="365" y="390" />
              <mxPoint x="440" y="390" />
            </Array>
          </mxGeometry>
        </mxCell>

        <!-- 9. Merge branch -> PACIENTES (Dashed) -->
        <mxCell id="edge_si_pacientes" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;dashed=1;dashPattern=2 3;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;entryX=0;entryY=0.5;entryDx=0;entryDy=0;" edge="1" target="ds_pacientes" parent="1">
          <mxGeometry relative="1" as="geometry">
            <mxPoint x="540" y="358" as="sourcePoint" />
          </mxGeometry>
        </mxCell>

        <!-- 10. ¿Existe superposición? -> Sí -> Reprogramar Turno -->
        <mxCell id="edge_dec2_si" value="Sí" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;fontColor=#ffffff;fontSize=10;labelBackgroundColor=#000000;exitX=0.5;exitY=1;exitDx=0;exitDy=0;entryX=0.5;entryY=0;entryDx=0;entryDy=0;" edge="1" source="dec_existe_superposicion" target="act_reprogramar_libres" parent="1">
          <mxGeometry relative="1" as="geometry">
            <mxPoint as="offset" />
          </mxGeometry>
        </mxCell>

        <!-- 11. ¿Existe superposición? -> No -> Seleccionar turno disponible -->
        <mxCell id="edge_dec2_no" value="No" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;fontColor=#ffffff;fontSize=10;labelBackgroundColor=#000000;exitX=1;exitY=0.5;exitDx=0;exitDy=0;entryX=1;entryY=0.5;entryDx=0;entryDy=0;" edge="1" source="dec_existe_superposicion" target="act_seleccionar_turno" parent="1">
          <mxGeometry relative="1" as="geometry">
            <Array as="points">
              <mxPoint x="540" y="443" />
              <mxPoint x="540" y="620" />
            </Array>
            <mxPoint as="offset" />
          </mxGeometry>
        </mxCell>

        <!-- 12. Reprogramar Turno -> Horario Seleccionado -->
        <mxCell id="edge_reprog_horario" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;exitX=0;exitY=0.5;exitDx=0;exitDy=0;entryX=1;entryY=0.5;entryDx=0;entryDy=0;" edge="1" source="act_reprogramar_libres" target="doc_horario_sel" parent="1">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>

        <!-- 13. Horario Seleccionado -> Seleccionar turno disponible -->
        <mxCell id="edge_horario_selec" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;exitX=0.5;exitY=1;exitDx=0;exitDy=0;entryX=0.5;entryY=0;entryDx=0;entryDy=0;" edge="1" source="doc_horario_sel" target="act_seleccionar_turno" parent="1">
          <mxGeometry relative="1" as="geometry">
            <Array as="points">
              <mxPoint x="150" y="580" />
              <mxPoint x="440" y="580" />
            </Array>
          </mxGeometry>
        </mxCell>

        <!-- 14. Seleccionar turno disponible -> Modificar Turno / Actualizar agenda -->
        <mxCell id="edge_selec_modificar" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;exitX=0.5;exitY=1;exitDx=0;exitDy=0;entryX=0.5;entryY=0;entryDx=0;entryDy=0;" edge="1" source="act_seleccionar_turno" target="act_modificar_turno" parent="1">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>

        <!-- 15. Modificar Turno -> Confirmacion Turno -->
        <mxCell id="edge_mod_confirmacion" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;exitX=0;exitY=0.5;exitDx=0;exitDy=0;entryX=1;entryY=0.5;entryDx=0;entryDy=0;" edge="1" source="act_modificar_turno" target="doc_confirmacion_turno" parent="1">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>

        <!-- 16. Modificar Turno -> BLOQUES HORARIOS (Dashed) -->
        <mxCell id="edge_mod_bloques" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;dashed=1;dashPattern=2 3;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;exitX=1;exitY=0.5;exitDx=0;exitDy=0;entryX=0;entryY=0.5;entryDx=0;entryDy=0;" edge="1" source="act_modificar_turno" target="ds_bloques_horarios" parent="1">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>

        <!-- 17. Modificar Turno -> ¿Solicita cambios en el turno? -->
        <mxCell id="edge_mod_dec3" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;exitX=0.5;exitY=1;exitDx=0;exitDy=0;entryX=0.5;entryY=0;entryDx=0;entryDy=0;" edge="1" source="act_modificar_turno" target="dec_solicita_cambios" parent="1">
          <mxGeometry relative="1" as="geometry" />
        </mxCell>

        <!-- 18. ¿Solicita cambios en el turno? -> Cancelación -> Cancelar Turno -->
        <mxCell id="edge_dec3_canc" value="Cancelación" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;fontColor=#ffffff;fontSize=10;labelBackgroundColor=#000000;exitX=0.5;exitY=1;exitDx=0;exitDy=0;entryX=0.5;entryY=0;entryDx=0;entryDy=0;" edge="1" source="dec_solicita_cambios" target="act_cancelar_turno" parent="1">
          <mxGeometry relative="1" as="geometry">
            <Array as="points">
              <mxPoint x="440" y="840" />
              <mxPoint x="348" y="840" />
            </Array>
            <mxPoint as="offset" />
          </mxGeometry>
        </mxCell>

        <!-- 19. ¿Solicita cambios en el turno? -> Reprogramación -> Reprogramar Turno -->
        <mxCell id="edge_dec3_reprog" value="Reprogramación" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;fontColor=#ffffff;fontSize=10;labelBackgroundColor=#000000;exitX=0.5;exitY=1;exitDx=0;exitDy=0;entryX=0.5;entryY=0;entryDx=0;entryDy=0;" edge="1" source="dec_solicita_cambios" target="act_reprogramar_mod" parent="1">
          <mxGeometry relative="1" as="geometry">
            <Array as="points">
              <mxPoint x="440" y="840" />
              <mxPoint x="533" y="840" />
            </Array>
            <mxPoint as="offset" />
          </mxGeometry>
        </mxCell>

        <!-- 20. Cancelar Turno & Reprogramar Turno -> End Node -->
        <mxCell id="edge_canc_end" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=classic;endFill=1;exitX=0.5;exitY=1;exitDx=0;exitDy=0;entryX=0.5;entryY=0;entryDx=0;entryDy=0;" edge="1" source="act_cancelar_turno" target="node_end" parent="1">
          <mxGeometry relative="1" as="geometry">
            <Array as="points">
              <mxPoint x="348" y="935" />
              <mxPoint x="440" y="935" />
            </Array>
          </mxGeometry>
        </mxCell>

        <mxCell id="edge_reprog_end" style="edgeStyle=orthogonalEdgeStyle;rounded=0;orthogonalLoop=1;jettySize=auto;html=1;strokeColor=#ffffff;strokeWidth=1.2;endArrow=none;exitX=0.5;exitY=1;exitDx=0;exitDy=0;" edge="1" source="act_reprogramar_mod" parent="1">
          <mxGeometry relative="1" as="geometry">
            <Array as="points">
              <mxPoint x="533" y="935" />
              <mxPoint x="440" y="935" />
            </Array>
            <mxPoint x="440" y="935" as="targetPoint" />
          </mxGeometry>
        </mxCell>

      </root>
    </mxGraphModel>
  </diagram>
</mxfile>'''
    return xml_content

if __name__ == '__main__':
    content = generate_drawio_xml()
    tree = ET.fromstring(content)
    print("XML is completely valid! Length:", len(content))
    with open('diagrama_turnos.drawio', 'w', encoding='utf-8') as f:
        f.write(content)
    print("Saved to diagrama_turnos.drawio successfully!")
