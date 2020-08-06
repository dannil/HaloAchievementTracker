using HaloAchievementTracker.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace HaloAchievementTracker.Common.Helpers
{
    public class AchievementHelper
    {
        public static IEnumerable<MisalignedAchievement> GetMisalignedAchievements(
            IEnumerable<IAchievement> steamAchievements, 
            IEnumerable<IAchievement> xboxLiveAchievements)
        {
            return steamAchievements
                .Join(
                    xboxLiveAchievements,
                    s => s.Name,
                    x => x.Name,
                    (s, x) => new MisalignedAchievement
                    {
                        Name = s.Name,
                        Game = s.Game,
                        Description = s.Description,
                        IsUnlockedOnSteam = s.IsUnlocked,
                        IsUnlockedOnXboxLive = x.IsUnlocked
                    })
                .Where(m => m.IsUnlockedOnSteam != m.IsUnlockedOnXboxLive)
                .OrderBy(m => m.Name);
        }

        public static Game GetGameFromDescription(string description)
        {
            //var games = new HashSet<string> { "Halo CE", "Halo: CE", "Halo 2", "Halo 2 MP", "Halo 2A MP", "Halo 3", "H3: ODST", "Halo: Reach", "Halo 4" };
            //if (games.Any(g => description.StartsWith(g))) {
            //    return games.First(g => description.Contains(g));
            //}
            Regex r = new Regex(@"^H(?:alo(?:: (?:Reach|CE)| (?:2A? MP|CE|2|[34]))|3: ODST)");
            Match m = r.Match(description);
            var value = m.Value;
            if (string.IsNullOrEmpty(value))
            {
                value = "Cross Game";
            }
            return GameFactory.Get(value);
            //if (m.Success)
            //{
            //    return Game.GetGame(m.Value);
            //}
            //return Game.GetGame("Cross Game");
        }
    }
}
