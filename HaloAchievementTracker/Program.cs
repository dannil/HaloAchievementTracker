using HaloAchievementTracker.Extensions;
using HaloAchievementTracker.Helpers;
using HaloAchievementTracker.Models;
using HaloAchievementTracker.Services;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
//using SteamWebAPI2.Utilities;
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
        private static readonly string CONSOLE_OUTPUT_NAME_COLUMN = "Name";
        private static readonly string CONSOLE_OUTPUT_GAME_COLUMN = "Game";
        private static readonly string CONSOLE_OUTPUT_DESCRIPTION_COLUMN = "Description";
        private static readonly string CONSOLE_OUTPUT_STEAM_COLUMN = "Unlocked on Steam";
        private static readonly string CONSOLE_OUTPUT_XBOXLIVE_COLUMN = "Unlocked on Xbox Live";

        public static async Task Main(string[] args)
        {
            var configuration = GetConfiguration(args);

            // var steamApiKey = configuration[Constants.CONFIGURATION_KEY_STEAM_API_KEY];
            var steamId = Convert.ToUInt64(configuration[Constants.CONFIGURATION_KEY_STEAM_ID]);

            //var webInterfaceFactory = new SteamWebInterfaceFactory(steamApiKey);
            //var steamHelper = new SteamService(webInterfaceFactory);
            var steamService = new SteamService();

            //var steamAchievements = (await steamHelper.GetAchievementsAsync(Constants.HALO_MCC_STEAM_APP_ID, steamId)).Achievements;
            var steamAchievements = await steamService.GetAchievements(Constants.HALO_MCC_STEAM_APP_ID, steamId);

            var htmlDocument = new HtmlDocument();
            var path = Path.Combine(Environment.CurrentDirectory, Constants.HALO_WAYPOINT_SERVICE_RECORD_PATH);
            htmlDocument.Load(path);
            var haloWaypointService = new HaloWaypointService(htmlDocument);
            var xboxLiveAchievements = haloWaypointService.GetAchievements();

            var misalignedAchievements = AchievementHelper.GetMisalignedAchievements(steamAchievements, xboxLiveAchievements);

            if (misalignedAchievements.Any())
            {
                int[] consoleColumnsWidths = GetConsoleColumnsWidths(misalignedAchievements);
                int consoleColumnsTotalWidth = consoleColumnsWidths.Sum();
                string consoleColumnsFormatting = GetConsoleColumnsFormatting(consoleColumnsWidths);
                string rowSeparator = new string('-', consoleColumnsTotalWidth - 1);

                Console.WriteLine("Following achievements are misaligned:");
                Console.WriteLine(rowSeparator);
                Console.WriteLine(consoleColumnsFormatting, CONSOLE_OUTPUT_NAME_COLUMN, CONSOLE_OUTPUT_GAME_COLUMN, CONSOLE_OUTPUT_DESCRIPTION_COLUMN,
                    CONSOLE_OUTPUT_STEAM_COLUMN, CONSOLE_OUTPUT_XBOXLIVE_COLUMN);
                Console.WriteLine(rowSeparator);
                foreach (MisalignedAchievement misaligned in misalignedAchievements)
                {
                    Console.WriteLine(consoleColumnsFormatting, misaligned.Name, misaligned.GameId, misaligned.Description,
                        misaligned.IsUnlockedOnSteam.ToMarks(), misaligned.IsUnlockedOnXboxLive.ToMarks());
                }
                Console.WriteLine(rowSeparator);
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
            int haloWaypointLength = Math.Max(misalignedAchievements.Max(m => m.IsUnlockedOnXboxLive.ToString().Length), CONSOLE_OUTPUT_XBOXLIVE_COLUMN.Length) + 4;

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
