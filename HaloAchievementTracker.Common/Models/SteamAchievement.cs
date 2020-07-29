using System;
using System.Collections.Generic;
using System.Text;

namespace HaloAchievementTracker.Common.Models
{
    public class SteamAchievement : IAchievement
    {
        public string Name { get; set; }
        public Game Game { get; set; }
        public string Description { get; set; }
        public bool IsUnlocked { get; set; }
    }
}
