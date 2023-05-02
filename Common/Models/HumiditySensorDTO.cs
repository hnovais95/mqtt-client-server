using System;

namespace Common.Models
{
    public class HumiditySensorDTO
    {
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
        public int Region { get; set; }
        public double Humidity { get; set; }
    }
}