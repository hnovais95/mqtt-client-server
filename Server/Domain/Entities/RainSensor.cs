using System;

namespace Server.Domain
{
    public class RainSensor
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new Exception("O nome do sensor é obrigatório.");
                }
                _name = value;
            }
        }

        public DateTime Timestamp { get; set; }
        public int Region { get; set; }
        public bool Rain { get; set; }
    }
}
