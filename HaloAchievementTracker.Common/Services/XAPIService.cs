using HaloAchievementTracker.Common.Models;
using HaloAchievementTracker.Common.Models.XAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HaloAchievementTracker.Common.Services
{
    /// <summary>
    /// Service for X API
    /// </summary>
    public class XAPIService : IXAPIService
    {
        private readonly HttpClient _httpClient;

        public XAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<XAPIXuidByGamertagResponse> GetXuidByGamertagAsync(string gamertag)
        {
            var endpoint = $"xuid/{gamertag}";
            var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<XAPIXuidByGamertagResponse>(responseBody);
            }
            throw new HttpRequestException();
        }

        public async Task<IEnumerable<IAchievement>> GetAchievementsForXuidAsync(string xuid, uint titleId)
        {
            var endpoint = $"{xuid}/achievements/{titleId}";
            var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<XAPIAchievement>>(responseBody);
            }
            throw new HttpRequestException();
        }

        public Task<Task> Warmup()
        {
            throw new NotImplementedException();
        }
    }

    public interface IXAPIService
    {
        Task<XAPIXuidByGamertagResponse> GetXuidByGamertagAsync(string gamertag);

        Task<IEnumerable<IAchievement>> GetAchievementsForXuidAsync(string xuid, uint titleId);
    }
}
