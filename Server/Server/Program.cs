﻿using Mqtt;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var mqttClient = new MqttClient("test.mosquitto.org", 1883);
            Server.Configure(mqttClient);
            Server.Start();
        }
    }
}
