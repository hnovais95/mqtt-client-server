using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mqtt;
using Common.Models;

namespace Client
{
    public partial class Form1 : Form
    {
        public MenuStrip MainMenu
        {
            get  { return menuStrip1; }
        }

        public Form1()
        {
            InitializeComponent();
            Client.NotificationCenter.OnResponseCustomers += NotificationCenter_OnResponseCustomers;
        }

        private void NotificationCenter_OnResponseCustomers(MqttMessage message)
        {
            MessageBox.Show("Recebeu a mensagem!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run((Action)(() =>
            {
                try
                {
                    var response = Client.NotificationCenter.PublishAndWaitCallback<IEnumerable<CustomerModel>>(NotificationName.Customers, null, 5000);
                    var description = response.Select(x => x.ToString()).Aggregate((acc, x) => acc + "\n" + x).Trim('\n');

                    if (response != null)
                    {
                        MessageBox.Show(description, "Recebeu a mensagem!");
                    }
                    else
                    {
                        MessageBox.Show("Deu erro!");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao publicar mensagem aguardando callback. Exc.: {e}");
                }
            }));
        }
    }
}
