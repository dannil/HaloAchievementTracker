using HaloAchievementTracker.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HaloAchievementTracker.Common.Adapters
{
    public interface IXboxLiveApiAdapter
    {

        Task<string> GetXuidByGamertagAsync(string gamertag);

        Task<IEnumerable<IAchievement>> GetAchievementsAsync(string xuid, uint titleId);

        Task<Task> Warmup();
    }
}
