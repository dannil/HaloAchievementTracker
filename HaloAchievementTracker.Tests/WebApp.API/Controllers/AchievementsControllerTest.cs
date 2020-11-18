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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloAchievementTracker.WebApp.API.Tests.Controllers
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
            new ConfigurationBuilder()
                .AddInMemoryCollection(configurationValues)
                .Build()
                .Bind("WebAppConfiguration", webAppConfiguration);

            memoryCache = new MemoryCache(new MemoryCacheOptions());

            _steamServiceMock = SteamServiceMockFactory.GetSteamService();
            _xboxLiveApiAdapterMock = XboxLiveMockFactory.GetXboxLiveApiAdapter();

            _controller = new AchievementsController(webAppConfiguration, memoryCache, _steamServiceMock.Object, _xboxLiveApiAdapterMock.Object);
        }

        [Test]
        public async Task GetMisaligned()
        {
            var query = new MisalignedAchievementsQuery
            {
                SteamId64 = uint.MaxValue,
                XboxLiveGamertag = "example"
            };

            IEnumerable<MisalignedAchievement> misalignedAchievements = await _controller.GetMisaligned(query);

            Assert.That(misalignedAchievements, Has.Count.EqualTo(1));
        }
    }
}
