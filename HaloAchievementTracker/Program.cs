using HaloAchievementTracker.Extensions;
using HaloAchievementTracker.Helpers;
using HaloAchievementTracker.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using SteamWebAPI2.Utilities;
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

            var webInterfaceFactory = new SteamWebInterfaceFactory(steamApiKey);
            var steamHelper = new SteamHelper(webInterfaceFactory);
            var steamAchievements = (await steamHelper.GetAchievementsAsync(Constants.HALO_MCC_STEAM_APP_ID, steamId)).Achievements;

            var htmlDocument = new HtmlDocument();
            var path = Path.Combine(Environment.CurrentDirectory, Constants.HALO_WAYPOINT_SITE_PATH);
            htmlDocument.Load(path);
            var haloWaypointHelper = new HaloWaypointHelper(htmlDocument);
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

            if (misalignedAchievements.Any())
            {
                int[] consoleColumnsWidths = GetConsoleColumnsWidths(misalignedAchievements);
                int consoleColumnsTotalWidth = consoleColumnsWidths.Sum();
                string consoleColumnsFormatting = GetConsoleColumnsFormatting(consoleColumnsWidths);

                Console.WindowWidth = consoleColumnsTotalWidth;

                Console.WriteLine("Following achievements are misaligned:");
                Console.WriteLine(new string('-', consoleColumnsTotalWidth - 1));
                Console.WriteLine(consoleColumnsFormatting, CONSOLE_OUTPUT_NAME_COLUMN, CONSOLE_OUTPUT_GAME_COLUMN, CONSOLE_OUTPUT_DESCRIPTION_COLUMN,
                    CONSOLE_OUTPUT_STEAM_COLUMN, CONSOLE_OUTPUT_HALOWAYPOINT_COLUMN);
                Console.WriteLine(new string('-', consoleColumnsTotalWidth - 1));
                foreach (MisalignedAchievement misaligned in misalignedAchievements)
                {
                    Console.WriteLine(consoleColumnsFormatting, misaligned.Name, misaligned.GameId, misaligned.Description,
                        misaligned.IsUnlockedOnSteam.ToMarks(), misaligned.IsUnlockedOnHaloWaypoint.ToMarks());
                }
                Console.WriteLine(new string('-', consoleColumnsTotalWidth - 1));
            }
            else
            {
                Console.WriteLine("No achievements are misaligned!");
            }
        }

        private static int[] GetConsoleColumnsWidths(IEnumerable<MisalignedAchievement> misalignedAchievements)
        {
            int nameLength = Math.Max(misalignedAchievements.Max(m => m.Name.Length), CONSOLE_OUTPUT_NAME_COLUMN.Length) + 4;
            int gameLength = Math.Max(misalignedAchievements.Max(m => m.GameId.Length), CONSOLE_OUTPUT_GAME_COLUMN.Length) + 4;
            int descriptionLength = Math.Max(misalignedAchievements.Max(m => m.Description.Length), CONSOLE_OUTPUT_DESCRIPTION_COLUMN.Length) + 4;
            int steamLength = Math.Max(misalignedAchievements.Max(m => m.IsUnlockedOnSteam.ToString().Length), CONSOLE_OUTPUT_STEAM_COLUMN.Length) + 4;
            int haloWaypointLength = Math.Max(misalignedAchievements.Max(m => m.IsUnlockedOnHaloWaypoint.ToString().Length), CONSOLE_OUTPUT_HALOWAYPOINT_COLUMN.Length) + 4;

            return new int[] { nameLength, gameLength, descriptionLength, steamLength, haloWaypointLength };
        }

        private static string GetConsoleColumnsFormatting(int[] widths)
        {
            StringBuilder builder = new StringBuilder();
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
