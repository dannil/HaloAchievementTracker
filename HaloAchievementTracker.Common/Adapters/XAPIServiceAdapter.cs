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

        public Task<string> GetXuidByGamertagAsync(string gamertag)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IAchievement>> GetAchievementsAsync(string xuid, uint titleId)
        {
            throw new NotImplementedException();
        }

        public Task<Task> Warmup()
        {
            throw new NotImplementedException();
        }
    }
}
