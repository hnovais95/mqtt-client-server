using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mqtt;
using DataAccess;

namespace Server
{
    class Server
    {
        private static IMqttClientService s_mqttClient;
        private static List<object> s_controllers = new();

        public static IServerNotificationCenter NotificationCenter { get; private set; }

        public static void Configure(IMqttClientService mqttClient)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            s_mqttClient = mqttClient;
            s_mqttClient.OnConnect += MqttClient_OnConnect;
            s_mqttClient.OnDisconnect += MqttClient_OnDisconnect;
            NotificationCenter = new ServerNotificationCenter(mqttClient);
            EntityMapper.RegisterTypeMaps();
        }

        public static void Start()
        {
            Console.WriteLine("Iniciou servidor!");
            s_mqttClient.Connect();
            while (true) ;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            s_mqttClient.OnDisconnect -= MqttClient_OnDisconnect;
            s_mqttClient.Disconnect();
            Console.WriteLine($"Erro inesperado por exceção sem tratamento. Terminating: {e.IsTerminating} Exceção: {(Exception)e.ExceptionObject}.");
        }

        private static void MqttClient_OnConnect()
        {
            Console.WriteLine("Servidor conectado ao broker MQTT!");
            RegisterControllers();
        }

        private static void MqttClient_OnDisconnect()
        {
            Console.WriteLine("Tentando reconectar ao broker MQTT...");
            s_mqttClient.Connect();
        }

        private static void RegisterControllers()
        {
            s_controllers.Add(CustomersControllerFactory.MakeController(NotificationCenter));

            foreach (var controller in s_controllers)
            {
                var type = controller.GetType();
                Attribute[] attributes = Attribute.GetCustomAttributes(type);

                foreach (Attribute attribute in attributes)
                {
                    if (attribute is Subscribe sub)
                    {
                        SubscribeTopics(sub.GetTopics());
                    }
                }
            }
        }

        private static void SubscribeTopics(string[] topics)
        {
            foreach (string topic in topics)
            {
                s_mqttClient.Subscribe(topic);
            }
        }
    }
}
