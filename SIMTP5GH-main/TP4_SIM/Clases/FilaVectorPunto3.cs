﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP4_SIM.Clases;
using TP4_SIM.Clases.Estadisticas;
using TP4_SIM.Clases.EventosFinAtencion;
using TP4_SIM.Clases.EventosLlegadas;

namespace TP4_SIM
{
    public class FilaVectorPunto3
    {
        int nroFIla;
        private string evento;
        private double reloj;
        private LlegadaEnvio llegadaEnvio;
        private LlegadaReclamo llegadaReclamo;
        private LlegadaVenta llegadaVenta;
        private LlegadaAE llegadaAE;
        private LlegadaPostales llegadaPostales;

        private FinAtencionEnvio fin_envio;
        private FinAtencionReclamo fin_reclamo;
        private FinAtencionVenta fin_venta;
        private FinAtencionEmpresarial fin_AE;
        private FinAtencionPostales fin_postales;
        private List<EnvioPaquete> envioPaquetes;
        private List<Reclamo> reclamos;
        private List<Venta> ventas;
        private List<AtencionEmpresarial> atencionEmp;
        private List<Postales> postales;

        private EstadisticaEnvio estadisticasEnvio;
        private EstadisticaReclamo estadisticasReclamo;
        private EstadisticaVenta estadisticasVenta;
        private EstadisticaAE estadisticasAE;
        private EstadisticaPostales estadisticasPostales;
        private List<Cliente> clientes;

        public FilaVectorPunto3()
        {
            this.evento = "";
            this.reloj = 0;
            this.LlegadaEnvio = new LlegadaEnvio();
            this.LlegadaReclamo = new LlegadaReclamo();
            this.LlegadaVenta = new LlegadaVenta();
            this.LlegadaAE = new LlegadaAE();
            this.LlegadaPostales = new LlegadaPostales();

            this.fin_envio = new FinAtencionEnvio();
            this.fin_reclamo = new FinAtencionReclamo(3);
            this.fin_venta = new FinAtencionVenta(2);
            this.fin_AE = new FinAtencionEmpresarial();
            this.fin_postales = new FinAtencionPostales();
            this.ventas = new List<Venta>
        {
            new Venta(),
            new Venta()
        };
            this.envioPaquetes = new List<EnvioPaquete>
        {
            new EnvioPaquete(),
            new EnvioPaquete(),
            new EnvioPaquete()
        };
            this.reclamos = new List<Reclamo>
        {
            new Reclamo(),
            new Reclamo(),
            new Reclamo()
        };
            this.atencionEmp = new List<AtencionEmpresarial>
        {
            new AtencionEmpresarial(),
            new AtencionEmpresarial()
        };
            this.postales = new List<Postales> { new Postales() };
            this.estadisticasEnvio = new EstadisticaEnvio();
            this.estadisticasReclamo = new EstadisticaReclamo();
            this.estadisticasPostales = new EstadisticaPostales();
            this.estadisticasVenta = new EstadisticaVenta();
            this.estadisticasAE = new EstadisticaAE();
            this.clientes = new List<Cliente>();




        }
        public int NroFila { get => nroFIla; set => nroFIla = value; }

        public string Evento { get => evento; set => evento = value; }
        public double Reloj { get => reloj; set => reloj = value; }

        public List<Cliente> Clientes { get => clientes; set => clientes = value; }
        public List<EnvioPaquete> EnvioPaquetes { get => envioPaquetes; set => envioPaquetes = value; }
        public List<Reclamo> Reclamos { get => reclamos; set => reclamos = value; }
        public List<Venta> Ventas { get => ventas; set => ventas = value; }
        public List<Postales> Postales { get => postales; set => postales = value; }
        public EstadisticaEnvio EstadisticasEnvio { get => estadisticasEnvio; set => estadisticasEnvio = value; }
        public EstadisticaReclamo EstadisticasReclamo { get => estadisticasReclamo; set => estadisticasReclamo = value; }
        public EstadisticaVenta EstadisticasVenta { get => estadisticasVenta; set => estadisticasVenta = value; }
        public EstadisticaAE EstadisticasAE { get => estadisticasAE; set => estadisticasAE = value; }
        public EstadisticaPostales EstadisticasPostales { get => estadisticasPostales; set => estadisticasPostales = value; }
        public FinAtencionEnvio Fin_envio { get => fin_envio; set => fin_envio = value; }
        public FinAtencionReclamo Fin_reclamo { get => fin_reclamo; set => fin_reclamo = value; }
        public FinAtencionVenta Fin_venta { get => fin_venta; set => fin_venta = value; }
        public FinAtencionEmpresarial Fin_AE { get => fin_AE; set => fin_AE = value; }
        public FinAtencionPostales Fin_postales { get => fin_postales; set => fin_postales = value; }
        public LlegadaEnvio LlegadaEnvio { get => llegadaEnvio; set => llegadaEnvio = value; }
        public LlegadaReclamo LlegadaReclamo { get => llegadaReclamo; set => llegadaReclamo = value; }
        public LlegadaVenta LlegadaVenta { get => llegadaVenta; set => llegadaVenta = value; }
        public LlegadaAE LlegadaAE { get => llegadaAE; set => llegadaAE = value; }
        public LlegadaPostales LlegadaPostales { get => llegadaPostales; set => llegadaPostales = value; }
        internal List<AtencionEmpresarial> AtencionEmp { get => atencionEmp; set => atencionEmp = value; }
    }
}
