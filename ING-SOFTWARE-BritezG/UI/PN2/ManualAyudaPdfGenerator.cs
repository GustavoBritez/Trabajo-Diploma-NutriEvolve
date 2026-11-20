using System;
using System.Collections.Generic;
using System.IO;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace UI.PN2
{
    /// <summary>
    /// Generador del Manual de Ayuda al Usuario del Sistema NutriEvolve en formato PDF.
    /// Enfocado en el usuario final: lenguaje simple y natural, sin tecnicismos de programación,
    /// sin colores invasivos (diseño sobrio y limpio), y con explicación botón por botón de cada pantalla.
    /// </summary>
    public static class ManualAyudaPdfGenerator
    {
        public static void GenerarPdf(string rutaArchivo)
        {
            var doc = new PdfDocument();
            doc.Info.Title = "NutriEvolve - Manual de Ayuda al Usuario";
            doc.Info.Author = "Sistema NutriEvolve";
            doc.Info.Subject = "Guía Práctica y Funciones de Pantallas";
            doc.Info.Keywords = "NutriEvolve, Ayuda, Manual, Usuario, Pacientes, Consultas, Curvas OMS";

            var builder = new DocumentBuilder(doc);
            builder.ConstruirManualCompleto();

            doc.Save(rutaArchivo);
        }

        private class DocumentBuilder
        {
            private readonly PdfDocument _doc;
            private PdfPage _page = null!;
            private XGraphics _gfx = null!;
            private double _y;

            private const double MarginLeft = 45;
            private const double MarginRight = 45;
            private const double MarginTop = 50;
            private const double MarginBottom = 50;
            private double UsableWidth => _page.Width - MarginLeft - MarginRight;
            private double MaxY => _page.Height - MarginBottom;

            // Paleta sobria y limpia (escala de grises / minimalista sin colores invasivos)
            private readonly XColor _colDark = XColor.FromArgb(25, 25, 25);
            private readonly XColor _colGray = XColor.FromArgb(70, 70, 70);
            private readonly XColor _colLightBg = XColor.FromArgb(248, 248, 248);
            private readonly XColor _colBorder = XColor.FromArgb(205, 205, 205);
            private readonly XColor _colTextDark = XColor.FromArgb(30, 30, 30);
            private readonly XColor _colTextMuted = XColor.FromArgb(100, 100, 100);

            // Tipografías
            private readonly XFont _fCoverTitle = new("Segoe UI", 26, XFontStyle.Bold);
            private readonly XFont _fCoverSub = new("Segoe UI", 13, XFontStyle.Bold);
            private readonly XFont _fCoverTag = new("Segoe UI", 10.5, XFontStyle.Regular);
            private readonly XFont _fChapterNum = new("Segoe UI", 9.5, XFontStyle.Bold);
            private readonly XFont _fChapterTitle = new("Segoe UI", 13, XFontStyle.Bold);
            private readonly XFont _fSecTitle = new("Segoe UI", 10.5, XFontStyle.Bold);
            private readonly XFont _fBody = new("Segoe UI", 9.5, XFontStyle.Regular);
            private readonly XFont _fBodyBold = new("Segoe UI", 9.5, XFontStyle.Bold);
            private readonly XFont _fBodyItalic = new("Segoe UI", 9, XFontStyle.Italic);
            private readonly XFont _fHeader = new("Segoe UI", 8, XFontStyle.Regular);
            private readonly XFont _fFooter = new("Segoe UI", 8, XFontStyle.Regular);
            private readonly XFont _fBoxTitle = new("Segoe UI", 9.5, XFontStyle.Bold);
            private readonly XFont _fBoxBody = new("Segoe UI", 9, XFontStyle.Regular);

            public DocumentBuilder(PdfDocument doc)
            {
                _doc = doc;
            }

            public void ConstruirManualCompleto()
            {
                // 1. Portada Limpia y Sobria
                GenerarPortada();

                // 2. Índice General
                NuevaPagina(true);
                GenerarIndiceGeneral();

                // 3. Capítulo 1: Bienvenida y Accesos Rápidos
                NuevaPagina();
                GenerarCapitulo1();

                // 4. Capítulo 2: Ficha y Gestión de Pacientes (Botón por Botón)
                GenerarCapitulo2();

                // 5. Capítulo 3: Teléfonos y Correos Electrónicos de Contacto
                GenerarCapitulo3();

                // 6. Capítulo 4: Consultas Nutricionales y Curvas OMS
                GenerarCapitulo4();

                // 7. Capítulo 5: Historial y Registro de Cambios
                GenerarCapitulo5();

                // 8. Capítulo 6: Reporte Predictivo de Crecimiento
                GenerarCapitulo6();

                // 9. Capítulo 7: Copias de Seguridad y Cuidado de la Información
                GenerarCapitulo7();

                // 10. Anexo: Guardar y Cargar Pacientes en Archivo
                GenerarAnexo();

                _gfx?.Dispose();

                // 11. Estampado de encabezados y pies de página
                EstamparEncabezadosYPie();
            }

            private void NuevaPagina(bool esPrimeraTrasPortada = false)
            {
                _gfx?.Dispose();
                _page = _doc.AddPage();
                _page.Size = PdfSharpCore.PageSize.A4;
                _gfx = XGraphics.FromPdfPage(_page);
                _y = MarginTop;
            }

            private void VerificarEspacio(double altoNecesario)
            {
                if (_y + altoNecesario > MaxY)
                {
                    NuevaPagina();
                }
            }

            #region Portada

            private void GenerarPortada()
            {
                _gfx?.Dispose();
                _page = _doc.AddPage();
                _page.Size = PdfSharpCore.PageSize.A4;
                _gfx = XGraphics.FromPdfPage(_page);

                // Marco exterior fino y elegante
                _gfx.DrawRectangle(new XPen(_colBorder, 1), MarginLeft - 10, MarginTop - 10, UsableWidth + 20, _page.Height - MarginTop - MarginBottom + 20);

                double y = 90;

                // Título Principal
                _gfx.DrawString("NUTRIEVOLVE", _fCoverTitle, new XSolidBrush(_colDark), new XPoint(MarginLeft, y));
                y += 28;
                _gfx.DrawString("SISTEMA DE GESTIÓN CLÍNICA Y NUTRICIÓN PEDIÁTRICA", _fCoverTag, new XSolidBrush(_colGray), new XPoint(MarginLeft, y));
                y += 18;
                _gfx.DrawLine(new XPen(_colDark, 1.5), MarginLeft, y, MarginLeft + UsableWidth, y);
                y += 35;

                // Subtítulo del manual
                _gfx.DrawString("MANUAL DE AYUDA AL USUARIO", _fCoverSub, new XSolidBrush(_colDark), new XPoint(MarginLeft, y));
                y += 18;
                _gfx.DrawString("Guía práctica de uso del sistema y funciones de cada pantalla.", _fBody, new XSolidBrush(_colTextDark), new XPoint(MarginLeft, y));
                y += 35;

                // Tarjeta Informativa para el Usuario
                double boxHeight = 150;
                _gfx.DrawRoundedRectangle(new XPen(_colBorder, 1), new XSolidBrush(_colLightBg), MarginLeft, y, UsableWidth, boxHeight, 4, 4);

                double by = y + 22;
                double bx = MarginLeft + 18;
                _gfx.DrawString("GUÍA DE USO Y AYUDA DEL SISTEMA:", _fSecTitle, new XSolidBrush(_colDark), new XPoint(bx, by));
                by += 22;

                string[] metadata = {
                    "• Destinatarios: Profesionales de la salud, nutricionistas pediátricos y personal del consultorio.",
                    "• Objetivo: Brindar una referencia clara de para qué sirve cada ventana y qué hace cada botón.",
                    "• Facilidad de uso: Redacción en lenguaje natural, sin términos técnicos de programación.",
                    "• Asistencia en todo momento: Puede presionar la tecla F1 en cualquier pantalla para consultar la ayuda.",
                    "• Botón '?': Cada ventana cuenta con un botón de ayuda contextual en la esquina superior."
                };

                foreach (var line in metadata)
                {
                    _gfx.DrawString(line, _fBody, new XSolidBrush(_colTextDark), new XPoint(bx, by));
                    by += 20;
                }

                y += boxHeight + 40;

                // Cuadro de Consejo Práctico
                double calloutH = 75;
                _gfx.DrawRoundedRectangle(new XPen(_colBorder, 1.2), new XSolidBrush(_colLightBg), MarginLeft, y, UsableWidth, calloutH, 4, 4);

                double cy = y + 22;
                double cx = MarginLeft + 18;
                _gfx.DrawString("CONSEJO PRÁCTICO:", _fBoxTitle, new XSolidBrush(_colDark), new XPoint(cx, cy));
                cy += 18;
                _gfx.DrawString("Recuerde hacer clic en el botón 'Guardar' o 'Aplicar' para confirmar cualquier nuevo registro o cambio.", _fBoxBody, new XSolidBrush(_colTextDark), new XPoint(cx, cy));
                cy += 16;
                _gfx.DrawString("Si comete algún error al escribir, siempre puede presionar 'Cancelar' para no guardar la modificación.", _fBoxBody, new XSolidBrush(_colTextDark), new XPoint(cx, cy));

                y += calloutH + 60;

                // Pie de portada
                _gfx.DrawLine(new XPen(_colBorder, 1), MarginLeft, y, MarginLeft + UsableWidth, y);
                y += 20;
                _gfx.DrawString("Sistema de Gestión NutriEvolve - Guía del Usuario", _fBodyItalic, new XSolidBrush(_colTextMuted), new XPoint(MarginLeft, y));
                _gfx.DrawString($"Fecha: {DateTime.Now:dd/MM/yyyy}", _fBodyBold, new XSolidBrush(_colDark), new XPoint(MarginLeft + UsableWidth - 120, y));
            }

            #endregion

            #region Índice General

            private void GenerarIndiceGeneral()
            {
                DibujarEncabezadoCapitulo("ÍNDICE GENERAL Y GUÍA DE REFERENCIA", "CONTENIDO");

                DibujarParrafo("Este manual está organizado en secciones cortas y prácticas. Puede acceder a la ayuda de cualquier pantalla presionando la tecla F1 o haciendo clic en el botón '? Ayuda' en la ventana donde se encuentre:");

                _y += 10;

                var capitulos = new[]
                {
                    ("Capítulo 1", "Bienvenida y Accesos Rápidos", "Primeros pasos en el sistema, navegación y uso de las teclas de ayuda."),
                    ("Capítulo 2", "Ficha y Gestión de Pacientes", "Cómo dar de alta, buscar, modificar datos y qué hace cada botón de la pantalla."),
                    ("Capítulo 3", "Teléfonos y Correos Electrónicos", "Cómo registrar varios teléfonos y correos para la familia del paciente."),
                    ("Capítulo 4", "Consultas y Curvas OMS", "Carga de peso y talla, interpretación visual del crecimiento e impresión."),
                    ("Capítulo 5", "Historial y Registro de Cambios", "Cómo consultar las modificaciones anteriores y recuperar datos si hubo un error."),
                    ("Capítulo 6", "Reporte Predictivo de Crecimiento", "Estimación de peso y talla a futuro mediante un semáforo de salud muy claro."),
                    ("Capítulo 7", "Copias de Seguridad y Cuidado de Datos", "Cómo hacer copias de seguridad de forma fácil para no perder nunca información."),
                    ("Anexo", "Guardar y Recuperar Pacientes en Archivo", "Exportar e importar fichas de pacientes y protección de datos personales.")
                };

                for (int i = 0; i < capitulos.Length; i++)
                {
                    VerificarEspacio(38);
                    double boxY = _y;
                    _gfx.DrawRoundedRectangle(new XPen(_colBorder, 0.8),
                        new XSolidBrush(i % 2 == 0 ? _colLightBg : XColor.FromArgb(255, 255, 255)),
                        MarginLeft, boxY, UsableWidth, 32, 4, 4);

                    _gfx.DrawString(capitulos[i].Item1, _fBodyBold, new XSolidBrush(_colDark), new XPoint(MarginLeft + 12, boxY + 14));
                    _gfx.DrawString(capitulos[i].Item2, _fSecTitle, new XSolidBrush(_colGray), new XPoint(MarginLeft + 95, boxY + 14));
                    _gfx.DrawString(capitulos[i].Item3, _fBody, new XSolidBrush(_colTextDark), new XPoint(MarginLeft + 95, boxY + 26));

                    _y += 36;
                }

                _y += 15;
                DibujarCajaDestacada("ACCESO FÁCIL EN TODO MOMENTO",
                    "Recuerde que no necesita memorizar este manual: en cualquier ventana del sistema presione la tecla F1 o haga clic en el botón '?' para consultar la ayuda específica de esa pantalla.");
            }

            #endregion

            #region Capítulos

            private void GenerarCapitulo1()
            {
                DibujarEncabezadoCapitulo("Capítulo 1: Bienvenida y Accesos Rápidos", "CAPÍTULO 1");

                DibujarSubtitulo("1. Bienvenida al Sistema NutriEvolve");
                DibujarParrafo("NutriEvolve es un sistema diseñado especialmente para el consultorio de nutrición infantil. Su propósito es brindar una herramienta ágil y sencilla para registrar las fichas de los niños, llevar el control periódico de sus consultas y observar de manera clara cómo van creciendo a lo largo de los meses.");

                DibujarSubtitulo("2. Accesos Rápidos y Ayuda en Pantalla");
                DibujarParrafo("El sistema está preparado para que cualquier persona del equipo de salud pueda utilizarlo fácilmente desde el primer día:");

                DibujarItemLista("Tecla Rápida F1", "En cualquier momento y en cualquier pantalla del sistema puede presionar la tecla F1 de su teclado para abrir este manual de ayuda interactivo.");
                DibujarItemLista("Botón '? Ayuda'", "En la barra superior de cada ventana encontrará un botón con el signo de interrogación ('?'). Al hacer clic sobre él, el manual se abrirá automáticamente en el capítulo correspondiente a esa ventana.");
                DibujarItemLista("Menú Principal", "En la parte superior de la ventana principal encontrará los accesos directos para ir a Pacientes, Consultas, Reportes, Historial y Copias de Seguridad con un solo clic.");
            }

            private void GenerarCapitulo2()
            {
                VerificarEspacio(70);
                DibujarEncabezadoCapitulo("Capítulo 2: Ficha y Gestión de Pacientes", "CAPÍTULO 2");

                DibujarSubtitulo("1. ¿Para qué sirve esta pantalla?");
                DibujarParrafo("La pantalla de Pacientes le permite ver la lista completa de niños registrados en el consultorio, buscar rápidamente una ficha existente y cargar o actualizar datos personales y de contacto.");

                DibujarSubtitulo("2. Guía Botón por Botón de la Pantalla");
                DibujarItemLista("Botón 'Añadir'", "Limpia y habilita los campos de texto para ingresar los datos de un nuevo paciente (DNI, Apellidos, Nombres, Dirección, etc.).");
                DibujarItemLista("Botón 'Modificar'", "Habilita la edición de los datos del paciente que haya seleccionado en la lista para actualizar su información.");
                DibujarItemLista("Botón 'Eliminar'", "Da de baja al paciente seleccionado. El sistema no borra sus consultas previas, garantizando que su historia médica quede siempre resguardada.");
                DibujarItemLista("Botón 'Aplicar'", "Confirma y guarda los datos que acaba de cargar o modificar en la lista de pacientes.");
                DibujarItemLista("Botón 'Cancelar'", "Cancela la operación actual y descarta cualquier cambio que no haya guardado.");
                DibujarItemLista("Botón 'Limpiar'", "Vacía los campos de texto y filtros para realizar una nueva búsqueda o volver a ver la lista completa.");
                DibujarItemLista("Botón 'Salir'", "Cierra la ventana y regresa a la pantalla principal del consultorio.");
                DibujarItemLista("Botones 'Emails' y 'Teléfonos'", "Abren la ventana de contactos para agregar más de un número o correo familiar.");
                DibujarItemLista("Botón 'Exportar (Guardar en Archivo)'", "Guarda una copia de la lista de pacientes en un archivo en su computadora para respaldo.");
                DibujarItemLista("Botón 'Importar (Cargar desde Archivo)'", "Permite cargar pacientes desde un archivo hacia el sistema.");
            }

            private void GenerarCapitulo3()
            {
                VerificarEspacio(70);
                DibujarEncabezadoCapitulo("Capítulo 3: Teléfonos y Correos Electrónicos de Contacto", "CAPÍTULO 3");

                DibujarSubtitulo("1. Gestión de Contactos (Emails y Teléfonos)");
                DibujarParrafo("En la atención pediátrica es muy común necesitar más de un número o correo de contacto (por ejemplo, el celular de la madre, el del padre, el teléfono fijo del hogar o el contacto de un tutor). El sistema permite registrar todos los contactos necesarios para asegurar una comunicación rápida ante cualquier eventualidad o recordatorio de turno.");

                DibujarSubtitulo("2. Ventana de Contactos (frmContactos1N) y sus Botones");
                DibujarParrafo("Al hacer clic en 'Emails' o 'Teléfonos' en la ficha del paciente, se abrirá una ventana para administrar sus canales de contacto:");

                DibujarItemLista("Botón 'Añadir Contacto'", "Abre un espacio para escribir un nuevo número telefónico o dirección de correo.");
                DibujarItemLista("Botón 'Modificar Contacto'", "Permite corregir o actualizar un contacto ya existente si la familia cambió de número o correo.");
                DibujarItemLista("Botón 'Eliminar Contacto'", "Quita de la lista un contacto que ya no se utilice.");
                DibujarItemLista("Casilla 'Principal'", "Permite elegir cuál es el contacto prioritario al que el consultorio enviará los avisos y turnos.");
                DibujarItemLista("Botón 'Aceptar / Guardar'", "Guarda todos los contactos ingresados y vuelve a la ficha del paciente.");
                DibujarItemLista("Botón 'Cancelar'", "Cierra la ventana sin realizar cambios en los contactos.");
            }

            private void GenerarCapitulo4()
            {
                VerificarEspacio(70);
                DibujarEncabezadoCapitulo("Capítulo 4: Consultas Nutricionales y Curvas OMS", "CAPÍTULO 4");

                DibujarSubtitulo("1. Registro de la Consulta Médica");
                DibujarParrafo("Esta pantalla permite registrar la atención médica periódica del paciente: antecedentes de salud, tipo de alimentación (lactancia o comidas), hábitos, alergias y las recomendaciones brindadas a la familia.");

                DibujarSubtitulo("2. Medición y Curvas de Crecimiento de la OMS");
                DibujarParrafo("Al ingresar el peso y la estatura del niño, el sistema ubica automáticamente sus valores en los gráficos oficiales de la Organización Mundial de la Salud (OMS). Esto permite ver de inmediato si el niño se encuentra en su peso y talla normales para su edad, o si presenta bajo peso o sobrepeso, facilitando la explicación visual a los padres.");

                DibujarSubtitulo("3. Guía de Botones de la Consulta");
                DibujarItemLista("Botón 'Nueva Consulta'", "Limpia la pantalla para iniciar la carga de un nuevo control médico.");
                DibujarItemLista("Botón 'Guardar Consulta'", "Registra la visita, las medidas físicas y el diagnóstico en la ficha del paciente.");
                DibujarItemLista("Botón 'Ver Gráfico / Curvas OMS'", "Abre el gráfico visual interactivo con las curvas de evolución del paciente.");
                DibujarItemLista("Botón 'Imprimir Comprobante'", "Emite la ficha oficial de la consulta con el plan alimentario y recomendaciones para entregar a los padres.");
                DibujarItemLista("Botón 'Salir'", "Cierra la ventana de consultas.");
            }

            private void GenerarCapitulo5()
            {
                VerificarEspacio(70);
                DibujarEncabezadoCapitulo("Capítulo 5: Historial y Registro de Cambios", "CAPÍTULO 5");

                DibujarSubtitulo("1. ¿Para qué sirve el Historial de Cambios?");
                DibujarParrafo("El sistema guarda un registro automático de cada modificación realizada en las fichas de los pacientes (qué dato cambió, qué usuario lo hizo y en qué fecha y hora). Esto brinda tranquilidad al consultorio y permite saber siempre cómo fue evolucionando la información.");

                DibujarSubtitulo("2. Recuperar una Versión Anterior");
                DibujarParrafo("Si un usuario comete un error al modificar una ficha médica, el sistema conserva todas las versiones anteriores y permite restaurar los datos correctos con un solo clic.");

                DibujarSubtitulo("3. Guía de Botones del Historial");
                DibujarItemLista("Botón 'Buscar / Filtrar'", "Permite buscar cambios por paciente o rango de fechas.");
                DibujarItemLista("Botón 'Limpiar'", "Restablece la búsqueda para ver todas las modificaciones recientes.");
                DibujarItemLista("Botón 'Activar Versión Anterior'", "Restaura los datos del paciente al estado que tenían en la fecha seleccionada.");
                DibujarItemLista("Botón 'Salir'", "Cierra la ventana de historial.");
            }

            private void GenerarCapitulo6()
            {
                VerificarEspacio(70);
                DibujarEncabezadoCapitulo("Capítulo 6: Reporte Predictivo de Crecimiento", "CAPÍTULO 6");

                DibujarSubtitulo("1. ¿Qué es el Reporte Predictivo?");
                DibujarParrafo("A diferencia de un listado común, esta herramienta analiza las visitas anteriores del niño y estima cómo continuará su peso y estatura en los próximos tres meses si mantiene sus hábitos actuales.");

                DibujarSubtitulo("2. Semáforo de Salud y Recomendaciones");
                DibujarItemLista("Semáforo Verde", "El crecimiento evoluciona de manera normal y favorable.");
                DibujarItemLista("Semáforo Amarillo", "Crecimiento más lento o aumento leve; se aconseja un control preventivo.");
                DibujarItemLista("Semáforo Rojo", "Alerta clínica; se recomienda programar un control médico más cercano.");

                DibujarSubtitulo("3. Guía de Botones del Reporte");
                DibujarItemLista("Botón 'Generar Reporte'", "Realiza el cálculo para el paciente seleccionado.");
                DibujarItemLista("Botón 'Imprimir'", "Imprime el reporte para adjuntarlo a la historia clínica.");
                DibujarItemLista("Botón 'Salir'", "Cierra la pantalla de reportes.");
            }

            private void GenerarCapitulo7()
            {
                VerificarEspacio(70);
                DibujarEncabezadoCapitulo("Capítulo 7: Copias de Seguridad y Cuidado de la Información", "CAPÍTULO 7");

                DibujarSubtitulo("1. Cuidado de los Datos del Consultorio");
                DibujarParrafo("La información médica de los pacientes es lo más valioso del consultorio. Realizar copias de seguridad de forma regular garantiza que ante cualquier desperfecto en la computadora, las fichas clínicas nunca se pierdan.");

                DibujarSubtitulo("2. Protección Automática de la Información");
                DibujarParrafo("El sistema cuenta con mecanismos de seguridad que verifican constantemente que ningún dato esté dañado ni haya sido alterado indebidamente. Si alguna vez necesita reinstalar el sistema, sus datos clínicos existentes no serán borrados ni sobreescritos.");

                DibujarSubtitulo("3. Guía de Botones de Respaldo");
                DibujarItemLista("Botón 'Crear Copia de Seguridad'", "Guarda una copia de todos los datos del sistema en una carpeta segura, disco externo o pendrive.");
                DibujarItemLista("Botón 'Restaurar Copia'", "Permite recuperar la información a partir de un archivo de respaldo previo.");
                DibujarItemLista("Botón 'Salir'", "Cierra la pantalla de respaldo.");
            }

            private void GenerarAnexo()
            {
                VerificarEspacio(70);
                DibujarEncabezadoCapitulo("Anexo: Guardar y Cargar Pacientes en Archivo", "ANEXO");

                DibujarSubtitulo("1. Guardar y Cargar Fichas (Exportar e Importar)");
                DibujarParrafo("Permite guardar fichas de pacientes en archivos para trasladar información a otra computadora del consultorio o para resguardo individual.");

                DibujarSubtitulo("2. Protección de la Privacidad del Paciente");
                DibujarParrafo("En cumplimiento de las normas de privacidad médica, los documentos de identidad de los niños se guardan de forma protegida para que personas no autorizadas no puedan acceder a información privada de los menores.");
            }

            #endregion

            #region Métodos de Dibujo Auxiliares

            private void DibujarEncabezadoCapitulo(string titulo, string etiqueta)
            {
                VerificarEspacio(60);
                double y = _y;

                // Franja decorativa sobria y limpia (escala de grises)
                _gfx.DrawRoundedRectangle(new XPen(_colBorder, 1), new XSolidBrush(_colLightBg), MarginLeft, y, UsableWidth, 34, 4, 4);

                _gfx.DrawString(etiqueta, _fChapterNum, new XSolidBrush(_colGray), new XPoint(MarginLeft + 14, y + 14));
                _gfx.DrawString(titulo, _fChapterTitle, new XSolidBrush(_colDark), new XPoint(MarginLeft + 14, y + 27));

                _y += 44;
            }

            private void DibujarSubtitulo(string texto)
            {
                VerificarEspacio(26);
                _gfx.DrawString(texto, _fSecTitle, new XSolidBrush(_colDark), new XPoint(MarginLeft, _y + 12));
                _gfx.DrawLine(new XPen(_colBorder, 0.8), MarginLeft, _y + 16, MarginLeft + UsableWidth, _y + 16);
                _y += 24;
            }

            private void DibujarParrafo(string texto)
            {
                var lineas = DividirEnLineas(texto, _fBody, UsableWidth);
                foreach (var linea in lineas)
                {
                    VerificarEspacio(14);
                    _gfx.DrawString(linea, _fBody, new XSolidBrush(_colTextDark), new XPoint(MarginLeft, _y + 10));
                    _y += 14;
                }
                _y += 4;
            }

            private void DibujarItemLista(string titulo, string descripcion)
            {
                double indentBullet = 16;
                string textoCompleto = $"•  {titulo}: {descripcion}";
                var lineas = DividirEnLineas(textoCompleto, _fBody, UsableWidth);

                for (int i = 0; i < lineas.Count; i++)
                {
                    VerificarEspacio(14);
                    if (i == 0)
                    {
                        _gfx.DrawString(lineas[i], _fBody, new XSolidBrush(_colTextDark), new XPoint(MarginLeft + 6, _y + 10));
                    }
                    else
                    {
                        _gfx.DrawString(lineas[i], _fBody, new XSolidBrush(_colTextDark), new XPoint(MarginLeft + indentBullet + 6, _y + 10));
                    }
                    _y += 14;
                }
                _y += 3;
            }

            private void DibujarCajaDestacada(string titulo, string texto)
            {
                var lineas = DividirEnLineas(texto, _fBoxBody, UsableWidth - 28);
                double boxHeight = 28 + (lineas.Count * 13) + 8;

                VerificarEspacio(boxHeight + 8);
                double y = _y;

                _gfx.DrawRoundedRectangle(new XPen(_colBorder, 1), new XSolidBrush(_colLightBg), MarginLeft, y, UsableWidth, boxHeight, 4, 4);

                _gfx.DrawString(titulo, _fBoxTitle, new XSolidBrush(_colDark), new XPoint(MarginLeft + 14, y + 16));

                double ly = y + 30;
                foreach (var linea in lineas)
                {
                    _gfx.DrawString(linea, _fBoxBody, new XSolidBrush(_colTextDark), new XPoint(MarginLeft + 14, ly));
                    ly += 13;
                }

                _y += boxHeight + 10;
            }

            private List<string> DividirEnLineas(string texto, XFont font, double anchoMax)
            {
                var resultado = new List<string>();
                if (string.IsNullOrWhiteSpace(texto)) return resultado;

                var palabras = texto.Split(' ');
                string lineaActual = "";

                foreach (var palabra in palabras)
                {
                    string prueba = string.IsNullOrEmpty(lineaActual) ? palabra : lineaActual + " " + palabra;
                    var medida = _gfx.MeasureString(prueba, font);

                    if (medida.Width <= anchoMax)
                    {
                        lineaActual = prueba;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(lineaActual))
                        {
                            resultado.Add(lineaActual);
                        }
                        lineaActual = palabra;
                    }
                }

                if (!string.IsNullOrEmpty(lineaActual))
                {
                    resultado.Add(lineaActual);
                }

                return resultado;
            }

            private void EstamparEncabezadosYPie()
            {
                int totalPaginas = _doc.Pages.Count;

                for (int i = 1; i < totalPaginas; i++)
                {
                    var page = _doc.Pages[i];
                    using var gfx = XGraphics.FromPdfPage(page);

                    // Encabezado superior limpio (sin textos técnicos)
                    gfx.DrawString("MANUAL DE USUARIO - NUTRIEVOLVE", _fHeader, new XSolidBrush(_colTextMuted), new XPoint(MarginLeft, 32));
                    gfx.DrawLine(new XPen(_colBorder, 0.7), MarginLeft, 38, page.Width - MarginRight, 38);

                    // Pie de página limpio con paginación
                    gfx.DrawLine(new XPen(_colBorder, 0.7), MarginLeft, page.Height - 34, page.Width - MarginRight, page.Height - 34);
                    gfx.DrawString("NutriEvolve - Ayuda al Usuario", _fFooter, new XSolidBrush(_colTextMuted), new XPoint(MarginLeft, page.Height - 22));

                    string paginaStr = $"Página {i + 1} de {totalPaginas}";
                    var medida = gfx.MeasureString(paginaStr, _fFooter);
                    gfx.DrawString(paginaStr, _fFooter, new XSolidBrush(_colDark), new XPoint(page.Width - MarginRight - medida.Width, page.Height - 22));
                }
            }

            #endregion
        }
    }
}
