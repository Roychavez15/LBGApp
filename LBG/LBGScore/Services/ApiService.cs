using LBGScore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LBGScore.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Standing>> GetStandingsAsync()
        {
            var url = "http://181.39.104.93:5028/api/tablapos";
            var json = await _httpClient.GetStringAsync(url);

            return JsonSerializer.Deserialize<List<Standing>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        public async Task<List<Goleador>> GetGoleadoresAsync()
        {
            var url = "http://181.39.104.93:5028/api/goleadores"; // Ajusta tu endpoint real
            var json = await _httpClient.GetStringAsync(url);

            return JsonSerializer.Deserialize<List<Goleador>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        public async Task<List<Partido>> GetPartidosFechaAsync()
        {
            var url = "http://181.39.104.93:5028/api/partidosfecha";
            var json = await _httpClient.GetStringAsync(url);

            return JsonSerializer.Deserialize<List<Partido>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Partido>();
        }

    }

}
