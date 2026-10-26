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
                    lblCapituloTitulo.Text = "Capítulo 1: Arquitectura y Navegación General";
                    sb.AppendLine("BIENVENIDO AL SISTEMA NUTRIEVOLVE");
                    sb.AppendLine("================================================================================");
                    sb.AppendLine("NutriEvolve es un sistema integral de gestión clínica pediátrica y seguimiento");
                    sb.AppendLine("nutricional basado en estándares antropométricos de la Organización Mundial de la Salud (OMS).");
                    sb.AppendLine();
                    sb.AppendLine("1. ARQUITECTURA MULTICAPA:");
                    sb.AppendLine("   * Capa de Presentación (UI): WinForms con controles visuales limpios y accesibles.");
                    sb.AppendLine("   * Capa de Negocio (BLL): Lógica clínica, z-scores OMS, y motor de inferencia.");
                    sb.AppendLine("   * Capa de Acceso a Datos (DAL): Desacoplada mediante ADO.NET tipado y transaccional.");
                    sb.AppendLine("   * Capa de Entidades (BE): Modelos de dominio sin lógica de persistencia.");
                    sb.AppendLine("   * Servicios de Soporte: Sesión singleton, bcrypt, integridad con dígitos verificadores.");
                    sb.AppendLine();
                    sb.AppendLine("2. ACCESOS RÁPIDOS Y AYUDA CONTEXTUAL:");
                    sb.AppendLine("   * Presione la tecla F1 en cualquier pantalla del sistema para invocar esta ayuda.");
                    sb.AppendLine("   * Cada formulario dispone de un botón '?' en su barra superior para abrir su capítulo temático.");
                    sb.AppendLine();
                    sb.AppendLine("ENLACES RELACIONADOS:");
                    sb.AppendLine("   -> Ver Capítulo 2: Maestro de Clientes (ABM-C)");
                    sb.AppendLine("   -> Ver Capítulo 4: Seguimiento Nutricional y Curvas OMS");
                    sb.AppendLine("   -> Ver Capítulo 7: Procedimiento de Reinstalación Segura");
                    break;

                case 2:
                    lblCapituloTitulo.Text = "Capítulo 2: Maestro de Clientes / Pacientes (ABM-C Clásico)";
                    sb.AppendLine("MAESTRO DE CLIENTES - DIRECTIVAS OPERATIVAS");
                    sb.AppendLine("================================================================================");
                    sb.AppendLine("El formulario de Maestro de Clientes implementa el patrón clásico ABM-C");
                    sb.AppendLine("para el mantenimiento de entidades maestras con máquina de estados finitos.");
                    sb.AppendLine();
                    sb.AppendLine("ESTADOS Y SECUENCIA DE BOTONES (EXPLICACIÓN BOTÓN POR BOTÓN):");
                    sb.AppendLine();
                    sb.AppendLine("1. ESTADO INICIAL (INICIO):");
                    sb.AppendLine("   * Grilla superior habilitada para navegación.");
                    sb.AppendLine("   * Campos de edición inferiores bloqueados (Solo lectura).");
                    sb.AppendLine("   * Botones habilitados: 'Añadir', 'Modificar' y 'Eliminar' (si hay selección), y 'Salir'.");
                    sb.AppendLine("   * Botones deshabilitados: 'Aplicar' y 'Cancelar'.");
                    sb.AppendLine();
                    sb.AppendLine("2. BOTÓN 'AÑADIR':");
                    sb.AppendLine("   * Limpia los campos de texto inferiores.");
                    sb.AppendLine("   * Desbloquea todos los campos para permitir el ingreso de datos.");
                    sb.AppendLine("   * Deshabilita la grilla y los botones de Añadir/Modificar/Eliminar/Salir.");
                    sb.AppendLine("   * Habilita los botones 'Aplicar' y 'Cancelar'. El foco se sitúa en el DNI.");
                    sb.AppendLine();
                    sb.AppendLine("3. BOTÓN 'MODIFICAR':");
                    sb.AppendLine("   * Carga los datos de la fila seleccionada en los campos inferiores.");
                    sb.AppendLine("   * Desbloquea los campos editables (Apellidos, Nombres, Email, Celular, Dirección).");
                    sb.AppendLine("   * El campo DNI permanece bloqueado por ser la clave primaria invariante.");
                    sb.AppendLine("   * Habilita 'Aplicar' y 'Cancelar', deshabilitando la grilla.");
                    sb.AppendLine();
                    sb.AppendLine("4. BOTÓN 'ELIMINAR':");
                    sb.AppendLine("   * Solicita confirmación explícita al usuario.");
                    sb.AppendLine("   * Ejecuta la BAJA LÓGICA (Activo = 0) sin pérdida física de registros históricos.");
                    sb.AppendLine();
                    sb.AppendLine("5. BOTÓN 'APLICAR':");
                    sb.AppendLine("   * Valida la obligatoriedad de campos esenciales (DNI, Apellidos, Nombres).");
                    sb.AppendLine("   * Persiste la operación (INSERT en alta, UPDATE en modificación) en la base de datos.");
                    sb.AppendLine("   * Sincroniza automáticamente los datos de contacto principales en las tablas 1:N.");
                    sb.AppendLine("   * Recarga la grilla y retorna automáticamente al Estado Inicial.");
                    sb.AppendLine();
                    sb.AppendLine("6. BOTÓN 'CANCELAR':");
                    sb.AppendLine("   * Descarta cualquier cambio no confirmado y retorna al Estado Inicial.");
                    sb.AppendLine();
                    sb.AppendLine("7. BOTÓN 'SALIR':");
                    sb.AppendLine("   * Cierra el formulario y retorna al entorno principal.");
                    sb.AppendLine();
                    sb.AppendLine("ENLACES RELACIONADOS:");
                    sb.AppendLine("   -> Ver Capítulo 3: Tablas 1:N y Despliegue de Ventanas de Contactos");
                    sb.AppendLine("   -> Ver Capítulo 5: Bitácora de Cambios y Rollback de Pacientes");
                    break;

                case 3:
                    lblCapituloTitulo.Text = "Capítulo 3: Tablas 1:N y Repercusión en Modificación y Agregado";
                    sb.AppendLine("GESTIÓN DE CONTACTOS 1:N (EMAILS Y TELÉFONOS)");
                    sb.AppendLine("================================================================================");
                    sb.AppendLine("Un cliente/paciente puede registrar múltiples direcciones de correo electrónico");
                    sb.AppendLine("y múltiples números de contacto (celular, fijo, urgencias) mediante tablas 1:n.");
                    sb.AppendLine();
                    sb.AppendLine("1. VENTANAS ADICIONALES DESPLEGABLES:");
                    sb.AppendLine("   * Al presionar los botones 'Emails 1:N' o 'Tels 1:N' en el maestro de clientes,");
                    sb.AppendLine("     se despliega la ventana modal auxiliar 'frmContactos1N_DNI101'.");
                    sb.AppendLine("   * Permite incorporar múltiples contactos asignando flags de 'Principal'.");
                    sb.AppendLine();
                    sb.AppendLine("2. REPERCUSIONES EN MODIFICACIÓN Y AGREGADO:");
                    sb.AppendLine("   * Al agregar un nuevo cliente desde el maestro, los valores principales de Email");
                    sb.AppendLine("     y Celular se insertan atómicamente en 'Paciente_Emails_DNI101' y");
                    sb.AppendLine("     'Paciente_Telefonos_DNI101' bajo la misma transacción.");
                    sb.AppendLine("   * Al modificar el email o celular principal en el maestro, se propaga");
                    sb.AppendLine("     automáticamente la actualización a las tablas hijas 1:N.");
                    sb.AppendLine();
                    sb.AppendLine("3. BAJA LÓGICA Y REPERCUSIÓN 1:N:");
                    sb.AppendLine("   * La eliminación de contactos individuales es LÓGICA (Activo_DNI101 = 0).");
                    sb.AppendLine("   * Si se da de baja lógica al cliente principal, la transacción marca en cascada");
                    sb.AppendLine("     como inactivos todos sus emails y teléfonos asociados, garantizando la");
                    sb.AppendLine("     integridad referencial y preservando auditorías clínicas.");
                    sb.AppendLine();
                    sb.AppendLine("ENLACES RELACIONADOS:");
                    sb.AppendLine("   -> Ver Capítulo 2: Maestro de Clientes (ABM-C)");
                    sb.AppendLine("   -> Ver Capítulo 5: Bitácora de Auditoría");
                    break;

                case 4:
                    lblCapituloTitulo.Text = "Capítulo 4: Consultas Nutricionales, Curvas OMS y Comprobante Formal";
                    sb.AppendLine("MÓDULO DE SEGUIMIENTO NUTRICIONAL PEDIÁTRICO (PN2)");
                    sb.AppendLine("================================================================================");
                    sb.AppendLine("1. REGISTRO DE CONSULTA CLÍNICA:");
                    sb.AppendLine("   * Captura anamnesis (lactancia, alimentación complementaria, alergias, antecedentes).");
                    sb.AppendLine("   * Mediciones antropométricas: Peso (kg), Talla (cm), Perímetro Cefálico, IMC.");
                    sb.AppendLine();
                    sb.AppendLine("2. CURVAS Y Z-SCORES OMS:");
                    sb.AppendLine("   * Cálculo exacto de Z-Score: Z = ((Medicion / M)^L - 1) / (L * S).");
                    sb.AppendLine("   * Graficación en tiempo real sobre percentiles P3, P15, P50, P85 y P97.");
                    sb.AppendLine("   * Generación de alertas clínicas automáticas ante desvíos mayores a +/- 2 DS.");
                    sb.AppendLine();
                    sb.AppendLine("3. COMPROBANTE FORMAL IMPRIMIBLE (FICHA DE PRESCRIPCIÓN):");
                    sb.AppendLine("   * Permite emitir una constancia oficial con membrete del consultorio,");
                    sb.AppendLine("     detalles antropométricos, plan calórico, distribución de macronutrientes,");
                    sb.AppendLine("     metas de salud, pautas familiares y firma profesional.");
                    sb.AppendLine("   * Cuenta con vista preliminar en pantalla, envío a impresora física y exportación.");
                    sb.AppendLine();
                    sb.AppendLine("ENLACES RELACIONADOS:");
                    sb.AppendLine("   -> Ver Capítulo 5: Bitácora de Cambios en Consultas y Planes");
                    sb.AppendLine("   -> Ver Capítulo 6: Reporte Inteligente Predictivo");
                    break;

                case 5:
                    lblCapituloTitulo.Text = "Capítulo 5: Bitácora de Cambios con Trigger SQL y Mecanismo Rollback";
                    sb.AppendLine("BITÁCORA DE CAMBIOS vs BITÁCORA DE EVENTOS");
                    sb.AppendLine("================================================================================");
                    sb.AppendLine("1. DIFERENCIA CONCEPTUAL FUNDAMENTAL:");
                    sb.AppendLine("   * Bitácora de Eventos (Seguridad): Registra 'QUÉ SUCEDIÓ' a nivel sistema");
                    sb.AppendLine("     (logins, cierres de sesión, backups, errores con criticidad 1-5).");
                    sb.AppendLine("   * Bitácora de Cambios (Auditoría de Datos): Registra 'EL ESTADO DE LOS DATOS'");
                    sb.AppendLine("     antes y después de una modificación sobre entidades del negocio.");
                    sb.AppendLine();
                    sb.AppendLine("2. DISPARADORES SQL (TRIGGERS AFTER UPDATE/DELETE):");
                    sb.AppendLine("   * Consultas Nutricionales: 'trg_ConsultasNutricionales_BitacoraCambios'.");
                    sb.AppendLine("   * Planes Alimentarios: 'trg_PlanesAlimentarios_BitacoraCambios'.");
                    sb.AppendLine("   * Pacientes: 'trg_Pacientes_BitacoraCambios'.");
                    sb.AppendLine("   * Los triggers leen la pseudotabla 'deleted' del motor SQL Server e insertan");
                    sb.AppendLine("     el estado previo junto con el usuario autenticado (SESSION_CONTEXT).");
                    sb.AppendLine();
                    sb.AppendLine("3. MECANISMO DE ROLLBACK (DESHACER CAMBIOS):");
                    sb.AppendLine("   * La pantalla de Bitácora de Cambios permite seleccionar cualquier versión histórica.");
                    sb.AppendLine("   * Al pulsar 'Revertir a esta Versión (Rollback)', el sistema restaura los datos de");
                    sb.AppendLine("     la entidad activa, provocando que el Trigger audite la reversión como un nuevo evento.");
                    sb.AppendLine();
                    sb.AppendLine("ENLACES RELACIONADOS:");
                    sb.AppendLine("   -> Ver Capítulo 4: Consultas Nutricionales");
                    sb.AppendLine("   -> Ver Capítulo 2: Maestro de Clientes");
                    break;

                case 6:
                    lblCapituloTitulo.Text = "Capítulo 6: Reporte Inteligente Predictivo y Soporte de Decisiones";
                    sb.AppendLine("REPORTE INTELIGENTE PREDICTIVO");
                    sb.AppendLine("================================================================================");
                    sb.AppendLine("No es un simple reporte CRUD estático; cruza longitudinalmente múltiples tablas");
                    sb.AppendLine("clínicas (Pacientes, Consultas, Mediciones, Z-Scores) para proyectar evolución.");
                    sb.AppendLine();
                    sb.AppendLine("1. MÉTRICAS CALCULADAS:");
                    sb.AppendLine("   * Velocidad Ponderal: Vp = Delta Peso / Delta Meses (kg/mes).");
                    sb.AppendLine("   * Velocidad de Crecimiento Longitudinal: Vt = Delta Talla / Delta Meses (cm/mes).");
                    sb.AppendLine("   * Deriva de Z-Score IMC: Delta Z-Score entre controles sucesivos.");
                    sb.AppendLine();
                    sb.AppendLine("2. PROYECCIÓN CLÍNICA A 90 DÍAS:");
                    sb.AppendLine("   * Predice el peso y talla esperados si continúa la tendencia observada.");
                    sb.AppendLine("   * Clasifica el riesgo futuro con semáforo de colores: Bajo, Moderado, Alto, Crítico.");
                    sb.AppendLine();
                    sb.AppendLine("3. RECOMENDACIONES DEL MOTOR DE DECISIÓN:");
                    sb.AppendLine("   * Frecuencia sugerida para el próximo control (ej: adelantar a 15 días en riesgo agudo).");
                    sb.AppendLine("   * Pautas dietoterápicas y suplementación específica adaptadas al paciente.");
                    sb.AppendLine();
                    sb.AppendLine("ENLACES RELACIONADOS:");
                    sb.AppendLine("   -> Ver Capítulo 4: Seguimiento Nutricional");
                    sb.AppendLine("   -> Ver Capítulo 5: Bitácora de Auditoría");
                    break;

                case 7:
                    lblCapituloTitulo.Text = "Capítulo 7: Reinstalador Seguro, Respaldo y Protección de Datos";
                    sb.AppendLine("DIRECTIVAS DE REINSTALACIÓN Y PRESERVACIÓN DE BASE DE DATOS");
                    sb.AppendLine("================================================================================");
                    sb.AppendLine("1. ADVERTENCIA PREVIA OBLIGATORIA:");
                    sb.AppendLine("   Realice un backup completo de la base de datos antes de reinstalar.");
                    sb.AppendLine();
                    sb.AppendLine("2. GARANTÍA DE NO SOBREESCRITURA DEL REINSTALADOR:");
                    sb.AppendLine("   * El instalador de NutriEvolve detecta si el catálogo SQL ya existe en el servidor.");
                    sb.AppendLine("   * En modo reinstalación, el ejecutable OMITIRÁ automáticamente cualquier");
                    sb.AppendLine("     sentencia destructiva (DELETE, DROP, TRUNCATE) o inserciones de prueba.");
                    sb.AppendLine("   * Se garantiza que la base de datos de producción NO SERÁ PISADA NI BORRADA.");
                    sb.AppendLine();
                    sb.AppendLine("3. INTEGRIDAD CON DÍGITOS VERIFICADORES:");
                    sb.AppendLine("   * Cada tabla protegida cuenta con Dígitos Verificadores Horizontales (DVH)");
                    sb.AppendLine("     y Verticales (DVV) para asegurar que ningún registro sea alterado externamente.");
                    sb.AppendLine();
                    sb.AppendLine("ENLACES RELACIONADOS:");
                    sb.AppendLine("   -> Ver Capítulo 1: Arquitectura General");
                    sb.AppendLine("   -> Módulo Respaldo y Restauración en Menú Principal");
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
                        botones[i].ColorPrincipal = Color.FromArgb(76, 124, 89);
                        botones[i].ColorSecundario = Color.FromArgb(50, 85, 60);
                        botones[i].ForeColor = Color.White;
                        botones[i].EsBotonSecundario = false;
                    }
                    else
                    {
                        botones[i].ColorPrincipal = Color.FromArgb(235, 242, 236);
                        botones[i].ColorSecundario = Color.FromArgb(215, 225, 218);
                        botones[i].ForeColor = Color.FromArgb(40, 70, 50);
                        botones[i].EsBotonSecundario = true;
                    }
                    botones[i].Invalidate();
                }
            }
        }
    }
}
