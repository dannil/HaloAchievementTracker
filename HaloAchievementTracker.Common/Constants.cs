using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HaloAchievementTracker.Common
{
    public static class Constants
    {
        public const string CONFIGURATION_KEY_STEAM_API_KEY = "Steam:ApiKey";
        public const string CONFIGURATION_KEY_STEAM_ID = "Steam:Id";
        public const string CONFIGURATION_KEY_OPENXBL_API_KEY = "WebAppConfiguration:Api:XboxLive:OpenXBL:Key";
        public const string CONFIGURATION_KEY_XAPI_API_KEY = "WebAppConfiguration:Api:XboxLive:XAPI:Key";

        public const uint HALO_MCC_STEAM_APP_ID = 976730;
        public const uint HALO_MCC_XBOX_LIVE_APP_ID = 1144039928;

        private static readonly string HALO_WAYPOINT_SERVICE_RECORD_FOLDER = $"Resources{Path.DirectorySeparatorChar}HaloWaypointData";
        private const string HALO_WAYPOINT_SERVICE_RECORD_FILENAME = "service-record.html";
        public static readonly string HALO_WAYPOINT_SERVICE_RECORD_PATH = $"{HALO_WAYPOINT_SERVICE_RECORD_FOLDER}{Path.DirectorySeparatorChar}{HALO_WAYPOINT_SERVICE_RECORD_FILENAME}";
        public const string HALO_WAYPOINT_SERVICE_RECORDS_ACHIEVEMENT_COLLECTION_DIV = "achievement-collection";
        public const string HALO_WAYPOINT_SERVICE_RECORDS_ACHIEVEMENT_LIST_CLASS = "achievement--list";

        private static readonly string STEAM_ACHIEVEMENTS_FOLDER = $"Resources{Path.DirectorySeparatorChar}SteamData";
        private const string STEAM_ACHIEVEMENTS_FILENAME = "achievements.html";
        public static readonly string STEAM_ACHIEVEMENTS_PATH = $"{STEAM_ACHIEVEMENTS_FOLDER}{Path.DirectorySeparatorChar}{STEAM_ACHIEVEMENTS_FILENAME}";
        public const string STEAM_ACHIEVE_ROW_DIV = "achieveRow";
        public const string STEAM_ACHIEVE_TXT_DIV = "achieveTxt";
    }
}
