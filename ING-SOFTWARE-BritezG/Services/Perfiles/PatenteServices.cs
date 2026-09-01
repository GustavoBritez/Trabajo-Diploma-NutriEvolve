using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Perfiles
{
    public class PatenteServices : Perfil
    {
        public PatenteServices(string nombre) : base(nombre)
        {
        }

        public override bool EsCompuesto() => false;
    }
}
