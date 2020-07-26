using System;
using System.Collections.Generic;
using System.Text;

namespace HaloAchievementTracker.Common.Models
{
    public class XboxLiveAchievement : IAchievement
    {
        public string Name { get; set; }
        public string GameId { get; set; }
        public string Description { get; set; }
        public bool IsUnlocked { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var other = (XboxLiveAchievement)obj;
            return Name.Equals(other.Name) && GameId.Equals(other.GameId) && IsUnlocked.Equals(other.IsUnlocked);
        }

        public override int GetHashCode()
            => HashCode.Combine(Name, GameId, IsUnlocked);
    }
}
