using System;
using System.Collections.Generic;
using BE.PN2;
using DAL.PN2;

namespace BLL.PN2
{
    public class BitacoraCambiosConsultaBLL_DNI101
    {
        private readonly BitacoraCambiosConsultaDAL_DNI101 _dal;

        public BitacoraCambiosConsultaBLL_DNI101()
        {
            _dal = new BitacoraCambiosConsultaDAL_DNI101();
        }

        public List<ConsultaCambioBE_DNI101> ListarCambios(int? idConsulta = null)
        {
            return _dal.ListarCambios(idConsulta);
        }

        public bool RevertirCambio(int idCambio, int usuarioDni)
        {
            if (idCambio <= 0) throw new ArgumentException("Identificador de cambio inválido.");
            bool ok = _dal.RevertirCambio(idCambio, usuarioDni);
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

        public List<PlanAlimentarioCambioBE_DNI101> ListarCambiosPlanes(int? idPlan = null)
        {
            return _dal.ListarCambiosPlanes(idPlan);
        }

        public bool RevertirCambioPlan(int idCambio, int usuarioDni)
        {
            if (idCambio <= 0) throw new ArgumentException("Identificador de cambio inválido.");
            bool ok = _dal.RevertirCambioPlan(idCambio, usuarioDni);
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

        public List<BE.PN1.PacienteCambioBE_DNI101> ListarCambiosPacientes(int? idPaciente = null)
        {
            return _dal.ListarCambiosPacientes(idPaciente);
        }

        public bool RevertirCambioPaciente(int idCambio, int usuarioDni)
        {
            if (idCambio <= 0) throw new ArgumentException("Identificador de cambio inválido.");
            bool ok = _dal.RevertirCambioPaciente(idCambio, usuarioDni);
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
    }
}
