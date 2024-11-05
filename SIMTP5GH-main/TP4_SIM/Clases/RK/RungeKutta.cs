using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4_SIM.Clases.RK
{
    public class RungeKutta
    {

        public static TablaRK IntegracionNumerica(double H, double Xo, double Yo, double Yf)
        {
            TablaRK tabla = new TablaRK();
            FilaRK filaActual = new FilaRK();
            int contador = 0;
            do
            {
                if (contador == 0)
                {
                    filaActual.xi = Xo;
                    filaActual.yi = Yo;
                    contador++;
                }
                else
                {
                    filaActual.xi = filaActual.nextXi;
                    filaActual.yi = filaActual.nextYi;
                }

                filaActual.k1 = CalcularK1(filaActual.xi, filaActual.yi);
                filaActual.k2 = CalcularK23(filaActual.xi, filaActual.yi, H, filaActual.k1);
                filaActual.k3 = CalcularK23(filaActual.xi, filaActual.yi, H, filaActual.k2);
                filaActual.k4 = CalcularK4(filaActual.xi, filaActual.yi, H, filaActual.k3);

                filaActual.nextXi = CalcularSiguienteX(filaActual.xi, H);
                filaActual.nextYi = CalcularSiguienteY(filaActual.yi, H, filaActual.k1, filaActual.k2, filaActual.k3, filaActual.k4);

                tabla.AgregarFila(filaActual);

                if (filaActual.yi < 0)
                {
                    tabla.ResultadoRK = filaActual.xi;
                    return tabla;
                }

            }
            while (filaActual.yi > 0);
            tabla.ResultadoRK = filaActual.xi;
            return tabla;
        }


        private static double CalcularK1(double x, double y)
        {
            double k1 = CalcularK(x, y);

            return k1;
        }

        private static double CalcularK(double x, double y)
        {
            double k = (0.025 * x - 0.5 * y - 12.85);
            return k;

        }
        private static Tuple<double, double> CalcularPuntosMedios(double x, double y, double h, double k)
        {

            double xMidPoint = x + (h / 2);
            double yMidPoint = y + (h / 2 * k);

            Tuple<double, double> midPoints = new Tuple<double, double>(xMidPoint, yMidPoint);

            return midPoints;
        }

        private static double CalcularSiguienteX(double x, double h)
        {

            double x1plus1 = x + h;
            return x1plus1;
        }

        private static double CalcularSiguienteY(double y, double h, double k1, double k2, double k3, double k4)
        {
            double y1plus1 = y + ((h / 6) * (k1 + 2 * k2 + 2 * k3 + k4));
            return y1plus1;
        }

        private static double CalcularK23(double x, double y, double h, double k)
        {
            Tuple<double, double> midPoints = CalcularPuntosMedios(x, y, h, k);

            double kx = CalcularK(midPoints.Item1, midPoints.Item2);

            return kx;

        }

        private static double CalcularK4(double x, double y, double h, double k3)
        {
            double x1plus1 = CalcularSiguienteX(x, h);
            double yplushk3 = y + (h * k3);
            double k4 = CalcularK(x1plus1, yplushk3);
            return k4;
        }
    }
}
