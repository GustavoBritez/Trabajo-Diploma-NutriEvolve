using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using BE;
using BE.PN2;
using UI.PN2;

namespace CapturasApp
{
    internal static class Program
    {
        private static readonly string OutputDir = @"c:\Users\Danie\Desktop\GIT\TD\Diagramas\Capturas";

        [STAThread]
        static void Main()
        {
            if (!Directory.Exists(OutputDir))
            {
                Directory.CreateDirectory(OutputDir);
            }

            Console.WriteLine($"Directorio de salida: {OutputDir}");

            // 1. Comprobante formal impreso (Hoja A4 médica)
            GenerarComprobanteImpreso();

            // 2. Ventana de Comprobante Formal con PrintPreviewControl
            GenerarVentanaComprobante();

            // 3. Bitácora de Cambios Multi-Entidad con Rollback
            GenerarCapturaBitacora();

            // 4. Reporte Inteligente Predictivo con Cruce de Tablas
            GenerarCapturaReporteInteligente();

            // 5. Maestro de Clientes ABM-C
            GenerarCapturaMaestroClientes();

            // 6. Contactos 1:N
            GenerarCapturaContactos1N();

            // 7. Manual de Ayuda con Índice Interactivo y Aviso de Seguridad
            GenerarCapturaManualAyuda();

            // 8. Asistente Reinstalador Seguro (Setup.exe)
            GenerarCapturaReinstaladorSeguro();

            Console.WriteLine("¡Todas las capturas generadas con éxito!");
        }

        private static void DrawText(Graphics g, string text, Font font, Color color, float x, float y)
        {
            using var b = new SolidBrush(color);
            g.DrawString(text, font, b, x, y);
        }

        private static void GenerarComprobanteImpreso()
        {
            var pac = new PacienteBE_DNI101
            {
                IdPaciente_DNI101 = 1,
                DNINiño_DNI101 = "48204110",
                Nombre_DNI101 = "Mateo",
                Apellido_DNI101 = "Benítez",
                FechaNacimiento_DNI101 = DateTime.Today.AddMonths(-24),
                Sexo_DNI101 = "Masculino",
                ObraSocial_DNI101 = "OSDE 210",
                Telefono_DNI101 = "11-4589-2231"
            };

            var con = new ConsultaNutricionalBE_DNI101
            {
                IdConsulta_DNI101 = 101,
                IdPaciente_DNI101 = 1,
                FechaControl_DNI101 = DateTime.Now,
                EdadMeses_DNI101 = 24,
                Medicion = new MedicionAntropometricaBE_DNI101(12.20m, 86.5m, 48.0m, 47.0m),
                Diagnostico = new DiagnosticoNutricionalBE_DNI101("Eutrófico (Percentil 50 OMS)", "Crecimiento pondoestatural armónico.", false)
            };

            var plan = new PlanAlimentarioBE_DNI101
            {
                IdPlan_DNI101 = 1,
                IdConsulta_DNI101 = 101,
                RequerimientoCalorico_DNI101 = 1350m,
                PctCarbohidratos_DNI101 = 55m,
                PctProteinas_DNI101 = 15m,
                PctGrasas_DNI101 = 30m,
                MetasSalud_DNI101 = "Mantener ganancia ponderal constante acorde a curva OMS de 24 meses. Estimular ingesta variada de vegetales.",
                PautasFamiliares_DNI101 = "4 comidas principales + 2 colaciones saludables (fruta fresca, yogur natural). Evitar bebidas azucaradas y ultraprocesados."
            };

            using var frm = new frmComprobantePrescripcion_DNI101(pac, con, plan, "Eutrófico (Percentil 50 OMS)");
            frm.CreateControl();

            using var bmp = new Bitmap(850, 1100);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var printMethod = typeof(frmComprobantePrescripcion_DNI101).GetMethod("PrintDoc_PrintPage", BindingFlags.NonPublic | BindingFlags.Instance);
            if (printMethod != null)
            {
                var args = new PrintPageEventArgs(g, new Rectangle(0, 0, 850, 1100), new Rectangle(0, 0, 850, 1100), new PageSettings());
                printMethod.Invoke(frm, new object[] { frm, args });
            }

            string path = Path.Combine(OutputDir, "05_Comprobante_Prescripcion_Formal.png");
            bmp.Save(path, ImageFormat.Png);
            Console.WriteLine($"Guardado: {path}");
        }

        private static void GenerarVentanaComprobante()
        {
            using var bmp = new Bitmap(1000, 720);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(240, 244, 241));
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Encabezado
            using var bTop = new SolidBrush(Color.FromArgb(76, 124, 89));
            g.FillRectangle(bTop, 0, 0, 1000, 65);

            using var fTit = new Font("Segoe UI", 12.5f, FontStyle.Bold);
            using var fSub = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            using var fBtn = new Font("Segoe UI", 9.5f, FontStyle.Bold);

            DrawText(g, "EMISIÓN DE COMPROBANTE FORMAL / FICHA DE PRESCRIPCIÓN PEDIÁTRICA", fTit, Color.White, 20, 12);
            DrawText(g, "Documento Clínico Oficial Pediátrico | Paciente: Mateo Benítez | DNI: 48.204.110 | Folio: NE-20261025-0101", fSub, Color.FromArgb(220, 240, 225), 22, 38);

