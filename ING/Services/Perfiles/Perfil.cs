using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Perfiles
{
    public abstract class Perfil
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        protected Perfil(string nombre)
        {
            this.Nombre = nombre;
        }

        protected Perfil()
        {
        }

        public abstract bool EsCompuesto();
    }
}
