using HaloAchievementTracker.Common.Adapters;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace HaloAchievementTracker.Common.Tests.Mock
{
    public class XboxLiveMockFactory
    {
        public static Mock<IXboxLiveApiAdapter> GetXboxLiveApiAdapter()
        {
            var _xboxLiveApiAdapterMock = new Mock<IXboxLiveApiAdapter>();

            return _xboxLiveApiAdapterMock;
        }
    }
}
