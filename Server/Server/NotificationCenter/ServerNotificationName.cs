using Common;

namespace Server
{
    sealed class ServerNotificationName
    {
        public static ServerNotificationName GetCustomers => new(@"^sys/client/[-\w]+/customers/get/[-\w]+$");
        public static ServerNotificationName AddCustomer => new(@"^sys/client/[-\w]+/customers/add/[-\w]+$");
        public static ServerNotificationName GetStatus => new(@"^sys/client/[-\w]+/status/[-\w]+$");

        public string Value { get; private set; }

        private ServerNotificationName(string value)
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
