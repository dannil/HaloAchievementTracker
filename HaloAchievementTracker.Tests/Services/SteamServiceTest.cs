using HaloAchievementTracker.Adapters;
using HaloAchievementTracker.Models;
using HaloAchievementTracker.Services;
using Moq;
using NUnit.Framework;
using Steam.Models.SteamPlayer;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HaloAchievementTracker.Tests.Services
{
    public class SteamServiceTest
    {
        private SteamService service;

        [SetUp]
        public void SetUp()
        {
            var achievements = new List<PlayerAchievementModel>
            {
                new PlayerAchievementModel()
                {
                    APIName = "1_0_LIFE_STORY",
                    Achieved = 0,
                    Description = "Complete the Master Chief Saga playlist.",
                    Name = "Life Story"
                },
                new PlayerAchievementModel()
                {
                    APIName = "1_1_JUST_GETTING_STARTED",
                    Achieved = 1,
                    Description = "Kill 100 enemies or players.",
                    Name = "Just Getting Started"
                }
            };

            var playerAchievementResultModel = new PlayerAchievementResultModel
            {
                Achievements = achievements
            };

            var steamWebResponseDataMock = new Mock<ISteamWebResponse<PlayerAchievementResultModel>>();
            steamWebResponseDataMock.Setup(m => m.Data).Returns(playerAchievementResultModel);

            var steamUserStatsMock = new Mock<SteamUserStatsAdapter>();
            steamUserStatsMock.Setup(m => m.GetPlayerAchievementsAsync(It.IsAny<uint>(), It.IsAny<ulong>(), It.IsAny<string>())).Returns(Task.FromResult(steamWebResponseDataMock.Object));

            var steamWebInterfaceFactoryMock = new Mock<ISteamWebInterfaceFactory>();
            steamWebInterfaceFactoryMock.Setup(m => m.CreateSteamWebInterface<SteamUserStatsAdapter>(It.IsAny<HttpClient>())).Returns(steamUserStatsMock.Object);

            service = new SteamService(steamWebInterfaceFactoryMock.Object);
        }

        [Test]
        public async Task GetAchievements()
        {
            var achievements = (await service.GetAchievementsAsync(uint.MaxValue, ulong.MaxValue)).Achievements;

            Assert.AreEqual(2, achievements.Count);
        }

    }
}
