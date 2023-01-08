using System;

namespace Server
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    class Subscribe: Attribute
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
