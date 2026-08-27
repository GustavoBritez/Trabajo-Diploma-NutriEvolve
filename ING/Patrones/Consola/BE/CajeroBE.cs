using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class CajeroBE : EmpleadoBE
    {
        public CajeroBE(string nombre, string apellido) : base( nombre, apellido)
        {
            this.nombre = nombre;
            this.apellido = apellido;
        }

        public int registroPedidos {  get; set; }
        public float facturacion {  get; set; }

    }
}
