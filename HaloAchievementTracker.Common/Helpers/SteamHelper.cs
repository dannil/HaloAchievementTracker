using HaloAchievementTracker.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HaloAchievementTracker.Common.Helpers
{
    public class SteamHelper
    {
        private static readonly Dictionary<string, Game> ACHIEVEMENTS_MISSING_IDENTIFIERS = new Dictionary<string, Game>
        {
            { "Checkmate", GameFactory.Get("Halo 4") },
            { "I Was Wondering What Would Break First", GameFactory.Get("Halo 4") },
            { "Pest Control", GameFactory.Get("Halo 4") },
            { "Well...Maybe One or Two", GameFactory.Get("Halo 3: ODST") },
            { "Remove the Bishops From the Board", GameFactory.Get("Halo 4") },
            { "Knightbane", GameFactory.Get("Halo 4") },
            { "Dogcatcher", GameFactory.Get("Halo 4") }
        };

        public static bool HasMissingGameIdentifier(string achievementName)
        {
            return ACHIEVEMENTS_MISSING_IDENTIFIERS.ContainsKey(achievementName);
        }

        public static Game Get(string name)
        {
            if (!ACHIEVEMENTS_MISSING_IDENTIFIERS.ContainsKey(name))
            {
                throw new ArgumentException();
            }
            return ACHIEVEMENTS_MISSING_IDENTIFIERS[name];
        }
    }
}
