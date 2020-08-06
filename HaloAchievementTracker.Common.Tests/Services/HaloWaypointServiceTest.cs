using HaloAchievementTracker.Common;
using HaloAchievementTracker.Common.Models;
using HaloAchievementTracker.Common.Services;
using HtmlAgilityPack;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HaloAchievementTracker.Common.Tests.Services
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
            var gameData = new List<(Game Game, int AmountTotalAchievements, int AmountUnlockedAchievements)>
            {
                (GameFactory.Get("Cross Game"), 80, 19),
                (GameFactory.Get("Halo: Reach"), 100, 29),
                (GameFactory.Get("Halo CE"), 95, 31),
                (GameFactory.Get("Halo 2"), 157, 34),
                (GameFactory.Get("Halo 3"), 89, 26),
                (GameFactory.Get("Halo 3: ODST"), 98, 0),
                (GameFactory.Get("Halo 4"), 81, 0),
            };

            int sumAmountTotalAchievements = gameData.Sum(g => g.AmountTotalAchievements);
            int sumAmountUnlockedAchievements = gameData.Sum(g => g.AmountUnlockedAchievements);

            var achievements = service.GetAchievements();
            var locked = achievements.Where(a => !a.IsUnlocked);
            var unlocked = achievements.Except(locked);

            foreach (var (Game, AmountTotalAchievements, AmountUnlockedAchievements) in gameData)
            {
                var gameAchievements = achievements.Where(a => a.Game.Equals(Game));
                Assert.AreEqual(AmountTotalAchievements, gameAchievements.Count());
                Assert.AreEqual(AmountUnlockedAchievements, gameAchievements.Count(a => a.IsUnlocked));
            }

            Assert.AreEqual(sumAmountTotalAchievements, locked.Count() + unlocked.Count());
            Assert.AreEqual(sumAmountTotalAchievements - sumAmountUnlockedAchievements, locked.Count());
            Assert.AreEqual(sumAmountUnlockedAchievements, unlocked.Count());
        }

    }
}
