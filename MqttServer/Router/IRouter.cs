using Mqtt;

namespace MqttServer
{
    public delegate void RequestCustomersDelegate(MqttMessage message);

    public interface IRouter
    {
        public event RequestCustomersDelegate OnRequestCustomers;
        public void SendMessage(MqttMessage message);
    }
}
