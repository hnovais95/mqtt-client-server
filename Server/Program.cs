using System;
using System.Collections.Generic;
using Mqtt;
using DataAccess;

namespace MqttServer
{
    class Program
    {
        private static IMqttClientService _mqttClient;
        private static IRouter _router;
        private static List<object> _controllers;

        static void Main(string[] args)
        {
            Console.WriteLine("Iniciou servidor.");

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            EntityMapper.RegisterTypeMaps();

            _mqttClient = new MqttClient("127.0.0.1", 1883);
            _mqttClient.OnConnect += MqttClient_OnConnect;
            _mqttClient.OnDisconnect += MqttClient_OnDisconnect;
            _mqttClient.Connect();

            _router = new Router(_mqttClient);

            while (true) ;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _mqttClient.OnDisconnect -= MqttClient_OnDisconnect;
            _mqttClient.Disconnect();
            Console.WriteLine($"Erro inesperado por exceção sem tratamento. Terminating: {e.IsTerminating} Exceção: {(Exception)e.ExceptionObject}.");
        }

        private static void MqttClient_OnConnect()
        {
            RegisterControllers();
        }

        private static void MqttClient_OnDisconnect()
        {
            Console.WriteLine("Tentando reconectar...");
            _mqttClient.Connect();
        }

        private static void RegisterControllers()
        {
            _controllers = new List<object>();
            _controllers.Add(CustomersControllerFactory.MakeController(_router));

            foreach (var controller in _controllers)
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
                _mqttClient.Subscribe(topic);
            }
        }
    }
}
