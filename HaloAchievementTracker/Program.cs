using HaloAchievementTracker.Extensions;
using HaloAchievementTracker.Helpers;
using HaloAchievementTracker.Models;
using Microsoft.Extensions.Configuration;
using Steam.Models.SteamPlayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HaloAchievementTracker
{
    public class Program
    {
        private static readonly int windowWidth = 186;
        private static readonly int windowHeight = 30;

        public static async Task Main(string[] args)
        {
            Console.WindowWidth = windowWidth;
            Console.WindowHeight = windowHeight;

            var configuration = GetConfiguration(args);

            var steamApiKey = configuration[Constants.CONFIGURATION_KEY_STEAM_API_KEY];
            var steamId = Convert.ToUInt64(configuration[Constants.CONFIGURATION_KEY_STEAM_ID]);

            var steamHelper = new SteamHelper(steamApiKey);
            var steamAchievements = await steamHelper.GetAchievementsAsync(Constants.HALO_MCC_STEAM_APP_ID, steamId);

            var haloWaypointHelper = new HaloWaypointHelper(Constants.HALO_WAYPOINT_SITE_PATH);
            var haloWaypointAchievements = haloWaypointHelper.GetAchievements();

            List<MisalignedAchievement> misalignedAchievements = new List<MisalignedAchievement>();
            foreach (var steamAchievement in steamAchievements.Achievements)
            {
                foreach (var haloWaypointAchievement in haloWaypointAchievements)
                {
                    bool isUnlockedOnSteam = Convert.ToBoolean(steamAchievement.Achieved);
                    if (steamAchievement.Name == haloWaypointAchievement.Name && isUnlockedOnSteam != haloWaypointAchievement.IsUnlocked)
                    {
                        MisalignedAchievement misaligned = new MisalignedAchievement
                        {
                            Name = steamAchievement.Name,
                            Description = steamAchievement.Description,
                            IsUnlockedOnSteam = isUnlockedOnSteam,
                            IsUnlockedOnHaloWaypoint = haloWaypointAchievement.IsUnlocked
                        };
                        misalignedAchievements.Add(misaligned);
                    }
                }
            }
            misalignedAchievements.Sort((x, y) => x.Name.CompareTo(y.Name));

            if (!misalignedAchievements.Any())
            {
                Console.WriteLine("No achievements are misaligned!");
            }
            else
            {
                Console.WriteLine("Following achievements are misaligned:");
                Console.WriteLine(new string('-', windowWidth - 1));
                Console.WriteLine("{0,-40}{1,-100}{2,-20}{3,-20}", "Name", "Description", "Unlocked on Steam", "Unlocked on Halo Waypoint");
                Console.WriteLine(new string('-', windowWidth - 1));
                foreach (MisalignedAchievement misaligned in misalignedAchievements)
                {
                    Console.WriteLine("{0,-40}{1,-100}{2,-20}{3,-20}", misaligned.Name, misaligned.Description, misaligned.IsUnlockedOnSteam.ToMarks(), misaligned.IsUnlockedOnHaloWaypoint.ToMarks());
                }
                Console.WriteLine(new string('-', windowWidth - 1));
            }
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
