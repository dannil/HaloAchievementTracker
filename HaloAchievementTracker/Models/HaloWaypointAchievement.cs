using System;
using System.Collections.Generic;
using System.Text;

namespace HaloAchievementTracker.Models
{
    public class HaloWaypointAchievement
    {
        public string Name { get; set; }
        public string GameId { get; set; }
        public bool IsUnlocked { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var other = (HaloWaypointAchievement)obj;
            return Name.Equals(other.Name) && GameId.Equals(other.GameId) && IsUnlocked.Equals(other.IsUnlocked);
        }

        public override int GetHashCode()
            => HashCode.Combine(Name, GameId, IsUnlocked);
    }
}
