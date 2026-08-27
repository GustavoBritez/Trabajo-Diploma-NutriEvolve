using BE;
using System;
using System.Collections.Generic;
using DAL;

namespace BLL
{
    public class EventoBLL
    {
        private EventoDAL _EventoDAL;

        public EventoBLL()
        {
            _EventoDAL = new EventoDAL();
        }

        public List<EventoBE> BuscarEventos(DateTime desde, DateTime hasta)
        {
            try
            {
                return _EventoDAL.FiltrarBitacora(desde, hasta);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar eventos: {ex.Message}");
                throw;
            }
        }

        public List<EventoBE> VerEventos()
        {
            try
            {
                return _EventoDAL.ObtenerBitacora();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener eventos: {ex.Message}");
                throw;
            }
        }
        public bool RegistrarEvento(int criticidad, string descripcion, int dni, string modulo)
        {
            try
            {
                EventoBE evento = new EventoBE(criticidad, descripcion, dni, DateTime.Now, modulo);
                _EventoDAL.GuardarBitacora(evento);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar evento en bitácora: {ex.Message}");
                return false;
            }
        }
    }
}