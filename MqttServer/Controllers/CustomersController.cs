using System.Collections.Generic;
using System.Linq;
using Mqtt;

namespace MqttServer
{
    public class CustomersController
    {
        private readonly IMqtt _mqttClient;
        private readonly ICustomerService _customerService;

        public CustomersController(IMqtt mqttClient, ICustomerService customerService)
        {
            _mqttClient = mqttClient;
            _mqttClient.OnConnect += MqttClient_OnConnect;
            _mqttClient.OnReceiveMessage += MqttClient_OnReceiveMessage;
            _customerService = customerService;
        }

        private void MqttClient_OnConnect()
        {
            _mqttClient.Subscribe("client/request/customers/+");
        }

        private void MqttClient_OnReceiveMessage(string clientId, string topic, Dictionary<string, object>? body)
        {
            var topicPattern = @"^client/request/\w+/\w+$";
            var isMatching = RegexEvaluator.Evaluete(topicPattern, topic);

            if (!isMatching) { return; }

            var resource = topic.Split('/')[^2];
            var messageId = topic.Split('/')[^1];

            var customers = _customerService.GetAllCustomers().Select(x => x.ContactName);
            var response = new Dictionary<string, object> { { "customers", customers } };
            _mqttClient.Publish($"client/{clientId}/response/{resource}/{messageId}", response);
        }
    }
}
