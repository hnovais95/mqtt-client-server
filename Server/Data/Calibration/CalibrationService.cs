using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Server.Domain;
using Common.Models;

namespace Server.Data
{
    public class CalibrationService: ICalibrationService
    {
        private readonly HttpClient _httpClient;

        public CalibrationService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<HumidityPredictionsDTO> GetPredictions(CalibrationRecordsDTO records)
		{
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/invocations", records);
            response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<HumidityPredictionsDTO>();
        }
    }
}

