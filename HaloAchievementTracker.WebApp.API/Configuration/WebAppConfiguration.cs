using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaloAchievementTracker.WebApp.API.Configuration
{

    public interface IWebAppConfiguration
    {
        string ClientAppUrl { get; set; }

        LoggingConfiguration Logging { get; set; }

        ApiConfiguration Api { get; set; }

        CacheConfiguration Cache { get; set; }
    }

    public interface IXboxLiveApiImplementationConfiguration
    {
        public string Key { get; set; }
    }

    public class WebAppConfiguration : IWebAppConfiguration
    {
        public string ClientAppUrl { get; set; }

        public LoggingConfiguration Logging { get; set; }

        public ApiConfiguration Api { get; set; }

        public CacheConfiguration Cache { get; set; }
    }

    public class LoggingConfiguration
    {
        public class LogLevelConfiguration
        {
            public string Default { get; set; }
        }

        public LogLevelConfiguration LogLevel { get; set; }
    }

    public class ApiConfiguration
    {
        public XboxLiveConfiguration XboxLive { get; set; }
    }

    public class XboxLiveConfiguration
    {
        public OpenXBLConfiguration OpenXBL { get; set; }

        public XAPIConfiguration XAPI { get; set; }

        public XboxLiveImplementations Implementation { get; set; }
    }

    public class OpenXBLConfiguration : IXboxLiveApiImplementationConfiguration
    {
        public string Key { get; set; }
    }

    public class XAPIConfiguration : IXboxLiveApiImplementationConfiguration
    {
        public string Key { get; set; }
    }

    public class CacheConfiguration
    {
        public AchievementsControllerConfiguration AchievementsController { get; set; }

        public class AchievementsControllerConfiguration
        {
            public int Duration { get; set; }
        }
    }

}
