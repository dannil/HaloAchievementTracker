using HaloAchievementTracker.Helpers;
using HtmlAgilityPack;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
            htmlDocument.Load(Constants.HALO_WAYPOINT_SITE_PATH);

            helper = new HaloWaypointHelper(htmlDocument);
        }

        [Test]
        public void GetAchievements()
        {
            var achievements = helper.GetAchievements();

            Assert.AreEqual(80, achievements.Where(a => a.GameId.Equals("CrossGame")).ToList().Count);
            Assert.AreEqual(100, achievements.Where(a => a.GameId.Equals("HaloReach")).ToList().Count);
            Assert.AreEqual(95, achievements.Where(a => a.GameId.Equals("HaloCombatEvolved")).ToList().Count);
            Assert.AreEqual(157, achievements.Where(a => a.GameId.Equals("Halo2")).ToList().Count);
            Assert.AreEqual(89, achievements.Where(a => a.GameId.Equals("Halo3")).ToList().Count);
            Assert.AreEqual(98, achievements.Where(a => a.GameId.Equals("Halo3Odst")).ToList().Count);
            Assert.AreEqual(81, achievements.Where(a => a.GameId.Equals("Halo4")).ToList().Count);

            var locked = achievements.Where(a => !a.IsUnlocked).ToList();
            var unlocked = achievements.Except(locked).ToList();

            Assert.AreEqual(700, locked.Count() + unlocked.Count());
            Assert.AreEqual(588, locked.Count());
            Assert.AreEqual(112, unlocked.Count());
        }

    }
}
