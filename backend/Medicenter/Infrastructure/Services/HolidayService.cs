using Application.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class HolidaysService : IHolidaysService
    {
        private readonly IHttpClientFactory _httpClientFactory = null!;
        public HttpClient HolidayClient { get; set; }

        private readonly string _apiKey = "d4c7d384c72040e6abdbec845fc4a0c3";

        public HolidaysService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            string? httpClientName = "HolidaysApi";
            HolidayClient = _httpClientFactory.CreateClient(httpClientName ?? "");
        }

        public async Task<string> GetHolidaysAsync(string country, string year, string? month = null, string? day = null)
        {
            try
            {
                // Construimos la URL con todos los parámetros necesarios
                var url = $"?api_key={_apiKey}&country={country}&year={year}";
                if (!string.IsNullOrEmpty(month))
                    url += $"&month={month}";
                if (!string.IsNullOrEmpty(day))
                    url += $"&day={day}";

                Console.WriteLine("Calling URL: " + HolidayClient.BaseAddress + url);

                var response = await HolidayClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"StatusCode: {response.StatusCode}, Content: {content}");

                response.EnsureSuccessStatusCode();
                return content;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Error calling Holidays API: " + ex.Message);
                return $"{{\"error\":\"{ex.Message}\"}}";
            }
        }
    }
}
