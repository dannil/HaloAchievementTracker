using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloAchievementTracker.WebApp.Models
{
    public class MisalignedAchievementQuery
    {
        public string XboxLiveGamerTag { get; set; }
        public ulong SteamId64 { get; set; }
    }
}
