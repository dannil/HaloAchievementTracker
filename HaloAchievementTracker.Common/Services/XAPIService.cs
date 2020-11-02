using HaloAchievementTracker.Common.Models;
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

        //public Task<object> GetXuidByGamertagAsync(string gamertag)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<object> GetAchievementsAsync(string xuid, uint titleId)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<Task> Warmup()
        {
            throw new NotImplementedException();
        }
    }

    public interface IXAPIService
    {
        //Task<object> GetFriendsByGamertagAsync(string gamertag);

        //Task<object> GetAchievementsAsync(string xuid, uint titleId);
    }
}
