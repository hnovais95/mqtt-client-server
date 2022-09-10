using System;
using Mqtt;
using DataAccess;

namespace MqttServer
{
    class Program
    {
        private static MqttClient _mqttClient;

        static void Main(string[] args)
        {
            Console.WriteLine("Iniciou servidor.");
            EntityMapper.RegisterTypeMaps();
            _mqttClient = new MqttClient("localhost", 1883);
            _ = new CustomersController(_mqttClient, new CustomerRepository());
            _mqttClient.Connect();
            while (true) ;
        }
    }
}
