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
    public class OpenXBLService : IOpenXBLService
    {
        private readonly string hostname;

        private readonly HttpClient httpClient;

        private OpenXBLService()
        {
            hostname = "https://xbl.io/api/v2";

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public OpenXBLService(string apiKey) : this()
        {
            httpClient.DefaultRequestHeaders.Add("X-Authorization", apiKey);
        }

        public async Task<OpenXBLFriendsSearchResponse> GetFriendsByGamertagAsync(string gamertag)
        {
            var endpoint = $"{hostname}/friends/search?gt={gamertag}";
            var response = await httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OpenXBLFriendsSearchResponse>(responseBody);
        }

        public async Task<OpenXBLAnotherPlayersAchievementsResponse> GetAchievementsAsync(string xuid, uint titleId)
        {
            //var settings = new JsonSerializerSettings();
            //settings.Converters.Add(new OpenXBLProgressStateConverter());

            var endpoint = $"{hostname}/achievements/player/{xuid}/title/{titleId}";
            var response = await httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OpenXBLAnotherPlayersAchievementsResponse>(responseBody/*, settings*/);
        }

    }

    public interface IOpenXBLService
    {
        Task<OpenXBLFriendsSearchResponse> GetFriendsByGamertagAsync(string gamertag);

        Task<OpenXBLAnotherPlayersAchievementsResponse> GetAchievementsAsync(string xuid, uint titleId);
    }
}
