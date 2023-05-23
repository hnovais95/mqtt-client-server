using System.Text.Json.Serialization;

namespace Common.Models
{
    public class CalibrationDTO
    {
        [JsonPropertyName("Er")]
        public double Er { get; set; }

        [JsonPropertyName("Sigma")]
        public long Sigma { get; set; }
    }
}

