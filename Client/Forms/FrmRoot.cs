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

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    var customers = Client.NotificationCenter.PublishAndWaitCallback<IEnumerable<CustomerModel>>(ClientPublishCommand.RequestCustomers, null, 5000);

                    if (customers != null)
                    {
                        Client.Screen.Show<FrmCustomers>(customers);
                    }
                    else
                    {
                        MessageBox.Show("Erro", "Não foi possível concluir operação.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao publicar mensagem aguardando callback. Exc.: {e}");
                }
            });
        }
    }
}
