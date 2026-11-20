using System;
using System.Collections.Generic;
using System.IO;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace UI.PN2
{
    /// <summary>
    /// Generador profesional del Manual de Ayuda y Directivas Operativas del Sistema NutriEvolve en formato PDF.
    /// Replica y expande el contenido del sistema de ayuda contextual con formato institucional.
    /// </summary>
    public static class ManualAyudaPdfGenerator
    {
        public static void GenerarPdf(string rutaArchivo)
        {
            var doc = new PdfDocument();
            doc.Info.Title = "NutriEvolve - Manual de Usuario y Directivas Operativas";
            doc.Info.Author = "Sistema NutriEvolve - Gabriel Brítez";
            doc.Info.Subject = "Documentación de Operación, Seguridad, Módulos PN1 y PN2";
            doc.Info.Keywords = "NutriEvolve, Ayuda, Manual, OMS, Bitácora, Seguridad, Rollback, DV";

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

            // Paleta de colores institucional NutriEvolve
            private readonly XColor _colDarkGreen = XColor.FromArgb(30, 60, 40);
            private readonly XColor _colBrandGreen = XColor.FromArgb(76, 124, 89);
            private readonly XColor _colBrandLight = XColor.FromArgb(238, 245, 239);
            private readonly XColor _colBorderGreen = XColor.FromArgb(180, 205, 185);
            private readonly XColor _colTextDark = XColor.FromArgb(35, 45, 40);
            private readonly XColor _colTextMuted = XColor.FromArgb(95, 105, 100);
            private readonly XColor _colWarnBg = XColor.FromArgb(254, 243, 199);
            private readonly XColor _colWarnBorder = XColor.FromArgb(217, 119, 6);
            private readonly XColor _colWarnText = XColor.FromArgb(146, 64, 14);

            // Tipografías
            private readonly XFont _fCoverTitle = new("Segoe UI", 24, XFontStyle.Bold);
            private readonly XFont _fCoverSub = new("Segoe UI", 13, XFontStyle.Bold);
            private readonly XFont _fCoverTag = new("Segoe UI", 10, XFontStyle.Regular);
            private readonly XFont _fChapterNum = new("Segoe UI", 9.5, XFontStyle.Bold);
            private readonly XFont _fChapterTitle = new("Segoe UI", 13, XFontStyle.Bold);
            private readonly XFont _fSecTitle = new("Segoe UI", 10.5, XFontStyle.Bold);
            private readonly XFont _fBody = new("Segoe UI", 9, XFontStyle.Regular);
            private readonly XFont _fBodyBold = new("Segoe UI", 9, XFontStyle.Bold);
            private readonly XFont _fBodyItalic = new("Segoe UI", 8.5, XFontStyle.Italic);
            private readonly XFont _fHeader = new("Segoe UI", 8, XFontStyle.Regular);
            private readonly XFont _fFooter = new("Segoe UI", 8, XFontStyle.Regular);
            private readonly XFont _fBoxTitle = new("Segoe UI", 9, XFontStyle.Bold);
            private readonly XFont _fBoxBody = new("Segoe UI", 8.5, XFontStyle.Regular);

            public DocumentBuilder(PdfDocument doc)
            {
                _doc = doc;
            }

            public void ConstruirManualCompleto()
            {
                // 1. Portada Institucional
                GenerarPortada();

                // 2. Índice General y Resumen
                NuevaPagina(true);
                GenerarIndiceGeneral();

                // 3. Capítulo 1: Arquitectura y Navegación
                NuevaPagina();
                GenerarCapitulo1();

                // 4. Capítulo 2: Maestro de Clientes (ABM-C Clásico y Botón por Botón)
                GenerarCapitulo2();

                // 5. Capítulo 3: Tablas 1:N y Baja Lógica
                GenerarCapitulo3();

                // 6. Capítulo 4: Consultas Nutricionales, Curvas OMS y Comprobante Formal
                GenerarCapitulo4();

                // 7. Capítulo 5: Bitácora de Cambios, Triggers SQL y Rollback
                GenerarCapitulo5();

                // 8. Capítulo 6: Reporte Predictivo Inteligente y Soporte de Decisiones
                GenerarCapitulo6();

                // 9. Capítulo 7: Reinstalador Seguro, Respaldo y Dígitos Verificadores
                GenerarCapitulo7();

                // 10. Anexo Técnico: Criptografía AES-256 y Serialización XML
                GenerarAnexoTecnico();

                // Liberar el objeto XGraphics de la última página antes del segundo pass
                _gfx?.Dispose();
                _gfx = null!;

                // 11. Estampar encabezados y números de página
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
                _page = _doc.AddPage();
                _page.Size = PdfSharpCore.PageSize.A4;
                _gfx = XGraphics.FromPdfPage(_page);

                // Barra superior de color corporativo
                _gfx.DrawRectangle(new XSolidBrush(_colDarkGreen), 0, 0, _page.Width, 18);
                _gfx.DrawRectangle(new XSolidBrush(_colBrandGreen), 0, 18, _page.Width, 6);

                // Marco exterior fino
                _gfx.DrawRectangle(new XPen(_colBorderGreen, 1), MarginLeft - 10, MarginTop - 10, UsableWidth + 20, _page.Height - MarginTop - MarginBottom + 20);

                double y = 80;

                // Emblema / Titular
                _gfx.DrawString("NUTRIEVOLVE", _fCoverTitle, new XSolidBrush(_colDarkGreen), new XPoint(MarginLeft, y));
                y += 28;
                _gfx.DrawString("SISTEMA DE GESTIÓN CLÍNICA PEDIÁTRICA Y SEGUIMIENTO NUTRICIONAL OMS", _fCoverTag, new XSolidBrush(_colBrandGreen), new XPoint(MarginLeft, y));
                y += 18;
                _gfx.DrawLine(new XPen(_colBrandGreen, 2), MarginLeft, y, MarginLeft + UsableWidth, y);
                y += 30;

                // Título del manual
                _gfx.DrawString("MANUAL INTEGRAL DE AYUDA Y DIRECTIVAS OPERATIVAS", _fCoverSub, new XSolidBrush(_colDarkGreen), new XPoint(MarginLeft, y));
                y += 16;
                _gfx.DrawString("Guía oficial para la navegación, operación modular, auditoría de datos y directivas de seguridad.", _fBody, new XSolidBrush(_colTextDark), new XPoint(MarginLeft, y));
                y += 35;

                // Tarjeta de Metadatos del Sistema
                double boxHeight = 160;
                _gfx.DrawRoundedRectangle(new XPen(_colBorderGreen, 1.2), new XSolidBrush(_colBrandLight), MarginLeft, y, UsableWidth, boxHeight, 6, 6);

                double by = y + 20;
                double bx = MarginLeft + 18;
                _gfx.DrawString("ESPECIFICACIONES DEL ENTORNO Y ALCANCE:", _fSecTitle, new XSolidBrush(_colDarkGreen), new XPoint(bx, by));
                by += 20;

                string[] metadata = {
                    "• Cátedra / Proyecto: Ingeniería de Software — Sistema de Seguimiento Nutricional Pediátrico",
                    "• Arquitectura: Multicapa Desacoplada (UI, BLL, DAL, BE, Services) sobre .NET 9.0 WinForms",
                    "• Perfiles contemplados: Administrador General del Sistema y Nutricionista Clínico",
                    "• Módulo PN1: Maestro Clientes (ABM-C), Contactos 1:N (Email/Tel), Agenda y Turnos Médicos",
                    "• Módulo PN2: Consultas Clínicas, Antropometría OMS (Z-Scores LMS), Prescripción, Reporte Predictivo",
                    "• Seguridad e Integridad: Dígitos Verificadores (DVH/DVV), Criptografía AES-256 en DNI, Bitácoras y Backup"
                };

                foreach (var line in metadata)
                {
                    _gfx.DrawString(line, _fBody, new XSolidBrush(_colTextDark), new XPoint(bx, by));
                    by += 18;
                }

                y += boxHeight + 35;

                // Callout de Seguridad Idéntico al Sistema
                double calloutH = 95;
                _gfx.DrawRoundedRectangle(new XPen(_colWarnBorder, 1.5), new XSolidBrush(_colWarnBg), MarginLeft, y, UsableWidth, calloutH, 6, 6);

                double cy = y + 20;
                double cx = MarginLeft + 18;
                _gfx.DrawString("⚠ DIRECTIVA CRÍTICA DE SEGURIDAD Y PREVENCIÓN:", _fBoxTitle, new XSolidBrush(_colWarnText), new XPoint(cx, cy));
                cy += 18;
                _gfx.DrawString("Realice un backup completo de la base de datos antes de reinstalar o modificar estructuras SQL.", _fBoxBody, new XSolidBrush(_colWarnText), new XPoint(cx, cy));
                cy += 15;
                _gfx.DrawString("El instalador de NutriEvolve detecta si el catálogo SQL ya existe en el servidor y omitirá sentencias destructivas", _fBoxBody, new XSolidBrush(_colWarnText), new XPoint(cx, cy));
                cy += 15;
                _gfx.DrawString("(DELETE, DROP, TRUNCATE) garantizando que la base de datos productiva no sea sobreescrita ni dañada.", _fBoxBody, new XSolidBrush(_colWarnText), new XPoint(cx, cy));

                y += calloutH + 50;

                // Pie de portada
                _gfx.DrawLine(new XPen(_colBorderGreen, 1), MarginLeft, y, MarginLeft + UsableWidth, y);
                y += 18;
                _gfx.DrawString("Documento generado automáticamente por el subsistema de ayuda de NutriEvolve.", _fBodyItalic, new XSolidBrush(_colTextMuted), new XPoint(MarginLeft, y));
                _gfx.DrawString($"Fecha de Emisión: {DateTime.Now:dd/MM/yyyy HH:mm} hs", _fBodyBold, new XSolidBrush(_colDarkGreen), new XPoint(MarginLeft + UsableWidth - 180, y));

                // Barra inferior
                _gfx.DrawRectangle(new XSolidBrush(_colDarkGreen), 0, _page.Height - 14, _page.Width, 14);
            }

            #endregion

            #region Índice General

            private void GenerarIndiceGeneral()
            {
                DibujarEncabezadoCapitulo("ÍNDICE GENERAL Y GUÍA DE REFERENCIA RÁPIDA", "ESTRUCTURA TEMÁTICA");

                DibujarParrafo("El presente manual recopila las directivas operativas, normativas técnicas y flujos funcionales del sistema NutriEvolve, correspondiendo a los 7 capítulos disponibles en la interfaz interactiva accesible mediante la tecla rápida F1 o el botón de ayuda contextual '?' de cada formulario:");

                _y += 10;

                var capitulos = new[]
                {
                    ("Capítulo 1", "Arquitectura y Navegación General", "Organización en capas (UI, BLL, DAL, BE, Services), accesos rápidos y control de perfiles."),
                    ("Capítulo 2", "Maestro de Clientes / Pacientes (ABM-C Clásico)", "Máquina de estados finitos, ciclo operativo y análisis botón por botón del formulario principal."),
                    ("Capítulo 3", "Tablas 1:N y Repercusión en Modificación y Agregado", "Gestión de correos y teléfonos (frmContactos1N), transacciones atómicas y baja lógica en cascada."),
                    ("Capítulo 4", "Consultas Nutricionales, Curvas OMS y Comprobante Formal", "Evaluación antropométrica, Z-Scores LMS de la OMS, percentiles y ficha oficial imprimible."),
                    ("Capítulo 5", "Bitácora de Cambios vs Eventos, Triggers SQL y Rollback", "Auditoría de mutación de datos con triggers, trazabilidad histórica y mecanismo de reversión."),
                    ("Capítulo 6", "Reporte Inteligente Predictivo y Soporte de Decisiones", "Cruce longitudinal, velocidad ponderal/crecimiento, semáforo de riesgo y proyección a 90 días."),
                    ("Capítulo 7", "Reinstalador Seguro, Respaldo y Protección de Datos", "Prevención de pérdida de datos, garantía de no sobreescritura y dígitos verificadores (DVH/DVV)."),
                    ("Anexo Técnico", "Criptografía Reversible AES-256 y Serialización XML", "Cifrado en reposo para DNI de pacientes y motor de exportación/importación XML tipado.")
                };

                for (int i = 0; i < capitulos.Length; i++)
                {
                    VerificarEspacio(42);
                    double boxY = _y;
                    _gfx.DrawRoundedRectangle(new XPen(_colBorderGreen, 0.8),
                        new XSolidBrush(i % 2 == 0 ? _colBrandLight : XColor.FromArgb(248, 251, 248)),
                        MarginLeft, boxY, UsableWidth, 36, 4, 4);

                    _gfx.DrawString(capitulos[i].Item1, _fBodyBold, new XSolidBrush(_colDarkGreen), new XPoint(MarginLeft + 12, boxY + 16));
                    _gfx.DrawString(capitulos[i].Item2, _fSecTitle, new XSolidBrush(_colBrandGreen), new XPoint(MarginLeft + 105, boxY + 16));
                    _gfx.DrawString(capitulos[i].Item3, _fBody, new XSolidBrush(_colTextDark), new XPoint(MarginLeft + 105, boxY + 30));

                    _y += 42;
                }

                _y += 15;
                DibujarCajaDestacada("ACCESO CONTEXTUAL EN CUALQUIER MOMENTO",
                    "Recuerde que en cualquier formulario del sistema puede presionar la tecla F1 o hacer clic en el botón '? Ayuda' ubicado en la esquina superior derecha para abrir directamente el capítulo correspondiente al módulo en uso.");
            }

            #endregion

            #region Capítulos

            private void GenerarCapitulo1()
            {
                DibujarEncabezadoCapitulo("Capítulo 1: Arquitectura y Navegación General", "CAPÍTULO 1");

                DibujarSubtitulo("1. Bienvenida al Sistema NutriEvolve");
                DibujarParrafo("NutriEvolve es una plataforma integral de gestión clínica pediátrica, diseñada bajo estrictos estándares de ingeniería de software para el seguimiento antropométrico y nutricional infantil basado en los patrones de crecimiento de la Organización Mundial de la Salud (OMS).");

                DibujarSubtitulo("2. Arquitectura Multicapa Desacoplada");
                DibujarParrafo("El software está construido siguiendo el paradigma de arquitectura en capas con bajo acoplamiento y alta cohesión:");

                DibujarItemLista("Capa de Presentación (UI)", "Implementada en Windows Forms bajo .NET 9.0. Ofrece controles visuales intuitivos, soporte multidioma reactivo (patrón Observer) y una máquina de estados estricta que previene errores del operador.");
                DibujarItemLista("Capa de Lógica de Negocio (BLL)", "Contiene las validaciones de dominio, los algoritmos de cálculo de percentiles y Z-Scores OMS (fórmula LMS), el motor predictivo y la gestión de auditoría.");
                DibujarItemLista("Capa de Acceso a Datos (DAL)", "Totalmente desacoplada mediante ADO.NET tipado con comandos SQL parametrizados y transacciones atómicas, protegiendo contra inyecciones SQL y garantizando integridad referencial.");
                DibujarItemLista("Capa de Entidades (BE)", "Clases del modelo de dominio libres de lógica de persistencia, facilitando el transporte seguro de información entre capas.");
                DibujarItemLista("Servicios Transversales (Services)", "Gestión de sesión singleton (ServicesSessionManager), autenticación segura con hashing BCrypt, encriptación reversible AES-256 para datos sensibles y control de integridad con Dígitos Verificadores.");

                DibujarSubtitulo("3. Accesos Rápidos y Ayuda Contextual");
                DibujarParrafo("• Presione la tecla F1 en cualquier ventana activa para desplegar este manual de ayuda interactivo.");
                DibujarParrafo("• Cada pantalla dispone de un botón '? Ayuda' que abre de forma contextual el capítulo y las directivas pertinentes al formulario actual.");
            }

            private void GenerarCapitulo2()
            {
                VerificarEspacio(70);
                DibujarEncabezadoCapitulo("Capítulo 2: Maestro de Clientes / Pacientes (ABM-C Clásico)", "CAPÍTULO 2");

                DibujarSubtitulo("1. Directivas Operativas y Máquina de Estados Finitos");
                DibujarParrafo("El Maestro de Clientes implementa el patrón clásico ABM-C (Alta, Baja, Modificación y Consulta) mediante una máquina de estados finitos. Cada botón controla estrictamente las transiciones del formulario para asegurar la consistencia de los datos:");

                DibujarItemLista("Estado 1 - Inicial (Navegación)", "La grilla superior se encuentra habilitada para explorar y seleccionar pacientes. Los campos inferiores de carga están bloqueados en solo lectura. Botones habilitados: 'Añadir', 'Modificar' y 'Eliminar' (habilitados al seleccionar una fila), y 'Salir'. Botones deshabilitados: 'Aplicar' y 'Cancelar'.");

                DibujarItemLista("Estado 2 - Botón 'Añadir' (Alta)", "Limpia automáticamente los campos inferiores y los desbloquea para la carga de datos. Deshabilita la grilla y los botones de navegación (Añadir, Modificar, Eliminar, Salir). Habilita 'Aplicar' y 'Cancelar'. El cursor se ubica en el campo DNI para iniciar el registro.");

                DibujarItemLista("Estado 3 - Botón 'Modificar' (Edición)", "Toma los datos del paciente seleccionado en la grilla y los carga en las cajas de texto inferiores. Desbloquea los campos editables (Apellidos, Nombres, Email, Celular, Dirección, Obra Social). El campo DNI permanece bloqueado por ser la clave primaria inmutable. Habilita 'Aplicar' y 'Cancelar', deshabilitando la grilla.");

                DibujarItemLista("Estado 4 - Botón 'Eliminar' (Baja Lógica)", "Solicita confirmación explícita al operador. Ejecuta una BAJA LÓGICA (estableciendo Activo = 0). No existe eliminación física; los registros históricos se conservan para auditoría y trazabilidad clínica.");

                DibujarItemLista("Estado 5 - Botón 'Aplicar' (Confirmar)", "Valida la obligatoriedad y el formato de los campos esenciales (DNI válido, Apellido, Nombre). Persiste la transacción (INSERT en alta, UPDATE en modificación) en la base de datos SQL. Sincroniza atómicamente los contactos principales en las tablas 1:N. Recarga la grilla y regresa al Estado Inicial.");

                DibujarItemLista("Estado 6 - Botón 'Cancelar' (Descartar)", "Descarta cualquier cambio no confirmado, limpia o restaura los controles y regresa de inmediato al Estado Inicial.");

                DibujarItemLista("Estado 7 - Botón 'Salir'", "Cierra la ventana actual y libera recursos retornando al Menú Principal.");

                DibujarSubtitulo("2. Botones de Utilidad Especiales");
                DibujarItemLista("Botones 'Emails 1:N' y 'Tels 1:N'", "Abren la ventana auxiliar de administración avanzada de múltiples contactos.");
                DibujarItemLista("Botón 'Bitácora Cambios'", "Abre la pantalla de auditoría histórica para ver el historial de mutaciones y aplicar Rollback.");
                DibujarItemLista("Botones 'XML Serializar' y 'XML Deserializar'", "Permiten exportar e importar pacientes en formato XML estándar.");
            }

            private void GenerarCapitulo3()
            {
                VerificarEspacio(70);
                DibujarEncabezadoCapitulo("Capítulo 3: Tablas 1:N y Repercusiones en Modificación y Agregado", "CAPÍTULO 3");

                DibujarSubtitulo("1. Gestión de Contactos 1:N (Emails y Teléfonos)");
                DibujarParrafo("Un paciente puede requerir múltiples canales de comunicación (correos de ambos progenitores, números fijos, celulares de urgencia). El sistema gestiona esta relación mediante tablas normalizadas 1:N (Paciente_Emails_DNI101 y Paciente_Telefonos_DNI101).");

                DibujarSubtitulo("2. Ventana Modal Desplegable (frmContactos1N)");
                DibujarParrafo("Al pulsar los botones 'Emails 1:N' o 'Tels 1:N' en el maestro, se abre una ventana modal que permite agregar, editar o dar de baja contactos individuales, designando obligatoriamente uno como 'Principal' mediante una bandera lógica.");

                DibujarSubtitulo("3. Repercusiones Transaccionales en Agregado y Modificación");
                DibujarItemLista("Alta Atómica", "Al dar de alta un paciente en el maestro, los valores ingresados en los campos de Email y Celular se insertan de forma automática como registros principales en las tablas 1:N bajo la misma transacción SQL.");
                DibujarItemLista("Modificación Sincronizada", "Al modificar el email o celular principal en la pantalla maestra, el sistema actualiza automáticamente el registro marcado como principal en la tabla hija correspondiente.");
                DibujarItemLista("Baja Lógica en Cascada", "La eliminación de contactos individuales es lógica (Activo_DNI101 = 0). Si se da de baja lógica al paciente completo, el sistema ejecuta una actualización en cascada que inactiva todos sus emails y teléfonos vinculados, garantizando coherencia e integridad referencial.");
            }

            private void GenerarCapitulo4()
            {
                VerificarEspacio(70);
                DibujarEncabezadoCapitulo("Capítulo 4: Consultas Nutricionales, Curvas OMS y Comprobante Formal", "CAPÍTULO 4");

                DibujarSubtitulo("1. Registro de Consultas Clínicas Pediátricas");
                DibujarParrafo("El módulo PN2 captura la anamnesis completa del niño: antecedentes clínicos, lactancia materna, incorporación de alimentos complementarios, hábitos de hidratación, alergias e intolerancias diagnosticadas.");

                DibujarSubtitulo("2. Evaluación Antropométrica y Z-Scores OMS");
                DibujarParrafo("El profesional ingresa las variables físicas: Peso (kg), Talla/Longitud (cm), Perímetro Cefálico (cm) e IMC calculado. El sistema aplica de manera matemática la fórmula LMS de la Organización Mundial de la Salud:");

                VerificarEspacio(40);
                double boxY = _y;
                _gfx.DrawRoundedRectangle(new XPen(_colBorderGreen, 1), new XSolidBrush(_colBrandLight), MarginLeft + 30, boxY, UsableWidth - 60, 32, 4, 4);
                _gfx.DrawString("Fórmula LMS OMS:   Z = [ (Medición / M)^L  -  1 ]  /  ( L * S )", _fBodyBold, new XSolidBrush(_colDarkGreen), new XPoint(MarginLeft + 55, boxY + 20));
                _y += 42;

                DibujarParrafo("El sistema proyecta en tiempo real los percentiles P3, P15, P50, P85 y P97 en curvas estandarizadas, disparando alertas clínicas automáticas cuando los desvíos superan las ±2 Desviaciones Estándar (indicadores de desnutrición aguda, emaciación, sobrepeso u obesidad).");

                DibujarSubtitulo("3. Ficha de Prescripción y Comprobante Formal");
                DibujarParrafo("Permite generar un comprobante oficial con membrete del consultorio médico que incluye:");
                DibujarParrafo("• Datos filiatorios del paciente pediátrico y su tutor responsable.");
                DibujarParrafo("• Registro antropométrico de la consulta y diagnóstico percentilar.");
                DibujarParrafo("• Prescripción calórica diaria calculada (Kcal/día) y balance de macronutrientes (Gramos y porcentajes de Carbohidratos, Proteínas y Grasas).");
                DibujarParrafo("• Metas de salud, pautas familiares personalizadas y espacio para firma profesional.");
                DibujarParrafo("• Funcionalidad completa de vista previa, envío directo a impresora física y exportación a PDF.");
            }

            private void GenerarCapitulo5()
            {
                VerificarEspacio(70);
                DibujarEncabezadoCapitulo("Capítulo 5: Bitácora de Cambios vs Eventos, Triggers SQL y Rollback", "CAPÍTULO 5");

                DibujarSubtitulo("1. Diferencia Conceptual Fundamental");
                DibujarItemLista("Bitácora de Eventos (Seguridad)", "Audita QUÉ SUCEDIÓ a nivel sistema: inicios de sesión, cierres de sesión, ejecuciones de backup, intentos no autorizados y excepciones con criticidad de 1 a 5.");
                DibujarItemLista("Bitácora de Cambios (Auditoría de Datos)", "Audita EL ESTADO DE LOS DATOS a nivel de atributo: captura la foto anterior y la foto posterior de cada registro modificado, el usuario responsable, la fecha exacta y la operación ejecutada.");

                DibujarSubtitulo("2. Disparadores SQL (Triggers AFTER INSERT / UPDATE)");
                DibujarParrafo("La auditoría de datos opera en el motor de base de datos mediante triggers automáticos:");
                DibujarParrafo("• Maestro de Clientes: 'trg_Pacientes_BitacoraCambios' audita ALTA, MODIFICACIÓN, BLOQUEAR y DESBLOQUEAR.");
                DibujarParrafo("• Consultas Nutricionales: 'trg_ConsultasNutricionales_BitacoraCambios' preserva mediciones y diagnósticos.");
                DibujarParrafo("• Planes Alimentarios: 'trg_PlanesAlimentarios_BitacoraCambios' guarda el histórico de requerimientos y macronutrientes.");
                DibujarParrafo("NOTA CLAVE: No existe borrado físico de registros en la arquitectura; la baja lógica se audita como BLOQUEAR.");

                DibujarSubtitulo("3. Mecanismo de Rollback (Reversión y Activación de Versiones)");
                DibujarParrafo("La pantalla de Bitácora de Cambios permite seleccionar cualquier versión histórica de un paciente o consulta.");
                DibujarParrafo("Al presionar el botón 'Activar / Revertir (Rollback)', el sistema restaura los valores históricos directamente en la tabla activa, reactiva el registro (Act = 1) y recalcula automáticamente los Dígitos Verificadores de la tabla.");
            }

            private void GenerarCapitulo6()
            {
                VerificarEspacio(70);
                DibujarEncabezadoCapitulo("Capítulo 6: Reporte Inteligente Predictivo y Soporte de Decisiones", "CAPÍTULO 6");

                DibujarSubtitulo("1. Análisis Longitudinal Multicriterio");
                DibujarParrafo("A diferencia de un listado estático, el Reporte Predictivo cruza longitudinalmente múltiples tablas clínicas (Pacientes, Consultas, Mediciones y Z-Scores) para evaluar la trayectoria de crecimiento a lo largo del tiempo.");

                DibujarSubtitulo("2. Métricas Clínicas Calculadas");
                DibujarItemLista("Velocidad Ponderal (Vp)", "Vp = Δ Peso / Δ Meses (kg/mes). Permite diagnosticar fallas de medro o ganancias de peso aceleradas.");
                DibujarItemLista("Velocidad de Crecimiento (Vt)", "Vt = Δ Talla / Δ Meses (cm/mes). Evalúa el ritmo de elongación ósea contra estándares de edad.");
                DibujarItemLista("Deriva de Z-Score IMC", "Mide el desplazamiento del percentil entre controles consecutivos para prevenir obesidad temprana.");

                DibujarSubtitulo("3. Proyección a 90 Días y Motor de Sugerencias");
                DibujarParrafo("El motor de inferencia clínica predice el peso y talla esperados a 90 días si se mantiene la tendencia actual, asignando una categoría de riesgo mediante un semáforo interactivo (Bajo, Moderado, Alto, Crítico) y sugiriendo adelantar o reprogramar el próximo control clínico.");
            }

            private void GenerarCapitulo7()
            {
                VerificarEspacio(70);
                DibujarEncabezadoCapitulo("Capítulo 7: Reinstalador Seguro, Respaldo y Protección de Datos", "CAPÍTULO 7");

                DibujarSubtitulo("1. Advertencia Previa y Respaldo Obligatorio");
                DibujarParrafo("Antes de cualquier proceso de mantenimiento técnico o reinstalación de componentes, el administrador debe ejecutar un Backup Completo de la base de datos desde la opción de menú 'Respaldo y Restauración'.");

                DibujarSubtitulo("2. Garantía de No Sobreescritura del Reinstalador");
                DibujarParrafo("El instalador inteligente de NutriEvolve verifica si la base de datos [ING] ya existe en el servidor SQL.");
                DibujarParrafo("Si la base existe, el asistente omite sentencias destructivas (DROP, TRUNCATE) e inserciones de prueba, garantizando que los datos clínicos productivos jamás sean pisados ni borrados.");

                DibujarSubtitulo("3. Control de Integridad con Dígitos Verificadores");
                DibujarParrafo("Cada fila de las tablas protegidas calcula un Dígito Verificador Horizontal (DVH) y cada tabla un Dígito Verificador Vertical (DVV) mediante hashes criptográficos. Cualquier alteración externa directa sobre la base de datos es detectada de forma inmediata al iniciar sesión.");
                DibujarParrafo("En caso de detectarse alteración, el sistema permite al administrador recalcular el DV. Al confirmar el recálculo, el sistema cierra automáticamente la sesión y redirige al LOGIN para exigir el reingreso de credenciales en un estado de base de datos íntegro.");
            }

            private void GenerarAnexoTecnico()
            {
                VerificarEspacio(70);
                DibujarEncabezadoCapitulo("Anexo Técnico: Criptografía Reversible AES-256 y Serialización XML", "ANEXO TÉCNICO");

                DibujarSubtitulo("1. Encriptación Reversible AES-256 en DNI de Pacientes");
                DibujarParrafo("En cumplimiento de las normativas de confidencialidad de historias clínicas pediátricas (Ley de Protección de Datos Personales), los documentos de identidad de los pacientes son encriptados mediante el algoritmo estándar AES-256 (Advanced Encryption Standard en modo CBC con relleno PKCS7):");
                DibujarItemLista("Cifrado en Reposo", "El DNI se persiste en SQL en formato Base64 (VARCHAR 100), impidiendo que consultas no autorizadas o volcados de datos expongan la identidad de los menores.");
                DibujarItemLista("Descifrado Transparente", "La capa de acceso a datos (DAL) y lógica (BLL) descifran el DNI en memoria exclusivamente para los perfiles autorizados.");
                DibujarItemLista("Idempotencia", "El algoritmo detecta cadenas ya cifradas, evitando re-encriptaciones accidentales que corrompan el dato.");

                DibujarSubtitulo("2. Serialización y Deserialización XML de Pacientes");
                DibujarParrafo("En el Maestro de Clientes se dispone de las funciones de resguardo y portabilidad mediante XML:");
                DibujarItemLista("Serialización XML", "Permite seleccionar uno o varios pacientes de la grilla y exportar sus fichas a un archivo estructurado XML con validación de esquema.");
                DibujarItemLista("Deserialización XML", "Lee archivos XML previamente exportados, valida la integridad de los nodos y permite visualizar o reincorporar los pacientes con verificación previa de duplicados.");
            }

            #endregion

            #region Métodos de Dibujo Auxiliares

            private void DibujarEncabezadoCapitulo(string titulo, string etiqueta)
            {
                VerificarEspacio(65);
                double y = _y;

                // Franja decorativa de capítulo
                _gfx.DrawRoundedRectangle(new XPen(_colDarkGreen, 1), new XSolidBrush(_colDarkGreen), MarginLeft, y, UsableWidth, 34, 4, 4);

                _gfx.DrawString(etiqueta, _fChapterNum, new XSolidBrush(_colBrandLight), new XPoint(MarginLeft + 14, y + 14));
                _gfx.DrawString(titulo, _fChapterTitle, XBrushes.White, new XPoint(MarginLeft + 14, y + 27));

                _y += 46;
            }

            private void DibujarSubtitulo(string texto)
            {
                VerificarEspacio(26);
                _gfx.DrawString(texto, _fSecTitle, new XSolidBrush(_colDarkGreen), new XPoint(MarginLeft, _y + 12));
                _gfx.DrawLine(new XPen(_colBorderGreen, 0.8), MarginLeft, _y + 16, MarginLeft + UsableWidth, _y + 16);
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
                double textWidth = UsableWidth - indentBullet;

                string textoCompleto = $"•  {titulo}: {descripcion}";
                var lineas = DividirEnLineas(textoCompleto, _fBody, UsableWidth);

                for (int i = 0; i < lineas.Count; i++)
                {
                    VerificarEspacio(14);
                    if (i == 0)
                    {
                        // Dibujar título en negrita si es posible
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

                _gfx.DrawRoundedRectangle(new XPen(_colBorderGreen, 1), new XSolidBrush(_colBrandLight), MarginLeft, y, UsableWidth, boxHeight, 4, 4);

                _gfx.DrawString("ℹ " + titulo, _fBoxTitle, new XSolidBrush(_colDarkGreen), new XPoint(MarginLeft + 14, y + 16));

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

                // Comenzamos desde la página 1 (índice 1 en 0-based), omitiendo la portada
                for (int i = 1; i < totalPaginas; i++)
                {
                    var page = _doc.Pages[i];
                    using var gfx = XGraphics.FromPdfPage(page);

                    // Encabezado superior
                    gfx.DrawString("NUTRIEVOLVE | SISTEMA DE GESTIÓN CLÍNICA Y AUDITORÍA INTEGRAL", _fHeader, new XSolidBrush(_colBrandGreen), new XPoint(MarginLeft, 32));
                    gfx.DrawString("MANUAL DE USUARIO Y DIRECTIVAS", _fHeader, new XSolidBrush(_colTextMuted), new XPoint(page.Width - MarginRight - 160, 32));
                    gfx.DrawLine(new XPen(_colBorderGreen, 0.7), MarginLeft, 38, page.Width - MarginRight, 38);

                    // Pie de página
                    gfx.DrawLine(new XPen(_colBorderGreen, 0.7), MarginLeft, page.Height - 34, page.Width - MarginRight, page.Height - 34);
                    gfx.DrawString("NutriEvolve v2.0 • Especialidad Nutrición Infantil OMS • Documento de Soporte", _fFooter, new XSolidBrush(_colTextMuted), new XPoint(MarginLeft, page.Height - 22));

                    string paginaStr = $"Página {i + 1} de {totalPaginas}";
                    var medida = gfx.MeasureString(paginaStr, _fFooter);
                    gfx.DrawString(paginaStr, _fFooter, new XSolidBrush(_colDarkGreen), new XPoint(page.Width - MarginRight - medida.Width, page.Height - 22));
                }
            }

            #endregion
        }
    }
}
