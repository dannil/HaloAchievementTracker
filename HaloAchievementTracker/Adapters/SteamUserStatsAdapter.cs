//using AutoMapper;
//using Steam.Models;
//using Steam.Models.SteamCommunity;
//using Steam.Models.SteamPlayer;
//using SteamWebAPI2.Interfaces;
//using SteamWebAPI2.Utilities;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace HaloAchievementTracker.Adapters
//{
//    public class SteamUserStatsAdapter
//    {
//        private readonly SteamUserStats steamUserStats;

//        public SteamUserStatsAdapter()
//        {

//        }

//        public SteamUserStatsAdapter(IMapper mapper, ISteamWebRequest steamWebRequest, ISteamWebInterface steamWebInterface = null)
//            => steamUserStats = new SteamUserStats(mapper, steamWebRequest, steamWebInterface);

//        public virtual Task<ISteamWebResponse<IReadOnlyCollection<GlobalAchievementPercentageModel>>> GetGlobalAchievementPercentagesForAppAsync(uint appId)
//            => steamUserStats.GetGlobalAchievementPercentagesForAppAsync(appId);

//        public virtual Task<ISteamWebResponse<IReadOnlyCollection<GlobalStatModel>>> GetGlobalStatsForGameAsync(uint appId, IReadOnlyList<string> statNames, DateTime? startDate = null, DateTime? endDate = null)
//            => steamUserStats.GetGlobalStatsForGameAsync(appId, statNames, startDate, endDate);

//        public virtual Task<ISteamWebResponse<uint>> GetNumberOfCurrentPlayersForGameAsync(uint appId)
//            => steamUserStats.GetNumberOfCurrentPlayersForGameAsync(appId);

//        public virtual Task<ISteamWebResponse<PlayerAchievementResultModel>> GetPlayerAchievementsAsync(uint appId, ulong steamId, string language = "en_us") 
//            => steamUserStats.GetPlayerAchievementsAsync(appId, steamId, language);

//        public virtual Task<ISteamWebResponse<SchemaForGameResultModel>> GetSchemaForGameAsync(uint appId, string language = "")
//            => steamUserStats.GetSchemaForGameAsync(appId, language);

//        public virtual Task<ISteamWebResponse<UserStatsForGameResultModel>> GetUserStatsForGameAsync(ulong steamId, uint appId)
//            => steamUserStats.GetUserStatsForGameAsync(steamId, appId);
//    }
//}
