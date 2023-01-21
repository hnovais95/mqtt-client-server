using Common;

namespace Client
{
    sealed class ClientNotificationName
    {
        public static ClientNotificationName GetCustomerResponse => new(@"^sys/client/[-\w]+/customers/get/callback/[-\w]+$");

        public string Value { get; private set; }

        private ClientNotificationName(string value)
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
