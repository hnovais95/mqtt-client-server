using Common;

namespace USBDriver
{
    sealed class USBDriverNotification
    {
        public static USBDriverNotification SendDataTransparent => new(@"^sys/server/[-\w]+/usb/send-transparent/[-\w]+$");
        public static USBDriverNotification SendData => new(@"^sys/server/[-\w]+/usb/send/[-\w]+$");

        public string Value { get; private set; }

        private USBDriverNotification(string value)
        {
            Value = value;
        }

        public bool MatchWith(string topic)
        {
            return RegexEvaluator.Evaluate(Value, topic);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
