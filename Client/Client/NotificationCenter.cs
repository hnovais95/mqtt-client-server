using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mqtt;
using Common;

namespace App
{
    class NotificationCenter
    {
        #region -- D E L E G A T E S

        public delegate void DelOnResponseCustomers(MqttMessage message);

        #endregion

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
            _mqttClient.Subscribe("client/+/response/customers/+");
        }

        private void MqttClient_OnReceiveMessage(MqttMessage message)
        {
            if (RegexEvaluator.Evaluate(NotificationName.Customers.Value, message.Topic))
            {
                OnResponseCustomers?.Invoke(message);
                return;
            }
        }

        public void SendMessage(NotificationName notificationName, object body)
        {
            if (notificationName.Equals(NotificationName.Customers))
            {
                var topic = $"client/{_mqttClient.ClientId}/request/customers/{Guid.NewGuid()}";
                var message = new MqttMessage(topic, null);
                _mqttClient.Publish(message);
                return;
            }
        }
    }

    public sealed class NotificationName: IEquatable<NotificationName>
    {
        public static NotificationName Customers => new(@"^client/[-\w]+/response/customers/[-\w]+$");

        public string Value { get; private set; }

        private NotificationName(string value)
        {
            Value = value;
        }

        public bool Equals(NotificationName other)
        {
            return (other != null) && (Value == other.Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
