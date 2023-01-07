using Mqtt;
using Common;

namespace MqttServer
{
    public class Router: IRouter
    {
        public event DelOnRequestCustomers OnRequestCustomers;

        private readonly IMqttClientService _mqttClient;

        public Router(IMqttClientService mqttClient)
        {
            _mqttClient = mqttClient;
            _mqttClient.OnReceiveMessage += MqttClient_OnReceiveMessage;
        }

        private void MqttClient_OnReceiveMessage(MqttMessage message)
        {
            if (RegexEvaluator.Evaluate(Route.Customers, message.Topic))
            {
                OnRequestCustomers?.Invoke(message);
                return;
            }
        }

        public void SendMessage(MqttMessage mqttMessage)
        {
            _mqttClient.Publish(mqttMessage);
        }
    }
}
