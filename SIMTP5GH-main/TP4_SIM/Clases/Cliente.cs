using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4_SIM
{
    public class Cliente
    {
        private string estado;
        private double horaInicioEspera;
        private double horaInicioAtencion;

        public Cliente()
        {
        }

        public string Estado { get => estado; set => estado = value; }
        public double HoraInicioEspera { get => horaInicioEspera; set => horaInicioEspera = value; }
        public double HoraInicioAtencion { get => horaInicioAtencion; set => horaInicioAtencion = value; }
    }
}
