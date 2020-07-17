using HaloAchievementTracker.Services;
using HtmlAgilityPack;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HaloAchievementTracker.Tests.Services
{
    public class HaloWaypointServiceTest
    {
        private HaloWaypointService service;

        [SetUp]
        public void SetUp()
        {
            var htmlDocument = new HtmlDocument();
            var path = Path.Combine(Environment.CurrentDirectory, Constants.HALO_WAYPOINT_SERVICE_RECORD_PATH);
            htmlDocument.Load(path);
            service = new HaloWaypointService(htmlDocument);
        }

        [Test]
        public void GetAchievements()
        {
            var gameData = new List<(string Name, int AmountTotalAchievements, int AmountUnlockedAchievements)>
            {
                ("CrossGame", 80, 19),
                ("HaloReach", 100, 29),
                ("HaloCombatEvolved", 95, 31),
                ("Halo2", 157, 34),
                ("Halo3", 89, 26),
                ("Halo3Odst", 98, 0),
                ("Halo4", 81, 0),
            };

            int sumAmountTotalAchievements = gameData.Sum(g => g.AmountTotalAchievements);
            int sumAmountUnlockedAchievements = gameData.Sum(g => g.AmountUnlockedAchievements);

            var achievements = service.GetAchievements();
            var locked = achievements.Where(a => !a.IsUnlocked);
            var unlocked = achievements.Except(locked);

            foreach (var (Name, AmountTotalAchievements, AmountUnlockedAchievements) in gameData)
            {
                var gameAchievements = achievements.Where(a => a.GameId.Equals(Name));
                Assert.AreEqual(AmountTotalAchievements, gameAchievements.Count());
                Assert.AreEqual(AmountUnlockedAchievements, gameAchievements.Count(a => a.IsUnlocked));
            }

            Assert.AreEqual(sumAmountTotalAchievements, locked.Count() + unlocked.Count());
            Assert.AreEqual(sumAmountTotalAchievements - sumAmountUnlockedAchievements, locked.Count());
            Assert.AreEqual(sumAmountUnlockedAchievements, unlocked.Count());
        }

    }
}
