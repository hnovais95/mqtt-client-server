using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Mqtt;
using DataAccess.Entities;
using Common.Models;

namespace MqttServer
{
    [Subscribe("sys/client/+/request/customers/+")]
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
            try
            {
                var entities = _customerService.GetAllCustomers();
                var customers = entities.Select(x => Model.Parse<CustomerModel>(x));

                var clientId = mqttMessage.Topic.Split('/')[^4];
                var topic = $"sys/client/{clientId}/response/customers/{mqttMessage.GetID()}";
                var response = new MqttMessage(topic, customers);
                _router.SendMessage(response);
            }
            catch (Exception e)
            {
                var payload = mqttMessage.Payload != null ? mqttMessage.Payload.ToString() : "";
                Console.WriteLine($"Erro ao tratar requisção. Tópico: {mqttMessage.Topic}; Payload: {payload}; Exc.: {e}");
            }
        }
    }
}
