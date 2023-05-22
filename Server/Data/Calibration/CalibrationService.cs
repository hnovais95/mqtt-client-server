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
            //var requestBody = new GetPredictionsRequestDTO { Records = calibrationParams };
            //         using HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/invocations", requestBody);
            //         response.EnsureSuccessStatusCode();
            //var responseBody = await response.Content.ReadFromJsonAsync<GetPredictionsResponseDTO>();
            //return responseBody.Predictions;


            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Fazer uma requisição GET
                    var requestBody = new GetPredictionsRequestDTO { Records = calibrationParams };
                    HttpResponseMessage response = await client.PostAsJsonAsync("https://model-mock.herokuapp.com/invocations", requestBody);

                    // Verificar se a requisição foi bem-sucedida
                    if (response.IsSuccessStatusCode)
                    {
                        // Ler o conteúdo da resposta
                        Console.WriteLine("A requisição deu sucesso.");
                        var responseBody = await response.Content.ReadFromJsonAsync<GetPredictionsResponseDTO>();
                        return responseBody.Predictions;
                    }
                    else
                    {
                        Console.WriteLine("A requisição falhou. Código de status: " + response.StatusCode);
                        return new List<Double>();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ocorreu um erro: " + ex.Message);
                    return new List<Double>();
                }
            }

        }
    }
}

