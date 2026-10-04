using BE;
using System;
using System.Collections.Generic;
using DAL;

namespace BLL
{
    public class BitacoraBLL
    {
        private BitacoraDAL _bitacoraDAL;

        public BitacoraBLL()
        {
            _bitacoraDAL = new BitacoraDAL();
        }

        public List<BitacoraBE> BuscarBitacora(DateTime desde, DateTime hasta)
        {
            try
            {
                return _bitacoraDAL.FiltrarBitacora(desde, hasta);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar en bitácora: {ex.Message}");
                throw;
            }
        }

        public List<BitacoraBE> BuscarEventos(DateTime desde, DateTime hasta) => BuscarBitacora(desde, hasta);

        public List<BitacoraBE> VerBitacora()
        {
            try
            {
                return _bitacoraDAL.ObtenerBitacora();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener bitácora: {ex.Message}");
                throw;
            }
        }

        public List<BitacoraBE> VerEventos() => VerBitacora();

        public bool RegistrarBitacora(int criticidad, string descripcion, int dni, string modulo)
        {
            try
            {
                BitacoraBE bitacora = new BitacoraBE(criticidad, descripcion, dni, DateTime.Now, modulo);
                _bitacoraDAL.GuardarBitacora(bitacora);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar en bitácora: {ex.Message}");
                return false;
            }
        }

        public bool RegistrarEvento(int criticidad, string descripcion, int dni, string modulo) => RegistrarBitacora(criticidad, descripcion, dni, modulo);
    }

    [Obsolete("Usar BitacoraBLL")]
    public class EventoBLL : BitacoraBLL
    {
    }
}
