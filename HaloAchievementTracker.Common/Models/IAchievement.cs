using System;
using System.Collections.Generic;
using System.Text;

namespace HaloAchievementTracker.Common.Models
{
    public interface IAchievement
    {
        string Name { get; set; }
        string GameId { get; set; }
        bool IsUnlocked { get; set; }


    }
}
