using System;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using Mqtt;
using Common.Models;
using System.Text.Json;

namespace USBDriver
{
	public class USBDriver
    {
        private static XBeeFacade s_xbee;
        private static IMqttClientService s_mqttClient;

        public static void Configure(IMqttClientService mqttClient)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var serialPort = new SerialPort();
            serialPort.PortName = IsWindows() ? "COM1" : "/dev/ttys001";
            s_xbee = new XBeeFacade(new XBee(serialPort));
            s_xbee.OnReceiveData += XBee_OnReceiveData;

            s_mqttClient = mqttClient;
            s_mqttClient.OnConnect += MqttClient_OnConnect;
            s_mqttClient.OnDisconnect += MqttClient_OnDisconnect;
            s_mqttClient.OnReceiveMessage += MqttClient_OnReceiveMessage;
        }

        private static bool IsWindows()
        {
            var windowsPlatform = System.Runtime.InteropServices.OSPlatform.Windows;
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(windowsPlatform);
        }

		public static void Start()
        {
            Console.WriteLine("Iniciou USBDriver!");
            s_mqttClient.Connect();
            s_xbee.Connect();
            while (true) ;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            s_mqttClient.OnDisconnect -= MqttClient_OnDisconnect;
            s_mqttClient.Disconnect();
            s_xbee.Disconnect();
            Console.WriteLine($"Erro inesperado por exceção sem tratamento. Terminating: {e.IsTerminating} Exc: {(Exception)e.ExceptionObject}.");
        }

        private static void XBee_OnReceiveData(byte length, byte address, byte[] bufferBytes)
        {
            Task.Run(() =>
            {
                var data = new XbeeDTO()
                {
                    Length = length,
                    Address = address,
                    BufferBytes = bufferBytes
                };
                var topic = $"sys/usb/{s_mqttClient.ClientId}/data/{Guid.NewGuid()}";
                var message = new MqttMessage(topic, data);
                s_mqttClient.Publish(message);
                Console.WriteLine($"Mensagem enviada ao broker MQTT. Topic: {message.Topic} Payload: {message.Payload}");
            });
        }

        private static void MqttClient_OnConnect()
        {
            Console.WriteLine("USBDriver conectado ao broker MQTT!");
            s_mqttClient.Subscribe("sys/server/+/usb/send-transparent/+");
            s_mqttClient.Subscribe("sys/server/+/usb/send/+");
        }

        private static void MqttClient_OnDisconnect()
        {
            Console.WriteLine("Tentando reconectar ao broker MQTT...");
            s_mqttClient.Connect();
        }

        private static void MqttClient_OnReceiveMessage(MqttMessage message)
        {
            if (USBDriverNotification.SendDataTransparent.MatchWith(message.Topic))
            {
                var data = JsonSerializer.Deserialize<XBeeBuffer>((string)message.Payload);
                if (data != null) s_xbee.SendData(data.Buffer);
            }

            if (USBDriverNotification.SendData.MatchWith(message.Topic))
            {
                var data = JsonSerializer.Deserialize<XBeeAddressableBuffer>((string)message.Payload);
                if (data != null) s_xbee.SendData(data.Address, data.Buffer);
            }
        }
    }
}

