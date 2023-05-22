using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Presentation;
using Server.Infra.Db;
using Server.Main;
using Mqtt;
using Common.Models;
using Server.Main.Factories;

namespace Server
{
    class Server
    {
        private static IMqttClientService s_mqttClient;
        private static readonly List<object> s_controllers = new();

        public static ServerNotificationCenter NotificationCenter { get; private set; }

        public static void Configure(IMqttClientService mqttClient)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            s_mqttClient = mqttClient;
            s_mqttClient.OnConnect += MqttClient_OnConnect;
            s_mqttClient.OnDisconnect += MqttClient_OnDisconnect;
            NotificationCenter = new ServerNotificationCenter(mqttClient);
            NotificationCenter.OnGetStatus += NotificationCenter_OnRequestStatus;
            RegisterMapping.Register();
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
            Console.WriteLine($"Erro inesperado por exceção sem tratamento. Terminating: {e.IsTerminating} Exc: {(Exception)e.ExceptionObject}.");
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

        private static void NotificationCenter_OnRequestStatus(MqttMessage message)
        {
            Task.Run(() =>
            {
                var topic = $"sys/server/{s_mqttClient.ClientId}/status/callback/{message.GetID()}";
                var result = new RequestResult()
                {
                    ResultCode = RequestResultCode.Success,
                    Message = "Healthy"
                };
                var callbackMessage = new MqttMessage(topic, result);
                s_mqttClient.Publish(callbackMessage);
                return;
            });
        }

        private static void RegisterControllers()
        {
            s_controllers.Add(CustomerControllerFactory.Create(NotificationCenter));
            s_controllers.Add(CalibrationControllerFactory.Create(NotificationCenter));

            foreach (var controller in s_controllers)
            {
                var type = controller.GetType();
                Attribute[] attributes = Attribute.GetCustomAttributes(type);

                foreach (Attribute attribute in attributes)
                {
                    if (attribute is Subscribe sub)
                    {
                        foreach (string topic in sub.GetTopics())
                        {
                            s_mqttClient.Subscribe(topic);
                        }
                    }
                }
            }
        }
    }
}
