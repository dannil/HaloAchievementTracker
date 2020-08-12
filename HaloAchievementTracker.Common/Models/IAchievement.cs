using System;
using System.Collections.Generic;
using System.Text;

namespace HaloAchievementTracker.Common.Models
{
    public interface IAchievement
    {
        string Name { get; set; }
        Game Game { get; set; }
        string Description { get; set; }
        bool IsUnlocked { get; set; }
    }
}
