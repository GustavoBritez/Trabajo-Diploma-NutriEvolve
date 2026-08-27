using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class EmpleadoBE
    {
        public EmpleadoBE(string nombre, string apellido)
        {
            this.nombre = nombre;
            this.apellido = apellido;
        }

        public string nombre {  get; set; }
        public string apellido { get; set; }    
    }
}
