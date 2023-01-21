using Mqtt;
using Common.Models;

namespace Server
{
    public delegate void DelOnRequestStatus(MqttMessage message);
    public delegate void DelOnRequestCustomers(MqttMessage message);
    public delegate void DelOnAddCustomer(MqttMessage message);

    interface IServerNotificationCenter
    {
        public event DelOnRequestStatus OnRequestStatus;
        public event DelOnRequestCustomers OnRequestCustomers;
        public event DelOnAddCustomer OnAddCustomer;

        public void Publish(ServerCommand command, object body, string callbackId = null);
        public RequestResult PublishAndWaitCallback(ServerCommand command, object body, int timeout);
    }
}
