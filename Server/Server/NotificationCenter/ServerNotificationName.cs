using System;

namespace Server
{
    sealed class ServerNotificationName : IEquatable<ServerNotificationName>
    {
        public static ServerNotificationName Customers => new(@"^sys/client/[-\w]+/customers/request/[-\w]+$");

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
