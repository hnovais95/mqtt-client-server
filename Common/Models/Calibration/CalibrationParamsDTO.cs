using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

namespace Common.Models
{
    public class CalibrationParamsDTO
    {
        [JsonPropertyName("Er")]
        public double Er { get; set; }

        [JsonPropertyName("Sigma")]
        public long Sigma { get; set; }
    }
}

