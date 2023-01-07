using System.Collections.Generic;
using Mqtt;

namespace MqttServer
{
    [Subscribe("client/+/request/customers/+")]
    public class CustomersController
    {
        private readonly IRouter _router;
        private readonly ICustomerService _customerService;

        public CustomersController(IRouter router, ICustomerService customerService)
        {
            _router = router;
            _router.OnRequestCustomers += Router_OnRequestCustomers;
            _customerService = customerService;
        }

        private void Router_OnRequestCustomers(MqttMessage mqttMessage)
        {
            var customers = _customerService.GetAllCustomers();
            var messageId = mqttMessage.Topic.Split('/')[^1];
            var clientId = mqttMessage.Topic.Split('/')[^4];
            var topic = $"client/{clientId}/response/customers/{messageId}";
            var payload = new Dictionary<string, object> { { "customers", customers } };
            var response = new MqttMessage(topic, payload);
            _router.SendMessage(response);
        }
    }
}
