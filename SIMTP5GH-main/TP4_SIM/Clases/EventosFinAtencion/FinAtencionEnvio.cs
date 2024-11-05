using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4_SIM.Clases.EventosFinAtencion
{
    public class FinAtencionEnvio
    {

        private double rnd;
        private double tiempoAtencion;
        private double[] finAtencion;

        public FinAtencionEnvio()
        {
            finAtencion = new double[3];
        }

        public double Rnd { get => rnd; set => rnd = value; }
        public double TiempoAtencion { get => tiempoAtencion; set => tiempoAtencion = value; }
        public double[] FinAtencion { get => finAtencion; set => finAtencion = value; }
    }
}