            // Botones de acción superior
            DibujarBoton(g, 620, 14, 110, 36, Color.FromArgb(46, 125, 50), "Imprimir", fBtn);
            DibujarBoton(g, 740, 14, 130, 36, Color.FromArgb(59, 130, 246), "Exportar Archivo", fBtn);
            DibujarBoton(g, 880, 14, 95, 36, Color.FromArgb(46, 95, 70), "? Ayuda", fBtn);

            // Panel de vista previa central (Hoja simulada en miniatura)
            using var bShadow = new SolidBrush(Color.FromArgb(200, 210, 205));
            g.FillRectangle(bShadow, 225, 85, 555, 605);
            g.FillRectangle(Brushes.White, 220, 80, 550, 600);
            using var pBorder = new Pen(Color.FromArgb(180, 195, 185));
            g.DrawRectangle(pBorder, 220, 80, 550, 600);

            // Contenido simulado de la hoja
            using var fMiniTit = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            using var fMiniTxt = new Font("Segoe UI", 7.5f, FontStyle.Regular);
            using var fMiniBld = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            Color darkGreen = Color.FromArgb(30, 60, 40);

            // Banner institucional en miniatura
            g.FillRectangle(new SolidBrush(Color.FromArgb(240, 246, 240)), 230, 90, 530, 45);
            DrawText(g, "NUTRIEVOLVE - CONSULTORIO NUTRICIONAL PEDIÁTRICO", fMiniTit, darkGreen, 240, 95);
            DrawText(g, "Especialidad en Nutrición Infantil y Crecimiento Antropométrico OMS | Matrícula MP-7842", fMiniTxt, Color.FromArgb(90, 100, 95), 240, 115);

            // Secciones miniatura
            int y = 145;
            DrawText(g, "1. DATOS DEL PACIENTE: Mateo Benítez | DNI: 48.204.110 | Edad: 24 meses | Cobertura: OSDE 210", fMiniBld, darkGreen, 240, y);
            y += 20;
            g.DrawLine(pBorder, 240, y, 740, y);
            y += 10;
            DrawText(g, "2. EVALUACIÓN ANTROPOMÉTRICA: Peso: 12.20 kg | Talla: 86.5 cm | IMC: 16.3 kg/m² | Diagnóstico: Eutrófico (P50)", fMiniBld, darkGreen, 240, y);
            y += 20;
            g.DrawLine(pBorder, 240, y, 740, y);
            y += 10;
            DrawText(g, "3. PRESCRIPCIÓN DIETOTERÁPICA: Requerimiento: 1.350 Kcal/día | Carbos: 55% (186g) | Prot: 15% (51g) | Grasas: 30% (45g)", fMiniBld, darkGreen, 240, y);
            y += 20;
            g.DrawLine(pBorder, 240, y, 740, y);
            y += 10;
            DrawText(g, "4. METAS Y PAUTAS: Crecimiento pondoestatural armónico. 4 comidas principales + 2 colaciones saludables.", fMiniTxt, darkGreen, 240, y);
            y += 350;
            DrawText(g, "Firma y Sello Profesional: Dra. Mariana González - MP 7842", fMiniTxt, Color.FromArgb(120, 130, 125), 450, y);

            string path = Path.Combine(OutputDir, "05_Ventana_Comprobante_Preview.png");
            bmp.Save(path, ImageFormat.Png);
            Console.WriteLine($"Guardado: {path}");
        }

        private static void GenerarCapturaBitacora()
        {
            using var bmp = new Bitmap(1100, 720);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(245, 248, 245));
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Header
            using var bTop = new SolidBrush(Color.FromArgb(76, 124, 89));
            g.FillRectangle(bTop, 0, 0, 1100, 65);

            using var fTit = new Font("Segoe UI", 12f, FontStyle.Bold);
            using var fSub = new Font("Segoe UI", 9f, FontStyle.Regular);
            using var fSec = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            using var fGridH = new Font("Segoe UI", 9f, FontStyle.Bold);
            using var fGridB = new Font("Segoe UI", 8.5f, FontStyle.Regular);
            using var fCode = new Font("Consolas", 9f, FontStyle.Regular);
            Color dark = Color.FromArgb(30, 45, 35);

            DrawText(g, "BITÁCORA DE CAMBIOS Y ROLLBACK - PLANES ALIMENTARIOS (TRIGGER SQL)", fTit, Color.White, 20, 12);
            DrawText(g, "Auditoría mediante Trigger AFTER UPDATE/DELETE sobre PlanesAlimentarios_DNI101 y ConsultasNutricionales_DNI101", fSub, Color.FromArgb(220, 240, 225), 22, 38);

            // Barra de filtros
            DibujarBoton(g, 980, 14, 95, 36, Color.FromArgb(46, 95, 70), "? Ayuda", fGridH);

            int y = 78;
            DrawText(g, "Entidad a Auditar:", fSec, dark, 20, y + 4);
            DibujarCaja(g, 150, y, 220, 28, "Planes Alimentarios", fGridB);
            DrawText(g, "ID Plan:", fSec, dark, 390, y + 4);
            DibujarCaja(g, 450, y, 90, 28, "1", fGridB);
            DibujarBoton(g, 550, y, 90, 28, Color.FromArgb(76, 124, 89), "Filtrar", fGridB);
            DibujarBoton(g, 650, y, 100, 28, Color.FromArgb(100, 116, 139), "Ver Todos", fGridB);

