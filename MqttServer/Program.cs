using System;
using Mqtt;

namespace MqttServer
{
    class Program
    {
        private static MqttClient _mqttClient;
        private static Interactor _interactor;

        static void Main(string[] args)
        {
            Console.WriteLine("Iniciou servidor.");
            _mqttClient = new MqttClient("localhost", 1883);
            _interactor = new Interactor(_mqttClient);
            _mqttClient.Connect();
            while (true) ;
        }
    }
}
