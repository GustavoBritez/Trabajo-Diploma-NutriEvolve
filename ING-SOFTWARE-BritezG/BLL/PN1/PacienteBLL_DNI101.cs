using BE;
using DAL;
using Services;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class PacienteBLL_DNI101
    {
        private readonly PacienteDAL_DNI101 _pacienteDAL = new();
        private readonly EventoBLL _bitacoraBLL = new();

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
            var pacienteExistente = _pacienteDAL.ObtenerPacientePorDNI(dniNiño);
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
                int idGenerado = _pacienteDAL.Guardar(nuevoPaciente);

                try
                {
                    int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                    _bitacoraBLL.RegistrarEvento(1, $"Registro de Paciente Pediátrico: {nuevoPaciente.NombreCompleto} (DNI: {nuevoPaciente.DNINiño_DNI101})", dniActual, "TurneroNutricional");
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
                    _bitacoraBLL.RegistrarEvento(3, $"Error al registrar paciente: {ex.Message}", dniActual, "TurneroNutricional");
                }
                catch { }
                throw;
            }
        }

        public PacienteBE_DNI101? ObtenerPacientePorDNI(string dniNiño)
        {
            if (string.IsNullOrWhiteSpace(dniNiño)) return null;
            return _pacienteDAL.ObtenerPacientePorDNI(dniNiño.Trim());
        }

        public PacienteBE_DNI101? ObtenerPorId(int idPaciente)
        {
            return _pacienteDAL.ObtenerPorId(idPaciente);
        }

        public List<PacienteBE_DNI101> ListarPacientes()
        {
            return _pacienteDAL.ListarTodos();
        }

        public bool ModificarPaciente(PacienteBE_DNI101 paciente)
        {
            if (paciente == null || paciente.IdPaciente_DNI101 <= 0) return false;
            try
            {
                bool ok = _pacienteDAL.Modificar(paciente);
                if (ok)
                {
                    try
                    {
                        int dniActual = ServicesSessionManager.Instancia.ObtenerDniUsuarioActual();
                        _bitacoraBLL.RegistrarEvento(2, $"Modificación de datos del Paciente: {paciente.NombreCompleto} (DNI: {paciente.DNINiño_DNI101})", dniActual, "TurneroNutricional");
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
                    _bitacoraBLL.RegistrarEvento(1, $"Error al modificar datos del Paciente (DNI: {paciente.DNINiño_DNI101}): {ex.Message}", dniActual, "TurneroNutricional");
                }
                catch { }
                throw;
            }
        }
    }
}
