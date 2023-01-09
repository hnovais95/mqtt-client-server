using System;
using System.Collections.Generic;
using System.Text.Json;
using Mqtt;
using Common;
using Common.Models;

namespace Server
{
    class ServerNotificationCenter: IServerNotificationCenter
    {
        public event DelOnRequestCustomers OnRequestCustomers;
        public event DelOnAddCustomer OnAddCustomer;

        private readonly IMqttClientService _mqttClient;

        public ServerNotificationCenter(IMqttClientService mqttClient)
        {
            _mqttClient = mqttClient;
            _mqttClient.OnReceiveMessage += MqttClient_OnReceiveMessage;
        }

        private void MqttClient_OnReceiveMessage(MqttMessage message)
        {
            if (RegexEvaluator.Evaluate(ServerNotificationName.RequestCustomers.Value, message.Topic))
            {
                OnRequestCustomers?.Invoke(message);
                return;
            }

            if (RegexEvaluator.Evaluate(ServerNotificationName.AddCustomer.Value, message.Topic))
            {
                OnAddCustomer?.Invoke(message);
                return;
            }
        }

        public void Publish(ServerPublishCommand command, object body, string callbackId)
        {
            switch (command)
            {
                case ServerPublishCommand.SendCustomers:
                    var topic = $"sys/client/{_mqttClient.ClientId}/customers/callback/{callbackId}";
                    var message = new MqttMessage(topic, body);
                    _mqttClient.Publish(message);
                    break;
            }
        }

        public T PublishAndWaitCallback<T>(ServerPublishCommand command, object body, int timeout) where T : class
        {
            return default;
        }
    }
}
