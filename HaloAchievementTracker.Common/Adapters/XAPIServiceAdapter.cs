using HaloAchievementTracker.Common.Models;
using HaloAchievementTracker.Common.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HaloAchievementTracker.Common.Adapters
{
    public class XAPIServiceAdapter : IXboxLiveApiAdapter
    {
        private readonly IXAPIService _service;

        public XAPIServiceAdapter(HttpClient httpClient)
        {
            _service = new XAPIService(httpClient);
        }

        public async Task<string> GetXuidByGamertagAsync(string gamertag)
        {
            var response = await _service.GetXuidByGamertagAsync(gamertag);
            return response.Xuid;
        }

        public async Task<IEnumerable<IAchievement>> GetAchievementsAsync(string xuid, uint titleId)
        {
            var response = await _service.GetAchievementsForXuidAsync(xuid, titleId);
            return response;
        }

        public Task<Task> Warmup()
        {
            throw new NotImplementedException();
        }
    }
}
