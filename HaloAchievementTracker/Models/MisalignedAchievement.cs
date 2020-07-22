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
        public bool IsUnlockedOnXboxLive { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var other =(MisalignedAchievement)obj;
            return Name.Equals(other.Name) && GameId.Equals(other.GameId) && Description.Equals(other.Description) 
                && IsUnlockedOnSteam.Equals(other.IsUnlockedOnSteam) && IsUnlockedOnXboxLive.Equals(other.IsUnlockedOnXboxLive);
        }

        public override int GetHashCode()
            => HashCode.Combine(Name, GameId, Description, IsUnlockedOnSteam, IsUnlockedOnXboxLive);
    }
}
