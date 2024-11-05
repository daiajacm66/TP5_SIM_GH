using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;
using TP4_SIM.Clases;
using TP4_SIM.Clases.RK;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TP4_SIM
{
    public partial class Simulacion : Form
    {
        public Simulacion()
        {
            InitializeComponent();
        }

        private void btnIniciarSimulacion_Click(object sender, EventArgs e)
        {
            
            int cantFilas = int.Parse(txtCantFilas.Text);
            // Valores de media (dist. exponencial) de los eventos 
            double mediaLlegadaPaquete = double.Parse(txtLlegadaPaquete.Text);
            double mediaFinPaquete = double.Parse(txtFinPaquete.Text);
            double mediaLlegadaReclamo = double.Parse(txtLlegadaReclamo.Text);
            double mediaFinReclamo = double.Parse(txtFinReclamo.Text);
            double mediaLlegadaVenta = double.Parse(txtLlegadaVenta.Text);
            double mediaFinVenta = double.Parse(txtFinVenta.Text);
            double mediaLlegadaAE = double.Parse(txtLlegadaAtencion.Text);
            double mediaFinAE = double.Parse(txtFinAtencion.Text);
            double mediaLlegadaPostales = double.Parse(txtLlegadaPostales.Text);
            double mediaFinPostales = double.Parse(txtFinPostales.Text);
            bool conCorte = false;

            if (rbCorteLuz.Checked)
            {
                conCorte = true;
            }

            
            double t = 8;

            // Nro de fila a partir de la que sea desea visualizar
            int nroPrimeraFilaMostrar = int.Parse(txtPrimeraFila.Text);
            int nroUltimaFilaMostrar = nroPrimeraFilaMostrar + 300;
            if (nroUltimaFilaMostrar > cantFilas) { nroUltimaFilaMostrar = cantFilas; }

            string[] nombresEventos = {"llegada_envio", "llegada_reclamo", "llegada_venta", "llegada_AE", "llegada_postales","llegada_corte_postal",
                "fin_envio1","fin_envio2", "fin_envio3", "fin_reclamo1", "fin_reclamo2", "fin_venta1", "fin_venta2", "fin_venta3",
                "fin_AE1", "fin_AE2", "fin_postales" , "fin_enfriamiento" };


            Random random = new Random();

            // Se completa la primera fila de inicialización 
            FilaVector fila1 = new FilaVector();
            fila1.NroFila = 1;
            fila1.Evento = "Inicializacion";
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
            if (rbCorteLuz.Checked)
            {
                fila1.LlegadaCorteLuz.Rnd = generarRandom(random);
                calcularProbabilidad(fila1, t);
            }
            

            // Se ponen todos los tiempos de los prox eventos en una lista para madarlos al método que busca al siguiente
            List<double> posiblesProxEventos = new List<double>();
            posiblesProxEventos.Add(fila1.LlegadaEnvio.ProxLlegada);
            posiblesProxEventos.Add(fila1.LlegadaReclamo.ProxLlegada);
            posiblesProxEventos.Add(fila1.LlegadaVenta.ProxLlegada);
            posiblesProxEventos.Add(fila1.LlegadaAE.ProxLlegada);
            posiblesProxEventos.Add(fila1.LlegadaPostales.ProxLlegada);
            if (rbCorteLuz.Checked)
            {
                posiblesProxEventos.Add(fila1.LlegadaCorteLuz.ProxLlegada);
            }

        List<double> proxEvento = buscarProxEvento(posiblesProxEventos);

            FilaVector fila2 = new FilaVector();
            fila2.Reloj = (double)proxEvento[0];
            fila2.Evento = nombresEventos[Convert.ToInt32(proxEvento[1])];

            List<FilaVector> filasMostrar = new List<FilaVector>();
            DataTable tablaResultado = new DataTable();
            agregarColumnasTabla(tablaResultado, conCorte);

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
                    if (conCorte)
                    {
                        listaAux1.Add(fila1.LlegadaCorteLuz.Rnd.ToString());
                        listaAux1.Add(fila1.LlegadaCorteLuz.ProxLlegada.ToString());
                    }
                    listaAux1.Add(fila1.Fin_envio.Rnd.ToString());
                    listaAux1.Add(fila1.Fin_envio.TiempoAtencion.ToString());
                    listaAux1.Add(fila1.Fin_envio.FinAtencion[0].ToString());
                    listaAux1.Add(fila1.Fin_envio.FinAtencion[1].ToString());
                    listaAux1.Add(fila1.Fin_envio.FinAtencion[2].ToString());
                    listaAux1.Add(fila1.Fin_reclamo.Rnd.ToString());
                    listaAux1.Add(fila1.Fin_reclamo.TiempoAtencion.ToString());
                    listaAux1.Add(fila1.Fin_reclamo.FinAtencion[0].ToString());
                    listaAux1.Add(fila1.Fin_reclamo.FinAtencion[1].ToString());
                    listaAux1.Add(fila1.Fin_venta.Rnd.ToString());
                    listaAux1.Add(fila1.Fin_venta.TiempoAtencion.ToString());
                    listaAux1.Add(fila1.Fin_venta.FinAtencion[0].ToString());
                    listaAux1.Add(fila1.Fin_venta.FinAtencion[1].ToString());
                    listaAux1.Add(fila1.Fin_venta.FinAtencion[2].ToString());
                    listaAux1.Add(fila1.Fin_AE.Rnd.ToString());
                    listaAux1.Add(fila1.Fin_AE.TiempoAtencion.ToString());
                    listaAux1.Add(fila1.Fin_AE.FinAtencion[0].ToString());
                    listaAux1.Add(fila1.Fin_AE.FinAtencion[1].ToString());
                    listaAux1.Add(fila1.Fin_postales.Rnd.ToString());
                    listaAux1.Add(fila1.Fin_postales.TiempoAtencion.ToString());
                    listaAux1.Add(fila1.Fin_postales.FinAtencion.ToString());
                    if (conCorte)
                    { 
                        listaAux1.Add(fila1.FinCorteLuz.T.ToString());
                        listaAux1.Add(fila1.FinCorteLuz.FinAtencion.ToString());
                    }
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
                    listaAux1.Add(fila1.Ventas[0].Cola.ToString());
                    listaAux1.Add(fila1.Ventas[0].Estado.ToString());
                    listaAux1.Add(fila1.Ventas[1].Cola.ToString());
                    listaAux1.Add(fila1.Ventas[1].Estado.ToString());
                    listaAux1.Add(fila1.Ventas[2].Cola.ToString());
                    listaAux1.Add(fila1.Ventas[2].Estado.ToString());
                    listaAux1.Add(fila1.AtencionEmp[0].Cola.ToString());
                    listaAux1.Add(fila1.AtencionEmp[0].Estado.ToString());
                    listaAux1.Add(fila1.AtencionEmp[1].Cola.ToString());
                    listaAux1.Add(fila1.AtencionEmp[1].Estado.ToString());
                    listaAux1.Add(fila1.Postales[0].Cola.ToString());
                    listaAux1.Add(fila1.Postales[0].Estado.ToString());
                    if (conCorte)
                    {
                        listaAux1.Add(fila1.Postales[0].Tiempo_Remanente.ToString());
                    }
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
                        if (Cli.Estado != "Eliminado")
                        {
                            listaAux1.Add(Cli.Estado);
                            listaAux1.Add(Cli.HoraInicioEspera.ToString());
                            listaAux1.Add(Cli.HoraInicioAtencion.ToString());
                        }
                        else
                        {
                            Cli.Estado = "Eliminado";
                            Cli.HoraInicioEspera = -1;
                            Cli.HoraInicioAtencion = -1;

                            listaAux1.Add("Eliminado");
                            listaAux1.Add("-");
                            listaAux1.Add("-");
                        }
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

                    // Como es una llegada tengo que agregar el cliente
                    fila2.Clientes.Add(cliente);
                    agregarClienteTabla(fila2, tablaResultado, cliente);
                    //grdSimulacion.Rows[fila2.NroFila - 1].Cells[columnaEstado.Index].Value = cliente.Estado.ToString();
                    //grdSimulacion.Rows[fila2.NroFila - 1].Cells[columnaHoraInicioEspera.Index].Value = cliente.HoraInicioEspera.ToString();
                }

                // LLEGADA VENTA
                if (fila2.Evento == "llegada_venta")
                {
                    copiarProximasLlegadas(fila1, fila2);

                    // Genero la prox. llegada_venta
                    fila2.LlegadaVenta.Rnd = generarRandom(random);
                    fila2.LlegadaVenta.TiempoEntreLlegadas = calcularTiempo(mediaLlegadaVenta, fila2.LlegadaVenta.Rnd);
                    fila2.LlegadaVenta.ProxLlegada = fila2.Reloj + fila2.LlegadaVenta.TiempoEntreLlegadas;

                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);

                    // Tengo que revisar las colas y los estados de VENTAS con el for
                    Cliente cliente = new Cliente();


                    //int cantColumnas = grdSimulacion.ColumnCount;
                    //DataGridViewColumn columnaEstado = new DataGridViewTextBoxColumn();
                    //columnaEstado.HeaderText = "Estado" + (fila2.Clientes.Count + 1);
                    //DataGridViewColumn columnaHoraInicioEspera = new DataGridViewTextBoxColumn();
                    //columnaHoraInicioEspera.HeaderText = "HEspera" + (fila2.Clientes.Count + 1);
                    //DataGridViewColumn columnaHoraInicioAtencion = new DataGridViewTextBoxColumn();
                    //columnaHoraInicioAtencion.HeaderText = "HAtención" + (fila2.Clientes.Count + 1);

                    //agregarColumnasClientes(fila2, tablaResultado);

                    //grdSimulacion.Columns.Add(columnaEstado);
                    //grdSimulacion.Columns.Add(columnaHoraInicioEspera);
                    //grdSimulacion.Columns.Add(columnaHoraInicioAtencion);

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
                        if (fila1.Ventas[0].Cola == fila1.Ventas[1].Cola && fila1.Ventas[0].Cola == fila1.Ventas[2].Cola)
                        {
                            // Si todos los objetos tienen la misma cola, espera en el objeto 1:
                            cliente.Estado = "EV1";
                            fila2.Ventas[0].Cola = fila1.Ventas[0].Cola + 1;
                        }

                        else if (fila1.Ventas[0].Cola < fila1.Ventas[1].Cola && fila1.Ventas[0].Cola < fila1.Ventas[2].Cola)
                        {
                            cliente.Estado = "EV1";
                            fila2.Ventas[0].Cola = fila1.Ventas[0].Cola + 1;
                        }
                        else if (fila1.Ventas[1].Cola < fila1.Ventas[0].Cola && fila1.Ventas[1].Cola < fila1.Ventas[2].Cola)
                        {
                            cliente.Estado = "EV2";
                            fila2.Ventas[1].Cola = fila1.Ventas[1].Cola + 1;
                        }
                        else if (fila1.Ventas[2].Cola < fila1.Ventas[0].Cola && fila1.Ventas[2].Cola < fila1.Ventas[1].Cola)
                        {
                            cliente.Estado = "EV3";
                            fila2.Ventas[2].Cola = fila1.Ventas[2].Cola + 1;
                        }
                        // Si dos colas son iguales y una diferente
                        else if (fila1.Ventas[0].Cola == fila1.Ventas[1].Cola && fila1.Ventas[0].Cola < fila1.Ventas[2].Cola)
                        {
                            // Las colas 1 y 2 son iguales y menores que la 3
                            cliente.Estado = "EV1";
                            fila2.Ventas[0].Cola = fila1.Ventas[0].Cola + 1;
                        }
                        else if (fila1.Ventas[0].Cola == fila1.Ventas[1].Cola && fila1.Ventas[0].Cola > fila1.Ventas[2].Cola)
                        {
                            // Las colas 1 y 2 son iguales y mayores que la 3
                            cliente.Estado = "EV3";
                            fila2.Ventas[2].Cola = fila1.Ventas[2].Cola + 1;
                        }

                        else if (fila1.Ventas[0].Cola == fila1.Ventas[2].Cola && fila1.Ventas[0].Cola < fila1.Ventas[1].Cola)
                        {
                            // Las colas  1 y 3 son iguales y menores que la 2
                            cliente.Estado = "EV1";
                            fila2.Ventas[0].Cola = fila1.Ventas[0].Cola + 1;
                        }
                        else if (fila1.Ventas[0].Cola == fila1.Ventas[2].Cola && fila1.Ventas[0].Cola > fila1.Ventas[1].Cola)
                        {
                            // Las colas  1 y 3 son iguales y mayores que la 2
                            cliente.Estado = "EV2";
                            fila2.Ventas[1].Cola = fila1.Ventas[1].Cola + 1;
                        }
                        else if (fila1.Ventas[1].Cola == fila1.Ventas[2].Cola && fila1.Ventas[1].Cola < fila1.Ventas[0].Cola)
                        {
                            // Las colas 2 y 3 son iguales y menores que la 1
                            cliente.Estado = "EV2";
                            fila2.Ventas[1].Cola = fila1.Ventas[1].Cola + 1;
                        }
                        else if (fila1.Ventas[1].Cola == fila1.Ventas[2].Cola && fila1.Ventas[1].Cola > fila1.Ventas[0].Cola)
                        {
                            // Las colas 2 y 3 son iguales y mayores que la 1
                            cliente.Estado = "EV1";
                            fila2.Ventas[0].Cola = fila1.Ventas[0].Cola + 1;
                        }

                    }

                    // Como es una llegada tengo que agregar el cliente
                    fila2.Clientes.Add(cliente);
                    agregarClienteTabla(fila2, tablaResultado, cliente);
                    //grdSimulacion.Rows[i + 1].Cells[columnaEstado.Index].Value = cliente.Estado.ToString();
                    //grdSimulacion.Rows[i + 1].Cells[columnaHoraInicioEspera.Index].Value = cliente.HoraInicioEspera.ToString();

                }

                // LLEGADA RECLAMO
                if (fila2.Evento == "llegada_reclamo")
                {
                    copiarProximasLlegadas(fila1, fila2);

                    // Genero la prox. llegada_reclamo
                    fila2.LlegadaReclamo.Rnd = generarRandom(random);
                    fila2.LlegadaReclamo.TiempoEntreLlegadas = calcularTiempo(mediaLlegadaReclamo, fila2.LlegadaReclamo.Rnd);
                    fila2.LlegadaReclamo.ProxLlegada = fila2.Reloj + fila2.LlegadaReclamo.TiempoEntreLlegadas;

                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);

                    // Tengo que revisar las colas y los estados de VENTAS con el for
                    Cliente cliente = new Cliente();


                    //int cantColumnas = grdSimulacion.ColumnCount;
                    //DataGridViewColumn columnaEstado = new DataGridViewTextBoxColumn();
                    //columnaEstado.HeaderText = "Estado" + (fila2.Clientes.Count + 1);
                    //DataGridViewColumn columnaHoraInicioEspera = new DataGridViewTextBoxColumn();
                    //columnaHoraInicioEspera.HeaderText = "HEspera" + (fila2.Clientes.Count + 1);
                    //DataGridViewColumn columnaHoraInicioAtencion = new DataGridViewTextBoxColumn();
                    //columnaHoraInicioAtencion.HeaderText = "HAtención" + (fila2.Clientes.Count + 1);

                    //agregarColumnasClientes(fila2, tablaResultado);

                    //grdSimulacion.Columns.Add(columnaEstado);
                    //grdSimulacion.Columns.Add(columnaHoraInicioEspera);
                    //grdSimulacion.Columns.Add(columnaHoraInicioAtencion);


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
                        // Si ninguna está libre, deberá esperar en la cola más corta
                        if (fila1.Reclamos[0].Cola == fila1.Reclamos[1].Cola)
                        {
                            cliente.Estado = "ER1";
                            fila2.Reclamos[0].Cola = fila1.Reclamos[0].Cola + 1;
                        }
                        else if (fila1.Reclamos[0].Cola < fila1.Reclamos[1].Cola)
                        {
                            cliente.Estado = "ER1";
                            fila2.Reclamos[0].Cola = fila1.Reclamos[0].Cola + 1;
                        }
                        else
                        {
                            cliente.Estado = "ER2";
                            fila2.Reclamos[1].Cola = fila1.Reclamos[1].Cola + 1;
                        }
                    }

                    // Como es una llegada tengo que agregar el cliente
                    fila2.Clientes.Add(cliente);
                    agregarClienteTabla(fila2, tablaResultado, cliente);
                    //grdSimulacion.Rows[fila2.NroFila - 1].Cells[columnaEstado.Index].Value = cliente.Estado.ToString();
                    //grdSimulacion.Rows[fila2.NroFila - 1].Cells[columnaHoraInicioEspera.Index].Value = cliente.HoraInicioEspera.ToString();
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


                    //int cantColumnas = grdSimulacion.ColumnCount;
                    //DataGridViewColumn columnaEstado = new DataGridViewTextBoxColumn();
                    //columnaEstado.HeaderText = "Estado" + (fila2.Clientes.Count + 1);
                    //DataGridViewColumn columnaHoraInicioEspera = new DataGridViewTextBoxColumn();
                    //columnaHoraInicioEspera.HeaderText = "HEspera" + (fila2.Clientes.Count + 1);
                    //DataGridViewColumn columnaHoraInicioAtencion = new DataGridViewTextBoxColumn();
                    //columnaHoraInicioAtencion.HeaderText = "HAtención" + (fila2.Clientes.Count + 1);

                    //agregarColumnasClientes(fila2, tablaResultado);

                    //grdSimulacion.Columns.Add(columnaEstado);
                    //grdSimulacion.Columns.Add(columnaHoraInicioEspera);
                    //grdSimulacion.Columns.Add(columnaHoraInicioAtencion);


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
                    //grdSimulacion.Rows[i+1].Cells[columnaEstado.Index].Value = cliente.Estado.ToString();
                    //grdSimulacion.Rows[i+1].Cells[columnaHoraInicioEspera.Index].Value = cliente.HoraInicioEspera.ToString();
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
                    // Como es una llegada tengo que agregar el cliente
                    fila2.Clientes.Add(cliente);
                    agregarClienteTabla(fila2, tablaResultado, cliente);
                    //grdSimulacion.Rows[fila2.NroFila - 1].Cells[columnaEstado.Index].Value = cliente.Estado.ToString();
                    //grdSimulacion.Rows[fila2.NroFila - 1].Cells[columnaHoraInicioEspera.Index].Value = cliente.HoraInicioEspera.ToString();

                }
                // LLEGADA CORTE LUZ - simulamos para postales
                if (fila2.Evento == "llegada_corte_postal")
                {
                    copiarProximasLlegadas(fila1, fila2);
                    // Genero la prox. llegada_corte
                    fila2.LlegadaCorteLuz.Rnd = generarRandom(random);
                    calcularProbabilidad(fila2, t);

                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);


                    // suspender al que estaban atendiendo
                    int indice = buscarClienteSuspendido("SP", fila1.Clientes);
                    if (indice != -1)
                    {
                        fila2.Clientes[indice].Estado = "SS";
                        // guardar cuanto le queda
                        double tiempo_rem = fila2.Fin_postales.FinAtencion - fila2.Reloj;
                        fila2.Postales[0].Tiempo_Remanente = tiempo_rem;
                        fila2.Fin_postales.FinAtencion = 0;

                    }
                    
                    // calcular tiempo enfriamiento
                    (double tiempo, double tiempo_enf) = RK(fila2);
                    fila2.FinCorteLuz.FinAtencion = tiempo_enf;
                    fila2.FinCorteLuz.T = tiempo;

                    fila2.Postales[0].Estado = "Interrumpido";

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
                if (fila2.Evento == "fin_venta3")
                {
                    copiarProximasLlegadas(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);

                    calcularFinAtencionVenta(3, fila1, fila2, random, mediaFinVenta);
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

                // FIN CORTE LUZ

                if (fila2.Evento == "fin_enfriamiento")
                {
                    copiarProximasLlegadas(fila1, fila2);
                    copiarFinesAtencion(fila1, fila2);
                    copiarObjetosFilaAnterior(fila1, fila2);
                    copiarEstadisticas(fila1, fila2);

                    calcularFinEnfriamiento(fila1, fila2, random, mediaFinPostales);
                }
                

                List<double> posiblesProximosEventos = new List<double>();
                posiblesProximosEventos.Add(fila2.LlegadaEnvio.ProxLlegada); // 0 
                posiblesProximosEventos.Add(fila2.LlegadaReclamo.ProxLlegada); // 1
                posiblesProximosEventos.Add(fila2.LlegadaVenta.ProxLlegada); // 2
                posiblesProximosEventos.Add(fila2.LlegadaAE.ProxLlegada); // 3
                posiblesProximosEventos.Add(fila2.LlegadaPostales.ProxLlegada); //4
                posiblesProximosEventos.Add((double)fila2.LlegadaCorteLuz.ProxLlegada);
                posiblesProximosEventos.Add((double)fila2.Fin_envio.FinAtencion[0]);
                posiblesProximosEventos.Add((double)fila2.Fin_envio.FinAtencion[1]);
                posiblesProximosEventos.Add((double)fila2.Fin_envio.FinAtencion[2]);
                posiblesProximosEventos.Add((double)fila2.Fin_reclamo.FinAtencion[0]);
                posiblesProximosEventos.Add((double)fila2.Fin_reclamo.FinAtencion[1]);
                posiblesProximosEventos.Add((double)fila2.Fin_venta.FinAtencion[0]);
                posiblesProximosEventos.Add((double)fila2.Fin_venta.FinAtencion[1]);
                posiblesProximosEventos.Add((double)fila2.Fin_venta.FinAtencion[2]);
                posiblesProximosEventos.Add((double)fila2.Fin_AE.FinAtencion[0]);
                posiblesProximosEventos.Add((double)fila2.Fin_AE.FinAtencion[1]);
                posiblesProximosEventos.Add((double)fila2.Fin_postales.FinAtencion);
                posiblesProximosEventos.Add((double)fila2.FinCorteLuz.FinAtencion);


                List<double> proximoEvento = buscarProxEvento(posiblesProximosEventos);

                filasMostrar.Add(fila1);
                fila1 = fila2;

                if (!(i == cantFilas - 1))
                {
                    fila2 = new FilaVector();
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
                    if (conCorte)
                    {
                        listaAux.Add(fila2.LlegadaCorteLuz.Rnd.ToString());
                        listaAux.Add(fila2.LlegadaCorteLuz.ProxLlegada.ToString());
                    }
                    listaAux.Add(fila2.Fin_envio.Rnd.ToString());
                    listaAux.Add(fila2.Fin_envio.TiempoAtencion.ToString());
                    listaAux.Add(fila2.Fin_envio.FinAtencion[0].ToString());
                    listaAux.Add(fila2.Fin_envio.FinAtencion[1].ToString());
                    listaAux.Add(fila2.Fin_envio.FinAtencion[2].ToString());
                    listaAux.Add(fila2.Fin_reclamo.Rnd.ToString());
                    listaAux.Add(fila2.Fin_reclamo.TiempoAtencion.ToString());
                    listaAux.Add(fila2.Fin_reclamo.FinAtencion[0].ToString());
                    listaAux.Add(fila2.Fin_reclamo.FinAtencion[1].ToString());
                    listaAux.Add(fila2.Fin_venta.Rnd.ToString());
                    listaAux.Add(fila2.Fin_venta.TiempoAtencion.ToString());
                    listaAux.Add(fila2.Fin_venta.FinAtencion[0].ToString());
                    listaAux.Add(fila2.Fin_venta.FinAtencion[1].ToString());
                    listaAux.Add(fila2.Fin_venta.FinAtencion[2].ToString());
                    listaAux.Add(fila2.Fin_AE.Rnd.ToString());
                    listaAux.Add(fila2.Fin_AE.TiempoAtencion.ToString());
                    listaAux.Add(fila2.Fin_AE.FinAtencion[0].ToString());
                    listaAux.Add(fila2.Fin_AE.FinAtencion[1].ToString());
                    listaAux.Add(fila2.Fin_postales.Rnd.ToString());
                    listaAux.Add(fila2.Fin_postales.TiempoAtencion.ToString());
                    listaAux.Add(fila2.Fin_postales.FinAtencion.ToString());

                    if (conCorte)
                    {
                        listaAux.Add(fila2.FinCorteLuz.T.ToString());
                        listaAux.Add(fila2.FinCorteLuz.FinAtencion.ToString());
                    }

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
                    listaAux.Add(fila2.Ventas[0].Cola.ToString());
                    listaAux.Add(fila2.Ventas[0].Estado.ToString());
                    listaAux.Add(fila2.Ventas[1].Cola.ToString());
                    listaAux.Add(fila2.Ventas[1].Estado.ToString());
                    listaAux.Add(fila2.Ventas[2].Cola.ToString());
                    listaAux.Add(fila2.Ventas[2].Estado.ToString());
                    listaAux.Add(fila2.AtencionEmp[0].Cola.ToString());
                    listaAux.Add(fila2.AtencionEmp[0].Estado.ToString());
                    listaAux.Add(fila2.AtencionEmp[1].Cola.ToString());
                    listaAux.Add(fila2.AtencionEmp[1].Estado.ToString());
                    listaAux.Add(fila2.Postales[0].Cola.ToString());
                    listaAux.Add(fila2.Postales[0].Estado.ToString());
                    if (conCorte)
                    {
                        listaAux.Add(fila2.Postales[0].Tiempo_Remanente.ToString());
                    }
                    
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
                        if (Cli.Estado != "Eliminado")
                        {
                            listaAux.Add(Cli.Estado);
                            listaAux.Add(Cli.HoraInicioEspera.ToString());
                            listaAux.Add(Cli.HoraInicioAtencion.ToString());
                        }
                        else
                        {
                            Cli.Estado = "Eliminado";
                            Cli.HoraInicioEspera = -1;
                            Cli.HoraInicioAtencion = -1;
                            listaAux.Add("Eliminado");
                            listaAux.Add("-");
                            listaAux.Add("-");
                        }
                    }

                    //filasMostrar.Add(fila2);

                    tablaResultado.Rows.Add(listaAux.ToArray());

                    //tengo que mostrar la útlima fila siempre
                    //grdSimulacion.Rows.Add(listaAux.ToArray()); // me falta agregar las columnas de ocupación en el data grid view
                }

            }
            CargarCsv(tablaResultado, "Sim.csv");
            cargarResultadosNormal(fila2);
            
            if (rbCorteLuz.Checked == true)
            {
                (double tiempo, double vueltaLuz) = RK(fila2);
                Console.WriteLine("vueltaLuz: " + vueltaLuz);
                rbCorteLuz.Checked = false;
                
            }
            
        }

        public TablaRK Tabla { get; set; }
        double TiempoEnfriamiento;
        double VueltaLuz;
        public (double, double) RK(FilaVector fila2)
        {
            double c0 = fila2.Reloj;
            double yf = 0;

            Tabla = RungeKutta.IntegracionNumerica(0.1, 0, c0, yf);
            TiempoEnfriamiento = (Tabla.ResultadoRK * 30) / 60;
            VueltaLuz = TiempoEnfriamiento + fila2.Reloj;

            GestorColasRK gestorRK = new GestorColasRK();
            gestorRK.T = c0;
            gestorRK.TablasRK.Add(Tabla);
            CargarRK(gestorRK.TablasRK, gestorRK.T);;
            DataTable TablaAux = Tabla.ToDataTable();

            ///CsvConverter.DataTableToCsv(TablaAux, "tp5rk.csv", true);

            return (Tabla.ResultadoRK, VueltaLuz);
        }
        private void CargarRK(List<TablaRK> tablasRK, double c)
        {
            // creamos el control de resultados
            var res = new RungeKuttaControl(tablasRK, c);

            FormularioConControl formulario = new FormularioConControl(res);
            formulario.Show();

        }
        
        public void cargarResultadosNormal(FilaVector fila2)
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

             // Falta verificar cuando la cantidad es 0 -> no se puede calcular (da NaN)
            txtEsperaE.Text = (Math.Truncate((vectorRes[0] / vectorRes[1]) * 100) / 100).ToString();
            txtEsperaR.Text = (Math.Truncate((vectorRes[2] / vectorRes[3]) * 100) / 100).ToString();
            txtEsperaV.Text = (Math.Truncate((vectorRes[4] / vectorRes[5]) * 100) / 100).ToString();
            txtEsperaAE.Text = (Math.Truncate((vectorRes[6] / vectorRes[7]) * 100) / 100).ToString();
            txtEsperaP.Text = (Math.Truncate((vectorRes[8] / vectorRes[9]) * 100) / 100).ToString();

            relojFinal = vectorRes[15];

            double ocupE = (vectorRes[10] / 3) * 100 / relojFinal;
            double ocupR = (vectorRes[11] / 3) * 100 / relojFinal;
            double ocupV = (vectorRes[12] / 3) * 100 / relojFinal;
            double ocupAE = (vectorRes[13] / 3) * 100 / relojFinal;
            double ocupP = (vectorRes[14] / 3) * 100 / relojFinal;

            txtOcupacionE.Text = (Math.Truncate(ocupE * 100) / 100).ToString() + "%";
            txtOcupacionR.Text = (Math.Truncate(ocupR * 100) / 100).ToString() + "%";
            txtOcupacionV.Text = (Math.Truncate(ocupV * 100) / 100).ToString() + "%";
            txtOcupacionAE.Text = (Math.Truncate(ocupAE * 100) / 100).ToString() + "%";
            txtOcupacionP.Text = (Math.Truncate(ocupP * 100) / 100).ToString() + "%";


            
            txtPromedioAtencionE.Text = (Math.Round((acOcupE/cantE), 2)).ToString();
            txtPromedioAtencionR.Text = (Math.Round((acOcupR / cantR), 2)).ToString();
            txtPromedioAtencionV.Text = (Math.Round((acOcupV / cantV), 2)).ToString();
            txtPromedioAtencionAE.Text = (Math.Round((acOcupAE / cantAE), 2)).ToString();
            txtPromedioAtencionP.Text = (Math.Round((acOcupP / cantP), 2)).ToString();

            txtTotalClientes.Text = (cantE + cantR + cantV + cantAE + cantP).ToString();

            double cantHoras = Math.Truncate(relojFinal / 60);

            txtCantXHoraE.Text = (Math.Truncate(cantE/cantHoras)).ToString();
            txtCantXHoraR.Text = (Math.Truncate(cantR / cantHoras)).ToString();
            txtCantXHoraV.Text = (Math.Truncate(cantV / cantHoras)).ToString();
            txtCantXHoraAE.Text = (Math.Truncate(cantAE / cantHoras)).ToString();
            txtCantXHoraP.Text = (Math.Truncate(cantP / cantHoras)).ToString();

            
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
        private int buscarClienteSuspendido(string estado, List<Cliente> clientes)
        {
            for (int i = 0; i < clientes.Count; i++)
            {
                if (clientes[i].Estado == estado)
                {
                    return i;
                }
            }
            return -1;
            //throw new InvalidOperationException($"No se encontró un cliente con el estado: {estado}");
        }


        private void copiarObjetosFilaAnterior(FilaVector fila1, FilaVector fila2)
        {
            fila2.EnvioPaquetes = fila1.EnvioPaquetes;
            fila2.Reclamos = fila1.Reclamos;
            fila2.Ventas = fila1.Ventas;
            fila2.AtencionEmp = fila1.AtencionEmp;
            fila2.Postales = fila1.Postales;


            fila2.Clientes = fila1.Clientes;
        }

        private void copiarFinesAtencion(FilaVector fila1, FilaVector fila2)
        {
            fila2.Fin_envio.FinAtencion = fila1.Fin_envio.FinAtencion;
            fila2.Fin_reclamo.FinAtencion = fila1.Fin_reclamo.FinAtencion;
            fila2.Fin_venta.FinAtencion = fila1.Fin_venta.FinAtencion;
            fila2.Fin_AE.FinAtencion = fila1.Fin_AE.FinAtencion;
            fila2.Fin_postales.FinAtencion = fila1.Fin_postales.FinAtencion;
            fila2.FinCorteLuz.FinAtencion = fila1.FinCorteLuz.FinAtencion;
        }

        private void copiarProximasLlegadas(FilaVector fila1, FilaVector fila2)
        {
            fila2.LlegadaReclamo.ProxLlegada = fila1.LlegadaReclamo.ProxLlegada;
            fila2.LlegadaVenta.ProxLlegada = fila1.LlegadaVenta.ProxLlegada;
            fila2.LlegadaAE.ProxLlegada = fila1.LlegadaAE.ProxLlegada;
            fila2.LlegadaPostales.ProxLlegada = fila1.LlegadaPostales.ProxLlegada;
            fila2.LlegadaEnvio.ProxLlegada = fila1.LlegadaEnvio.ProxLlegada;
            fila2.LlegadaCorteLuz.ProxLlegada = fila1.LlegadaCorteLuz.ProxLlegada;
        }

        private void copiarEstadisticas(FilaVector fila1, FilaVector fila2)
        {
            fila2.EstadisticasEnvio = fila1.EstadisticasEnvio;
            fila2.EstadisticasReclamo = fila1.EstadisticasReclamo;
            fila2.EstadisticasVenta = fila1.EstadisticasVenta;
            fila2.EstadisticasAE = fila1.EstadisticasAE;
            fila2.EstadisticasPostales = fila1.EstadisticasPostales;
        }

        private void calcularFinAtencionEnvio(int nroObjetoEnvio, FilaVector fila1, FilaVector fila2, Random random, double media)
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

        private void calcularFinAtencionReclamo(int nroObjetoReclamo, FilaVector fila1, FilaVector fila2, Random random, double media)
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

        private void calcularFinAtencionVenta(int nroObjetoVenta, FilaVector fila1, FilaVector fila2, Random random, double media)
        {
            string estadoSiendoAtendido = "SV" + nroObjetoVenta;


            int indice = nroObjetoVenta - 1;

            int indexClienteAtendido = buscarClientePorEstado(estadoSiendoAtendido, fila1.Clientes);

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

        private void calcularFinAtencionEmp(int nroObjetoAE, FilaVector fila1, FilaVector fila2, Random random, double media)
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
        private void calcularFinAtencionPostales(FilaVector fila1, FilaVector fila2, Random random, double media)
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
        private void calcularFinEnfriamiento(FilaVector fila1, FilaVector fila2, Random random, double media)
        {
            // si hay uno suspendido, lo debe atender primero, antes que los de la cola
            // SS siendo suspendido

            int indexClienteAtendido = buscarClienteSuspendido("SS", fila1.Clientes);
            if (indexClienteAtendido == -1)
            {
                if (fila1.Postales[0].Cola != 0)
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
                else
                {
                    fila2.Postales[0].Estado = "Libre";
                    fila2.Fin_postales.FinAtencion = 0;
                }
                    
            }
            else
            {
                fila2.Clientes[indexClienteAtendido].Estado = "SP";
                fila2.Postales[0].Estado = "Ocupado";
                fila2.Fin_postales.FinAtencion = fila2.Postales[0].Tiempo_Remanente + fila2.Reloj;
                fila2.EstadisticasPostales.AcumuladorEspera = truncarNumero(fila2.Reloj - fila2.Clientes[indexClienteAtendido].HoraInicioEspera) + fila1.EstadisticasPostales.AcumuladorEspera;
                fila2.Clientes[indexClienteAtendido].HoraInicioEspera = 0;
                
            }
            fila2.FinCorteLuz.FinAtencion = 0;
            
        }


        // SIMULACIÓN CON NUEVO SERVICIO -------------------------------------------------------------------------------------------------------------------------------

        private void btnNuevoServicio_Click(object sender, EventArgs e)
        {
            int cantFilas = int.Parse(txtCantFilas.Text);
            // Valores de media (dist. exponencial) de los eventos 
            double mediaLlegadaPaquete = double.Parse(txtLlegadaPaquete.Text);
            double mediaFinPaquete = double.Parse(txtFinPaquete.Text);
            double mediaLlegadaReclamo = double.Parse(txtLlegadaReclamo.Text);
            double mediaFinReclamo = double.Parse(txtFinReclamo.Text);
            double mediaLlegadaVenta = double.Parse(txtLlegadaVenta.Text);
            double mediaFinVenta = double.Parse(txtFinVenta.Text);
            double mediaLlegadaAE = double.Parse(txtLlegadaAtencion.Text);
            double mediaFinAE = double.Parse(txtFinAtencion.Text);
            double mediaLlegadaPostales = double.Parse(txtLlegadaPostales.Text);
            double mediaFinPostales = double.Parse(txtFinPostales.Text);

            // Nro de fila a partir de la que sea desea visualizar
            int nroPrimeraFilaMostrar = int.Parse(txtPrimeraFila.Text);
            int nroUltimaFilaMostrar = nroPrimeraFilaMostrar + 300;
            if (nroUltimaFilaMostrar > cantFilas) { nroUltimaFilaMostrar = cantFilas; }


            double[] medias = {mediaLlegadaPaquete, mediaFinPaquete, mediaLlegadaReclamo, mediaFinReclamo, mediaLlegadaVenta, mediaFinVenta,
            mediaLlegadaAE, mediaFinAE, mediaLlegadaPostales, mediaFinPostales};

            SimNuevoServicio ventana = new SimNuevoServicio(cantFilas, medias, nroPrimeraFilaMostrar, nroUltimaFilaMostrar);
            ventana.Show();

        }
        public static void CargarCsv(DataTable Tabla, string name)
        {
            CsvConverter.DataTableToCsv(Tabla, name, true);

        }
        public static void agregarColumnasTabla(DataTable tablaResultado, bool conCorte)
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

            if (conCorte)
            {
                tablaResultado.Columns.Add("RND Corte");
                tablaResultado.Columns.Add("Proximo corte");
            }

            tablaResultado.Columns.Add("RND fin EP");
            tablaResultado.Columns.Add("Tiempo atencion EP");
            tablaResultado.Columns.Add("Tiempo fin envio 1");
            tablaResultado.Columns.Add("Tiempo fin envio 2");
            tablaResultado.Columns.Add("Tiempo fin envio 3");
            tablaResultado.Columns.Add("RND fin R");
            tablaResultado.Columns.Add("Tiempo atencion R");
            tablaResultado.Columns.Add("Tiempo fin reclamo 1");
            tablaResultado.Columns.Add("Tiempo fin reclamo 2");
            tablaResultado.Columns.Add("RND fin V");
            tablaResultado.Columns.Add("Tiempo atencion V");
            tablaResultado.Columns.Add("Tiempo fin venta 1");
            tablaResultado.Columns.Add("Tiempo fin venta 2");
            tablaResultado.Columns.Add("Tiempo fin venta 3");
            tablaResultado.Columns.Add("RND fin AE");
            tablaResultado.Columns.Add("Tiempo atencion AE");
            tablaResultado.Columns.Add("Tiempo fin atencion 1");
            tablaResultado.Columns.Add("Tiempo fin atencion 2");
            tablaResultado.Columns.Add("RND fin P");
            tablaResultado.Columns.Add("Tiempo atencion P");
            tablaResultado.Columns.Add("Tiempo fin postal");
            if (conCorte)
            {
                tablaResultado.Columns.Add("Tiempo enfriamiento");
                tablaResultado.Columns.Add("Fin enfriamiento");
            }
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
            tablaResultado.Columns.Add("V COLA 1");
            tablaResultado.Columns.Add("V Estado 1");
            tablaResultado.Columns.Add("V COLA 2");
            tablaResultado.Columns.Add("V Estado 2");
            tablaResultado.Columns.Add("V COLA 3");
            tablaResultado.Columns.Add("V Estado 3");
            tablaResultado.Columns.Add("AE COLA 1");
            tablaResultado.Columns.Add("AE Estado 1");
            tablaResultado.Columns.Add("AE COLA 2");
            tablaResultado.Columns.Add("AE Estado 2");

            tablaResultado.Columns.Add("P COLA 1");
            tablaResultado.Columns.Add("P Estado 1");
            if (conCorte)
            {
                tablaResultado.Columns.Add("Tiempo Remanente");
            }

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

        // agregar o reemplazar cliente
        public static void agregarClienteTabla(FilaVector fila2, DataTable tablaResultado, Cliente cliente)
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
        
        

        public void calcularProbabilidad(FilaVector fila2, double t)
        {
            if (fila2.LlegadaCorteLuz.Rnd < 0.20)
            {
                fila2.LlegadaCorteLuz.ProxLlegada = fila2.Reloj + (4 * t);
            }
            else if (0.80 > fila2.LlegadaCorteLuz.Rnd && fila2.LlegadaCorteLuz.Rnd > 0.20)
            {
                fila2.LlegadaCorteLuz.ProxLlegada = fila2.Reloj + (6 * t);
            }
            else
            {
                fila2.LlegadaCorteLuz.ProxLlegada = fila2.Reloj + (8 * t);
            }
        }

        private void btnAusencia_Click(object sender, EventArgs e)
        {
            int cantFilas = int.Parse(txtCantFilas.Text);
            // Valores de media (dist. exponencial) de los eventos 
            double mediaLlegadaPaquete = double.Parse(txtLlegadaPaquete.Text);
            double mediaFinPaquete = double.Parse(txtFinPaquete.Text);
            double mediaLlegadaReclamo = double.Parse(txtLlegadaReclamo.Text);
            double mediaFinReclamo = double.Parse(txtFinReclamo.Text);
            double mediaLlegadaVenta = double.Parse(txtLlegadaVenta.Text);
            double mediaFinVenta = double.Parse(txtFinVenta.Text);
            double mediaLlegadaAE = double.Parse(txtLlegadaAtencion.Text);
            double mediaFinAE = double.Parse(txtFinAtencion.Text);
            double mediaLlegadaPostales = double.Parse(txtLlegadaPostales.Text);
            double mediaFinPostales = double.Parse(txtFinPostales.Text);

            // Nro de fila a partir de la que sea desea visualizar
            int nroPrimeraFilaMostrar = int.Parse(txtPrimeraFila.Text);
            int nroUltimaFilaMostrar = nroPrimeraFilaMostrar + 300;
            if (nroUltimaFilaMostrar > cantFilas) { nroUltimaFilaMostrar = cantFilas; }


            double[] medias = {mediaLlegadaPaquete, mediaFinPaquete, mediaLlegadaReclamo, mediaFinReclamo, mediaLlegadaVenta, mediaFinVenta,
            mediaLlegadaAE, mediaFinAE, mediaLlegadaPostales, mediaFinPostales};

            SimAusenciaAE ventana = new SimAusenciaAE(cantFilas, medias, nroPrimeraFilaMostrar, nroUltimaFilaMostrar);
            ventana.Show();

        }

        private void btnPunto3_Click(object sender, EventArgs e)
        {

            int cantFilas = int.Parse(txtCantFilas.Text);
            // Valores de media (dist. exponencial) de los eventos 
            double mediaLlegadaPaquete = double.Parse(txtLlegadaPaquete.Text);
            double mediaFinPaquete = double.Parse(txtFinPaquete.Text);
            double mediaLlegadaReclamo = double.Parse(txtLlegadaReclamo.Text);
            double mediaFinReclamo = double.Parse(txtFinReclamo.Text);
            double mediaLlegadaVenta = double.Parse(txtLlegadaVenta.Text);
            double mediaFinVenta = double.Parse(txtFinVenta.Text);
            double mediaLlegadaAE = double.Parse(txtLlegadaAtencion.Text);
            double mediaFinAE = double.Parse(txtFinAtencion.Text);
            double mediaLlegadaPostales = double.Parse(txtLlegadaPostales.Text);
            double mediaFinPostales = double.Parse(txtFinPostales.Text);

            // Nro de fila a partir de la que sea desea visualizar
            int nroPrimeraFilaMostrar = int.Parse(txtPrimeraFila.Text);
            int nroUltimaFilaMostrar = nroPrimeraFilaMostrar + 300;
            if (nroUltimaFilaMostrar > cantFilas) { nroUltimaFilaMostrar = cantFilas; }


            double[] medias = {mediaLlegadaPaquete, mediaFinPaquete, mediaLlegadaReclamo, mediaFinReclamo, mediaLlegadaVenta, mediaFinVenta,
            mediaLlegadaAE, mediaFinAE, mediaLlegadaPostales, mediaFinPostales};

            SimPunto3 ventana = new SimPunto3(cantFilas, medias, nroPrimeraFilaMostrar, nroUltimaFilaMostrar);
            ventana.Show();
        }


    }
}
    
