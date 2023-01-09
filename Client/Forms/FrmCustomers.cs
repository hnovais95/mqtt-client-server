using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Models;

namespace Client
{
    public partial class FrmCustomers : Form
    {
        public FrmCustomers()
        {
            InitializeComponent();
        }

        public void Update(List<CustomerModel> customers)
        {
            dataGridView1.DataSource = customers;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            ((FrmCustomers)sender).Hide();
            e.Cancel = true;
        }
    }
}
