using Common;

namespace Server
{
    sealed class NotificationName
    {
        public static NotificationName GetRainMeansurements => new(@"^sys/sensor/[-\w]+/rain/get/[-\w]+$");
        public static NotificationName AddRainMeansurement => new(@"^sys/sensor/[-\w]+/rain/add/[-\w]+$");
        public static NotificationName GetHumidityMeansurements => new(@"^sys/sensor/[-\w]+/humidity/get/[-\w]+$");
        public static NotificationName AddHumidityMeansurement => new(@"^sys/sensor/[-\w]+/humidity/add/[-\w]+$");
        public static NotificationName GetStatus => new(@"^sys/client/[-\w]+/status/[-\w]+$");

        public string Value { get; private set; }

        private NotificationName(string value)
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
