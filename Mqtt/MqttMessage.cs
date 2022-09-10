using System.Collections.Generic;

namespace Mqtt
{
    public class MqttMessage
    {
        public string Topic { get; set; }
        public Dictionary<string, object>? Payload { get; set; }

        public MqttMessage(string topic, Dictionary<string, object>? payload)
        {
            Topic = topic;
            Payload = payload;
        }
    }
}
