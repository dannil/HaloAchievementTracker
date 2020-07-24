using HaloAchievementTracker.Common.Adapters;
using HaloAchievementTracker.Common.Models;
using HaloAchievementTracker.Common.Services;
using Moq;
using NUnit.Framework;
using Steam.Models.SteamPlayer;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HaloAchievementTracker.Tests.Services
{
    public class SteamServiceTest
    {
        private Mock<SteamService> serviceMock;
        private Mock<ISteamWebInterfaceFactory> steamWebInterfaceFactoryMock;

        private SteamService service;

        [SetUp]
        public void SetUp()
        {
            service = new SteamService();

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

            steamWebInterfaceFactoryMock = new Mock<ISteamWebInterfaceFactory>();
            steamWebInterfaceFactoryMock.Setup(m => m.CreateSteamWebInterface<SteamUserStatsAdapter>(It.IsAny<HttpClient>())).Returns(steamUserStatsMock.Object);

            IEnumerable<SteamAchievement> steamAchievements = new HashSet<SteamAchievement>
            {
                new SteamAchievement()
                {
                    Name = "Going Bananas",
                    Description = "Kill 100 Brutes.",
                    IsUnlocked = false
                },
                new SteamAchievement()
                {
                    Name = "Where Am I?",
                    Description = "Complete 10 missions or multiplayer games.",
                    IsUnlocked = true
                },
                new SteamAchievement()
                {
                    Name = "Tempered Blade",
                    Description = "Win 10 multiplayer games.",
                    IsUnlocked = true
                }
            };

            serviceMock = new Mock<SteamService>();
            serviceMock.Setup(m => m.GetAchievementsByScrapingAsync(It.IsAny<uint>(), It.IsAny<ulong>())).Returns(Task.FromResult(steamAchievements));
        }

        [Test]
        public async Task GetAchievementsByApiAsync()
        {
            var achievements = await service.GetAchievementsByApiAsync(steamWebInterfaceFactoryMock.Object, uint.MaxValue, ulong.MaxValue);

            Assert.AreEqual(2, achievements.Count());
        }

        [Test]
        public async Task GetAchievementsByScrapingAsync()
        {
            var achievements = await serviceMock.Object.GetAchievementsByScrapingAsync(uint.MaxValue, ulong.MaxValue);

            Assert.AreEqual(3, achievements.Count());
            Assert.That(achievements.Count(a => a.Name.Equals("Going Bananas")), Is.EqualTo(1));
            Assert.That(achievements.Count(a => a.Name.Equals("Where Am I?")), Is.EqualTo(1));
            Assert.That(achievements.Count(a => a.Name.Equals("Tempered Blade")), Is.EqualTo(1));
            Assert.That(achievements.Count(a => a.IsUnlocked), Is.EqualTo(2));
            Assert.That(achievements.Count(a => !a.IsUnlocked), Is.EqualTo(1));
        }

    }
}
