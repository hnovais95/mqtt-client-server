using System;
using Mqtt;
using Common.Models;

namespace Server
{
    class ServerNotificationCenter
    {
        #region -- D E L E G A T E S

        public delegate void DelOnGetStatus(MqttMessage message);
        public delegate void DelOnGetCustomers(MqttMessage message);
        public delegate void DelOnAddCustomer(MqttMessage message);
        public delegate void DelOnReceiveSerialData(MqttMessage message);
        public delegate void DelOnGetPredictions(MqttMessage message);

        #endregion

        #region -- E V E N T S

        public event DelOnGetStatus OnGetStatus;
        public event DelOnGetCustomers OnGetCustomers;
        public event DelOnAddCustomer OnAddCustomer;
        public event DelOnReceiveSerialData OnReceiveSerialData;
        public event DelOnGetPredictions OnGetPredictions;

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
            _mqttClient.Subscribe("sys/usb/+/data/+");
        }

        private void MqttClient_OnReceiveMessage(MqttMessage message)
        {
            if (ServerNotification.GetCustomers.MatchWith(message.Topic))
                OnGetCustomers?.Invoke(message);

            if (ServerNotification.AddCustomer.MatchWith(message.Topic))
                OnAddCustomer?.Invoke(message);

            if (ServerNotification.GetStatus.MatchWith(message.Topic))
                OnGetStatus.Invoke(message);

            if (ServerNotification.ReceiveSerialData.MatchWith(message.Topic))
            {
                OnReceiveSerialData.Invoke(message);
                Console.WriteLine($"Mensagem da porta serial recebida. Topic: {message.Topic} Payload: {message.Payload}");
            }

            if (ServerNotification.GetPredictions.MatchWith(message.Topic))
                OnGetPredictions.Invoke(message);
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
                case ServerCommand.GetPredictionsResponse:
                    topic = $"sys/server/{_mqttClient.ClientId}/calibration/predictions/get/callback/{callbackId}";
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