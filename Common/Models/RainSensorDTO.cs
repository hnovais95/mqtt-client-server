using System;

namespace Common.Models
{
    public class RainSensorDTO
    {
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public int Region { get; set; }
        public bool Rain { get; set; }
    }
}