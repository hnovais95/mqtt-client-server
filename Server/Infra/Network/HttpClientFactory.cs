using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Server.Infra.Network
{
	public class HttpClientFactory
	{
        public static HttpClient Create(string baseUrl = "https://model-mock.herokuapp.com")
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}

