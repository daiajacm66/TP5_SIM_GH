using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP4_SIM
{
    public partial class SimPunto3 : Form
    {
        private int cantFilas;
        double mediaLlegadaPaquete;
        double mediaFinPaquete;
        double mediaLlegadaReclamo;
        double mediaFinReclamo;
        double mediaLlegadaVenta;
        double mediaFinVenta;
        double mediaLlegadaAE;
        double mediaFinAE;
        double mediaLlegadaPostales;
        double mediaFinPostales;
        private int nroPrimeraFilaMostrar;
        private int nroUltimaFilaMostrar;
        public SimPunto3(int cant, double[] medias, int nroPrimeraFila, int nroUltimaFila)
        {
            InitializeComponent(); this.cantFilas = cant;
            // Valores de media (dist. exponencial) de los eventos 
            this.mediaLlegadaPaquete = medias[0];
            this.mediaFinPaquete = medias[1];
            this.mediaLlegadaReclamo = medias[2];
            this.mediaFinReclamo = medias[3];
            this.mediaLlegadaVenta = medias[4];
            this.mediaFinVenta = medias[5];
            this.mediaLlegadaAE = medias[6];
            this.mediaFinAE = medias[7];
            this.mediaLlegadaPostales = medias[8];
            this.mediaFinPostales = medias[9];

            // Nro de fila a partir de la que sea desea visualizar
            this.nroPrimeraFilaMostrar = nroPrimeraFila;
            this.nroUltimaFilaMostrar = nroUltimaFila;

        }

        private void simular3(object sender, EventArgs e)
        {
            string[] nombresEventos = {"llegada_envio", "llegada_reclamo", "llegada_venta", "llegada_AE", "llegada_postales",
                "fin_envio1","fin_envio2", "fin_envio3", "fin_reclamo1", "fin_reclamo2", "fin_reclamo3", "fin_venta1", "fin_venta2",
                "fin_AE1", "fin_AE2", "fin_postales"};


            Random random = new Random();

            // Se completa la primera fila de inicialización 
            FilaVectorPunto3 fila1 = new FilaVectorPunto3();
            fila1.NroFila = 1;
            fila1.Evento = "inicializacion";
            fila1.Reloj = 0;

            fila1.LlegadaEnvio.Rnd = generarRandom(random);
            fila1.LlegadaEnvio.TiempoEntreLlegadas = calcularTiempo(mediaLlegadaPaquete, fila1.LlegadaEnvio.Rnd);
            fila1.LlegadaEnvio.ProxLlegada = fila1.Reloj + fila1.LlegadaEnvio.TiempoEntreLlegadas;

            fila1.LlegadaReclamo.Rnd = generarRandom(random);
            fila1.LlegadaReclamo.TiempoEntreLlegadas = calcularTiempo(mediaLlegadaReclamo, fila1.LlegadaReclamo.Rnd);
            fila1.LlegadaReclamo.ProxLlegada = fila1.Reloj + fila1.LlegadaReclamo.TiempoEntreLlegadas;

            fila1.LlegadaVenta.Rnd = generarRandom(random);
            fila1.LlegadaVenta.TiempoEntreLlegadas = calcularTiempo(mediaLlegadaVenta, fila1.LlegadaVenta.Rnd);
            fila1.LlegadaVenta.ProxLlegada = fila1.Reloj + fila1.LlegadaVenta.TiempoEntreLlegadas;

            fila1.LlegadaAE.Rnd = generarRandom(random);
            fila1.LlegadaAE.TiempoEntreLlegadas = calcularTiempo(mediaLlegadaAE, fila1.LlegadaAE.Rnd);
            fila1.LlegadaAE.ProxLlegada = fila1.Reloj + fila1.LlegadaAE.TiempoEntreLlegadas;

            fila1.LlegadaPostales.Rnd = generarRandom(random);
            fila1.LlegadaPostales.TiempoEntreLlegadas = calcularTiempo(mediaLlegadaPostales, fila1.LlegadaPostales.Rnd);
            fila1.LlegadaPostales.ProxLlegada = fila1.Reloj + fila1.LlegadaPostales.TiempoEntreLlegadas;


            // Se ponen todos los tiempos de los prox eventos en una lista para madarlos al método que busca al siguiente
            List<double> posiblesProxEventos = new List<double>();
            posiblesProxEventos.Add(fila1.LlegadaEnvio.ProxLlegada);
            posiblesProxEventos.Add(fila1.LlegadaReclamo.ProxLlegada);
            posiblesProxEventos.Add(fila1.LlegadaVenta.ProxLlegada);
            posiblesProxEventos.Add(fila1.LlegadaAE.ProxLlegada);
            posiblesProxEventos.Add(fila1.LlegadaPostales.ProxLlegada);

            List<double> proxEvento = buscarProxEvento(posiblesProxEventos);

            FilaVectorPunto3 fila2 = new FilaVectorPunto3();
            fila2.Reloj = (double)proxEvento[0];
            fila2.Evento = nombresEventos[Convert.ToInt32(proxEvento[1])];

            List<FilaVectorPunto3> filasMostrar = new List<FilaVectorPunto3>();

            DataTable tablaResultado = new DataTable();
            agregarColumnasTabla(tablaResultado);

            // For que genera las filas
            for (int i = 0; i < cantFilas; i++)
            {
                fila2.NroFila = i + 2; //tiene que empezar desde el 2
                //filasMostrar.Add(fila1);

                if ((i + 1) >= nroPrimeraFilaMostrar && (i + 1) <= nroUltimaFilaMostrar)
                {
                    // Agregar cada atributo individualmente de fila1
                    List<string> listaAux1 = new List<string>();

                    // Agregar cada atributo individualmente de fila1
                    listaAux1.Add(fila1.Evento.ToString());
                    listaAux1.Add(fila1.Reloj.ToString());
                    listaAux1.Add(fila1.LlegadaEnvio.Rnd.ToString());
                    listaAux1.Add(fila1.LlegadaEnvio.TiempoEntreLlegadas.ToString());
                    listaAux1.Add(fila1.LlegadaEnvio.ProxLlegada.ToString());
                    listaAux1.Add(fila1.LlegadaReclamo.Rnd.ToString());
                    listaAux1.Add(fila1.LlegadaReclamo.TiempoEntreLlegadas.ToString());
                    listaAux1.Add(fila1.LlegadaReclamo.ProxLlegada.ToString());
                    listaAux1.Add(fila1.LlegadaVenta.Rnd.ToString());
                    listaAux1.Add(fila1.LlegadaVenta.TiempoEntreLlegadas.ToString());
                    listaAux1.Add(fila1.LlegadaVenta.ProxLlegada.ToString());
                    listaAux1.Add(fila1.LlegadaAE.Rnd.ToString());
                    listaAux1.Add(fila1.LlegadaAE.TiempoEntreLlegadas.ToString());
                    listaAux1.Add(fila1.LlegadaAE.ProxLlegada.ToString());
                    listaAux1.Add(fila1.LlegadaPostales.Rnd.ToString());
                    listaAux1.Add(fila1.LlegadaPostales.TiempoEntreLlegadas.ToString());
                    listaAux1.Add(fila1.LlegadaPostales.ProxLlegada.ToString());
                    listaAux1.Add(fila1.Fin_envio.Rnd.ToString());
                    listaAux1.Add(fila1.Fin_envio.TiempoAtencion.ToString());
                    listaAux1.Add(fila1.Fin_envio.FinAtencion[0].ToString());
                    listaAux1.Add(fila1.Fin_envio.FinAtencion[1].ToString());
                    listaAux1.Add(fila1.Fin_envio.FinAtencion[2].ToString());
                    listaAux1.Add(fila1.Fin_reclamo.Rnd.ToString());
                    listaAux1.Add(fila1.Fin_reclamo.TiempoAtencion.ToString());
                    listaAux1.Add(fila1.Fin_reclamo.FinAtencion[0].ToString());
                    listaAux1.Add(fila1.Fin_reclamo.FinAtencion[1].ToString());
                    listaAux1.Add(fila1.Fin_reclamo.FinAtencion[2].ToString());


                    listaAux1.Add(fila1.Fin_venta.Rnd.ToString());
                    listaAux1.Add(fila1.Fin_venta.TiempoAtencion.ToString());
                    listaAux1.Add(fila1.Fin_venta.FinAtencion[0].ToString());
                    listaAux1.Add(fila1.Fin_venta.FinAtencion[1].ToString());
                    listaAux1.Add(fila1.Fin_AE.Rnd.ToString());
                    listaAux1.Add(fila1.Fin_AE.TiempoAtencion.ToString());
                    listaAux1.Add(fila1.Fin_AE.FinAtencion[0].ToString());
                    listaAux1.Add(fila1.Fin_AE.FinAtencion[1].ToString());
                    listaAux1.Add(fila1.Fin_postales.Rnd.ToString());
                    listaAux1.Add(fila1.Fin_postales.TiempoAtencion.ToString());
                    listaAux1.Add(fila1.Fin_postales.FinAtencion.ToString());
                    listaAux1.Add(fila1.EnvioPaquetes[0].Cola.ToString());
                    listaAux1.Add(fila1.EnvioPaquetes[0].Estado.ToString());
                    listaAux1.Add(fila1.EnvioPaquetes[1].Cola.ToString());
                    listaAux1.Add(fila1.EnvioPaquetes[1].Estado.ToString());
                    listaAux1.Add(fila1.EnvioPaquetes[2].Cola.ToString());
                    listaAux1.Add(fila1.EnvioPaquetes[2].Estado.ToString());
                    listaAux1.Add(fila1.Reclamos[0].Cola.ToString());
                    listaAux1.Add(fila1.Reclamos[0].Estado.ToString());
                    listaAux1.Add(fila1.Reclamos[1].Cola.ToString());
                    listaAux1.Add(fila1.Reclamos[1].Estado.ToString());

                    listaAux1.Add(fila1.Reclamos[2].Cola.ToString());
                    listaAux1.Add(fila1.Reclamos[2].Estado.ToString());

                    listaAux1.Add(fila1.Ventas[0].Cola.ToString());
                    listaAux1.Add(fila1.Ventas[0].Estado.ToString());
                    listaAux1.Add(fila1.Ventas[1].Cola.ToString());
                    listaAux1.Add(fila1.Ventas[1].Estado.ToString());

                    listaAux1.Add(fila1.AtencionEmp[0].Cola.ToString());
                    listaAux1.Add(fila1.AtencionEmp[0].Estado.ToString());
                    listaAux1.Add(fila1.AtencionEmp[1].Cola.ToString());
                    listaAux1.Add(fila1.AtencionEmp[1].Estado.ToString());
                    listaAux1.Add(fila1.Postales[0].Cola.ToString());
                    listaAux1.Add(fila1.Postales[0].Estado.ToString());
                    listaAux1.Add(fila1.EstadisticasEnvio.AcumuladorEspera.ToString());
                    listaAux1.Add(fila1.EstadisticasEnvio.AcumuladorOcupacion.ToString());
                    listaAux1.Add(fila1.EstadisticasEnvio.CantClientesAtendidos.ToString());
                    listaAux1.Add(fila1.EstadisticasReclamo.AcumuladorEspera.ToString());
                    listaAux1.Add(fila1.EstadisticasReclamo.AcumuladorOcupacion.ToString());
                    listaAux1.Add(fila1.EstadisticasReclamo.CantClientesAtendidos.ToString());
                    listaAux1.Add(fila1.EstadisticasVenta.AcumuladorEspera.ToString());
                    listaAux1.Add(fila1.EstadisticasVenta.AcumuladorOcupacion.ToString());
                    listaAux1.Add(fila1.EstadisticasVenta.CantClientesAtendidos.ToString());
                    listaAux1.Add(fila1.EstadisticasAE.AcumuladorEspera.ToString());
                    listaAux1.Add(fila1.EstadisticasAE.AcumuladorOcupacion.ToString());
                    listaAux1.Add(fila1.EstadisticasAE.CantClientesAtendidos.ToString());
                    listaAux1.Add(fila1.EstadisticasPostales.AcumuladorEspera.ToString());
                    listaAux1.Add(fila1.EstadisticasPostales.AcumuladorOcupacion.ToString());
                    listaAux1.Add(fila1.EstadisticasPostales.CantClientesAtendidos.ToString());
                    foreach (Cliente Cli in fila1.Clientes)
                    {
                        listaAux1.Add(Cli.Estado);
                        listaAux1.Add(Cli.HoraInicioEspera.ToString());
                        listaAux1.Add(Cli.HoraInicioAtencion.ToString());
                    }
                    tablaResultado.Rows.Add(listaAux1.ToArray());

                }

                // ----------------------------------------- LLEGADAS -----------------------------------------------------------------------------
                //LLEGADA ENVIO
                if (fila2.Evento == "llegada_envio")
                {
                    copiarProximasLlegadas(fila1, fila2);
                    // Genero la prox. llegada_envio
                    fila2.LlegadaEnvio.Rnd = generarRandom(random);
                    fila2.LlegadaEnvio.TiempoEntreLlegadas = calcularTiempo(mediaLlegadaPaquete, fila2.LlegadaEnvio.Rnd);
                    fila2.LlegadaEnvio.ProxLlegada = fila2.Reloj + fila2.LlegadaEnvio.TiempoEntreLlegadas;

                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);

                    Cliente cliente = new Cliente();   //Al ser una llegada tengo que crear al cliente

                    verificarLlegadaEnvio(fila1, fila2, random, cliente);



                    // Como es una llegada tengo que agregar el cliente
                    fila2.Clientes.Add(cliente);
                    agregarClienteTabla(fila2, tablaResultado, cliente);
                }

                // LLEGADA RECLAMO
                if (fila2.Evento == "llegada_reclamo")
                {
                    copiarProximasLlegadas(fila1, fila2);

                    // Genero la prox. llegada_venta
                    fila2.LlegadaReclamo.Rnd = generarRandom(random);
                    fila2.LlegadaReclamo.TiempoEntreLlegadas = calcularTiempo(mediaLlegadaReclamo, fila2.LlegadaReclamo.Rnd);
                    fila2.LlegadaReclamo.ProxLlegada = fila2.Reloj + fila2.LlegadaReclamo.TiempoEntreLlegadas;

                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);

                    // Tengo que revisar las colas y los estados de VENTAS con el for
                    Cliente cliente = new Cliente();
                    bool empleadoLibre = false;
                    for (int j = 0; j < fila1.Reclamos.Count; j++)
                    {
                        if (fila1.Reclamos[j].Estado == "Libre")
                        {
                            // Si es atendido se cambiam los estado correspondientes, y se genera el fin de atención
                            cliente.Estado = "SR" + (j + 1);
                            cliente.HoraInicioAtencion = fila2.Reloj;
                            fila2.Reclamos[j].Estado = "Ocupado";

                            fila2.Fin_reclamo.Rnd = generarRandom(random);
                            fila2.Fin_reclamo.TiempoAtencion = calcularTiempo(mediaFinReclamo, fila2.Fin_reclamo.Rnd);
                            fila2.Fin_reclamo.FinAtencion[j] = fila2.Reloj + fila2.Fin_reclamo.TiempoAtencion;
                            empleadoLibre = true;
                            break;
                        }
                    }

                    if (!empleadoLibre)
                    {
                        cliente.HoraInicioEspera = fila2.Reloj;
                        if (fila1.Reclamos[0].Cola == fila1.Reclamos[1].Cola && fila1.Reclamos[0].Cola == fila1.Reclamos[2].Cola)
                        {
                            // Si todos los objetos tienen la misma cola, espera en el objeto 1:
                            cliente.Estado = "ER1";
                            fila2.Reclamos[0].Cola = fila1.Reclamos[0].Cola + 1;
                        }

                        else if (fila1.Reclamos[0].Cola < fila1.Reclamos[1].Cola && fila1.Reclamos[0].Cola < fila1.Reclamos[2].Cola)
                        {
                            cliente.Estado = "ER1";
                            fila2.Reclamos[0].Cola = fila1.Reclamos[0].Cola + 1;
                        }
                        else if (fila1.Reclamos[1].Cola < fila1.Reclamos[0].Cola && fila1.Reclamos[1].Cola < fila1.Reclamos[2].Cola)
                        {
                            cliente.Estado = "ER2";
                            fila2.Reclamos[1].Cola = fila1.Reclamos[1].Cola + 1;
                        }
                        else if (fila1.Reclamos[2].Cola < fila1.Reclamos[0].Cola && fila1.Reclamos[2].Cola < fila1.Reclamos[1].Cola)
                        {
                            cliente.Estado = "ER3";
                            fila2.Reclamos[2].Cola = fila1.Reclamos[2].Cola + 1;
                        }
                        // Si dos colas son iguales y una diferente
                        else if (fila1.Reclamos[0].Cola == fila1.Reclamos[1].Cola && fila1.Reclamos[0].Cola < fila1.Reclamos[2].Cola)
                        {
                            // Las colas 1 y 2 son iguales y menores que la 3
                            cliente.Estado = "ER1";
                            fila2.Reclamos[0].Cola = fila1.Reclamos[0].Cola + 1;
                        }
                        else if (fila1.Reclamos[0].Cola == fila1.Reclamos[1].Cola && fila1.Reclamos[0].Cola > fila1.Reclamos[2].Cola)
                        {
                            // Las colas 1 y 2 son iguales y mayores que la 3
                            cliente.Estado = "ER3";
                            fila2.Reclamos[2].Cola = fila1.Reclamos[2].Cola + 1;
                        }

                        else if (fila1.Reclamos[0].Cola == fila1.Reclamos[2].Cola && fila1.Reclamos[0].Cola < fila1.Reclamos[1].Cola)
                        {
                            // Las colas  1 y 3 son iguales y menores que la 2
                            cliente.Estado = "ER1";
                            fila2.Reclamos[0].Cola = fila1.Reclamos[0].Cola + 1;
                        }
                        else if (fila1.Reclamos[0].Cola == fila1.Reclamos[2].Cola && fila1.Reclamos[0].Cola > fila1.Reclamos[1].Cola)
                        {
                            // Las colas  1 y 3 son iguales y mayores que la 2
                            cliente.Estado = "ER2";
                            fila2.Reclamos[1].Cola = fila1.Reclamos[1].Cola + 1;
                        }
                        else if (fila1.Reclamos[1].Cola == fila1.Reclamos[2].Cola && fila1.Reclamos[1].Cola < fila1.Reclamos[0].Cola)
                        {
                            // Las colas 2 y 3 son iguales y menores que la 1
                            cliente.Estado = "ER2";
                            fila2.Reclamos[1].Cola = fila1.Reclamos[1].Cola + 1;
                        }
                        else if (fila1.Reclamos[1].Cola == fila1.Reclamos[2].Cola && fila1.Reclamos[1].Cola > fila1.Reclamos[0].Cola)
                        {
                            // Las colas 2 y 3 son iguales y mayores que la 1
                            cliente.Estado = "ER1";
                            fila2.Reclamos[0].Cola = fila1.Reclamos[0].Cola + 1;
                        }

                    }

                    // Como es una llegada tengo que agregar el cliente
                    fila2.Clientes.Add(cliente);
                    agregarClienteTabla(fila2, tablaResultado, cliente);

                }

                // LLEGADA VENTA
                if (fila2.Evento == "llegada_venta")
                {
                    copiarProximasLlegadas(fila1, fila2);

                    // Genero la prox. llegada_reclamo
                    fila2.LlegadaVenta.Rnd = generarRandom(random);
                    fila2.LlegadaVenta.TiempoEntreLlegadas = calcularTiempo(mediaLlegadaVenta, fila2.LlegadaVenta.Rnd);
                    fila2.LlegadaVenta.ProxLlegada = fila2.Reloj + fila2.LlegadaVenta.TiempoEntreLlegadas;

                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);

                    // Tengo que revisar las colas y los estados de VENTAS con el for
                    Cliente cliente = new Cliente();

                    bool empleadoLibre = false;
                    for (int j = 0; j < fila1.Ventas.Count; j++)
                    {
                        if (fila1.Ventas[j].Estado == "Libre")
                        {
                            // Si es atendido se cambiam los estado correspondientes, y se genera el fin de atención
                            cliente.Estado = "SV" + (j + 1);
                            cliente.HoraInicioAtencion = fila2.Reloj;
                            fila2.Ventas[j].Estado = "Ocupado";

                            fila2.Fin_venta.Rnd = generarRandom(random);
                            fila2.Fin_venta.TiempoAtencion = calcularTiempo(mediaFinVenta, fila2.Fin_venta.Rnd);
                            fila2.Fin_venta.FinAtencion[j] = fila2.Reloj + fila2.Fin_venta.TiempoAtencion;
                            empleadoLibre = true;
                            break;
                        }
                    }
                    if (!empleadoLibre)
                    {
                        cliente.HoraInicioEspera = fila2.Reloj;
                        // Si ninguna está libre, deberá esperar en la cola más corta
                        if (fila1.Ventas[0].Cola == fila1.Ventas[1].Cola)
                        {
                            cliente.Estado = "EV1";
                            fila2.Ventas[0].Cola = fila1.Ventas[0].Cola + 1;
                        }
                        else if (fila1.Ventas[0].Cola < fila1.Ventas[1].Cola)
                        {
                            cliente.Estado = "EV1";
                            fila2.Ventas[0].Cola = fila1.Ventas[0].Cola + 1;
                        }
                        else
                        {
                            cliente.Estado = "EV2";
                            fila2.Ventas[1].Cola = fila1.Ventas[1].Cola + 1;
                        }
                    }

                    // Como es una llegada tengo que agregar el cliente
                    fila2.Clientes.Add(cliente);
                    agregarClienteTabla(fila2, tablaResultado, cliente);
                }

                // LLEGADA ATENCIÓN EMPRESARIAL
                if (fila2.Evento == "llegada_AE")
                {
                    copiarProximasLlegadas(fila1, fila2);

                    // Genero la prox. llegada_AE
                    fila2.LlegadaAE.Rnd = generarRandom(random);
                    fila2.LlegadaAE.TiempoEntreLlegadas = calcularTiempo(mediaLlegadaAE, fila2.LlegadaAE.Rnd);
                    fila2.LlegadaAE.ProxLlegada = fila2.Reloj + fila2.LlegadaAE.TiempoEntreLlegadas;

                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);

                    // Tengo que revisar las colas y los estados de VENTAS con el for

                    Cliente cliente = new Cliente();

                    bool empleadoLibre = false;
                    for (int j = 0; j < fila1.AtencionEmp.Count; j++)
                    {
                        if (fila1.AtencionEmp[j].Cola == 0 && fila1.AtencionEmp[j].Estado == "Libre")
                        {
                            // Si es atendido se cambiam los estado correspondientes, y se genera el fin de atención
                            cliente.Estado = "SAE" + (j + 1);
                            cliente.HoraInicioAtencion = fila2.Reloj;
                            fila2.AtencionEmp[j].Estado = "Ocupado";

                            fila2.Fin_AE.Rnd = generarRandom(random);
                            fila2.Fin_AE.TiempoAtencion = calcularTiempo(mediaFinAE, fila2.Fin_AE.Rnd);
                            fila2.Fin_AE.FinAtencion[j] = fila2.Reloj + fila2.Fin_AE.TiempoAtencion;

                            empleadoLibre = true;
                            break;
                        }
                    }
                    if (!empleadoLibre)
                    {
                        cliente.HoraInicioEspera = fila2.Reloj;
                        // Si ninguna está libre, deberá esperar en la cola más corta
                        if (fila1.AtencionEmp[0].Cola == fila1.AtencionEmp[1].Cola)
                        {
                            cliente.Estado = "EAE1";
                            fila2.AtencionEmp[0].Cola = fila1.AtencionEmp[0].Cola + 1;
                        }
                        else if (fila1.AtencionEmp[0].Cola < fila1.AtencionEmp[1].Cola)
                        {
                            cliente.Estado = "EAE1";
                            fila2.AtencionEmp[0].Cola = fila1.AtencionEmp[0].Cola + 1;
                        }
                        else
                        {
                            cliente.Estado = "EAE2";
                            fila2.AtencionEmp[1].Cola = fila1.AtencionEmp[1].Cola + 1;
                        }
                    }
                    // Como es una llegada tengo que agregar el cliente
                    fila2.Clientes.Add(cliente);
                    agregarClienteTabla(fila2, tablaResultado, cliente);
                }

                // LLEGADA POSTALES
                if (fila2.Evento == "llegada_postales")
                {
                    copiarProximasLlegadas(fila1, fila2);
                    // Genero la prox. llegada_AE
                    fila2.LlegadaPostales.Rnd = generarRandom(random);
                    fila2.LlegadaPostales.TiempoEntreLlegadas = calcularTiempo(mediaLlegadaPostales, fila2.LlegadaPostales.Rnd);
                    fila2.LlegadaPostales.ProxLlegada = fila2.Reloj + fila2.LlegadaPostales.TiempoEntreLlegadas;

                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);

                    Cliente cliente = new Cliente();

                    verificarLlegadaPostales(fila1, fila2, random, cliente);



                    // Como es una llegada tengo que agregar el cliente
                    fila2.Clientes.Add(cliente);
                    agregarClienteTabla(fila2, tablaResultado, cliente);
                }

                // --------------------------------------------FINES DE ATENCIÓN---------------------------------------------------------------------------------------
                // FIN ENVIO PAQUETES

                if (fila2.Evento == "fin_envio1")
                {
                    copiarProximasLlegadas(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarObjetosFilaAnterior(fila1, fila2);  // incluye los clientes, y los objetos (servidores)
                    copiarEstadisticas(fila1, fila2);  // copio los acumuladores y contadores

                    calcularFinAtencionEnvio(1, fila1, fila2, random, mediaFinPaquete);
                }

                if (fila2.Evento == "fin_envio2")
                {
                    copiarProximasLlegadas(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarObjetosFilaAnterior(fila1, fila2);  // incluye los clientes, y los objetos (servidores)
                    copiarEstadisticas(fila1, fila2);  // copio los acumuladores y contadores

                    calcularFinAtencionEnvio(2, fila1, fila2, random, mediaFinPaquete);
                }
                if (fila2.Evento == "fin_envio3")
                {
                    copiarProximasLlegadas(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarObjetosFilaAnterior(fila1, fila2);  // incluye los clientes, y los objetos (servidores)
                    copiarEstadisticas(fila1, fila2);  // copio los acumuladores y contadores

                    calcularFinAtencionEnvio(3, fila1, fila2, random, mediaFinPaquete);
                }

                // FIN RECLAMO
                if (fila2.Evento == "fin_reclamo1")
                {
                    copiarProximasLlegadas(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);

                    calcularFinAtencionReclamo(1, fila1, fila2, random, mediaFinReclamo);
                }

                if (fila2.Evento == "fin_reclamo2")
                {
                    copiarProximasLlegadas(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);

                    calcularFinAtencionReclamo(2, fila1, fila2, random, mediaFinReclamo);
                }

                if (fila2.Evento == "fin_reclamo3")
                {
                    copiarProximasLlegadas(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);

                    calcularFinAtencionReclamo(3, fila1, fila2, random, mediaFinReclamo);
                }




                // FIN VENTAS 
                if (fila2.Evento == "fin_venta1")
                {
                    copiarProximasLlegadas(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);

                    calcularFinAtencionVenta(1, fila1, fila2, random, mediaFinVenta);
                }


                if (fila2.Evento == "fin_venta2")
                {
                    copiarProximasLlegadas(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);

                    calcularFinAtencionVenta(2, fila1, fila2, random, mediaFinVenta);
                }

                // FIN ATENCIÓN EMPRESARIAL

                if (fila2.Evento == "fin_AE1")
                {
                    copiarProximasLlegadas(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);

                    calcularFinAtencionEmp(1, fila1, fila2, random, mediaFinAE);
                }
                if (fila2.Evento == "fin_AE2")
                {
                    copiarProximasLlegadas(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);

                    calcularFinAtencionEmp(2, fila1, fila2, random, mediaFinAE);
                }

                // FIN POSTALES
                if (fila2.Evento == "fin_postales")
                {
                    copiarProximasLlegadas(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);

                    calcularFinAtencionPostales(fila1, fila2, random, mediaFinPostales);
                }


                List<double> posiblesProximosEventos = new List<double>();
                posiblesProximosEventos.Add(fila2.LlegadaEnvio.ProxLlegada); // 0 
                posiblesProximosEventos.Add(fila2.LlegadaReclamo.ProxLlegada); // 1
                posiblesProximosEventos.Add(fila2.LlegadaVenta.ProxLlegada); // 2
                posiblesProximosEventos.Add(fila2.LlegadaAE.ProxLlegada); // 3
                posiblesProximosEventos.Add(fila2.LlegadaPostales.ProxLlegada); //4
                posiblesProximosEventos.Add((double)fila2.Fin_envio.FinAtencion[0]);
                posiblesProximosEventos.Add((double)fila2.Fin_envio.FinAtencion[1]);
                posiblesProximosEventos.Add((double)fila2.Fin_envio.FinAtencion[2]);
                posiblesProximosEventos.Add((double)fila2.Fin_reclamo.FinAtencion[0]);
                posiblesProximosEventos.Add((double)fila2.Fin_reclamo.FinAtencion[1]);
                posiblesProximosEventos.Add((double)fila2.Fin_reclamo.FinAtencion[2]);
                posiblesProximosEventos.Add((double)fila2.Fin_venta.FinAtencion[0]);
                posiblesProximosEventos.Add((double)fila2.Fin_venta.FinAtencion[1]);
                posiblesProximosEventos.Add((double)fila2.Fin_AE.FinAtencion[0]);
                posiblesProximosEventos.Add((double)fila2.Fin_AE.FinAtencion[1]);
                posiblesProximosEventos.Add((double)fila2.Fin_postales.FinAtencion);


                List<double> proximoEvento = buscarProxEvento(posiblesProximosEventos);

                filasMostrar.Add(fila1);
                fila1 = fila2;

                if (!(i == cantFilas - 1))
                {
                    fila2 = new FilaVectorPunto3();
                    fila2.Reloj = (double)proximoEvento[0];
                    fila2.Evento = nombresEventos[(int)proximoEvento[1]];
                }
                else
                {
                    List<String> listaAux = new List<String>();
                    listaAux.Add(fila2.Evento.ToString());
                    listaAux.Add(fila2.Reloj.ToString());
                    listaAux.Add(fila2.LlegadaEnvio.Rnd.ToString());
                    listaAux.Add(fila2.LlegadaEnvio.TiempoEntreLlegadas.ToString());
                    listaAux.Add(fila2.LlegadaEnvio.ProxLlegada.ToString());
                    listaAux.Add(fila2.LlegadaReclamo.Rnd.ToString());
                    listaAux.Add(fila2.LlegadaReclamo.TiempoEntreLlegadas.ToString());
                    listaAux.Add(fila1.LlegadaReclamo.ProxLlegada.ToString());
                    listaAux.Add(fila2.LlegadaVenta.Rnd.ToString());
                    listaAux.Add(fila2.LlegadaVenta.TiempoEntreLlegadas.ToString());
                    listaAux.Add(fila2.LlegadaVenta.ProxLlegada.ToString());
                    listaAux.Add(fila2.LlegadaAE.Rnd.ToString());
                    listaAux.Add(fila2.LlegadaAE.TiempoEntreLlegadas.ToString());
                    listaAux.Add(fila2.LlegadaAE.ProxLlegada.ToString());
                    listaAux.Add(fila2.LlegadaPostales.Rnd.ToString());
                    listaAux.Add(fila2.LlegadaPostales.TiempoEntreLlegadas.ToString());
                    listaAux.Add(fila2.LlegadaPostales.ProxLlegada.ToString());
                    listaAux.Add(fila2.Fin_envio.Rnd.ToString());
                    listaAux.Add(fila2.Fin_envio.TiempoAtencion.ToString());
                    listaAux.Add(fila2.Fin_envio.FinAtencion[0].ToString());
                    listaAux.Add(fila2.Fin_envio.FinAtencion[1].ToString());
                    listaAux.Add(fila2.Fin_envio.FinAtencion[2].ToString());
                    listaAux.Add(fila2.Fin_reclamo.Rnd.ToString());
                    listaAux.Add(fila2.Fin_reclamo.TiempoAtencion.ToString());
                    listaAux.Add(fila2.Fin_reclamo.FinAtencion[0].ToString());
                    listaAux.Add(fila2.Fin_reclamo.FinAtencion[1].ToString());
                    listaAux.Add(fila2.Fin_reclamo.FinAtencion[2].ToString());


                    listaAux.Add(fila2.Fin_venta.Rnd.ToString());
                    listaAux.Add(fila2.Fin_venta.TiempoAtencion.ToString());
                    listaAux.Add(fila2.Fin_venta.FinAtencion[0].ToString());
                    listaAux.Add(fila2.Fin_venta.FinAtencion[1].ToString());

                    listaAux.Add(fila2.Fin_AE.Rnd.ToString());
                    listaAux.Add(fila2.Fin_AE.TiempoAtencion.ToString());
                    listaAux.Add(fila2.Fin_AE.FinAtencion[0].ToString());
                    listaAux.Add(fila2.Fin_AE.FinAtencion[1].ToString());
                    listaAux.Add(fila2.Fin_postales.Rnd.ToString());
                    listaAux.Add(fila2.Fin_postales.TiempoAtencion.ToString());
                    listaAux.Add(fila2.Fin_postales.FinAtencion.ToString());

                    listaAux.Add(fila2.EnvioPaquetes[0].Cola.ToString());
                    listaAux.Add(fila2.EnvioPaquetes[0].Estado.ToString());
                    listaAux.Add(fila2.EnvioPaquetes[1].Cola.ToString());
                    listaAux.Add(fila2.EnvioPaquetes[1].Estado.ToString());
                    listaAux.Add(fila2.EnvioPaquetes[2].Cola.ToString());
                    listaAux.Add(fila2.EnvioPaquetes[2].Estado.ToString());
                    listaAux.Add(fila2.Reclamos[0].Cola.ToString());
                    listaAux.Add(fila2.Reclamos[0].Estado.ToString());
                    listaAux.Add(fila2.Reclamos[1].Cola.ToString());
                    listaAux.Add(fila2.Reclamos[1].Estado.ToString());
                    listaAux.Add(fila2.Reclamos[2].Cola.ToString());
                    listaAux.Add(fila2.Reclamos[2].Estado.ToString());

                    listaAux.Add(fila2.Ventas[0].Cola.ToString());
                    listaAux.Add(fila2.Ventas[0].Estado.ToString());
                    listaAux.Add(fila2.Ventas[1].Cola.ToString());
                    listaAux.Add(fila2.Ventas[1].Estado.ToString());


                    listaAux.Add(fila2.AtencionEmp[0].Cola.ToString());
                    listaAux.Add(fila2.AtencionEmp[0].Estado.ToString());
                    listaAux.Add(fila2.AtencionEmp[1].Cola.ToString());
                    listaAux.Add(fila2.AtencionEmp[1].Estado.ToString());
                    listaAux.Add(fila2.Postales[0].Cola.ToString());
                    listaAux.Add(fila2.Postales[0].Estado.ToString());
                    listaAux.Add(fila2.EstadisticasEnvio.AcumuladorEspera.ToString());
                    listaAux.Add(fila2.EstadisticasEnvio.AcumuladorOcupacion.ToString());
                    listaAux.Add(fila2.EstadisticasEnvio.CantClientesAtendidos.ToString());
                    listaAux.Add(fila2.EstadisticasReclamo.AcumuladorEspera.ToString());
                    listaAux.Add(fila2.EstadisticasReclamo.AcumuladorOcupacion.ToString());
                    listaAux.Add(fila2.EstadisticasReclamo.CantClientesAtendidos.ToString());
                    listaAux.Add(fila2.EstadisticasVenta.AcumuladorEspera.ToString());
                    listaAux.Add(fila2.EstadisticasVenta.AcumuladorOcupacion.ToString());
                    listaAux.Add(fila2.EstadisticasVenta.CantClientesAtendidos.ToString());
                    listaAux.Add(fila2.EstadisticasAE.AcumuladorEspera.ToString());
                    listaAux.Add(fila2.EstadisticasAE.AcumuladorOcupacion.ToString());
                    listaAux.Add(fila2.EstadisticasAE.CantClientesAtendidos.ToString());
                    listaAux.Add(fila2.EstadisticasPostales.AcumuladorEspera.ToString());
                    listaAux.Add(fila2.EstadisticasPostales.AcumuladorOcupacion.ToString());
                    listaAux.Add(fila2.EstadisticasPostales.CantClientesAtendidos.ToString());

                    foreach (Cliente Cli in fila2.Clientes)
                    {
                        listaAux.Add(Cli.Estado);
                        listaAux.Add(Cli.HoraInicioEspera.ToString());
                        listaAux.Add(Cli.HoraInicioAtencion.ToString());
                    }

                    filasMostrar.Add(fila2);

                    tablaResultado.Rows.Add(listaAux.ToArray());

                    //tengo que mostrar la útlima fila siempre
                    //grdSimulacion.Rows.Add(listaAux.ToArray()); // me falta agregar las columnas de ocupación en el data grid view
                }

            }
            Simulacion.CargarCsv(tablaResultado, "SimPunto3.csv");
            cargarResultados(fila2);
        }
        private void cargarResultados(FilaVectorPunto3 fila2)
        {
            double acEsperaE = fila2.EstadisticasEnvio.AcumuladorEspera;
            double acOcupE = fila2.EstadisticasEnvio.AcumuladorOcupacion;
            double cantE = fila2.EstadisticasEnvio.CantClientesAtendidos;

            double acEsperaR = fila2.EstadisticasReclamo.AcumuladorEspera;
            double acOcupR = fila2.EstadisticasReclamo.AcumuladorOcupacion;
            double cantR = fila2.EstadisticasReclamo.CantClientesAtendidos;

            double acEsperaV = fila2.EstadisticasVenta.AcumuladorEspera;
            double acOcupV = fila2.EstadisticasVenta.AcumuladorOcupacion;
            double cantV = fila2.EstadisticasVenta.CantClientesAtendidos;

            double acEsperaAE = fila2.EstadisticasAE.AcumuladorEspera;
            double acOcupAE = fila2.EstadisticasAE.AcumuladorOcupacion;
            double cantAE = fila2.EstadisticasAE.CantClientesAtendidos;

            double acEsperaP = fila2.EstadisticasPostales.AcumuladorEspera;
            double acOcupP = fila2.EstadisticasPostales.AcumuladorOcupacion;
            double cantP = fila2.EstadisticasPostales.CantClientesAtendidos;

            double relojFinal = fila2.Reloj;

            double[] vectorRes = { acEsperaE,cantE, acEsperaR, cantR, acEsperaV, cantV, acEsperaAE, cantAE, acEsperaP, cantP,
            acOcupE, acOcupR, acOcupV, acOcupAE, acOcupP, relojFinal};


            // FALTA CORREGIR LOS CÁLCULOS PARA EL NUEVO SERVICIO !!!

            // Falta verificar cuando la cantidad es 0 -> no se puede calcular (da NaN)
            txtEsperaE.Text = (Math.Truncate((vectorRes[0] / vectorRes[1]) * 100) / 100).ToString();
            txtEsperaR.Text = (Math.Truncate((vectorRes[2] / vectorRes[3]) * 100) / 100).ToString();
            txtEsperaV.Text = (Math.Truncate((vectorRes[4] / vectorRes[5]) * 100) / 100).ToString();
            txtEsperaAE.Text = (Math.Truncate((vectorRes[6] / vectorRes[7]) * 100) / 100).ToString();
            txtEsperaP.Text = (Math.Truncate((vectorRes[8] / vectorRes[9]) * 100) / 100).ToString();

            relojFinal = vectorRes[15];

            double ocupE = (vectorRes[10] / 3) * 100 / relojFinal;
            double ocupR = (vectorRes[11] / 3) * 100 / relojFinal;
            double ocupV = (vectorRes[12] / 2) * 100 / relojFinal;
            double ocupAE = (vectorRes[13] / 2) * 100 / relojFinal;
            double ocupP = (vectorRes[14] * 100) / relojFinal;

            txtOcupacionE.Text = (Math.Truncate(ocupE * 100) / 100).ToString() + "%";
            txtOcupacionR.Text = (Math.Truncate(ocupR * 100) / 100).ToString() + "%";
            txtOcupacionV.Text = (Math.Truncate(ocupV * 100) / 100).ToString() + "%";
            txtOcupacionAE.Text = (Math.Truncate(ocupAE * 100) / 100).ToString() + "%";
            txtOcupacionP.Text = (Math.Truncate(ocupP * 100) / 100).ToString() + "%";
        }

        private List<double> buscarProxEvento(List<double> tiempos)
        {
            double proxReloj = double.MaxValue;
            int indice = -1;

            for (int i = 0; i < tiempos.Count; i++)
            {
                if (tiempos[i] != 0 && tiempos[i] < proxReloj)
                {
                    proxReloj = tiempos[i];
                    indice = i;
                }
                // RESOLVER PARA EL CASO DE QUE SEAN IGUALES
            }
            List<double> res = new List<double>();
            res.Add(proxReloj);
            res.Add(indice);

            return res;


        }

        private double calcularTiempo(double media, double rnd)
        {
            double tiempo = truncarNumero(-media * Math.Log(1 - rnd));
            return tiempo;

        }

        private double truncarNumero(double numero)
        {
            double factor = Math.Pow(10, 2);
            return Math.Truncate(numero * factor) / factor;
        }


        private double generarRandom(Random random)
        {
            double numero = random.NextDouble();
            return truncarNumero(numero);
        }

        private int buscarClientePorEstado(string estado, List<Cliente> clientes)
        {
            for (int i = 0; i < clientes.Count; i++)
            {
                if (clientes[i].Estado == estado)
                {
                    return i;
                }
            }
            throw new InvalidOperationException($"No se encontró un cliente con el estado: {estado}");
        }

        private void copiarObjetosFilaAnterior(FilaVectorPunto3 fila1, FilaVectorPunto3 fila2)
        {
            fila2.EnvioPaquetes = fila1.EnvioPaquetes;
            fila2.Reclamos = fila1.Reclamos;
            fila2.Ventas = fila1.Ventas;
            fila2.AtencionEmp = fila1.AtencionEmp;
            fila2.Postales = fila1.Postales;


            fila2.Clientes = fila1.Clientes;
        }

        private void copiarFinesAtencion(FilaVectorPunto3 fila1, FilaVectorPunto3 fila2)
        {
            fila2.Fin_envio.FinAtencion = fila1.Fin_envio.FinAtencion;
            fila2.Fin_reclamo.FinAtencion = fila1.Fin_reclamo.FinAtencion;
            fila2.Fin_venta.FinAtencion = fila1.Fin_venta.FinAtencion;
            fila2.Fin_AE.FinAtencion = fila1.Fin_AE.FinAtencion;
            fila2.Fin_postales.FinAtencion = fila1.Fin_postales.FinAtencion;
        }

        private void copiarProximasLlegadas(FilaVectorPunto3 fila1, FilaVectorPunto3 fila2)
        {
            fila2.LlegadaReclamo.ProxLlegada = fila1.LlegadaReclamo.ProxLlegada;
            fila2.LlegadaVenta.ProxLlegada = fila1.LlegadaVenta.ProxLlegada;
            fila2.LlegadaAE.ProxLlegada = fila1.LlegadaAE.ProxLlegada;
            fila2.LlegadaPostales.ProxLlegada = fila1.LlegadaPostales.ProxLlegada;
            fila2.LlegadaEnvio.ProxLlegada = fila1.LlegadaEnvio.ProxLlegada;
        }

        private void copiarEstadisticas(FilaVectorPunto3 fila1, FilaVectorPunto3 fila2)
        {
            fila2.EstadisticasEnvio = fila1.EstadisticasEnvio;
            fila2.EstadisticasReclamo = fila1.EstadisticasReclamo;
            fila2.EstadisticasVenta = fila1.EstadisticasVenta;
            fila2.EstadisticasAE = fila1.EstadisticasAE;
            fila2.EstadisticasPostales = fila1.EstadisticasPostales;
        }

        private void calcularFinAtencionEnvio(int nroObjetoEnvio, FilaVectorPunto3 fila1, FilaVectorPunto3 fila2, Random random, double media)
        {
            string estadoSiendoAtendido = "SE" + nroObjetoEnvio;

            int indice = nroObjetoEnvio - 1;

            // Busco al cliente que estaba siendo atendido en ese objeto y lo elimino, y cambio las estadísticas
            int indexClienteAtendido = buscarClientePorEstado(estadoSiendoAtendido, fila2.Clientes);

            fila2.EstadisticasEnvio.CantClientesAtendidos += 1;

            fila2.EstadisticasEnvio.AcumuladorOcupacion += truncarNumero(fila2.Reloj - fila2.Clientes[indexClienteAtendido].HoraInicioAtencion);
            fila2.Clientes[indexClienteAtendido].Estado = "Eliminado";

            //Si en el servidore donde estaba sinedo atendido no hay cola, pasa a estar libre
            if (fila1.EnvioPaquetes[indice].Cola == 0)
            {
                fila2.EnvioPaquetes[indice].Estado = "Libre";
                fila2.Fin_envio.FinAtencion[indice] = 0;
            }
            // De lo contrario, el cliente que estaba en la cola debe ser atendido (el servidor sigue ocupado y
            // se genera un nuevo fin de atención), y el estado de ese cliente debe pasar a "siendo atendido"
            else
            {
                string estadoEsperando = "EE" + nroObjetoEnvio;
                fila2.EnvioPaquetes[indice].Cola = fila1.EnvioPaquetes[indice].Cola - 1;
                fila2.EnvioPaquetes[indice].Estado = "Ocupado";
                int indexClientePorAtender = buscarClientePorEstado(estadoEsperando, fila1.Clientes);
                fila2.Clientes[indexClientePorAtender].Estado = estadoSiendoAtendido;
                fila2.Clientes[indexClientePorAtender].HoraInicioAtencion = fila2.Reloj;




                fila2.Fin_envio.Rnd = generarRandom(random);
                fila2.Fin_envio.TiempoAtencion = calcularTiempo(media, fila2.Fin_envio.Rnd);
                fila2.Fin_envio.FinAtencion[indice] = fila2.Fin_envio.TiempoAtencion + fila2.Reloj;

                fila2.EstadisticasEnvio.AcumuladorEspera = truncarNumero((fila2.Reloj - fila2.Clientes[indexClientePorAtender].HoraInicioEspera) + fila1.EstadisticasEnvio.AcumuladorEspera);
                fila2.Clientes[indexClientePorAtender].HoraInicioEspera = 0;
            }
        }

        private void calcularFinAtencionReclamo(int nroObjetoReclamo, FilaVectorPunto3 fila1, FilaVectorPunto3 fila2, Random random, double media)
        {
            string estadoSiendoAtendido = "SR" + nroObjetoReclamo;

            int indice = nroObjetoReclamo - 1;

            int indexClienteAtendido = buscarClientePorEstado(estadoSiendoAtendido, fila1.Clientes);

            fila2.EstadisticasReclamo.CantClientesAtendidos += 1;
            fila2.EstadisticasReclamo.AcumuladorOcupacion = truncarNumero(fila2.Reloj - fila2.Clientes[indexClienteAtendido].HoraInicioAtencion) + fila1.EstadisticasReclamo.AcumuladorOcupacion;
            fila2.Clientes[indexClienteAtendido].Estado = "Eliminado";

            if (fila1.Reclamos[indice].Cola == 0)
            {
                fila2.Reclamos[indice].Estado = "Libre";
                fila2.Fin_reclamo.FinAtencion[indice] = 0;
            }
            else
            {
                string estadoEsperando = "ER" + nroObjetoReclamo;
                fila2.Reclamos[indice].Cola = fila1.Reclamos[indice].Cola - 1;
                fila2.Reclamos[indice].Estado = "Ocupado";
                int indexClientePorAtender = buscarClientePorEstado(estadoEsperando, fila1.Clientes);
                fila2.Clientes[indexClientePorAtender].Estado = estadoSiendoAtendido;
                fila2.Clientes[indexClientePorAtender].HoraInicioAtencion = fila2.Reloj;


                fila2.Fin_reclamo.Rnd = generarRandom(random);
                fila2.Fin_reclamo.TiempoAtencion = calcularTiempo(media, fila2.Fin_reclamo.Rnd);
                fila2.Fin_reclamo.FinAtencion[indice] = fila2.Fin_reclamo.TiempoAtencion + fila2.Reloj;

                fila2.EstadisticasReclamo.AcumuladorEspera = truncarNumero(fila2.Reloj - fila2.Clientes[indexClientePorAtender].HoraInicioEspera) + fila1.EstadisticasReclamo.AcumuladorEspera;
                fila2.Clientes[indexClientePorAtender].HoraInicioEspera = 0;
            }
        }

        private void calcularFinAtencionVenta(int nroObjetoVenta, FilaVectorPunto3 fila1, FilaVectorPunto3 fila2, Random random, double media)
        {
            string estadoSiendoAtendido = "SV" + nroObjetoVenta;


            int indice = nroObjetoVenta - 1;

            int indexClienteAtendido = buscarClientePorEstado(estadoSiendoAtendido, fila2.Clientes);

            fila2.EstadisticasVenta.CantClientesAtendidos += 1;

            fila2.EstadisticasVenta.AcumuladorOcupacion = truncarNumero(fila2.Reloj - fila2.Clientes[indexClienteAtendido].HoraInicioAtencion) + fila1.EstadisticasVenta.AcumuladorOcupacion;
            fila2.Clientes[indexClienteAtendido].Estado = "Eliminado";

            if (fila1.Ventas[indice].Cola == 0)
            {
                fila2.Ventas[indice].Estado = "Libre";
                fila2.Fin_venta.FinAtencion[indice] = 0;
            }
            else
            {
                string estadoEsperando = "EV" + nroObjetoVenta;

                fila2.Ventas[indice].Cola = fila1.Ventas[indice].Cola - 1;
                fila2.Ventas[indice].Estado = "Ocupado";
                int indexClientePorAtender = buscarClientePorEstado(estadoEsperando, fila1.Clientes);
                fila2.Clientes[indexClientePorAtender].Estado = estadoSiendoAtendido;
                fila2.Clientes[indexClientePorAtender].HoraInicioAtencion = fila2.Reloj;


                fila2.Fin_venta.Rnd = generarRandom(random);
                fila2.Fin_venta.TiempoAtencion = calcularTiempo(media, fila2.Fin_venta.Rnd);
                fila2.Fin_venta.FinAtencion[indice] = fila2.Fin_venta.TiempoAtencion + fila2.Reloj;

                fila2.EstadisticasVenta.AcumuladorEspera = truncarNumero(fila2.Reloj - fila2.Clientes[indexClientePorAtender].HoraInicioEspera) + fila1.EstadisticasVenta.AcumuladorEspera;
                fila2.Clientes[indexClientePorAtender].HoraInicioEspera = 0;
            }
        }

        private void calcularFinAtencionEmp(int nroObjetoAE, FilaVectorPunto3 fila1, FilaVectorPunto3 fila2, Random random, double media)
        {

            string estadoSiendoAtendido = "SAE" + nroObjetoAE;

            int indice = nroObjetoAE - 1;

            int indexClienteAtendido = buscarClientePorEstado(estadoSiendoAtendido, fila1.Clientes);

            fila2.EstadisticasAE.CantClientesAtendidos += 1;

            fila2.EstadisticasAE.AcumuladorOcupacion = truncarNumero(fila2.Reloj - fila2.Clientes[indexClienteAtendido].HoraInicioAtencion) + fila1.EstadisticasAE.AcumuladorOcupacion;
            fila2.Clientes[indexClienteAtendido].Estado = "Eliminado";

            if (fila1.AtencionEmp[indice].Cola == 0)
            {
                fila2.AtencionEmp[indice].Estado = "Libre";
                fila2.Fin_AE.FinAtencion[indice] = 0;
            }
            else
            {
                string estadoEsperando = "EAE" + nroObjetoAE;
                fila2.AtencionEmp[indice].Cola = fila1.AtencionEmp[indice].Cola - 1;
                fila2.AtencionEmp[indice].Estado = "Ocupado";
                int indexClientePorAtender = buscarClientePorEstado(estadoEsperando, fila1.Clientes);
                fila2.Clientes[indexClientePorAtender].Estado = estadoSiendoAtendido;
                fila2.Clientes[indexClientePorAtender].HoraInicioAtencion = fila2.Reloj;


                fila2.Fin_AE.Rnd = generarRandom(random);
                fila2.Fin_AE.TiempoAtencion = calcularTiempo(media, fila2.Fin_AE.Rnd);
                fila2.Fin_AE.FinAtencion[indice] = fila2.Fin_AE.TiempoAtencion + fila2.Reloj;

                fila2.EstadisticasAE.AcumuladorEspera = truncarNumero(fila2.Reloj - fila2.Clientes[indexClientePorAtender].HoraInicioEspera) + fila1.EstadisticasAE.AcumuladorEspera;
                fila2.Clientes[indexClientePorAtender].HoraInicioEspera = 0;
            }
        }
        private void calcularFinAtencionPostales(FilaVectorPunto3 fila1, FilaVectorPunto3 fila2, Random random, double media)
        {

            int indexClienteAtendido = buscarClientePorEstado("SP", fila1.Clientes);

            fila2.EstadisticasPostales.CantClientesAtendidos += 1;

            fila2.EstadisticasPostales.AcumuladorOcupacion = truncarNumero(fila2.Reloj - fila2.Clientes[indexClienteAtendido].HoraInicioAtencion) + fila1.EstadisticasPostales.AcumuladorOcupacion;
            fila2.Clientes[indexClienteAtendido].Estado = "Eliminado";

            if (fila1.Postales[0].Cola == 0)
            {
                fila2.Postales[0].Estado = "Libre";
                fila2.Fin_postales.FinAtencion = 0;
            }
            else
            {
                fila2.Postales[0].Cola = fila1.Postales[0].Cola - 1;
                fila2.Postales[0].Estado = "Ocupado";
                int indiceClientePorAtender = buscarClientePorEstado("EP", fila1.Clientes);
                fila2.Clientes[indiceClientePorAtender].Estado = "SP";
                fila2.Clientes[indiceClientePorAtender].HoraInicioAtencion = fila2.Reloj;


                fila2.Fin_postales.Rnd = generarRandom(random);
                fila2.Fin_postales.TiempoAtencion = calcularTiempo(media, fila2.Fin_postales.Rnd);
                fila2.Fin_postales.FinAtencion = fila2.Fin_postales.TiempoAtencion + fila2.Reloj;

                fila2.EstadisticasPostales.AcumuladorEspera = truncarNumero(fila2.Reloj - fila2.Clientes[indiceClientePorAtender].HoraInicioEspera) + fila1.EstadisticasPostales.AcumuladorEspera;
                fila2.Clientes[indiceClientePorAtender].HoraInicioEspera = 0;
            }
        }


        public void verificarLlegadaEnvio(FilaVectorPunto3 fila1, FilaVectorPunto3 fila2, Random random, Cliente cliente)
        {

            // Reviso las colas y los estados de los objetos ENVIO (en este caso son 3)
            bool empleadoLibre = false;
            for (int j = 0; j < fila1.EnvioPaquetes.Count; j++)
            {
                if (fila1.EnvioPaquetes[j].Estado == "Libre")
                {
                    // Hay un objeto libre: le asigno al nuevo cliente el estado siendo atendido, e indico horaInicioAtención, cambio el estado del objeto donde se
                    // atiende, y genero el fin de atención para ese mismo objeto.
                    cliente.Estado = "SE" + (j + 1);
                    cliente.HoraInicioAtencion = fila2.Reloj;
                    fila2.EnvioPaquetes[j].Estado = "Ocupado";

                    fila2.Fin_envio.Rnd = generarRandom(random);
                    fila2.Fin_envio.TiempoAtencion = calcularTiempo(mediaFinPaquete, fila2.Fin_envio.Rnd);
                    fila2.Fin_envio.FinAtencion[j] = fila2.Reloj + fila2.Fin_envio.TiempoAtencion;

                    empleadoLibre = true;
                    break;

                }
            }
            if (!empleadoLibre)
            {
                // No hay un objeto libre, entonces tengo que buscar el de menor cola y esperar
                cliente.HoraInicioEspera = fila2.Reloj;
                if (fila1.EnvioPaquetes[0].Cola == fila1.EnvioPaquetes[1].Cola && fila1.EnvioPaquetes[1].Cola == fila1.EnvioPaquetes[2].Cola)
                {
                    // Si todos los objetos tienen la misma cola, espera en el objeto 1:
                    cliente.Estado = "EE1";
                    fila2.EnvioPaquetes[0].Cola += 1;
                }
                else if (fila1.EnvioPaquetes[0].Cola < fila1.EnvioPaquetes[1].Cola && fila1.EnvioPaquetes[0].Cola < fila1.EnvioPaquetes[2].Cola)
                {
                    // La menor cola es la del objeto 1
                    cliente.Estado = "EE1";
                    fila2.EnvioPaquetes[0].Cola += 1;
                }
                else if (fila1.EnvioPaquetes[1].Cola < fila1.EnvioPaquetes[0].Cola && fila1.EnvioPaquetes[1].Cola < fila1.EnvioPaquetes[2].Cola)
                {
                    // La menor cola es la del objeto 2
                    cliente.Estado = "EE2";
                    fila2.EnvioPaquetes[1].Cola += 1;
                }
                else if (fila1.EnvioPaquetes[2].Cola < fila1.EnvioPaquetes[0].Cola && fila1.EnvioPaquetes[2].Cola < fila1.EnvioPaquetes[1].Cola)
                {
                    // La menor cola es la del objeto 3
                    cliente.Estado = "EE3";
                    fila2.EnvioPaquetes[2].Cola += 1;
                }
                // Si dos colas son iguales y una diferente
                else if (fila1.EnvioPaquetes[0].Cola == fila1.EnvioPaquetes[1].Cola && fila1.EnvioPaquetes[0].Cola < fila1.EnvioPaquetes[2].Cola)
                { // Las colas 1 y 2  son iguales y menores que la 3
                    cliente.Estado = "EE1";
                    fila2.EnvioPaquetes[0].Cola += 1;
                }
                else if (fila1.EnvioPaquetes[0].Cola == fila1.EnvioPaquetes[1].Cola && fila1.EnvioPaquetes[0].Cola > fila1.EnvioPaquetes[2].Cola)
                {
                    // Las colas 1 y 2  son iguales y mayores que la 3
                    cliente.Estado = "EE3";
                    fila2.EnvioPaquetes[2].Cola += 1;
                }
                else if (fila1.EnvioPaquetes[0].Cola == fila1.EnvioPaquetes[2].Cola && fila1.EnvioPaquetes[0].Cola < fila1.EnvioPaquetes[1].Cola)
                {
                    // Las colas del 1 y 3 son iguales y menores que la 2
                    cliente.Estado = "EE1";
                    fila2.EnvioPaquetes[0].Cola += 1;
                }
                else if (fila1.EnvioPaquetes[0].Cola == fila1.EnvioPaquetes[2].Cola && fila1.EnvioPaquetes[0].Cola > fila1.EnvioPaquetes[1].Cola)
                {
                    // Las colas del 1 y 3 son iguales y mayores que la 2
                    cliente.Estado = "EE2";
                    fila2.EnvioPaquetes[1].Cola += 1;
                }
                else if (fila1.EnvioPaquetes[1].Cola == fila1.EnvioPaquetes[2].Cola && fila1.EnvioPaquetes[2].Cola < fila1.EnvioPaquetes[0].Cola)
                {
                    // Las colas 2 y 3 son iguales y menores que la 1
                    cliente.Estado = "EE2";
                    fila2.EnvioPaquetes[1].Cola += 1;
                }
                else if (fila1.EnvioPaquetes[1].Cola == fila1.EnvioPaquetes[2].Cola && fila1.EnvioPaquetes[2].Cola > fila1.EnvioPaquetes[0].Cola)
                {
                    // Las colas 2 y 3 son iguales y mayores que la 1
                    cliente.Estado = "EE1";
                    fila2.EnvioPaquetes[0].Cola += 1;
                }

            }
        }

        private void verificarLlegadaPostales(FilaVectorPunto3 fila1, FilaVectorPunto3 fila2, Random random, Cliente cliente)
        {


            if (fila1.Postales[0].Cola == 0 && fila1.Postales[0].Estado == "Libre")
            {
                // Si es atendido se cambiam los estado correspondientes, y se genera el fin de atención

                cliente.Estado = "SP";
                cliente.HoraInicioAtencion = fila2.Reloj;
                fila2.Postales[0].Estado = "Ocupado";

                //Calculo fin_atención
                fila2.Fin_postales.Rnd = generarRandom(random);
                fila2.Fin_postales.TiempoAtencion = calcularTiempo(mediaFinPostales, fila2.Fin_postales.Rnd);
                fila2.Fin_postales.FinAtencion = fila2.Reloj + fila2.Fin_postales.TiempoAtencion;

            }
            else
            {
                cliente.Estado = "EP";
                cliente.HoraInicioEspera = fila2.Reloj;
                fila2.Postales[0].Cola = fila1.Postales[0].Cola + 1;
            }
        }

        public static void agregarColumnasTabla(DataTable tablaResultado)
        {
            tablaResultado.Columns.Add("Evento");
            tablaResultado.Columns.Add("Reloj");
            tablaResultado.Columns.Add("RND Llegada EP");
            tablaResultado.Columns.Add("Tiempo entre llegadas EP");
            tablaResultado.Columns.Add("Proxima llegada EP");
            tablaResultado.Columns.Add("RND Llegada R");
            tablaResultado.Columns.Add("Tiempo entre llegadas R");
            tablaResultado.Columns.Add("Proxima llegada R");
            tablaResultado.Columns.Add("RND Llegada V");
            tablaResultado.Columns.Add("Tiempo entre llegadas V");
            tablaResultado.Columns.Add("Proxima llegada V");
            tablaResultado.Columns.Add("RND Llegada AE");
            tablaResultado.Columns.Add("Tiempo entre llegadas AE");
            tablaResultado.Columns.Add("Proxima llegada AE");
            tablaResultado.Columns.Add("RND Llegada P");
            tablaResultado.Columns.Add("Tiempo entre llegadas P");
            tablaResultado.Columns.Add("Proxima llegada P");

            tablaResultado.Columns.Add("RND fin EP");
            tablaResultado.Columns.Add("Tiempo atencion EP");
            tablaResultado.Columns.Add("Fin envio 1");
            tablaResultado.Columns.Add("Fin envio 2");
            tablaResultado.Columns.Add("Fin envio 3");
            tablaResultado.Columns.Add("RND fin R");
            tablaResultado.Columns.Add("Tiempo atencion R");
            tablaResultado.Columns.Add("Fin reclamo 1");
            tablaResultado.Columns.Add("Fin reclamo 2");
            tablaResultado.Columns.Add("Fin reclamo 3");

            tablaResultado.Columns.Add("RND fin V");
            tablaResultado.Columns.Add("Tiempo atencion V");
            tablaResultado.Columns.Add("Fin venta 1");
            tablaResultado.Columns.Add("Fin venta 2");

            tablaResultado.Columns.Add("RND fin AE");
            tablaResultado.Columns.Add("Tiempo atencion AE");
            tablaResultado.Columns.Add("Fin atencion 1");
            tablaResultado.Columns.Add("Fin atencion 2");
            tablaResultado.Columns.Add("RND fin P");
            tablaResultado.Columns.Add("Tiempo atencion P");
            tablaResultado.Columns.Add("Fin postal");

            tablaResultado.Columns.Add("EP COLA 1");
            tablaResultado.Columns.Add("EP Estado 1");
            tablaResultado.Columns.Add("EP COLA 2");
            tablaResultado.Columns.Add("EP Estado 2");
            tablaResultado.Columns.Add("EP COLA 3");
            tablaResultado.Columns.Add("EP Estado 3");

            tablaResultado.Columns.Add("R COLA 1");
            tablaResultado.Columns.Add("R Estado 1");
            tablaResultado.Columns.Add("R COLA 2");
            tablaResultado.Columns.Add("R Estado 2");
            tablaResultado.Columns.Add("R COLA 3");
            tablaResultado.Columns.Add("R Estado 3");

            tablaResultado.Columns.Add("V COLA 1");
            tablaResultado.Columns.Add("V Estado 1");
            tablaResultado.Columns.Add("V COLA 2");
            tablaResultado.Columns.Add("V Estado 2");

            tablaResultado.Columns.Add("AE COLA 1");
            tablaResultado.Columns.Add("AE Estado 1");
            tablaResultado.Columns.Add("AE COLA 2");
            tablaResultado.Columns.Add("AE Estado 2");
            tablaResultado.Columns.Add("P COLA 1");
            tablaResultado.Columns.Add("P Estado 1");

            tablaResultado.Columns.Add("AC tiempo espera EP");
            tablaResultado.Columns.Add("AC ocupacion EP");
            tablaResultado.Columns.Add("CANT EP");
            tablaResultado.Columns.Add("AC tiempo espera R");
            tablaResultado.Columns.Add("AC ocupacion R");
            tablaResultado.Columns.Add("CANT R");
            tablaResultado.Columns.Add("AC tiempo espera V");
            tablaResultado.Columns.Add("AC ocupacion V");
            tablaResultado.Columns.Add("CANT V");
            tablaResultado.Columns.Add("AC tiempo espera AE");
            tablaResultado.Columns.Add("AC ocupacion AE");
            tablaResultado.Columns.Add("CANT AE");
            tablaResultado.Columns.Add("AC tiempo espera P");
            tablaResultado.Columns.Add("AC ocupacion P");
            tablaResultado.Columns.Add("CANT P");

        }

        public static void agregarClienteTabla(FilaVectorPunto3 fila2, DataTable tablaResultado, Cliente cliente)
        {
            bool fueEliminado = false;
            foreach (Cliente Cli in fila2.Clientes)
            {
                if (Cli.Estado == "Eliminado")
                {
                    Cli.Estado = cliente.Estado;
                    Cli.HoraInicioEspera = cliente.HoraInicioEspera;
                    Cli.HoraInicioAtencion = cliente.HoraInicioAtencion;
                    fila2.Clientes.Remove(cliente);
                    fueEliminado = true;
                    break;
                }
            }
            if (fueEliminado == false)
            {
                DataGridViewColumn columnaEstado = new DataGridViewTextBoxColumn();
                columnaEstado.HeaderText = "Estado" + (fila2.Clientes.Count);
                DataGridViewColumn columnaHoraInicioEspera = new DataGridViewTextBoxColumn();
                columnaHoraInicioEspera.HeaderText = "HEspera" + (fila2.Clientes.Count);
                DataGridViewColumn columnaHoraInicioAtencion = new DataGridViewTextBoxColumn();
                columnaHoraInicioAtencion.HeaderText = "HAtención" + (fila2.Clientes.Count);

                tablaResultado.Columns.Add("Estado " + (fila2.Clientes.Count));
                tablaResultado.Columns.Add("HEspera " + (fila2.Clientes.Count));
                tablaResultado.Columns.Add("HAtencion " + (fila2.Clientes.Count));
            }

        }

       
    }

}
