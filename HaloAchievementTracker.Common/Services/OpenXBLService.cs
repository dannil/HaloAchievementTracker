using HaloAchievementTracker.Common.Converters;
using HaloAchievementTracker.Common.Models;
using HaloAchievementTracker.Common.Models.OpenXBL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HaloAchievementTracker.Common.Services
{
    /// <summary>
    /// Service for OpenXBL
    /// </summary>
    public class OpenXBLService : IOpenXBLService
    {
        private readonly HttpClient _httpClient;

        public OpenXBLService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OpenXBLFriendsSearchResponse> GetFriendsByGamertagAsync(string gamertag)
        {
            var endpoint = $"friends/search?gt={gamertag}";
            var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<OpenXBLFriendsSearchResponse>(responseBody);
            }
            throw new HttpRequestException();
        }

        public async Task<OpenXBLAnotherPlayersAchievementsResponse> GetAchievementsAsync(string xuid, uint titleId)
        {
            //var settings = new JsonSerializerSettings();
            //settings.Converters.Add(new OpenXBLProgressStateConverter());

            var endpoint = $"achievements/player/{xuid}/title/{titleId}";
            var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<OpenXBLAnotherPlayersAchievementsResponse>(responseBody/*, settings*/);
            }
            throw new HttpRequestException();
        }

        public async Task<Task> FindClubs(string query)
        {
            var endpoint = $"clubs/find?q={query}";
            var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {

            }
            return Task.CompletedTask;
        }
    }

    public interface IOpenXBLService
    {
        Task<OpenXBLFriendsSearchResponse> GetFriendsByGamertagAsync(string gamertag);

        Task<OpenXBLAnotherPlayersAchievementsResponse> GetAchievementsAsync(string xuid, uint titleId);

        Task<Task> FindClubs(string query);
    }
}
