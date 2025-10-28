using BL.Contracts.GeneralService;
using Newtonsoft.Json;
using System.Text;

namespace BL.GeneralService
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<T> GetAsync<T>(string url, Dictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            // Apply additional headers if provided
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var response = await _httpClient.SendAsync(request);

            // Check response status
            response.EnsureSuccessStatusCode();

            // Read response content as JSON
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response
            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request,
            Dictionary<string, string> headers = null)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);

            // Apply additional headers if provided
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpRequest.Headers.Add(header.Key, header.Value);
                }
            }

            // Serialize request object to JSON
            var jsonRequest = JsonConvert.SerializeObject(request);
            httpRequest.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(httpRequest);

            // Check response status
            response.EnsureSuccessStatusCode();

            // Read response content as JSON
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response
            return JsonConvert.DeserializeObject<TResponse>(jsonResponse);
        }
    }
}
