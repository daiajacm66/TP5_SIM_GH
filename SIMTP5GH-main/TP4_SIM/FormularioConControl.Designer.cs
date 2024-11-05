using System.Collections.Generic;
using TP4_SIM.Clases.RK;

namespace TP4_SIM
{
    public partial class FormularioConControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlRk = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlRk
            // 
            this.pnlRk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRk.Location = new System.Drawing.Point(0, 0);
            this.pnlRk.Name = "pnlRk";
            this.pnlRk.Size = new System.Drawing.Size(800, 450);
            this.pnlRk.TabIndex = 3;
            // 
            // FormularioConControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlRk);
            this.Name = "FormularioConControl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormularioConControl";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlRk;

        
    }
}