using HaloAchievementTracker.Common.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HaloAchievementTracker.Common.Tests.Helpers
{
    public class AchievementHelperTest
    {
        [Test]
        public void GetGameIdFromDescriptionHaloCE()
        {
            var description = "Halo CE: Find and claim all Skulls in Halo: CE.";
            var gameId = AchievementHelper.GetGameFromDescription(description);

            Assert.AreEqual("Halo CE", gameId);
        }

        [Test]
        public void GetGameIdFromDescriptionHaloCEWithColon()
        {
            var description = "Halo: CE: Play a CTF custom game on Blood Gulch with 4 players. Tri-tip optional.";
            var gameId = AchievementHelper.GetGameFromDescription(description);

            Assert.AreEqual("Halo: CE", gameId);
        }

        [Test]
        public void GetGameIdFromDescriptionHalo2()
        {
            var description = "Halo 2: Beat the par score on every Halo 2 level.";
            var gameId = AchievementHelper.GetGameFromDescription(description);

            Assert.AreEqual("Halo 2", gameId);
        }

        [Test]
        public void GetGameIdFromDescriptionHalo2MP()
        {
            var description = "Halo 2 MP: In Classic or Anniversary matchmaking, stop the killing spree of another player.";
            var gameId = AchievementHelper.GetGameFromDescription(description);

            Assert.AreEqual("Halo 2 MP", gameId);
        }

        [Test]
        public void GetGameIdFromDescriptionHalo2AnniversaryMP()
        {
            var description = "Halo 2A MP: Get awarded at least 10 different medals in one multiplayer game.";
            var gameId = AchievementHelper.GetGameFromDescription(description);

            Assert.AreEqual("Halo 2A MP", gameId);
        }

        [Test]
        public void GetGameIdFromDescriptionHalo3()
        {
            var description = "Halo 3: Beat the par score on every Halo 3 level.";
            var gameId = AchievementHelper.GetGameFromDescription(description);

            Assert.AreEqual("Halo 3", gameId);
        }

        [Test]
        public void GetGameIdFromDescriptionHalo3ODST()
        {
            var description = "H3: ODST: Beat par score on all Halo 3: ODST levels.";
            var gameId = AchievementHelper.GetGameFromDescription(description);

            Assert.AreEqual("H3: ODST", gameId);
        }

        [Test]
        public void GetGameIdFromDescriptionHalo4()
        {
            var description = "Halo 4: Beat the par score on every Halo 4 level.";
            var gameId = AchievementHelper.GetGameFromDescription(description);

            Assert.AreEqual("Halo 4", gameId);
        }

        [Test]
        public void GetGameIdFromDescriptionCrossGame()
        {
            var description = "Find and claim all the campaign skulls in Halo:CE, Halo 2, and Halo 3.";
            var gameId = AchievementHelper.GetGameFromDescription(description);

            Assert.AreEqual("Cross Game", gameId);
        }
    }
}
