using HaloAchievementTracker.Models;
using Steam.Models.SteamPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaloAchievementTracker.Helpers
{
    public class AchievementHelper
    {
        public static IEnumerable<MisalignedAchievement> GetMisalignedAchievements(
            IEnumerable<PlayerAchievementModel> steamAchievements, 
            IEnumerable<XboxLiveAchievement> xboxLiveAchievements)
        {
            return steamAchievements
                .Join(
                    xboxLiveAchievements,
                    s => s.Name,
                    h => h.Name,
                    (s, h) => new MisalignedAchievement
                    {
                        Name = s.Name,
                        GameId = h.GameId,
                        Description = s.Description,
                        IsUnlockedOnSteam = Convert.ToBoolean(s.Achieved),
                        IsUnlockedOnXboxLive = h.IsUnlocked
                    })
                .Where(m => m.IsUnlockedOnSteam != m.IsUnlockedOnXboxLive)
                .OrderBy(m => m.Name);
        }
    }
}
