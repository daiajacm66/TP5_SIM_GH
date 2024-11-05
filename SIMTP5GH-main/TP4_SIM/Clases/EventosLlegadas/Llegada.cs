using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4_SIM.Clases.EventosLlegadas
{
    public class Llegada
    {
        private double rnd;
        private double tiempoEntreLlegadas;
        private double proxLlegada;

        public double Rnd { get => rnd; set => rnd = value; }
        public double TiempoEntreLlegadas { get => tiempoEntreLlegadas; set => tiempoEntreLlegadas = value; }
        public double ProxLlegada { get => proxLlegada; set => proxLlegada = value; }
    }
}
