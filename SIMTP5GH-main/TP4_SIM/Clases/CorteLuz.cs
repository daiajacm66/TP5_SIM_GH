using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4_SIM.Clases
{
    public class CorteLuz
    {
        private string estado;
        private double tiempo;
        private double horaInicio;
        private double horaFin;

        public CorteLuz()
        {
        }

        public string Estado { get => estado; set => estado = value; }
        public double HoraInicioEspera { get => horaInicio; set => horaInicio = value; }
        public double HoraInicioAtencion { get => horaFin; set => horaFin = value; }
        public double Tiempo { get => tiempo; set => tiempo = value; }
    }
}
