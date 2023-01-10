using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
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
        private List<MqttResquest> _requestList = new();

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
                        var receivedMessage = new MqttMessage(topic, payload);

                        lock (_requestList)
                        {
                            var request = _requestList.Find(x => x.SentMessage.GetID() == receivedMessage.GetID());
                            if (request != null)
                            {
                                request.CallbackMessage = receivedMessage;
                                request.WaitCallbackManualResetEvent.Set();
                                return;
                            }
                        }

                        Console.WriteLine($"Recebeu mensagem. Tópico: {topic}; Payload: {payload}");
                        var message = new MqttMessage(topic, payload);
                        OnReceiveMessage?.Invoke(receivedMessage);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Erro ao receber mensagem. Exceção: {e}.");
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
            catch (Exception e)
            {
                Console.WriteLine($"Erro subscrever tópico {topic}. Exc.: {e}.");
            }
        }

        public async void Publish(MqttMessage mqttMessage)
        {
            try
            {
                Console.WriteLine($"Vai publicar mensagem. Tópico: {mqttMessage.Topic}");

                if (!_client.IsConnected)
                {
                    Console.WriteLine("Erro ao publicar mensagem. O cliente MQTT não está conectado ao broker.");
                    throw new Exception($"O cliente MQTT não está conectado ao broker.");
                }

                var jsonString = mqttMessage.Payload != null ? JsonSerializer.Serialize(mqttMessage.Payload) : "";
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic(mqttMessage.Topic)
                    .WithPayload(jsonString)
                    .WithAtLeastOnceQoS()
                    .Build();

                var result = await _client.PublishAsync(message);

                if (result.ReasonCode == MqttClientPublishReasonCode.Success)
                {
                    Console.WriteLine($"Mensagem publicada com sucesso. Tópico: {mqttMessage.Topic}; Payload: {jsonString}");
                } 
                else
                {
                    throw new Exception($"A mensagem não foi publicada. ReasonCode: {result.ReasonCode};");
                }
            }
            catch (Exception e)
            {
                var payload = mqttMessage.Payload != null ? mqttMessage.Payload.ToString() : "";
                Console.WriteLine($"Erro publicar mensagem. Tópico: {mqttMessage.Topic}; Payload: {payload}; Exc.: {e}.");
                throw;
            }
        }

        public async Task<MqttMessage> PublishAndWaitCallback(MqttMessage mqttMessage, int timeout)
        {
            var callbackMessage = await Task.Run(() =>
            {
                try
                {
                    MqttResquest request = new MqttResquest(mqttMessage, new ManualResetEvent(false));
                    _requestList.Add(request);
                    Console.WriteLine($"Inclui envio de mensagem aguardando callback na fila. Tópico: {mqttMessage.Topic}");

                    Publish(mqttMessage);

                    try
                    {
                        if (request.WaitCallbackManualResetEvent.WaitOne(timeout, false))
                        {
                            Console.WriteLine($"Callback recebido. Tópico: {mqttMessage.Topic}; Payload: {request.CallbackMessage.Payload}");
                            return request.CallbackMessage;
                        }
                        else
                        {
                            Console.WriteLine($"Tempo de aguardo do callback expirou. MsgID: {mqttMessage.GetID()}");
                            throw new TimeoutException($"Tempo de aguardo do callback expirou.");
                        }
                    }
                    finally
                    {
                        lock (_requestList)
                        {
                            _requestList.Remove(request);
                            Console.WriteLine($"Callaback retirado da fila. MsgID: {mqttMessage.GetID()}");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao fazer requisição. Exceção: {e}.");
                    throw;
                }
            });

            return callbackMessage;
        }
    }
}
