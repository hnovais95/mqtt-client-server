using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mqtt;

namespace App
{
    static partial class Client
    {
        private static Form1 s_frmRoot;
        private static IMqttClientService s_mqttClient;

        public static NotificationCenter NotificationCenter { get; private set; }

        public static void Configure(IMqttClientService mqttClient)
        {
            NotificationCenter = new NotificationCenter(mqttClient);
            s_frmRoot = new();
            s_mqttClient = mqttClient;
            s_mqttClient.OnConnect += MqttClient_OnConnect;
            s_mqttClient.OnDisconnect += MqttClient_OnDisconnect;
            BuildMenu();
        }

        public static void Start()
        {
            s_mqttClient.Connect();
            Application.Run(s_frmRoot);
        }

        private static void MqttClient_OnConnect()
        {
            Console.WriteLine("Cliente conectado!");
        }

        private static void MqttClient_OnDisconnect()
        {
            Console.WriteLine("Tentando reconectar...");
            s_mqttClient.Connect();
        }
    }
}