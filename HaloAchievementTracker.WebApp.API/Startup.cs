using HaloAchievementTracker.Common;
using HaloAchievementTracker.Common.Adapters;
using HaloAchievementTracker.Common.Services;
using HaloAchievementTracker.WebApp.API.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace HaloAchievementTracker.WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebAppConfiguration webAppConfiguration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            webAppConfiguration = new WebAppConfiguration();
            Configuration.Bind("WebAppConfiguration", webAppConfiguration);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllersWithViews();
            services.AddMemoryCache();

            services.AddSingleton(webAppConfiguration);

            services.AddHttpClient<ISteamService, SteamService>(typeof(SteamService).Name, client =>
            {
                client.BaseAddress = new Uri("https://steamcommunity.com/profiles/");
            });

            var openXBLImplementation = XboxLiveImplementations.OpenXBL;
            var xAPIImplementation = XboxLiveImplementations.XAPI;
            var actualImplementation = webAppConfiguration.Api.XboxLive.Implementation;
            if (actualImplementation.Equals(openXBLImplementation))
            {
                services.AddHttpClient<IXboxLiveApiAdapter, OpenXBLServiceAdapter>(typeof(OpenXBLService).Name, client =>
                {
                    client.BaseAddress = new Uri("https://xbl.io/api/v2/");
                    client.DefaultRequestHeaders.Add("X-Authorization", Configuration[Constants.CONFIGURATION_KEY_OPENXBL_API_KEY]);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });
            }
            else if (actualImplementation.Equals(xAPIImplementation))
            {
                services.AddHttpClient<IXboxLiveApiAdapter, XAPIServiceAdapter>(typeof(XAPIService).Name, client =>
                {
                    client.BaseAddress = new Uri("https://xapi.us/v2/");
                    client.DefaultRequestHeaders.Add("X-Auth", Configuration[Constants.CONFIGURATION_KEY_XAPI_API_KEY]);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });
            }
            else
            {
                throw new ArgumentException($"Xbox Live implementation {actualImplementation} is not a valid configuration.");
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime, IXboxLiveApiAdapter xboxLiveApiAdapter)
        {
            app.UseCors(options =>
            {
                options.WithOrigins(webAppConfiguration.ClientAppUrl)
                       .AllowAnyMethod();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            lifetime.ApplicationStarted.Register(OnApplicationStartedAsync(xboxLiveApiAdapter).Wait);
        }

        private static async Task<Action> OnApplicationStartedAsync(IXboxLiveApiAdapter xboxLiveApiAdapter)
        {
            await xboxLiveApiAdapter.Warmup();
            return null;
        }
    }
}
