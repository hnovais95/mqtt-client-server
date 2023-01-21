using System.Threading;

namespace Mqtt
{
    class MqttResquest
    {
        public MqttMessage SentMessage { get; }
        public MqttMessage CallbackMessage { get; set; }
        public ManualResetEvent WaitCallbackManualResetEvent { get; }

        public MqttResquest(MqttMessage sentMessage, ManualResetEvent waitCallbackManualResetEvent)
        {
            SentMessage = sentMessage;
            WaitCallbackManualResetEvent = waitCallbackManualResetEvent;
        }
    }
}
