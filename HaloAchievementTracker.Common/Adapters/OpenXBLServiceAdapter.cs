using HaloAchievementTracker.Common.Models;
using HaloAchievementTracker.Common.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HaloAchievementTracker.Common.Adapters
{
    public class OpenXBLServiceAdapter : IXboxLiveApiAdapter
    {
        private readonly IOpenXBLService _service;

        public OpenXBLServiceAdapter(HttpClient httpClient)
        {
            _service = new OpenXBLService(httpClient);
        }

        public async Task<string> GetXuidByGamertagAsync(string gamertag)
        {
            var response = await _service.GetFriendsByGamertagAsync(gamertag);
            return response.ProfileUsers[0].Id;
        }

        public async Task<IEnumerable<IAchievement>> GetAchievementsAsync(string xuid, uint titleId)
        {
            var response = await _service.GetAnotherPlayersAchievementsAsync(xuid, titleId);
            return response.Achievements;
        }

        public async Task<Task> Warmup()
        {
            await GetXuidByGamertagAsync("example");
            return Task.CompletedTask;
        }
    }
}
