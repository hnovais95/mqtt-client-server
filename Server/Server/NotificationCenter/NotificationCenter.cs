using System;
using Mqtt;
using Common.Models;

namespace Server
{
    class NotificationCenter
    {
        #region -- D E L E G A T E S

        public delegate void DelOnRequestStatus(MqttMessage message);
        public delegate void DelOnGetRainMeansurements(MqttMessage message);
        public delegate void DelOnAddRainMeansurement(MqttMessage message);
        public delegate void DelOnGetHumidityMeansurements(MqttMessage message);
        public delegate void DelOnAddHumidityMeansurement(MqttMessage message);

        #endregion

        #region -- E V E N T S

        public event DelOnRequestStatus OnRequestStatus;
        public event DelOnGetRainMeansurements OnGetRainMeansurements;
        public event DelOnAddRainMeansurement OnAddRainMeansurement;
        public event DelOnGetHumidityMeansurements OnGetHumidityMeansurements;
        public event DelOnAddHumidityMeansurement OnAddHumidityMeansurement;

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
            _mqttClient.Subscribe("sys/client/+/status/+");
        }

        private void MqttClient_OnReceiveMessage(MqttMessage message)
        {
            if (NotificationName.GetRainMeansurements.MatchWith(message.Topic))
                OnGetRainMeansurements?.Invoke(message);

            if (NotificationName.AddRainMeansurement.MatchWith(message.Topic))
                OnAddRainMeansurement?.Invoke(message);

            if (NotificationName.GetHumidityMeansurements.MatchWith(message.Topic))
                OnGetHumidityMeansurements?.Invoke(message);

            if (NotificationName.AddHumidityMeansurement.MatchWith(message.Topic))
                OnAddHumidityMeansurement?.Invoke(message);

            if (NotificationName.GetStatus.MatchWith(message.Topic))
                OnRequestStatus.Invoke(message);
        }

        public void Publish(ServerCommand command, object body, string callbackId)
        {
            string topic;

            switch (command)
            {
                case ServerCommand.GetRainMeansurementResponse:
                    topic = $"sys/server/{_mqttClient.ClientId}/rain/get/callback/{callbackId}";
                    break;
                case ServerCommand.GetHumidityMeansurementResponse:
                    topic = $"sys/server/{_mqttClient.ClientId}/humidity/add/callback/{callbackId}";
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
