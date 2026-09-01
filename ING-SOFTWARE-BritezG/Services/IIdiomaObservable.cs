using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IIdiomaObservable
    {
        void Suscribir(IIdiomaObserver observer);
        void Desuscribir(IIdiomaObserver observer);
        void Notificar();
    }
}
