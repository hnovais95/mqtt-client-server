using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mqtt;

namespace Client
{
    static partial class Client
    {
        private static FrmRoot s_frmRoot;
        private static IMqttClientService s_mqttClient;

        public static IClientNotificationCenter NotificationCenter { get; private set; }

        public static void Configure(IMqttClientService mqttClient)
        {
            NotificationCenter = new ClientNotificationCenter(mqttClient);
            s_frmRoot = new();
            s_mqttClient = mqttClient;
            s_mqttClient.OnConnect += MqttClient_OnConnect;
            s_mqttClient.OnDisconnect += MqttClient_OnDisconnect;
            BuildMenu();
        }

        public static void Start()
        {
            Console.WriteLine("Iniciou cliente!");
            s_mqttClient.Connect();
            Application.Run(s_frmRoot);
        }

        private static void MqttClient_OnConnect()
        {
            Console.WriteLine("Cliente conectado ao broker MQTT!");
        }

        private static void MqttClient_OnDisconnect()
        {
            Console.WriteLine("Tentando reconectar ao broker MQTT...");
            s_mqttClient.Connect();
        }
    }
}