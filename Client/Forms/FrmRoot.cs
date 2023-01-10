using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class FrmRoot : Form
    {
        public MenuStrip MainMenu
        {
            get  { return menuStrip1; }
        }

        public FrmRoot()
        {
            InitializeComponent();
        }

        private void FrmRoot_Shown(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void FrmRoot_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (Client.HealthStatus)
            {
                label1.BackColor = Color.Lime;
            }
            else
            {
                if (label1.BackColor == Color.Red)
                    label1.BackColor = Color.Brown;
                else
                    label1.BackColor = Color.Red;
            }
        }
    }
}
