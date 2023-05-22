using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Server.Data
{
    public class GetPredictionsResponseDTO
    {
        [JsonPropertyName("predictions")]
        public List<Double> Predictions { get; set; }
    }
}

