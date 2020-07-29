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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HaloAchievementTracker.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MisalignedAchievementsController : ControllerBase
    {
        private readonly ILogger<MisalignedAchievementsController> _logger;

        private readonly ISteamService _steamService;
        private readonly IOpenXBLService _openXBLService;

        public MisalignedAchievementsController(ISteamService steamService, IOpenXBLService openXBLService, ILogger<MisalignedAchievementsController> logger)
        {
            _steamService = steamService;
            _openXBLService = openXBLService;

            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<MisalignedAchievement>> GetQuery([FromQuery] MisalignedAchievementQuery query)
        {
            var friendsByGamertag = await _openXBLService.GetFriendsByGamertagAsync(query.XboxLiveGamertag);
            var xboxLiveXuid = friendsByGamertag.ProfileUsers[0].Id;

            var steamAchievements = await _steamService.GetAchievementsByScrapingAsync(Constants.HALO_MCC_STEAM_APP_ID, query.SteamId64);
            var xboxLiveAchievements = await _openXBLService.GetAchievementsAsync(xboxLiveXuid, Constants.HALO_MCC_XBOX_LIVE_APP_ID);

            var misalignedAchievements = AchievementHelper.GetMisalignedAchievements(steamAchievements, xboxLiveAchievements.Achievements);
            return misalignedAchievements;
        }
    }
}
