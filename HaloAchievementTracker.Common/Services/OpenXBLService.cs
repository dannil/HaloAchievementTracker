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
    public class OpenXBLService
    {
        private readonly string hostname;

        private readonly HttpClient httpClient;

        public OpenXBLService()
        {
            hostname = "https://xbl.io/api/v2";
        }

        public OpenXBLService(string apiKey) : this()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-Authorization", apiKey);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<OpenXBLFriendsSearchResponse> GetFriendsByGamertagAsync(string gamerTag)
        {
            var endpoint = $"{hostname}/friends/search?gt={gamerTag}";
            var response = await httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OpenXBLFriendsSearchResponse>(responseBody);
        }

        public async Task<OpenXBLAnotherPlayersAchievementsResponse> GetAchievementsAsync(string xuid, string titleId)
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
}
