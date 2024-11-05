using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TP4_SIM.Clases.RK;

namespace TP4_SIM
{
    public partial class FormularioConControl : Form
    {
        public FormularioConControl(RungeKuttaControl control)
        {
            // Configura el formulario
            this.Text = "Formulario con RungeKuttaControl";
            this.Size = new Size(800, 600); // Ajusta el tamaño según sea necesario

            // Añade el control de usuario al formulario
            control.Dock = DockStyle.Fill;
            this.Controls.Add(control);
        }

        public FormularioConControl()
        {
            InitializeComponent();
        }

        //public void CargarRK(List<TablaRK> tablasRK, double c)
        //{
        //    this.pnlRk.Controls.Clear();
        //    // creamos el control de resultados
        //    var res = new RungeKuttaControl(tablasRK, c);
        //    this.pnlRk.Controls.Add(res);
        //    //this.pnlRk.Controls.Add(res);
        //    //FormularioConControl formulario = new FormularioConControl(res);
        //    //formulario.Show();

        //}

    }
}
