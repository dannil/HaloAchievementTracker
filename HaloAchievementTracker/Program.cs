using HaloAchievementTracker.Extensions;
using HaloAchievementTracker.Helpers;
using HaloAchievementTracker.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HaloAchievementTracker
{
    public class Program
    {
        private static readonly int windowWidth = 206;
        private static readonly int windowHeight = 30;

        private static readonly string CONSOLE_OUTPUT_NAME_COLUMN = "Name";
        private static readonly string CONSOLE_OUTPUT_GAME_COLUMN = "Game";
        private static readonly string CONSOLE_OUTPUT_DESCRIPTION_COLUMN = "Description";
        private static readonly string CONSOLE_OUTPUT_STEAM_COLUMN = "Unlocked on Steam";
        private static readonly string CONSOLE_OUTPUT_HALOWAYPOINT_COLUMN = "Unlocked on Halo Waypoint";

        public static async Task Main(string[] args)
        {
            Console.WindowHeight = windowHeight;

            var configuration = GetConfiguration(args);

            var steamApiKey = configuration[Constants.CONFIGURATION_KEY_STEAM_API_KEY];
            var steamId = Convert.ToUInt64(configuration[Constants.CONFIGURATION_KEY_STEAM_ID]);

            var steamHelper = new SteamHelper(steamApiKey);
            var steamAchievementsAsyncResult = await steamHelper.GetAchievementsAsync(Constants.HALO_MCC_STEAM_APP_ID, steamId);
            var steamAchievements = steamAchievementsAsyncResult.Achievements;

            var haloWaypointHelper = new HaloWaypointHelper(Constants.HALO_WAYPOINT_SITE_PATH);
            var haloWaypointAchievements = haloWaypointHelper.GetAchievements();

            var misalignedAchievements = steamAchievements
                .Join(
                    haloWaypointAchievements,
                    s => s.Name,
                    h => h.Name,
                    (s, h) => new MisalignedAchievement
                    {
                        Name = s.Name,
                        GameId = h.GameId,
                        Description = s.Description,
                        IsUnlockedOnSteam = Convert.ToBoolean(s.Achieved),
                        IsUnlockedOnHaloWaypoint = h.IsUnlocked
                    })
                .Where(m => m.IsUnlockedOnSteam != m.IsUnlockedOnHaloWaypoint)
                .OrderBy(m => m.Name)
                .ToList();

            int consoleColumnsWidth = GetConsoleColumnsWidth(misalignedAchievements).Sum();
            Console.WindowWidth = consoleColumnsWidth;

            if (!misalignedAchievements.Any())
            {
                Console.WriteLine("No achievements are misaligned!");
            }
            else
            {
                Console.WriteLine("Following achievements are misaligned:");
                Console.WriteLine(new string('-', consoleColumnsWidth - 1));
                Console.WriteLine(GetConsoleColumnsFormatting(misalignedAchievements), CONSOLE_OUTPUT_NAME_COLUMN, CONSOLE_OUTPUT_GAME_COLUMN, CONSOLE_OUTPUT_DESCRIPTION_COLUMN,
                    CONSOLE_OUTPUT_STEAM_COLUMN, CONSOLE_OUTPUT_HALOWAYPOINT_COLUMN);
                Console.WriteLine(new string('-', consoleColumnsWidth - 1));
                foreach (MisalignedAchievement misaligned in misalignedAchievements)
                {
                    Console.WriteLine(GetConsoleColumnsFormatting(misalignedAchievements), misaligned.Name, misaligned.GameId, misaligned.Description, 
                        misaligned.IsUnlockedOnSteam.ToMarks(), misaligned.IsUnlockedOnHaloWaypoint.ToMarks());
                }
                Console.WriteLine(new string('-', consoleColumnsWidth - 1));
            }
        }

        private static int[] GetConsoleColumnsWidth(IEnumerable<MisalignedAchievement> misalignedAchievements)
        {
            int nameLength = Math.Max(misalignedAchievements.Max(m => m.Name.Length), CONSOLE_OUTPUT_NAME_COLUMN.Length) + 4;
            int gameLength = Math.Max(misalignedAchievements.Max(m => m.GameId.Length), CONSOLE_OUTPUT_GAME_COLUMN.Length) + 4;
            int descriptionLength = Math.Max(misalignedAchievements.Max(m => m.Description.Length), CONSOLE_OUTPUT_DESCRIPTION_COLUMN.Length) + 4;
            int steamLength = Math.Max(misalignedAchievements.Max(m => m.IsUnlockedOnSteam.ToString().Length), CONSOLE_OUTPUT_STEAM_COLUMN.Length) + 4;
            int haloWaypointLength = Math.Max(misalignedAchievements.Max(m => m.IsUnlockedOnHaloWaypoint.ToString().Length), CONSOLE_OUTPUT_HALOWAYPOINT_COLUMN.Length) + 4;

            return new int[] { nameLength, gameLength, descriptionLength, steamLength, haloWaypointLength };
        }

        private static string GetConsoleColumnsFormatting(IEnumerable<MisalignedAchievement> misalignedAchievements)
        {
            StringBuilder builder = new StringBuilder();
            int[] widths = GetConsoleColumnsWidth(misalignedAchievements);
            for (int i = 0; i < widths.Length; i++)
            {
                builder.Append("{" + i + ",-" + widths[i] + "}");
            }
            return builder.ToString();
        }

        private static Dictionary<string, string> GetConfiguration(string[] args)
        {
            var configuration = new Dictionary<string, string>();
            string env = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            if ("Development".Equals(env))
            {
                // If in dev, overwrite with values from secret store
                var builder = new ConfigurationBuilder();
                builder.AddUserSecrets<Program>();
                var configurationRoot = builder.Build();
                configuration[Constants.CONFIGURATION_KEY_STEAM_API_KEY] = configurationRoot[Constants.CONFIGURATION_KEY_STEAM_API_KEY];
                configuration[Constants.CONFIGURATION_KEY_STEAM_ID] = configurationRoot[Constants.CONFIGURATION_KEY_STEAM_ID];
            }
            else
            {
                configuration[Constants.CONFIGURATION_KEY_STEAM_API_KEY] = args[0];
                configuration[Constants.CONFIGURATION_KEY_STEAM_ID] = args[1];
            }
            return configuration;
        }

    }
}
