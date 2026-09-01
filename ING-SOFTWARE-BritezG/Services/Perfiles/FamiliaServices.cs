using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Perfiles
{
    public class FamiliaServices : Perfil
    {
        private List<Perfil> _hijos = new();
        public IReadOnlyList<Perfil> Hijos => _hijos.AsReadOnly();

        public FamiliaServices(string nombre) : base(nombre)
        {

        }

        public FamiliaServices() 
        {

        }
        public void Agregar(Perfil c) => _hijos.Add(c);

        public override bool EsCompuesto() => true;

    }
}
