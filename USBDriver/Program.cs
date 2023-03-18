using Mqtt;

namespace USBDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            var mqttClient = new MqttClient("test.mosquitto.org", 1883);
            USBDriver.Configure(mqttClient);
            USBDriver.Start();
        }
    }
}
