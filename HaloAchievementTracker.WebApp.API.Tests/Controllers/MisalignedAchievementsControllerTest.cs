using HaloAchievementTracker.Common.Adapters;
using HaloAchievementTracker.Common.Models;
using HaloAchievementTracker.Common.Services;
using HaloAchievementTracker.Common.Tests.Mock;
using HaloAchievementTracker.WebApp.API.Controllers;
using HaloAchievementTracker.WebApp.API.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HaloAchievementTracker.WebApp.API.Tests.Controllers
{
    public class MisalignedAchievementsControllerTest
    {
        private Mock<ISteamService> _steamServiceMock;
        private Mock<IXboxLiveApiAdapter> _xboxLiveApiAdapterMock;

        private MisalignedAchievementsController _controller;

        [SetUp]
        public void Setup()
        {
            _steamServiceMock = SteamServiceMockFactory.GetSteamService();
            _xboxLiveApiAdapterMock = XboxLiveMockFactory.GetXboxLiveApiAdapter();

            _controller = new MisalignedAchievementsController(null, null, _steamServiceMock.Object, _xboxLiveApiAdapterMock.Object);
        }

        [Test]
        public async void GetQuery()
        {
            var query = new MisalignedAchievementsQuery
            {
                SteamId64 = uint.MaxValue,
                XboxLiveGamertag = "example"
            };

            IEnumerable<MisalignedAchievement> misalignedAchievements = await _controller.GetQuery(query);
        }
    }
}
