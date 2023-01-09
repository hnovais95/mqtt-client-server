using System;

namespace Server
{
    sealed class ServerNotificationName : IEquatable<ServerNotificationName>
    {
        public static ServerNotificationName RequestCustomers => new(@"^sys/client/[-\w]+/customers/request/[-\w]+$");
        public static ServerNotificationName AddCustomer => new(@"^sys/client/[-\w]+/customers/add/[-\w]+$");

        public string Value { get; private set; }

        private ServerNotificationName(string value)
        {
            Value = value;
        }

        public bool Equals(ServerNotificationName other)
        {
            return (other != null) && (Value == other.Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
