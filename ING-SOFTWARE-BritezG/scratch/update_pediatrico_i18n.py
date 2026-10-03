import json

es_additions = {
    # FrmRegistrarTurno_DNI101
    "msg_dni_no_encontrado_padron": "El DNI {0} no se encuentra en el padrón de pacientes.\n\nA continuación, se abrirá la ventana para ingresar los datos personales del Paciente (CUN-02).",
    "titulo_ext_cun02": "Punto de Extensión CUN-02",
    "msg_registro_paciente_interrumpido": "No se completó el registro del paciente. El turno no puede darse de alta.",
    "titulo_cun01_interrumpido": "CUN01 Interrumpido",
    "msg_error_recuperar_paciente": "Error al recuperar el paciente registrado.",
    "msg_error_registrar_turno": "Ocurrió un error al registrar el turno:\n{0}",
    "titulo_error_cun01": "Error CUN01",
    "msg_pantalla_no_cerrar_directo": "Esta pantalla no puede cerrarse directamente.\nDebe completar el registro del turno o presionar el botón 'Cancelar'.",
    "titulo_accion_requerida": "Acción Requerida",

    # FrmReprogramarTurno_DNI101
    "msg_sin_bloque_reprogramar": "No hay un bloque horario disponible seleccionado para la reprogramación.",
    "titulo_horario_no_seleccionado": "Horario no seleccionado",
    "msg_turno_reprogramado_ok_det": "¡El turno '{0}' ha sido reprogramado exitosamente!\n\n• Nueva Fecha: {1}\n• Nuevo Horario: {2}\n• Nuevo Estado: Confirmado",
    "titulo_reprogramacion_exitosa": "Reprogramación Exitosa",
    "msg_error_reprogramar_turno": "Ocurrió un error al reprogramar el turno:\n{0}",
    "titulo_error_reprogramar": "Error al Reprogramar",

    # RegistrarPaciente_DNI101
    "msg_ingrese_nombre_paciente": "Por favor, ingrese el nombre del paciente.",
    "msg_ingrese_apellido_paciente": "Por favor, ingrese el apellido del paciente.",
    "msg_ingrese_dni_paciente": "Por favor, ingrese el DNI del niño/a.",
    "msg_seleccione_obra_social": "Por favor, seleccione una Obra Social (Medicus, OSPEP o SAO).",
    "msg_paciente_registrado_det": "El paciente '{0}, {1}' con DNI {2} y Obra Social {3} fue registrado exitosamente.",
    "msg_no_pudo_registrar_paciente": "No se pudo completar el registro del paciente.",
    "msg_error_registrar_paciente": "Ocurrió un error al registrar el paciente:\n{0}",
    "titulo_campo_requerido": "Campo requerido",
    "titulo_error_registrar_paciente": "Error al Registrar",

    # frmModificarTurno_DNI101
    "msg_ingrese_codigo_turno": "Por favor, ingrese un código de turno para buscar.",
    "titulo_busqueda_requerida": "Búsqueda Requerida",
    "msg_turno_no_encontrado_cod": "No se encontró ningún turno registrado con el código '{0}'.\nVerifique el código e intente nuevamente.",
    "titulo_turno_no_encontrado": "Turno no encontrado",
    "msg_error_consultar_turno": "Error al consultar el turno: {0}",
    "titulo_error_consulta": "Error de Consulta",
    "msg_sel_turno_antes_modificar": "Por favor busque y seleccione un turno antes de intentar modificarlo.",
    "titulo_turno_requerido": "Turno Requerido",
    "msg_motivo_vacio": "El motivo de consulta no puede estar vacío.",
    "msg_sel_estado_valido": "Debe seleccionar un estado válido para el turno.",
    "msg_confirmar_modificar_turno": "¿Está seguro de que desea guardar las modificaciones del turno '{0}'?\n\n• Estado previo: {1} ➔ Nuevo Estado: {2}\n• Nuevo Motivo: {3}",
    "titulo_confirmar_modificacion": "Confirmar Modificación",
    "msg_turno_modificado_ok_det": "El turno '{0}' fue modificado exitosamente.\nEstado: '{1}'\nDígitos Verificadores recalculados y asentado en Bitácora.",
    "msg_error_modificar_turno": "Ocurrió un error al modificar el turno:\n{0}",
    "titulo_error_modificar": "Error al Modificar",

    # Form Titles & Subtitles explicit scoped keys
    "FrmRegistrarTurno_DNI101_lblTitulo": "📅 Registrar Turno Nutricional - CUN01 (PN1)",
    "FrmRegistrarTurno_DNI101_lblSubtitulo": "Consulta la disponibilidad y registra el turno del paciente.",
    "FrmReprogramarTurno_DNI101_lblTitulo": "🔄 Reprogramar Turno Nutricional - CUN03 (PN1)",
    "FrmReprogramarTurno_DNI101_lblSubtitulo": "Seleccione la nueva fecha y profesional deseado para reubicar el bloque horario del turno.",
    "RegistrarPaciente_DNI101_lblTitulo": "👤 Registro de Paciente Pediátrico",
    "RegistrarPaciente_DNI101_lblSubtitulo": "Complete los datos del paciente para vincularlo al turno nutricional.",
    "frmModificarTurno_DNI101_lblTitulo": "📝 Modificar Turno Nutricional - CUN04 (PN1)",
    "frmModificarTurno_DNI101_lblSubtitulo": "Busque un turno por su Código de Turno y actualice su Estado y Motivo de Consulta."
}

