using Mqtt;

namespace Client
{

    public delegate void DelOnReceiveCustomers(MqttMessage message);

    interface IClientNotificationCenter
    {
        public event DelOnReceiveCustomers OnReceiveCustomers;

        public void Publish(ClientPublishCommand command, object body);
        public T PublishAndWaitCallback<T>(ClientPublishCommand command, object body, int timeout) where T : class;
    }
}
