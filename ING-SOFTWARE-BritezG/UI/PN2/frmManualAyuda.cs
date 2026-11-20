using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BLL;
using Services;

namespace UI.PN2
{
    public partial class frmManualAyuda : Form, IIdiomaObserver
    {
        private int _capituloActual = 1;

        public frmManualAyuda(int capituloInicial = 1)
        {
            InitializeComponent();
            _capituloActual = capituloInicial;
            MostrarCapitulo(_capituloActual);
            TraductorUI.SuscribirFormulario(this, this);
            ActualizarIdioma();
        }

        public void ActualizarIdioma()
        {
            if (ServicesSessionManager.Instancia.ObtenerIdioma() != null)
            {
                var idiomaBLL = new IdiomaBLL();
                TraductorUI.TraducirFormulario(this, idiomaBLL);
                MostrarCapitulo(_capituloActual);
            }
        }

        public void MostrarCapitulo(int numero)
        {
            _capituloActual = numero;
            ActualizarEstiloBotones(numero);

            var sb = new StringBuilder();

            switch (numero)
            {
                case 1:
                    lblCapituloTitulo.Text = "Capítulo 1: Bienvenida y Accesos Rápidos";
                    sb.AppendLine("BIENVENIDO AL SISTEMA NUTRIEVOLVE");
                    sb.AppendLine("================================================================================");
                    sb.AppendLine("NutriEvolve es un sistema diseñado especialmente para el consultorio de nutrición infantil.");
                    sb.AppendLine("Su propósito es facilitar el registro de las fichas de los niños, llevar el control");
                    sb.AppendLine("periódico de sus consultas y observar de manera clara cómo van creciendo.");
                    sb.AppendLine();
                    sb.AppendLine("1. ACCESOS RÁPIDOS Y AYUDA EN PANTALLA:");
                    sb.AppendLine("   * Tecla rápida F1: En cualquier pantalla puede presionar F1 para abrir esta ventana de ayuda.");
                    sb.AppendLine("   * Botón '?': En la esquina superior de cada ventana encontrará el botón de ayuda contextual");
                    sb.AppendLine("     que abre directamente las explicaciones de la pantalla donde se encuentra.");
                    sb.AppendLine("   * Menú Principal: Permite navegar rápidamente entre Pacientes, Consultas, Reportes e Historial.");
                    sb.AppendLine();
                    sb.AppendLine("SECCIONES DE AYUDA DISPONIBLES:");
                    sb.AppendLine("   -> Capítulo 2: Ficha y Gestión de Pacientes (Guía Botón por Botón)");
                    sb.AppendLine("   -> Capítulo 3: Teléfonos y Correos Electrónicos de Contacto");
                    sb.AppendLine("   -> Capítulo 4: Consultas Nutricionales y Curvas de Crecimiento OMS");
                    sb.AppendLine("   -> Capítulo 5: Historial y Registro de Cambios");
                    sb.AppendLine("   -> Capítulo 6: Reporte Predictivo de Crecimiento");
                    sb.AppendLine("   -> Capítulo 7: Copias de Seguridad y Cuidado de la Información");
                    break;

                case 2:
                    lblCapituloTitulo.Text = "Capítulo 2: Ficha y Gestión de Pacientes";
                    sb.AppendLine("GESTIÓN DE PACIENTES - GUÍA BOTÓN POR BOTÓN");
                    sb.AppendLine("================================================================================");
                    sb.AppendLine("Esta pantalla permite consultar la lista general de pacientes y administrar sus fichas.");
                    sb.AppendLine();
                    sb.AppendLine("¿QUÉ HACE CADA BOTÓN DE LA PANTALLA?:");
                    sb.AppendLine();
                    sb.AppendLine("1. BOTÓN 'AÑADIR':");
                    sb.AppendLine("   * Limpia y habilita los campos de texto inferiores para escribir los datos de un nuevo paciente.");
                    sb.AppendLine("   * El cursor se ubica en el campo DNI para comenzar la carga.");
                    sb.AppendLine();
                    sb.AppendLine("2. BOTÓN 'MODIFICAR':");
                    sb.AppendLine("   * Habilita los campos para corregir o actualizar la información del paciente seleccionado en la lista.");
                    sb.AppendLine();
                    sb.AppendLine("3. BOTÓN 'ELIMINAR':");
                    sb.AppendLine("   * Da de baja al paciente seleccionado.");
                    sb.AppendLine("   * Las consultas anteriores del paciente no se borran; su historia clínica queda protegida.");
                    sb.AppendLine();
                    sb.AppendLine("4. BOTÓN 'APLICAR':");
                    sb.AppendLine("   * Confirma y guarda los datos cargados o modificados en la lista de pacientes.");
                    sb.AppendLine();
                    sb.AppendLine("5. BOTÓN 'CANCELAR':");
                    sb.AppendLine("   * Cancela la operación actual y descarta cualquier cambio que no haya guardado.");
                    sb.AppendLine();
                    sb.AppendLine("6. BOTÓN 'LIMPIAR':");
                    sb.AppendLine("   * Vacía los campos de texto y filtros para realizar una nueva búsqueda.");
                    sb.AppendLine();
                    sb.AppendLine("7. BOTÓN 'SALIR':");
                    sb.AppendLine("   * Cierra la ventana y regresa a la pantalla principal del consultorio.");
                    sb.AppendLine();
                    sb.AppendLine("8. BOTONES 'EMAILS' Y 'TELÉFONOS':");
                    sb.AppendLine("   * Abren la ventana de contactos para registrar varios teléfonos o correos de la familia.");
                    sb.AppendLine();
                    sb.AppendLine("9. BOTÓN 'EXPORTAR (XML)':");
                    sb.AppendLine("   * Guarda una copia de la lista de pacientes en un archivo en su computadora.");
                    sb.AppendLine();
                    sb.AppendLine("10. BOTÓN 'IMPORTAR (XML)':");
                    sb.AppendLine("   * Permite cargar pacientes desde un archivo hacia el sistema.");
                    break;

                case 3:
                    lblCapituloTitulo.Text = "Capítulo 3: Teléfonos y Correos Electrónicos de Contacto";
                    sb.AppendLine("GESTIÓN DE TELÉFONOS Y CORREOS DE CONTACTO");
                    sb.AppendLine("================================================================================");
                    sb.AppendLine("1. GESTIÓN DE CONTACTOS (EMAILS Y TELÉFONOS):");
                    sb.AppendLine("   * Es muy común que un niño necesite más de un contacto registrado (el celular de la mamá,");
                    sb.AppendLine("     el del papá, un teléfono fijo del hogar o el contacto de un tutor).");
                    sb.AppendLine("   * El sistema permite guardar todos los contactos necesarios para asegurar una comunicación ágil.");
                    sb.AppendLine();
                    sb.AppendLine("2. VENTANA DE CONTACTOS Y SUS BOTONES:");
                    sb.AppendLine("   * BOTÓN 'AÑADIR CONTACTO': Abre un espacio para escribir un nuevo número o correo.");
                    sb.AppendLine("   * BOTÓN 'MODIFICAR CONTACTO': Permite corregir o actualizar un contacto si la familia cambió de número.");
                    sb.AppendLine("   * BOTÓN 'ELIMINAR CONTACTO': Quita de la lista un contacto que ya no esté en uso.");
                    sb.AppendLine("   * CASILLA 'PRINCIPAL': Permite elegir cuál es el número o correo preferido para avisos y turnos.");
                    sb.AppendLine("   * BOTÓN 'ACEPTAR / GUARDAR': Guarda todos los contactos ingresados y vuelve a la ficha del paciente.");
                    sb.AppendLine("   * BOTÓN 'CANCELAR': Cierra la ventana sin realizar cambios en los contactos.");
                    break;

                case 4:
                    lblCapituloTitulo.Text = "Capítulo 4: Consultas Nutricionales y Curvas de Crecimiento";
                    sb.AppendLine("CONSULTAS NUTRICIONALES Y CURVAS DE CRECIMIENTO OMS");
                    sb.AppendLine("================================================================================");
                    sb.AppendLine("1. REGISTRO DE LA CONSULTA MÉDICA:");
                    sb.AppendLine("   * Permite registrar la visita médica del niño: antecedentes, alimentación, hábitos y alergias.");
                    sb.AppendLine("   * Al ingresar el peso y la estatura, el sistema calcula de inmediato el estado del paciente.");
                    sb.AppendLine();
                    sb.AppendLine("2. CURVAS DE CRECIMIENTO DE LA OMS:");
                    sb.AppendLine("   * El sistema ubica automáticamente al niño en las curvas oficiales de la OMS.");
                    sb.AppendLine("   * Permite ver de forma gráfica si el niño está en su peso y talla normales para su edad,");
                    sb.AppendLine("     o si presenta bajo peso o sobrepeso, facilitando la explicación a los padres.");
                    sb.AppendLine();
                    sb.AppendLine("3. GUÍA DE BOTONES DE LA CONSULTA:");
                    sb.AppendLine("   * BOTÓN 'NUEVA CONSULTA': Limpia la pantalla para iniciar la carga de un nuevo control médico.");
                    sb.AppendLine("   * BOTÓN 'GUARDAR CONSULTA': Registra la visita, las medidas físicas y el diagnóstico en la ficha.");
                    sb.AppendLine("   * BOTÓN 'VER GRÁFICO / CURVAS OMS': Abre el gráfico interactivo con las curvas de crecimiento.");
                    sb.AppendLine("   * BOTÓN 'IMPRIMIR COMPROBANTE': Emite la ficha oficial con el plan alimentario y recomendaciones.");
                    sb.AppendLine("   * BOTÓN 'SALIR': Cierra la ventana de consultas.");
                    break;

                case 5:
                    lblCapituloTitulo.Text = "Capítulo 5: Historial y Registro de Cambios";
                    sb.AppendLine("HISTORIAL Y REGISTRO DE CAMBIOS");
                    sb.AppendLine("================================================================================");
                    sb.AppendLine("1. ¿PARA QUÉ SIRVE EL HISTORIAL DE CAMBIOS?:");
                    sb.AppendLine("   * Guarda un registro automático de cada modificación realizada en las fichas de los pacientes");
                    sb.AppendLine("     (qué dato cambió, qué usuario lo hizo y en qué fecha y hora).");
                    sb.AppendLine("   * Permite al consultorio saber siempre cómo fue evolucionando la información de cada ficha.");
                    sb.AppendLine();
                    sb.AppendLine("2. RECUPERAR UNA VERSIÓN ANTERIOR:");
                    sb.AppendLine("   * Si alguien comete un error al modificar una ficha médica, el sistema conserva todas");
                    sb.AppendLine("     las versiones anteriores y permite restaurar los datos correctos con un solo clic.");
                    sb.AppendLine();
                    sb.AppendLine("3. GUÍA DE BOTONES DEL HISTORIAL:");
                    sb.AppendLine("   * BOTÓN 'BUSCAR / FILTRAR': Permite buscar modificaciones por paciente o rango de fechas.");
                    sb.AppendLine("   * BOTÓN 'LIMPIAR': Restablece la búsqueda para ver todas las modificaciones recientes.");
                    sb.AppendLine("   * BOTÓN 'ACTIVAR VERSIÓN ANTERIOR': Restaura los datos del paciente al estado de la fecha elegida.");
                    sb.AppendLine("   * BOTÓN 'SALIR': Cierra la ventana de historial.");
                    break;

                case 6:
                    lblCapituloTitulo.Text = "Capítulo 6: Reporte Predictivo de Crecimiento";
                    sb.AppendLine("REPORTE PREDICTIVO DE CRECIMIENTO INFANTIL");
                    sb.AppendLine("================================================================================");
                    sb.AppendLine("1. ¿QUÉ ES EL REPORTE PREDICTIVO?:");
                    sb.AppendLine("   * Analiza las visitas anteriores del niño y estima cómo continuará su peso y estatura");
                    sb.AppendLine("     en los próximos tres meses si mantiene sus hábitos actuales.");
                    sb.AppendLine();
                    sb.AppendLine("2. SEMÁFORO DE SALUD Y RECOMENDACIONES:");
                    sb.AppendLine("   * SEMÁFORO VERDE: El crecimiento evoluciona de manera normal y favorable.");
                    sb.AppendLine("   * SEMÁFORO AMARILLO: Crecimiento más lento o aumento leve; se aconseja control preventivo.");
                    sb.AppendLine("   * SEMÁFORO ROJO: Alerta clínica; se recomienda programar una visita médica más cercana.");
                    sb.AppendLine();
                    sb.AppendLine("3. GUÍA DE BOTONES DEL REPORTE:");
                    sb.AppendLine("   * BOTÓN 'GENERAR REPORTE': Realiza el cálculo para el paciente seleccionado.");
                    sb.AppendLine("   * BOTÓN 'IMPRIMIR': Imprime el reporte para adjuntarlo a la historia clínica.");
                    sb.AppendLine("   * BOTÓN 'SALIR': Cierra la pantalla de reportes.");
                    break;

                case 7:
                    lblCapituloTitulo.Text = "Capítulo 7: Copias de Seguridad y Cuidado de la Información";
                    sb.AppendLine("COPIAS DE SEGURIDAD Y CUIDADO DE LA INFORMACIÓN");
                    sb.AppendLine("================================================================================");
                    sb.AppendLine("1. CUIDADO DE LOS DATOS DEL CONSULTORIO:");
                    sb.AppendLine("   * Las fichas de los pacientes son lo más valioso del consultorio.");
                    sb.AppendLine("   * Realizar copias de seguridad regularmente garantiza que la información nunca se pierda.");
                    sb.AppendLine();
                    sb.AppendLine("2. PROTECCIÓN AUTOMÁTICA DE LA INFORMACIÓN:");
                    sb.AppendLine("   * El sistema verifica de forma constante que ningún dato esté dañado ni alterado.");
                    sb.AppendLine("   * Si alguna vez necesita reinstalar el sistema, sus datos existentes no serán borrados.");
                    sb.AppendLine();
                    sb.AppendLine("3. GUÍA DE BOTONES DE RESPALDO:");
                    sb.AppendLine("   * BOTÓN 'CREAR COPIA DE SEGURIDAD': Guarda todos los datos en una carpeta segura o pendrive.");
                    sb.AppendLine("   * BOTÓN 'RESTAURAR COPIA': Recupera los datos a partir de una copia previa.");
                    sb.AppendLine("   * BOTÓN 'SALIR': Cierra la pantalla de respaldo.");
                    break;
            }

            rtbContenido.Text = sb.ToString();
        }

