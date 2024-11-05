using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4_SIM.Clases
{
    public class Postales
    {

        private int cola;
        private string estado;
        private double tiempo_Remanente;

        public Postales()
        {
            cola = 0;
            estado = "Libre";
            tiempo_Remanente = 0;
        }

        public int Cola { get => cola; set => cola = value; }
        public string Estado { get => estado; set => estado = value; }
        public double Tiempo_Remanente { get => tiempo_Remanente;set => tiempo_Remanente = value; }
    }
}
