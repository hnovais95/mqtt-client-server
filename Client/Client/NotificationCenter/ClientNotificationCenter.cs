using System;
using System.Collections.Generic;
using System.Text.Json;
using Mqtt;
using Common;

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
            _mqttClient.Subscribe("sys/client/+/customers/callback/+");
        }

        private void MqttClient_OnReceiveMessage(MqttMessage message)
        {
            if (RegexEvaluator.Evaluate(ClientNotificationName.Customers.Value, message.Topic))
            {
                OnReceiveCustomers?.Invoke(message);
                return;
            }
        }

        public void Publish(ClientPublishCommand command, object body)
        {
            switch (command)
            {
                case ClientPublishCommand.AddCustomer:
                    var topic = $"sys/client/{_mqttClient.ClientId}/customers/add/{Guid.NewGuid()}";
                    var message = new MqttMessage(topic, body);
                    _mqttClient.Publish(message);
                    break;
            }
        }

        public T PublishAndWaitCallback<T>(ClientPublishCommand command, object body, int timeout) where T : class
        {
            switch (command)
            {
                case ClientPublishCommand.RequestCustomers:
                    var topic = $"sys/client/{_mqttClient.ClientId}/customers/request/{Guid.NewGuid()}";
                    var message = new MqttMessage(topic, null);
                    var callback = _mqttClient.PublishAndWaitCallback(message, timeout);
                    var response = JsonSerializer.Deserialize<T>((string)callback.Result.Payload);
                    return response;
            }

            return default;
        }
    }
}
