using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using BE.PN1;
using BE.PN2;

namespace DAL.PN2
{
    public class BitacoraCambiosConsultaDAL_DNI101
    {
        private readonly Conexion _conexion;

        public BitacoraCambiosConsultaDAL_DNI101()
        {
            _conexion = new Conexion();
        }

        public List<ConsultaCambioBE_DNI101> ListarCambios(int? idConsulta = null, string? nombrePaciente = null, DateTime? fechaIni = null, DateTime? fechaFin = null)
        {
            var lista = new List<ConsultaCambioBE_DNI101>();
            string sql = @"
                SELECT c.IdCambio, c.IdConsulta_DNI101, c.IdPaciente_DNI101, c.DniNutricionista_DNI101,
                       c.FechaControl_DNI101, c.EdadMeses_DNI101, c.TipoLactancia_DNI101,
                       c.AlimentacionComplementaria_DNI101, c.Alergias_DNI101, c.AntecedentesFamiliares_DNI101,
                       c.Observaciones_DNI101, c.FechaCambio, c.UsuarioDni, c.TipoOperacion,
                       ISNULL(c.Act, 1) AS Act,
                       ISNULL(p.Nombre_DNI101, '') AS Nombre,
                       ISNULL(p.Apellido_DNI101, '') AS Apellido,
                       ISNULL(p.DniNiño_DNI101, '') AS DniNiño,
                       c.PesoKg_DNI101, c.TallaCm_DNI101, c.PerimetroCefalicoCm_DNI101, c.IMC_DNI101, c.ClasificacionOMS_DNI101
                FROM ConsultasNutricionales_Cambios_DNI101 c
                LEFT JOIN Pacientes_DNI101 p ON c.IdPaciente_DNI101 = p.IdPaciente_DNI101
                WHERE 1 = 1 ";

            if (idConsulta.HasValue && idConsulta.Value > 0)
            {
                sql += " AND c.IdConsulta_DNI101 = @IdConsulta ";
            }
            if (!string.IsNullOrWhiteSpace(nombrePaciente))
            {
                sql += " AND (p.Nombre_DNI101 LIKE @Nombre OR p.Apellido_DNI101 LIKE @Nombre OR (p.Apellido_DNI101 + ' ' + p.Nombre_DNI101) LIKE @Nombre) ";
            }
            if (fechaIni.HasValue)
            {
                sql += " AND c.FechaCambio >= @FechaIni ";
            }
            if (fechaFin.HasValue)
            {
                sql += " AND c.FechaCambio <= @FechaFin ";
            }

            sql += " ORDER BY c.FechaCambio DESC, c.IdCambio DESC";

            using (var cn = _conexion.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                if (idConsulta.HasValue && idConsulta.Value > 0)
                {
                    cmd.Parameters.AddWithValue("@IdConsulta", idConsulta.Value);
                }
                if (!string.IsNullOrWhiteSpace(nombrePaciente))
                {
                    cmd.Parameters.AddWithValue("@Nombre", $"%{nombrePaciente.Trim()}%");
                }
                if (fechaIni.HasValue)
                {
                    cmd.Parameters.AddWithValue("@FechaIni", fechaIni.Value.Date);
                }
                if (fechaFin.HasValue)
                {
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin.Value.Date.AddDays(1).AddTicks(-1));
                }

                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string nom = dr.GetString(15);
                        string ape = dr.GetString(16);
                        string nombreCompleto = string.IsNullOrWhiteSpace(ape) ? nom : $"{ape}, {nom}";

                        lista.Add(new ConsultaCambioBE_DNI101
                        {
                            IdCambio = dr.GetInt32(0),
                            IdConsulta = dr.GetInt32(1),
                            IdPaciente = dr.GetInt32(2),
                            DniNutricionista = dr.GetInt32(3),
                            FechaControl = dr.GetDateTime(4),
                            EdadMeses = dr.GetInt32(5),
                            TipoLactancia = dr.IsDBNull(6) ? null : dr.GetString(6),
                            AlimentacionComplementaria = dr.IsDBNull(7) ? null : dr.GetString(7),
                            Alergias = dr.IsDBNull(8) ? null : dr.GetString(8),
                            AntecedentesFamiliares = dr.IsDBNull(9) ? null : dr.GetString(9),
                            Observaciones = dr.IsDBNull(10) ? null : dr.GetString(10),
                            FechaCambio = dr.GetDateTime(11),
                            UsuarioDni = dr.GetInt32(12),
                            TipoOperacion = dr.GetString(13),
                            Act = dr.GetBoolean(14),
                            NombrePaciente = nombreCompleto,
                            DniPaciente = dr.GetString(17),
                            PesoKg = dr.IsDBNull(18) ? null : dr.GetDecimal(18),
                            TallaCm = dr.IsDBNull(19) ? null : dr.GetDecimal(19),
                            PerimetroCefalico = dr.IsDBNull(20) ? null : dr.GetDecimal(20),
                            IMC = dr.IsDBNull(21) ? null : dr.GetDecimal(21),
                            ClasificacionOMS = dr.IsDBNull(22) ? null : dr.GetString(22)
                        });
                    }
                }
            }
            return lista;
        }

        public bool ActivarVersionHistorica(int idCambio, int usuarioDni)
        {
            return _conexion.ExecuteTransaction(tx =>
            {
                // 1. Obtener los valores del estado histórico registrado
                string sqlSel = @"
                    SELECT IdConsulta_DNI101, TipoLactancia_DNI101, AlimentacionComplementaria_DNI101,
                           Alergias_DNI101, AntecedentesFamiliares_DNI101, Observaciones_DNI101,
                           EdadMeses_DNI101, FechaControl_DNI101,
                           PesoKg_DNI101, TallaCm_DNI101, PerimetroCefalicoCm_DNI101, IMC_DNI101, ClasificacionOMS_DNI101
                    FROM ConsultasNutricionales_Cambios_DNI101
                    WHERE IdCambio = @IdCambio";

                int idConsulta = 0;
                string? tipoLactancia = null;
                string? alimComp = null;
                string? alergias = null;
                string? antFam = null;
                string? observaciones = null;
                int edadMeses = 0;
                DateTime fechaControl = DateTime.Now;
                decimal? pesoKg = null;
                decimal? tallaCm = null;
                decimal? perimetroCefalico = null;
                decimal? imc = null;
                string? clasifOMS = null;

                using (var cmdSel = new SqlCommand(sqlSel, tx.Connection, tx))
                {
                    cmdSel.Parameters.AddWithValue("@IdCambio", idCambio);
                    using (var dr = cmdSel.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            idConsulta = dr.GetInt32(0);
                            tipoLactancia = dr.IsDBNull(1) ? null : dr.GetString(1);
                            alimComp = dr.IsDBNull(2) ? null : dr.GetString(2);
                            alergias = dr.IsDBNull(3) ? null : dr.GetString(3);
                            antFam = dr.IsDBNull(4) ? null : dr.GetString(4);
                            observaciones = dr.IsDBNull(5) ? null : dr.GetString(5);
                            edadMeses = dr.GetInt32(6);
                            fechaControl = dr.GetDateTime(7);
                            pesoKg = dr.IsDBNull(8) ? null : dr.GetDecimal(8);
                            tallaCm = dr.IsDBNull(9) ? null : dr.GetDecimal(9);
                            perimetroCefalico = dr.IsDBNull(10) ? null : dr.GetDecimal(10);
                            imc = dr.IsDBNull(11) ? null : dr.GetDecimal(11);
                            clasifOMS = dr.IsDBNull(12) ? null : dr.GetString(12);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                // 2. Desmarcar como activo cualquier otro registro de la misma consulta y marcar 1 en este
                string sqlAct = @"
                    UPDATE ConsultasNutricionales_Cambios_DNI101
                    SET Act = 0
                    WHERE IdConsulta_DNI101 = @IdConsulta;

                    UPDATE ConsultasNutricionales_Cambios_DNI101
                    SET Act = 1
                    WHERE IdCambio = @IdCambio;";

                using (var cmdAct = new SqlCommand(sqlAct, tx.Connection, tx))
                {
                    cmdAct.Parameters.AddWithValue("@IdConsulta", idConsulta);
                    cmdAct.Parameters.AddWithValue("@IdCambio", idCambio);
                    cmdAct.ExecuteNonQuery();
                }

                // 3. Establecer SESSION_CONTEXT para que el trigger no registre una versión duplicada
                using (var cmdCtx = new SqlCommand(@"
                    EXEC sp_set_session_context @key=N'UsuarioDni', @value=@val;
                    EXEC sp_set_session_context @key=N'EsReversion', @value=1;", tx.Connection, tx))
                {
                    cmdCtx.Parameters.AddWithValue("@val", usuarioDni);
                    cmdCtx.ExecuteNonQuery();
                }

                // 4. Restaurar los valores en ConsultasNutricionales_DNI101, MedicionesAntropometricas_DNI101 y DiagnosticosNutricionales_DNI101
                string sqlUpd = @"
                    UPDATE ConsultasNutricionales_DNI101
                    SET TipoLactancia_DNI101 = @Lact,
                        AlimentacionComplementaria_DNI101 = @Alim,
                        Alergias_DNI101 = @Alergias,
                        AntecedentesFamiliares_DNI101 = @AntFam,
                        Observaciones_DNI101 = @Obs,
                        EdadMeses_DNI101 = @Edad,
                        FechaControl_DNI101 = @FechaControl
                    WHERE IdConsulta_DNI101 = @IdConsulta;

                    IF @Peso IS NOT NULL
                    BEGIN
                        UPDATE MedicionesAntropometricas_DNI101
                        SET PesoKg_DNI101 = @Peso,
                            TallaCm_DNI101 = @Talla,
                            PerimetroCefalicoCm_DNI101 = @Perimetro,
                            IMC_DNI101 = @IMC
                        WHERE IdConsulta_DNI101 = @IdConsulta;
                    END

                    IF @Clasif IS NOT NULL
                    BEGIN
                        UPDATE DiagnosticosNutricionales_DNI101
                        SET ClasificacionOMS_DNI101 = @Clasif
                        WHERE IdConsulta_DNI101 = @IdConsulta;
                    END;";

                using (var cmdUpd = new SqlCommand(sqlUpd, tx.Connection, tx))
                {
                    cmdUpd.Parameters.AddWithValue("@IdConsulta", idConsulta);
                    cmdUpd.Parameters.AddWithValue("@Lact", (object?)tipoLactancia ?? DBNull.Value);
                    cmdUpd.Parameters.AddWithValue("@Alim", (object?)alimComp ?? DBNull.Value);
                    cmdUpd.Parameters.AddWithValue("@Alergias", (object?)alergias ?? DBNull.Value);
                    cmdUpd.Parameters.AddWithValue("@AntFam", (object?)antFam ?? DBNull.Value);
                    cmdUpd.Parameters.AddWithValue("@Obs", (object?)observaciones ?? DBNull.Value);
                    cmdUpd.Parameters.AddWithValue("@Edad", edadMeses);
                    cmdUpd.Parameters.AddWithValue("@FechaControl", fechaControl);
                    cmdUpd.Parameters.AddWithValue("@Peso", (object?)pesoKg ?? DBNull.Value);
                    cmdUpd.Parameters.AddWithValue("@Talla", (object?)tallaCm ?? DBNull.Value);
                    cmdUpd.Parameters.AddWithValue("@Perimetro", (object?)perimetroCefalico ?? DBNull.Value);
                    cmdUpd.Parameters.AddWithValue("@IMC", (object?)imc ?? DBNull.Value);
                    cmdUpd.Parameters.AddWithValue("@Clasif", (object?)clasifOMS ?? DBNull.Value);
                    cmdUpd.ExecuteNonQuery();
                }

                // 5. Restablecer contexto
                using (var cmdClear = new SqlCommand("EXEC sp_set_session_context @key=N'EsReversion', @value=0;", tx.Connection, tx))
                {
                    cmdClear.ExecuteNonQuery();
                }

                return true;
            });
        }

        public bool RevertirCambio(int idCambio, int usuarioDni)
        {
            return ActivarVersionHistorica(idCambio, usuarioDni);
        }

        public List<PlanAlimentarioCambioBE_DNI101> ListarCambiosPlanes(int? idPlan = null)
        {
            var lista = new List<PlanAlimentarioCambioBE_DNI101>();
            string sql = @"
                SELECT IdCambio, IdPlan_DNI101, IdConsulta_DNI101, RequerimientoCalorico_DNI101,
                       PctCarbohidratos_DNI101, PctProteinas_DNI101, PctGrasas_DNI101,
                       PautasFamiliares_DNI101, MetasSalud_DNI101, FechaCambio, UsuarioDni, TipoOperacion
                FROM PlanesAlimentarios_Cambios_DNI101 ";

            if (idPlan.HasValue && idPlan.Value > 0)
            {
                sql += " WHERE IdPlan_DNI101 = @IdPlan ";
            }
            sql += " ORDER BY FechaCambio DESC, IdCambio DESC";

            using (var cn = _conexion.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                if (idPlan.HasValue && idPlan.Value > 0)
                {
                    cmd.Parameters.AddWithValue("@IdPlan", idPlan.Value);
                }
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new PlanAlimentarioCambioBE_DNI101
                        {
                            IdCambio = dr.GetInt32(0),
                            IdPlan = dr.GetInt32(1),
                            IdConsulta = dr.GetInt32(2),
                            RequerimientoCalorico = dr.GetDecimal(3),
                            PctCarbohidratos = dr.GetDecimal(4),
                            PctProteinas = dr.GetDecimal(5),
                            PctGrasas = dr.GetDecimal(6),
                            PautasFamiliares = dr.IsDBNull(7) ? null : dr.GetString(7),
                            MetasSalud = dr.IsDBNull(8) ? null : dr.GetString(8),
                            FechaCambio = dr.GetDateTime(9),
                            UsuarioDni = dr.GetInt32(10),
                            TipoOperacion = dr.GetString(11)
                        });
                    }
                }
            }
            return lista;
        }

        public bool RevertirCambioPlan(int idCambio, int usuarioDni)
        {
            return _conexion.ExecuteTransaction(tx =>
            {
                string sqlSel = @"
                    SELECT IdPlan_DNI101, RequerimientoCalorico_DNI101, PctCarbohidratos_DNI101,
                           PctProteinas_DNI101, PctGrasas_DNI101, PautasFamiliares_DNI101, MetasSalud_DNI101
                    FROM PlanesAlimentarios_Cambios_DNI101
                    WHERE IdCambio = @IdCambio";

                int idPlan = 0;
                decimal kcal = 0, carbos = 0, prote = 0, grasas = 0;
                string? pautas = null, metas = null;

                using (var cmdSel = new SqlCommand(sqlSel, tx.Connection, tx))
                {
                    cmdSel.Parameters.AddWithValue("@IdCambio", idCambio);
                    using (var dr = cmdSel.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            idPlan = dr.GetInt32(0);
                            kcal = dr.GetDecimal(1);
                            carbos = dr.GetDecimal(2);
                            prote = dr.GetDecimal(3);
                            grasas = dr.GetDecimal(4);
                            pautas = dr.IsDBNull(5) ? null : dr.GetString(5);
                            metas = dr.IsDBNull(6) ? null : dr.GetString(6);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                using (var cmdCtx = new SqlCommand("EXEC sp_set_session_context @key=N'UsuarioDni', @value=@val;", tx.Connection, tx))
                {
                    cmdCtx.Parameters.AddWithValue("@val", usuarioDni);
                    cmdCtx.ExecuteNonQuery();
                }

                string sqlUpd = @"
                    UPDATE PlanesAlimentarios_DNI101
                    SET RequerimientoCalorico_DNI101 = @Kcal,
                        PctCarbohidratos_DNI101 = @Carbos,
                        PctProteinas_DNI101 = @Prote,
                        PctGrasas_DNI101 = @Grasas,
                        PautasFamiliares_DNI101 = @Pautas,
                        MetasSalud_DNI101 = @Metas
                    WHERE IdPlan_DNI101 = @IdPlan;";

                using (var cmdUpd = new SqlCommand(sqlUpd, tx.Connection, tx))
                {
                    cmdUpd.Parameters.AddWithValue("@IdPlan", idPlan);
                    cmdUpd.Parameters.AddWithValue("@Kcal", kcal);
                    cmdUpd.Parameters.AddWithValue("@Carbos", carbos);
                    cmdUpd.Parameters.AddWithValue("@Prote", prote);
                    cmdUpd.Parameters.AddWithValue("@Grasas", grasas);
                    cmdUpd.Parameters.AddWithValue("@Pautas", (object?)pautas ?? DBNull.Value);
                    cmdUpd.Parameters.AddWithValue("@Metas", (object?)metas ?? DBNull.Value);
                    cmdUpd.ExecuteNonQuery();
                }

                return true;
            });
        }

        public List<PacienteCambioBE_DNI101> ListarCambiosPacientes(int? idPaciente = null)
        {
            var lista = new List<PacienteCambioBE_DNI101>();
            string sql = @"
                SELECT IdCambio, IdPaciente_DNI101, DniNiño_DNI101, Nombre_DNI101, Apellido_DNI101,
                       Telefono_DNI101, Email_DNI101, Direccion_DNI101, Activo_DNI101,
                       FechaCambio, UsuarioDni, TipoOperacion
                FROM Pacientes_Cambios_DNI101 ";

            if (idPaciente.HasValue && idPaciente.Value > 0)
            {
                sql += " WHERE IdPaciente_DNI101 = @IdPaciente ";
            }
            sql += " ORDER BY FechaCambio DESC, IdCambio DESC";

            using (var cn = _conexion.CrearConexion())
            using (var cmd = new SqlCommand(sql, cn))
            {
                if (idPaciente.HasValue && idPaciente.Value > 0)
                {
                    cmd.Parameters.AddWithValue("@IdPaciente", idPaciente.Value);
                }
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new PacienteCambioBE_DNI101
                        {
                            IdCambio = dr.GetInt32(0),
                            IdPaciente = dr.GetInt32(1),
                            Dni = dr.GetString(2),
                            Nombre = dr.GetString(3),
                            Apellido = dr.GetString(4),
                            Telefono = dr.IsDBNull(5) ? null : dr.GetString(5),
                            Email = dr.IsDBNull(6) ? null : dr.GetString(6),
                            Direccion = dr.IsDBNull(7) ? null : dr.GetString(7),
                            Activo = dr.GetBoolean(8),
                            FechaCambio = dr.GetDateTime(9),
                            UsuarioDni = dr.GetInt32(10),
                            TipoOperacion = dr.GetString(11)
                        });
                    }
                }
            }
            return lista;
        }

        public bool RevertirCambioPaciente(int idCambio, int usuarioDni)
        {
            return _conexion.ExecuteTransaction(tx =>
            {
                string sqlSel = @"
                    SELECT IdPaciente_DNI101, Nombre_DNI101, Apellido_DNI101,
                           Telefono_DNI101, Email_DNI101, Direccion_DNI101, Activo_DNI101
                    FROM Pacientes_Cambios_DNI101
                    WHERE IdCambio = @IdCambio";

                int idPaciente = 0;
                string? nombre = null, apellido = null, tel = null, email = null, dir = null;
                bool activo = true;

                using (var cmdSel = new SqlCommand(sqlSel, tx.Connection, tx))
                {
                    cmdSel.Parameters.AddWithValue("@IdCambio", idCambio);
                    using (var dr = cmdSel.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            idPaciente = dr.GetInt32(0);
                            nombre = dr.GetString(1);
                            apellido = dr.GetString(2);
                            tel = dr.IsDBNull(3) ? null : dr.GetString(3);
                            email = dr.IsDBNull(4) ? null : dr.GetString(4);
                            dir = dr.IsDBNull(5) ? null : dr.GetString(5);
                            activo = dr.GetBoolean(6);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                using (var cmdCtx = new SqlCommand("EXEC sp_set_session_context @key=N'UsuarioDni', @value=@val;", tx.Connection, tx))
                {
                    cmdCtx.Parameters.AddWithValue("@val", usuarioDni);
                    cmdCtx.ExecuteNonQuery();
                }

                string sqlUpd = @"
                    UPDATE Pacientes_DNI101
                    SET Nombre_DNI101 = @Nombre,
                        Apellido_DNI101 = @Apellido,
                        Telefono_DNI101 = @Tel,
                        Email_DNI101 = @Email,
                        Direccion_DNI101 = @Dir,
                        Activo_DNI101 = @Activo
                    WHERE IdPaciente_DNI101 = @IdPaciente;";

                using (var cmdUpd = new SqlCommand(sqlUpd, tx.Connection, tx))
                {
                    cmdUpd.Parameters.AddWithValue("@IdPaciente", idPaciente);
                    cmdUpd.Parameters.AddWithValue("@Nombre", (object?)nombre ?? DBNull.Value);
                    cmdUpd.Parameters.AddWithValue("@Apellido", (object?)apellido ?? DBNull.Value);
                    cmdUpd.Parameters.AddWithValue("@Tel", (object?)tel ?? DBNull.Value);
                    cmdUpd.Parameters.AddWithValue("@Email", (object?)email ?? DBNull.Value);
                    cmdUpd.Parameters.AddWithValue("@Dir", (object?)dir ?? DBNull.Value);
                    cmdUpd.Parameters.AddWithValue("@Activo", activo);
                    cmdUpd.ExecuteNonQuery();
                }

                return true;
            });
        }
    }
}
