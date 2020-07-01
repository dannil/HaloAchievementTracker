using System;
using System.Collections.Generic;
using System.Text;

namespace HaloAchievementTracker.Models
{
    public class MisalignedAchievement
    {
        public string Name { get; set; }
        public string GameId { get; set; }
        public string Description { get; set; }
        public bool IsUnlockedOnSteam { get; set; }
        public bool IsUnlockedOnHaloWaypoint { get; set; }
    }
}
