using System.Collections.Generic;
using Mqtt;

namespace MqttServer
{
    [Subscribe("client/request/customers/+")]
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

        private void Router_OnRequestCustomers(MqttMessage message)
        {
            var customers = _customerService.GetAllCustomers();
            var messageId = message.Topic.Split('/')[^1];
            var topic = $"client/{message.ClientId}/response/customers/{messageId}";
            var payload = new Dictionary<string, object> { { "customers", customers } };
            var response = new MqttMessage(topic, payload);
            _router.SendMessage(response);
        }
    }
}
