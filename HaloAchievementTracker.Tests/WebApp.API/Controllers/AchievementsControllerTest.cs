using HaloAchievementTracker.Common.Adapters;
using HaloAchievementTracker.Common.Models;
using HaloAchievementTracker.Common.Services;
using HaloAchievementTracker.Tests.Mock;
using HaloAchievementTracker.WebApp.API.Configuration;
using HaloAchievementTracker.WebApp.API.Controllers;
using HaloAchievementTracker.WebApp.API.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using Steam.Models.SteamPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloAchievementTracker.Tests.WebApp.API.Controllers
{
    public class AchievementsControllerTest
    {
        private WebAppConfiguration webAppConfiguration;
        private IMemoryCache memoryCache;

        private Mock<ISteamService> _steamServiceMock;
        private Mock<IXboxLiveApiAdapter> _xboxLiveApiAdapterMock;

        private AchievementsController _controller;

        [SetUp]
        public void Setup()
        {
            var configurationValues = new Dictionary<string, string>
            {
                { "WebAppConfiguration:Cache:MisalignedAchievementsController:Duration", "86400" }
            };

            webAppConfiguration = new WebAppConfiguration();

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configurationValues)
                .Build();
            configuration.Bind("WebAppConfiguration", webAppConfiguration);

            memoryCache = new MemoryCache(new MemoryCacheOptions());

            _steamServiceMock = SteamServiceMockFactory.GetSteamService();
            _xboxLiveApiAdapterMock = XboxLiveMockFactory.GetXboxLiveApiAdapter();

            _controller = new AchievementsController(webAppConfiguration, memoryCache, _steamServiceMock.Object, _xboxLiveApiAdapterMock.Object);
        }

        [Test]
        public async Task GetMisaligned()
        {
            IEnumerable<IAchievement> xboxLiveAchievements = new List<IAchievement>
            {
                new XboxLiveAchievement()
                {
                    Description = "Description for achievement 1",
                    Game = GameFactory.Get("Halo 2"),
                    IsUnlocked = true,
                    Name = "Achievement 1"
                },
                new XboxLiveAchievement()
                {
                    Description = "Description for achievement 2",
                    Game = GameFactory.Get("Halo 3"),
                    IsUnlocked = false,
                    Name = "Achievement 2"
                }
            };

            _xboxLiveApiAdapterMock.Setup(m => m.GetAchievementsAsync(It.IsAny<string>(), It.IsAny<uint>())).Returns(Task.FromResult(xboxLiveAchievements));

            IEnumerable<IAchievement> steamAchievements = new List<IAchievement>()
            {
                new SteamAchievement()
                {
                    Description = "Description for achievement 1",
                    Game = GameFactory.Get("Halo 2"),
                    IsUnlocked = true,
                    Name = "Achievement 1"
                },
                new SteamAchievement()
                {
                    Description = "Description for achievement 2",
                    Game = GameFactory.Get("Halo 3"),
                    IsUnlocked = true,
                    Name = "Achievement 2"
                }
            };

            _steamServiceMock.Setup(m => m.GetAchievementsByScrapingAsync(It.IsAny<uint>(), It.IsAny<ulong>())).Returns(Task.FromResult(steamAchievements));

            var query = new MisalignedAchievementsQuery
            {
                SteamId64 = uint.MaxValue,
                XboxLiveGamertag = "example"
            };

            IEnumerable<MisalignedAchievement> misalignedAchievements = await _controller.GetMisaligned(query);

            Assert.AreEqual(1, misalignedAchievements.Count());
        }
    }
}