en_additions = {
    # FrmRegistrarTurno_DNI101
    "msg_dni_no_encontrado_padron": "ID {0} was not found in patient records.\n\nThe window to register patient personal information (CUN-02) will now open.",
    "titulo_ext_cun02": "Extension Point CUN-02",
    "msg_registro_paciente_interrumpido": "Patient registration was not completed. Appointment cannot be created.",
    "titulo_cun01_interrumpido": "CUN01 Interrupted",
    "msg_error_recuperar_paciente": "Error retrieving registered patient.",
    "msg_error_registrar_turno": "An error occurred while booking the appointment:\n{0}",
    "titulo_error_cun01": "Error CUN01",
    "msg_pantalla_no_cerrar_directo": "This screen cannot be closed directly.\nPlease complete appointment booking or click 'Cancel'.",
    "titulo_accion_requerida": "Action Required",

    # FrmReprogramarTurno_DNI101
    "msg_sin_bloque_reprogramar": "No available time slot is selected for rescheduling.",
    "titulo_horario_no_seleccionado": "Time Slot Not Selected",
    "msg_turno_reprogramado_ok_det": "Appointment '{0}' has been rescheduled successfully!\n\n• New Date: {1}\n• New Time: {2}\n• New Status: Confirmed",
    "titulo_reprogramacion_exitosa": "Rescheduling Successful",
    "msg_error_reprogramar_turno": "An error occurred while rescheduling the appointment:\n{0}",
    "titulo_error_reprogramar": "Rescheduling Error",

    # RegistrarPaciente_DNI101
    "msg_ingrese_nombre_paciente": "Please enter patient first name.",
    "msg_ingrese_apellido_paciente": "Please enter patient last name.",
    "msg_ingrese_dni_paciente": "Please enter child ID / DNI.",
    "msg_seleccione_obra_social": "Please select a Health Insurance (Medicus, OSPEP or SAO).",
    "msg_paciente_registrado_det": "Patient '{0}, {1}' with ID {2} and Health Insurance {3} was registered successfully.",
    "msg_no_pudo_registrar_paciente": "Could not complete patient registration.",
    "msg_error_registrar_paciente": "An error occurred while registering the patient:\n{0}",
    "titulo_campo_requerido": "Required Field",
    "titulo_error_registrar_paciente": "Registration Error",

    # frmModificarTurno_DNI101
    "msg_ingrese_codigo_turno": "Please enter an appointment code to search.",
    "titulo_busqueda_requerida": "Search Required",
    "msg_turno_no_encontrado_cod": "No appointment found registered with code '{0}'.\nPlease verify the code and try again.",
    "titulo_turno_no_encontrado": "Appointment Not Found",
    "msg_error_consultar_turno": "Error querying appointment: {0}",
    "titulo_error_consulta": "Query Error",
    "msg_sel_turno_antes_modificar": "Please search and select an appointment before modifying.",
    "titulo_turno_requerido": "Appointment Required",
    "msg_motivo_vacio": "The reason for consultation cannot be empty.",
    "msg_sel_estado_valido": "You must select a valid appointment status.",
    "msg_confirmar_modificar_turno": "Are you sure you want to save modifications for appointment '{0}'?\n\n• Previous Status: {1} ➔ New Status: {2}\n• New Reason: {3}",
    "titulo_confirmar_modificacion": "Confirm Modification",
    "msg_turno_modificado_ok_det": "Appointment '{0}' was modified successfully.\nStatus: '{1}'\nCheck digits recalculated and logged in Audit.",
    "msg_error_modificar_turno": "An error occurred while modifying the appointment:\n{0}",
    "titulo_error_modificar": "Modification Error",

    # Form Titles & Subtitles explicit scoped keys
    "FrmRegistrarTurno_DNI101_lblTitulo": "📅 Register Nutrition Appointment - CUN01",
    "FrmRegistrarTurno_DNI101_lblSubtitulo": "Check availability and register patient appointment.",
    "FrmReprogramarTurno_DNI101_lblTitulo": "🔄 Reschedule Nutrition Appointment - CUN03",
    "FrmReprogramarTurno_DNI101_lblSubtitulo": "Select new date and doctor to reallocate appointment slot.",
    "RegistrarPaciente_DNI101_lblTitulo": "👤 Pediatric Patient Registration",
    "RegistrarPaciente_DNI101_lblSubtitulo": "Complete patient details to link to nutrition appointment.",
    "frmModificarTurno_DNI101_lblTitulo": "📝 Modify Nutrition Appointment - CUN04",
    "frmModificarTurno_DNI101_lblSubtitulo": "Search appointment by Code and update Status and Reason for Consultation."
}

