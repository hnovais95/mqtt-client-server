using Mqtt;
using Common.Models;

namespace Client
{

    public delegate void DelOnReceiveCustomers(MqttMessage message);

    interface IClientNotificationCenter
    {
        public event DelOnReceiveCustomers OnReceiveCustomers;

        public void Publish(ClientCommand command, object body);
        public RequestResult PublishAndWaitCallback(ClientCommand command, object body, int timeout);
    }
}
