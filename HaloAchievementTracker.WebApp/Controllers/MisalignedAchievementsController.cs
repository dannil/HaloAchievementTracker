using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using HaloAchievementTracker.Common;
using HaloAchievementTracker.Common.Helpers;
using HaloAchievementTracker.Common.Models;
using HaloAchievementTracker.Common.Services;
using HaloAchievementTracker.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HaloAchievementTracker.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MisalignedAchievementsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;

        private readonly ISteamService _steamService;
        private readonly IOpenXBLService _openXBLService;

        private static readonly string CACHE_KEY_PREFIX = "MisalignedAchievementsController";
        private static readonly string CACHE_DURATION_KEY = $"Cache:{CACHE_KEY_PREFIX}:Duration";

        public MisalignedAchievementsController(IConfiguration configuration, IMemoryCache cache, ISteamService steamService, IOpenXBLService openXBLService)
        {
            _configuration = configuration;
            _cache = cache;
            _steamService = steamService;
            _openXBLService = openXBLService;
        }

        [HttpGet]
        public async Task<IEnumerable<MisalignedAchievement>> GetQuery([FromQuery] MisalignedAchievementQuery query)
        {
            var steamId64 = query.SteamId64;
            var xboxLiveGamertag = query.XboxLiveGamertag;

            var cacheKey = $"{CACHE_KEY_PREFIX}-GetQuery-{steamId64}-{xboxLiveGamertag}";

            return await _cache.GetOrCreateAsync(cacheKey, async cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_configuration.GetValue<int>(CACHE_DURATION_KEY));

                var friendsByGamertag = await _openXBLService.GetFriendsByGamertagAsync(xboxLiveGamertag);
                var xboxLiveXuid = friendsByGamertag.ProfileUsers[0].Id;
                var xboxLiveAchievementsRequest = _openXBLService.GetAchievementsAsync(xboxLiveXuid, Constants.HALO_MCC_XBOX_LIVE_APP_ID);

                var steamAchievementsRequest = _steamService.GetAchievementsByScrapingAsync(Constants.HALO_MCC_STEAM_APP_ID, steamId64);

                var steamAchievements = await steamAchievementsRequest;
                var xboxLiveAchievements = await xboxLiveAchievementsRequest;

                return AchievementHelper.GetMisalignedAchievements(steamAchievements, xboxLiveAchievements.Achievements);
            });
        }
    }
}