po_additions = {
    # FrmRegistrarTurno_DNI101
    "msg_dni_no_encontrado_padron": "O DNI {0} não foi encontrado no cadastro de pacientes.\n\nA seguir, abrirá a janela para cadastrar os dados pessoais do Paciente (CUN-02).",
    "titulo_ext_cun02": "Ponto de Extensão CUN-02",
    "msg_registro_paciente_interrumpido": "O cadastro do paciente não foi concluído. A consulta não pode ser criada.",
    "titulo_cun01_interrumpido": "CUN01 Interrompido",
    "msg_error_recuperar_paciente": "Erro ao recuperar o paciente cadastrado.",
    "msg_error_registrar_turno": "Ocorreu um erro ao registrar a consulta:\n{0}",
    "titulo_error_cun01": "Erro CUN01",
    "msg_pantalla_no_cerrar_directo": "Esta tela não pode ser fechada diretamente.\nVocê deve concluir o agendamento ou clicar em 'Cancelar'.",
    "titulo_accion_requerida": "Ação Necessária",

    # FrmReprogramarTurno_DNI101
    "msg_sin_bloque_reprogramar": "Nenhum horário disponível foi selecionado para o reagendamento.",
    "titulo_horario_no_seleccionado": "Horário Não Selecionado",
    "msg_turno_reprogramado_ok_det": "A consulta '{0}' foi reagendada com sucesso!\n\n• Nova Data: {1}\n• Novo Horário: {2}\n• Novo Estado: Confirmado",
    "titulo_reprogramacion_exitosa": "Reagendamento com Sucesso",
    "msg_error_reprogramar_turno": "Ocorreu um erro ao reagendar a consulta:\n{0}",
    "titulo_error_reprogramar": "Erro ao Reagendar",

    # RegistrarPaciente_DNI101
    "msg_ingrese_nombre_paciente": "Por favor, insira o nome do paciente.",
    "msg_ingrese_apellido_paciente": "Por favor, insira o sobrenome do paciente.",
    "msg_ingrese_dni_paciente": "Por favor, insira o DNI da criança.",
    "msg_seleccione_obra_social": "Por favor, selecione um Convênio Médico (Medicus, OSPEP ou SAO).",
    "msg_paciente_registrado_det": "O paciente '{0}, {1}' com DNI {2} e Convênio {3} foi cadastrado com sucesso.",
    "msg_no_pudo_registrar_paciente": "Não foi possível concluir o cadastro do paciente.",
    "msg_error_registrar_paciente": "Ocorreu um erro ao cadastrar o paciente:\n{0}",
    "titulo_campo_requerido": "Campo Obrigatório",
    "titulo_error_registrar_paciente": "Erro ao Cadastrar",

    # frmModificarTurno_DNI101
    "msg_ingrese_codigo_turno": "Por favor, insira um código de consulta para pesquisar.",
    "titulo_busqueda_requerida": "Pesquisa Necessária",
    "msg_turno_no_encontrado_cod": "Nenhuma consulta foi encontrada registrada com o código '{0}'.\nVerifique o código e tente novamente.",
    "titulo_turno_no_encontrado": "Consulta Não Encontrada",
    "msg_error_consultar_turno": "Erro ao consultar a consulta: {0}",
    "titulo_error_consulta": "Erro de Consulta",
    "msg_sel_turno_antes_modificar": "Por favor pesquise e selecione uma consulta antes de modificar.",
    "titulo_turno_requerido": "Consulta Necessária",
    "msg_motivo_vacio": "O motivo da consulta não pode estar vazio.",
    "msg_sel_estado_valido": "Você deve selecionar um estado válido para a consulta.",
    "msg_confirmar_modificar_turno": "Tem certeza de que deseja salvar as alterações da consulta '{0}'?\n\n• Estado anterior: {1} ➔ Novo Estado: {2}\n• Novo Motivo: {3}",
    "titulo_confirmar_modificacion": "Confirmar Alteração",
    "msg_turno_modificado_ok_det": "A consulta '{0}' foi alterada com sucesso.\nEstado: '{1}'\nDígitos verificadores recalculados e registrados na Auditoria.",
    "msg_error_modificar_turno": "Ocorreu um erro ao modificar a consulta:\n{0}",
    "titulo_error_modificar": "Erro ao Modificar",

    # Form Titles & Subtitles explicit scoped keys
    "FrmRegistrarTurno_DNI101_lblTitulo": "📅 Registrar Consulta Nutricional Pediátrica - CUN01",
    "FrmRegistrarTurno_DNI101_lblSubtitulo": "Consulte a disponibilidade e registre a consulta do paciente.",
    "FrmReprogramarTurno_DNI101_lblTitulo": "🔄 Reagendar Consulta Nutricional - CUN03",
    "FrmReprogramarTurno_DNI101_lblSubtitulo": "Selecione a nova data e profissional para realocar o horário da consulta.",
    "RegistrarPaciente_DNI101_lblTitulo": "👤 Cadastro de Paciente Pediátrico",
    "RegistrarPaciente_DNI101_lblSubtitulo": "Preencha os dados do paciente para vinculá-lo à consulta nutricional.",
    "frmModificarTurno_DNI101_lblTitulo": "📝 Modificar Consulta Nutricional - CUN04",
    "frmModificarTurno_DNI101_lblSubtitulo": "Pesquise uma consulta pelo Código e atualize o Estado e Motivo da Consulta."
}

def update_file(filepath, additions):
    with open(filepath, 'r', encoding='utf-8') as f:
        data = json.load(f)
    data.update(additions)
    with open(filepath, 'w', encoding='utf-8') as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
    print(f"Updated {filepath} successfully. Total keys: {len(data)}")

base_dir = r"c:\Users\Navegador\Desktop\TD\ING-SOFTWARE-BritezG\UI\Idiomas"
update_file(f"{base_dir}\\es.json", es_additions)
update_file(f"{base_dir}\\en.json", en_additions)
update_file(f"{base_dir}\\po.json", po_additions)
