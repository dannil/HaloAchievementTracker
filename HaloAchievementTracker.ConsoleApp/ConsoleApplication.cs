using HaloAchievementTracker.Common;
using HaloAchievementTracker.Common.Extensions;
using HaloAchievementTracker.Common.Helpers;
using HaloAchievementTracker.Common.Models;
using HaloAchievementTracker.Common.Services;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HaloAchievementTracker.ConsoleApp
{
    public class ConsoleApplication
    {
        private readonly string CONSOLE_OUTPUT_NAME_COLUMN = "Name";
        private readonly string CONSOLE_OUTPUT_GAME_COLUMN = "Game";
        private readonly string CONSOLE_OUTPUT_DESCRIPTION_COLUMN = "Description";
        private readonly string CONSOLE_OUTPUT_STEAM_COLUMN = "Unlocked on Steam";
        private readonly string CONSOLE_OUTPUT_XBOXLIVE_COLUMN = "Unlocked on Xbox Live";

        private readonly ISteamService _steamService;
        private readonly IHaloWaypointService _haloWaypointService;

        public ConsoleApplication(ISteamService steamService, IHaloWaypointService haloWaypointService)
        {
            _steamService = steamService;
            _haloWaypointService = haloWaypointService;
        }

        public async Task Run(string[] args)
        {
            var configuration = GetConfiguration(args);

            var steamId = Convert.ToUInt64(configuration[Constants.CONFIGURATION_KEY_STEAM_ID]);

            var steamAchievements = await _steamService.GetAchievementsByScrapingAsync(Constants.HALO_MCC_STEAM_APP_ID, steamId);
            var xboxLiveAchievements = _haloWaypointService.GetAchievements();

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

        private int[] GetConsoleColumnsWidths(IEnumerable<MisalignedAchievement> misalignedAchievements)
        {
            int nameLength = Math.Max(misalignedAchievements.Max(m => m.Name.Length), CONSOLE_OUTPUT_NAME_COLUMN.Length) + 4;
            int gameLength = Math.Max(misalignedAchievements.Max(m => m.GameId.Length), CONSOLE_OUTPUT_GAME_COLUMN.Length) + 4;
            int descriptionLength = Math.Max(misalignedAchievements.Max(m => m.Description.Length), CONSOLE_OUTPUT_DESCRIPTION_COLUMN.Length) + 4;
            int steamLength = Math.Max(misalignedAchievements.Max(m => m.IsUnlockedOnSteam.ToString().Length), CONSOLE_OUTPUT_STEAM_COLUMN.Length) + 4;
            int haloWaypointLength = Math.Max(misalignedAchievements.Max(m => m.IsUnlockedOnXboxLive.ToString().Length), CONSOLE_OUTPUT_XBOXLIVE_COLUMN.Length) + 4;

            return new int[] { nameLength, gameLength, descriptionLength, steamLength, haloWaypointLength };
        }

        private string GetConsoleColumnsFormatting(int[] widths)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < widths.Length; i++)
            {
                builder.Append("{" + i + ",-" + widths[i] + "}");
            }
            return builder.ToString();
        }

        private Dictionary<string, string> GetConfiguration(string[] args)
        {
            var configuration = new Dictionary<string, string>();
            string env = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            if ("Development".Equals(env))
            {
                // If in dev, overwrite with values from secret store
                var builder = new ConfigurationBuilder();
                builder.AddUserSecrets<Program>();
                var configurationRoot = builder.Build();
                configuration[Constants.CONFIGURATION_KEY_STEAM_ID] = configurationRoot[Constants.CONFIGURATION_KEY_STEAM_ID];
            }
            else
            {
                configuration[Constants.CONFIGURATION_KEY_STEAM_ID] = args[0];
            }
            return configuration;
        }
    }
}
