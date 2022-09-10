using Mqtt;

namespace MqttServer
{
    public class Router: IRouter
    {
        public event RequestCustomersDelegate OnRequestCustomers;

        private readonly IMqtt _mqttClient;

        public Router(IMqtt mqttClient)
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

        public void SendMessage(MqttMessage message)
        {
            _mqttClient.Publish(message.Topic, message.Payload);
        }
    }
}
