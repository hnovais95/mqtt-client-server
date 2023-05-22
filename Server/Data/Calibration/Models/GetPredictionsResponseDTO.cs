using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using Common.Models;

namespace Server.Data
{
    public class GetPredictionsRequestDTO
    {
        [JsonPropertyName("dataframe_records")]
        public List<CalibrationParamsDTO> Records { get; set; }
    }
}

