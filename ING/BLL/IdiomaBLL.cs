using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DAL;
using System.Windows.Forms;
namespace BLL
{
    public class IdiomaBLL
    {
        private IdiomaDAL idiomaDAL= new IdiomaDAL();
        public List<Idioma> ObtenerIdiomas()=> idiomaDAL.ObtenerIdiomas();
        public string Traducir(string clave)=>idiomaDAL.Traducir(clave);




        public DialogResult MostrarMensaje( string claveMensaje,string claveTitulo, MessageBoxButtons botones = MessageBoxButtons.OK,
            MessageBoxIcon icono = MessageBoxIcon.None, params object[] parametros)
        {
            string mensaje = Traducir(claveMensaje);
            string titulo = Traducir(claveTitulo);

            if (parametros.Length > 0)
                mensaje = string.Format(mensaje, parametros);

            return MessageBox.Show(mensaje, titulo, botones, icono);
        }


    }
}
