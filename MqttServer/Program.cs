using System;
using System.Collections.Generic;
using Mqtt;
using DataAccess;

namespace MqttServer
{
    class Program
    {
        private static IMqtt _mqttClient;
        private static IRouter _router;
        private static List<object> _controllers;

        static void Main(string[] args)
        {
            Console.WriteLine("Iniciou servidor.");

            EntityMapper.RegisterTypeMaps();

            _mqttClient = new MqttClient("localhost", 1883);
            _mqttClient.OnConnect += MqttClient_OnConnect;
            _mqttClient.Connect();

            _router = new Router(_mqttClient);

            while (true) ;
        }

        private static void MqttClient_OnConnect()
        {
            RegisterControllers();
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
