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
            IEnumerable<HaloWaypointAchievement> haloWaypointAchievements)
        {
            return steamAchievements
                .Join(
                    haloWaypointAchievements,
                    s => s.Name,
                    h => h.Name,
                    (s, h) => new MisalignedAchievement
                    {
                        Name = s.Name,
                        GameId = h.GameId,
                        Description = s.Description,
                        IsUnlockedOnSteam = Convert.ToBoolean(s.Achieved),
                        IsUnlockedOnHaloWaypoint = h.IsUnlocked
                    })
                .Where(m => m.IsUnlockedOnSteam != m.IsUnlockedOnHaloWaypoint)
                .OrderBy(m => m.Name);
        }
    }
}
