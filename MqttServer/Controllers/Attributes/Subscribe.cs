using System;
namespace MqttServer
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class Subscribe: Attribute
    {
        private readonly string[] _topics;

        public Subscribe(params string[] topics)
        {
            _topics = topics;
        }

        public string[] GetTopics()
        {
            return _topics;
        }
    }
}
