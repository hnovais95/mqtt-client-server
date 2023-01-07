using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Subscribing;
using MQTTnet.Client.Publishing;

namespace Mqtt
{
    public class MqttClient: IMqttClientService
    {
        private IMqttClient _client;
        private IMqttClientOptions _options;

        public bool IsConnected => _client.IsConnected;
        public string ClientId => _client.Options.ClientId;

        public event DelOnReceiveMessage OnReceiveMessage;
        public event Action OnConnect;
        public event Action OnDisconnect;

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

            _client.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("Desconectou do broker.");
                OnDisconnect?.Invoke();
            });

            _client.UseApplicationMessageReceivedHandler(e =>
            {
                Task.Run(() =>
                {
                    try
                    {
                        var topic = e.ApplicationMessage.Topic;
                        var payload = e.ApplicationMessage.Payload != null ? Encoding.UTF8.GetString(e.ApplicationMessage.Payload) : "";
                        Console.WriteLine($"Recebeu mensagem. Tópico: {topic}; Payload: {payload}");

                        var message = new MqttMessage(topic, payload);
                        OnReceiveMessage?.Invoke(message);
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine($"Erro ao receber mensagem. Exceção: {exc}.");
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

        public async void Publish(MqttMessage mqttMessage)
        {
            try
            {
                if (!_client.IsConnected)
                {
                    Console.WriteLine("Erro ao publicar mensagem. O cliente MQTT não está conectado ao broker.");
                    return;
                }

                var jsonString = mqttMessage.Payload != null ? JsonSerializer.Serialize(mqttMessage.Payload) : "";
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(mqttMessage.Topic)
                    .WithPayload(jsonString)
                    .WithAtLeastOnceQoS()
                    .Build();

                var task = await _client.PublishAsync(message);

                if (task.ReasonCode == MqttClientPublishReasonCode.Success)
                {
                    Console.WriteLine($"Mensagem publicada com sucesso. Tópico: {mqttMessage.Topic}; Payload: {jsonString}");
                } 
                else
                {
                    Console.WriteLine($"Erro publicar mensagem. Tópico: {mqttMessage.Topic}; Payload: {jsonString}");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"Erro publicar mensagem. Exceção: {exc}.");
            }
        }
    }
}