        private void ActualizarEstiloBotones(int activo)
        {
            var botones = new[] { btnTema1, btnTema2, btnTema3, btnTema4, btnTema5, btnTema6, btnTema7 };
            for (int i = 0; i < botones.Length; i++)
            {
                if (botones[i] != null)
                {
                    if (i + 1 == activo)
                    {
                        botones[i].ColorPrincipal = Color.FromArgb(70, 70, 70);
                        botones[i].ColorSecundario = Color.FromArgb(40, 40, 40);
                        botones[i].ForeColor = Color.White;
                        botones[i].EsBotonSecundario = false;
                    }
                    else
                    {
                        botones[i].ColorPrincipal = Color.FromArgb(245, 245, 245);
                        botones[i].ColorSecundario = Color.FromArgb(230, 230, 230);
                        botones[i].ForeColor = Color.FromArgb(40, 40, 40);
                        botones[i].EsBotonSecundario = true;
                    }
                    botones[i].Invalidate();
                }
            }
        }

        private void btnExportarPdf_Click(object? sender, EventArgs e)
        {
            try
            {
                using var sfd = new SaveFileDialog();
                sfd.Filter = "Documento PDF (*.pdf)|*.pdf";
                sfd.FileName = "Manual_de_Ayuda_NutriEvolve.pdf";
                sfd.Title = "Guardar Manual de Ayuda en PDF";

                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    ManualAyudaPdfGenerator.GenerarPdf(sfd.FileName);
                    MessageBox.Show("¡El Manual de Ayuda se ha generado y exportado exitosamente a PDF!", "Exportación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar el manual de ayuda a PDF: {ex.Message}", "Error de Exportación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
