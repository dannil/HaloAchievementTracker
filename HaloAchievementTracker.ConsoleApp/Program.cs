using HaloAchievementTracker.Common;
using HaloAchievementTracker.Common.Extensions;
using HaloAchievementTracker.Common.Helpers;
using HaloAchievementTracker.Common.Models;
using HaloAchievementTracker.Common.Services;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HaloAchievementTracker.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create service collection and configure our services
            var services = ConfigureServices();

            // Generate a provider
            var serviceProvider = services.BuildServiceProvider();

            // Kick off our actual code
            var run = serviceProvider.GetService<ConsoleApplication>();
            run.Run(args).Wait();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddHttpClient<ISteamService, SteamService>(typeof(SteamService).Name, client =>
            {
                client.BaseAddress = new Uri("https://steamcommunity.com/profiles/");
            });

            services.AddSingleton<IHaloWaypointService>(s =>
            {
                var htmlDocument = new HtmlDocument();
                var path = Path.Combine(Environment.CurrentDirectory, Constants.HALO_WAYPOINT_SERVICE_RECORD_PATH);
                htmlDocument.Load(path);
                return new HaloWaypointService(htmlDocument);
            });

            // IMPORTANT! Register our application entry point
            services.AddTransient<ConsoleApplication>(); 
            
            return services;
        }
    }
}
