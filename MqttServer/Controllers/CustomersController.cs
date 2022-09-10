using System.Collections.Generic;
using System.Linq;
using Mqtt;

namespace MqttServer
{
    public class CustomersController
    {
        private IMqtt _mqttClient;
        private ICustomerRepository _customerRepository;

        public CustomersController(IMqtt mqttClient, ICustomerRepository customerRepository)
        {
            _mqttClient = mqttClient;
            _mqttClient.OnConnect += MqttClient_OnConnect;
            _mqttClient.OnReceiveMessage += MqttClient_OnReceiveMessage;
            _customerRepository = customerRepository;
        }

        private void MqttClient_OnConnect()
        {
            _mqttClient.Subscribe("client/request/customers/+");
        }

        private void MqttClient_OnReceiveMessage(string clientId, string topic, Dictionary<string, object>? body)
        {
            var components = topic.Split('/');
            if (components.Length < 4) { return; }

            var resource = components[components.Length - 2];
            var messageId = components[components.Length - 1];

            var customers = _customerRepository.GetAll().Select(x => x.ContactName);
            var response = new Dictionary<string, object> { { "customers", customers } };
            _mqttClient.Publish($"client/{clientId}/response/{resource}/{messageId}", response);
        }
    }
}
