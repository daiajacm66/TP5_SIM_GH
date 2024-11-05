using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4_SIM.Clases
{
    public class EnvioPaquete
    {
        private int cola;
        private string estado;

        public EnvioPaquete()
        {
            cola = 0;
            estado = "Libre";
        }
        public int Cola { get => cola; set => cola = value; }
        public string Estado { get => estado; set => estado = value; }
    }
}
