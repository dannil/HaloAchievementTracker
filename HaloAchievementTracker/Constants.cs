﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HaloAchievementTracker
{
    public static class Constants
    {
        public const string CONFIGURATION_KEY_STEAM_API_KEY = "Steam:ApiKey";
        public const string CONFIGURATION_KEY_STEAM_ID = "Steam:Id";

        public const uint HALO_MCC_STEAM_APP_ID = 976730;

        public static readonly string HALO_WAYPOINT_SITE_PATH = $"Resources{Path.DirectorySeparatorChar}HaloWaypointData{Path.DirectorySeparatorChar}site.html";
        public const string HALO_WAYPOINT_SERVICE_RECORDS_ACHIEVEMENT_DIV = "service-record-achievements--halomasterchiefcollection";
        public const string HALO_WAYPOINT_SERVICE_RECORDS_ACHIEVEMENT_COLLECTION_DIV = "achievement-collection";
        public const string HALO_WAYPOINT_SERVICE_RECORDS_ACHIEVEMENT_LIST_CLASS = "achievement--list";
    }
}
