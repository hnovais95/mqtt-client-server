﻿using Common;

namespace Server
{
    sealed class ServerNotification
    {
        public static ServerNotification GetCustomers => new(@"^sys/client/[-\w]+/customers/get/[-\w]+$");
        public static ServerNotification AddCustomer => new(@"^sys/client/[-\w]+/customers/add/[-\w]+$");
        public static ServerNotification GetStatus => new(@"^sys/client/[-\w]+/status/[-\w]+$");
        public static ServerNotification SerialData => new(@"^sys/usb/[-\w]+/data/[-\w]+$");

        public string Value { get; private set; }

        private ServerNotification(string value)
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
