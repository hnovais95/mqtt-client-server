using System.Collections.Generic;

namespace Mqtt
{
    public class MqttMessage
    {
        public string Topic { get; set; }
        public object Payload { get; set; }

        public MqttMessage(string topic, object payload)
        {
            Topic = topic;
            Payload = payload;
        }
    }
}