            // Grilla de cambios
            y = 120;
            int gx = 20, gw = 1060;
            g.FillRectangle(new SolidBrush(Color.FromArgb(46, 80, 58)), gx, y, gw, 30);

            string[] cols = { "Nro Cambio", "ID Plan", "ID Consulta", "Kcal Previas", "% Carbos", "% Proteínas", "% Grasas", "Fecha Modificación", "DNI Usuario", "Operación" };
            int[] widths = { 90, 70, 90, 100, 80, 90, 80, 190, 110, 160 };

            int cx = gx;
            for (int i = 0; i < cols.Length; i++)
            {
                DrawText(g, cols[i], fGridH, Color.White, cx + 6, y + 6);
                cx += widths[i];
            }

            // Filas
            string[][] rows = {
                new[] { "1", "1", "101", "1500 kcal", "50%", "20%", "30%", $"{DateTime.Now.AddDays(-5):dd/MM/yyyy HH:mm:ss}", "11111111", "UPDATE" },
                new[] { "2", "2", "102", "1800 kcal", "60%", "15%", "25%", $"{DateTime.Now.AddDays(-2):dd/MM/yyyy HH:mm:ss}", "28994012", "UPDATE" },
                new[] { "3", "3", "103", "1400 kcal", "55%", "15%", "30%", $"{DateTime.Now.AddHours(-12):dd/MM/yyyy HH:mm:ss}", "11111111", "UPDATE" }
            };

            for (int r = 0; r < rows.Length; r++)
            {
                y += 28;
                bool sel = (r == 0);
                using var bRow = new SolidBrush(sel ? Color.FromArgb(220, 238, 225) : (r % 2 == 0 ? Color.White : Color.FromArgb(245, 249, 246)));
                g.FillRectangle(bRow, gx, y, gw, 28);
                using var pRow = new Pen(Color.FromArgb(220, 230, 225));
                g.DrawRectangle(pRow, gx, y, gw, 28);

                cx = gx;
                for (int i = 0; i < rows[r].Length; i++)
                {
                    var f = (sel && i == 0) ? fGridH : fGridB;
                    DrawText(g, rows[r][i], f, dark, cx + 6, y + 6);
                    cx += widths[i];
                }
            }

            // Detalle Histórico
            y += 45;
            DrawText(g, "Inspección de Estado Previo y Parámetros Auditados:", fSec, dark, gx, y);
            y += 22;

            using var pDet = new Pen(Color.FromArgb(180, 200, 185));
            g.FillRectangle(new SolidBrush(Color.FromArgb(250, 252, 250)), gx, y, gw, 160);
            g.DrawRectangle(pDet, gx, y, gw, 160);

            string detalle = 
                "[ID CAMBIO: 1] - Operación detectada por Trigger: UPDATE\r\n" +
                $"Fecha/Hora de Registro: {DateTime.Now.AddDays(-5):dd/MM/yyyy HH:mm:ss} | Usuario Responsable: 11111111 (Dra. González)\r\n" +
                "Plan Alimentario ID: 1 | Consulta Nutricional Asociada: ID 101 | Paciente: Mateo Benítez (DNI 48.204.110)\r\n" +
                "----------------------------------------------------------------------------------------------------\r\n" +
                "Requerimiento Calórico Previo: 1.500 Kcal/día\r\n" +
                "Distribución Macronutrientes Previa: Carbohidratos 50% (187.5g) | Proteínas 20% (75g) | Grasas 30% (50g)\r\n" +
                "Metas de Salud Previas: Descenso de peso acelerado (valor previo antes de corrección clínica pediátrica)\r\n" +
                "Pautas Familiares Previas: Restricción severa de carbohidratos simples sin justificación antropométrica\r\n" +
                "==> ESTADO ANTERIOR RECUPERABLE: Al pulsar 'Deshacer Cambio / Rollback', el plan se restaurará a esta versión.";

            DrawText(g, detalle, fCode, dark, gx + 12, y + 10);

            // Botón Rollback Inferior
            y += 180;
            DibujarBoton(g, gx, y, 320, 42, Color.FromArgb(234, 88, 12), "Deshacer Cambio / Rollback (F5)", fTit);
            DibujarBoton(g, gx + 340, y, 120, 42, Color.FromArgb(100, 116, 139), "Cerrar", fTit);

