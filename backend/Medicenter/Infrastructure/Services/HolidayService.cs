using System;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Infrastructure.Services
{
    public class HolidaysService : IHolidaysService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey = "d4c7d384c72040e6abdbec845fc4a0c3";

        public HolidaysService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetHolidaysAsync(string country, string year, string? month = null, string? day = null)
        {
            // ✅ Si no se especifica mes/día, usa la fecha actual
            var today = DateTime.UtcNow;
            month ??= today.Month.ToString("D2");
            day ??= today.Day.ToString("D2");

            var client = _httpClientFactory.CreateClient("HolidaysApi");
            var url = $"?api_key={_apiKey}&country={country}&year={year}&month={month}&day={day}";

            Console.WriteLine($"Calling URL: {client.BaseAddress}{url}");

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error calling Holidays API: {response.StatusCode} - {error}");
                return $"{{\"error\":\"{response.StatusCode}\",\"message\":\"{error}\"}}";
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}