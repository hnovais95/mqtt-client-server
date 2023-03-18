using Common;

namespace Client
{
    sealed class ClientNotification
    {
        public static ClientNotification GetCustomerResponse => new(@"^sys/client/[-\w]+/customers/get/callback/[-\w]+$");

        public string Value { get; private set; }

        private ClientNotification(string value)
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
