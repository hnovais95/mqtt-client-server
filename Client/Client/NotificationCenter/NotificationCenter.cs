using System;
using System.Collections.Generic;
using System.Text.Json;
using Mqtt;
using Common;

namespace Client
{
    class NotificationCenter: INotificationCenter
    {

        #region -- E V E N T S

        public event DelOnResponseCustomers OnResponseCustomers;

        #endregion

        private readonly IMqttClientService _mqttClient;

        public NotificationCenter(IMqttClientService mqttClient)
        {
            _mqttClient = mqttClient;
            _mqttClient.OnConnect += MqttClient_OnConnect;
            _mqttClient.OnReceiveMessage += MqttClient_OnReceiveMessage;
        }

        private void MqttClient_OnConnect()
        {
            _mqttClient.Subscribe("sys/client/+/response/customers/+");
        }

        private void MqttClient_OnReceiveMessage(MqttMessage message)
        {
            if (RegexEvaluator.Evaluate(NotificationName.Customers.Value, message.Topic))
            {
                OnResponseCustomers?.Invoke(message);
                return;
            }
        }

        public void Publish(NotificationName notificationName, object body)
        {
        }

        public T PublishAndWaitCallback<T>(NotificationName notificationName, object body, int timeout) where T : class
        {
            if (notificationName.Equals(NotificationName.Customers))
            {
                var topic = $"sys/client/{_mqttClient.ClientId}/request/customers/{Guid.NewGuid()}";
                var message = new MqttMessage(topic, null);
                var callback = _mqttClient.PublishAndWaitCallback(message, timeout);
                var response = JsonSerializer.Deserialize<T>((string)callback.Result.Payload);
                return response;
            }

            return default;
        }
    }
}
