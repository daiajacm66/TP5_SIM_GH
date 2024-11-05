using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4_SIM.Clases.RK
{
    public class GestorColasRK
    {
     
        public double T { get; set; }
        public List<TablaRK> TablasRK { get; set; }

        public GestorColasRK() 
        {
            TablasRK = new List<TablaRK>();
        }

        public GestorColasRK(double T, List<TablaRK> TablasRK)
        {
            this.T = T;
            this.TablasRK = TablasRK;
        }

    }
}
