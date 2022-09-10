using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using MQTTnet.Client.Publishing;
using Newtonsoft.Json;

namespace Mqtt
{
    public class MqttClient: IMqtt
    {
        private IMqttClient _client;
        private IMqttClientOptions _options;
        public event ReceiveMessageDelegate OnReceiveMessage;
        public event ConnectDelegate OnConnect;

        public MqttClient(string server, int port)
        {
            var mqttFactory = new MqttFactory();
            _client = mqttFactory.CreateMqttClient();
            _options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                .WithTcpServer(server, port)
                .WithCleanSession()
                .Build();

            RegisterCallbacks();
        }

        private void RegisterCallbacks()
        {
            _client.UseConnectedHandler(e =>
            {
                Console.WriteLine("Conectou ao broker com sucesso.");
                OnConnect?.Invoke();
            });

            _client.UseApplicationMessageReceivedHandler(e =>
            {
                Task.Run(() =>
                {
                    try
                    {
                        var clientId = e.ClientId;
                        var topic = e.ApplicationMessage.Topic;
                        var payload = e.ApplicationMessage.Payload != null ? Encoding.UTF8.GetString(e.ApplicationMessage.Payload) : "";
                        Console.WriteLine($"Recebeu mensagem de {clientId}. Tópico: {topic}; Payload: {payload}");

                        var json = String.IsNullOrEmpty(payload) ? null : JsonConvert.DeserializeObject<Dictionary<string, object>>(payload);
                        var message = new MqttMessage(topic, json, clientId);
                        OnReceiveMessage?.Invoke(message);
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine($"Erro ao decodificar mensagem. Exceção: {exc}.");
                    }
                });
            });
        }

        public void Connect()
        {
            _client.ConnectAsync(_options);
        }

        public void Disconnect()
        {
            _client.DisconnectAsync();
        }

        public async void Subscribe(string topic)
        {
            try
            {
                if (!_client.IsConnected)
                {
                    Console.WriteLine($"Erro ao subscrever tópico {topic}. O cliente MQTT não está conectado ao broker.");
                    return;
                }

                var topicFilter = new MqttTopicFilterBuilder()
                    .WithTopic(topic)
                    .Build();

                var task = await _client.SubscribeAsync(topicFilter);

                if ((task.Items.Count > 0) &&
                    (task.Items[0].ResultCode == MqttClientSubscribeResultCode.GrantedQoS0) ||
                    (task.Items[0].ResultCode == MqttClientSubscribeResultCode.GrantedQoS1) ||
                    (task.Items[0].ResultCode == MqttClientSubscribeResultCode.GrantedQoS2))
                {
                    Console.WriteLine($"Tópico {topic} assinado com sucesso.");
                }
                else
                {
                    Console.WriteLine($"Erro subscrever tópico {topic}.");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Erro subscrever tópico {topic}. Exceção: {exc}.");
            }
        }

        public async void Publish(string topic, Dictionary<string, object>? payload)
        {
            try
            {
                if (!_client.IsConnected)
                {
                    Console.WriteLine("Erro ao publicar mensagem. O cliente MQTT não está conectado ao broker.");
                    return;
                }

                var jsonString = payload != null ? JsonConvert.SerializeObject(payload) : "";
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(jsonString)
                    .WithAtLeastOnceQoS()
                    .Build();

                var task = await _client.PublishAsync(message);

                if (task.ReasonCode == MqttClientPublishReasonCode.Success)
                {
                    Console.WriteLine($"Mensagem publicada com sucesso. Tópico: {topic}; Payload: {jsonString}");
                } 
                else
                {
                    Console.WriteLine($"Erro publicar mensagem. Tópico: {topic}; Payload: {jsonString}");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Erro publicar mensagem. Exceção: {exc}.");
            }
        }
    }
}
