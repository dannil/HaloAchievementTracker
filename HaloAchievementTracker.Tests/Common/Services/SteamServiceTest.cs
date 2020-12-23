using HaloAchievementTracker.Common;
using HaloAchievementTracker.Common.Adapters;
using HaloAchievementTracker.Common.Models;
using HaloAchievementTracker.Common.Services;
using HaloAchievementTracker.Tests.Mock;
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

namespace HaloAchievementTracker.Tests.Common.Services
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

            steamWebInterfaceFactoryMock = SteamServiceMockFactory.GetSteamWebInterfaceFactory(playerAchievementResultModel);
        }

        [Test]
        public async Task GetAchievementsByApiAsync()
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://steamcommunity.com/profiles/")
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
                BaseAddress = new Uri("https://steamcommunity.com/profiles/"),
            };

            var service = new SteamService(httpClient);
            var achievements = await service.GetAchievementsByScrapingAsync(uint.MaxValue, ulong.MaxValue);

            var gameData = new List<(Game Game, int AmountTotalAchievements, int AmountUnlockedAchievements)>
            {
                (GameFactory.Get("Cross Game"), 80, 19),
                (GameFactory.Get("Halo: Reach"), 100, 29),
                (GameFactory.Get("Halo CE"), 95, 32),
                (GameFactory.Get("Halo 2"), 157, 35),
                (GameFactory.Get("Halo 3"), 89, 26),
                (GameFactory.Get("Halo 3: ODST"), 98, 0),
                (GameFactory.Get("Halo 4"), 81, 0),
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
