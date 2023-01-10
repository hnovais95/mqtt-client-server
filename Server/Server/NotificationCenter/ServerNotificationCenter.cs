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
            _mqttClient.OnConnect += MqttClient_OnConnect;
            _mqttClient.OnReceiveMessage += MqttClient_OnReceiveMessage;
        }

        private void MqttClient_OnConnect()
        {
            _mqttClient.Subscribe("sys/client/+/status/+");
        }

        private void MqttClient_OnReceiveMessage(MqttMessage message)
        {
            if (RegexEvaluator.Evaluate(ServerNotificationName.GetCustomers.Value, message.Topic))
            {
                OnRequestCustomers?.Invoke(message);
                return;
            }

            if (RegexEvaluator.Evaluate(ServerNotificationName.AddCustomer.Value, message.Topic))
            {
                OnAddCustomer?.Invoke(message);
                return;
            }

            if (RegexEvaluator.Evaluate(ServerNotificationName.GetStatus.Value, message.Topic))
            {
                var topic = $"sys/server/{_mqttClient.ClientId}/status/callback/{message.GetID()}";
                var result = new RequestResult()
                {
                    ResultCode = RequestResultCode.Success,
                    Message = "Healthy"
                };
                var callbackMessage = new MqttMessage(topic, result);
                _mqttClient.Publish(callbackMessage);
                return;
            }
        }

        public void Publish(ServerPublishCommand command, object body, string callbackId)
        {
            string topic;

            switch (command)
            {
                case ServerPublishCommand.GetCustomersResponse:
                    topic = $"sys/server/{_mqttClient.ClientId}/customers/get/callback/{callbackId}";
                    break;
                case ServerPublishCommand.AddCustomerResponse:
                    topic = $"sys/server/{_mqttClient.ClientId}/customers/add/callback/{callbackId}";
                    break;
                default:
                    return;
            }

            var message = new MqttMessage(topic, body);
            _mqttClient.Publish(message);
        }

        public RequestResult PublishAndWaitCallback(ServerPublishCommand command, object body, int timeout)
        {
            throw new NotImplementedException();
        }
    }
}
