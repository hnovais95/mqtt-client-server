using Mqtt;

namespace Server
{
    public delegate void DelOnRequestCustomers(MqttMessage mqttMessage);

    interface IServerNotificationCenter
    {
        public event DelOnRequestCustomers OnRequestCustomers;

        public void Publish(ServerPublishCommand command, object body, string callbackId = null);
        public T PublishAndWaitCallback<T>(ServerPublishCommand command, object body, int timeout) where T : class;
    }
}
