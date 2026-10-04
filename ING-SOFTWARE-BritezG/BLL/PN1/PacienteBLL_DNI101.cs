using BE;
using DAL;
using Services;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class PacienteBLL_DNI101
    {
        private readonly PacienteDAL_DNI101 _pacienteDAL = new();
        private readonly BitacoraBLL _bitacoraBLL = new();

        /// <summary>
        /// Método unificado de RegistrarPaciente (CUN02 - Registrar Paciente Pediátrico).
        /// Recibe los datos del paciente, valida la información, verifica duplicados por DNI,
        /// construye la entidad PacienteBE_DNI101, guarda el registro, audita en bitácora y actualiza DV.
        /// </summary>
        public int RegistrarPaciente(string nombre, string apellido, string dniNiño, string? telefono = null, string? email = null, string? obraSocial = null)
        {
            if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("El nombre del paciente es obligatorio.");
            if (string.IsNullOrWhiteSpace(apellido)) throw new ArgumentException("El apellido del paciente es obligatorio.");
            if (string.IsNullOrWhiteSpace(dniNiño)) throw new ArgumentException("El DNI del niño es obligatorio.");

            dniNiño = dniNiño.Trim();

            // Validar si ya existe un paciente registrado con el mismo DNI
            var pacienteExistente = ObtenerPacientePorDNI(dniNiño);
            if (pacienteExistente != null)
            {
                throw new InvalidOperationException($"El paciente con DNI {dniNiño} ya se encuentra registrado.");
            }

            var nuevoPaciente = new PacienteBE_DNI101
            {
                Nombre_DNI101 = nombre.Trim(),
                Apellido_DNI101 = apellido.Trim(),
                DNINiño_DNI101 = dniNiño,
                Telefono_DNI101 = string.IsNullOrWhiteSpace(telefono) ? null : telefono.Trim(),
                Email_DNI101 = string.IsNullOrWhiteSpace(email) ? null : email.Trim(),
                ObraSocial_DNI101 = string.IsNullOrWhiteSpace(obraSocial) ? "Particular" : obraSocial.Trim(),
                FechaNacimiento_DNI101 = DateTime.Today
            };

            try
            {
                int idGenerado = _pacienteDAL.Guardar(
                    nuevoPaciente.DNINiño_DNI101,
                    nuevoPaciente.Nombre_DNI101,
                    nuevoPaciente.Apellido_DNI101,
                    nuevoPaciente.Telefono_DNI101,
                    nuevoPaciente.Email_DNI101,
                    nuevoPaciente.FechaNacimiento_DNI101,
                    nuevoPaciente.Sexo_DNI101,
                    nuevoPaciente.ObraSocial_DNI101,
                    nuevoPaciente.DV
                );
                nuevoPaciente.IdPaciente_DNI101 = idGenerado;

                try
                {
                    int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                    _bitacoraBLL.RegistrarBitacora(1, $"Registro de Paciente Pediátrico: {nuevoPaciente.NombreCompleto} (DNI: {nuevoPaciente.DNINiño_DNI101})", dniActual, "TurneroNutricional");
                }
                catch { }

                try
                {
                    new DigitoVerificadorBLL().RecalcularYPersistir();
                }
                catch { }

                return idGenerado;
            }
            catch (Exception ex)
            {
                try
                {
                    int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                    _bitacoraBLL.RegistrarBitacora(3, $"Error al registrar paciente: {ex.Message}", dniActual, "TurneroNutricional");
                }
                catch { }
                throw;
            }
        }

        public PacienteBE_DNI101? ObtenerPacientePorDNI(string dniNiño)
        {
            if (string.IsNullOrWhiteSpace(dniNiño)) return null;
            DataTable dt = _pacienteDAL.ObtenerPacientePorDNI(dniNiño.Trim());
            if (dt != null && dt.Rows.Count > 0)
            {
                return MapearPaciente(dt.Rows[0]);
            }
            return null;
        }

        /// <summary>
        /// Orquesta la identificación o alta de un paciente (CUN-01 con Punto de Extensión CUN-02):
        /// Busca al paciente por su DNI. Si no se encuentra en el padrón, dispara la solicitud de registro
        /// delegando la interacción visual mediante un callback sin acoplar la BLL a la interfaz de usuario.
        /// </summary>
        public PacienteBE_DNI101? ObtenerOAsegurarPaciente(string dniNiño, Func<string, bool> solicitarRegistroUI)
        {
            if (string.IsNullOrWhiteSpace(dniNiño)) return null;
            if (solicitarRegistroUI == null) throw new ArgumentNullException(nameof(solicitarRegistroUI));

            string dniLimpio = dniNiño.Trim();

            // Paso 5: Buscar al paciente en el padrón
            var paciente = ObtenerPacientePorDNI(dniLimpio);
            if (paciente != null)
            {
                return paciente;
            }

            // Punto de Extensión CUN-02: Paciente no registrado
            // La BLL orquesta el flujo de negocio invocando la acción de registro
            bool registradoExitosamente = solicitarRegistroUI(dniLimpio);
            if (registradoExitosamente)
            {
                return ObtenerPacientePorDNI(dniLimpio);
            }

            return null;
        }

        public PacienteBE_DNI101? ObtenerPorId(int idPaciente)
        {
            DataTable dt = _pacienteDAL.ObtenerPorId(idPaciente);
            if (dt != null && dt.Rows.Count > 0)
            {
                return MapearPaciente(dt.Rows[0]);
            }
            return null;
        }

        public List<PacienteBE_DNI101> ListarPacientes()
        {
            DataTable dt = _pacienteDAL.ListarTodos();
            var lista = new List<PacienteBE_DNI101>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(MapearPaciente(row));
                }
            }
            return lista;
        }

        private PacienteBE_DNI101 MapearPaciente(DataRow row)
        {
            return new PacienteBE_DNI101
            {
                IdPaciente_DNI101 = Convert.ToInt32(row["IdPaciente_DNI101"]),
                DNINiño_DNI101 = row["DniNiño_DNI101"].ToString() ?? string.Empty,
                Nombre_DNI101 = row["Nombre_DNI101"].ToString() ?? string.Empty,
                Apellido_DNI101 = row["Apellido_DNI101"].ToString() ?? string.Empty,
                Telefono_DNI101 = row["Telefono_DNI101"] != DBNull.Value ? row["Telefono_DNI101"].ToString() : null,
                Email_DNI101 = row["Email_DNI101"] != DBNull.Value ? row["Email_DNI101"].ToString() : null,
                FechaNacimiento_DNI101 = Convert.ToDateTime(row["FechaNacimiento_DNI101"]),
                Sexo_DNI101 = row["Sexo_DNI101"] != DBNull.Value ? row["Sexo_DNI101"].ToString() : null,
                ObraSocial_DNI101 = row["ObraSocial_DNI101"] != DBNull.Value ? row["ObraSocial_DNI101"].ToString() : null,
                DV = row["DV"] != DBNull.Value ? row["DV"].ToString() : null
            };
        }

        public bool ModificarPaciente(PacienteBE_DNI101 paciente)
        {
            if (paciente == null || paciente.IdPaciente_DNI101 <= 0) return false;
            try
            {
                bool ok = _pacienteDAL.Modificar(
                    paciente.IdPaciente_DNI101,
                    paciente.DNINiño_DNI101,
                    paciente.Nombre_DNI101,
                    paciente.Apellido_DNI101,
                    paciente.Telefono_DNI101,
                    paciente.Email_DNI101,
                    paciente.FechaNacimiento_DNI101,
                    paciente.Sexo_DNI101,
                    paciente.ObraSocial_DNI101,
                    paciente.DV
                );
                if (ok)
                {
                    try
                    {
                        int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                        _bitacoraBLL.RegistrarBitacora(2, $"Modificación de datos del Paciente: {paciente.NombreCompleto} (DNI: {paciente.DNINiño_DNI101})", dniActual, "TurneroNutricional");
                    }
                    catch { }

                    try
                    {
                        new DigitoVerificadorBLL().RecalcularYPersistir();
                    }
                    catch { }
                }
                return ok;
            }
            catch (Exception ex)
            {
                try
                {
                    int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                    _bitacoraBLL.RegistrarBitacora(1, $"Error al modificar datos del Paciente (DNI: {paciente.DNINiño_DNI101}): {ex.Message}", dniActual, "TurneroNutricional");
                }
                catch { }
                throw;
            }
        }
    }
}
