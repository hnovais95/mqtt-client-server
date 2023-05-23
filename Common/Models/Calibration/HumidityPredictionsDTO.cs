using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Common.Models
{
    public class HumidityPredictionsDTO
    {
        [JsonPropertyName("predictions")]
        public List<Double> Predictions { get; set; }
    }
}

