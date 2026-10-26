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

        public DataTable ListarMaestro()
        {
            return _pacienteDAL.ListarMaestro();
        }

        public int GuardarMaestro(string dni, string apellidos, string nombres, string? email, string? celular, string? direccion)
        {
            if (string.IsNullOrWhiteSpace(dni)) throw new ArgumentException("El DNI es obligatorio.");
            if (string.IsNullOrWhiteSpace(apellidos)) throw new ArgumentException("El apellido es obligatorio.");
            if (string.IsNullOrWhiteSpace(nombres)) throw new ArgumentException("El nombre es obligatorio.");

            dni = dni.Trim();
            var existente = ObtenerPacientePorDNI(dni);
            if (existente != null)
            {
                throw new InvalidOperationException($"Ya existe un paciente registrado con el DNI {dni}.");
            }

            int id = _pacienteDAL.GuardarMaestro(dni, apellidos, nombres, email, celular, direccion);
            try
            {
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                _bitacoraBLL.RegistrarBitacora(2, $"Alta de Cliente/Paciente: {apellidos}, {nombres} (DNI: {dni}) en Maestro ABM-C", dniActual, "MaestroClientes");
            }
            catch { }

            try
            {
                new DigitoVerificadorBLL().RecalcularYPersistir();
            }
            catch { }

            return id;
        }

        public bool ModificarMaestro(int idPaciente, string dni, string apellidos, string nombres, string? email, string? celular, string? direccion)
        {
            if (idPaciente <= 0) throw new ArgumentException("Identificador de paciente inválido.");
            if (string.IsNullOrWhiteSpace(dni)) throw new ArgumentException("El DNI es obligatorio.");
            if (string.IsNullOrWhiteSpace(apellidos)) throw new ArgumentException("El apellido es obligatorio.");
            if (string.IsNullOrWhiteSpace(nombres)) throw new ArgumentException("El nombre es obligatorio.");

            bool ok = _pacienteDAL.ModificarMaestro(idPaciente, dni, apellidos, nombres, email, celular, direccion);
            try
            {
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                _bitacoraBLL.RegistrarBitacora(2, $"Modificación en Maestro ABM-C del Cliente/Paciente ID {idPaciente} (DNI: {dni})", dniActual, "MaestroClientes");
            }
            catch { }

            if (ok)
            {
                try
                {
                    new DigitoVerificadorBLL().RecalcularYPersistir();
                }
                catch { }
            }

            return ok;
        }

        public bool EliminarLogico(int idPaciente)
        {
            if (idPaciente <= 0) throw new ArgumentException("Identificador de paciente inválido.");

            bool ok = _pacienteDAL.EliminarLogico(idPaciente);
            try
            {
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                _bitacoraBLL.RegistrarBitacora(3, $"Baja Lógica del Cliente/Paciente ID {idPaciente} y sus datos 1:N asociados", dniActual, "MaestroClientes");
            }
            catch { }

            if (ok)
            {
                try
                {
                    new DigitoVerificadorBLL().RecalcularYPersistir();
                }
                catch { }
            }

            return ok;
        }

        public DataTable ObtenerEmails1N(int idPaciente)
        {
            return _pacienteDAL.ObtenerEmails1N(idPaciente);
        }

        public void AgregarEmail1N(int idPaciente, string email, bool esPrincipal)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("El email no puede estar vacío.");
            _pacienteDAL.AgregarEmail1N(idPaciente, email, esPrincipal);
            try
            {
                new DigitoVerificadorBLL().RecalcularYPersistir();
            }
            catch { }
        }

        public void EliminarEmailLogico1N(int idEmail)
        {
            if (idEmail <= 0) return;
            _pacienteDAL.EliminarEmailLogico1N(idEmail);
            try
            {
                new DigitoVerificadorBLL().RecalcularYPersistir();
            }
            catch { }
        }

        public DataTable ObtenerTelefonos1N(int idPaciente)
        {
            return _pacienteDAL.ObtenerTelefonos1N(idPaciente);
        }

        public void AgregarTelefono1N(int idPaciente, string numero, string tipo, bool esPrincipal)
        {
            if (string.IsNullOrWhiteSpace(numero)) throw new ArgumentException("El número de teléfono no puede estar vacío.");
            _pacienteDAL.AgregarTelefono1N(idPaciente, numero, tipo, esPrincipal);
            try
            {
                new DigitoVerificadorBLL().RecalcularYPersistir();
            }
            catch { }
        }

        public void EliminarTelefonoLogico1N(int idTelefono)
        {
            if (idTelefono <= 0) return;
            _pacienteDAL.EliminarTelefonoLogico1N(idTelefono);
            try
            {
                new DigitoVerificadorBLL().RecalcularYPersistir();
            }
            catch { }
        }

        public void ImportarClientesDesdeXml(List<(string dni, string apellidos, string nombres, string email, string celular, string direccion)> clientes)
        {
            if (clientes == null || clientes.Count == 0) return;
            _pacienteDAL.ImportarClientesDesdeXml(clientes);

            try
            {
                int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                _bitacoraBLL.RegistrarBitacora(2, $"Des-serialización XML: sincronizados {clientes.Count} clientes en la base de datos", dniActual, "MaestroClientes");
            }
            catch { }

            try
            {
                new DigitoVerificadorBLL().RecalcularYPersistir();
            }
            catch { }
        }
    }
}
