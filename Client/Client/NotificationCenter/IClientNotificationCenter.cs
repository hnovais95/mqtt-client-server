using Mqtt;
using Common.Models;

namespace Client
{

    public delegate void DelOnReceiveCustomers(MqttMessage message);

    interface IClientNotificationCenter
    {
        public event DelOnReceiveCustomers OnReceiveCustomers;

        public void Publish(ClientPublishCommand command, object body);
        public RequestResult PublishAndWaitCallback(ClientPublishCommand command, object body, int timeout);
    }
}
