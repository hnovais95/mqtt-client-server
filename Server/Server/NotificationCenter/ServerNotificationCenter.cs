using System;
using Mqtt;
using Common.Models;

namespace Server
{
    class ServerNotificationCenter: IServerNotificationCenter
    {
        #region -- E V E N T S

        public event DelOnRequestStatus OnRequestStatus;
        public event DelOnRequestCustomers OnRequestCustomers;
        public event DelOnAddCustomer OnAddCustomer;

        #endregion

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
            if (ServerNotificationName.GetCustomers.MatchWith(message.Topic))
                OnRequestCustomers?.Invoke(message);

            if (ServerNotificationName.AddCustomer.MatchWith(message.Topic))
                OnAddCustomer?.Invoke(message);

            if (ServerNotificationName.GetStatus.MatchWith(message.Topic))
                OnRequestStatus.Invoke(message);
        }

        public void Publish(ServerCommand command, object body, string callbackId)
        {
            string topic;

            switch (command)
            {
                case ServerCommand.GetCustomersResponse:
                    topic = $"sys/server/{_mqttClient.ClientId}/customers/get/callback/{callbackId}";
                    break;
                case ServerCommand.AddCustomerResponse:
                    topic = $"sys/server/{_mqttClient.ClientId}/customers/add/callback/{callbackId}";
                    break;
                default:
                    return;
            }

            var message = new MqttMessage(topic, body);
            _mqttClient.Publish(message);
        }

        public RequestResult PublishAndWaitCallback(ServerCommand command, object body, int timeout)
        {
            throw new NotImplementedException();
        }
    }
}
