using System;
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

        private static readonly string HALO_WAYPOINT_SERVICE_RECORD_FOLDER = $"Resources{Path.DirectorySeparatorChar}HaloWaypointData";
        private const string HALO_WAYPOINT_SERVICE_RECORD_FILENAME = "service-record.html";
        public static readonly string HALO_WAYPOINT_SERVICE_RECORD_PATH = $"{HALO_WAYPOINT_SERVICE_RECORD_FOLDER}{Path.DirectorySeparatorChar}{HALO_WAYPOINT_SERVICE_RECORD_FILENAME}";

        public const string STEAM_ACHIEVE_ROW_DIV = "achieveRow";
        public const string STEAM_ACHIEVE_TXT_DIV = "achieveTxt";

        public const string HALO_WAYPOINT_SERVICE_RECORDS_ACHIEVEMENT_COLLECTION_DIV = "achievement-collection";
        public const string HALO_WAYPOINT_SERVICE_RECORDS_ACHIEVEMENT_LIST_CLASS = "achievement--list";
    }
}
