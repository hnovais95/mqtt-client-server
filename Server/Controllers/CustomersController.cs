using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Mqtt;
using DataAccess.Entities;
using Common.Models;

namespace Server
{
    [Subscribe("sys/client/+/customers/request/+")]
    class CustomersController
    {
        private readonly IServerNotificationCenter _notificationCenter;
        private readonly ICustomerService _customerService;

        public CustomersController(IServerNotificationCenter notifcationCenter, ICustomerService customerService)
        {
            _notificationCenter = notifcationCenter;
            _notificationCenter.OnRequestCustomers += NotificationCenter_OnRequestCustomers;
            _customerService = customerService;
        }

        private void NotificationCenter_OnRequestCustomers(MqttMessage mqttMessage)
        {
            try
            {
                var entities = _customerService.GetAllCustomers();
                var customers = entities.Select(x => Model.Parse<CustomerModel>(x));
                var callbackId = mqttMessage.GetID();
                _notificationCenter.Publish(ServerPublishCommand.SendCustomers, customers, callbackId);
            }
            catch (Exception e)
            {
                var payload = mqttMessage.Payload != null ? mqttMessage.Payload.ToString() : "";
                Console.WriteLine($"Erro ao tratar requisção. Tópico: {mqttMessage.Topic}; Payload: {payload}; Exc.: {e}");
            }
        }
    }
}
