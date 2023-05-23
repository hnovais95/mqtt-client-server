using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Common.Models
{
    public class CalibrationRecordsDTO
    {
        [JsonPropertyName("dataframe_records")]
        public List<CalibrationDTO> Records { get; set; }
    }
}

