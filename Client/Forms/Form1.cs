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

namespace App
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
            var topic = $"client/1/request/customers/{Guid.NewGuid()}";
            var mqttMessage = new MqttMessage(topic, null);
            Client.NotificationCenter.SendMessage(NotificationName.Customers, mqttMessage);
        }
    }
}
