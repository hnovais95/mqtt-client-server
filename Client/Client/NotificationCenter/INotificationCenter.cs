using Mqtt;

namespace Client
{

    public delegate void DelOnResponseCustomers(MqttMessage message);

    interface INotificationCenter
    {
        public event DelOnResponseCustomers OnResponseCustomers;

        public void Publish(NotificationName notificationName, object body);
        public T PublishAndWaitCallback<T>(NotificationName notificationName, object body, int timeout) where T : class;
    }
}