            string path = Path.Combine(OutputDir, "01_Bitacora_Cambios_Rollback.png");
            bmp.Save(path, ImageFormat.Png);
            Console.WriteLine($"Guardado: {path}");
        }

        private static void GenerarCapturaReporteInteligente()
        {
            using var bmp = new Bitmap(1150, 730);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(245, 248, 245));
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Header
            using var bTop = new SolidBrush(Color.FromArgb(76, 124, 89));
            g.FillRectangle(bTop, 0, 0, 1150, 65);

            using var fTit = new Font("Segoe UI", 12f, FontStyle.Bold);
            using var fSub = new Font("Segoe UI", 9f, FontStyle.Regular);
            using var fSec = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            using var fGridH = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            using var fGridB = new Font("Segoe UI", 8.2f, FontStyle.Regular);
            Color dark = Color.FromArgb(30, 45, 35);

            DrawText(g, "REPORTE INTELIGENTE PREDICTIVO - CRUCE MULTI-TABLA Y CURVAS OMS", fTit, Color.White, 20, 12);
            DrawText(g, "Inferencia clínica automática, Z-Scores OMS, velocidad de crecimiento y recomendación de decisión médica", fSub, Color.FromArgb(220, 240, 225), 22, 38);

            DibujarBoton(g, 1030, 14, 95, 36, Color.FromArgb(46, 95, 70), "? Ayuda", fGridH);

            int y = 78;
            int gx = 20, gw = 1110;
            g.FillRectangle(new SolidBrush(Color.FromArgb(46, 80, 58)), gx, y, gw, 30);

            string[] cols = { "DNI Niño", "Paciente", "Edad", "Últ. Peso", "Talla", "IMC", "Z-Score", "Vel. Pond.", "Vel. Estat.", "Proy. 90 Días", "Semáforo", "Recomendación Médica de Decisión" };
            int[] widths = { 75, 110, 65, 70, 65, 65, 80, 80, 75, 95, 90, 240 };

            int cx = gx;
            for (int i = 0; i < cols.Length; i++)
            {
                DrawText(g, cols[i], fGridH, Color.White, cx + 4, y + 7);
                cx += widths[i];
            }

            var rows = new[] {
                new { Dni="48204110", Pac="Benítez, Mateo", Edad="24 m", Peso="12.20 kg", Talla="86.5 cm", Imc="16.3", Z="+0.12", VP="+18.5 g/d", VE="+0.9 cm/m", Proy="13.86 kg (Normal)", Sem="VERDE", Rec="Mantener plan normocalórico. Próximo control en 60 días.", SemColor=Color.FromArgb(46, 125, 50) },
                new { Dni="49112304", Pac="Rossi, Sofía", Edad="36 m", Peso="16.80 kg", Talla="96.0 cm", Imc="18.2", Z="+1.65", VP="+28.0 g/d", VE="+0.7 cm/m", Proy="19.32 kg (Sobrepeso)", Sem="AMARILLO", Rec="Reducir colaciones azucaradas. Recontrol en 20 días.", SemColor=Color.FromArgb(245, 124, 0) },
                new { Dni="50884219", Pac="Gómez, Thiago", Edad="14 m", Peso="8.40 kg", Talla="74.0 cm", Imc="15.3", Z="-2.15", VP="+4.2 g/d", VE="+0.3 cm/m", Proy="8.78 kg (Desnutrición)", Sem="ROJO", Rec="Interconsulta urgente. Suplementación y control semanal.", SemColor=Color.FromArgb(220, 38, 38) },
                new { Dni="47901882", Pac="Martínez, Emma", Edad="48 m", Peso="22.40 kg", Talla="102.0 cm", Imc="21.5", Z="+2.48", VP="+32.4 g/d", VE="+0.8 cm/m", Proy="25.31 kg (Obesidad)", Sem="ROJO", Rec="Plan hipocalórico moderado y actividad lúdica familiar.", SemColor=Color.FromArgb(220, 38, 38) }
            };

            for (int r = 0; r < rows.Length; r++)
            {
                y += 32;
                var row = rows[r];
                using var bRow = new SolidBrush(r % 2 == 0 ? Color.White : Color.FromArgb(248, 251, 248));
                g.FillRectangle(bRow, gx, y, gw, 32);
                using var pRow = new Pen(Color.FromArgb(220, 230, 225));
                g.DrawRectangle(pRow, gx, y, gw, 32);

                string[] vals = { row.Dni, row.Pac, row.Edad, row.Peso, row.Talla, row.Imc, row.Z, row.VP, row.VE, row.Proy };
                cx = gx;
                for (int i = 0; i < vals.Length; i++)
                {
                    DrawText(g, vals[i], fGridB, dark, cx + 4, y + 8);
                    cx += widths[i];
                }

                // Semáforo Badge
                using var bSem = new SolidBrush(row.SemColor);
                g.FillRectangle(bSem, cx + 6, y + 6, 75, 20);
                DrawText(g, row.Sem, fGridH, Color.White, cx + 18, y + 8);
                cx += widths[10];

                DrawText(g, row.Rec, fGridB, dark, cx + 4, y + 8);
            }

            // Panel de inferencia global
            y += 50;
            DrawText(g, "Síntesis Epidemiológica y Proyecciones Predictivas:", fSec, dark, gx, y);
            y += 22;

            g.FillRectangle(new SolidBrush(Color.FromArgb(250, 252, 250)), gx, y, gw, 160);
            using var pPan = new Pen(Color.FromArgb(180, 200, 185));
            g.DrawRectangle(pPan, gx, y, gw, 160);

            string synth =
                "CRUCE RELACIONAL Y REGLAS DE DECISIÓN EXPERTA APLICADAS:\r\n" +
                "1. Algoritmo OMS: Evalúa correlación de tablas Pacientes_DNI101 + MedicionesAntropometricas_DNI101 + PlanesAlimentarios_DNI101.\r\n" +
                "2. Detección de Desnutrición Aguda (Z < -2): Thiago Gómez presenta velocidad ponderal inferior a 5 g/día con riesgo estatural.\r\n" +
                "3. Detección de Riesgo de Sobrepeso y Obesidad (Z > +1.5): Sofía Rossi y Emma Martínez proyectan avance a percentil crítico a 90 días.\r\n" +
                "4. Recomendación Prescriptiva Generada: Se propone ajuste en macronutrientes desde el módulo CUN09 y calendarización de turnos prioritarios.";

            DrawText(g, synth, new Font("Consolas", 9f), dark, gx + 15, y + 15);

            string path = Path.Combine(OutputDir, "03_Reporte_Inteligente_Predictivo.png");
            bmp.Save(path, ImageFormat.Png);
            Console.WriteLine($"Guardado: {path}");
        }

        private static void GenerarCapturaMaestroClientes()
        {
            using var bmp = new Bitmap(1000, 680);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(245, 248, 245));
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Header
            using var bTop = new SolidBrush(Color.FromArgb(76, 124, 89));
            g.FillRectangle(bTop, 0, 0, 1000, 65);

            using var fTit = new Font("Segoe UI", 12f, FontStyle.Bold);
            using var fSub = new Font("Segoe UI", 9f, FontStyle.Regular);
            using var fSec = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            using var fGridH = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            using var fGridB = new Font("Segoe UI", 8.5f, FontStyle.Regular);
            Color dark = Color.FromArgb(30, 45, 35);

            DrawText(g, "MAESTRO DE CLIENTES - ABM (CUN01 a CUN05)", fTit, Color.White, 20, 12);
            DrawText(g, "Gestión centralizada de pacientes pediátricos, tutores, contactos 1:N y estado de cobertura", fSub, Color.FromArgb(220, 240, 225), 22, 38);

            DibujarBoton(g, 880, 14, 95, 36, Color.FromArgb(46, 95, 70), "? Ayuda", fGridH);

            // Grilla
            int y = 78;
            int gx = 20, gw = 820;
            g.FillRectangle(new SolidBrush(Color.FromArgb(46, 80, 58)), gx, y, gw, 28);

            string[] cols = { "DNI", "APELLIDOS", "NOMBRES", "EMAIL", "CELULAR", "DIRECCIÓN" };
            int[] widths = { 90, 120, 120, 200, 120, 170 };

            int cx = gx;
            for (int i = 0; i < cols.Length; i++)
            {
                DrawText(g, cols[i], fGridH, Color.White, cx + 6, y + 6);
                cx += widths[i];
            }

            string[][] rows = {
                new[] { "48204110", "Benítez", "Mateo", "familia.benitez@gmail.com", "11-4589-2231", "Av. Santa Fe 3420, CABA" },
                new[] { "49112304", "Rossi", "Sofía", "mariana.rossi@yahoo.com", "11-6672-9901", "Belgrano 1250, Quilmes" },
                new[] { "50884219", "Gómez", "Thiago", "gomez.lucas@outlook.com", "11-3312-8874", "Mitre 450, Avellaneda" },
                new[] { "47901882", "Martínez", "Emma", "dra.martinez@fibertel.com.ar", "11-5541-0092", "Rivadavia 8840, CABA" }
            };

            for (int r = 0; r < rows.Length; r++)
            {
                y += 28;
                bool sel = (r == 0);
                using var bRow = new SolidBrush(sel ? Color.FromArgb(220, 238, 225) : (r % 2 == 0 ? Color.White : Color.FromArgb(248, 251, 248)));
                g.FillRectangle(bRow, gx, y, gw, 28);
                using var pRow = new Pen(Color.FromArgb(220, 230, 225));
                g.DrawRectangle(pRow, gx, y, gw, 28);

                cx = gx;
                for (int i = 0; i < rows[r].Length; i++)
                {
                    DrawText(g, rows[r][i], sel ? fGridH : fGridB, dark, cx + 6, y + 6);
                    cx += widths[i];
                }
            }

            // Botonera lateral derecha
            int bx = 860, by = 78;
            DibujarBoton(g, bx, by, 115, 36, Color.FromArgb(76, 124, 89), "Añadir", fSec);
            DibujarBoton(g, bx, by + 46, 115, 36, Color.FromArgb(76, 124, 89), "Modificar", fSec);
            DibujarBoton(g, bx, by + 92, 115, 36, Color.FromArgb(198, 40, 40), "Eliminar", fSec);
            DibujarBoton(g, bx, by + 148, 115, 36, Color.FromArgb(37, 99, 235), "Aplicar", fSec);
            DibujarBoton(g, bx, by + 194, 115, 36, Color.FromArgb(100, 116, 139), "Cancelar", fSec);
            DibujarBoton(g, bx, by + 240, 115, 36, Color.FromArgb(46, 125, 50), "Contactos 1:N", fSec);
            DibujarBoton(g, bx, by + 286, 115, 36, Color.FromArgb(50, 60, 55), "Salir", fSec);

            // Panel inferior de edición
            y = 230;
            DrawText(g, "Panel de Edición y Datos del Paciente (Bloqueado en Inicio):", fSec, dark, gx, y);
            y += 24;

            g.FillRectangle(new SolidBrush(Color.FromArgb(250, 252, 250)), gx, y, gw, 250);
            using var pEd = new Pen(Color.FromArgb(180, 200, 185));
            g.DrawRectangle(pEd, gx, y, gw, 250);

            int ey = y + 20;
            DibujarCampo(g, gx + 20, ey, "DNI:", "48204110", 150, fSec, fGridB);
            DibujarCampo(g, gx + 280, ey, "Apellidos:", "Benítez", 220, fSec, fGridB);
            DibujarCampo(g, gx + 540, ey, "Nombres:", "Mateo", 220, fSec, fGridB);

            ey += 50;
            DibujarCampo(g, gx + 20, ey, "Email:", "familia.benitez@gmail.com", 240, fSec, fGridB);
            DibujarCampo(g, gx + 280, ey, "Celular:", "11-4589-2231", 180, fSec, fGridB);
            DibujarCampo(g, gx + 540, ey, "Obra Social:", "OSDE 210", 220, fSec, fGridB);

            ey += 50;
            DibujarCampo(g, gx + 20, ey, "Dirección:", "Av. Santa Fe 3420, CABA", 450, fSec, fGridB);

            string path = Path.Combine(OutputDir, "02_Maestro_Clientes_ABM.png");
            bmp.Save(path, ImageFormat.Png);
            Console.WriteLine($"Guardado: {path}");
        }

        private static void GenerarCapturaContactos1N()
        {
            using var bmp = new Bitmap(850, 580);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(245, 248, 245));
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Header
            using var bTop = new SolidBrush(Color.FromArgb(76, 124, 89));
            g.FillRectangle(bTop, 0, 0, 850, 65);

            using var fTit = new Font("Segoe UI", 12f, FontStyle.Bold);
            using var fSub = new Font("Segoe UI", 9f, FontStyle.Regular);
            using var fSec = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            using var fGridH = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            using var fGridB = new Font("Segoe UI", 8.5f, FontStyle.Regular);
            Color dark = Color.FromArgb(30, 45, 35);

            DrawText(g, "GESTIÓN DE CONTACTOS 1:N - MÚLTIPLES CANALES", fTit, Color.White, 20, 12);
            DrawText(g, "Cliente / Paciente: Benítez, Mateo (DNI 48.204.110 | ID: 1)", fSub, Color.FromArgb(220, 240, 225), 22, 38);

            DibujarBoton(g, 730, 14, 95, 36, Color.FromArgb(46, 95, 70), "? Ayuda", fGridH);

            // Grilla Emails (Izquierda)
            int y = 85;
            DrawText(g, "Direcciones de Correo Electrónico (1:N):", fSec, dark, 20, y);
            y += 24;

            int ex = 20, ew = 380;
            g.FillRectangle(new SolidBrush(Color.FromArgb(46, 80, 58)), ex, y, ew, 26);
            DrawText(g, "Correo Electrónico", fGridH, Color.White, ex + 6, y + 5);
            DrawText(g, "¿Es Principal?", fGridH, Color.White, ex + 260, y + 5);

            string[][] emRows = {
                new[] { "familia.benitez@gmail.com", "Sí (Principal)" },
                new[] { "padre.benitez@empresa.com", "No" },
                new[] { "abuela.rosa@hotmail.com", "No" }
            };

            for (int r = 0; r < emRows.Length; r++)
            {
                y += 26;
                using var bRow = new SolidBrush(r == 0 ? Color.FromArgb(220, 238, 225) : Color.White);
                g.FillRectangle(bRow, ex, y, ew, 26);
                using var pRow = new Pen(Color.FromArgb(220, 230, 225));
                g.DrawRectangle(pRow, ex, y, ew, 26);
                DrawText(g, emRows[r][0], fGridB, dark, ex + 6, y + 5);
                DrawText(g, emRows[r][1], r == 0 ? fGridH : fGridB, r == 0 ? Color.FromArgb(46, 125, 50) : dark, ex + 260, y + 5);
            }

            // Grilla Teléfonos (Derecha)
            y = 85;
            int tx = 430, tw = 390;
            DrawText(g, "Teléfonos y Vías Móviles Familiares (1:N):", fSec, dark, tx, y);
            y += 24;

            g.FillRectangle(new SolidBrush(Color.FromArgb(46, 80, 58)), tx, y, tw, 26);
            DrawText(g, "Número", fGridH, Color.White, tx + 6, y + 5);
            DrawText(g, "Tipo / Vía", fGridH, Color.White, tx + 140, y + 5);
            DrawText(g, "¿Principal?", fGridH, Color.White, tx + 280, y + 5);

            string[][] telRows = {
                new[] { "11-4589-2231", "Celular Madre", "Sí (Principal)" },
                new[] { "11-6677-8899", "Celular Padre", "No" },
                new[] { "011-4780-1234", "Fijo Hogar", "No" }
            };

            for (int r = 0; r < telRows.Length; r++)
            {
                y += 26;
                using var bRow = new SolidBrush(r == 0 ? Color.FromArgb(220, 238, 225) : Color.White);
                g.FillRectangle(bRow, tx, y, tw, 26);
                using var pRow = new Pen(Color.FromArgb(220, 230, 225));
                g.DrawRectangle(pRow, tx, y, tw, 26);
                DrawText(g, telRows[r][0], fGridB, dark, tx + 6, y + 5);
                DrawText(g, telRows[r][1], fGridB, dark, tx + 140, y + 5);
                DrawText(g, telRows[r][2], r == 0 ? fGridH : fGridB, r == 0 ? Color.FromArgb(46, 125, 50) : dark, tx + 280, y + 5);
            }

            // Botones inferiores
            y = 230;
            DibujarBoton(g, ex, y, 140, 32, Color.FromArgb(76, 124, 89), "+ Agregar Email", fGridH);
            DibujarBoton(g, ex + 150, y, 140, 32, Color.FromArgb(198, 40, 40), "- Quitar Email", fGridH);

            DibujarBoton(g, tx, y, 140, 32, Color.FromArgb(76, 124, 89), "+ Agregar Teléfono", fGridH);
            DibujarBoton(g, tx + 150, y, 140, 32, Color.FromArgb(198, 40, 40), "- Quitar Teléfono", fGridH);

            string path = Path.Combine(OutputDir, "02_Contactos_1N_Paciente.png");
            bmp.Save(path, ImageFormat.Png);
            Console.WriteLine($"Guardado: {path}");
        }

        private static void GenerarCapturaManualAyuda()
        {
            using var bmp = new Bitmap(1000, 680);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(245, 248, 245));
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Header
            using var bTop = new SolidBrush(Color.FromArgb(76, 124, 89));
            g.FillRectangle(bTop, 0, 0, 1000, 65);

            using var fTit = new Font("Segoe UI", 12f, FontStyle.Bold);
            using var fSub = new Font("Segoe UI", 9f, FontStyle.Regular);
            using var fSec = new Font("Segoe UI", 10f, FontStyle.Bold);
            using var fText = new Font("Segoe UI", 9f, FontStyle.Regular);
            using var fBold = new Font("Segoe UI", 9f, FontStyle.Bold);
            Color dark = Color.FromArgb(30, 45, 35);

            DrawText(g, "SISTEMA DE AYUDA CON LINKEO E ÍNDICE INTERACTIVO", fTit, Color.White, 20, 12);
            DrawText(g, "Manual Integral de Operación, Seguridad, Backups y Procedimientos Clínicos NutriEvolve (F1)", fSub, Color.FromArgb(220, 240, 225), 22, 38);

            // Panel Izquierdo: Índice interactivo de capítulos
            int ix = 20, iy = 80, iw = 280, ih = 570;
            g.FillRectangle(new SolidBrush(Color.FromArgb(235, 242, 238)), ix, iy, iw, ih);
            using var pIdx = new Pen(Color.FromArgb(180, 200, 185));
            g.DrawRectangle(pIdx, ix, iy, iw, ih);

            DrawText(g, "ÍNDICE DE CONTENIDOS", fSec, dark, ix + 15, iy + 15);
            g.DrawLine(pIdx, ix + 15, iy + 38, ix + iw - 15, iy + 38);

            string[] caps = {
                "Capítulo 1: Gestión de Turnos Médicos",
                "Capítulo 2: Maestro de Clientes (ABM-C)",
                "Capítulo 3: Contactos Familiares 1:N",
                "Capítulo 4: Consultas y Prescripción Pediátrica",
                "Capítulo 5: Bitácora de Cambios y Rollback",
                "Capítulo 6: Reporte Inteligente Predictivo",
                "Capítulo 7: Reinstalador Seguro y Backup"
            };

            int cy = iy + 50;
            for (int i = 0; i < caps.Length; i++)
            {
                bool sel = (i == 6); // Seleccionado el capítulo 7
                if (sel)
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(76, 124, 89)), ix + 8, cy, iw - 16, 32);
                    DrawText(g, caps[i], fBold, Color.White, ix + 14, cy + 8);
                }
                else
                {
                    DrawText(g, caps[i], fText, dark, ix + 14, cy + 8);
                }
                cy += 38;
            }

            // Panel Derecho: Contenido del Capítulo 7 (Reinstalador y Backup)
            int cx = 320, cy2 = 80, cw = 660, ch = 570;
            g.FillRectangle(Brushes.White, cx, cy2, cw, ch);
            g.DrawRectangle(pIdx, cx, cy2, cw, ch);

            DrawText(g, "CAPÍTULO 7: PROCEDIMIENTO DE REINSTALACIÓN SEGURA Y BACKUPS", fSec, Color.FromArgb(76, 124, 89), cx + 25, cy2 + 20);

            // Cuadro de Alerta Roja Destacada
            int ay = cy2 + 55;
            using var bAlertBg = new SolidBrush(Color.FromArgb(254, 242, 242));
            using var pAlert = new Pen(Color.FromArgb(220, 38, 38), 2);
            g.FillRectangle(bAlertBg, cx + 25, ay, cw - 50, 80);
            g.DrawRectangle(pAlert, cx + 25, ay, cw - 50, 80);

            DrawText(g, "AVISO DE SEGURIDAD OBLIGATORIO - POLÍTICA INSTITUCIONAL DE DATOS:", fBold, Color.FromArgb(185, 28, 28), cx + 40, ay + 12);
            string alertMsg = "Antes de ejecutar cualquier proceso de actualización, migración de servidor o reinstalación del sistema,\r\n" +
                              "el profesional o administrador DEBE generar un Respaldo Completo (Backup .BAK) de la base de datos\r\n" +
                              "desde el módulo 'Administración > Respaldo y Restauración' para prevenir contingencias de hardware.";
            DrawText(g, alertMsg, fText, Color.FromArgb(127, 29, 29), cx + 40, ay + 32);

            // Explicación técnica del instalador seguro
            int ty = ay + 95;
            string txtExp = 
                "El instalador de NutriEvolve (Setup.exe) incorpora una arquitectura de detección de catálogos existentes:\r\n\r\n" +
                "1. Reconocimiento de Base de Datos Preexistente:\r\n" +
                "   Al iniciarse, el asistente consulta sys.databases. Si 'NutriEvolve_DNI101' ya existe, activa\r\n" +
                "   automáticamente el Modo Reinstalador / Reparador Seguro.\r\n\r\n" +
                "2. Preservación Estricta de Datos Clínicos:\r\n" +
                "   Por defecto, la opción 'Conservar base de datos existente' permanece activada, garantizando que ninguna\r\n" +
                "   consulta médica, evolución percentilar ni paciente registrado sea eliminado o sobrescrito.\r\n\r\n" +
                "3. Reversibilidad mediante Rollback:\r\n" +
                "   Si requiere consultar o restaurar versiones anteriores de prescripciones dietoterápicas o historias clínicas,\r\n" +
                "   utilice la pantalla de auditoría conectada a los triggers del motor SQL.";

            DrawText(g, txtExp, fText, dark, cx + 25, ty);

            // Enlaces interactivos
            int ly = ty + 260;
            DrawText(g, "Enlaces a Capítulos Relacionados:", fBold, Color.FromArgb(76, 124, 89), cx + 25, ly);
            ly += 22;
            DibujarBoton(g, cx + 25, ly, 230, 32, Color.FromArgb(59, 130, 246), "Ir a Cap. 5: Bitácora y Rollback", fText);
            DibujarBoton(g, cx + 270, ly, 230, 32, Color.FromArgb(76, 124, 89), "Ir a Cap. 4: Consultas Clínicas", fText);

            string path = Path.Combine(OutputDir, "04_Manual_Ayuda_Indice_Interactivo.png");
            bmp.Save(path, ImageFormat.Png);
            Console.WriteLine($"Guardado: {path}");
        }

        private static void GenerarCapturaReinstaladorSeguro()
        {
            using var bmp = new Bitmap(800, 580);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.FromArgb(245, 248, 245));
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Header Setup.exe
            using var bTop = new SolidBrush(Color.FromArgb(37, 99, 235));
            g.FillRectangle(bTop, 0, 0, 800, 65);

            using var fTit = new Font("Segoe UI", 12f, FontStyle.Bold);
            using var fSub = new Font("Segoe UI", 9f, FontStyle.Regular);
            using var fSec = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            using var fText = new Font("Segoe UI", 9f, FontStyle.Regular);
            using var fBold = new Font("Segoe UI", 9f, FontStyle.Bold);
            Color dark = Color.FromArgb(30, 45, 35);

            DrawText(g, "ASISTENTE DE INSTALACIÓN Y REPARACIÓN - NUTRIEVOLVE", fTit, Color.White, 20, 12);
            DrawText(g, "Paso 3 de 5: Configuración de Base de datos", fSub, Color.FromArgb(220, 235, 255), 22, 38);

            // Badge Reinstalación
            using var bBadge = new SolidBrush(Color.FromArgb(234, 88, 12));
            g.FillRectangle(bBadge, 620, 16, 150, 32);
            DrawText(g, "REINSTALACIÓN", fBold, Color.White, 638, 24);

            int y = 85;
            DrawText(g, "Servidor / Instancia SQL Server:", fSec, dark, 30, y);
            y += 24;
            DibujarCaja(g, 30, y, 350, 30, "(localdb)\\MSSQLLocalDB", fText);
            DibujarBoton(g, 395, y, 130, 30, Color.FromArgb(37, 99, 235), "Probar Conexión", fBold);

            // Panel de Detección de Base Previa
            y += 45;
            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 247, 237)), 30, y, 740, 60);
            using var pWarn = new Pen(Color.FromArgb(234, 88, 12), 2);
            g.DrawRectangle(pWarn, 30, y, 740, 60);

            using var fAviso = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            DrawText(g, "SE DETECTO UNA BASE DE DATOS EXISTENTE EN ESTE SERVIDOR", fAviso, Color.FromArgb(194, 65, 12), 45, y + 18);

            // Botones inferiores
            y = 515;
            DibujarBoton(g, 460, y, 120, 38, Color.FromArgb(100, 116, 139), "< Anterior", fBold);
            DibujarBoton(g, 595, y, 175, 38, Color.FromArgb(234, 88, 12), "Reinstalar Ahora >", fBold);

            string path = Path.Combine(OutputDir, "06_Reinstalador_Modo_Seguro.png");
            bmp.Save(path, ImageFormat.Png);
            Console.WriteLine($"Guardado: {path}");
        }

        private static void DibujarBoton(Graphics g, int x, int y, int w, int h, Color bg, string text, Font font)
        {
            using var b = new SolidBrush(bg);
            g.FillRectangle(b, x, y, w, h);
            using var p = new Pen(Color.FromArgb(20, 0, 0, 0));
            g.DrawRectangle(p, x, y, w, h);

            var size = g.MeasureString(text, font);
            float tx = x + (w - size.Width) / 2;
            float ty = y + (h - size.Height) / 2;
            using var bWhite = new SolidBrush(Color.White);
            g.DrawString(text, font, bWhite, tx, ty);
        }

        private static void DibujarCaja(Graphics g, int x, int y, int w, int h, string text, Font font)
        {
            g.FillRectangle(Brushes.White, x, y, w, h);
            using var p = new Pen(Color.FromArgb(180, 200, 185));
            g.DrawRectangle(p, x, y, w, h);
            DrawText(g, text, font, Color.FromArgb(30, 45, 35), x + 6, y + 6);
        }

        private static void DibujarCampo(Graphics g, int x, int y, string label, string val, int w, Font fLbl, Font fVal)
        {
            DrawText(g, label, fLbl, Color.FromArgb(76, 124, 89), x, y);
            DibujarCaja(g, x, y + 18, w, 28, val, fVal);
        }
    }
}
