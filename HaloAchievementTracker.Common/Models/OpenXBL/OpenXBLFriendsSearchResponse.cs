using System;
using System.Collections.Generic;
using System.Text;

namespace HaloAchievementTracker.Common.Models.OpenXBL
{
    public class OpenXBLFriendsSearchResponse
    {
        public List<OpenXBLProfileUser> ProfileUsers;
    }

    public class OpenXBLProfileUser
    {
        public string Id { get; set; }
        public string HostId { get; set; }
    }
}
