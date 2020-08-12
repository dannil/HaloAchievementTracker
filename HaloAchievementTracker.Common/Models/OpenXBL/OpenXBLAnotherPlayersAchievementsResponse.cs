using HaloAchievementTracker.Common.Converters;
using HaloAchievementTracker.Common.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HaloAchievementTracker.Common.Models.OpenXBL
{
    public class OpenXBLAnotherPlayersAchievementsResponse
    {
        public List<OpenXBLAchievement> Achievements { get; set; }
    }

    public class OpenXBLAchievement : IAchievement
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Game Game { get; set; }

        public string Description { get; set; }

        [JsonConverter(typeof(OpenXBLProgressStateConverter))]
        public bool IsUnlocked { get; set; }

        [JsonConstructor]
        public OpenXBLAchievement(string description)
        {
            Game = AchievementHelper.GetGameFromDescription(description);
        }
    }

}
