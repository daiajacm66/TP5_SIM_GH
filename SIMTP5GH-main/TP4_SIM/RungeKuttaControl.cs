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
    public class RungeKuttaControl : UserControl
    {
        private DataGridView dgvRungeKutta;
        private DataGridViewTextBoxColumn xi;
        private DataGridViewTextBoxColumn yi;
        private DataGridViewTextBoxColumn k1;
        private DataGridViewTextBoxColumn k2;
        private DataGridViewTextBoxColumn k3;
        private DataGridViewTextBoxColumn k4;
        private DataGridViewTextBoxColumn xi1;
        private DataGridViewTextBoxColumn yi1;
        ////private Panel pnlBotones;
        //private Button btnSiguiente;
        //private Button btnAnterior;
        List<TablaRK> TablasRK;
        private Label lblT;
        private Label lblValorT;
        int indice;

        public RungeKuttaControl(List<TablaRK> tablasRK, double t)
        {
            InitializeComponent();
            this.TablasRK = tablasRK;
            this.indice = 0;
            //this.btnAnterior.Enabled = false;
            //if (tablasRK.Count == indice + 1)
            //{
            //    this.btnSiguiente.Enabled = false;
            //}
            lblValorT.Text = t.ToString();
            CargarTabla();
            this.Dock = DockStyle.Fill;
        }

        private void CargarTabla()
        {
            dgvRungeKutta.Rows.Clear();
            if (this.TablasRK.Count > 0)
            {
                this.dgvRungeKutta.Rows.Clear();
                foreach (FilaRK fila in TablasRK[indice].FilasRK)
                {
                    this.dgvRungeKutta.Rows.Add(fila.ListaString());
                }
            }
        }
        private void btnAnterior_Click(object sender, EventArgs e)
        {
            this.indice--;
            //btnSiguiente.Enabled = true;
            //if (indice == 0)
            //{
            //    btnAnterior.Enabled = false;
            //}
            CargarTabla();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            this.indice++;
            //btnAnterior.Enabled = true;
            //if (indice == TablasRK.Count - 1)
            //{
            //    btnSiguiente.Enabled = false;
            //}

            CargarTabla();
        }

        private void InitializeComponent()
        {
            this.dgvRungeKutta = new System.Windows.Forms.DataGridView();
            this.xi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.k1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.k2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.k3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.k4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xi1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yi1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //this.pnlBotones = new System.Windows.Forms.Panel();
            //this.btnSiguiente = new System.Windows.Forms.Button();
            //this.btnAnterior = new System.Windows.Forms.Button();
            this.lblT = new System.Windows.Forms.Label();
            this.lblValorT = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRungeKutta)).BeginInit();
            //this.pnlBotones.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvRungeKutta
            // 
            this.dgvRungeKutta.AllowUserToAddRows = false;
            this.dgvRungeKutta.AllowUserToDeleteRows = false;
            this.dgvRungeKutta.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRungeKutta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRungeKutta.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.xi,
            this.yi,
            this.k1,
            this.k2,
            this.k3,
            this.k4,
            this.xi1,
            this.yi1});
            this.dgvRungeKutta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRungeKutta.Location = new System.Drawing.Point(0, 0);
            this.dgvRungeKutta.Name = "dgvRungeKutta";
            this.dgvRungeKutta.ReadOnly = true;
            this.dgvRungeKutta.RowHeadersVisible = false;
            this.dgvRungeKutta.RowHeadersWidth = 51;
            this.dgvRungeKutta.Size = new System.Drawing.Size(1139, 540);
            this.dgvRungeKutta.TabIndex = 3;
            // 
            // xi
            // 
            this.xi.HeaderText = "Ti";
            this.xi.MinimumWidth = 6;
            this.xi.Name = "Ti";
            this.xi.ReadOnly = true;
            // 
            // yi
            // 
            this.yi.HeaderText = "Ci";
            this.yi.MinimumWidth = 6;
            this.yi.Name = "Ci";
            this.yi.ReadOnly = true;
            // 
            // k1
            // 
            this.k1.HeaderText = "K1";
            this.k1.MinimumWidth = 6;
            this.k1.Name = "k1";
            this.k1.ReadOnly = true;
            // 
            // k2
            // 
            this.k2.HeaderText = "K2";
            this.k2.MinimumWidth = 6;
            this.k2.Name = "k2";
            this.k2.ReadOnly = true;
            // 
            // k3
            // 
            this.k3.HeaderText = "K3";
            this.k3.MinimumWidth = 6;
            this.k3.Name = "k3";
            this.k3.ReadOnly = true;
            // 
            // k4
            // 
            this.k4.HeaderText = "K4";
            this.k4.MinimumWidth = 6;
            this.k4.Name = "k4";
            this.k4.ReadOnly = true;
            // 
            // xi1
            // 
            this.xi1.HeaderText = "Ti + 1";
            this.xi1.MinimumWidth = 6;
            this.xi1.Name = "Ti1";
            this.xi1.ReadOnly = true;
            // 
            // yi1
            // 
            this.yi1.HeaderText = "Ci + 1";
            this.yi1.MinimumWidth = 6;
            this.yi1.Name = "Ci1";
            this.yi1.ReadOnly = true;
            // 
            //// pnlBotones
            //// 
            //this.pnlBotones.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(148)))), ((int)(((byte)(54)))));
            //this.pnlBotones.Controls.Add(this.lblValorT);
            //this.pnlBotones.Controls.Add(this.lblT);
            //this.pnlBotones.Controls.Add(this.btnSiguiente);
            //this.pnlBotones.Controls.Add(this.btnAnterior);
            //this.pnlBotones.Dock = System.Windows.Forms.DockStyle.Bottom;
            //this.pnlBotones.Location = new System.Drawing.Point(0, 540);
            //this.pnlBotones.Name = "pnlBotones";
            //this.pnlBotones.Size = new System.Drawing.Size(1139, 100);
            //this.pnlBotones.TabIndex = 2;
            //// 
            //// btnSiguiente
            //// 
            //this.btnSiguiente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            //this.btnSiguiente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.btnSiguiente.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            //this.btnSiguiente.ForeColor = System.Drawing.SystemColors.ControlText;
            //this.btnSiguiente.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //this.btnSiguiente.Location = new System.Drawing.Point(701, 26);
            //this.btnSiguiente.Name = "btnSiguiente";
            //this.btnSiguiente.Size = new System.Drawing.Size(248, 40);
            //this.btnSiguiente.TabIndex = 17;
            //this.btnSiguiente.Text = "Siguiente";
            //this.btnSiguiente.UseVisualStyleBackColor = false;
            //this.btnSiguiente.Click += new System.EventHandler(this.btnSiguiente_Click);
            //// 
            //// btnAnterior
            //// 
            //this.btnAnterior.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            //this.btnAnterior.Enabled = false;
            //this.btnAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.btnAnterior.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            //this.btnAnterior.ForeColor = System.Drawing.SystemColors.ControlText;
            //this.btnAnterior.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //this.btnAnterior.Location = new System.Drawing.Point(235, 26);
            //this.btnAnterior.Name = "btnAnterior";
            //this.btnAnterior.Size = new System.Drawing.Size(248, 40);
            //this.btnAnterior.TabIndex = 16;
            //this.btnAnterior.Text = "Anterior";
            //this.btnAnterior.UseVisualStyleBackColor = false;
            //this.btnAnterior.Click += new System.EventHandler(this.btnAnterior_Click);
            // 
            // lblT
            // 
            this.lblT.AutoSize = true;
            this.lblT.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            this.lblT.Location = new System.Drawing.Point(14, 26);
            this.lblT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblT.Name = "lblT";
            this.lblT.Size = new System.Drawing.Size(110, 25);
            this.lblT.TabIndex = 18;
            this.lblT.Text = "Valor de T:";
            this.lblT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblValorT
            // 
            this.lblValorT.AutoSize = true;
            this.lblValorT.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblValorT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(242)))), ((int)(((byte)(250)))));
            this.lblValorT.Location = new System.Drawing.Point(129, 26);
            this.lblValorT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblValorT.Name = "lblValorT";
            this.lblValorT.Size = new System.Drawing.Size(99, 25);
            this.lblValorT.TabIndex = 19;
            this.lblValorT.Text = "Resultado";
            this.lblValorT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RungeKuttaControl
            // 
            this.Controls.Add(this.dgvRungeKutta);
            //this.Controls.Add(this.pnlBotones);
            this.Name = "RungeKuttaControl";
            this.Size = new System.Drawing.Size(1139, 640);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRungeKutta)).EndInit();
            //this.pnlBotones.ResumeLayout(false);
            //this.pnlBotones.PerformLayout();
            this.ResumeLayout(false);

        }

        
    }
}
