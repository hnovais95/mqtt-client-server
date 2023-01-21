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
            _mqttClient.Subscribe("sys/server/+/customers/get/callback/+");
            _mqttClient.Subscribe("sys/server/+/customers/add/callback/+");
            _mqttClient.Subscribe("sys/server/+/status/callback/+");
        }

        private void MqttClient_OnReceiveMessage(MqttMessage message)
        {
            if (ClientNotificationName.GetCustomerResponse.MatchWith(message.Topic))
                OnReceiveCustomers?.Invoke(message);
        }

        public void Publish(ClientCommand command, object body) {
            throw new NotImplementedException();
        }

        public RequestResult PublishAndWaitCallback(ClientCommand command, object body, int timeout)
        {
            string topic;

            switch (command)
            {
                case ClientCommand.GetCustomers:
                    topic = $"sys/client/{_mqttClient.ClientId}/customers/get/{Guid.NewGuid()}";
                    break;
                case ClientCommand.AddCustomer:
                    topic = $"sys/client/{_mqttClient.ClientId}/customers/add/{Guid.NewGuid()}";
                    break;
                case ClientCommand.HealthCheck:
                    topic = $"sys/client/{_mqttClient.ClientId}/status/{Guid.NewGuid()}";
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
