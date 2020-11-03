using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloAchievementTracker.WebApp.API.Models
{
    public class MisalignedAchievementsQuery
    {
        public string XboxLiveGamertag { get; set; }
        public ulong SteamId64 { get; set; }
    }
}
