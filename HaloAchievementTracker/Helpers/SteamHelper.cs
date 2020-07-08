using HaloAchievementTracker.Models;
using Steam.Models.SteamPlayer;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HaloAchievementTracker.Helpers
{
    public class SteamHelper
    {
        private readonly ISteamWebInterfaceFactory webInterfaceFactory;

        public SteamHelper(ISteamWebInterfaceFactory webInterfaceFactory)
        {
            this.webInterfaceFactory = webInterfaceFactory;
        }

        public async Task<PlayerAchievementResultModel> GetAchievementsAsync(uint appId, ulong steamId)
        {
            var steamInterface = webInterfaceFactory.CreateSteamWebInterface<SteamUserStatsAdapter>(new HttpClient());
            var stats = await steamInterface.GetPlayerAchievementsAsync(appId, steamId);
            return stats.Data;
        }

    }
}
