﻿using HaloAchievementTracker.Common.Adapters;
using HaloAchievementTracker.Common.Models;
using HaloAchievementTracker.Common.Services;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Steam.Models.SteamPlayer;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HaloAchievementTracker.Common.Tests.Services
{
    public class SteamServiceTest
    {
        private Mock<ISteamWebInterfaceFactory> steamWebInterfaceFactoryMock;

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
        }

        [Test]
        public async Task GetAchievementsByApiAsync()
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://steamcommunity.com/")
            };

            var service = new SteamService(httpClient);
            var achievements = await service.GetAchievementsByApiAsync(steamWebInterfaceFactoryMock.Object, uint.MaxValue, ulong.MaxValue);

            Assert.AreEqual(2, achievements.Count());
        }

        [Test]
        public async Task GetAchievementsByScrapingAsync()
        {
            var serviceRecordHtmlContent = File.ReadAllText(Constants.STEAM_ACHIEVEMENTS_PATH);
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(serviceRecordHtmlContent),
               })
               .Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://steamcommunity.com/"),
            };

            var service = new SteamService(httpClient);
            var achievements = await service.GetAchievementsByScrapingAsync(uint.MaxValue, ulong.MaxValue);

            var gameData = new List<(Game Game, int AmountTotalAchievements, int AmountUnlockedAchievements)>
            {
                (GameFactory.Get("Cross Game"), 80, 63),
                (GameFactory.Get("Halo: Reach"), 100, 90),
                (GameFactory.Get("Halo CE"), 95, 83),
                (GameFactory.Get("Halo 2"), 157, 133),
                (GameFactory.Get("Halo 3"), 89, 86),
                (GameFactory.Get("Halo 3: ODST"), 98, 90),
                (GameFactory.Get("Halo 4"), 81, 36),
            };

            int sumAmountTotalAchievements = gameData.Sum(g => g.AmountTotalAchievements);
            int sumAmountUnlockedAchievements = gameData.Sum(g => g.AmountUnlockedAchievements);

            var locked = achievements.Where(a => !a.IsUnlocked);
            var unlocked = achievements.Except(locked);

            foreach (var (Game, AmountTotalAchievements, AmountUnlockedAchievements) in gameData)
            {
                var gameAchievements = achievements.Where(a => a.Game.Equals(Game));
                Assert.AreEqual(AmountTotalAchievements, gameAchievements.Count());
                var gameUnlockedAchievements = gameAchievements.Count(a => a.IsUnlocked);
                Assert.AreEqual(AmountUnlockedAchievements, gameUnlockedAchievements);
            }

            Assert.AreEqual(sumAmountTotalAchievements, locked.Count() + unlocked.Count());
            Assert.AreEqual(sumAmountTotalAchievements - sumAmountUnlockedAchievements, locked.Count());
            Assert.AreEqual(sumAmountUnlockedAchievements, unlocked.Count());
        }

    }
}
