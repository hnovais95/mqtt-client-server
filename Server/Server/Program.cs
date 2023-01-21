using Mqtt;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var mqttClient = new MqttClient("127.0.0.1", 1883);
            Server.Configure(mqttClient);
            Server.Start();
        }
    }
}
