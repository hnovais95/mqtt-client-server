using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mqtt;

namespace MqttServer
{
    public class Interactor
    {
        private IMqtt _mqttClient;

        public Interactor(IMqtt mqttClient)
        {
            _mqttClient = mqttClient;
            _mqttClient.OnConnect += MqttClient_OnConnect;
            _mqttClient.OnReceiveMessage += MqttClient_OnReceiveMessage;
        }

        private void MqttClient_OnConnect()
        {
            _mqttClient.Subscribe("client/request/+");
        }

        private void MqttClient_OnReceiveMessage(string clientId, string topic, Dictionary<string, object>? body)
        {
            var messageId = topic.Split('/').Last() ?? "";
            var response = new Dictionary<string, object>();
            response.Add("data", "value");
            _mqttClient.Publish($"client/{clientId}/response/{messageId}", response);
        }
    }
}
