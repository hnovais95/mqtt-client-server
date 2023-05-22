using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Common.Models;
using Server.Domain;

namespace Server.Data
{
    public class CalibrationService: ICalibrationService
    {
        private readonly HttpClient _httpClient;

        public CalibrationService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<IEnumerable<Double>> GetPredictions(List<CalibrationParamsDTO> calibrationParams)
		{
			var requestBody = new GetPredictionsRequestDTO { Records = calibrationParams };
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/invocations", requestBody);
            response.EnsureSuccessStatusCode();
			var responseBody = await response.Content.ReadFromJsonAsync<GetPredictionsResponseDTO>();
			return responseBody.Predictions;
        }
    }
}

