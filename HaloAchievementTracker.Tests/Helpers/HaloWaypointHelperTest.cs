using HaloAchievementTracker.Helpers;
using HtmlAgilityPack;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HaloAchievementTracker.Tests.Helpers
{
    public class HaloWaypointHelperTest
    {
        private HaloWaypointHelper helper;

        [SetUp]
        public void SetUp()
        {
            var htmlDocument = new HtmlDocument();
            var path = Path.Combine(Environment.CurrentDirectory, Constants.HALO_WAYPOINT_SITE_PATH);
            htmlDocument.Load(path);
            helper = new HaloWaypointHelper(htmlDocument);
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

            var achievements = helper.GetAchievements();
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
