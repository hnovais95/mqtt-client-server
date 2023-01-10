using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mqtt;
using Common.Models;

namespace Client
{
    static partial class Client
    {
        private static FrmRoot s_frmRoot;
        private static IMqttClientService s_mqttClient;
        private static readonly System.Timers.Timer t_timer = new(30000);

        public static IClientNotificationCenter NotificationCenter { get; private set; }
        public static bool HealthStatus { get; private set; }

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
            HealthCheck();
        }

        private static void MqttClient_OnDisconnect()
        {
            Console.WriteLine("Tentando reconectar ao broker MQTT...");
            s_mqttClient.Connect();
        }

        private static void HealthCheck()
        {
            t_timer.AutoReset = true;
            t_timer.Elapsed += (source, eventArgs) =>
            {
                Task.Run(() => {
                    t_timer.Enabled = false;

                    var result = new RequestResult();

                    try
                    {
                        result = NotificationCenter.PublishAndWaitCallback(ClientPublishCommand.HealthCheck, null, 5000);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Erro ao verificar integridade do sistema. Exc.: {e}");
                        result.ResultCode = RequestResultCode.Failure;
                    }
                    finally
                    {
                        HealthStatus = result.ResultCode == RequestResultCode.Success;
                        t_timer.Enabled = true;
                    }
                });
            };
            t_timer.Start();
        }
    }
}