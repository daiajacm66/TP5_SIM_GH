using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4_SIM.Clases.EventosFinAtencion
{
    public class FinAtencionReclamo
    {
        private double rnd;
        private double tiempoAtencion;
        private double[] finAtencion;
        public FinAtencionReclamo()
        {
            finAtencion = new double[2];
        }

        public FinAtencionReclamo(int nroEmpleados)
        {
            finAtencion = new double[nroEmpleados];
        }

        public double Rnd { get => rnd; set => rnd = value; }
        public double TiempoAtencion { get => tiempoAtencion; set => tiempoAtencion = value; }
        public double[] FinAtencion { get => finAtencion; set => finAtencion = value; }
    }
}
