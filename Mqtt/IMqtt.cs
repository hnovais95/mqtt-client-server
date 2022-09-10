using System;
using System.Collections.Generic;

namespace Mqtt
{
    public delegate void ReceiveMessageDelegate(MqttMessage message);

    public interface IMqtt
    {
        public event ReceiveMessageDelegate OnReceiveMessage;
        public event Action OnConnect;
        public event Action OnDisconnect;

        public bool IsConnected { get; }

        public void Connect();
        public void Disconnect();
        public void Subscribe(string topic);
        public void Publish(string topic, Dictionary<string, object>? payload);
    }
}
