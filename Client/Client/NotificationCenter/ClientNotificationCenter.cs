using System;
using System.Collections.Generic;
using System.Text.Json;
using Mqtt;
using Common;
using Common.Models;

namespace Client
{
    class ClientNotificationCenter: IClientNotificationCenter
    {

        #region -- E V E N T S

        public event DelOnReceiveCustomers OnReceiveCustomers;

        #endregion

        private readonly IMqttClientService _mqttClient;

        public ClientNotificationCenter(IMqttClientService mqttClient)
        {
            _mqttClient = mqttClient;
            _mqttClient.OnConnect += MqttClient_OnConnect;
            _mqttClient.OnReceiveMessage += MqttClient_OnReceiveMessage;
        }

        private void MqttClient_OnConnect()
        {
            _mqttClient.Subscribe("sys/client/+/customers/get/callback/+");
            _mqttClient.Subscribe("sys/client/+/customers/add/callback/+");
        }

        private void MqttClient_OnReceiveMessage(MqttMessage message)
        {
            if (RegexEvaluator.Evaluate(ClientNotificationName.Customers.Value, message.Topic))
            {
                OnReceiveCustomers?.Invoke(message);
                return;
            }
        }

        public void Publish(ClientPublishCommand command, object body) {
            throw new NotImplementedException();
        }

        public RequestResult PublishAndWaitCallback(ClientPublishCommand command, object body, int timeout)
        {
            string topic;

            switch (command)
            {
                case ClientPublishCommand.GetCustomers:
                    topic = $"sys/client/{_mqttClient.ClientId}/customers/get/{Guid.NewGuid()}";
                    break;
                case ClientPublishCommand.AddCustomer:
                    topic = $"sys/client/{_mqttClient.ClientId}/customers/add/{Guid.NewGuid()}";
                    break;
                default:
                    return default;
            }

            var message = new MqttMessage(topic, body);
            var callback = _mqttClient.PublishAndWaitCallback(message, timeout);
            var result = JsonSerializer.Deserialize<RequestResult>((string)callback.Result.Payload);
            return result;
        }
    }
}
