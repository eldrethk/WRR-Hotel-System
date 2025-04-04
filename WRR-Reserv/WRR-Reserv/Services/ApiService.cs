using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WRR_Reserv.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<string>> GetValuesAsync()
        {
            var response = await _httpClient.GetAsync("api/values"); // Relative to the base address
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<string>>() ?? new List<string>();
        }

    }
}
