using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class JefeAreaBE : EmpleadoBE
    {
        public int cantidadSubordinados {  get; set; }
        public string nombreArea {  get; set; }
        public JefeAreaBE(string nombre, string apellido) : base(nombre, apellido)
        {

        }
    }
}
