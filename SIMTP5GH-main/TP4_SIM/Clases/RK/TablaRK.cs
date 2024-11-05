using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4_SIM.Clases.RK
{
    public class TablaRK
    {
        public List<FilaRK> FilasRK { get; set; }
        public double ResultadoRK { get; set; }
        public TablaRK()
        {
            FilasRK = new List<FilaRK>();
        }
        public void AgregarFila(FilaRK fila)
        {
            FilasRK.Add(new FilaRK(fila));
        }

        public DataTable ToDataTable()
        {
            // Crear un DataTable con las columnas especificadas
            DataTable dataTable = new DataTable();

            // Agregar las columnas con los nombres indicados
            dataTable.Columns.Add("t", typeof(double));
            dataTable.Columns.Add("C", typeof(double));
            dataTable.Columns.Add("k1", typeof(double));
            dataTable.Columns.Add("k2", typeof(double));
            dataTable.Columns.Add("k3", typeof(double));
            dataTable.Columns.Add("k4", typeof(double));
            dataTable.Columns.Add("T+1", typeof(double));
            dataTable.Columns.Add("C+1", typeof(double));

            // Agregar las filas de FilasRK al DataTable
            foreach (FilaRK fila in FilasRK)
            {
                DataRow row = dataTable.NewRow();
                row["t"] = fila.xi;
                row["C"] = fila.yi;
                row["k1"] = fila.k1;
                row["k2"] = fila.k2;
                row["k3"] = fila.k3;
                row["k4"] = fila.k4;
                row["T+1"] = fila.nextXi;
                row["C+1"] = fila.nextYi;

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

    }
}
