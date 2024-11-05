using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TP4_SIM.Clases
{
    public class CsvConverter
    {
        public static void DataTableToCsv(DataTable dataTable, string filePath, bool includeHeaders = true)
        {
            try
            {
                StringBuilder csv = new StringBuilder();

                if (includeHeaders)
                {
                    // Escribir los encabezados
                    var columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
                    csv.AppendLine(string.Join(";", columnNames));
                }

                // Escribir las filas
                foreach (DataRow row in dataTable.Rows)
                {
                    var fields = row.ItemArray.Select(field => field.ToString());
                    csv.AppendLine(string.Join(";", fields));
                }

                // Escribir el CSV a un archivo
                File.WriteAllText(filePath, csv.ToString());
                Console.WriteLine("El archivo CSV se ha creado correctamente en " + filePath);

                // Abrir el archivo CSV con la aplicación predeterminada
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se produjo un error al crear el archivo CSV: " + ex.Message);
            }
        }

    }
}
