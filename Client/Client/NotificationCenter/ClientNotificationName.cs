using System;

namespace Client
{
    sealed class ClientNotificationName : IEquatable<ClientNotificationName>
    {
        public static ClientNotificationName Customers => new(@"^sys/client/[-\w]+/customers/get/callback/[-\w]+$");

        public string Value { get; private set; }

        private ClientNotificationName(string value)
        {
            Value = value;
        }

        public bool Equals(ClientNotificationName other)
        {
            return (other != null) && (Value == other.Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
