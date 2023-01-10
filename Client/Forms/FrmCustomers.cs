using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void FrmCustomers_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == false)
            {
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    var result = Client.NotificationCenter.PublishAndWaitCallback(ClientPublishCommand.GetCustomers, null, 5000);

                    if (result.ResultCode == RequestResultCode.Success)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            dataGridView1.DataSource = result.DeserializeBody<List<CustomerDTO>>();
                        });
                    }
                    else
                    {
                        throw new Exception(result.Message);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao carregar clientes. Exc.: {e}");
                    MessageBox.Show(e.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Hide();
                }
            });
        }
    }
}
