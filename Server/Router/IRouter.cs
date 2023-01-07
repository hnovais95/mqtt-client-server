using Mqtt;

namespace MqttServer
{
    public delegate void DelOnRequestCustomers(MqttMessage mqttMessage);

    public interface IRouter
    {
        public event DelOnRequestCustomers OnRequestCustomers;
        public void SendMessage(MqttMessage mqttMessage);
    }
}
