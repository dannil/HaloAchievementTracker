using System;
using System.Collections.Generic;
using System.Text;

namespace HaloAchievementTracker.Models
{
    public class SteamAchievement : IAchievement
    {
        public string Name { get; set; }
        public string GameId { get; set; }
        public string Description { get; set; }
        public bool IsUnlocked { get; set; }
    }
}
