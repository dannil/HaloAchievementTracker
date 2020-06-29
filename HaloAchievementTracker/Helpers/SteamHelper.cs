using Steam.Models.SteamPlayer;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HaloAchievementTracker
{
    public class SteamHelper
    {
        private readonly SteamWebInterfaceFactory webInterfaceFactory;

        public SteamHelper(string apiKey)
        {
            webInterfaceFactory = new SteamWebInterfaceFactory(apiKey);
        }

        public async Task<PlayerAchievementResultModel> GetAchievementsAsync(uint appId, ulong steamId)
        {
            var steamInterface = webInterfaceFactory.CreateSteamWebInterface<SteamUserStats>(new HttpClient());
            var stats = await steamInterface.GetPlayerAchievementsAsync(appId, steamId);
            return stats.Data;
        }

    }
}
