using System;
using System.Threading.Tasks;

namespace Mqtt
{
    public delegate void DelOnReceiveMessage(MqttMessage message);

    public interface IMqttClientService
    {
        public event DelOnReceiveMessage OnReceiveMessage;
        public event Action OnConnect;
        public event Action OnDisconnect;

        public bool IsConnected { get; }
        public string ClientId { get; }

        public void Connect();
        public void Disconnect();
        public void Subscribe(string topic);
        public void Publish(MqttMessage mqttMessage);
        public Task<MqttMessage> PublishAndWaitCallback(MqttMessage mqttMessage, int timeout);
    }
}
