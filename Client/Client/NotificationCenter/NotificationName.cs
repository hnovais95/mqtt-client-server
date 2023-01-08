using System;

namespace Client
{
    sealed class NotificationName : IEquatable<NotificationName>
    {
        public static NotificationName Customers => new(@"^sys/client/[-\w]+/response/customers/[-\w]+$");

        public string Value { get; private set; }

        private NotificationName(string value)
        {
            Value = value;
        }

        public bool Equals(NotificationName other)
        {
            return (other != null) && (Value == other.Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
