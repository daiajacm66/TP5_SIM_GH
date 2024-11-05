using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4_SIM.Clases.RK
{
    public class FilaRK
    {
        public double xi { get; set; }
        public double yi { get; set; }
        public double k1 { get; set; }
        public double k2 { get; set; }
        public double k3 { get; set; }
        public double k4 { get; set; }
        public double nextXi { get; set; }
        public double nextYi { get; set; }

        public FilaRK()
        {

        }
        public FilaRK(FilaRK filaRunge)
        {
            this.xi = filaRunge.xi;
            this.yi = filaRunge.yi;
            this.k1 = filaRunge.k1;
            this.k2 = filaRunge.k2;
            this.k3 = filaRunge.k3;
            this.k4 = filaRunge.k4;
            this.nextXi = filaRunge.nextXi;
            this.nextYi = filaRunge.nextYi;

        }
        public string[] ListaString()
        {
            string[] row = {
                    xi.ToString(),
                    yi.ToString(),
                    k1.ToString(),
                    k2.ToString(),
                    k3.ToString(),
                    k4.ToString(),
                    nextXi.ToString(),
                    nextYi.ToString()
                            };
            return row;
        }
        public string ToCsvString()
        {
            // Convertir cada propiedad a una cadena y unirlas con comas
            return $"{xi},{yi},{k1},{k2},{k3},{k4},{nextXi},{nextYi}";
        }

    }
}
