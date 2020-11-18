using HaloAchievementTracker.Common.Adapters;
using HaloAchievementTracker.Common.Services;
using Moq;
using Steam.Models.SteamPlayer;
using SteamWebAPI2.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HaloAchievementTracker.Tests.Mock
{
    public class SteamServiceMockFactory
    {
        public static Mock<ISteamWebInterfaceFactory> GetSteamWebInterfaceFactory(PlayerAchievementResultModel playerAchievementResultModel)
        {
            var steamWebResponseDataMock = new Mock<ISteamWebResponse<PlayerAchievementResultModel>>();
            steamWebResponseDataMock.Setup(m => m.Data).Returns(playerAchievementResultModel);

            var steamUserStatsMock = new Mock<SteamUserStatsAdapter>();
            steamUserStatsMock.Setup(m => m.GetPlayerAchievementsAsync(It.IsAny<uint>(), It.IsAny<ulong>(), It.IsAny<string>())).Returns(Task.FromResult(steamWebResponseDataMock.Object));

            var steamWebInterfaceFactoryMock = new Mock<ISteamWebInterfaceFactory>();
            steamWebInterfaceFactoryMock.Setup(m => m.CreateSteamWebInterface<SteamUserStatsAdapter>(It.IsAny<HttpClient>())).Returns(steamUserStatsMock.Object);

            return steamWebInterfaceFactoryMock;
        }

        public static Mock<ISteamService> GetSteamService()
        {
            var httpClientMock = new Mock<HttpClient>();

            var steamServiceMock = new Mock<ISteamService>(httpClientMock.Object);

            return steamServiceMock;
        }
    }
}
