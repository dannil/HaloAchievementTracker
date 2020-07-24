using System;
using System.Collections.Generic;
using System.Text;

namespace HaloAchievementTracker.Models
{
    public interface IAchievement
    {
        public string Name { get; set; }
        public string GameId { get; set; }
        public bool IsUnlocked { get; set; }


    }
}
