using Mqtt;
using Common.Models;

namespace Server
{
    public delegate void DelOnRequestCustomers(MqttMessage mqttMessage);
    public delegate void DelOnAddCustomer(MqttMessage mqttMessage);

    interface IServerNotificationCenter
    {
        public event DelOnRequestCustomers OnRequestCustomers;
        public event DelOnAddCustomer OnAddCustomer;

        public void Publish(ServerPublishCommand command, object body, string callbackId = null);
        public RequestResult PublishAndWaitCallback(ServerPublishCommand command, object body, int timeout);
    }
}
