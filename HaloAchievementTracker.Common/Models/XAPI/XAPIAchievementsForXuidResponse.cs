using HaloAchievementTracker.Common.Converters;
using HaloAchievementTracker.Common.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HaloAchievementTracker.Common.Models.XAPI
{
    public class XAPIAchievementsForXuidResponse
    {
    }

    public class XAPIAchievement : IAchievement
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Game Game { get; set; }

        public string Description { get; set; }

        [JsonProperty("progressState")]
        [JsonConverter(typeof(XboxLiveAchievementProgressStateConverter))]
        public bool IsUnlocked { get; set; }

        [JsonConstructor]
        public XAPIAchievement(string description)
        {
            Game = AchievementHelper.GetGameFromDescription(description);
        }
    }
}
