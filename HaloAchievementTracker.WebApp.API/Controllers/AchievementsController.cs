﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HaloAchievementTracker.Common;
using HaloAchievementTracker.Common.Adapters;
using HaloAchievementTracker.Common.Helpers;
using HaloAchievementTracker.Common.Models;
using HaloAchievementTracker.Common.Services;
using HaloAchievementTracker.WebApp.API.Configuration;
using HaloAchievementTracker.WebApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HaloAchievementTracker.WebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementsController : ControllerBase
    {
        private readonly IWebAppConfiguration _configuration;
        private readonly IMemoryCache _cache;

        private readonly ISteamService _steamService;
        private readonly IXboxLiveApiAdapter _xboxLiveApiAdapter;

        private static readonly string CACHE_KEY_PREFIX = typeof(AchievementsController).Name;

        public AchievementsController(IWebAppConfiguration configuration, IMemoryCache cache, ISteamService steamService, IXboxLiveApiAdapter xboxLiveApiAdapter)
        {
            _configuration = configuration;
            _cache = cache;
            _steamService = steamService;
            _xboxLiveApiAdapter = xboxLiveApiAdapter;
        }

        [Route("misaligned")]
        [HttpGet]
        public async Task<IEnumerable<MisalignedAchievement>> GetMisaligned([FromQuery] MisalignedAchievementsQuery query)
        {
            var steamId64 = query.SteamId64;
            var xboxLiveGamertag = query.XboxLiveGamertag;

            var cacheKey = $"{CACHE_KEY_PREFIX}-GetQuery-{steamId64}-{xboxLiveGamertag}";

            async Task<IEnumerable<MisalignedAchievement>> GetMisalignedWrapper(ulong steamId64, string xboxLiveGamertag)
            {
                var xboxLiveXuid = await _xboxLiveApiAdapter.GetXuidByGamertagAsync(xboxLiveGamertag);
                var xboxLiveAchievementsRequest = _xboxLiveApiAdapter.GetAchievementsAsync(xboxLiveXuid, Constants.HALO_MCC_XBOX_LIVE_APP_ID);

                var steamAchievementsRequest = _steamService.GetAchievementsByScrapingAsync(Constants.HALO_MCC_STEAM_APP_ID, steamId64);

                var steamAchievements = await steamAchievementsRequest;
                var xboxLiveAchievements = await xboxLiveAchievementsRequest;

                return AchievementHelper.GetMisalignedAchievements(steamAchievements, xboxLiveAchievements);
            }

            return await _cache.GetOrCreateAsync(cacheKey, async cacheEntry =>
            {
                cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_configuration.Cache.MisalignedAchievementsController.Duration);

                return await GetMisalignedWrapper(steamId64, xboxLiveGamertag);
            });
        }
    }
}